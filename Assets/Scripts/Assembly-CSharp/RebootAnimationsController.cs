using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RebootAnimationsController : MonoBehaviour
{
	public enum gimmick
	{
		None = 0,
		OmniEnhanced = 1,
		OmniKix = 2
	}

	public gimmick watchGimmick;

	[Header("Scripts References")]
	public MenuController menuController;

	public PrototypeDisplayAliensController prototypeDisplayAliensController;

	[Header("Screen Animation Elements")]
	public Animator screenAnimator;

	public SpriteRenderer screenBackground;

	public MeshRenderer[] diamonds;

	public Material grayMaterial;

	public Material blackMaterial;

	[Header("Watch Parts")]
	public Transform mobileParts;

	public Transform ring;

	public MeshRenderer[] shinyParts;

	public GameObject core;

	[Header("Lights")]
	public Color[] lightColors;

	public float maxFlashLightIntensity = 100f;

	public float defaultFlashLightIntensity = 10f;

	public Light omniEnhancedLight;

	[Header("Materials")]
	public Material greenLightMat;

	public Material redLightMat;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public ParticleSystem omniEnhancedParticles;

	public GameObject omniEnhancedTransformationBackground;

	public Renderer watchScreenRenderer;

	public float defaultScreenIntensity = 0.6f;

	private float currentScreenIntensity;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	public SpriteRenderer shineCover;

	private float shinePosition;

	[Header("Modes Colors")]
	public Color defaultColor;

	public Color transformedColor;

	public Color timeOutColor;

	[Header("Flash Colors")]
	public Color transformFlashColor;

	public Color timeOutFlashColor;

	public Color omniEnhancedFlashColor;

	[Header("Omni-Kix Parts")]
	public Transform screenTrnsScene;

	public Transform part0;

	public Transform[] partA;

	public Transform[] partB;

	public Transform[] partC;

	public Transform[] partD;

	public Transform[] partE;

	public Transform[] partF;

	public Transform[] partG;

	public Transform[] kixParts;

	public List<Vector3> kixPartsOgPos;

	[Header("Extra")]
	public Transform transformationSceneBg;

	public GameObject transformationScene;

	public bool omniEnhancedAnimation;

	public bool omniKixAnimation;

	private string[] selectSounds = { "Select1", "Select2", "Select3" };

	private Vector3[] ogDiamondsPos = { Vector3.zero, Vector3.zero };

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private float waitCounter;

	// The original looks the AudioManager up on every call.
	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		watchStatus = "normal";
		SaveKixPartsPositions();
		transformationScene.SetActive(false);
		cameraFollowWatch = true;
		CameraIntro();
	}

	private void Update()
	{
		UpdateShinePosition();
		UpdateLightShaftsProgress();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
	}

	private void UpdateShinePosition()
	{
		shineCover.material.SetFloat("_ShineLocation", shinePosition);
	}

	private void UpdateLightShaftsProgress()
	{
		for (int i = 0; i < 2; i++)
		{
			lightShaftsQuads[i].material.SetFloat("_AppearProgress", lightShaftsProgress);
			Color flash = flashScreen.color;
			lightShaftsQuads[i].material.SetColor("_MainColor", new Color(flash.r, flash.g, flash.b, 1f));
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

	// Tween-based timer: fires methods after the given seconds.
	private void WaitSeconds(float seconds, TweenCallback methods)
	{
		waitCounter = 0f;
		DOTween.To(() => waitCounter, x => waitCounter = x, 1f, seconds).OnComplete(methods);
	}

	private void SwitchDiamondsMaterial(Material material)
	{
		for (int i = 0; i < 2; i++)
		{
			diamonds[i].material = material;
			diamonds[i].material.renderQueue = 3002;
		}
	}

	public void ToggleOmniEnhancedAnimation(bool newState)
	{
		omniEnhancedAnimation = newState;
	}

	public void ToggleOmniKixAnimation(bool newState)
	{
		omniKixAnimation = newState;
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
		case "transformed":
			EnableControls(false);
			if (watchGimmick == gimmick.OmniKix && omniKixAnimation)
			{
				OmniKixAnim();
				watchStatus = "kix";
			}
			else if (watchGimmick == gimmick.OmniEnhanced && omniEnhancedAnimation)
			{
				OmniEnhancedAnim();
				watchStatus = "enhanced";
			}
			else
			{
				TimeOutAnim();
				watchStatus = "timeOut";
			}
			break;
		case "enhanced":
		case "kix":
			EnableControls(false);
			TimeOutAnim();
			watchStatus = "timeOut";
			break;
		case "timeOut":
			EnableControls(false);
			ResetWatch();
			watchStatus = "normal";
			break;
		default:
			Debug.Log("Invalid Status");
			break;
		}
	}

	public void ActivationAnim()
	{
		core.SetActive(true);
		mobileParts.DOMoveY(mobileParts.position.y + 0.34f, 0.5f).SetEase(Ease.OutBack);
		Audio.Play("Recharged");
		Audio.Play("PopUp");
		WaitSeconds(0.8f, CameraZoom);
		WaitSeconds(1.2f, ScreenActivationAnim);
	}

	private void CameraZoom()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 0.75f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 0.75f);
	}

	public void ScreenActivationAnim()
	{
		screenAnimator.Play("RebootActivation");
		Audio.Play("DialActivation");
		WaitSeconds(0.2f, DisplayAliens);
	}

	public void DisplayAliens()
	{
		prototypeDisplayAliensController.ShowAlien(0.25f);
		WaitSeconds(0.5f, () => EnableControls(true));
	}

	private void PushAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y - 0.34f, 0.25f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		StartTransformFlash(0.5f);
		Audio.Play("Transform");
		Audio.Play("Push");
	}

	private void StartTransformFlash(float duration)
	{
		FlashScreen(transformFlashColor, 0f, 1f, duration, Ease.InExpo, SwitchToTransformedScene);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
	}

	private void SwitchToTransformedScene()
	{
		screenAnimator.Play("Default");
		transformationScene.SetActive(true);
		prototypeDisplayAliensController.HideAlien(0.25f);
		if (watchStatus != "kix")
		{
			ColorScreenBackground(defaultColor, transformedColor, 0f);
		}
		defaultCameraHolder.position = cameraPositions[3].position;
		defaultCameraHolder.rotation = cameraPositions[3].rotation;
		SwitchDiamondsMaterial(blackMaterial);
		EndTransformFlash(0.5f);
	}

	private void EndTransformFlash(float duration)
	{
		shinePosition = 0f;
		FlashScreen(transformFlashColor, 1f, 0f, duration, Ease.InQuart, () =>
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true)));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
	}

	private void TimeOutAnim()
	{
		Audio.Play("TimeOut");
		StartTimeOutFlash(0.25f);
	}

	private void StartTimeOutFlash(float duration)
	{
		ColorScreenBackground(screenBackground.color, timeOutColor, duration);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = redLightMat;
		}
		FlashScreen(timeOutFlashColor, 0f, 1f, duration, Ease.InExpo, SwitchToNormalScene);
		Audio.Play("Transform");
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
	}

	private void SwitchToNormalScene()
	{
		Camera.main.fieldOfView = 60f;
		RestoreKixPartsPositions();
		screenTrnsScene.DOLocalRotate(Vector3.zero, 0.5f);
		transformationScene.SetActive(false);
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		SwitchDiamondsMaterial(grayMaterial);
		if (transformationSceneBg)
		{
			transformationSceneBg.localPosition = Vector3.zero;
		}
		if (omniEnhancedTransformationBackground)
		{
			omniEnhancedTransformationBackground.SetActive(false);
		}
		EndTimeOutFlash(0.5f);
	}

	private void EndTimeOutFlash(float duration)
	{
		FlashScreen(timeOutFlashColor, 1f, 0f, duration, Ease.InQuart, () => EnableControls(true));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
	}

	private void ResetWatch()
	{
		Audio.Play("Recharged");
		ColorScreenBackground(defaultColor, defaultColor, 0f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = greenLightMat;
		}
		EnableControls(true);
	}

	public void RotateLeft()
	{
		RotateRing(-90f, SwitchAlienLeft);
	}

	// Swapped on purpose in the original: the display list scrolls the other way.
	private void SwitchAlienLeft()
	{
		prototypeDisplayAliensController.SwitchAlienRight();
		prototypeDisplayAliensController.ShowAlien(0.15f);
	}

	public void RotateRight()
	{
		RotateRing(90f, SwitchAlienRight);
	}

	private void SwitchAlienRight()
	{
		prototypeDisplayAliensController.SwitchAlienLeft();
		prototypeDisplayAliensController.ShowAlien(0.15f);
	}

	private void RotateRing(float degrees, TweenCallback switchAlien)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		Audio.Play("Dial");
		screenAnimator.Play("RebootSwitchAlien");
		prototypeDisplayAliensController.HideAlien(0.15f);
		WaitSeconds(0.34f, switchAlien);
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.5f).SetEase(Ease.Flash).OnComplete(() => EnableControls(true));
	}

	private void OmniEnhancedAnim()
	{
		transformationSceneBg.DOLocalMoveY(-1f, 1f);
		Audio.Play("Enhanced");
		omniEnhancedParticles.Play();
		omniEnhancedLight.DOIntensity(100f, 2f).SetEase(Ease.InQuart).OnComplete(StartOmniEnhancedFlash);
	}

	private void StartOmniEnhancedFlash()
	{
		Audio.Play("Transform");
		FlashScreen(omniEnhancedFlashColor, 0f, 1f, 0.5f, Ease.Flash, EndOmniEnhancedFlash);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, 0.5f).SetEase(Ease.InQuad);
	}

	private void EndOmniEnhancedFlash()
	{
		omniEnhancedTransformationBackground.SetActive(true);
		omniEnhancedLight.intensity = 0f;
		FlashScreen(omniEnhancedFlashColor, 1f, 0f, 0.5f, Ease.Flash, () =>
		{
			omniEnhancedParticles.Stop();
			shinePosition = 0f;
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true));
		});
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, 0.5f).SetEase(Ease.OutQuad);
		omniEnhancedLight.DOIntensity(0f, 0.5f).OnComplete(() => EnableControls(true));
	}

	private void SaveKixPartsPositions()
	{
		foreach (Transform part in kixParts)
		{
			kixPartsOgPos.Add(part.position);
		}
	}

	private void RestoreKixPartsPositions()
	{
		for (int i = 0; i < kixParts.Length; i++)
		{
			kixParts[i].position = kixPartsOgPos[i];
		}
		partF[0].localRotation = Quaternion.Euler(40f, 45f, -90f);
		partF[1].localRotation = Quaternion.Euler(0f, 45f, -130f);
		partF[2].localRotation = Quaternion.Euler(0f, 45f, -50f);
		partF[3].localRotation = Quaternion.Euler(-40f, 45f, -90f);
		SetPairActive(partB, false);
		SetPairActive(partC, false);
		SetPairActive(partD, false);
		SetPairActive(partE, false);
	}

	private static void SetPairActive(Transform[] parts, bool active)
	{
		parts[0].gameObject.SetActive(active);
		parts[1].gameObject.SetActive(active);
	}

	// Slides a pair of Omni-Kix parts home, chaining next when the second one lands.
	private static Tween SnapPair(Transform[] parts, float duration, TweenCallback next)
	{
		parts[0].DOLocalMove(Vector3.zero, duration).SetEase(Ease.Flash);
		return parts[1].DOLocalMove(Vector3.zero, duration).SetEase(Ease.Flash).OnComplete(next);
	}

	private void OmniKixAnim()
	{
		screenTrnsScene.DOLocalRotate(new Vector3(0f, -90f, 0f), 0.5f).OnComplete(OmniKixAnim0);
		Audio.Play("ScreenKix");
		Audio.Play("Dial");
	}

	private void OmniKixAnim0()
	{
		Camera.main.DOFieldOfView(80f, 2f);
		transformationSceneBg.DOLocalMoveY(-1.8f, 0.5f).SetEase(Ease.Flash);
		ColorScreenBackground(transformedColor, defaultColor, 0.5f, OmniKixAnimA);
		Audio.Play("Effect3");
	}

	private void OmniKixAnimA()
	{
		part0.DOLocalMove(Vector3.zero, 0.5f).SetEase(Ease.Flash);
		partA[0].DOLocalMove(Vector3.zero, 0.5f);
		partA[1].DOLocalMove(Vector3.zero, 0.5f).OnComplete(OmniKixAnimB);
	}

	private void OmniKixAnimB()
	{
		Audio.Play("MetalEffectA");
		Audio.Play("Effect2");
		SetPairActive(partB, true);
		SnapPair(partB, 0.25f, OmniKixAnimC);
	}

	private void OmniKixAnimC()
	{
		Audio.Play("MetalEffectB");
		SetPairActive(partC, true);
		SnapPair(partC, 0.25f, OmniKixAnimD);
	}

	private void OmniKixAnimD()
	{
		Audio.Play("MetalEffectB");
		SetPairActive(partD, true);
		SnapPair(partD, 0.25f, OmniKixAnimE);
	}

	private void OmniKixAnimE()
	{
		Audio.Play("MetalEffectB");
		SetPairActive(partE, true);
		partE[0].DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.Flash).OnComplete(OmniKixAnimF);
		partE[1].DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.Flash).OnComplete(OmniKixAnimG);
	}

	private void OmniKixAnimF()
	{
		Audio.Play("MetalEffectB");
		for (int i = 0; i < 4; i++)
		{
			partF[i].DOLocalRotate(new Vector3(0f, 45f, -90f), 1f);
		}
	}

	private void OmniKixAnimG()
	{
		Audio.Play("Effect1");
		partG[0].DOLocalMove(Vector3.zero, 1f);
		partG[1].DOLocalMove(Vector3.zero, 1f);
		partG[2].DOLocalMove(Vector3.zero, 1f).OnComplete(() => Audio.Play("Transform2"));
		partG[3].DOLocalMove(Vector3.zero, 1f).OnComplete(() => StartTransformFlash(0.5f));
	}
}
