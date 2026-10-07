using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AntitrixAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	public PrototypeDisplayAliensController prototypeDisplayAliensController;

	[Header("Screen Animation Elements")]
	public SpriteRenderer screenBackground;

	public Transform[] diamonds;

	public Renderer timeOutScreenShape;

	[Header("Watch Parts")]
	public Transform mobileParts;

	public Transform ring;

	public GameObject core;

	[Header("Effects")]
	public Light flashLight;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	public SpriteRenderer shineCover;

	private float shinePosition;

	[Header("Lights")]
	public Light coreLight;

	public float maxFlashLightIntensity = 100f;

	public float defaultFlashLightIntensity = 10f;

	[Header("Extra")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	public Image flashScreen;

	public GameObject transformationScene;

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		watchStatus = "normal";
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
			lightShaftsQuads[i].material.SetColor("_MainColor", screenBackground.color);
		}
	}

	private void CameraIntro()
	{
		SnapCamera(cameraPositions[0]);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 2f).SetEase(Ease.OutQuad).OnComplete(() => EnableControls(true));
	}

	private void SnapCamera(Transform target)
	{
		defaultCameraHolder.position = target.position;
		defaultCameraHolder.rotation = target.rotation;
	}

	private void EnableControls(bool status)
	{
		controlsEnabled = status;
		cameraFollowWatch = false;
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
			StartBlink();
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
		coreLight.DOIntensity(4f, 0.8f).SetEase(Ease.Flash);
		mobileParts.DOMoveY(mobileParts.position.y + 0.34f, 0.8f).SetEase(Ease.Flash);
		Audio.Play("CoreUp");
		StartCoroutine(WaitForActivationAnim());
	}

	private IEnumerator WaitForActivationAnim()
	{
		yield return new WaitForSeconds(1.1f);
		CameraZoom();
		ScreenActivationAnim();
	}

	private void CameraZoom()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 0.75f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 0.75f);
	}

	public void ScreenActivationAnim()
	{
		diamonds[0].DOLocalMoveX(-0.8f, 0.25f).SetEase(Ease.Flash);
		diamonds[1].DOLocalMoveX(0.8f, 0.25f).SetEase(Ease.Flash);
		StartCoroutine(WaitToDisplayAliens());
	}

	private IEnumerator WaitToDisplayAliens()
	{
		yield return new WaitForSeconds(0.05f);
		DisplayAliens();
	}

	public void DisplayAliens()
	{
		prototypeDisplayAliensController.ShowAlien(0.25f);
		Audio.Play("Beep");
		StartCoroutine(WaitToEnableControls(0.2f));
	}

	private IEnumerator WaitToEnableControls(float time)
	{
		yield return new WaitForSeconds(time);
		EnableControls(true);
	}

	private void PushAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y - 0.34f, 0.25f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		StartTransformFlash(0.35f);
		Audio.Play("Transform");
		Audio.Play("Push");
	}

	private void StartTransformFlash(float duration)
	{
		coreLight.DOIntensity(8f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(SwitchToTransformedScene);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InCubic);
	}

	private void SwitchToTransformedScene()
	{
		transformationScene.SetActive(true);
		prototypeDisplayAliensController.HideAlien(0.25f);
		diamonds[0].localPosition = Vector3.zero;
		diamonds[1].localPosition = Vector3.zero;
		SnapCamera(cameraPositions[3]);
		EndTransformFlash(0.5f);
	}

	private void EndTransformFlash(float duration)
	{
		flashLight.DOIntensity(10f, duration);
		coreLight.DOIntensity(1.5f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(() =>
		{
			shinePosition = 0f;
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true));
		});
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
	}

	private void TimeOutAnim()
	{
		StartBlink();
	}

	public void RedBackgroundBlink(float duration, TweenCallback onCompleteMethod)
	{
		Audio.Play("Beep");
		timeOutScreenShape.material.DOFade(0f, 0f);
		timeOutScreenShape.material.DOFade(1f, duration).SetEase(Ease.Flash).OnComplete(onCompleteMethod);
	}

	public void RedLightBlink(float duration)
	{
		flashLight.DOIntensity(20f, 0f);
		coreLight.DOIntensity(8f, 0f);
		flashLight.DOIntensity(10f, duration);
		coreLight.DOIntensity(1.5f, duration).SetEase(Ease.Flash);
	}

	public void StartBlink()
	{
		RedLightBlink(0.5f);
		RedBackgroundBlink(0.5f, Blink1);
	}

	public void Blink1()
	{
		RedLightBlink(0.5f);
		RedBackgroundBlink(0.5f, Blink2);
	}

	public void Blink2()
	{
		RedLightBlink(0.5f);
		RedBackgroundBlink(0.5f, Blink3);
	}

	public void Blink3()
	{
		RedLightBlink(0.5f);
		RedBackgroundBlink(0.5f, () => StartTimeOutFlash(0.35f));
	}

	private void StartTimeOutFlash(float duration)
	{
		flashLight.DOIntensity(100f, duration);
		coreLight.DOIntensity(8f, duration);
		timeOutScreenShape.material.DOFade(0f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(SwitchToNormalScene);
		Audio.Play("Transform");
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InCubic);
	}

	private void SwitchToNormalScene()
	{
		transformationScene.SetActive(false);
		SnapCamera(cameraPositions[1]);
		EndTimeOutFlash(0.5f);
	}

	private void EndTimeOutFlash(float duration)
	{
		flashLight.DOIntensity(0f, duration);
		coreLight.DOIntensity(1.5f, duration);
		timeOutScreenShape.material.DOFade(1f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(() => EnableControls(true));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
	}

	public void RotateLeft()
	{
		RotateRing(-90f, SwitchAlienLeft);
	}

	private void SwitchAlienLeft()
	{
		Audio.Play("Beep");
		prototypeDisplayAliensController.SwitchAlienRight();
		EnableControls(true);
	}

	public void RotateRight()
	{
		RotateRing(90f, SwitchAlienRight);
	}

	private void SwitchAlienRight()
	{
		Audio.Play("Beep");
		prototypeDisplayAliensController.SwitchAlienLeft();
		EnableControls(true);
	}

	private void RotateRing(float degrees, TweenCallback onComplete)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.25f).SetEase(Ease.Flash).OnComplete(onComplete);
	}
}
