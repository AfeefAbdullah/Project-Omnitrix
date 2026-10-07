namespace GoogleMobileAds.Ump.Common
{
	public interface IUmpClientFactory
	{
		IConsentFormClient ConsentFormClient();

		IConsentInformationClient ConsentInformationClient();
	}
}
