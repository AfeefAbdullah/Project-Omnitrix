using System;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
	public class RewardedAd
	{
		private Action<AdValue> OnAdPaid;

		private Action OnAdClicked;

		private Action OnAdImpressionRecorded;

		private Action OnAdFullScreenContentOpened;

		private Action m_OnAdFullScreenContentClosed;

		private Action<AdError> m_OnAdFullScreenContentFailed;

		private IRewardedAdClient _client;

		private bool _canShowAd;

		private Action<Reward> _userRewardEarnedCallback;

		public event Action OnAdFullScreenContentClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<AdError> OnAdFullScreenContentFailed
		{
			add
			{
			}
			remove
			{
			}
		}

		internal RewardedAd(IRewardedAdClient client)
		{
		}

		public static void Load(string adUnitId, AdRequest request, Action<RewardedAd, LoadAdError> adLoadCallback)
		{
		}

		public bool CanShowAd()
		{
			return false;
		}

		public void Show(Action<Reward> userRewardEarnedCallback)
		{
		}

		public void Destroy()
		{
		}

		private void RegisterAdEvents()
		{
		}
	}
}
