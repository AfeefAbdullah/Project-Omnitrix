using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GoogleMobileAds.Common
{
	public class MobileAdsEventExecutor : MonoBehaviour
	{
		public static MobileAdsEventExecutor instance;

		private static List<Action> adEventsQueue;

		private static bool adEventsQueueEmpty;

		private static int UnityMainThreadId;

		public static void Initialize()
		{
		}

		public static bool IsOnMainThread()
		{
			return false;
		}

		public static bool IsActive()
		{
			return false;
		}

		public void Awake()
		{
		}

		public static void ExecuteInUpdate(Action action)
		{
		}

		public static void InvokeInUpdate(UnityEvent eventParam)
		{
		}

		public void Update()
		{
		}

		public void OnDisable()
		{
		}
	}
}
