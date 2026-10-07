using System.Collections.Generic;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class AdapterResponseInfoClient : IAdapterResponseInfoClient
	{
		private AndroidJavaObject _adapterResponseInfo;

		public string AdapterClassName => null;

		public string AdSourceId => null;

		public string AdSourceName => null;

		public string AdSourceInstanceId => null;

		public string AdSourceInstanceName => null;

		public long LatencyMillis => 0L;

		public Dictionary<string, string> AdUnitMapping => null;

		public IAdErrorClient AdError => null;

		public AdapterResponseInfoClient(AndroidJavaObject adapterResponseInfo)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
