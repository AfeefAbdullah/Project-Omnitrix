using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GoogleMobileAds.Api
{
	public class AdapterStatus
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AdapterState InitializationState__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string Description__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int Latency__BackingField;

		private AdapterState InitializationState
		{
			[CompilerGenerated]
			set
			{
				InitializationState__BackingField = value;
			}
		}

		private string Description
		{
			[CompilerGenerated]
			set
			{
				Description__BackingField = value;
			}
		}

		private int Latency
		{
			[CompilerGenerated]
			set
			{
				Latency__BackingField = value;
			}
		}

		internal AdapterStatus(AdapterState state, string description, int latency)
		{
		}
	}
}
