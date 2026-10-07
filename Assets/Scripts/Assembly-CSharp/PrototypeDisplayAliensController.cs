using DG.Tweening;
using UnityEngine;

public class PrototypeDisplayAliensController : MonoBehaviour
{
	public GameObject[] aliensImages;

	public GameObject displayingAlien;

	public int currentAlienIndex;

	public bool[] aliensCanTransform;

	public bool currentAlienCanTransform;

	private void Start()
	{
		CheckIfCanUltimate();
		displayingAlien.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
	}

	public void ShowAlien(float duration = 0.25f)
	{
		displayingAlien.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 1f), duration).SetEase(Ease.Flash);
	}

	public void HideAlien(float duration = 0.25f)
	{
		displayingAlien.GetComponent<SpriteRenderer>().DOColor(new Color(0f, 0f, 0f, 0f), duration).SetEase(Ease.Flash);
	}

	public void SwitchAlienLeft()
	{
		if (currentAlienIndex >= 0)
		{
			displayingAlien.SetActive(false);
			currentAlienIndex = currentAlienIndex > 0 ? currentAlienIndex - 1 : aliensImages.Length - 1;
			aliensImages[currentAlienIndex].SetActive(true);
			displayingAlien = aliensImages[currentAlienIndex];
		}
		CheckIfCanUltimate();
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Camera.main.transform.position = Vector3.zero;
		}
	}

	public void SwitchAlienRight()
	{
		int last = aliensImages.Length - 1;
		if (currentAlienIndex <= last)
		{
			displayingAlien.SetActive(false);
			currentAlienIndex = currentAlienIndex < last ? currentAlienIndex + 1 : 0;
			aliensImages[currentAlienIndex].SetActive(true);
			displayingAlien = aliensImages[currentAlienIndex];
		}
		CheckIfCanUltimate();
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Destroy(GameObject.Find("MobileParts"));
		}
	}

	private void CheckIfCanUltimate()
	{
		currentAlienCanTransform = aliensCanTransform[currentAlienIndex];
	}
}
