using System;
using UnityEngine;
using UnityEngine.UI;

public class COPPASetter : MonoBehaviour
{
	public Button button;

	public Dropdown dropdownDay;

	public Dropdown dropdownMonth;

	public Dropdown dropdownYear;

	public GameObject endingTransition;

	private void Start()
	{
		if (!PlayerPrefs.HasKey("UserIsChild"))
		{
			return;
		}
		Debug.Log("UserAge Prefs found");
		if (PlayerPrefs.HasKey("CurrentScene"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentScene"));
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Prototype");
		}
	}

	private void CheckAge()
	{
		try
		{
			DateTime now = DateTime.Now;
			int age = now.Year - Selected(dropdownYear);
			int month = Selected(dropdownMonth);
			// Adult once the 13th birthday has passed.
			bool adult = age > 13 || (age == 13 && (month < now.Month || (month == now.Month && Selected(dropdownDay) <= now.Day)));
			Debug.Log(adult ? "Mayor" : "Menor");
			PlayerPrefs.SetString("UserIsChild", adult ? "false" : "true");
		}
		catch (Exception)
		{
		}
	}

	private static int Selected(Dropdown dropdown)
	{
		return int.Parse(dropdown.options[dropdown.value].text);
	}

	public void EnableContinueButton()
	{
		button.interactable = true;
	}

	public void ContinueButton()
	{
		CheckAge();
		endingTransition.SetActive(true);
	}
}
