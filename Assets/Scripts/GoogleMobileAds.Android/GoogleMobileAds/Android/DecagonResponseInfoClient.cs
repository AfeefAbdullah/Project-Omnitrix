using System.Collections.Generic;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class DecagonResponseInfoClient : IResponseInfoClient
	{
		private AndroidJavaObject _androidResponseInfo;

		public DecagonResponseInfoClient(AndroidJavaObject androidJavaObject)
		{
		}

		public List<IAdapterResponseInfoClient> GetAdapterResponses()
		{
			return null;
		}

		public IAdapterResponseInfoClient GetLoadedAdapterResponseInfo()
		{
			return null;
		}

		public string GetMediationAdapterClassName()
		{
			return null;
		}

		public Dictionary<string, string> GetResponseExtras()
		{
			return null;
		}

		public string GetResponseId()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
