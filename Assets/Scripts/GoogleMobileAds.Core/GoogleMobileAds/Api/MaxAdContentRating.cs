namespace GoogleMobileAds.Api
{
	public class MaxAdContentRating
	{
		public static MaxAdContentRating G => null;

		public static MaxAdContentRating PG => null;

		public static MaxAdContentRating T => null;

		public static MaxAdContentRating MA => null;

		public string Value { get; set; }

		private MaxAdContentRating(string value)
		{
		}

		public static MaxAdContentRating ToMaxAdContentRating(string value)
		{
			return null;
		}
	}
}
