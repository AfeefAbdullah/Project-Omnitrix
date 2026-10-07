using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OpeningTransition : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Image>().color = Color.black;
		StartCoroutine(waitToStartTransition());
	}

	private IEnumerator waitToStartTransition()
	{
		yield return new WaitForSeconds(1.5f);
		startTransition();
	}

	private void startTransition()
	{
		GetComponent<Image>().DOFade(0f, 0.5f).OnComplete(disableGameObject);
	}

	private void disableGameObject()
	{
		gameObject.SetActive(false);
	}
}
