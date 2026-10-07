using UnityEngine;
using UnityEngine.Events;

public class TapInput : MonoBehaviour
{
	public UnityEvent ActFunction;

	private FreeCameraController freeCameraController;

	private MenuController menuController;

	[SerializeField]
	private Touch touch;

	private Vector2 beginTouchPosition;

	private Vector2 endTouchPosition;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void Act()
	{
	}
}
