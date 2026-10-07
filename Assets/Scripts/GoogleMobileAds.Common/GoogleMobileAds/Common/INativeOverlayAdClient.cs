using System;
using GoogleMobileAds.Api;

namespace GoogleMobileAds.Common
{
	public interface INativeOverlayAdClient
	{
		long PlacementId { get; set; }

		event EventHandler<EventArgs> OnAdLoaded;

		event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad;

		event EventHandler<EventArgs> OnAdDidRecordImpression;

		event EventHandler<EventArgs> OnAdDidPresentFullScreenContent;

		event EventHandler<EventArgs> OnAdDidDismissFullScreenContent;

		event Action<AdValue> OnPaidEvent;

		event Action OnAdClicked;

		void Load(string adUnitId, AdRequest request, NativeAdOptions nativeOptions);

		void Render(NativeTemplateStyle templateViewStyle, AdSize adSize, AdPosition adPosition);

		void Render(NativeTemplateStyle templateViewStyle, AdSize adSize, int x, int y);

		void Render(NativeTemplateStyle templateViewStyle, AdPosition adPosition);

		void Render(NativeTemplateStyle templateViewStyle, int x, int y);

		void Hide();

		void Show();

		void SetPosition(AdPosition adPosition);

		void SetPosition(int x, int y);

		void DestroyAd();

		float GetHeightInPixels();

		float GetWidthInPixels();

		IResponseInfoClient GetResponseInfoClient();
	}
}
