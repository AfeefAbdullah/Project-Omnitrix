using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class InterstitialAdPreloaderClient : AndroidJavaProxy, IInterstitialAdPreloaderClient
	{
		private readonly AndroidJavaObject _unityInterstitialPreloader;

		private Action<string, IResponseInfoClient> _onAdPreloaded;

		private Action<string, IAdErrorClient> _onAdFailedToPreload;

		private Action<string> _onAdsExhausted;

		public InterstitialAdPreloaderClient()
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

		public IInterstitialClient DequeueAd(string preloadId)
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
