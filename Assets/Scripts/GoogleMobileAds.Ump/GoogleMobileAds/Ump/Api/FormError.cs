using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GoogleMobileAds.Ump.Api
{
	public class FormError
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int ErrorCode__BackingField;

		private int ErrorCode
		{
			[CompilerGenerated]
			set
			{
				ErrorCode__BackingField = value;
			}
		}

		public string Message { get; private set; }

		internal FormError(int errorCode, string message)
		{
		}
	}
}
