using UnityEngine;
using UnityEngine.UI;

public class FreeCameraController : MonoBehaviour
{
	public Camera mainCamera;

	public Transform defaultCameraHolder;

	public Transform freeCameraHolderAxis;

	public Transform freeCameraHolder;

	public Transform[] freeCameraHolderPositions;

	public Transform fixedCameraHolder;

	public float sensitivity;

	public cameraModes cameraMode;

	public float verticalRotationDegrees;

	public float maxZoomValue = -4f;

	public float minZoomValue = -12f;

	private MenuController menuController;

	public Text[] controlsTexts;

	public GameObject watchBracelet;

	private bool CanControl => cameraMode == cameraModes.freeMode && !menuController.menuOpen;

	private void Start()
	{
		cameraMode = cameraModes.defaultMode;
		verticalRotationDegrees = 40f;
		menuController = GetComponent<MenuController>();
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
			camera.backgroundColor = Color.green;
			camera.fieldOfView = 179f;
		}
	}

	private void FixedUpdate()
	{
		LookAround();
		PinchZoom();
	}

	public void SwitchCameraMode()
	{
		string text;
		if (cameraMode == cameraModes.defaultMode)
		{
			SwitchCameraHolder(freeCameraHolder);
			cameraMode = cameraModes.freeMode;
			text = "Free camera controls:\nUse touch screen to rotate\nand zoom the camera.";
		}
		else if (cameraMode == cameraModes.freeMode || cameraMode == cameraModes.fixedMode)
		{
			SwitchCameraHolder(defaultCameraHolder);
			cameraMode = cameraModes.defaultMode;
			text = "Watch controls:\nTap the watch to interact.\nSwipe to switch alien.";
		}
		else
		{
			return;
		}
		if ((bool)watchBracelet)
		{
			watchBracelet.SetActive(true);
		}
		foreach (Text controlsText in controlsTexts)
		{
			controlsText.text = text;
		}
	}

	private void LookAround()
	{
		if (Input.touchCount == 1 && CanControl)
		{
			Touch touch = Input.GetTouch(0);
			freeCameraHolderAxis.Rotate(transform.up, touch.deltaPosition.x * sensitivity * Time.deltaTime, Space.World);
			verticalRotationDegrees = Mathf.Clamp(verticalRotationDegrees - touch.deltaPosition.y * sensitivity * Time.deltaTime, -35f, 75f);
			Vector3 euler = freeCameraHolderAxis.rotation.eulerAngles;
			freeCameraHolderAxis.rotation = Quaternion.Euler(verticalRotationDegrees, euler.y, euler.z);
		}
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Destroy(GameObject.Find("MobileParts"));
		}
	}

	public void MoveFreeCamera(int posIndex)
	{
		freeCameraHolderAxis.position = freeCameraHolderPositions[posIndex].position;
	}

	private void PinchZoom()
	{
		if (Input.touchCount == 2 && CanControl)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			float previousDistance = ((touchZero.position - touchZero.deltaPosition) - (touchOne.position - touchOne.deltaPosition)).magnitude;
			float currentDistance = (touchZero.position - touchOne.position).magnitude;
			Zoom((currentDistance - previousDistance) * 0.01f);
		}
		MouseZoom();
	}

	private void Zoom(float increment)
	{
		Vector3 position = freeCameraHolder.localPosition;
		freeCameraHolder.localPosition = new Vector3(position.x, position.y, Mathf.Clamp(position.z + increment, minZoomValue, maxZoomValue));
	}

	private void SwitchCameraHolder(Transform holder)
	{
		mainCamera.transform.parent = holder.transform;
		mainCamera.transform.localPosition = Vector3.zero;
		mainCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
		mainCamera.transform.localScale = Vector3.one;
	}

	private void MouseZoom()
	{
		if (CanControl)
		{
			Zoom(Input.GetAxis("Mouse ScrollWheel") * 10f);
		}
	}
}
