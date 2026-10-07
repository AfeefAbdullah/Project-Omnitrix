using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
	[Header("UI Toggles")]
	public Toggle postProcessingToggle;

	public Toggle ultimateAnimationToggle;

	public Toggle omniKixAnimationToggle;

	public Toggle omniEnhancedAnimationToggle;

	public Toggle transformAnimationToggle;

	[Header("ToggleEvents")]
	public BoolEvent togglePostProcessing;

	public BoolEvent toggleUltimateAnimation;

	public BoolEvent toggleOmniKixAnimation;

	public BoolEvent toggleOmniEnhancedAnimation;

	public BoolEvent toggleTransformAnimation;

	private static string TransformAnimKey => SceneManager.GetActiveScene().name + "TransformAnim";

	private void Start()
	{
		foreach (string key in new[] { "ultimateAnim", "omniKixAnim", "omniEnhancedAnim" })
		{
			if (!PlayerPrefs.HasKey(key))
			{
				PlayerPrefs.SetInt(key, 1);
			}
		}
		if (!PlayerPrefs.HasKey(TransformAnimKey))
		{
			PlayerPrefs.SetInt(TransformAnimKey, 0);
		}
		UpdateToggles();
	}

	private void UpdateToggles()
	{
		ultimateAnimationToggle.isOn = PlayerPrefs.GetInt("ultimateAnim") != 0;
		omniKixAnimationToggle.isOn = PlayerPrefs.GetInt("omniKixAnim") != 0;
		omniEnhancedAnimationToggle.isOn = PlayerPrefs.GetInt("omniEnhancedAnim") != 0;
		transformAnimationToggle.isOn = PlayerPrefs.GetInt(TransformAnimKey) != 0;
	}

	public void TogglePostProcessing(bool newState)
	{
		togglePostProcessing.Invoke(newState);
	}

	public void ToggleUltimateAnimation(bool newState)
	{
		toggleUltimateAnimation.Invoke(newState);
		PlayerPrefs.SetInt("ultimateAnim", newState ? 1 : 0);
	}

	public void ToggleOmniKixAnimation(bool newState)
	{
		toggleOmniKixAnimation.Invoke(newState);
		PlayerPrefs.SetInt("omniKixAnim", newState ? 1 : 0);
	}

	public void ToggleOmniEnhancedAnimation(bool newState)
	{
		toggleOmniEnhancedAnimation.Invoke(newState);
		PlayerPrefs.SetInt("omniEnhancedAnim", newState ? 1 : 0);
	}

	public void ToggleTransformAnimation(bool newState)
	{
		toggleTransformAnimation.Invoke(newState);
		PlayerPrefs.SetInt(TransformAnimKey, newState ? 1 : 0);
	}
}
