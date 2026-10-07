using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class PreloadListener : AndroidJavaProxy
	{
		private Action<PreloadConfiguration> onAdAvailableAction;

		private Action<PreloadConfiguration> onAdsExhaustedAction;

		public PreloadListener(Action<PreloadConfiguration> onAdAvailable, Action<PreloadConfiguration> onAdsExhausted)
			: base((string)null)
		{
		}

		private void onAdsAvailable(AndroidJavaObject preloadConfiguration)
		{
		}

		private void onAdsExhausted(AndroidJavaObject preloadConfiguration)
		{
		}
	}
}
