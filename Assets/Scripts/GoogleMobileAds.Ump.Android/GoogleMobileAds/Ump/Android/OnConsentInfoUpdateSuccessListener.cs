using System;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	public class OnConsentInfoUpdateSuccessListener : AndroidJavaProxy
	{
		private Action _onConsentInfoUpdateSuccessAction;

		public OnConsentInfoUpdateSuccessListener(Action onConsentInfoUpdateSuccessAction)
			: base((string)null)
		{
		}

		internal void onConsentInfoUpdateSuccess()
		{
		}
	}
}
