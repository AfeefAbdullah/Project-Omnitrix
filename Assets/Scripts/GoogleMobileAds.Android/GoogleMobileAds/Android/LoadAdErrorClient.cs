using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class LoadAdErrorClient : ILoadAdErrorClient, IAdErrorClient
	{
		private AndroidJavaObject loadAdError;

		public LoadAdErrorClient(AndroidJavaObject loadAdError)
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
