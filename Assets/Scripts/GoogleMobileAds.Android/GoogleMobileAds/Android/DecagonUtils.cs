using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class DecagonUtils
	{
		public const string MaxAdContentRatingClassName = "com.google.android.libraries.ads.mobile.sdk.common.RequestConfiguration$MaxAdContentRating";

		public const string PublisherPrivacyPersonalizationStateEnumName = "com.google.android.libraries.ads.mobile.sdk.common.RequestConfiguration$PublisherPrivacyPersonalizationState";

		public const string RequestConfigurationBuilderClassName = "com.google.android.libraries.ads.mobile.sdk.common.RequestConfiguration$Builder";

		public const string TagForChildDirectedTreatmentClassName = "com.google.android.libraries.ads.mobile.sdk.common.RequestConfiguration$TagForChildDirectedTreatment";

		public const string TagForUnderAgeOfConsentClassName = "com.google.android.libraries.ads.mobile.sdk.common.RequestConfiguration$TagForUnderAgeOfConsent";

		public const string InitializationConfigBuilderClassName = "com.google.android.libraries.ads.mobile.sdk.initialization.InitializationConfig$Builder";

		public const string AdapterStatusInitializationStateName = "com.google.android.libraries.ads.mobile.sdk.initialization.AdapterStatus$InitializationState";

		public const string MobileAdsClassName = "com.google.android.libraries.ads.mobile.sdk.MobileAds";

		public const string OnInitializationCompleteListenerClassName = "com.google.android.libraries.ads.mobile.sdk.initialization.OnAdapterInitializationCompleteListener";

		public const string AdRequestBuilderClassName = "com.google.android.libraries.ads.mobile.sdk.common.AdRequest$Builder";

		public const string BannerAdRequestBuilderClassName = "com.google.android.libraries.ads.mobile.sdk.banner.BannerAdRequest$Builder";

		public const string AdSizeClassName = "com.google.android.libraries.ads.mobile.sdk.banner.AdSize";

		public const string UnityMobileAdsClassName = "com.google.unity.ads.decagon.UnityMobileAds";

		public const string UnityAdInspectorClassName = "com.google.unity.ads.decagon.UnityAdInspector";

		public const string UnityAdInspectorListenerClassName = "com.google.unity.ads.decagon.UnityAdInspectorListener";

		public const string UnityAppOpenAdClassName = "com.google.unity.ads.decagon.UnityAppOpenAd";

		public const string UnityAppOpenAdCallbackClassName = "com.google.unity.ads.decagon.UnityAppOpenAdCallback";

		public const string UnityBannerAdClassName = "com.google.unity.ads.decagon.UnityBannerAd";

		public const string UnityBannerAdCallbackClassName = "com.google.unity.ads.decagon.UnityBannerAdCallback";

		public const string UnityInterstitialAdClassName = "com.google.unity.ads.decagon.UnityInterstitialAd";

		public const string UnityInterstitialAdCallbackClassName = "com.google.unity.ads.decagon.UnityInterstitialAdCallback";

		public const string UnityRewardedAdClassName = "com.google.unity.ads.decagon.UnityRewardedAd";

		public const string UnityRewardedAdCallbackClassName = "com.google.unity.ads.decagon.UnityRewardedAdCallback";

		public const string UnityRewardedInterstitialAdClassName = "com.google.unity.ads.decagon.UnityRewardedInterstitialAd";

		public const string UnityRewardedInterstitialAdCallbackClassName = "com.google.unity.ads.decagon.UnityRewardedInterstitialAdCallback";

		public static AndroidJavaObject GetAdRequestJavaObject(AdRequest request, string adUnitId)
		{
			return null;
		}

		public static AndroidJavaObject GetBannerAdRequestJavaObject(string adUnitId, AdRequest request, AdSize adSize)
		{
			return null;
		}

		public static AndroidJavaObject GetAdSizeJavaObject(AdSize adSize)
		{
			return null;
		}
	}
}
