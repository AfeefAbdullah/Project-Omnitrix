using System;

namespace GoogleMobileAds.Common
{
	public interface IAppStateEventClient
	{
		event Action<AppState> AppStateChanged;
	}
}
