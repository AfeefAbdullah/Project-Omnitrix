using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.AdManager;

namespace GoogleMobileAds.Common
{
	public interface IAdManagerBannerClient : IBannerClient
	{
		List<AdSize> ValidAdSizes { get; set; }

		event Action<AppEvent> OnAppEvent;
	}
}
