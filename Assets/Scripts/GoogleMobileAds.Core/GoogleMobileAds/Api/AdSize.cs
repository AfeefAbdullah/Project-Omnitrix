using System;

namespace GoogleMobileAds.Api
{
	public class AdSize
	{
		public enum Type
		{
			Standard = 0,
			[Obsolete("Deprecated. Use AnchoredAdaptive.")]
			SmartBanner = 1,
			AnchoredAdaptive = 2
		}

		private Type _type;

		private Orientation _orientation;

		private int _width;

		private int _height;

		public static readonly AdSize Banner;

		public static readonly AdSize MediumRectangle;

		public static readonly AdSize IABBanner;

		public static readonly AdSize Leaderboard;

		[Obsolete("Deprecated. Use AnchoredAdaptive.")]
		public static readonly AdSize SmartBanner;

		public static readonly int FullWidth;

		public int Width => 0;

		public int Height => 0;

		public Type AdType => Type.Standard;

		internal Orientation Orientation => Orientation.Current;

		public AdSize(int width, int height)
		{
		}

		private AdSize(int width, int height, Type type)
		{
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}
