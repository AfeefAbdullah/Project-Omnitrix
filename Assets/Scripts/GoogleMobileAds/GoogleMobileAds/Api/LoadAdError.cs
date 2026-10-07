using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class LoadAdError : AdError
	{
		private ILoadAdErrorClient _client;

		public LoadAdError(ILoadAdErrorClient client)
			: base(null)
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
