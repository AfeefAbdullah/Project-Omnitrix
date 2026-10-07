using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CWaitOneFrame_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MenuManagement _003C_003E4__this;

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
		public _003CWaitOneFrame_003Ed__9(int _003C_003E1__state)
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

	public Image background;

	public ScrollRect scrollRect;

	public GameObject endingTransition;

	public GameObject menuPanel;

	public bool menuOpen;

	public GameObject[] lockButtons;

	public GameObject[] adMessages;

	public GameObject moreAppsPanel;

	public void OpenCloseMenu()
	{
	}

	[IteratorStateMachine(typeof(_003CWaitOneFrame_003Ed__9))]
	private IEnumerator WaitOneFrame()
	{
		return null;
	}

	public void ChangeScene(string name)
	{
	}

	public void ShowAdMessage(RectTransform adMessage)
	{
	}

	public void HideAdMessage(RectTransform adMessage)
	{
	}

	public void DisableLockAndMessage(int i)
	{
	}

	public void EnableDisableMoreAppsPanel(bool activeState)
	{
	}
}
