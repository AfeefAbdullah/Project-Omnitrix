using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class RewardedInterstitialAdClient : AndroidJavaProxy, IRewardedInterstitialAdClient
	{
		private AndroidJavaObject androidRewardedInterstitialAd;

		private EventHandler<EventArgs> m_OnAdLoaded;

		private EventHandler<LoadAdErrorClientEventArgs> m_OnAdFailedToLoad;

		private EventHandler<Reward> m_OnUserEarnedReward;

		private Action<AdValue> m_OnPaidEvent;

		private EventHandler<AdErrorClientEventArgs> m_OnAdFailedToPresentFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidPresentFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidDismissFullScreenContent;

		private EventHandler<EventArgs> m_OnAdDidRecordImpression;

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

		public event EventHandler<Reward> OnUserEarnedReward
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

		public event Action OnAdClicked
		{
			add
			{
			}
			remove
			{
			}
		}

		public RewardedInterstitialAdClient()
			: base((string)null)
		{
		}

		public void CreateRewardedInterstitialAd()
		{
		}

		public void LoadAd(string adUnitId, AdRequest request)
		{
		}

		public void Show()
		{
		}

		public Reward GetRewardItem()
		{
			return null;
		}

		public void SetServerSideVerificationOptions(ServerSideVerificationOptions serverSideVerificationOptions)
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

		public void DestroyRewardedInterstitialAd()
		{
		}

		private void onRewardedInterstitialAdLoaded()
		{
		}

		private void onRewardedInterstitialAdFailedToLoad(AndroidJavaObject error)
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

		private void onUserEarnedReward(string type, float amount)
		{
		}

		public void onPaidEvent(int precision, long valueInMicros, string currencyCode)
		{
		}
	}
}
