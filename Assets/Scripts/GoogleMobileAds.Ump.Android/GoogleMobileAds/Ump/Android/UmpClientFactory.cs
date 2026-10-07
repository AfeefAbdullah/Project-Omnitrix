using GoogleMobileAds.Ump.Common;
using UnityEngine.Scripting;

namespace GoogleMobileAds.Ump.Android
{
	[Preserve]
	public class UmpClientFactory : IUmpClientFactory
	{
		public IConsentFormClient ConsentFormClient()
		{
			return null;
		}

		public IConsentInformationClient ConsentInformationClient()
		{
			return null;
		}
	}
}
