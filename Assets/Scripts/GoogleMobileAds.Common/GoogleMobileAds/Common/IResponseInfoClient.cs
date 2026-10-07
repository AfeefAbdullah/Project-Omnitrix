using System.Collections.Generic;

namespace GoogleMobileAds.Common
{
	public interface IResponseInfoClient
	{
		IAdapterResponseInfoClient GetLoadedAdapterResponseInfo();

		List<IAdapterResponseInfoClient> GetAdapterResponses();

		Dictionary<string, string> GetResponseExtras();

		string GetMediationAdapterClassName();

		string GetResponseId();
	}
}
