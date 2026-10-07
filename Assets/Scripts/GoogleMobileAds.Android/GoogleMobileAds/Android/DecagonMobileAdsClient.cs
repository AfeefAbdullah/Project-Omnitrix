using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class DecagonMobileAdsClient : AndroidJavaProxy, IMobileAdsClient
	{
		private static readonly DecagonMobileAdsClient _instance;

		private readonly AndroidJavaObject _mobileAdsClass;

		private Action<IInitializationStatusClient> _initCompleteAction;

		public static DecagonMobileAdsClient Instance => null;

		private DecagonMobileAdsClient()
			: base((string)null)
		{
		}

		public void Initialize(Action<IInitializationStatusClient> initCompleteAction)
		{
		}

		public void DisableMediationInitialization()
		{
		}

		public void SetApplicationVolume(float volume)
		{
		}

		public void SetApplicationMuted(bool muted)
		{
		}

		public void SetRequestConfiguration(RequestConfiguration requestConfiguration)
		{
		}

		public RequestConfiguration GetRequestConfiguration()
		{
			return null;
		}

		public void SetiOSAppPauseOnBackground(bool pause)
		{
		}

		public void DisableSDKCrashReporting()
		{
		}

		public void OpenAdInspector(Action<AdInspectorErrorClientEventArgs> onAdInspectorClosed)
		{
		}

		public void Preload(List<PreloadConfiguration> configurations, Action<PreloadConfiguration> onAdAvailable, Action<PreloadConfiguration> onAdsExhausted)
		{
		}

		public float GetDeviceScale()
		{
			return 0f;
		}

		public int GetDeviceSafeWidth()
		{
			return 0;
		}

		public Version GetSDKVersion()
		{
			return null;
		}

		public void onAdapterInitializationComplete(AndroidJavaObject initStatus)
		{
		}
	}
}
