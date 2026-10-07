using System;
using System.Collections.Generic;

namespace GoogleMobileAds.Ump.Api
{
	[Serializable]
	public class ConsentDebugSettings
	{
		public DebugGeography DebugGeography;

		public List<string> TestDeviceHashedIds;
	}
}
