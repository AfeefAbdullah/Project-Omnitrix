using DG.Tweening;
using UnityEngine;

public class RecalibratedAsDisplayAliensController : MonoBehaviour
{
	public GameObject[] aliensModels;

	public GameObject currentAlien;

	private MeshRenderer currentAlienMesh;

	public GameObject aliensGroup;

	public int currentAlienIndex;

	private RecalibratedAsAnimationsController recalibratedAsAnimationsController;

	private float glitchIntensity;

	private float flickIntensity;

	private void Start()
	{
		recalibratedAsAnimationsController = GetComponent<RecalibratedAsAnimationsController>();
		currentAlienMesh = aliensModels[currentAlienIndex].GetComponent<MeshRenderer>();
		LevitateDown();
	}

	private void Update()
	{
		UpdateCurrentAlienMaterial();
	}

	private void UpdateCurrentAlienMaterial()
	{
		currentAlienMesh.material.SetFloat("_GlitchIntensity", glitchIntensity);
		currentAlienMesh.material.SetFloat("_FlickerIntensity", flickIntensity);
	}

	public void ShowAlien(float duration)
	{
		aliensGroup.transform.DOScale(Vector3.one, duration).OnComplete(() => recalibratedAsAnimationsController.EnableControls(true));
		DOTween.To(() => glitchIntensity, x => glitchIntensity = x, 0f, duration);
		DOTween.To(() => flickIntensity, x => flickIntensity = x, 0f, duration);
		FindObjectOfType<AudioManager>().Play("DisplayHologram1");
	}

	private void HideAlien(float duration, TweenCallback onCompleteCallback)
	{
		aliensGroup.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InExpo).OnComplete(onCompleteCallback);
		DOTween.To(() => glitchIntensity, x => glitchIntensity = x, 6f, duration).SetEase(Ease.OutQuad);
		DOTween.To(() => flickIntensity, x => flickIntensity = x, 1f, duration).SetEase(Ease.OutQuart);
	}

	public void SwitchAlien(string direction)
	{
		if (direction == "left")
		{
			HideAlien(0.25f, SwitchAlienLeft);
		}
		else if (direction == "right")
		{
			HideAlien(0.25f, SwitchAlienRight);
		}
	}

	public void SwitchAlienLeft()
	{
		currentAlien.SetActive(false);
		if (currentAlienIndex >= 0)
		{
			currentAlienIndex = currentAlienIndex > 0 ? currentAlienIndex - 1 : aliensModels.Length - 1;
			SelectCurrentAlien();
		}
		currentAlien.SetActive(true);
		ShowAlien(0.25f);
	}

	public void SwitchAlienRight()
	{
		currentAlien.SetActive(false);
		int last = aliensModels.Length - 1;
		if (currentAlienIndex <= last)
		{
			currentAlienIndex = currentAlienIndex < last ? currentAlienIndex + 1 : 0;
			SelectCurrentAlien();
		}
		currentAlien.SetActive(true);
		ShowAlien(0.25f);
	}

	private void SelectCurrentAlien()
	{
		currentAlien = aliensModels[currentAlienIndex];
		currentAlienMesh = currentAlien.GetComponent<MeshRenderer>();
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
