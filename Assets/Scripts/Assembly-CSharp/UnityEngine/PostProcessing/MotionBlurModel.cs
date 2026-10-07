using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		[Serializable]
		public struct Settings
		{
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			[Tooltip("The amount of sample points, which affects quality and performances.")]
			[Range(4f, 32f)]
			public int sampleCount;

			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			[Range(0f, 1f)]
			public float frameBlending;

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
