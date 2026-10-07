using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NemetrixAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private NemetrixDisplayAliensController nemetrixDisplayAliensController;

	[Header("Lights")]
	public Light flashLight;

	[Header("Extra")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	public Image greenScreen;

	public GameObject transformationScene;

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private void Start()
	{
		nemetrixDisplayAliensController = GetComponent<NemetrixDisplayAliensController>();
		transformationScene.SetActive(false);
		watchStatus = "normal";
		cameraFollowWatch = true;
		CameraIntro();
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
			CameraZoom();
			watchStatus = "activated";
			break;
		case "activated":
			EnableControls(false);
			StartTransformFlash();
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
		defaultCameraHolder.DORotate(cameraPositions[2].rotation.eulerAngles, 0.5f).OnComplete(DisplayAliens);
	}

	public void DisplayAliens()
	{
		FindObjectOfType<AudioManager>().Play("Activated");
		nemetrixDisplayAliensController.DisplayAliens();
	}

	private void StartTransformFlash()
	{
		FindObjectOfType<AudioManager>().Play("Transform");
		flashLight.DOIntensity(100f, 0.5f);
		greenScreen.DOFade(1f, 0.5f).SetEase(Ease.InExpo).OnComplete(SwitchToTransformedScene);
	}

	private void SwitchToTransformedScene()
	{
		transformationScene.SetActive(true);
		nemetrixDisplayAliensController.currentAlien.SetActive(false);
		defaultCameraHolder.position = cameraPositions[3].position;
		defaultCameraHolder.rotation = cameraPositions[3].rotation;
		EndTransformFlash();
	}

	private void EndTransformFlash()
	{
		flashLight.DOIntensity(10f, 0.5f);
		greenScreen.DOFade(0f, 0.5f).SetEase(Ease.InQuart).OnComplete(() => EnableControls(true));
	}

	private void StartTimeOutFlash()
	{
		FindObjectOfType<AudioManager>().Play("Untransform");
		flashLight.DOIntensity(100f, 0.2f).SetEase(Ease.Flash).OnComplete(SwitchToNormalScene);
		greenScreen.DOFade(1f, 0.2f).SetEase(Ease.InExpo);
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
		flashLight.DOIntensity(0f, 0.5f).OnComplete(() => EnableControls(true));
		greenScreen.DOFade(0f, 0.5f).SetEase(Ease.InQuart);
	}

	public void RotateLeft()
	{
		Rotate("left");
	}

	public void RotateRight()
	{
		Rotate("right");
	}

	private void Rotate(string direction)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		nemetrixDisplayAliensController.SwitchAlien(direction);
		FindObjectOfType<AudioManager>().Play("Dial");
	}
}
