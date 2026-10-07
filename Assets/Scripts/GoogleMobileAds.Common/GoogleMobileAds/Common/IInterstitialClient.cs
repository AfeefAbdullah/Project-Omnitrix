using System;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface IInterstitialClient
	{
		long PlacementId { get; set; }

		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

		event Action<AdValue> OnPaidEvent;

		event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent;

		event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

		event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

		event EventHandler<EventArgs> OnAdDidRecordImpression;

		event Action OnAdClicked;

		void CreateInterstitialAd();

		bool IsAdAvailable(string adUnitId);

		IInterstitialClient PollAd(string adUnitId);

		void LoadAd(string adUnitID, AdRequest request);

		void Show();

		string GetAdUnitID();

		IResponseInfoClient GetResponseInfoClient();

		void DestroyInterstitial();
	}
}
