using System;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public abstract class PostProcessingModel
	{
		[GetSet("enabled")]
		[SerializeField]
		private bool m_Enabled;

		public bool enabled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public abstract void Reset();

		public virtual void OnValidate()
		{
		}
	}
}
