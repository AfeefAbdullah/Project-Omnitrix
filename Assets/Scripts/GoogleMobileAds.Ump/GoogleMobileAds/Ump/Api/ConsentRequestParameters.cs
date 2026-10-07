using System;

namespace GoogleMobileAds.Ump.Api
{
	[Serializable]
	public class ConsentRequestParameters
	{
		public bool TagForUnderAgeOfConsent;

		public ConsentDebugSettings ConsentDebugSettings;
	}
}
