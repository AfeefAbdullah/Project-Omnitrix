using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UltimatrixDisplayAliensController : MonoBehaviour
{
	public GameObject[] aliensModels;

	public GameObject currentAlien;

	public GameObject aliensGroup;

	public int currentAlienIndex;

	public bool[] aliensCanUltimate;

	private UltimatrixAnimationsController ultimatrixAnimationsController;

	private PotisAltiareAnimations potisAltiareAnimations;

	public bool aliensGroupFollowsFreeCam;

	public FreeCameraController freeCameraController;

	public Transform freeCameraHolder2;

	private void Start()
	{
		ultimatrixAnimationsController = GetComponent<UltimatrixAnimationsController>();
		potisAltiareAnimations = GetComponent<PotisAltiareAnimations>();
		LevitateDown();
	}

	private void Update()
	{
		FollowFreeCam();
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
		if (ultimatrixAnimationsController != null && ultimatrixAnimationsController.watchStatus == "activated")
		{
			ultimatrixAnimationsController.EnableControls(true);
			currentAlien.SetActive(true);
		}
		if (potisAltiareAnimations != null)
		{
			potisAltiareAnimations.CheckIfCameraZoomed();
			currentAlien.SetActive(true);
		}
	}

	public void SwitchAlien(string direction)
	{
		BlinkAlienModel(true);
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Camera.main.fieldOfView = 1000f;
		}
		if (ultimatrixAnimationsController != null && ultimatrixAnimationsController.watchStatus == "activated")
		{
			StartCoroutine(SwitchAlienA(direction));
		}
		if (potisAltiareAnimations != null)
		{
			StartCoroutine(SwitchAlienA(direction));
		}
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
		FindObjectOfType<AudioManager>().Play("DisplayAlien");
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
		CheckIfCanUltimate();
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
		CheckIfCanUltimate();
	}

	private void LevitateUp()
	{
		aliensGroup.transform.DOMoveY(aliensGroup.transform.position.y + 0.2f, 3f).SetEase(Ease.Flash).OnComplete(LevitateDown);
	}

	private void LevitateDown()
	{
		aliensGroup.transform.DOMoveY(aliensGroup.transform.position.y - 0.2f, 3f).SetEase(Ease.Flash).OnComplete(LevitateUp);
	}

	private void CheckIfCanUltimate()
	{
		bool canUltimate = aliensCanUltimate[currentAlienIndex];
		if (ultimatrixAnimationsController != null)
		{
			ultimatrixAnimationsController.currentAlienCanUltimate = canUltimate;
		}
		if (potisAltiareAnimations != null)
		{
			potisAltiareAnimations.currentAlienCanUltimate = canUltimate;
		}
	}

	private void FollowFreeCam()
	{
		if (!aliensGroupFollowsFreeCam)
		{
			return;
		}
		Transform group = aliensGroup.transform;
		if (freeCameraController.cameraMode == cameraModes.freeMode)
		{
			group.LookAt(new Vector3(freeCameraHolder2.position.x, group.position.y, freeCameraHolder2.position.z));
		}
		else
		{
			group.rotation = Quaternion.Euler(0f, -135f, 0f);
		}
	}
}
