using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	internal class AdInspectorErrorClient : AdErrorClient, IAdInspectorErrorClient, IAdErrorClient
	{
		public AdInspectorErrorClient(AndroidJavaObject error)
			: base(null)
		{
		}
	}
}
