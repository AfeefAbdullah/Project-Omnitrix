using System;

namespace GoogleMobileAds.Common
{
	public class AdInspectorErrorClientEventArgs : EventArgs
	{
		public IAdInspectorErrorClient AdErrorClient { get; set; }
	}
}
