using System;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class DecagonAdInspectorCallback : AndroidJavaProxy
	{
		private Action<AdInspectorErrorClientEventArgs> adInspectorClosedAction;

		public DecagonAdInspectorCallback(Action<AdInspectorErrorClientEventArgs> adInspectorClosedAction)
			: base((string)null)
		{
		}

		private void onAdInspectorClosed(AndroidJavaObject error)
		{
		}
	}
}
