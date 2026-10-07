using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class NativeOverlayAdClient : AndroidJavaProxy, INativeOverlayAdClient
	{
		private AndroidJavaObject nativeOverlayAd;

		private EventHandler<EventArgs> m_OnAdLoaded;

		private EventHandler<LoadAdErrorClientEventArgs> m_OnAdFailedToLoad;

		private EventHandler<EventArgs> m_OnAdDidRecordImpression;

		private EventHandler<EventArgs> m_OnAdDidPresentFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidDismissFullScreenContent;

		private Action m_OnAdClicked;

		private Action<AdValue> m_OnPaidEvent;

		public long PlacementId
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		public event EventHandler<EventArgs> OnAdLoaded
		{
			add
			{
			}
			remove
			{
			}
		}

		public event EventHandler<LoadAdErrorClientEventArgs> OnAdFailedToLoad
		{
			add
			{
			}
			remove
			{
			}
		}

		public event EventHandler<EventArgs> OnAdDidRecordImpression
		{
			add
			{
			}
			remove
			{
			}
		}

		public event EventHandler<EventArgs> OnAdDidPresentFullScreenContent
		{
			add
			{
			}
			remove
			{
			}
		}

		public event EventHandler<EventArgs> OnAdDidDismissFullScreenContent
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnAdClicked
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<AdValue> OnPaidEvent
		{
			add
			{
			}
			remove
			{
			}
		}

		public NativeOverlayAdClient()
			: base((string)null)
		{
		}

		public void Load(string adUnitID, AdRequest request, NativeAdOptions options)
		{
		}

		public void Hide()
		{
		}

		public void Show()
		{
		}

		public void SetPosition(AdPosition position)
		{
		}

		public void SetPosition(int x, int y)
		{
		}

		public void Render(NativeTemplateStyle templateViewStyle, AdSize adSize, AdPosition adPosition)
		{
		}

		public void Render(NativeTemplateStyle templateViewStyle, AdSize adSize, int x, int y)
		{
		}

		public void Render(NativeTemplateStyle templateViewStyle, AdPosition adPosition)
		{
		}

		public void Render(NativeTemplateStyle templateViewStyle, int x, int y)
		{
		}

		public void DestroyAd()
		{
		}

		public IResponseInfoClient GetResponseInfoClient()
		{
			return null;
		}

		public float GetHeightInPixels()
		{
			return 0f;
		}

		public float GetWidthInPixels()
		{
			return 0f;
		}

		private void onNativeAdLoaded()
		{
		}

		private void onNativeAdFailedToLoad(AndroidJavaObject error)
		{
		}

		private void onAdImpression()
		{
		}

		private void onAdClicked()
		{
		}

		private void onAdShowedFullScreenContent()
		{
		}

		private void onAdDismissedFullScreenContent()
		{
		}

		private void onPaidEvent(int precision, long valueInMicros, string currencyCode)
		{
		}

		private AndroidJavaObject GetNativeTemplateStyleJavaObject(NativeTemplateStyle tmplStyle)
		{
			return null;
		}

		private AndroidJavaObject GetNativeTemplateTextStyleJavaObject(NativeTemplateTextStyle text)
		{
			return null;
		}
	}
}
