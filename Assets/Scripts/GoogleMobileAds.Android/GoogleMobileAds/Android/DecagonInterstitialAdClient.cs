using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class DecagonInterstitialAdClient : AndroidJavaProxy, IInterstitialClient
	{
		internal AndroidJavaObject _androidInterstitialAd;

		private EventHandler<EventArgs> m_OnAdLoaded;

		private EventHandler<LoadAdErrorClientEventArgs> m_OnAdFailedToLoad;

		private EventHandler<AdErrorClientEventArgs> m_OnAdFailedToPresentFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidPresentFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidDismissFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidRecordImpression;

		private Action<AdValue> m_OnPaidEvent;

		private Action m_OnAdClicked;

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

		public event EventHandler<AdErrorClientEventArgs> OnAdFailedToPresentFullScreenContent
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

		public event EventHandler<EventArgs> OnAdDidRecordImpression
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

		public DecagonInterstitialAdClient()
			: base((string)null)
		{
		}

		public void CreateInterstitialAd()
		{
		}

		public void LoadAd(string adUnitId, AdRequest request)
		{
		}

		public void Show()
		{
		}

		public void DestroyInterstitial()
		{
		}

		public string GetAdUnitID()
		{
			return null;
		}

		public IResponseInfoClient GetResponseInfoClient()
		{
			return null;
		}

		public bool IsAdAvailable(string adUnitId)
		{
			return false;
		}

		public IInterstitialClient PollAd(string adUnitId)
		{
			return null;
		}

		public void onInterstitialAdLoaded()
		{
		}

		public void onInterstitialAdFailedToLoad(AndroidJavaObject error)
		{
		}

		private void onAdFailedToShowFullScreenContent(AndroidJavaObject error)
		{
		}

		private void onAdShowedFullScreenContent()
		{
		}

		private void onAdDismissedFullScreenContent()
		{
		}

		private void onAdImpression()
		{
		}

		internal void onAdClicked()
		{
		}

		private void onPaidEvent(int precision, long valueInMicros, string currencyCode)
		{
		}
	}
}
