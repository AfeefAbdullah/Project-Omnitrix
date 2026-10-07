namespace GoogleMobileAds.Common
{
	public interface ILoadAdErrorClient : IAdErrorClient
	{
		IResponseInfoClient GetResponseInfoClient();
	}
}
