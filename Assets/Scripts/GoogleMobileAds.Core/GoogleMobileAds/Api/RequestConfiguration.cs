using System;
using System.Collections.Generic;

namespace GoogleMobileAds.Api
{
	[Serializable]
	public class RequestConfiguration
	{
		public MaxAdContentRating MaxAdContentRating;

		public TagForChildDirectedTreatment? TagForChildDirectedTreatment;

		public TagForUnderAgeOfConsent? TagForUnderAgeOfConsent;

		public PublisherPrivacyPersonalizationState? PublisherPrivacyPersonalizationState;

		public List<string> TestDeviceIds;

		public bool? PublisherFirstPartyIdEnabled;
	}
}
