using System;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface IRewardedInterstitialAdClient
	{
		long PlacementId { get; set; }

		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

		event Action<AdValue> OnPaidEvent;

		event EventHandler<Reward> OnUserEarnedReward;

		event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent;

		event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

		event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

		event EventHandler<EventArgs> OnAdDidRecordImpression;

		event Action OnAdClicked;

		void CreateRewardedInterstitialAd();

		void LoadAd(string adUnitID, AdRequest request);

		Reward GetRewardItem();

		void Show();

		string GetAdUnitID();

		void SetServerSideVerificationOptions(ServerSideVerificationOptions serverSideVerificationOptions);

		IResponseInfoClient GetResponseInfoClient();

		void DestroyRewardedInterstitialAd();
	}
}
