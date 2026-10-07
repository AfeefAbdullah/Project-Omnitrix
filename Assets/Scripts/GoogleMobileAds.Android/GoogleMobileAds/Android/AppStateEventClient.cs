using System;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
	public class AppStateEventClient : AndroidJavaProxy, IAppStateEventClient
	{
		private Action<AppState> m_appStateChanged;

		private AndroidJavaObject appStateEventNotifer;

		private event Action<AppState> appStateChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		public event Action<AppState> AppStateChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		public AppStateEventClient()
			: base((string)null)
		{
		}

		private void onAppStateChanged(bool isBackground)
		{
		}
	}
}
