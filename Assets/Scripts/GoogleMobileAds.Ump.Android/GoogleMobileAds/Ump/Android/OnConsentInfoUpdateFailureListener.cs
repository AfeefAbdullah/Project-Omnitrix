using System;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	public class OnConsentInfoUpdateFailureListener : AndroidJavaProxy
	{
		private Action<FormError> _onConsentInfoUpdateFailureAction;

		public OnConsentInfoUpdateFailureListener(Action<FormError> onConsentInfoUpdateFailureAction)
			: base((string)null)
		{
		}

		internal void onConsentInfoUpdateFailure(AndroidJavaObject error)
		{
		}
	}
}
