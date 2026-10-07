using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingTransition : MonoBehaviour
{
	public string sceneName;

	public bool checkForContextualAds;

	private void OnEnable()
	{
		GetComponent<Image>().DOFade(1f, 0.25f).OnComplete(changeScene);
	}

	private void changeScene()
	{
		SceneManager.LoadScene(!checkForContextualAds || PlayerPrefs.HasKey("ContextualAds") ? sceneName : "COPPAScreen");
	}
}
