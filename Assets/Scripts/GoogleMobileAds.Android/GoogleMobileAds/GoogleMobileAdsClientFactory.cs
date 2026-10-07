using GoogleMobileAds.Common;
using UnityEngine.Scripting;

namespace GoogleMobileAds
{
	[Preserve]
	public class GoogleMobileAdsClientFactory : IClientFactory
	{
		private static bool? _decagonEnabled;

		public IAppStateEventClient BuildAppStateEventClient()
		{
			return null;
		}

		public IAppOpenAdClient BuildAppOpenAdClient()
		{
			return null;
		}

		public IBannerClient BuildBannerClient()
		{
			return null;
		}

		public IAdManagerBannerClient BuildAdManagerBannerClient()
		{
			return null;
		}

		public IInterstitialClient BuildInterstitialClient()
		{
			return null;
		}

		public IAdManagerInterstitialClient BuildAdManagerInterstitialClient()
		{
			return null;
		}

		public IRewardedAdClient BuildRewardedAdClient()
		{
			return null;
		}

		public IRewardedInterstitialAdClient BuildRewardedInterstitialAdClient()
		{
			return null;
		}

		public INativeOverlayAdClient BuildNativeOverlayAdClient()
		{
			return null;
		}

		public IApplicationPreferencesClient ApplicationPreferencesInstance()
		{
			return null;
		}

		public IMobileAdsClient MobileAdsInstance()
		{
			return null;
		}

		private bool IsDecagonEnabled()
		{
			return false;
		}

		public IAppOpenAdPreloaderClient BuildAppOpenAdPreloaderClient()
		{
			return null;
		}

		public IInterstitialAdPreloaderClient BuildInterstitialAdPreloaderClient()
		{
			return null;
		}

		public IRewardedAdPreloaderClient BuildRewardedAdPreloaderClient()
		{
			return null;
		}
	}
}
