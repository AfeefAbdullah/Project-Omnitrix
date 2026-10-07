using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class DecagonLoadAdErrorClient : ILoadAdErrorClient, IAdErrorClient
	{
		private AndroidJavaObject _loadAdError;

		public DecagonLoadAdErrorClient(AndroidJavaObject loadAdError)
		{
		}

		public int GetCode()
		{
			return 0;
		}

		public string GetDomain()
		{
			return null;
		}

		public string GetMessage()
		{
			return null;
		}

		public IAdErrorClient GetCause()
		{
			return null;
		}

		public IResponseInfoClient GetResponseInfoClient()
		{
			return null;
		}

		public override string ToString()
		{
			return null;
		}
	}
}
