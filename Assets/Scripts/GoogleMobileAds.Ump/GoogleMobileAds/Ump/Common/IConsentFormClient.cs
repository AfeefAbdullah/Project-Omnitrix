using System;
using GoogleMobileAds.Ump.Api;

namespace GoogleMobileAds.Ump.Common
{
	public interface IConsentFormClient
	{
		void LoadAndShowConsentFormIfRequired(Action<FormError> onDismissed);
	}
}
