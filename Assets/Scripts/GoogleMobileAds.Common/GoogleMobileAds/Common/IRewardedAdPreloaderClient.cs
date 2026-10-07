using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface IRewardedAdPreloaderClient
	{
		bool Preload(string preloadId, PreloadConfiguration preloadConfiguration, Action<string, IResponseInfoClient> onAdPreloaded = null, Action<string, IAdErrorClient> onAdFailedToPreload = null, Action<string> onAdsExhausted = null);

		bool IsAdAvailable(string preloadId);

		IRewardedAdClient DequeueAd(string preloadId);

		int GetNumAdsAvailable(string preloadId);

		PreloadConfiguration GetConfiguration(string preloadId);

		Dictionary<string, PreloadConfiguration> GetConfigurations();

		void Destroy(string preloadId);

		void DestroyAll();
	}
}
