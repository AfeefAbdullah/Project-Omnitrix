using System;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class AdInspectorListener : AndroidJavaProxy
	{
		private Action<AdInspectorErrorClientEventArgs> adInspectorClosedAction;

		public AdInspectorListener(Action<AdInspectorErrorClientEventArgs> adInspectorClosedAction)
			: base((string)null)
		{
		}

		private void onAdInspectorClosed(AndroidJavaObject error)
		{
		}
	}
}
