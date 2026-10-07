using System;

namespace GoogleMobileAds.Common
{
	public class AdErrorClientEventArgs : EventArgs
	{
		public IAdErrorClient AdErrorClient { get; set; }
	}
}
