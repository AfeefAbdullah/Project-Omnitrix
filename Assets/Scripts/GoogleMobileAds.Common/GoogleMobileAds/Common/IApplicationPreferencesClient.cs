namespace GoogleMobileAds.Common
{
	public interface IApplicationPreferencesClient
	{
		void SetInt(string key, int value);

		void SetString(string key, string value);

		int GetInt(string key);

		string GetString(string key);
	}
}
