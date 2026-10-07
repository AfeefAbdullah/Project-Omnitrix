using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	public PrototypeDisplayAliensController prototypeDisplayAliensController;

	[Header("Screen Animation Elements")]
	public SpriteRenderer screenBackground;

	public Transform orangeRingsMask;

	public SpriteRenderer orangeRingsBackground;

	public Animator screenAnimator;

	[Header("Watch Parts")]
	public Transform mobileParts;

	public Transform ring;

	public MeshRenderer[] shinyParts;

	public GameObject core;

	public Transform screen;

	[Header("Lights")]
	public Light[] flashLights;

	public float maxFlashLightIntensity = 100f;

	public float defaultFlashLightIntensity = 10f;

	[Header("Materials")]
	public Material greenLightMat;

	public Material redLightMat;

	public Material yellowLightMat;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public ParticleSystem selfDestructParticles;

	public Transform energyRing;

	public Transform energySphere;

	public GameObject selfDestructElements;

	public Renderer watchScreenRenderer;

	public float defaultScreenIntensity = 0.6f;

	private float currentScreenIntensity;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	public MeshRenderer transformationBackground;

	private float backgroundCellSize;

	public SpriteRenderer shineCover;

	private float shinePosition;

	[Header("Modes Colors")]
	public Color defaultColor;

	public Color transformedColor;

	public Color timeOutColor;

	public Color scanModeColor;

	public Color selfDestructColor;

	public Color recalibrationColor;

	public Color disabledColor;

	[Header("Flash Colors")]
	public Color transformFlashColor;

	public Color timeOutFlashColor;

	public Color selfDestructFlashColor;

	[Header("Extra")]
	public GameObject transformationScene;

	public float mobilePartsUpHeight = 0.34f;

	public string secretCode;

	public bool transformAnimation;

	private string[] selectSounds = { "Select1", "Select2", "Select3" };

	private string[] transformationThemes = { "TransformationTheme1", "TransformationTheme2" };

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private int timeOutBlinks;

	private int masterControlRotations;

	private int scaneModeRotations;

	private float startRotationDuration = 1.3f;

	private float scanRotationDuration = 0.7f;

	private float endrotationDuration = 1.3f;

	private int selfDestructBlinks;

	private int selfDestructPhase;

	private Color originalRingsColor;

	private int recalibrationBlinks;

	private int recalibrationRotations;

	// The original looks the AudioManager up on every call.
	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		watchStatus = "normal";
		secretCode = "";
		cameraFollowWatch = true;
		currentScreenIntensity = defaultScreenIntensity;
		originalRingsColor = orangeRingsBackground.color;
		orangeRingsMask.localScale = new Vector3(2f, 2f, 2f);
		backgroundCellSize = 1f;
		transformationScene.SetActive(false);
		selfDestructElements.SetActive(false);
		ColorScreenBackground(defaultColor, defaultColor, 0f);
		CameraIntro();
	}

	private void Update()
	{
		energyRing.Rotate(Vector3.forward * 50f * Time.deltaTime);
		EnergySphereFollowCam();
		UpdateScreenEmissionIntensity();
		UpdateLightShaftsProgress();
		UpdateBackgroundCellSize();
		UpdateShinePosition();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.transform.LookAt(cameraFollowPoint.position);
		}
	}

	private void UpdateScreenEmissionIntensity()
	{
		float i = currentScreenIntensity;
		watchScreenRenderer.material.SetColor("_EmissionColor", new Color(i, i, i, i));
		watchScreenRenderer.material.EnableKeyword("_EMISSION");
	}

	private void UpdateBackgroundCellSize()
	{
		transformationBackground.material.SetFloat("_CellSizeControl", backgroundCellSize);
	}

	private void UpdateShinePosition()
	{
		shineCover.material.SetFloat("_ShineLocation", shinePosition);
	}

	private void UpdateLightShaftsProgress()
	{
		for (int i = 0; i < 2; i++)
		{
			Material material = lightShaftsQuads[i].material;
			material.SetFloat("_AppearProgress", lightShaftsProgress);
			Color flash = flashScreen.color;
			material.SetColor("_MainColor", new Color(flash.r, flash.g, flash.b, 1f));
		}
	}

	private void ModifyFlashLightsIntensity(Color color, float startValue, float endValue, float duration)
	{
		foreach (Light flashLight in flashLights)
		{
			flashLight.color = color;
		}
		foreach (Light flashLight in flashLights)
		{
			flashLight.intensity = startValue;
		}
		foreach (Light flashLight in flashLights)
		{
			flashLight.DOIntensity(endValue, duration).SetEase(Ease.Flash);
		}
	}

	private void ColorScreenBackground(Color startColor, Color endColor, float transitionDuration, TweenCallback onCompleteMethod = null, Ease ease = Ease.Flash)
	{
		screenBackground.color = startColor;
		screenBackground.DOColor(endColor, transitionDuration).SetEase(ease).OnComplete(onCompleteMethod);
	}

	private void FlashScreen(Color color, float startValue, float endValue, float duration, Ease ease = Ease.Flash, TweenCallback onCompleteMethod = null)
	{
		flashScreen.color = new Color(color.r, color.g, color.b, color.a * startValue);
		flashScreen.DOFade(endValue, duration).OnComplete(onCompleteMethod).SetEase(ease);
	}

	private void ModifyScreenEmissiveness(float startValue, float endValue, float duration, TweenCallback onCompleteCallback = null)
	{
		currentScreenIntensity = startValue;
		DOTween.To(() => currentScreenIntensity, x => currentScreenIntensity = x, endValue, duration).OnComplete(onCompleteCallback);
	}

	private void CameraIntro()
	{
		defaultCameraHolder.position = cameraPositions[0].position;
		defaultCameraHolder.rotation = cameraPositions[0].rotation;
		defaultCameraHolder.DOMove(cameraPositions[1].position, 2f).SetEase(Ease.OutQuad).OnComplete(() => EnableControls(true));
	}

	private void EnableControls(bool status)
	{
		controlsEnabled = status;
		cameraFollowWatch = false;
	}

	public void ToggleTransformAnimation(bool newState)
	{
		transformAnimation = newState;
	}

	public void Act()
	{
		if (menuController.menuOpen || !controlsEnabled)
		{
			return;
		}
		switch (watchStatus)
		{
		case "normal":
			EnableControls(false);
			ActivationAnim();
			watchStatus = "activated";
			break;
		case "activated":
			EnableControls(false);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transforming":
			watchStatus = "transformed";
			break;
		case "transformed":
			EnableControls(false);
			TimeOutAnim();
			watchStatus = "timeOut";
			break;
		case "timeOut":
			EnableControls(false);
			ResetWatch();
			watchStatus = "normal";
			break;
		case "recalibration":
			RecalibrationPress();
			break;
		default:
			Debug.Log("Invalid Status");
			break;
		}
	}

	public void ActivationAnim()
	{
		core.SetActive(true);
		ModifyFlashLightsIntensity(defaultColor, 0f, 10f, 0.2f);
		mobileParts.DOMoveY(mobileParts.position.y + mobilePartsUpHeight, 0.5f).SetEase(Ease.OutBack);
		Audio.Play("Activated");
		StartCoroutine(WaitForActivationAnim());
		ModifyScreenEmissiveness(defaultScreenIntensity, 1f, 0.5f);
	}

	private void FlickScreen(float intensity)
	{
		ModifyScreenEmissiveness(currentScreenIntensity, currentScreenIntensity + intensity, 0.1f,
			() => ModifyScreenEmissiveness(currentScreenIntensity, currentScreenIntensity - intensity, 0.14f));
	}

	private IEnumerator WaitForActivationAnim()
	{
		yield return new WaitForSeconds(0.8f);
		CameraZoom();
		ScreenActivationAnim();
	}

	private void CameraZoom()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 1f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 1f).SetEase(Ease.OutSine);
	}

	public void ScreenActivationAnim()
	{
		Audio.Play("Preparing");
		StartCoroutine(ScreenActivationA());
		screenAnimator.Play("ScreenActivation");
	}

	private IEnumerator ScreenActivationA()
	{
		yield return new WaitForSeconds(0.5f);
		DisplayAliens();
	}

	public void DisplayAliens()
	{
		prototypeDisplayAliensController.ShowAlien(0.25f);
		Audio.Play("Prepared");
		Audio.Play("Waiting");
		StartCoroutine(WaitToEnableControls(0.5f));
	}

	private IEnumerator WaitToEnableControls(float time)
	{
		yield return new WaitForSeconds(time);
		EnableControls(true);
	}

	private void PushAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y - mobilePartsUpHeight, 0.25f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		StartTransformFlash(0.5f);
		Audio.Stop("Waiting");
	}

	private void StartTransformFlash(float duration = 0.5f)
	{
		ModifyScreenEmissiveness(currentScreenIntensity, 2f, duration);
		ModifyFlashLightsIntensity(defaultColor, 0f, 100f, duration);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
		Audio.Play("Transform");
		if (!transformAnimation)
		{
			FlashScreen(transformFlashColor, 0f, 1f, duration, Ease.InExpo, SwitchToTransformedScene);
			return;
		}
		Audio.Play(transformationThemes[Random.Range(0, 2)]);
		FlashScreen(transformFlashColor, 0f, 1f, duration, Ease.InExpo, TransformingSequence);
		menuController.ShowLetterboxing(true);
	}

	private void TransformingSequence()
	{
		EndTransformFlash(1f);
		transformationBackground.gameObject.SetActive(true);
		DOTween.To(() => backgroundCellSize, x => backgroundCellSize = x, 0.2f, 3.5f);
		defaultCameraHolder.DOMove(cameraPositions[3].position, 5f);
		defaultCameraHolder.DORotate(cameraPositions[3].rotation.eulerAngles, 5f);
		StartCoroutine(TransformingSequenceA());
	}

	private IEnumerator TransformingSequenceA()
	{
		yield return new WaitForSeconds(1.5f);
		prototypeDisplayAliensController.HideAlien(0.5f);
		screenAnimator.SetFloat("SpeedMultiplier", 0.25f);
		screenAnimator.Play("ReverseScreenActivation");
		yield return new WaitForSeconds(5f);
		Audio.Play("Transform");
		ModifyScreenEmissiveness(currentScreenIntensity, 2f, 0.5f);
		ModifyFlashLightsIntensity(defaultColor, 0f, 100f, 0.5f);
		FlashScreen(transformFlashColor, 0f, 1f, 1f, Ease.InExpo, SwitchToTransformedScene);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, 0.5f).SetEase(Ease.InQuint);
	}

	private void SwitchToTransformedScene()
	{
		transformationScene.SetActive(true);
		prototypeDisplayAliensController.HideAlien(0.25f);
		ColorScreenBackground(defaultColor, transformedColor, 0f);
		screenAnimator.Play("Default");
		defaultCameraHolder.position = cameraPositions[4].position;
		defaultCameraHolder.rotation = cameraPositions[4].rotation;
		transformationBackground.gameObject.SetActive(false);
		EndTransformFlash(1.5f, false, () =>
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true)));
		menuController.ShowLetterboxing(false);
	}

	private void EndTransformFlash(float duration = 0.5f, bool enableControlsAtEnd = false, TweenCallback onCompleteCallback = null)
	{
		ModifyScreenEmissiveness(2f, defaultScreenIntensity, duration);
		ModifyFlashLightsIntensity(transformedColor, 0f, 10f, duration);
		FlashScreen(transformFlashColor, 1f, 0f, duration, Ease.InQuart, () => EnableControls(enableControlsAtEnd));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration).SetEase(Ease.OutQuint).OnComplete(onCompleteCallback);
	}

	private void TimeOutAnim()
	{
		Audio.Play("TimeOut");
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = redLightMat;
		}
		TimeOutBlink();
	}

	private void TimeOutBlink()
	{
		timeOutBlinks++;
		bool lastBlink = timeOutBlinks >= 5;
		ColorScreenBackground(timeOutColor, transformedColor, lastBlink ? 1f : 0.5f, lastBlink ? StartTimeOutFlash : (TweenCallback)TimeOutBlink);
		ModifyFlashLightsIntensity(timeOutColor, 20f, 0f, lastBlink ? 1f : 0.45f);
	}

	private void StartTimeOutFlash()
	{
		ModifyScreenEmissiveness(defaultScreenIntensity, 2f, 0.5f);
		ModifyFlashLightsIntensity(timeOutColor, 0f, 100f, 0.5f);
		ColorScreenBackground(transformedColor, timeOutColor, 0.5f);
		FlashScreen(timeOutFlashColor, 0f, 1f, 0.5f, Ease.InExpo, SwitchToNormalScene);
		lightShaftsProgress = 0f;
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, 0.5f).SetEase(Ease.InQuint);
	}

	private void SwitchToNormalScene()
	{
		transformationScene.SetActive(false);
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		EndTimeOutFlash();
	}

	private void EndTimeOutFlash()
	{
		ModifyScreenEmissiveness(2f, defaultScreenIntensity, 0.5f);
		ModifyFlashLightsIntensity(timeOutColor, 100f, 0f, 0.5f);
		FlashScreen(timeOutFlashColor, 1f, 0f, 0.5f, Ease.InQuart, () => EnableControls(true));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, 0.5f).SetEase(Ease.OutQuint);
	}

	private void ResetWatch()
	{
		Audio.Play("Recharge");
		watchStatus = "normal";
		secretCode = "";
		scaneModeRotations = 0;
		selfDestructBlinks = 0;
		selfDestructPhase = 0;
		recalibrationBlinks = 0;
		recalibrationRotations = 0;
		timeOutBlinks = 0;
		masterControlRotations = 0;
		currentScreenIntensity = defaultScreenIntensity;
		backgroundCellSize = 1f;
		shinePosition = 0f;
		ModifyFlashLightsIntensity(defaultColor, 0f, 0f, 0f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = greenLightMat;
		}
		orangeRingsBackground.DOFade(0f, 0.15f).OnComplete(() =>
		{
			orangeRingsMask.localScale = new Vector3(2f, 2f, 2f);
			orangeRingsBackground.DOFade(1f, 0.15f);
		});
		screenAnimator.Play("Default");
		screenAnimator.SetFloat("SpeedMultiplier", 1f);
		energyRing.GetComponent<SpriteRenderer>().DOFade(1f, 0f);
		ColorScreenBackground(screenBackground.color, defaultColor, 0.15f, () => EnableControls(true));
	}

	public void RotateLeft()
	{
		if (controlsEnabled && watchStatus == "activated" && !menuController.menuOpen)
		{
			EnableControls(false);
			RotateRing(-90f, 0.25f).SetEase(Ease.Flash).OnComplete(SwitchAlienLeft);
			FlickScreen(0.1f);
			Audio.Play("Dial");
			UpdateSecretCode("L");
		}
		else if (controlsEnabled && watchStatus == "recalibration" && !menuController.menuOpen)
		{
			RecalibrationRotationLeft();
		}
	}

	private void SwitchAlienLeft()
	{
		Audio.Play(selectSounds[Random.Range(0, 3)]);
		ring.localRotation = Quaternion.Euler(0f, 0f, 0f);
		// Swapped on purpose in the original: the display list scrolls the other way.
		prototypeDisplayAliensController.SwitchAlienRight();
		CheckForSecretCode();
	}

	public void RotateRight()
	{
		if (controlsEnabled && watchStatus == "activated" && !menuController.menuOpen)
		{
			EnableControls(false);
			RotateRing(90f, 0.25f).SetEase(Ease.Flash).OnComplete(SwitchAlienRight);
			FlickScreen(0.1f);
			Audio.Play("Dial");
			UpdateSecretCode("R");
		}
		else if (controlsEnabled && watchStatus == "recalibration" && !menuController.menuOpen)
		{
			RecalibrationRotationRight();
		}
	}

	private void SwitchAlienRight()
	{
		Audio.Play(selectSounds[Random.Range(0, 3)]);
		ring.localRotation = Quaternion.Euler(0f, 0f, 0f);
		prototypeDisplayAliensController.SwitchAlienLeft();
		CheckForSecretCode();
	}

	private Tween RotateRing(float degrees, float duration)
	{
		return ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), duration);
	}

	private void UpdateSecretCode(string newDirection)
	{
		if (secretCode.Length > 5)
		{
			secretCode = secretCode.Remove(0, 1);
		}
		secretCode += newDirection;
	}

	private void CheckForSecretCode()
	{
		switch (secretCode)
		{
		case "RRLLRL":
			MasterControlAnimA();
			break;
		case "LRLRLL":
			ScanModeAnimA();
			break;
		case "RLRLRR":
			SelfDestructAnimA();
			break;
		case "LLRRLR":
			RecalibrationAnimA();
			break;
		default:
			EnableControls(true);
			break;
		}
	}

	private void MasterControlAnimA()
	{
		EnableControls(false);
		ColorScreenBackground(defaultColor, transformedColor, 0.5f);
		prototypeDisplayAliensController.HideAlien(0.25f);
		screenAnimator.Play("ReverseScreenActivation");
		Audio.Play("MasterControl");
		Audio.Stop("Waiting");
		StartCoroutine(MasterControlAnimB());
		StartCoroutine(MasterControlAnimC());
	}

	private IEnumerator MasterControlAnimB()
	{
		yield return new WaitForSeconds(0.8f);
		ColorScreenBackground(transformedColor, defaultColor, 0.25f);
	}

	private IEnumerator MasterControlAnimC()
	{
		yield return new WaitForSeconds(1.4f);
		RotateRing(90f, 0.65f).SetEase(Ease.InCubic);
		screenAnimator.Play("ScreenActivation");
		StartCoroutine(MasterControlAnimE());
	}

	private IEnumerator MasterControlAnimE()
	{
		yield return new WaitForSeconds(0.64f);
		prototypeDisplayAliensController.ShowAlien(0.25f);
		MasterControlAnimG();
	}

	private void MasterControlAnimG()
	{
		RotateRing(90f, 0.35f).SetEase(Ease.Flash).OnComplete(MasterControlAnimH);
	}

	private void MasterControlAnimH()
	{
		ring.localRotation = Quaternion.Euler(0f, 0f, 0f);
		masterControlRotations++;
		prototypeDisplayAliensController.SwitchAlienLeft();
		if (masterControlRotations < 20)
		{
			MasterControlAnimG();
			return;
		}
		PushAnim();
		Audio.Stop("MasterControl");
		watchStatus = "transformed";
	}

	private void ScanModeAnimA()
	{
		EnableControls(false);
		ColorScreenBackground(defaultColor, transformedColor, 0.5f);
		prototypeDisplayAliensController.HideAlien(0.25f);
		screenAnimator.Play("ReverseScreenActivation");
		Audio.Play("SecretCodeActivation");
		Audio.Stop("Waiting");
		StartCoroutine(ScanModeAnimB());
	}

	private IEnumerator ScanModeAnimB()
	{
		yield return new WaitForSeconds(1.05f);
		Audio.Play("ScanActivation");
		ColorScreenBackground(transformedColor, scanModeColor, 0.15f);
		ModifyFlashLightsIntensity(scanModeColor, 10f, 10f, 0f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = yellowLightMat;
		}
		yield return new WaitForSeconds(0.45f);
		ScanModeAnimD();
	}

	private void ScanModeAnimD()
	{
		Audio.Play("Preparing");
		screenAnimator.Play("ScreenScanMode");
		StartCoroutine(WaitForScreenScanAnim());
	}

	private IEnumerator WaitForScreenScanAnim()
	{
		yield return new WaitForSeconds(0.7f);
		Audio.Play("StartScan");
		yield return new WaitForSeconds(0.1f);
		ScanModeAnimF();
	}

	private void ScanModeAnimF()
	{
		Audio.Play("Scanning");
		SpinRingAndScreen(startRotationDuration, Ease.InQuad, ScanModeAnimG);
	}

	private void ScanModeAnimG()
	{
		scaneModeRotations++;
		SpinRingAndScreen(scanRotationDuration, Ease.Flash, ScanModeAnimH);
	}

	private void ScanModeAnimH()
	{
		if (scaneModeRotations < 5)
		{
			ScanModeAnimG();
		}
		else
		{
			ScanModeAnimI();
		}
	}

	private void ScanModeAnimI()
	{
		SpinRingAndScreen(endrotationDuration, Ease.OutQuad, ScanModeAnimJ);
	}

	// The original targets the screen with the ring's angles too.
	private void SpinRingAndScreen(float duration, Ease ease, TweenCallback onComplete)
	{
		Vector3 fullTurn = new Vector3(0f, 360f, 0f);
		ring.localRotation = Quaternion.Euler(0f, 0f, 0f);
		ring.DORotate(ring.localEulerAngles + fullTurn, duration, RotateMode.FastBeyond360).SetEase(ease);
		screen.localRotation = Quaternion.Euler(0f, 0f, 0f);
		screen.DORotate(ring.localEulerAngles + fullTurn, duration, RotateMode.FastBeyond360).SetEase(ease).OnComplete(onComplete);
	}

	private void ScanModeAnimJ()
	{
		Audio.Play("NewAlien");
		screenAnimator.Play("ScreenEndScanMode");
		StartCoroutine(ScanModeAnimK());
	}

	private IEnumerator ScanModeAnimK()
	{
		yield return new WaitForSeconds(0.25f);
		scaneModeRotations = 0;
		secretCode = "";
		DisplayAliens();
		ColorScreenBackground(scanModeColor, defaultColor, 0.25f);
		ModifyFlashLightsIntensity(defaultColor, 10f, 10f, 0f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = greenLightMat;
		}
	}

	private void SelfDestructAnimA()
	{
		EnableControls(false);
		selfDestructElements.SetActive(true);
		prototypeDisplayAliensController.HideAlien(0.25f);
		screenAnimator.Play("ReverseScreenActivation");
		Audio.Play("SecretCodeActivation");
		Audio.Stop("Waiting");
		StartCoroutine(SelfDestructAnimB());
	}

	private IEnumerator SelfDestructAnimB()
	{
		yield return new WaitForSeconds(1.05f);
		SelfDestructAnimC();
		Audio.Play("Mechanism1");
		Audio.Play("SelfDestructPowerCharge");
		Audio.Play("Waiting");
		Audio.Play("SelfDestructStartCharge");
	}

	private void SelfDestructAnimC()
	{
		Audio.Play("SelfDestructBeep");
		selfDestructBlinks++;
		ColorScreenBackground(defaultColor, selfDestructColor, 0.025f, () => StartCoroutine(SelfDestructAnimD()));
		ModifyFlashLightsIntensity(selfDestructColor, 10f, 10f, 0f);
	}

	private IEnumerator SelfDestructAnimD()
	{
		yield return new WaitForSeconds(0.15f);
		ColorScreenBackground(selfDestructColor, defaultColor, 0.15f, SelfDestructAnimE);
	}

	private void SelfDestructAnimE()
	{
		if (selfDestructBlinks > 2)
		{
			selfDestructBlinks = 0;
			StartCoroutine(SelfDestructAnimF());
		}
		else
		{
			SelfDestructAnimC();
		}
	}

	private IEnumerator SelfDestructAnimF()
	{
		yield return new WaitForSeconds(0.5f);
		SelfDestructAnimG();
	}

	private void SelfDestructAnimG()
	{
		Audio.Play("Mechanism1");
		orangeRingsBackground.color = new Color(1f, 0f, 0f, 1f);
		orangeRingsBackground.DOColor(originalRingsColor, 0.4f).SetEase(Ease.Flash);
		ModifyScreenEmissiveness(1f, defaultScreenIntensity, 1f);
		float scale;
		switch (selfDestructPhase)
		{
		case 0:
			scale = 1.55f;
			break;
		case 1:
			scale = 1.15f;
			break;
		case 2:
			scale = 0.76f;
			break;
		case 3:
			scale = 0.38f;
			break;
		case 4:
			scale = 0f;
			break;
		default:
			return;
		}
		orangeRingsMask.DOScale(Vector3.one * scale, 0.1f).SetEase(Ease.Flash).OnComplete(() => StartCoroutine(SelfDestructAnimH()));
	}

	private IEnumerator SelfDestructAnimH()
	{
		if (selfDestructPhase < 4)
		{
			yield return new WaitForSeconds(0.15f);
			selfDestructPhase++;
			SelfDestructAnimC();
		}
		else
		{
			InvokeRepeating("SelfDestructAnimI", 0f, 0.15f);
			SelfDestructAnimK();
		}
	}

	private void SelfDestructAnimI()
	{
		orangeRingsBackground.DOFade(0f, 0f);
		StartCoroutine(SelfDestructAnimJ());
	}

	private IEnumerator SelfDestructAnimJ()
	{
		yield return new WaitForSeconds(0.075f);
		orangeRingsBackground.DOFade(1f, 0f);
	}

	private void SelfDestructAnimK()
	{
		selfDestructParticles.Play();
		energyRing.DOScale(0.35f, 2f).SetEase(Ease.OutBack).OnComplete(SelfDestructAnimL);
		Audio.Play("SelfDestructExplosionStart");
		Audio.Stop("Waiting");
	}

	private void SelfDestructAnimL()
	{
		ModifyFlashLightsIntensity(selfDestructColor, 10f, 100f, 1f);
		energyRing.DOScale(1.5f, 1f).SetEase(Ease.InSine).OnComplete(SelfDestructAnimM);
		energyRing.GetComponent<SpriteRenderer>().DOFade(0f, 1f).SetEase(Ease.InExpo);
		energySphere.DOScale(2f, 3.5f).SetEase(Ease.Flash);
		energySphere.GetComponent<SpriteRenderer>().DOFade(0.85f, 0.5f);
		StartCoroutine(SelfDestructAnimN());
		Audio.Play("SelfDestructExplosion");
		Audio.Play("SelfDestructExplosion2");
	}

	private void SelfDestructAnimM()
	{
		FlashScreen(selfDestructFlashColor, 0f, 1f, 2f, Ease.InExpo);
		ModifyScreenEmissiveness(defaultScreenIntensity, 2f, 1f);
	}

	private IEnumerator SelfDestructAnimN()
	{
		yield return new WaitForSeconds(3.5f);
		CancelInvoke("SelfDestructAnimI");
		selfDestructParticles.Stop();
		selfDestructParticles.Clear();
		energyRing.localScale = Vector3.zero;
		energySphere.localScale = Vector3.zero;
		energySphere.GetComponent<SpriteRenderer>().DOFade(0f, 0f);
		mobileParts.DOMoveY(mobileParts.position.y - mobilePartsUpHeight, 0f);
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		ModifyScreenEmissiveness(0.4f, defaultScreenIntensity, 0f);
		ModifyFlashLightsIntensity(defaultColor, 0f, 0f, 0f);
		watchStatus = "timeOut";
		selfDestructElements.SetActive(false);
		FlashScreen(selfDestructFlashColor, 1f, 0f, 2f, Ease.InQuart, () => EnableControls(true));
	}

	private void EnergySphereFollowCam()
	{
		energySphere.LookAt(Camera.main.transform.position);
	}

	private void RecalibrationAnimA()
	{
		EnableControls(false);
		watchStatus = "recalibration";
		prototypeDisplayAliensController.HideAlien(0.25f);
		screenAnimator.Play("ReverseScreenActivation");
		Audio.Play("SecretCodeActivation");
		Audio.Stop("Waiting");
		StartCoroutine(RecalibrationAnimB());
	}

	private IEnumerator RecalibrationAnimB()
	{
		yield return new WaitForSeconds(1.05f);
		mobileParts.DOMoveY(mobileParts.position.y - mobilePartsUpHeight, 0.2f).SetEase(Ease.Flash).OnComplete(RecalibrationAnimC);
		Audio.Play("RecalibrationMode");
	}

	private void RecalibrationAnimC()
	{
		core.SetActive(false);
		ModifyScreenEmissiveness(defaultScreenIntensity, 2f, 1f);
		ModifyFlashLightsIntensity(defaultColor, 10f, 50f, 1f);
		StartCoroutine(RecalibrationAnimD());
	}

	private IEnumerator RecalibrationAnimD()
	{
		yield return new WaitForSeconds(1.5f);
		ModifyScreenEmissiveness(2f, defaultScreenIntensity, 2.5f);
		ModifyFlashLightsIntensity(defaultColor, 50f, 0f, 2.5f);
		RecalibrationAnimE();
	}

	private void RecalibrationAnimE()
	{
		if (recalibrationBlinks < 8)
		{
			Audio.Play("RecalibrationModeBeep");
			ColorScreenBackground(defaultColor, recalibrationColor, 0.025f, RecalibrationAnimF);
			recalibrationBlinks++;
		}
	}

	private void RecalibrationAnimF()
	{
		ColorScreenBackground(recalibrationColor, disabledColor, 0.175f, RecalibrationAnimG);
	}

	private void RecalibrationAnimG()
	{
		if (recalibrationBlinks > 7)
		{
			ColorScreenBackground(disabledColor, recalibrationColor, 0.55f, () => EnableControls(true));
			ModifyFlashLightsIntensity(recalibrationColor, 0f, 5f, 0.55f);
		}
		else
		{
			ColorScreenBackground(disabledColor, recalibrationColor, 0.025f, RecalibrationAnimH);
		}
	}

	private void RecalibrationAnimH()
	{
		ColorScreenBackground(recalibrationColor, disabledColor, 0.375f, RecalibrationAnimE);
	}

	private void RecalibrationRotationLeft()
	{
		RecalibrationRotation(-90f);
	}

	private void RecalibrationRotationRight()
	{
		RecalibrationRotation(90f);
	}

	private void RecalibrationRotation(float degrees)
	{
		EnableControls(false);
		ring.localRotation = Quaternion.Euler(0f, 0f, 0f);
		RotateRing(degrees, 0.25f).SetEase(Ease.Flash);
		Audio.Play("Dial");
		Audio.Play("RecalibrationModeRotate");
		StartCoroutine(RecalibrationAnimI());
	}

	private void RecalibrationPress()
	{
		EnableControls(false);
		Audio.Play("RecalibrationModeDisabled");
		ColorScreenBackground(Color.cyan, recalibrationColor, 1f, () => EnableControls(true), Ease.OutQuart);
	}

	private IEnumerator RecalibrationAnimI()
	{
		yield return new WaitForSeconds(0.6f);
		recalibrationRotations++;
		if (recalibrationRotations < 4)
		{
			EnableControls(true);
			yield break;
		}
		Audio.Play("RecalibrationModeEnd");
		defaultCameraHolder.DOMove(cameraPositions[1].position, 2f);
		defaultCameraHolder.DORotate(cameraPositions[1].rotation.eulerAngles, 2f).SetEase(Ease.OutQuint);
		ColorScreenBackground(Color.cyan, recalibrationColor, 0.2f, RecalibrationAnimJ);
	}

	private void RecalibrationAnimJ()
	{
		ColorScreenBackground(Color.cyan, recalibrationColor, 2f, RecalibrationAnimK, Ease.OutQuart);
	}

	private void RecalibrationAnimK()
	{
		ColorScreenBackground(recalibrationColor, Color.cyan, 2f, ResetWatch, Ease.InQuad);
	}
}
