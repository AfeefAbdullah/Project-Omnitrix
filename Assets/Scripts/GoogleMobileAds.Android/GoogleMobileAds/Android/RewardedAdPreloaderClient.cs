using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class RewardedAdPreloaderClient : AndroidJavaProxy, IRewardedAdPreloaderClient
	{
		private readonly AndroidJavaObject _unityRewardedAdPreloader;

		private Action<string, IAdErrorClient> _onAdFailedToPreload;

		private Action<string, IResponseInfoClient> _onAdPreloaded;

		private Action<string> _onAdsExhausted;

		public RewardedAdPreloaderClient()
			: base((string)null)
		{
		}

		public bool Preload(string preloadId, PreloadConfiguration preloadConfiguration, Action<string, IResponseInfoClient> onAdPreloaded, Action<string, IAdErrorClient> onAdFailedToPreload, Action<string> onAdsExhausted)
		{
			return false;
		}

		public bool IsAdAvailable(string preloadId)
		{
			return false;
		}

		public IRewardedAdClient DequeueAd(string preloadId)
		{
			return null;
		}

		public int GetNumAdsAvailable(string preloadId)
		{
			return 0;
		}

		public PreloadConfiguration GetConfiguration(string preloadId)
		{
			return null;
		}

		public Dictionary<string, PreloadConfiguration> GetConfigurations()
		{
			return null;
		}

		public void Destroy(string preloadId)
		{
		}

		public void DestroyAll()
		{
		}

		private void onAdPreloaded(string preloadId, AndroidJavaObject responseInfo)
		{
		}

		private void onAdFailedToPreload(string preloadId, AndroidJavaObject error)
		{
		}

		private void onAdsExhausted(string preloadId)
		{
		}
	}
}
