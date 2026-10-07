using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class DecagonInitializationStatusClient : IInitializationStatusClient
	{
		private AndroidJavaObject status;

		private AndroidJavaObject statusMap;

		public DecagonInitializationStatusClient(AndroidJavaObject status)
		{
		}

		public AdapterStatus getAdapterStatusForClassName(string className)
		{
			return null;
		}

		public Dictionary<string, AdapterStatus> getAdapterStatusMap()
		{
			return null;
		}

		private string[] getKeys()
		{
			return null;
		}
	}
}
