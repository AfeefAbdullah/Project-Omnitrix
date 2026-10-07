using UnityEngine;
using UnityEngine.UI;

public class QualityController : MonoBehaviour
{
	public GameObject postProcessingObj;

	public Toggle postProcessingToggle;

	public Dropdown qualityDropdown;

	private void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		if (!PlayerPrefs.HasKey("PostProcessing"))
		{
			PlayerPrefs.SetInt("PostProcessing", 0);
		}
		if (!PlayerPrefs.HasKey("QualityLevel"))
		{
			PlayerPrefs.SetInt("QualityLevel", 1);
		}
		UpdateUISettings();
	}

	private void UpdateQualitySettings()
	{
		postProcessingObj.SetActive(PlayerPrefs.GetInt("PostProcessing") == 1);
		QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
	}

	private void UpdateUISettings()
	{
		postProcessingToggle.isOn = PlayerPrefs.GetInt("PostProcessing") == 1;
		qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel");
	}

	public void QualityDropdown()
	{
		PlayerPrefs.SetInt("QualityLevel", qualityDropdown.value);
		UpdateQualitySettings();
	}

	public void TogglePostProcessing(bool newState)
	{
		PlayerPrefs.SetInt("PostProcessing", newState ? 1 : 0);
		UpdateQualitySettings();
	}
}
