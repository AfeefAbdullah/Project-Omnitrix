using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnitrixAnimationsController : MonoBehaviour
{
	public MenuController menuController;

	public Transform defaultCameraHolder;

	public Transform mobileParts;

	public Image greenScreen;

	public Transform cameraFollowPoint;

	public bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	public Transform[] cameraPositions;

	public MeshRenderer coreCircuits;

	public MeshRenderer coreBackground;

	public Material circuitsOn;

	public Material circuitsOff;

	public Material grenLight;

	public Material metalCore;

	public GameObject shinyParts;

	private void Start()
	{
		watchStatus = "normal";
		cameraFollowWatch = true;
		CameraIntro();
		coreBackground.gameObject.SetActive(true);
		coreBackground.material.DOFade(0f, 0f);
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
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
			StartCoroutine(WaitToActivationAnim());
			CamZoomAnim1();
			watchStatus = "activated";
			break;
		case "activated":
			EnableControls(false);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transformed":
			EnableControls(false);
			StartTimeOutFlash();
			watchStatus = "normal";
			break;
		}
	}

	private IEnumerator WaitToActivationAnim()
	{
		yield return new WaitForSeconds(0.25f);
		ActivationAnim();
	}

	private void ActivationAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y + 0.375f, 0.4f);
		FindObjectOfType<AudioManager>().Play("CoreUp");
	}

	private void CamZoomAnim1()
	{
		defaultCameraHolder.DOMove(cameraPositions[2].position, 0.65f);
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 0.65f).OnComplete(() => EnableControls(true));
	}

	private void PushAnim()
	{
		mobileParts.DOMoveY(mobileParts.position.y - 0.375f, 0.15f).SetEase(Ease.Flash);
		CamZoomAnim2();
		StartCoroutine(WaitToStartFlash());
		FindObjectOfType<AudioManager>().Play("Push");
		FindObjectOfType<AudioManager>().Play("Transform2");
	}

	private void CamZoomAnim2()
	{
		defaultCameraHolder.DOMove(cameraPositions[3].position, 3f).SetEase(Ease.InOutCubic);
		defaultCameraHolder.DORotate(cameraPositions[3].rotation.eulerAngles, 3f).SetEase(Ease.InOutCubic);
		coreBackground.material.DOFade(1f, 3f);
	}

	private IEnumerator WaitToStartFlash()
	{
		yield return new WaitForSeconds(2f);
		StartTransformationFlash();
	}

	private void StartTransformationFlash()
	{
		greenScreen.DOFade(1f, 1f).SetEase(Ease.InExpo).OnComplete(EndTransformationFlash);
	}

	private void EndTransformationFlash()
	{
		coreBackground.material = metalCore;
		coreCircuits.material = circuitsOff;
		shinyParts.SetActive(true);
		greenScreen.DOFade(0f, 0.75f).SetEase(Ease.InQuart).OnComplete(() => EnableControls(true));
	}

	private void StartTimeOutFlash()
	{
		FindObjectOfType<AudioManager>().Play("Transform0");
		greenScreen.DOFade(1f, 1f).SetEase(Ease.InExpo).OnComplete(EndTimeOutFlash);
	}

	private void EndTimeOutFlash()
	{
		defaultCameraHolder.position = cameraPositions[1].position;
		defaultCameraHolder.rotation = cameraPositions[1].rotation;
		coreBackground.material = grenLight;
		coreBackground.material.DOFade(0f, 0f);
		coreCircuits.material = circuitsOn;
		shinyParts.SetActive(false);
		greenScreen.DOFade(0f, 0.75f).SetEase(Ease.InQuart).OnComplete(() => EnableControls(true));
	}
}
