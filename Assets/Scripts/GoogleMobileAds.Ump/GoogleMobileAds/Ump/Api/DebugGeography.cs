using System;

namespace GoogleMobileAds.Ump.Api
{
	public enum DebugGeography
	{
		Disabled = 0,
		EEA = 1,
		[Obsolete("Deprecated. Use DebugGeography.Other instead.")]
		NotEEA = 2,
		RegulatedUSState = 3,
		Other = 4
	}
}
