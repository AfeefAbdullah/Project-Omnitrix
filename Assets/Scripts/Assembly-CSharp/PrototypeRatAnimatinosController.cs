using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeRatAnimatinosController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private PrototypeRatDisplayAliensController prototypeRatDisplayAliensController;

	[Header("Screen Animation Elements")]
	public Animator screenAnimator;

	public GameObject defaultScreenBackground;

	public SpriteRenderer screenBackgroundTransformed;

	public Renderer screenRenderer;

	public float defaultScreenIntensity = 0.6f;

	public float currentScreenIntensity;

	public MeshRenderer[] diamondsMeshes;

	public SpriteRenderer[] greenBackgroundLayers;

	public SpriteRenderer[] eonBackgroundLayers;

	public SpriteRenderer[] backgroundLayers0;

	public SpriteRenderer[] backgroundLayers1;

	public SpriteRenderer backgroundLayer2;

	private float layer0CellSize;

	private float layer1CellSize;

	private Color layer2Color;

	[Header("Watch Parts")]
	public Transform mobileParts;

	public Transform ring;

	public GameObject[] shinyParts;

	public Renderer purpleShinyPart;

	public GameObject core;

	public Transform shinyPartsMask;

	public MeshRenderer coreCylinder;

	[Header("Lights")]
	public Color[] lightColors;

	public Light[] flashLights;

	public float maxFlashLightIntensity = 100f;

	public float defaultFlashLightIntensity = 10f;

	[Header("Materials")]
	public Material greenLightMat;

	public Material redLightMat;

	public Material purpleLightMat;

	public Material grayMat;

	public Material whiteMat;

	public Material screenGreenParticles;

	public Material screenPurpleParticles;

	[Header("Camera")]
	public Camera mainCamera;

	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	public Transform[] transformingPositions;

	[Header("Effects")]
	public Image flashScreen;

	public GameObject[] transformingScene;

	public ParticleSystem[] transformingBackgroundParticles;

	public ParticleSystem[] transformingIntroParticles;

	public ParticleSystem[] transformingParticles;

	public ParticleSystem[] transformingEndParticles;

	[Header("Modes Colors")]
	public Color defaultColor;

	public Color transformedColor;

	public Color timeOutColor;

	public Color eonColor;

	public Color disabledColor;

	[Header("Flash Colors")]
	public Color transformFlashColor;

	public Color timeOutFlashColor;

	[Header("Extra")]
	public GameObject transformationScene;

	public float mobilePartsUpHeight = 0.34f;

	public bool transformAnimation;

	private bool controlsEnabled;

	private string watchStatus;

	public bool cameraFollowWatch;

	private bool eonMode;

	private float waitCounter;

	private int timeOutBlinks;

	// The original looks the AudioManager up on every call.
	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	// Eon mode swaps the green flash colour for the eon colour.
	private Color FlashColor => eonMode ? eonColor : transformFlashColor;

	private int EonIndex => eonMode ? 1 : 0;

	private void Start()
	{
		prototypeRatDisplayAliensController = GetComponent<PrototypeRatDisplayAliensController>();
		watchStatus = "normal";
		currentScreenIntensity = defaultScreenIntensity;
		transformationScene.SetActive(false);
		cameraFollowWatch = true;
		purpleShinyPart.material.DOFade(0f, 0f);
		foreach (GameObject part in shinyParts)
		{
			part.SetActive(false);
		}
		layer0CellSize = 1.2f;
		layer1CellSize = 0f;
		layer2Color = defaultColor;
		CameraIntro();
	}

	private void Update()
	{
		UpdateScreenLayers();
		UpdateScreenEmissionIntensity();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
	}

	private void UpdateScreenLayers()
	{
		foreach (SpriteRenderer layer in backgroundLayers0)
		{
			layer.material.SetFloat("_CellSize", layer0CellSize);
		}
		foreach (SpriteRenderer layer in backgroundLayers1)
		{
			layer.material.SetFloat("_CellSize", layer1CellSize);
		}
		backgroundLayer2.material.SetColor("_ReflectionColor", layer2Color);
	}

	private void UpdateScreenEmissionIntensity()
	{
		float i = currentScreenIntensity;
		screenRenderer.material.SetColor("_EmissionColor", new Color(i, i, i, i));
		screenRenderer.material.EnableKeyword("_EMISSION");
	}

	private void IntensityFlashLights(Color color, float startValue, float endValue, float duration)
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

	// Tween-based timer: fires methods after the given seconds.
	private void WaitSeconds(float seconds, TweenCallback methods)
	{
		waitCounter = 0f;
		DOTween.To(() => waitCounter, x => waitCounter = x, 1f, seconds).OnComplete(methods);
	}

	private void MoveCamera(Transform target, float duration, Ease ease = Ease.Unset, TweenCallback onCompleteMethod = null)
	{
		defaultCameraHolder.DOMove(target.position, duration).SetEase(ease).OnComplete(onCompleteMethod);
	}

	private void RotateCamera(Transform target, float duration, Ease ease = Ease.Unset)
	{
		defaultCameraHolder.DORotate(target.rotation.eulerAngles, duration).SetEase(ease);
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
			ScreenActivationAnim();
			watchStatus = "activated";
			break;
		case "activated":
			EnableControls(false);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transformed":
			// The time-out sequence ends back on the normal screen.
			EnableControls(false);
			TimeOutAnim();
			watchStatus = "normal";
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
		IntensityFlashLights(defaultColor, 0f, 10f, 0.2f);
		ModifyScreenEmissiveness(currentScreenIntensity, 1f, 0.5f);
		mobileParts.DOMoveY(mobileParts.position.y + mobilePartsUpHeight, 0.5f).SetEase(Ease.OutBack);
		shinyParts[0].SetActive(true);
		shinyPartsMask.DOMoveY(-3f, 0.5f).SetEase(Ease.Flash).OnComplete(() => shinyPartsMask.gameObject.SetActive(false));
		Audio.Play("Activated");
	}

	public void ScreenActivationAnim()
	{
		if (prototypeRatDisplayAliensController.currentAlienIndex == 10)
		{
			EnableEonMode();
		}
		screenAnimator.Play("Activation");
		DOTween.To(() => layer0CellSize, x => layer0CellSize = x, 0.85f, 1f);
		DOTween.To(() => layer1CellSize, x => layer1CellSize = x, 0.5f, 1f);
		WaitSeconds(0.5f, DisplayAliens);
	}

	public void DisplayAliens()
	{
		prototypeRatDisplayAliensController.ShowAlien();
		StartCoroutine(WaitToCameraZoom(0.5f));
	}

	private IEnumerator WaitToCameraZoom(float time)
	{
		yield return new WaitForSeconds(time);
		CameraZoom();
	}

	private void CameraZoom()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 1f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 1f).OnComplete(() => EnableControls(true));
	}

	private void PushAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y - mobilePartsUpHeight, 0.25f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		Audio.Play("Push");
		if (!transformAnimation)
		{
			Audio.Play("Transform");
			StartTransformFlash(SwitchToTransformedScene);
			return;
		}
		Audio.Play("TransformationTheme");
		StartTransformFlash(TransformingSequence);
		WaitSeconds(6f, () => MoveCamera(transformingPositions[2], 2.5f, Ease.InQuart, TransformingSequenceA));
		WaitSeconds(13.5f, () => FlashScreen(FlashColor, 0f, 1f, 0.25f, Ease.Unset, TransformingSequenceC));
		WaitSeconds(15f, () =>
		{
			cameraFollowWatch = false;
			MoveCamera(transformingPositions[5], 2.1f, Ease.InOutSine, TransformingSequenceD);
			RotateCamera(transformingPositions[5], 2.1f, Ease.InOutSine);
		});
		WaitSeconds(15.5f, () => transformingEndParticles[EonIndex].Play());
	}

	private void StartTransformFlash(TweenCallback onCompleteMethod)
	{
		mainCamera.DOFieldOfView(20f, 0.75f).SetEase(Ease.InSine);
		IntensityFlashLights(eonMode ? eonColor : defaultColor, 10f, 100f, 0.75f);
		FlashScreen(FlashColor, 0f, 1f, 0.75f, Ease.InExpo, onCompleteMethod);
	}

	private void TransformingSequence()
	{
		mainCamera.fieldOfView = 60f;
		EndTransformFlash(null);
		int i = EonIndex;
		transformingScene[i].SetActive(true);
		transformingBackgroundParticles[i].Play();
		transformingIntroParticles[i].Play();
		WaitSeconds(1.25f, () => transformingParticles[i].Play());
		cameraFollowWatch = true;
		defaultCameraHolder.position = transformingPositions[0].position;
		MoveCamera(transformingPositions[1], 6.25f, Ease.OutCubic);
	}

	private void TransformingSequenceA()
	{
		FlashScreen(FlashColor, 1f, 0f, 0.25f, Ease.InQuart);
		defaultCameraHolder.position = transformingPositions[3].position;
		MoveCamera(transformingPositions[4], 7f);
		WaitSeconds(2f, () => transformingParticles[EonIndex].Stop());
		WaitSeconds(3f, TransformingSequenceB);
	}

	private void TransformingSequenceB()
	{
		screenAnimator.SetFloat("timeScale", 0.25f);
		screenAnimator.Play("ReverseActivation");
		prototypeRatDisplayAliensController.HideAlien(1f);
	}

	private void TransformingSequenceC()
	{
		ShowTransformedScreen();
		FlashScreen(FlashColor, 1f, 0f, 0.25f, Ease.InQuart);
	}

	private void ShowTransformedScreen()
	{
		defaultScreenBackground.SetActive(false);
		screenBackgroundTransformed.gameObject.SetActive(true);
		currentScreenIntensity = defaultScreenIntensity;
		SetDiamondsColor(new Color(0.5f, 0.5f, 0.5f, 1f));
	}

	private void SetDiamondsColor(Color color)
	{
		diamondsMeshes[0].material.SetColor("_Color", color);
		diamondsMeshes[1].material.SetColor("_Color", color);
	}

	private void TransformingSequenceD()
	{
		mainCamera.DOFieldOfView(20f, 0.25f).SetEase(Ease.InCubic);
		IntensityFlashLights(eonMode ? eonColor : defaultColor, 10f, 100f, 0.25f);
		FlashScreen(FlashColor, 0f, 1f, 0.25f, Ease.InExpo, SwitchToTransformedScene);
	}

	private void SwitchToTransformedScene()
	{
		mainCamera.fieldOfView = 60f;
		screenAnimator.SetFloat("timeScale", 1f);
		transformationScene.SetActive(true);
		transformingScene[EonIndex].SetActive(false);
		transformingEndParticles[EonIndex].Stop();
		screenAnimator.Play("Default");
		prototypeRatDisplayAliensController.HideAlien(0.2f);
		ShowTransformedScreen();
		defaultCameraHolder.position = cameraPositions[4].position;
		defaultCameraHolder.rotation = cameraPositions[4].rotation;
		shinyPartsMask.gameObject.SetActive(true);
		shinyPartsMask.position = Vector3.zero;
		foreach (GameObject part in shinyParts)
		{
			part.SetActive(false);
		}
		EndTransformFlash(() => EnableControls(true));
	}

	private void EndTransformFlash(TweenCallback onCompleteMethod)
	{
		IntensityFlashLights(transformedColor, 100f, 10f, 0.5f);
		FlashScreen(FlashColor, 1f, 0f, 0.5f, Ease.InQuart, onCompleteMethod);
	}

	private void TimeOutAnim()
	{
		Audio.Play("TimeOut");
		TimeOutBlink();
	}

	private void TimeOutBlink()
	{
		timeOutBlinks++;
		bool more = timeOutBlinks < 6;
		screenBackgroundTransformed.color = transformedColor;
		screenBackgroundTransformed.DOColor(timeOutColor, 0.15f).OnComplete(more ? TimeOutBlink : (TweenCallback)StartTimeOutFlash);
		IntensityFlashLights(timeOutColor, 20f, 10f, more ? 0.5f : 1f);
	}

	private void StartTimeOutFlash()
	{
		Audio.Play("Transform2");
		IntensityFlashLights(timeOutColor, 10f, 100f, 0.5f);
		screenBackgroundTransformed.color = transformedColor;
		screenBackgroundTransformed.DOColor(timeOutColor, 0.5f);
		FlashScreen(timeOutFlashColor, 0f, 1f, 0.5f, Ease.InExpo, SwitchToNormalScene);
	}

	private void SwitchToNormalScene()
	{
		prototypeRatDisplayAliensController.ShowAlien();
		screenAnimator.Play("Activated");
		ResetWatch();
		transformationScene.SetActive(false);
		screenBackgroundTransformed.gameObject.SetActive(false);
		defaultScreenBackground.SetActive(true);
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		EndTimeOutFlash();
		if (prototypeRatDisplayAliensController.currentAlienIndex == 10)
		{
			DisableEonMode();
		}
	}

	private void EndTimeOutFlash()
	{
		IntensityFlashLights(timeOutColor, 100f, 0f, 0.5f);
		FlashScreen(timeOutFlashColor, 1f, 0f, 0.8f, Ease.OutQuart, InverseScreenActivationAnim);
	}

	private void InverseScreenActivationAnim()
	{
		prototypeRatDisplayAliensController.HideAlien(0.2f);
		DOTween.To(() => layer0CellSize, x => layer0CellSize = x, 1.2f, 0.6f);
		DOTween.To(() => layer1CellSize, x => layer1CellSize = x, 0f, 0.6f);
		screenAnimator.Play("ReverseActivation");
		WaitSeconds(0.6f, () => EnableControls(true));
	}

	private void ResetWatch()
	{
		SetDiamondsColor(new Color(1f, 1f, 1f, 1f));
		screenBackgroundTransformed.color = transformedColor;
		timeOutBlinks = 0;
	}

	public void RotateLeft()
	{
		RotateRing(-90f);
	}

	public void RotateRight()
	{
		RotateRing(90f);
	}

	// Swapped on purpose in the original: rotating left scrolls the display right.
	private void RotateRing(float degrees)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.5f).SetEase(Ease.Flash).OnComplete(() => EnableControls(true));
		if (degrees < 0f)
		{
			prototypeRatDisplayAliensController.SwitchAlienRight();
		}
		else
		{
			prototypeRatDisplayAliensController.SwitchAlienLeft();
		}
		Audio.Play("Dial");
	}

	public void EnableEonMode()
	{
		eonMode = true;
		shinyParts[1].SetActive(true);
		coreCylinder.material = purpleLightMat;
		SwapBackgroundLayers(greenBackgroundLayers, eonBackgroundLayers);
		DOTween.To(() => layer2Color, x => layer2Color = x, eonColor, 0.5f);
		TintLights(eonColor);
		purpleShinyPart.material.DOFade(1f, 0.5f).OnComplete(() => shinyParts[0].SetActive(false));
	}

	public void DisableEonMode()
	{
		eonMode = false;
		shinyParts[0].SetActive(true);
		coreCylinder.material = greenLightMat;
		SwapBackgroundLayers(eonBackgroundLayers, greenBackgroundLayers);
		DOTween.To(() => layer2Color, x => layer2Color = x, defaultColor, 0.5f);
		TintLights(defaultColor);
		purpleShinyPart.material.DOFade(0f, 0.5f).OnComplete(() => shinyParts[1].SetActive(false));
	}

	private static void SwapBackgroundLayers(SpriteRenderer[] fadeOut, SpriteRenderer[] fadeIn)
	{
		foreach (SpriteRenderer layer in fadeOut)
		{
			layer.DOFade(0f, 0.5f).OnComplete(() => layer.gameObject.SetActive(false));
		}
		foreach (SpriteRenderer layer in fadeIn)
		{
			layer.gameObject.SetActive(true);
			layer.DOFade(1f, 0.5f);
		}
	}

	private void TintLights(Color color)
	{
		foreach (Light flashLight in flashLights)
		{
			flashLight.DOColor(color, 0.5f);
		}
	}
}
