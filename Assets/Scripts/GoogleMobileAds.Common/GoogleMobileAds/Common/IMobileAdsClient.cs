using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface IMobileAdsClient
	{
		void Initialize(Action<IInitializationStatusClient> initCompleteAction);

		void Preload(List<PreloadConfiguration> configurations, Action<PreloadConfiguration> onAdAvailable, Action<PreloadConfiguration> onAdsExhausted);

		void DisableMediationInitialization();

		void SetApplicationVolume(float volume);

		void SetApplicationMuted(bool muted);

		void SetiOSAppPauseOnBackground(bool pause);

		void DisableSDKCrashReporting();

		float GetDeviceScale();

		int GetDeviceSafeWidth();

		void SetRequestConfiguration(RequestConfiguration requestConfiguration);

		Version GetSDKVersion();

		RequestConfiguration GetRequestConfiguration();

		void OpenAdInspector(Action<AdInspectorErrorClientEventArgs> adInspectorClosedAction);
	}
}
