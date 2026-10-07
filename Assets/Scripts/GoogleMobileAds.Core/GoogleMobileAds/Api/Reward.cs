using System;

namespace GoogleMobileAds.Api
{
	[Serializable]
	public class Reward : EventArgs
	{
		public string Type;

		public double Amount;
	}
}
