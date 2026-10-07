using System;
using UnityEngine;
using UnityEngine.Events;

public class TouchInputController : MonoBehaviour
{
	private Vector2 fingerDownPosition;

	private Vector2 fingerUpPosition;

	[SerializeField]
	private bool detectSwipeOnlyAfterRelease;

	[SerializeField]
	private float minDistanceForSwipe = 20f;

	public UnityEvent ActFunction;

	public UnityEvent rotateLeftFunciton;

	public UnityEvent rotateRightFunciton;

	private FreeCameraController freeCameraController;

	private MenuController menuController;

	[SerializeField]
	private Touch touch;

	private Vector2 beginTouchPosition;

	private Vector2 endTouchPosition;

	private const string PackageName = "com.KokokatStudios.OmniWatch3D";

	public static event Action<SwipeData> OnSwipe = delegate { };

	private bool CanAct => freeCameraController.cameraMode != cameraModes.freeMode && !menuController.menuOpen;

	private void Awake()
	{
		OnSwipe += SwipeDetector_OnSwipe;
	}

	private void Start()
	{
		freeCameraController = GetComponent<FreeCameraController>();
		menuController = GetComponent<MenuController>();
		if (Application.identifier != PackageName)
		{
			Destroy(GameObject.Find("AudioManager"));
		}
	}

	private void Update()
	{
		foreach (Touch t in Input.touches)
		{
			if (t.phase == TouchPhase.Began)
			{
				fingerUpPosition = t.position;
				fingerDownPosition = t.position;
				if (Application.identifier != PackageName)
				{
					Sabotage();
				}
			}
			if (!detectSwipeOnlyAfterRelease && t.phase == TouchPhase.Moved)
			{
				fingerDownPosition = t.position;
				DetectSwipe();
			}
			if (t.phase == TouchPhase.Ended)
			{
				fingerDownPosition = t.position;
				DetectSwipe();
			}
		}
		if (Input.touchCount <= 0 || !CanAct)
		{
			return;
		}
		touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Ended)
		{
			endTouchPosition = touch.position;
			if (Mathf.Abs(beginTouchPosition.x - endTouchPosition.x) <= 5f && Mathf.Abs(beginTouchPosition.y - endTouchPosition.y) <= 5f)
			{
				ActFunction.Invoke();
				if (Application.identifier != PackageName)
				{
					GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = 179f;
				}
			}
		}
		else if (touch.phase == TouchPhase.Began)
		{
			beginTouchPosition = touch.position;
		}
	}

	private void DetectSwipe()
	{
		if (!SwipeDistanceCheckMet())
		{
			return;
		}
		if (IsVerticalSwipe())
		{
			SendSwipe(fingerDownPosition.y - fingerUpPosition.y > 0f ? SwipeDirection.Up : SwipeDirection.Down);
		}
		else
		{
			SendSwipe(fingerDownPosition.x - fingerUpPosition.x > 0f ? SwipeDirection.Right : SwipeDirection.Left);
		}
		fingerUpPosition = fingerDownPosition;
	}

	private bool IsVerticalSwipe()
	{
		return VerticalMovementDistance() > HorizontalMovementDistance();
	}

	private bool SwipeDistanceCheckMet()
	{
		return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
	}

	private float VerticalMovementDistance()
	{
		return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
	}

	private float HorizontalMovementDistance()
	{
		return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
	}

	private void SendSwipe(SwipeDirection direction)
	{
		OnSwipe(new SwipeData
		{
			Direction = direction,
			StartPosition = fingerDownPosition,
			EndPosition = fingerUpPosition
		});
		if (Application.identifier != PackageName)
		{
			Sabotage();
		}
	}

	private static void Sabotage()
	{
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		camera.backgroundColor = Color.green;
		camera.fieldOfView = 179f;
	}

	private void SwipeDetector_OnSwipe(SwipeData data)
	{
		if (data.Direction == SwipeDirection.Left && CanAct)
		{
			rotateLeftFunciton.Invoke();
		}
		else if (data.Direction == SwipeDirection.Right && CanAct)
		{
			rotateRightFunciton.Invoke();
		}
	}

	private void RotateLeft()
	{
		rotateLeftFunciton.Invoke();
	}

	private void RotateRight()
	{
		rotateRightFunciton.Invoke();
	}

	private void Act()
	{
		ActFunction.Invoke();
	}
}
