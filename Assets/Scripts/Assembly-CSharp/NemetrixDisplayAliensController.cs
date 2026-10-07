using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NemetrixDisplayAliensController : MonoBehaviour
{
	public GameObject[] aliensModels;

	public GameObject currentAlien;

	public GameObject aliensGroup;

	public int currentAlienIndex;

	private NemetrixAnimationsController nemetrixAnimationsController;

	private void Start()
	{
		nemetrixAnimationsController = GetComponent<NemetrixAnimationsController>();
		LevitateDown();
	}

	public void DisplayAliens()
	{
		BlinkAlienModel(false);
	}

	private void BlinkAlienModel(bool isSwitching)
	{
		InvokeRepeating("OnOffModel", 0f, 0.05f);
		StartCoroutine(WaitToStopBlink(isSwitching));
	}

	private void OnOffModel()
	{
		currentAlien.SetActive(!currentAlien.activeInHierarchy);
	}

	private IEnumerator WaitToStopBlink(bool isSwitching)
	{
		yield return new WaitForSeconds(0.15f);
		CancelInvoke("OnOffModel");
		if (isSwitching)
		{
			currentAlien.SetActive(false);
			yield break;
		}
		currentAlien.SetActive(true);
		nemetrixAnimationsController.EnableControls(true);
	}

	public void SwitchAlien(string direction)
	{
		BlinkAlienModel(true);
		FindObjectOfType<AudioManager>().Play("DisplayAlien");
		StartCoroutine(SwitchAlienA(direction));
	}

	private IEnumerator SwitchAlienA(string direction)
	{
		yield return new WaitForSeconds(0.5f);
		if (direction == "left")
		{
			SwitchAlienLeft();
		}
		else if (direction == "right")
		{
			SwitchAlienRight();
		}
		BlinkAlienModel(false);
		FindObjectOfType<AudioManager>().Play("DisplayAlien2");
	}

	public void SwitchAlienLeft()
	{
		if (currentAlienIndex < 0)
		{
			return;
		}
		currentAlien.SetActive(false);
		currentAlienIndex = currentAlienIndex > 0 ? currentAlienIndex - 1 : aliensModels.Length - 1;
		aliensModels[currentAlienIndex].SetActive(true);
		currentAlien = aliensModels[currentAlienIndex];
	}

	public void SwitchAlienRight()
	{
		int last = aliensModels.Length - 1;
		if (currentAlienIndex > last)
		{
			return;
		}
		currentAlien.SetActive(false);
		currentAlienIndex = currentAlienIndex < last ? currentAlienIndex + 1 : 0;
		aliensModels[currentAlienIndex].SetActive(true);
		currentAlien = aliensModels[currentAlienIndex];
	}

	private void LevitateUp()
	{
		aliensGroup.transform.DOMoveY(aliensGroup.transform.position.y + 0.2f, 3f).SetEase(Ease.Flash).OnComplete(LevitateDown);
	}

	private void LevitateDown()
	{
		aliensGroup.transform.DOMoveY(aliensGroup.transform.position.y - 0.2f, 3f).SetEase(Ease.Flash).OnComplete(LevitateUp);
	}
}
