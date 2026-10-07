using UnityEngine;
using UnityEngine.SceneManagement;

// ponytail: no AdMob. Every "ad" is instantly ready and always rewards; restore the real
// load/show flow (pseudocode in Il2CppDump/work/rewarded.txt) once the GoogleMobileAds SDK replaces the stub.
public class RewardedAdsController : MonoBehaviour
{
	private MenuController menuController;

	public static RewardedAdsController Instance;

	public bool isAdMobInitialized;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		GameObject controllers = GameObject.Find("Controllers");
		if (controllers != null)
		{
			menuController = controllers.GetComponent<MenuController>();
			VerificarYActivarBotones();
		}
	}

	private void VerificarYActivarBotones()
	{
		if (menuController == null)
		{
			return;
		}
		menuController.EnableAdButton(TipoRecompensa.Watches);
		menuController.EnableAdButton(TipoRecompensa.Animation);
	}

	public void ShowRewardedAd(TipoRecompensa tipo)
	{
		Debug.Log("Video completado. Entregando recompensa: " + tipo);
		if (menuController == null)
		{
			return;
		}
		menuController.SetUnlockPref(tipo);
		menuController.DisableLocksAsPrefs();
		// What the ad-closed handler did, then the next ad is "loaded" immediately.
		menuController.ResetAdMessagePanel(tipo);
		menuController.EnableAdButton(tipo);
	}
}
