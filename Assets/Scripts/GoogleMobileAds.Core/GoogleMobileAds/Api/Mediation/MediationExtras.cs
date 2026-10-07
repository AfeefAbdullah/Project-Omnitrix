using System.Collections.Generic;

namespace GoogleMobileAds.Api.Mediation
{
	public abstract class MediationExtras
	{
		public Dictionary<string, string> Extras { get; }

		public abstract string AndroidMediationExtraBuilderClassName { get; }
	}
}
