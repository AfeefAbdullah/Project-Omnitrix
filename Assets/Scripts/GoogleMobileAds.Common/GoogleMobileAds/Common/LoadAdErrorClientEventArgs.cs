using System;

namespace GoogleMobileAds.Common
{
	public class LoadAdErrorClientEventArgs : EventArgs
	{
		public ILoadAdErrorClient LoadAdErrorClient { get; set; }
	}
}
