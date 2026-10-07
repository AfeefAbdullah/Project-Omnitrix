using System;
using GoogleMobileAds.Ump.Common;

namespace GoogleMobileAds.Ump.Api
{
	public class ConsentInformation
	{
		private static IUmpClientFactory _clientFactory;

		internal static IUmpClientFactory ClientFactory => null;

		public static void Update(ConsentRequestParameters request, Action<FormError> consentInfoUpdateCallback)
		{
		}

		public static bool CanRequestAds()
		{
			return false;
		}
	}
}
