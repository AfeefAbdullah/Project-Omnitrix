using System;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface IBannerClient
	{
		long PlacementId { get; set; }

		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

		event EventHandler<EventArgs> OnAdOpening;

		event EventHandler<EventArgs> OnAdClosed;

		event Action<AdValue> OnPaidEvent;

		event Action OnAdClicked;

		event Action OnAdImpressionRecorded;

		void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position);

		void CreateBannerView(string adUnitId, AdSize adSize, int x, int y);

		void LoadAd(AdRequest request);

		void ShowBannerView();

		void HideBannerView();

		void DestroyBannerView();

		string GetAdUnitID();

		float GetHeightInPixels();

		float GetWidthInPixels();

		void SetPosition(AdPosition adPosition);

		void SetPosition(int x, int y);

		bool IsCollapsible();

		IResponseInfoClient GetResponseInfoClient();
	}
}
