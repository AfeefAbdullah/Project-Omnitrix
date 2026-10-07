using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		[Serializable]
		public struct DepthSettings
		{
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			[Range(0f, 1f)]
			public float scale;

			public static DepthSettings defaultSettings => default;
		}

		[Serializable]
		public struct MotionVectorsSettings
		{
			[Tooltip("Opacity of the source render.")]
			[Range(0f, 1f)]
			public float sourceOpacity;

			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			[Range(0f, 1f)]
			public float motionImageOpacity;

			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			[Min(0f)]
			public float motionImageAmplitude;

			[Tooltip("Opacity for the motion vector arrows.")]
			[Range(0f, 1f)]
			public float motionVectorsOpacity;

			[Tooltip("The arrow density on screen.")]
			[Range(8f, 64f)]
			public int motionVectorsResolution;

			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;

			public static MotionVectorsSettings defaultSettings => default;
		}

		public enum Mode
		{
			None = 0,
			Depth = 1,
			Normals = 2,
			MotionVectors = 3,
			AmbientOcclusion = 4,
			EyeAdaptation = 5,
			FocusPlane = 6,
			PreGradingLog = 7,
			LogLut = 8,
			UserLut = 9
		}

		[Serializable]
		public struct Settings
		{
			public Mode mode;

			public DepthSettings depth;

			public MotionVectorsSettings motionVectors;

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

		public bool willInterrupt => false;

		public override void Reset()
		{
		}

		public bool IsModeActive(Mode mode)
		{
			return false;
		}
	}
}
