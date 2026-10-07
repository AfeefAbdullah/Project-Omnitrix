using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CarnitrixAnimationsController : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CScreamTransform_003Ed__57 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CScreamTransform_003Ed__57(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CScreenActivationA_003Ed__51 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CarnitrixAnimationsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CScreenActivationA_003Ed__51(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitToEnableControls_003Ed__55 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public float time;

		public CarnitrixAnimationsController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CWaitToEnableControls_003Ed__55(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Header("Scripts References")]
	public MenuController menuController;

	public PrototypeDisplayAliensController prototypeDisplayAliensController;

	[Header("Screen Animation Elements")]
	public SpriteRenderer screenBackground;

	public Animator screenAnimator;

	public Transform diamondShape;

	[Header("Watch Parts")]
	public Renderer watchScreenRenderer;

	public float defaultScreenIntensity;

	private float currentScreenIntensity;

	public Transform[] mobileParts;

	public GameObject core;

	[Header("Lights")]
	public Light screenLight;

	public Color screenLightColor;

	public Light flashLight;

	public float maxFlashLightIntensity;

	public float defaultFlashLightIntensity;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public Animator watchAnimator;

	public PostProcessVolume volume;

	private ChromaticAberration chromaticLayer;

	private LensDistortion lensLayer;

	public Image veinsImage;

	private float veinsProgress;

	[Header("Modes Colors")]
	public Color defaultColor;

	public Color transformedColor;

	public Color timeOutColor;

	public Color disabledColor;

	[Header("Flash Colors")]
	public Color transformFlashColor;

	public Color timeOutFlashColor;

	[Header("Extra")]
	public GameObject transformationScene;

	public float mobilePartsUpHeight;

	private bool controlsEnabled;

	private string watchStatus;

	private bool cameraFollowWatch;

	private float waitCounter;

	private int timeOutBlinks;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateScreenEmissionIntensity()
	{
	}

	private void UpdateVeins()
	{
	}

	private void ColorScreenBackground(Color startColor, Color endColor, float transitionDuration, TweenCallback onCompleteMethod = null, Ease ease = Ease.Flash)
	{
	}

	private void FlashScreen(Color color, float startValue, float endValue, float duration, Ease ease = Ease.Flash, TweenCallback onCompleteMethod = null)
	{
	}

	private void CameraIntro()
	{
	}

	private void EnableControls(bool status)
	{
	}

	private void WaitSeconds(float seconds, TweenCallback methods)
	{
	}

	public void Act()
	{
	}

	public void ActivationAnim()
	{
	}

	private void CameraZoom()
	{
	}

	public void ScreenActivationAnim()
	{
	}

	[IteratorStateMachine(typeof(_003CScreenActivationA_003Ed__51))]
	private IEnumerator ScreenActivationA()
	{
		return null;
	}

	private void WaitingAnim()
	{
	}

	private void Heartbeat()
	{
	}

	public void DisplayAliens()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitToEnableControls_003Ed__55))]
	private IEnumerator WaitToEnableControls(float time)
	{
		return null;
	}

	private void PushAnim()
	{
	}

	[IteratorStateMachine(typeof(_003CScreamTransform_003Ed__57))]
	private IEnumerator ScreamTransform()
	{
		return null;
	}

	private void FastHeartbeat()
	{
	}

	private void StartTransformFlash()
	{
	}

	private void SwitchToTransformedScene()
	{
	}

	private void EndTransformFlash()
	{
	}

	private void TimeOutBlink()
	{
	}

	private void StartTimeOutFlash(float duration)
	{
	}

	private void SwitchToNormalScene()
	{
	}

	private void EndTimeOutFlash(float duration)
	{
	}

	private void ResetWatch()
	{
	}

	public void RotateLeft()
	{
	}

	private void SwitchAlienLeft()
	{
	}

	public void RotateRight()
	{
	}

	private void SwitchAlienRight()
	{
	}
}
