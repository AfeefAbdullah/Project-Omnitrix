using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CompleteDisplayAliensController : MonoBehaviour
{
	public Transform aliensInterface;

	public Transform aliensWheel;

	public SpriteRenderer[] interfaceParts;

	public SpriteRenderer whiteIcons;

	private CompleteAnimationsController completeAnimationsController;

	public float interfaceAlpha = 0.784f;

	private Quaternion originalRotationWheel;

	private int rotations;

	private void Start()
	{
		originalRotationWheel = aliensWheel.rotation;
		completeAnimationsController = GetComponent<CompleteAnimationsController>();
		FadeInterface(interfaceAlpha, 0f);
	}

	private void FadeInterface(float alpha, float duration)
	{
		foreach (SpriteRenderer part in interfaceParts)
		{
			part.DOFade(alpha, duration);
		}
		if ((bool)whiteIcons)
		{
			whiteIcons.DOFade(alpha > 0f ? 1f : 0f, 0f);
		}
	}

	public void DisplayAliens(bool enableControls)
	{
		FindObjectOfType<AudioManager>().Play("Activation");
		aliensInterface.DOScale(1f, 0.25f);
		aliensInterface.DOMoveY(aliensInterface.position.y + 0.45f, 0.25f).OnComplete(() => completeAnimationsController.EnableControls(enableControls));
		StartCoroutine(WaitToDisplayAliensSound());
	}

	private IEnumerator WaitToDisplayAliensSound()
	{
		yield return new WaitForSeconds(0.2f);
		FindObjectOfType<AudioManager>().Play("DisplayAliens");
	}

	public void HideInterface()
	{
		FadeInterface(0f, 0.25f);
		aliensInterface.DOScale(2f, 0.25f).OnComplete(ResetInterface);
	}

	private void ResetInterface()
	{
		aliensInterface.localScale = Vector3.zero;
		Vector3 position = aliensInterface.position;
		aliensInterface.position = new Vector3(position.x, position.y - 0.45f, position.z);
		FadeInterface(interfaceAlpha, 0.25f);
	}

	public void SwitchAlienLeft()
	{
		RotateWheel(36f);
	}

	public void SwitchAlienRight()
	{
		RotateWheel(-36f);
	}

	private void RotateWheel(float degrees)
	{
		FindObjectOfType<AudioManager>().Play("Dial");
		aliensWheel.DORotate(aliensWheel.localEulerAngles + new Vector3(0f, degrees, 0f), 0.25f).OnComplete(completeAnimationsController.CheckForSecretCode);
	}

	public void RotateRandomA()
	{
		rotations = completeAnimationsController.rotations;
		aliensWheel.DORotate(aliensWheel.localEulerAngles + new Vector3(0f, -360f, 0f), completeAnimationsController.startRotationDuration, RotateMode.FastBeyond360).SetEase(Ease.InQuad).OnComplete(RotateRandomB);
	}

	private void RotateRandomB()
	{
		if (rotations < 8)
		{
			rotations++;
			aliensWheel.DORotate(aliensWheel.localEulerAngles + new Vector3(0f, -360f, 0f), completeAnimationsController.scanRotationDuration, RotateMode.FastBeyond360).SetEase(Ease.Flash).OnComplete(RotateRandomB);
		}
		else
		{
			aliensWheel.rotation = originalRotationWheel;
			HideInterface();
		}
	}
}
