using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class Utils
	{
		public const string AdMobAdapterClassName = "com.google.ads.mediation.admob.AdMobAdapter";

		public const string AdListenerClassName = "com.google.android.gms.ads.AdListener";

		public const string AdRequestClassName = "com.google.android.gms.ads.AdRequest";

		public const string AdRequestBuilderClassName = "com.google.android.gms.ads.AdRequest$Builder";

		public const string AdManagerAdRequestBuilderClassName = "com.google.android.gms.ads.admanager.AdManagerAdRequest$Builder";

		public const string AdSizeClassName = "com.google.android.gms.ads.AdSize";

		public const string AppOpenAdClassName = "com.google.android.gms.ads.appopen.AppOpenAd";

		public const string PlayStorePurchaseListenerClassName = "com.google.android.gms.ads.purchase.PlayStorePurchaseListener";

		public const string PublisherPrivacyPersonalizationStateEnumName = "com.google.android.gms.ads.RequestConfiguration$PublisherPrivacyPersonalizationState";

		public const string MobileAdsClassName = "com.google.android.gms.ads.MobileAds";

		public const string PreloadConfigurationClassName = "com.google.android.gms.ads.preload.PreloadConfiguration";

		public const string PreloadConfigurationBuilderClassName = "com.google.android.gms.ads.preload.PreloadConfiguration$Builder";

		public const string PreloadListenerClassname = "com.google.android.gms.ads.preload.PreloadCallback";

		public const string PreloadCallbackClassname = "com.google.unity.ads.UnityPreloadCallback";

		public const string AdFormatEnumName = "com.google.android.gms.ads.AdFormat";

		public const string RequestConfigurationClassName = "com.google.android.gms.ads.RequestConfiguration";

		public const string RequestConfigurationBuilderClassName = "com.google.android.gms.ads.RequestConfiguration$Builder";

		public const string ServerSideVerificationOptionsClassName = "com.google.android.gms.ads.rewarded.ServerSideVerificationOptions";

		public const string ServerSideVerificationOptionsBuilderClassName = "com.google.android.gms.ads.rewarded.ServerSideVerificationOptions$Builder";

		public const string NativeAdOptionsBuilderClassName = "com.google.android.gms.ads.nativead.NativeAdOptions$Builder";

		public const string VideoOptionsBuilderClassName = "com.google.android.gms.ads.VideoOptions$Builder";

		public const string UnityAdSizeClassName = "com.google.unity.ads.UnityAdSize";

		public const string UnityMobileAdsClassName = "com.google.unity.ads.UnityMobileAds";

		public const string BannerViewClassName = "com.google.unity.ads.Banner";

		public const string InterstitialClassName = "com.google.unity.ads.Interstitial";

		public const string RewardBasedVideoClassName = "com.google.unity.ads.RewardBasedVideo";

		public const string UnityRewardedAdClassName = "com.google.unity.ads.UnityRewardedAd";

		public const string UnityRewardedAdPreloaderClassName = "com.google.unity.ads.UnityRewardedAdPreloader";

		public const string UnityAdListenerClassName = "com.google.unity.ads.UnityAdListener";

		public const string UnityAdManagerAdListenerClassName = "com.google.unity.ads.admanager.UnityAdManagerAdListener";

		public const string UnityAppStateEventNotifierClassName = "com.google.unity.ads.UnityAppStateEventNotifier";

		public const string UnityAppStateEventCallbackClassName = "com.google.unity.ads.UnityAppStateEventCallback";

		public const string UnityRewardedAdCallbackClassName = "com.google.unity.ads.UnityRewardedAdCallback";

		public const string UnityAdManagerBannerViewClassName = "com.google.unity.ads.admanager.UnityAdManagerBannerView";

		public const string UnityInterstitialAdCallbackClassName = "com.google.unity.ads.UnityInterstitialAdCallback";

		public const string UnityAdManagerInterstitialAdClassName = "com.google.unity.ads.admanager.UnityAdManagerInterstitialAd";

		public const string UnityAdManagerInterstitialAdCallbackClassName = "com.google.unity.ads.admanager.UnityAdManagerInterstitialAdCallback";

		public const string UnityFullScreenContentCallbackClassName = "com.google.unity.ads.UnityFullScreenContentCallback";

		public const string UnityAdapterStatusEnumName = "com.google.android.gms.ads.initialization.AdapterStatus$State";

		public const string UnityAppOpenAdClassName = "com.google.unity.ads.UnityAppOpenAd";

		public const string UnityAppOpenAdCallbackClassName = "com.google.unity.ads.UnityAppOpenAdCallback";

		public const string UnityAppOpenAdPreloaderClassName = "com.google.unity.ads.UnityAppOpenAdPreloader";

		public const string OnInitializationCompleteListenerClassName = "com.google.android.gms.ads.initialization.OnInitializationCompleteListener";

		public const string UnityAdLoaderListenerClassName = "com.google.unity.ads.UnityAdLoaderListener";

		public const string UnityAdInspectorClassName = "com.google.unity.ads.UnityAdInspector";

		public const string UnityAdInspectorListenerClassname = "com.google.unity.ads.UnityAdInspectorListener";

		public const string UnityPaidEventListenerClassName = "com.google.unity.ads.UnityPaidEventListener";

		public const string UnityRewardedInterstitialAdClassName = "com.google.unity.ads.UnityRewardedInterstitialAd";

		public const string UnityInterstitialAdPreloaderClassName = "com.google.unity.ads.UnityInterstitialAdPreloader";

		public const string UnityRewardedInterstitialAdCallbackClassName = "com.google.unity.ads.UnityRewardedInterstitialAdCallback";

		public const string PluginUtilsClassName = "com.google.unity.ads.PluginUtils";

		public const string UnityApplicationPreferencesClassName = "com.google.unity.ads.UnityApplicationPreferences";

		public const string UnityNativeTemplateAdClassName = "com.google.unity.ads.nativead.UnityNativeTemplateAd";

		public const string UnityNativeTemplateAdCallbackClassName = "com.google.unity.ads.nativead.UnityNativeTemplateAdCallback";

		public const string UnityNativeTemplateFontStyleClassName = "com.google.unity.ads.nativead.UnityNativeTemplateFontStyle";

		public const string UnityNativeTemplateStyleClassName = "com.google.unity.ads.nativead.UnityNativeTemplateStyle";

		public const string UnityNativeTemplateTextStyleClassName = "com.google.unity.ads.nativead.UnityNativeTemplateTextStyle";

		public const string UnityNativeTemplateTypeClassName = "com.google.unity.ads.nativead.UnityNativeTemplateType";

		public const string UnityActivityClassName = "com.unity3d.player.UnityPlayer";

		public const string ColorClassName = "android.graphics.Color";

		public const string ColorDrawableClassName = "android.graphics.drawable.ColorDrawable";

		public const string BundleClassName = "android.os.Bundle";

		public const string PreferenceManagerClassName = "android.preference.PreferenceManager";

		public const string DisplayMetricsClassName = "android.util.DisplayMetrics";

		public const string DoubleClassName = "java.lang.Double";

		public const string DateClassName = "java.util.Date";

		public static AndroidJavaObject GetAdSizeJavaObject(AdSize adSize)
		{
			return null;
		}

		public static Dictionary<string, string> GetDictionary(AndroidJavaObject androidBundle)
		{
			return null;
		}

		internal static int GetScreenWidth()
		{
			return 0;
		}

		public static AndroidJavaObject GetAdRequestJavaObject(AdRequest request, string nativePluginVersion = null)
		{
			return null;
		}

		public static AndroidJavaObject GetAdManagerAdRequestJavaObject(AdRequest request, string nativePluginVersion = null)
		{
			return null;
		}

		public static AndroidJavaObject GetPreloadConfigurationJavaObject(PreloadConfiguration preloadConfiguration)
		{
			return null;
		}

		public static PreloadConfiguration GetPreloadConfiguration(AndroidJavaObject configurationJavaObject)
		{
			return null;
		}

		public static AndroidJavaObject GetJavaListObject(List<string> csTypeList)
		{
			return null;
		}

		public static List<string> GetCsTypeList(AndroidJavaObject javaTypeList)
		{
			return null;
		}

		public static AndroidJavaObject GetServerSideVerificationOptionsJavaObject(ServerSideVerificationOptions serverSideVerificationOptions)
		{
			return null;
		}

		public static AndroidJavaObject GetNativeAdOptionsJavaObject(NativeAdOptions nativeAdOptions)
		{
			return null;
		}

		public static AndroidJavaObject GetVideoOptionsJavaObject(VideoOptions videoOptions)
		{
			return null;
		}

		internal static AndroidJavaObject GetCurrentActivityAndroidJavaObject()
		{
			return null;
		}
	}
}
