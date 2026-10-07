using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class EyeAdaptationModel : PostProcessingModel
	{
		public enum EyeAdaptationType
		{
			Progressive = 0,
			Fixed = 1
		}

		[Serializable]
		public struct Settings
		{
			[Tooltip("Filters the dark part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			[Range(1f, 99f)]
			public float lowPercent;

			[Tooltip("Filters the bright part of the histogram when computing the average luminance to avoid very dark pixels from contributing to the auto exposure. Unit is in percent.")]
			[Range(1f, 99f)]
			public float highPercent;

			[Tooltip("Minimum average luminance to consider for auto exposure (in EV).")]
			public float minLuminance;

			[Tooltip("Maximum average luminance to consider for auto exposure (in EV).")]
			public float maxLuminance;

			[Tooltip("Exposure bias. Use this to offset the global exposure of the scene.")]
			[Min(0f)]
			public float keyValue;

			[Tooltip("Set this to true to let Unity handle the key value automatically based on average luminance.")]
			public bool dynamicKeyValue;

			[Tooltip("Use \"Progressive\" if you want the auto exposure to be animated. Use \"Fixed\" otherwise.")]
			public EyeAdaptationType adaptationType;

			[Min(0f)]
			[Tooltip("Adaptation speed from a dark to a light environment.")]
			public float speedUp;

			[Min(0f)]
			[Tooltip("Adaptation speed from a light to a dark environment.")]
			public float speedDown;

			[Range(-16f, -1f)]
			[Tooltip("Lower bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			public int logMin;

			[Tooltip("Upper bound for the brightness range of the generated histogram (in EV). The bigger the spread between min & max, the lower the precision will be.")]
			[Range(1f, 16f)]
			public int logMax;

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
