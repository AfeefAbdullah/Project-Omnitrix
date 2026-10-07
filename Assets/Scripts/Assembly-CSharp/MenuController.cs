using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[Header("Variables")]
	public bool menuOpen;

	public int lockToDisableWatches;

	[Header("Panels")]
	public GameObject omnitrixPanel;

	public GameObject menuPanel;

	public GameObject adPanelWatches;

	public GameObject adPanelAnimation;

	public GameObject moreAppsPanel;

	[Header("OmnitrixPanelElements")]
	public Image background;

	public ScrollRect scrollRect;

	public GameObject[] locks;

	public GameObject transformAnimationLock;

	[Header("ExtraReferences")]
	public GameObject[] adMessages;

	public GameObject[] adMessageIconPacks;

	private RewardedAdsController adsController;

	[Header("UIelements")]
	public Color[] UIColors;

	public Button adButtonWatches;

	public GameObject loadingWheelWatches;

	public Button adButtonAnimation;

	public GameObject loadingWheelAnimation;

	public Button[] screenshotButtons;

	public Image[] menuButtonsBackgrounds;

	public Image[] menuButtonsIcons;

	public Image[] freeCamButtonsBackgrounds;

	public Image[] freeCamButtonsIcons;

	public Image[] themeButtonsBackgrounds;

	public Image[] themeButtonsIcons;

	public Image openingTransition;

	public Image endingTransition;

	public Image screenshotFlash;

	public GameObject savingScreenshotText;

	public GameObject[] portraitMenuElements;

	public GameObject[] landscapeMenuElements;

	public CanvasGroup[] screenshotHideElements;

	public GameObject[] extraLandscapeButtons;

	public RectTransform[] letterboxingPanels;

	public RectTransform[] letterboxPos0;

	public RectTransform[] letterboxPos1;

	public Color[] cameraBackgroundColors;

	private string[] lockPrefKeys = { "unlock0", "unlock1", "unlock2", "unlock3", "unlock4", "unlock5", "unlock6", "unlock7", "unlock8", "unlock9", "unlock10" };

	private ScreenOrientation currentScreenOrientation;

	private const string PackageName = "com.KokokatStudios.OmniWatch3D";

	private void Awake()
	{
		cameraBackgroundColors[0] = Camera.main.backgroundColor;
	}

	private void Start()
	{
		adsController = GameObject.Find("AdsController").GetComponent<RewardedAdsController>();
		SceneFadeIn();
		DisableLocksAsPrefs();
		UpdateMenuElements();
		UpdateBackgroundTheme();
		UpdateCurrentScenePref();
		IdController.Check();
		if (Application.identifier != PackageName)
		{
			Destroy(GameObject.Find("MobileParts"));
		}
	}

	private void Update()
	{
		CheckIfOrientationChanged();
		if (Input.GetKeyDown(KeyCode.Delete) && Application.isEditor)
		{
			PlayerPrefs.DeleteAll();
		}
	}

	public void ShowLetterboxing(bool show)
	{
		RectTransform[] targets = show ? letterboxPos1 : letterboxPos0;
		for (int i = 0; i < letterboxingPanels.Length; i++)
		{
			letterboxingPanels[i].DOMove(targets[i].position, 1f);
		}
	}

	public void OpenCloseMenu()
	{
		if (!menuPanel.activeInHierarchy)
		{
			menuOpen = true;
			SetActive(menuPanel, extraLandscapeButtons, true);
			scrollRect.horizontal = true;
		}
		else
		{
			SetActive(menuPanel, extraLandscapeButtons, false);
			scrollRect.horizontal = false;
			StartCoroutine(WaitOneFrame());
		}
		if (Application.identifier != PackageName)
		{
			GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = 179f;
		}
	}

	private static void SetActive(GameObject panel, GameObject[] extras, bool active)
	{
		panel.SetActive(active);
		foreach (GameObject extra in extras)
		{
			extra.SetActive(active);
		}
	}

	public void SwitchCameraButton()
	{
		menuOpen = true;
		GetComponent<FreeCameraController>().SwitchCameraMode();
		StartCoroutine(WaitOneFrame());
	}

	private IEnumerator WaitOneFrame()
	{
		yield return 0;
		menuOpen = false;
	}

	public void SwitchScene(string name)
	{
		endingTransition.gameObject.SetActive(true);
		endingTransition.DOFade(1f, 0.25f).OnComplete(() => SceneManager.LoadScene(name));
	}

	private void SceneFadeIn()
	{
		openingTransition.gameObject.SetActive(true);
		openingTransition.DOFade(0f, 0.5f).OnComplete(() => EnableDisablePanel(openingTransition.gameObject));
	}

	public void EnableDisablePanel(GameObject panel)
	{
		panel.SetActive(!panel.activeInHierarchy);
	}

	public void EnableAdPanel(string type)
	{
		if (type == "Watches")
		{
			adPanelWatches.SetActive(true);
		}
		else if (type == "Animation")
		{
			adPanelAnimation.SetActive(true);
		}
	}

	public void PrepareToUnlockWatches(int lockId)
	{
		lockToDisableWatches = lockId;
		adMessageIconPacks[lockId].SetActive(true);
	}

	public void SetUnlockPref(TipoRecompensa tipo)
	{
		if (tipo == TipoRecompensa.Animation)
		{
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "TransformAnimationUnlocked", 1);
		}
		else if (tipo == TipoRecompensa.Watches)
		{
			PlayerPrefs.SetInt("unlock" + lockToDisableWatches, 1);
		}
	}

	public void DisableLocksAsPrefs()
	{
		for (int i = 0; i < lockPrefKeys.Length; i++)
		{
			if (PlayerPrefs.HasKey(lockPrefKeys[i]))
			{
				locks[i].SetActive(false);
			}
		}
		if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "TransformAnimationUnlocked"))
		{
			transformAnimationLock.SetActive(false);
		}
		if (Application.identifier != PackageName)
		{
			Destroy(GameObject.Find("AudioManager"));
		}
	}

	public void EnableAdButton(TipoRecompensa tipo)
	{
		if (tipo == TipoRecompensa.Animation)
		{
			adButtonAnimation.interactable = true;
			loadingWheelAnimation.SetActive(false);
		}
		else if (tipo == TipoRecompensa.Watches)
		{
			adButtonWatches.interactable = true;
			loadingWheelWatches.SetActive(false);
		}
	}

	public void ShowAd(string tipo)
	{
		if (tipo == "Watches")
		{
			adsController.ShowRewardedAd(TipoRecompensa.Watches);
		}
		else if (tipo == "Animation")
		{
			adsController.ShowRewardedAd(TipoRecompensa.Animation);
		}
		else
		{
			Debug.LogError("Tipo de anuncio inválido.");
		}
	}

	public void DisableIconPacks()
	{
		foreach (GameObject iconPack in adMessageIconPacks)
		{
			iconPack.SetActive(false);
		}
	}

	public void ResetAdMessagePanel(TipoRecompensa tipo)
	{
		if (tipo == TipoRecompensa.Animation)
		{
			adPanelAnimation.SetActive(false);
			adButtonAnimation.interactable = false;
			loadingWheelAnimation.SetActive(true);
		}
		else if (tipo == TipoRecompensa.Watches)
		{
			adPanelWatches.SetActive(false);
			DisableIconPacks();
			adButtonWatches.interactable = false;
			loadingWheelWatches.SetActive(true);
		}
	}

	private void CheckIfOrientationChanged()
	{
		if (Screen.orientation == currentScreenOrientation)
		{
			return;
		}
		UpdateMenuElements();
		currentScreenOrientation = Screen.orientation;
		if (Application.identifier != PackageName)
		{
			Destroy(GameObject.Find("MobileParts"));
		}
	}

	private void UpdateMenuElements()
	{
		bool landscape = Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight;
		foreach (GameObject element in landscapeMenuElements)
		{
			element.SetActive(landscape);
		}
		foreach (GameObject element in portraitMenuElements)
		{
			element.SetActive(!landscape);
		}
	}

	public void ChangeThemeButtonsColors(bool lightTheme)
	{
		Color backgroundColor = lightTheme ? UIColors[1] : UIColors[0];
		Color iconColor = lightTheme ? UIColors[0] : UIColors[1];
		foreach (Image image in themeButtonsBackgrounds)
		{
			image.color = backgroundColor;
		}
		foreach (Image image in themeButtonsIcons)
		{
			image.color = iconColor;
		}
	}

	public void StartScreenshotSequence()
	{
		foreach (Button button in screenshotButtons)
		{
			button.interactable = false;
		}
		foreach (CanvasGroup group in screenshotHideElements)
		{
			group.alpha = 0f;
		}
		string filename = "OmniWatchScreenshot_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";
		ScreenCapture.CaptureScreenshot(filename);
		StartCoroutine(WaitToFlash());
		StartCoroutine(WaitToSaveToGallery(filename));
	}

	private IEnumerator WaitToFlash()
	{
		yield return new WaitForEndOfFrame();
		ScreenshotFlash();
	}

	private void ScreenshotFlash()
	{
		screenshotFlash.gameObject.SetActive(true);
		screenshotFlash.color = Color.white;
		screenshotFlash.DOColor(Color.clear, 0.25f).SetEase(Ease.Flash).OnComplete(EndScreenshotSequence);
	}

	private void EndScreenshotSequence()
	{
		screenshotFlash.gameObject.SetActive(false);
		savingScreenshotText.SetActive(true);
		foreach (CanvasGroup group in screenshotHideElements)
		{
			group.alpha = 1f;
		}
	}

	private IEnumerator WaitToSaveToGallery(string filename)
	{
		yield return new WaitForSeconds(2f);
		StartCoroutine(WaitForSavingImage());
		NativeGallery.SaveImageToGallery(Application.persistentDataPath + "/" + filename, "Omni-Watch", filename);
	}

	private IEnumerator WaitForSavingImage()
	{
		yield return new WaitForSeconds(2f);
		savingScreenshotText.SetActive(false);
		foreach (Button button in screenshotButtons)
		{
			button.interactable = true;
		}
	}

	private void UpdateBackgroundTheme()
	{
		if (!PlayerPrefs.HasKey("LightTheme"))
		{
			PlayerPrefs.SetInt("LightTheme", 0);
		}
		Camera.main.backgroundColor = cameraBackgroundColors[PlayerPrefs.GetInt("LightTheme") == 1 ? 1 : 0];
	}

	public void ChangeBackgroundThemeButton()
	{
		PlayerPrefs.SetInt("LightTheme", PlayerPrefs.GetInt("LightTheme") != 1 ? 1 : 0);
		UpdateBackgroundTheme();
	}

	private void UpdateCurrentScenePref()
	{
		PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
	}
}
