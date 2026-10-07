using System;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class MobileAds
	{
		private readonly IMobileAdsClient client;

		private static IClientFactory clientFactory;

		private static MobileAds instance;

		public static MobileAds Instance => null;

		public static bool RaiseAdEventsOnUnityMainThread { get; set; }

		public static void Initialize(Action<InitializationStatus> initCompleteAction)
		{
		}

		public static void SetRequestConfiguration(RequestConfiguration requestConfiguration)
		{
		}

		internal static IClientFactory GetClientFactory()
		{
			return null;
		}

		internal static void RaiseAction(Action action)
		{
		}

		private static IMobileAdsClient GetMobileAdsClient()
		{
			return null;
		}
	}
}
