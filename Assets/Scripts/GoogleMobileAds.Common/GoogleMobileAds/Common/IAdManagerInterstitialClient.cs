using System;
using GoogleMobileAds.Api.AdManager;

namespace GoogleMobileAds.Common
{
	public interface IAdManagerInterstitialClient : IInterstitialClient
	{
		event Action<AppEvent> OnAppEvent;

		IAdManagerInterstitialClient PollAdManagerAd(string adUnitId);
	}
}
