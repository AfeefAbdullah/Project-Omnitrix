using System;
using System.Runtime.InteropServices;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		[Serializable]
		[StructLayout((LayoutKind)0, Size = 1)]
		public struct Settings
		{
			public static Settings defaultSettings => default;
		}

		[SerializeField]
		private Settings m_Settings;

		public Settings settings
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public override void Reset()
		{
		}
	}
}
