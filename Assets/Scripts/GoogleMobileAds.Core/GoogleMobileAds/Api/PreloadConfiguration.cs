using System;

namespace GoogleMobileAds.Api
{
	[Serializable]
	public sealed class PreloadConfiguration
	{
		public string AdUnitId;

		[Obsolete]
		public AdFormat Format;

		public AdRequest Request;

		public uint BufferSize;
	}
}
