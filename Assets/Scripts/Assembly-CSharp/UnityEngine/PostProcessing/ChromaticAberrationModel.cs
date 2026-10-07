using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		[Serializable]
		public struct Settings
		{
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			[Tooltip("Amount of tangential distortion.")]
			[Range(0f, 1f)]
			public float intensity;

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
