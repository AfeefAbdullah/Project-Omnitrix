using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Android
{
	public class AdManagerBannerClient : BannerClient, IAdManagerBannerClient, IBannerClient
	{
		private Action<AppEvent> m_OnAppEvent;

		private List<AdSize> _validAdSizes;

		private bool refreshValidAdSizes;

		public List<AdSize> ValidAdSizes
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public event Action<AppEvent> OnAppEvent
		{
			add
			{
			}
			remove
			{
			}
		}

		public AdManagerBannerClient()
			: base(null)
		{
		}

		public override void LoadAd(AdRequest request)
		{
		}

		public void onAppEvent(string name, string data)
		{
		}
	}
}
