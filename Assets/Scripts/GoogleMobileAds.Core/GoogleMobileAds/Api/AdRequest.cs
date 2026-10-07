using System;
using System.Collections.Generic;
using GoogleMobileAds.Api.Mediation;

namespace GoogleMobileAds.Api
{
	[Serializable]
	public class AdRequest
	{
		public Dictionary<string, string> CustomTargeting;

		public HashSet<string> Keywords;

		public Dictionary<string, string> Extras;

		public long PlacementID;

		public List<MediationExtras> MediationExtras;

		public static string Version { get; private set; }

		static AdRequest()
		{
		}

		internal static string BuildVersionString(string nativePluginVersion = null)
		{
			return null;
		}
	}
}
