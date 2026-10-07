using GoogleMobileAds.Common;

namespace GoogleMobileAds
{
	public interface IClientFactory
	{
		IAppStateEventClient BuildAppStateEventClient();

		IAppOpenAdClient BuildAppOpenAdClient();

		IBannerClient BuildBannerClient();

		IAdManagerBannerClient BuildAdManagerBannerClient();

		IInterstitialClient BuildInterstitialClient();

		IAdManagerInterstitialClient BuildAdManagerInterstitialClient();

		IRewardedAdClient BuildRewardedAdClient();

		IRewardedInterstitialAdClient BuildRewardedInterstitialAdClient();

		INativeOverlayAdClient BuildNativeOverlayAdClient();

		IApplicationPreferencesClient ApplicationPreferencesInstance();

		IMobileAdsClient MobileAdsInstance();

		IAppOpenAdPreloaderClient BuildAppOpenAdPreloaderClient();

		IInterstitialAdPreloaderClient BuildInterstitialAdPreloaderClient();

		IRewardedAdPreloaderClient BuildRewardedAdPreloaderClient();
	}
}
