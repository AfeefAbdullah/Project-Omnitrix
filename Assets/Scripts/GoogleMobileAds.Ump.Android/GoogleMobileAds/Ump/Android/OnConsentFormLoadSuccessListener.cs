using System;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	public class OnConsentFormLoadSuccessListener : AndroidJavaProxy
	{
		private readonly Action<AndroidJavaObject> _onConsentFormLoadSuccessAction;

		public OnConsentFormLoadSuccessListener(Action<AndroidJavaObject> onConsentFormLoadSuccessAction)
			: base((string)null)
		{
		}

		internal void onConsentFormLoadSuccess(AndroidJavaObject consentFormJavaObject)
		{
		}
	}
}
