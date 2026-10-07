using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class AdErrorClient : IAdErrorClient
	{
		private AndroidJavaObject error;

		public AdErrorClient(AndroidJavaObject error)
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

		public override string ToString()
		{
			return null;
		}
	}
}
