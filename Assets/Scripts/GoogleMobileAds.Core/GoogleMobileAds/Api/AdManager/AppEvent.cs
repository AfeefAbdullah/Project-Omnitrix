using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GoogleMobileAds.Api.AdManager
{
	public class AppEvent
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string Name__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string Data__BackingField;

		public string Name
		{
			[CompilerGenerated]
			set
			{
				Name__BackingField = value;
			}
		}

		public string Data
		{
			[CompilerGenerated]
			set
			{
				Data__BackingField = value;
			}
		}
	}
}
