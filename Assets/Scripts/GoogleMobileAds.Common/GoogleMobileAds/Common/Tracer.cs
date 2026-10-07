using System;

namespace GoogleMobileAds.Common
{
	public class Tracer : ITracer
	{
		private class NoOpTrace : ITrace, IDisposable
		{
			public void Dispose()
			{
			}
		}

		private static readonly NoOpTrace _noOpTrace;

		public Tracer(IInsightsEmitter insightsEmitter)
		{
		}

		public ITrace StartTrace(string name)
		{
			return null;
		}
	}
}
