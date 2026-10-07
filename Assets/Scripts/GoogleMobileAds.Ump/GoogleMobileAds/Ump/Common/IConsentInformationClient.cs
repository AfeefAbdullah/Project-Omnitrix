using System;
using GoogleMobileAds.Ump.Api;

namespace GoogleMobileAds.Ump.Common
{
	public interface IConsentInformationClient
	{
		void Update(ConsentRequestParameters consentRequestParameters, Action onConsentInfoUpdateSuccessCallback, Action<FormError> onConsentInfoUpdateFailureCallback);

		bool CanRequestAds();
	}
}
