using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class ApplicationPreferencesClient : IApplicationPreferencesClient
	{
		private static AndroidJavaObject _androidApplicationPreferences;

		public void SetInt(string key, int value)
		{
		}

		public void SetString(string key, string value)
		{
		}

		public int GetInt(string key)
		{
			return 0;
		}

		public string GetString(string key)
		{
			return null;
		}
	}
}
