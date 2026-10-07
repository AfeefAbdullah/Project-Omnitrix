using System;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	public class OnConsentFormDismissedListener : AndroidJavaProxy
	{
		private readonly Action<FormError> _onConsentFormDismissedAction;

		public OnConsentFormDismissedListener(Action<FormError> onConsentFormDismissedAction)
			: base((string)null)
		{
		}

		internal void onConsentFormDismissed(AndroidJavaObject error)
		{
		}
	}
}
