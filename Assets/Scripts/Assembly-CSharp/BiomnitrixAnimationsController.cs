using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BiomnitrixAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	[Header("Watch Parts")]
	public Transform[] biomnitrixPieces;

	[Header("Lights")]
	public Light flashLight;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public Renderer[] touchPadsRenderers;

	private float touchPadsIntensity;

	public Renderer watchScreenRenderer;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	private float lightshaftsCoreBlur;

	public MeshRenderer transformationBackground;

	private float backgroundCellSize;

	public SpriteRenderer shineCover;

	private float shinePosition;

	public Animator watchAnimator;

	private Color defaultTouchPadColor;

	[Header("Extra")]
	public GameObject transformationScene;

	public Transform[] biomnitrixPositions0;

	public Transform[] biomnitrixPositions1;

	public Transform[] biomnitrixPositions2;

	public Transform[] biomnitrixPositions3;

	public Transform[] biomnitrixPositions4;

	public Transform[] biomnitrixPositions5;

	public bool transformAnimation;

	private string watchStatus;

	private bool controlsEnabled;

	private bool cameraFollowWatch;

	private float waitCounter;

	private Color defaultColor;

	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		watchStatus = "normal";
		transformationScene.SetActive(false);
		cameraFollowWatch = true;
		touchPadsIntensity = 0.45f;
		defaultTouchPadColor = touchPadsRenderers[0].material.color;
		CameraIntro();
	}

	private void Update()
	{
		UpdateTouchPadsEmissionIntensity();
		UpdateBackgroundCellSize();
		UpdateShinePosition();
		UpdateLightShafts();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
	}

	private void UpdateTouchPadsEmissionIntensity()
	{
		foreach (Renderer touchPad in touchPadsRenderers)
		{
			touchPad.material.SetColor("_EmissionColor", defaultTouchPadColor * touchPadsIntensity);
			touchPad.material.EnableKeyword("_EMISSION");
		}
	}

	private void UpdateBackgroundCellSize()
	{
		transformationBackground.material.SetFloat("_CellSizeControl", backgroundCellSize);
	}

	private void UpdateShinePosition()
	{
		shineCover.material.SetFloat("_ShineLocation", shinePosition);
	}

	private void UpdateLightShafts()
	{
		foreach (MeshRenderer quad in lightShaftsQuads)
		{
			quad.material.SetFloat("_AppearProgress", lightShaftsProgress);
			quad.material.SetFloat("_CoreBlur", lightshaftsCoreBlur);
		}
	}

	private void CameraIntro()
	{
		defaultCameraHolder.position = cameraPositions[0].position;
		defaultCameraHolder.rotation = cameraPositions[0].rotation;
		defaultCameraHolder.DOMove(cameraPositions[1].position, 1.5f).SetEase(Ease.OutQuad).OnComplete(() => EnableControls(true));
	}

	public void EnableControls(bool status)
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
		if (watchStatus == "normal")
		{
			EnableControls(false);
			ActivationAnimA();
			watchStatus = "transformed";
		}
		else if (watchStatus == "transformed")
		{
			EnableControls(false);
			TimeOutAnim();
			watchStatus = "normal";
		}
		else
		{
			Debug.Log("Invalid Status");
		}
	}

	// A tween used as a timer.
	private void WaitSeconds(float seconds, TweenCallback methods)
	{
		waitCounter = 0f;
		DOTween.To(() => waitCounter, x => waitCounter = x, 1f, seconds).OnComplete(methods);
	}

	private void AnimateCamera(Transform target, float duration)
	{
		defaultCameraHolder.DOMove(target.position, duration);
		defaultCameraHolder.DORotate(target.rotation.eulerAngles, duration);
	}

	private void AnimateBiomnitrix(Transform[] target, float duration, Ease ease = Ease.Unset, TweenCallback onCompleteMethod = null)
	{
		for (int i = 0; i < biomnitrixPieces.Length; i++)
		{
			biomnitrixPieces[i].DOMove(target[i].position, duration).SetEase(ease);
			biomnitrixPieces[i].DORotate(target[i].rotation.eulerAngles, duration).SetEase(ease).OnComplete(onCompleteMethod);
		}
	}

	private void StartFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(100f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(onCompleteMethod);
		lightShaftsProgress = 0f;
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
		lightshaftsCoreBlur = 0.1f;
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.213f, duration).SetEase(Ease.InQuint);
	}

	private void EndFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(10f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(onCompleteMethod);
		lightShaftsProgress = 0f;
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
		lightshaftsCoreBlur = 0.1f;
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.1f, duration).SetEase(Ease.OutQuint);
	}

	private void ActivationAnimA()
	{
		Audio.Play("Activation");
		AnimateCamera(cameraPositions[2], 0.5f);
		AnimateBiomnitrix(biomnitrixPositions1, 0.5f, Ease.InOutBack);
		WaitSeconds(0.7f, ActivationAnimB);
	}

	private void ActivationAnimB()
	{
		AnimateBiomnitrix(biomnitrixPositions2, 0.5f);
		DOTween.To(() => touchPadsIntensity, x => touchPadsIntensity = x, 0.8f, 0.49f);
		if (!transformAnimation)
		{
			StartFlash(0.5f, SwitchToTransformedScene);
			Audio.Play("Transform");
			return;
		}
		Audio.Play("TransformationTheme");
		StartFlash(0.5f, TransformingSequenceA);
		menuController.ShowLetterboxing(true);
	}

	private void TransformingSequenceA()
	{
		EndFlash(1f, null);
		transformationBackground.gameObject.SetActive(true);
		backgroundCellSize = 1f;
		DOTween.To(() => backgroundCellSize, x => backgroundCellSize = x, 0.2f, 2f);
		for (int i = 0; i < biomnitrixPieces.Length; i++)
		{
			biomnitrixPieces[i].position = biomnitrixPositions3[i].position;
			biomnitrixPieces[i].rotation = biomnitrixPositions3[i].rotation;
		}
		AnimateBiomnitrix(biomnitrixPositions4, 4.3f, Ease.InOutQuad, TransformingSequenceB);
	}

	private void TransformingSequenceB()
	{
		AnimateBiomnitrix(biomnitrixPositions5, 0.25f, Ease.InQuad);
		StartFlash(0.5f, SwitchToTransformedScene);
	}

	private void SwitchToTransformedScene()
	{
		menuController.ShowLetterboxing(false);
		transformationBackground.gameObject.SetActive(false);
		transformationScene.SetActive(true);
		defaultCameraHolder.position = cameraPositions[3].position;
		defaultCameraHolder.rotation = cameraPositions[3].rotation;
		AnimateBiomnitrix(biomnitrixPositions0, 0.25f);
		touchPadsIntensity = 0.45f;
		shinePosition = 0f;
		EndFlash(0.5f, () =>
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true)));
	}

	private void TimeOutAnim()
	{
		Audio.Play("TimeOut");
		StartBlink();
	}

	private void Blink(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.intensity = 20f;
		flashLight.DOIntensity(10f, duration);
		defaultColor = watchScreenRenderer.material.color;
		watchScreenRenderer.material.color = Color.white;
		watchScreenRenderer.material.DOColor(defaultColor, duration).OnComplete(onCompleteMethod);
	}

	public void StartBlink()
	{
		Blink(0.45f, Blink1);
	}

	public void Blink1()
	{
		Blink(0.45f, Blink2);
	}

	public void Blink2()
	{
		Blink(0.5f, () => StartFlash(0.2f, SwitchToNormalScene));
	}

	private void SwitchToNormalScene()
	{
		transformationScene.SetActive(false);
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		EndFlash(0.5f, () => EnableControls(true));
	}
}
