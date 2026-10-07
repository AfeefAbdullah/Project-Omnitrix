using System;
using System.Collections.Generic;

namespace GoogleMobileAds.Api.AdManager
{
	[Serializable]
	public class AdManagerAdRequest : AdRequest
	{
		public string PublisherProvidedId;

		public HashSet<string> CategoryExclusions;
	}
}
