using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class DecagonBannerAdClient : AndroidJavaProxy, IBannerClient
	{
		protected internal AndroidJavaObject bannerView;

		private string adUnitId;

		private AdSize adSize;

		private EventHandler<EventArgs> m_OnAdLoaded;

		private EventHandler<LoadAdErrorClientEventArgs> m_OnAdFailedToLoad;

		private EventHandler<EventArgs> m_OnAdOpening;

		private EventHandler<EventArgs> m_OnAdClosed;

		private Action<AdValue> m_OnPaidEvent;

		private Action m_OnAdClicked;

		private Action m_OnAdImpressionRecorded;

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

		public event EventHandler<EventArgs> OnAdOpening
		{
			add
			{
			}
			remove
			{
			}
		}

		public event EventHandler<EventArgs> OnAdClosed
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

		public event Action OnAdClicked
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action OnAdImpressionRecorded
		{
			add
			{
			}
			remove
			{
			}
		}

		protected internal DecagonBannerAdClient(string className)
			: base((string)null)
		{
		}

		public DecagonBannerAdClient()
			: base((string)null)
		{
		}

		public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
		{
		}

		public void CreateBannerView(string adUnitId, AdSize adSize, int x, int y)
		{
		}

		public virtual void LoadAd(AdRequest request)
		{
		}

		public void ShowBannerView()
		{
		}

		public void HideBannerView()
		{
		}

		public void DestroyBannerView()
		{
		}

		public string GetAdUnitID()
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

		public void SetPosition(AdPosition adPosition)
		{
		}

		public void SetPosition(int x, int y)
		{
		}

		public bool IsCollapsible()
		{
			return false;
		}

		public IResponseInfoClient GetResponseInfoClient()
		{
			return null;
		}

		public void onAdLoaded()
		{
		}

		public void onAdFailedToLoad(AndroidJavaObject error)
		{
		}

		public void onAdOpened()
		{
		}

		public void onAdClosed()
		{
		}

		public void onPaidEvent(int precision, long valueInMicros, string currencyCode)
		{
		}

		internal void onAdClicked()
		{
		}

		internal void onAdImpression()
		{
		}
	}
}
