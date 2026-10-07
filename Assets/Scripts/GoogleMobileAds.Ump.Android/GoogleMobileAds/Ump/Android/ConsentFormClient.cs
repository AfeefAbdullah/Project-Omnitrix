using System;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Ump.Common;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	internal class ConsentFormClient : AndroidJavaProxy, IConsentFormClient
	{
		internal OnConsentFormLoadSuccessListener _onSuccess;

		internal OnConsentFormLoadFailureListener _onFailure;

		internal OnConsentFormDismissedListener _onDismissed;

		private static readonly ConsentFormClient _instance;

		private readonly AndroidJavaObject _activity;

		private readonly AndroidJavaObject _userMessagingPlatformClass;

		private AndroidJavaObject _consentForm;

		private AndroidJavaObject _unityConsentForm;

		private Action<FormError> _onConsentFormDismissed;

		public static ConsentFormClient Instance => null;

		public ConsentFormClient()
			: base((string)null)
		{
		}

		public void Load(Action onFormLoaded, Action<FormError> onError)
		{
		}

		public void Show(Action<FormError> onDismissed)
		{
		}

		public void LoadAndShowConsentFormIfRequired(Action<FormError> onDismissed)
		{
		}

		public void ShowPrivacyOptionsForm(Action<FormError> onDismissed)
		{
		}

		public void onConsentFormDismissed(AndroidJavaObject error)
		{
		}
	}
}
