using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CompleteAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private CompleteDisplayAliensController completedDisplayAliensController;

	[Header("Screen Animation Elements")]
	public Transform[] diamonds;

	public CanvasGroup[] screenItemsGroups;

	public RectTransform sprite4;

	public Transform screenCamera;

	private Quaternion originalRotationSprite4;

	public GameObject spriteScan;

	public RectTransform scanTarget;

	private Color32 originalSpriteScanColor;

	[Header("Watch Parts")]
	public Transform touchPad;

	public GameObject core;

	[Header("Lights")]
	public Light flashLight;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public Renderer touchPadRenderer;

	private float touchPadIntensity;

	public Renderer watchScreenRenderer;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	private float lightshaftsCoreBlur;

	public MeshRenderer transformationBackground;

	private float backgroundCellSize;

	public SpriteRenderer shineCover;

	private float shinePosition;

	[Header("Extra")]
	public GameObject transformationScene;

	public string secretCode;

	public bool transformAnimation;

	private string watchStatus;

	private bool controlsEnabled;

	private bool cameraFollowWatch;

	private Color defaultColor;

	public int rotations;

	public float startRotationDuration = 1.3f;

	public float scanRotationDuration = 0.7f;

	public float endrotationDuration = 1.3f;

	private Quaternion originalScreenCameraRotation;

	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		originalSpriteScanColor = spriteScan.GetComponent<Image>().color;
		originalScreenCameraRotation = screenCamera.rotation;
		originalRotationSprite4 = sprite4.rotation;
		completedDisplayAliensController = GetComponent<CompleteDisplayAliensController>();
		watchStatus = "normal";
		transformationScene.SetActive(false);
		touchPadIntensity = 0f;
		cameraFollowWatch = true;
		backgroundCellSize = 1f;
		lightshaftsCoreBlur = 0.1f;
		CameraIntro();
	}

	private void Update()
	{
		UpdateTouchPadEmissionIntensity();
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

	private void UpdateTouchPadEmissionIntensity()
	{
		float i = touchPadIntensity;
		Material material = touchPadRenderer.materials[1];
		material.SetColor("_EmissionColor", new Color(i, i, i, i));
		touchPadRenderer.materials[1].EnableKeyword("_EMISSION");
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
		lightShaftsQuads[0].material.SetFloat("_AppearProgress", lightShaftsProgress);
		lightShaftsQuads[1].material.SetFloat("_AppearProgress", lightShaftsProgress);
		lightShaftsQuads[0].material.SetFloat("_CoreBlur", lightshaftsCoreBlur);
		lightShaftsQuads[1].material.SetFloat("_CoreBlur", lightshaftsCoreBlur);
	}

	private void CameraIntro()
	{
		SnapCamera(cameraPositions[0]);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 1.5f).SetEase(Ease.OutQuad).OnComplete(() => EnableControls(true));
	}

	private void SnapCamera(Transform target)
	{
		defaultCameraHolder.position = target.position;
		defaultCameraHolder.rotation = target.rotation;
	}

	private Tween MoveCamera(Transform target, float duration)
	{
		defaultCameraHolder.DOMove(target.position, duration);
		return defaultCameraHolder.DORotate(target.rotation.eulerAngles, duration);
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
		switch (watchStatus)
		{
		case "normal":
			EnableControls(false);
			CameraZoomA();
			watchStatus = "activated";
			break;
		case "activated":
			EnableControls(false);
			core.SetActive(true);
			TouchpadAnimA();
			completedDisplayAliensController.HideInterface();
			watchStatus = "activated2";
			break;
		case "activated2":
			EnableControls(false);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transformed":
			EnableControls(false);
			TimeOutAnim();
			watchStatus = "normal";
			break;
		default:
			Debug.Log("Invalid Status");
			break;
		}
	}

	private void CameraZoomA()
	{
		MoveCamera(cameraPositions[2], 0.5f).OnComplete(Activation);
	}

	private void Activation()
	{
		completedDisplayAliensController.DisplayAliens(true);
	}

	private void TouchpadAnimA()
	{
		CameraZoomB();
		touchPad.DOLocalMoveY(0.11f, 0.2f).OnComplete(TouchpadAnimB);
		DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 0f, 0.5f);
		Audio.Play("CoreUp");
	}

	private void CameraZoomB()
	{
		MoveCamera(cameraPositions[3], 0.75f);
	}

	private void TouchpadAnimB()
	{
		touchPad.DOLocalMoveX(-0.565f, 0.2f);
		touchPad.DOLocalMoveY(0.04f, 0.2f);
		touchPad.DORotate(new Vector3(0f, 0f, 16.3f), 0.2f).SetEase(Ease.InQuad).OnComplete(CoreUpAnim);
	}

	private void CoreUpAnim()
	{
		core.transform.DOMoveY(core.transform.position.y + 0.2f, 0.25f).OnComplete(() => EnableControls(true));
		Audio.Play("Waiting");
	}

	private void PushAnim()
	{
		core.transform.DOMoveY(core.transform.position.y - 0.327f, 0.25f).OnComplete(() => core.SetActive(true));
		Audio.Stop("Waiting");
		if (!transformAnimation)
		{
			StartFlash(0.5f, SwitchToTransformedScene);
			Audio.Play("Transform");
			return;
		}
		Audio.Play("TransformationTheme");
		StartFlash(0.5f, TransformingSequence);
		menuController.ShowLetterboxing(true);
	}

	private void ResetTouchPadAndCore()
	{
		touchPad.localRotation = Quaternion.Euler(Vector3.zero);
		touchPad.localPosition = Vector3.zero;
		core.transform.localPosition = Vector3.zero;
		core.SetActive(false);
	}

	private void TransformingSequence()
	{
		EndFlash(1f, null);
		ResetTouchPadAndCore();
		transformationBackground.gameObject.SetActive(true);
		DOTween.To(() => backgroundCellSize, x => backgroundCellSize = x, 0.2f, 2f);
		SnapCamera(cameraPositions[3]);
		defaultCameraHolder.DOMove(cameraPositions[4].position, 3f).SetEase(Ease.InOutBack);
		defaultCameraHolder.DORotate(cameraPositions[4].rotation.eulerAngles, 3f).OnComplete(TransformingSequenceA);
	}

	private void TransformingSequenceA()
	{
		DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 1.5f, 1.5f).SetEase(Ease.InQuad);
		StartCoroutine(TransformingSequenceB());
	}

	private IEnumerator TransformingSequenceB()
	{
		yield return new WaitForSeconds(1.25f);
		StartFlash(0.5f, null);
		Camera.main.DOFieldOfView(30f, 0.5f).SetEase(Ease.InCubic).OnComplete(SwitchToTransformedScene);
	}

	private void SwitchToTransformedScene()
	{
		menuController.ShowLetterboxing(false);
		ResetTouchPadAndCore();
		Camera.main.fieldOfView = 60f;
		backgroundCellSize = 1f;
		transformationScene.SetActive(true);
		transformationBackground.gameObject.SetActive(false);
		touchPadIntensity = 0f;
		SnapCamera(cameraPositions[5]);
		shinePosition = 0f;
		EndFlash(0.5f, () =>
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true)));
	}

	private void StartFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(100f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(onCompleteMethod);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.213f, duration).SetEase(Ease.InQuint);
	}

	private void EndFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(10f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration + 0.2f).OnComplete(onCompleteMethod);
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.1f, duration + 0.2f);
	}

	private void TimeOutAnim()
	{
		Audio.Play("TimeOut");
		secretCode = "";
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
		SnapCamera(cameraPositions[1]);
		EndFlash(0.5f, () => EnableControls(true));
	}

	public void RotateLeft()
	{
		Rotate(true);
	}

	public void RotateRight()
	{
		Rotate(false);
	}

	private void Rotate(bool left)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		if (left)
		{
			completedDisplayAliensController.SwitchAlienLeft();
			UpdateSecretCode("L");
		}
		else
		{
			completedDisplayAliensController.SwitchAlienRight();
			UpdateSecretCode("R");
		}
	}

	private void UpdateSecretCode(string newDirection)
	{
		if (secretCode.Length > 5)
		{
			secretCode = secretCode.Remove(0, 1);
		}
		secretCode += newDirection;
	}

	public void CheckForSecretCode()
	{
		if (secretCode == "RLRLRR")
		{
			CommandAcceptedAnimation(RandomizerA);
		}
		else if (secretCode == "LRLRLL")
		{
			CommandAcceptedAnimation(ScanA);
		}
		else
		{
			EnableControls(true);
			return;
		}
		secretCode = "";
	}

	private void CommandAcceptedAnimation(TweenCallback onCompleteMethod)
	{
		Audio.Play("CodeAccepted");
		completedDisplayAliensController.HideInterface();
		DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 1f, 1f);
		defaultCameraHolder.DOMove(cameraPositions[6].position, 1f).OnComplete(onCompleteMethod);
	}

	private void RandomizerA()
	{
		Audio.Play("Beep5");
		DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 0.75f, 0.44f).SetEase(Ease.Flash);
		diamonds[0].DOLocalMoveX(1.3f, 0.44f).SetEase(Ease.Flash);
		diamonds[1].DOLocalMoveX(-1.3f, 0.44f).SetEase(Ease.Flash).OnComplete(() => StartCoroutine(RandomizerB()));
	}

	private IEnumerator RandomizerB()
	{
		yield return new WaitForSeconds(0.3f);
		Audio.Play("RandomizerActivated");
		screenItemsGroups[1].DOFade(1f, 0.15f);
		Audio.Play("Beep1");
		yield return new WaitForSeconds(1f);
		screenItemsGroups[1].DOFade(0f, 0.15f);
		screenItemsGroups[2].DOFade(1f, 0.15f);
		completedDisplayAliensController.DisplayAliens(false);
		MoveCamera(cameraPositions[7], 0.25f);
		yield return new WaitForSeconds(0.25f);
		sprite4.DOLocalRotate(sprite4.localEulerAngles + new Vector3(0f, 0f, 360f), startRotationDuration, RotateMode.FastBeyond360).SetEase(Ease.InQuad).OnComplete(RandomizerC);
		completedDisplayAliensController.RotateRandomA();
		yield return new WaitForSeconds(0.42f);
		Audio.Play("StartRandom");
		yield return new WaitForSeconds(1.11f);
		Audio.Play("Randomizing");
	}

	private void RandomizerC()
	{
		if (rotations == 7)
		{
			Audio.Play("Beep4");
		}
		if (rotations < 8)
		{
			rotations++;
			sprite4.DOLocalRotate(sprite4.localEulerAngles + new Vector3(0f, 0f, 360f), scanRotationDuration, RotateMode.FastBeyond360).SetEase(Ease.Flash).OnComplete(RandomizerC);
			return;
		}
		sprite4.rotation = originalRotationSprite4;
		Audio.Stop("Randomizing");
		RandomizerD();
	}

	private void RandomizerD()
	{
		Audio.Play("TransformationTheme");
		StartFlash(0.5f, () =>
		{
			diamonds[0].localPosition = new Vector3(-0.179f, 1f, 0f);
			diamonds[1].localPosition = new Vector3(0.179f, 1f, 0f);
			screenItemsGroups[2].alpha = 0f;
			touchPadIntensity = 0f;
			TransformingSequence();
		});
		watchStatus = "transformed";
		rotations = 0;
	}

	private void ScanA()
	{
		if (rotations == 0)
		{
			Audio.Play("Beep4");
			Audio.Play("Beep9");
		}
		if (rotations == 1)
		{
			Audio.Play("NewLifeFormIdentified");
		}
		if (rotations < 4)
		{
			rotations++;
			screenCamera.DORotate(screenCamera.localEulerAngles + new Vector3(0f, 0f, 90f), 0.5f).OnComplete(ScanA).SetEase(Ease.Flash);
			return;
		}
		rotations = 0;
		screenCamera.rotation = originalScreenCameraRotation;
		StartCoroutine(WaitToStopRotation());
	}

	private IEnumerator WaitToStopRotation()
	{
		yield return new WaitForSeconds(0.35f);
		Audio.Play("Beep5");
		diamonds[0].DOLocalMoveX(-1f, 0.35f).SetEase(Ease.Flash);
		diamonds[1].DOLocalMoveX(1f, 0.35f).SetEase(Ease.Flash).OnComplete(() => screenItemsGroups[0].DOFade(1f, 0.15f).OnComplete(ScanB));
		DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 0.75f, 0.35f).SetEase(Ease.Flash);
	}

	private void ScanB()
	{
		RectTransform scanRect = spriteScan.GetComponent<RectTransform>();
		Image scanImage = spriteScan.GetComponent<Image>();
		if (rotations == 0)
		{
			scanTarget.DOLocalMoveY(0f, 5f);
			Audio.Play("DetectingDna");
		}
		if (rotations < 5)
		{
			rotations++;
			scanRect.localScale = Vector3.zero;
			Color color = originalSpriteScanColor;
			color.a = 0.2941177f;
			scanImage.color = color;
			Audio.Play("Beep1");
			scanRect.DOScale(1.5f, 1f);
			scanImage.DOFade(0f, 1f).SetEase(Ease.InExpo).OnComplete(ScanB);
			return;
		}
		rotations = 0;
		scanRect.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		Audio.Play("Beep6");
		scanImage.DOFade(0.85f, 0.5f).OnComplete(() => scanImage.DOFade(0f, 0.5f));
		Audio.Play("NewLifeFormAnalized");
		StartCoroutine(ScanC());
	}

	private IEnumerator ScanC()
	{
		yield return new WaitForSeconds(2.2f);
		Audio.Play("AcquisitionCompleted");
		screenItemsGroups[0].DOFade(0f, 0.25f).OnComplete(() => scanTarget.DOLocalMoveY(-1.5f, 1f));
		diamonds[0].DOLocalMoveX(-0.179f, 0.5f).SetEase(Ease.Flash);
		diamonds[1].DOLocalMoveX(0.179f, 0.5f).SetEase(Ease.Flash).OnComplete(() => DOTween.To(() => touchPadIntensity, x => touchPadIntensity = x, 0f, 1f));
		Audio.Play("Beep10");
		yield return new WaitForSeconds(1.25f);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 1f);
		defaultCameraHolder.DORotate(cameraPositions[1].rotation.eulerAngles, 1f).SetEase(Ease.OutQuint).OnComplete(() => EnableControls(true));
		watchStatus = "normal";
	}
}
