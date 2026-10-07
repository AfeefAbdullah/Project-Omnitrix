using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class MobileAdsClient : AndroidJavaProxy, IMobileAdsClient
	{
		private static readonly MobileAdsClient _instance;

		private readonly AndroidJavaClass _mobileAdsClass;

		private readonly IInsightsEmitter _insightsEmitter;

		private readonly ITracer _tracer;

		private Action<IInitializationStatusClient> _initCompleteAction;

		public static MobileAdsClient Instance => null;

		private MobileAdsClient()
			: base((string)null)
		{
		}

		public void Initialize(Action<IInitializationStatusClient> initCompleteAction)
		{
		}

		public void SetApplicationVolume(float volume)
		{
		}

		public void DisableMediationInitialization()
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

		public void onInitializationComplete(AndroidJavaObject initStatus)
		{
		}
	}
}
