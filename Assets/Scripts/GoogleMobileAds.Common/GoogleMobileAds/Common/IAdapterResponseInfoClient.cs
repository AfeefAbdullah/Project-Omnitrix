using System.Collections.Generic;

namespace GoogleMobileAds.Common
{
	public interface IAdapterResponseInfoClient
	{
		string AdapterClassName { get; }

		IAdErrorClient AdError { get; }

		string AdSourceId { get; }

		string AdSourceName { get; }

		string AdSourceInstanceId { get; }

		string AdSourceInstanceName { get; }

		Dictionary<string, string> AdUnitMapping { get; }

		long LatencyMillis { get; }
	}
}
