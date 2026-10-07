using System;
using UnityEngine;

namespace GoogleMobileAds.Api
{
	[Serializable]
	public class NativeTemplateTextStyle
	{
		public Color BackgroundColor { get; }

		public Color TextColor { get; }

		public int FontSize { get; }

		public NativeTemplateFontStyle Style { get; }
	}
}
