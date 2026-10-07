using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		[Serializable]
		public struct BloomSettings
		{
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			[Range(0f, 1f)]
			public float softKnee;

			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;

			public float thresholdLinear
			{
				get
				{
					return 0f;
				}
				set
				{
				}
			}

			public static BloomSettings defaultSettings => default;
		}

		[Serializable]
		public struct LensDirtSettings
		{
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			[Tooltip("Amount of lens dirtiness.")]
			[Min(0f)]
			public float intensity;

			public static LensDirtSettings defaultSettings => default;
		}

		[Serializable]
		public struct Settings
		{
			public BloomSettings bloom;

			public LensDirtSettings lensDirt;

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
