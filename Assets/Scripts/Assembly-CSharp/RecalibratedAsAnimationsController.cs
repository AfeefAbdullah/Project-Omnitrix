using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecalibratedAsAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private RecalibratedAsDisplayAliensController recalibratedAsDisplayAliensController;

	public FreeCameraController freeCameraController;

	[Header("Screen Animation Elements")]
	public Transform diamond;

	[Header("Watch Parts")]
	public Transform mobilePartsUp;

	public Transform ring;

	public GameObject core;

	public Transform[] buttonsB;

	public MeshRenderer[] shinyParts;

	[Header("Lights")]
	public Light[] flashLights;

	public Light coreLight;

	public Color[] lightColors;

	[Header("Materials")]
	public Material greenLightMat;

	public Material ogShinyPartsMat;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	public MeshRenderer lightCone;

	private float lightConeFadeLevel;

	[Header("Extra")]
	public Image greenScreen;

	public GameObject transformationScene;

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private string[] selectSounds = { "Dial1", "Dial2" };

	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		recalibratedAsDisplayAliensController = GetComponent<RecalibratedAsDisplayAliensController>();
		transformationScene.SetActive(false);
		watchStatus = "normal";
		cameraFollowWatch = true;
		lightConeFadeLevel = 1f;
		CameraIntro();
	}

	private void Update()
	{
		UpdateLightShafts();
		UpdateLightCone();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
	}

	private void UpdateLightShafts()
	{
		lightShaftsQuads[0].material.SetFloat("_AppearProgress", lightShaftsProgress);
		lightShaftsQuads[1].material.SetFloat("_AppearProgress", lightShaftsProgress);
	}

	private void UpdateLightCone()
	{
		lightCone.material.SetFloat("_FadeStart", lightConeFadeLevel);
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

	public void EnableControls(bool status)
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
			freeCameraController.MoveFreeCamera(1);
			CameraZoom();
			watchStatus = "activated";
			StartCoroutine(WaitToActivationAnim());
			break;
		case "activated":
			EnableControls(false);
			freeCameraController.MoveFreeCamera(0);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transformed":
			EnableControls(false);
			StartTimeOutFlash();
			watchStatus = "normal";
			break;
		default:
			Debug.Log("Invalid Status");
			break;
		}
	}

	private void CameraZoom()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 0.5f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 0.5f);
	}

	private IEnumerator WaitToActivationAnim()
	{
		yield return new WaitForSeconds(0.25f);
		ActivationAnim();
	}

	private void ActivationAnim()
	{
		core.SetActive(true);
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y + 0.55f, 0.25f).SetEase(Ease.Flash).OnComplete(() => recalibratedAsDisplayAliensController.ShowAlien(0.25f));
		coreLight.DOIntensity(20f, 0.2f).SetEase(Ease.Flash);
		flashLights[0].DOIntensity(35f, 0.2f).SetEase(Ease.Flash);
		flashLights[1].DOIntensity(35f, 0.2f).SetEase(Ease.Flash);
		buttonsB[0].DOMoveZ(buttonsB[0].position.z + 0.15f, 0.25f);
		buttonsB[1].DOMoveZ(buttonsB[1].position.z - 0.15f, 0.25f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = greenLightMat;
		}
		diamond.DOScale(1.75f, 0.25f).SetEase(Ease.Flash);
		lightConeFadeLevel = 1f;
		DOTween.To(() => lightConeFadeLevel, x => lightConeFadeLevel = x, 0f, 0.25f).SetEase(Ease.Flash);
		Audio.Play("CoreUp");
		Audio.Play("DisplayHologram0");
	}

	private void PushAnim()
	{
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y - 0.55f, 0.1f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		DOTween.To(() => lightConeFadeLevel, x => lightConeFadeLevel = x, 1f, 0.1f).SetEase(Ease.Flash);
		StartTransformFlash(0.5f);
		Audio.Play("Push");
		Audio.Play("Transform0");
	}

	private void StartTransformFlash(float duration)
	{
		flashLights[0].DOIntensity(100f, duration);
		flashLights[1].DOIntensity(100f, duration);
		greenScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(SwitchToTransformedScene);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
	}

	private void SwitchToTransformedScene()
	{
		transformationScene.SetActive(true);
		flashLights[1].color = lightColors[1];
		coreLight.DOIntensity(0f, 0.2f).SetEase(Ease.Flash);
		diamond.localScale = Vector3.zero;
		buttonsB[0].DOLocalMove(Vector3.zero, 0.5f);
		buttonsB[1].DOLocalMove(Vector3.zero, 0.5f);
		foreach (MeshRenderer part in shinyParts)
		{
			part.material = ogShinyPartsMat;
		}
		recalibratedAsDisplayAliensController.aliensGroup.transform.localScale = Vector3.zero;
		SnapCamera(cameraPositions[3]);
		EndTransformFlash(0.5f);
	}

	private void EndTransformFlash(float duration)
	{
		flashLights[0].DOIntensity(10f, duration);
		flashLights[1].DOIntensity(10f, duration);
		greenScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(() => EnableControls(true));
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration).SetEase(Ease.OutQuint);
	}

	private void StartTimeOutFlash()
	{
		Audio.Play("Transform1");
		flashLights[0].DOIntensity(100f, 0.8f).SetEase(Ease.Flash).OnComplete(SwitchToNormalScene);
		flashLights[1].DOIntensity(100f, 0.8f).SetEase(Ease.Flash);
		greenScreen.DOFade(1f, 0.8f).SetEase(Ease.InExpo);
	}

	private void SwitchToNormalScene()
	{
		flashLights[1].color = lightColors[0];
		transformationScene.SetActive(false);
		SnapCamera(cameraPositions[1]);
		EndTimeOutFlash();
	}

	private void EndTimeOutFlash()
	{
		flashLights[0].DOIntensity(0f, 0.5f).OnComplete(() => EnableControls(true));
		flashLights[1].DOIntensity(0f, 0.5f);
		greenScreen.DOFade(0f, 0.5f).SetEase(Ease.InQuart);
	}

	public void RotateLeft()
	{
		Rotate(-90f, "left");
	}

	public void RotateRight()
	{
		Rotate(90f, "right");
	}

	private void Rotate(float degrees, string direction)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.25f).SetEase(Ease.Flash);
		recalibratedAsDisplayAliensController.SwitchAlien(direction);
		Audio.Play("Dial0");
		Audio.Play(selectSounds[Random.Range(0, 2)]);
	}
}
