using GoogleMobileAds.Ump.Api;
using UnityEngine;

namespace GoogleMobileAds.Ump.Android
{
	internal class Utils
	{
		public const string ConsentDebugSettingsBuilderClassName = "com.google.android.ump.ConsentDebugSettings$Builder";

		public const string UnityConsentFormClassName = "com.google.unity.ump.UnityConsentForm";

		public const string UnityConsentFormCallbackClassName = "com.google.unity.ump.UnityConsentFormCallback";

		public const string ConsentRequestParametersBuilderClassName = "com.google.android.ump.ConsentRequestParameters$Builder";

		public const string UserMessagingPlatformClassName = "com.google.android.ump.UserMessagingPlatform";

		public const string OnConsentInfoUpdateSuccessListenerClassName = "com.google.android.ump.ConsentInformation$OnConsentInfoUpdateSuccessListener";

		public const string OnConsentInfoUpdateFailureListenerClassName = "com.google.android.ump.ConsentInformation$OnConsentInfoUpdateFailureListener";

		public const string OnConsentFormLoadSuccessListenerClassName = "com.google.android.ump.UserMessagingPlatform$OnConsentFormLoadSuccessListener";

		public const string OnConsentFormLoadFailureListenerClassName = "com.google.android.ump.UserMessagingPlatform$OnConsentFormLoadFailureListener";

		public const string OnConsentFormDismissedListenerClassName = "com.google.android.ump.ConsentForm$OnConsentFormDismissedListener";

		public const string UnityActivityClassName = "com.unity3d.player.UnityPlayer";

		public static AndroidJavaObject GetConsentRequestParametersJavaObject(ConsentRequestParameters consentRequestParameters, AndroidJavaObject activity)
		{
			return null;
		}
	}
}
