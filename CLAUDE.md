# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

An **AssetRipper 2.0 (free edition)** export of the Android game *Omni-Watch 3D* by Kokokat Studios (`com.KokokatStudios.OmniWatch3D`), a 3D viewer/simulator of Omnitrix-style watches. The game is an **IL2CPP arm64** build. Open the project in **Unity 2022.3.62f3**, which is installed under `C:\Program Files\Unity\Hub\Editor\`. There is no git repo and there are no tests.

## The key fact: signatures are real, method bodies are empty

Every script has its real classes, fields, properties and method signatures, recovered from IL2CPP metadata. **Every method body is empty** (`{ }`, `return false;`, `return null;`, `return default;`). This was true of the game code in `Assembly-CSharp` **and** of the decompiled libraries next to it: `UnityEngine.UI`, `Unity.Postprocessing.Runtime`, DOTween, GoogleMobileAds and NativeGallery. The libraries except GoogleMobileAds now have real source; see Porting status. The export was set to Level 3, but the free edition silently treats Level 3 as Level 2.

Scenes and prefabs **do** contain the serialized field values. For example, a component's `menuController: {fileID: 1215}` reference is intact, so the wiring and tuning numbers in the `.unity` YAML are trustworthy.

Rules that follow from this:
- To restore behavior, fill in the method bodies. Keep each class name, file name and `.cs.meta` GUID, because scenes bind scripts by `m_Script` GUID. Never regenerate or delete `.meta` files.
- For library assemblies, don't rewrite the code. Overwrite each stub `.cs` with the real source file of the same name from the matching package or asset version, and keep the existing `.meta`. Installing the UPM package instead (`com.unity.ugui`, `com.unity.postprocessing`, …) brings new GUIDs and breaks every scene reference. `Packages/manifest.json` lists only the built-in `com.unity.modules.*`, so all of these libraries come only from the folders under `Assets/Scripts/`. Where the real source puts several stub types in one file (e.g. ugui's `EventInterfaces.cs`), delete the stub-only files; that is safe only for types no asset references by GUID.
- TextMeshPro, Timeline and AI Navigation appear in the build's assembly list, but no scene or prefab references them.

## Recovering the original logic (IL2CPP → pseudocode)

The original logic exists only as compiled ARM64 code. Everything for reading it lives outside this project, under `D:\RippedProject\`:

| Path | What |
|---|---|
| `Il2CppDump\pseudocode\<Class>.c` | **Start here.** Ghidra-decompiled C for every method of each `Assembly-CSharp` class, one file per class. Each function has a header comment with the IL2CPP name, RVA and C signature. |
| `Il2CppDump\dump.cs` | Il2CppDumper's C# view of every type, with **field offsets** (`// 0x18`) and method RVAs. Use it to turn `*(param_1 + 0x18)` in the pseudocode into a field name. |
| `Il2CppDump\script.json`, `il2cpp.h`, `stringliteral.json`, `DummyDll\` | Il2CppDumper output: name/address map, C structs, string literals, and reference DLLs that can be opened in ILSpy/dnSpy. |
| `Ghidra\Omni.gpr` | An analyzed Ghidra project with every function and string literal named. Open it in the Ghidra GUI to browse cross-references. |
| `Tools\` | Il2CppDumper 6.7.46, Ghidra 12.1.4, a portable JDK 21 and `ghidra_scripts\ExportIl2CppPseudocode.java`. |
| `C:\Users\AfeefAbdullah\Downloads\omni_merged\` | `base.apk` and `split_config.arm64_v8a.apk` unpacked together: `lib\arm64-v8a\libil2cpp.so` and `assets\bin\Data\Managed\Metadata\global-metadata.dat`. |

How to read the pseudocode:
- Each function's header comment gives its IL2CPP name, e.g. `MenuController$$OpenCloseMenu`. In the C code it appears as `MenuController__OpenCloseMenu`, and calls look like `UnityEngine_GameObject__SetActive(obj, 1, 0)`. Translate those back into normal C# calls.
- A coroutine body is in `MenuController.<Name>d__N$$MoveNext`, and a lambda is in `<>c__DisplayClassN_M$$<Method>b__0`. The C# file only declares the coroutine method; its logic lives in `MoveNext`.
- `param_1` is `this`. Map `*(param_1 + 0x30)` to a field through the offsets in `dump.cs`; in `MenuController`, `0x30` is `menuPanel`. For arrays, the length is at `+0x18` and the elements start at `+0x20`.
- The last argument (`MethodInfo*`, usually `0`) is IL2CPP plumbing. So is the `if ((DAT_xxx & 1) == 0) { FUN_00b626c4(...) }` block at the top of a function, which initializes metadata. `goto LAB_…` jumps into `FUN_00b62918` and similar functions are null-reference or index-out-of-range throws. Drop all of these.
- `PTR_str_<text>_…` references are string literals: PlayerPrefs keys, scene names, animator parameters. Full values are in `stringliteral.json`.
- Value types with no methods (`SwipeData`, `SwipeDirection`, `TipoRecompensa`, `cameraModes`) have no `.c` file; their definitions are already in the C# stubs.
- **Ghidra drops float arguments.** Tween durations, colors passed in registers, and any 9th or later float (passed on the stack, e.g. `ColorScreenBackground`'s duration) are missing from the C. Get them from `calls.py` or `asm.py` below.
- A function that decompiles to a single `FUN_017854a8(...)` call starts with a Cortex-A53 erratum veneer (`b 0x16854xx`, which jumps straight back). Read that function with `asm.py`.
- Ghidra addresses are RVA + `0x100000`.

### Porting tools (`D:\RippedProject\Tools\`, run from Git Bash)

| Command | Output |
|---|---|
| `view.sh <Class>` | Readable pseudocode: IL2CPP noise stripped; `PTR_DAT_*` resolved to `{Type}` / `{Class.Method()}` (e.g. the target of each `TweenCallback`); `DAT_004…` and hex float bits shown as floats. |
| `python calls.py <Class> [MethodSubstr…]` | Each call with its resolved arguments: `this.field` names, float constants, and `stack0` for stack-passed floats. Values are best effort; `?` means a value it couldn't trace. |
| `python asm.py "<Class>$$<Method>"` | Raw ARM64 with calls and float constants annotated, for when the two views above disagree. |
| `python lit.py <mangled_str_label>` | The exact string literal, with escapes, for a `str_`/`"..."` label that `clean_pseudo.sh` mangled (accents, newlines). |
| `/d/RippedProject/Tools/portc.sh <Class> > /d/RippedProject/Il2CppDump/work/<x>.txt` | **The usual starting point.** One file per class with the `dump.cs` layout (field offsets), every method's cleaned pseudocode, and a `// floats:` note under each method listing resolved float arguments. Read it in chunks. (`port.py` is the uncompacted version.) |
| `python ctorconst.py <Class>` | Floats the constructor stores, i.e. field initializers such as `defaultScreenIntensity = 0.6f`. |

Workflow per class: `portc.sh` → resolve leftover `?` floats with `calls.py`/`asm.py` → write C# → compile-check.

Gotchas learned while porting:
- **DOTween `Ease` numbers** in `SetEase(t, N)`: 0 Unset, 1 Linear, 2 InSine, 3 OutSine, 4 InOutSine, 5 InQuad, 6 OutQuad, 7 InOutQuad, 8 InCubic, 9 OutCubic, 10 InOutCubic, 11 InQuart, 12 OutQuart, 13 InOutQuart, 14 InQuint, 15 OutQuint, 16 InOutQuint, 17 InExpo, 18 OutExpo, 19 InOutExpo, 20–22 Circ, 23–25 Elastic, 26 InBack, 27 OutBack, 28 InOutBack, 29–31 Bounce, 32 Flash. `RotateMode` 1 = FastBeyond360. No `SetEase` call means DOTween's default, OutQuad.
- **Vector3 statics:** `*(Vector3 static base + 0xb8)` at offset 0 is `Vector3.zero`, `0xC` is `one`, `0x48` is `forward`, `0x54` is `back`.
- **Packed constants:** `str d0, [x, #off]` writes two adjacent float fields at once. Find the `adrp`/`ldr d0` page and offset with `asm.py`, then read 8 bytes from `libil2cpp.so` at that file offset with `struct.unpack('<2f', …)`.
- `*(undefined2 *)(this + X) = 1` that covers `controlsEnabled` plus `cameraFollowWatch` is an inlined `EnableControls(true)`.
- Array element `i` is at `+0x20 + i*8`, so `+0x48` is index 5, not 1.
- Writing C# through a Bash heredoc breaks on quoting; use the Write tool (Read the file first).
- To check whether a script is used at all, grep its `.cs.meta` GUID across `Assets/**/*.unity` and `*.prefab`. Replace compiler-generated coroutine classes (`_003CName_003Ed__N`) with normal `yield` methods. Keep the tamper checks (`Application.identifier != "com.KokokatStudios.OmniWatch3D"` → green background, 179° FOV, objects destroyed); set the package name in Player Settings to that value when testing.

## Porting status

Ported (compiles, 0 errors), in main-flow order:
- **COPPA scene:** `COPPASetter`, `OpeningTransition`, `EndingTransition`.
- **Prototype scene:** `MenuController`, `PrototypeAnimationsController`, `PrototypeDisplayAliensController`, `FreeCameraController`, `TouchInputController`, `KeyboardInputController`, `QualityController`, `LanguajeController`, `OptionsController`, `AudioManager`, `IdController`, `ModifyRenderQueue`, `RotateAnimIcon`.
- **Every watch that a scene uses**, as `*AnimationsController` plus `*DisplayAliensController`: Nemetrix, Unitrix, Biomnitrix, Complete, RecalibratedAs, Antitrix, PotisAltiare (shares `UltimatrixDisplayAliensController`), Ultimatrix, PrototypeRat (`PrototypeRatAnimatinosController`), Reboot (uses `PrototypeDisplayAliensController`), Recalibrated. Scene mapping: the Prototype scripts also drive Prototype10k/23/Gwen/Zombi/Samurai; Reboot drives the Reboot* scenes; Recalibrated drives Recalibrated and RecalibratedAlbedo.

Stubbed on purpose: `RewardedAdsController` has no AdMob code. It enables both ad buttons on scene load, and `ShowRewardedAd` grants the reward right away (`SetUnlockPref`, `DisableLocksAsPrefs`, then it resets and re-enables the button). The original load/show/consent flow is in `Il2CppDump/work/rewarded.txt`; port it once the real GoogleMobileAds SDK replaces the stub.

Not yet ported:
- Dead scripts that no scene or prefab references (checked by `.meta` GUID): `CarnitrixAnimationsController`, `OmnitrixSelection`, `MenuManagement`, `DisplayAliens*`, `EnableAdMessage`/`disableAdMessage`, `QualitySelector`, `VeinsUI`, `SwipeInput`, `KeyboardInput`, `TapInput`, `BrandLogoScreen`, `LanguageControl`, `spin`, `SetQualityAtStart`, `ExampleWheelController`, `ControlPlayerPrefs`, `fpsCounter`, `MaskObject`, `AutoRotate`. Skip them unless one turns out to be needed.

Libraries replaced with real source (stub `.meta` GUIDs kept, compiles with 0 errors):
- `UnityEngine.UI`: ugui 1.0.0, copied from the editor's `BuiltInPackages\com.unity.ugui\Runtime`.
- `Unity.Postprocessing.Runtime`: com.unity.postprocessing **3.4.0** (identified by the stub's `isAndroidOpenGL`; 3.5 renamed it). AssetRipper's PP shaders were dummies, so the package's real `Shaders` tree is in `Assets/PostProcessing/Shaders`. Each real `.shader`/`.compute` carries a minimal `.meta` with the old dummy's GUID, which keeps `Assets/MonoBehaviour/PostProcessResources.asset` resolving. The dummy `Hidden_PostProcessing_*.shader` and `ComputeShader/*.asset` files were removed.
- DOTween **1.2.420** (core, from GitHub Demigiant/dotween commit `059e5ef`, without the `*Wrapper*` files, which only build with `COMPATIBLE`) and its 7 Modules in `Assets/Plugins/Assembly-CSharp-firstpass/DG/Tweening` (from `_DOTween.Assembly/bin/Modules`).
- NativeGallery **1.9.1** (yasirkula/UnityNativeGallery), plus `Assets/Plugins/NativeGallery/Android/NativeGallery.aar`.

PostProcessing compute shaders must be referenced from `PostProcessResources.asset` as `type: 3` (imported). With the old `type: 2` (native asset), Unity reports "Failed to load … .compute".

Still stubs: GoogleMobileAds (nothing references it now), the legacy PostProcessing v1 under `Assembly-CSharp/UnityEngine/PostProcessing` (no asset or script references it).

Custom shaders: all 19 in `Assets/Shader` are rebuilt from the shipped GLES3 GLSL in `data.unity3d` (header comment "Reconstructed from the shipped GLES3 program"), with each original `.meta` kept. They compile, but none has been checked visually against the real app yet. `SFHologram.cginc` is shared by `HologramShader` and `Holograma`, whose shipped programs are identical. The hologram shaders select variants with `shader_feature` keywords (`_SCAN_ON`/`_GLOW_ON`/`_GLITCH_ON`/`_GRID_ON`). To re-extract, use the Python venv and the `dump_shaders.py`/`dump_state.py` scripts in the session scratchpad (UnityPy). `UnityPy.export()` drops Blend and Stencil state and Unity 2022's `m_PlayerSubPrograms`, so read each pass's state and its `progVertex.m_PlayerSubPrograms` (program type 4 = GLES3, `m_BlobIndex` into the decompressed platform-9 blob) directly. PostProcessing shader includes were repointed from `Packages/com.unity.postprocessing/...` to `Assets/PostProcessing/...`.

## Commands

Ghidra and the headless scripts need `JAVA_HOME` set to the portable JDK. In PowerShell:

```powershell
$env:JAVA_HOME = "D:\RippedProject\Tools\jdk-21.0.12.1+1"

# Open the analyzed project in the GUI
& D:\RippedProject\Tools\ghidra_12.1.4_PUBLIC\ghidraRun.bat      # then File → Open Project → D:\RippedProject\Ghidra\Omni.gpr

# Re-export pseudocode (e.g. after editing game_classes.txt to add classes) without re-analyzing
& D:\RippedProject\Tools\ghidra_12.1.4_PUBLIC\support\analyzeHeadless.bat D:\RippedProject\Ghidra Omni -process libil2cpp.so -noanalysis -scriptPath D:\RippedProject\Tools\ghidra_scripts -postScript ExportIl2CppPseudocode.java D:\RippedProject\Il2CppDump\script.json D:\RippedProject\Il2CppDump\game_classes.txt D:\RippedProject\Il2CppDump\pseudocode

# Re-run Il2CppDumper
& D:\RippedProject\Tools\Il2CppDumper\Il2CppDumper.exe C:\Users\AfeefAbdullah\Downloads\omni_merged\lib\arm64-v8a\libil2cpp.so C:\Users\AfeefAbdullah\Downloads\omni_merged\assets\bin\Data\Managed\Metadata\global-metadata.dat D:\RippedProject\Il2CppDump

# Compile-check the Unity project headlessly; errors land in the log
& "C:\Program Files\Unity\Hub\Editor\2022.3.62f3\Editor\Unity.exe" -batchmode -quit -projectPath I:\Projects\ProjectOmnitrix -logFile D:\RippedProject\unity_build.log
```

`game_classes.txt` lists the class names whose methods get exported. It defaults to every top-level `.cs` file in `Assets/Scripts/Assembly-CSharp/`.

## Project layout

AssetRipper organizes assets **by Unity type**, not by feature: `Assets/Mesh`, `Material`, `Texture2D`, `AnimationClip`, `AnimatorController`, `Shader` (dummy shaders, since the originals were compiled for Android and can't be decompiled), `AudioClip`, and `MonoBehaviour` (ScriptableObject assets such as post-processing profiles). Code lives in `Assets/Scripts/<AssemblyName>/`, one folder per original assembly, plus `Assets/Plugins/Assembly-CSharp-firstpass/`. `Assets/Scripts/Assembly-CSharp/UnityEngine/PostProcessing/` is the legacy Post-Processing Stack v1, compiled into the game assembly.

## App structure

Build order is in `ProjectSettings/EditorBuildSettings.asset` (27 scenes):
1. `Assets/Scenes/COPPAScreen.unity`: the age/COPPA gate (`COPPASetter`).
2. `Assets/Scenes/Watches/*.unity`: **one scene per watch variant**, such as Prototype, Recalibrated, Ultimatrix, Nemetrix, Antitrix, Biomnitrix, Complete*, Reboot*, Unitrix, Samurai and PotisAltiare. A same-named subfolder next to each scene holds its baked lighting data.

Each watch family pairs a `<Watch>AnimationsController` (opening, closing and transforming the watch, mostly with DOTween) with a `<Watch>DisplayAliensController` or `DisplayAliens*` (cycling alien icons on the dial). They talk to each other through serialized references, e.g. `menuController` and `prototypeDisplayAliensController`. Shared systems:
- Input: `SwipeDetector`, `SwipeInput`, `TapInput`, `TouchInputController` (`OnSwipe` event), `KeyboardInput*`.
- Menus: `MenuController` handles the menu, scene switching (`SwitchScene(string)`), camera switching and saving a screenshot to the gallery. `MenuManagement`, `OptionsController` and `QualityController`/`QualitySelector` cover the rest.
- Localization: `LanguageControl`/`LanguajeController`.
- Settings: `ControlPlayerPrefs`.
- Ads: `RewardedAdsController` is the only user of the GoogleMobileAds API and of UMP consent. `EnableAdMessage`/`disableAdMessage` round this out.
- Audio: `AudioManager`, `Sound`.
