using UnityEngine;
using UnityEngine.Events;

public class KeyboardInputController : MonoBehaviour
{
	public UnityEvent ActFunction;

	public UnityEvent rotateLeftFunciton;

	public UnityEvent rotateRightFunciton;

	private FreeCameraController freeCameraController;

	private MenuController menuController;

	private void Start()
	{
		freeCameraController = GetComponent<FreeCameraController>();
		menuController = GetComponent<MenuController>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && CanAct())
		{
			ActFunction.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && CanAct())
		{
			rotateLeftFunciton.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && CanAct())
		{
			rotateRightFunciton.Invoke();
		}
	}

	private bool CanAct()
	{
		return freeCameraController.cameraMode != cameraModes.freeMode && !menuController.menuOpen;
	}
}
