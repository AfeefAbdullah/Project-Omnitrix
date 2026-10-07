using System;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	public class OnConsentFormLoadFailureListener : AndroidJavaProxy
	{
		private readonly Action<FormError> _onConsentFormLoadFailureAction;

		public OnConsentFormLoadFailureListener(Action<FormError> onConsentFormLoadFailureAction)
			: base((string)null)
		{
		}

		internal void onConsentFormLoadFailure(AndroidJavaObject error)
		{
		}
	}
}
