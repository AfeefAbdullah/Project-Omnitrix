using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GoogleMobileAds.Api
{
	public class AdValue
	{
		public enum PrecisionType
		{
			Unknown = 0,
			Estimated = 1,
			PublisherProvided = 2,
			Precise = 3
		}

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PrecisionType Precision__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long Value__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string CurrencyCode__BackingField;

		public PrecisionType Precision
		{
			[CompilerGenerated]
			set
			{
				Precision__BackingField = value;
			}
		}

		public long Value
		{
			[CompilerGenerated]
			set
			{
				Value__BackingField = value;
			}
		}

		public string CurrencyCode
		{
			[CompilerGenerated]
			set
			{
				CurrencyCode__BackingField = value;
			}
		}
	}
}
