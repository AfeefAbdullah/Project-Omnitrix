using System;
using GoogleMobileAds.Ump.Api;
using GoogleMobileAds.Ump.Common;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	internal class ConsentInformationClient : IConsentInformationClient
	{
		internal OnConsentInfoUpdateSuccessListener _onSuccess;

		internal OnConsentInfoUpdateFailureListener _onFailure;

		private static readonly ConsentInformationClient _instance;

		private readonly AndroidJavaObject _consentInformation;

		private readonly AndroidJavaObject _activity;

		public static ConsentInformationClient Instance => null;

		private ConsentInformationClient()
		{
		}

		public void Update(ConsentRequestParameters request, Action onConsentInfoUpdateSuccessCallback, Action<FormError> onConsentInfoUpdateFailureCallback)
		{
		}

		public void Reset()
		{
		}

		public int GetConsentStatus()
		{
			return 0;
		}

		public int GetPrivacyOptionsRequirementStatus()
		{
			return 0;
		}

		public bool CanRequestAds()
		{
			return false;
		}

		public bool IsConsentFormAvailable()
		{
			return false;
		}
	}
}
