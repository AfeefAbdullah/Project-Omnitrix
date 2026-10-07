using DG.Tweening;
using UnityEngine;

public class PrototypeRatDisplayAliensController : MonoBehaviour
{
	public Transform[] aliensTransform;

	public SpriteRenderer[] aliensSprites;

	public int currentAlienIndex;

	public Transform[] aliensRefPos;

	private PrototypeRatAnimatinosController prototypeRatAnimatinosController;

	private void Start()
	{
		aliensSprites[currentAlienIndex].color = new Color(0f, 0f, 0f, 0f);
		prototypeRatAnimatinosController = GetComponent<PrototypeRatAnimatinosController>();
	}

	public void ShowAlien()
	{
		aliensSprites[currentAlienIndex].DOColor(new Color(0f, 0f, 0f, 1f), 0.2f).SetEase(Ease.Flash);
	}

	public void HideAlien(float duration = 0.2f)
	{
		aliensSprites[currentAlienIndex].DOColor(new Color(0f, 0f, 0f, 0f), duration).SetEase(Ease.Flash);
	}

	public void SwitchAlienLeft()
	{
		if (currentAlienIndex >= 0)
		{
			aliensTransform[currentAlienIndex].DOMoveX(aliensRefPos[0].position.x, 0.5f).SetEase(Ease.Flash);
			currentAlienIndex = currentAlienIndex == 0 ? aliensSprites.Length - 1 : currentAlienIndex - 1;
			BringIn(aliensRefPos[2]);
		}
		UpdateEonMode();
	}

	public void SwitchAlienRight()
	{
		int last = aliensSprites.Length - 1;
		if (currentAlienIndex <= last)
		{
			aliensTransform[currentAlienIndex].DOMoveX(aliensRefPos[2].position.x, 0.5f).SetEase(Ease.Flash);
			currentAlienIndex = currentAlienIndex < last ? currentAlienIndex + 1 : 0;
			BringIn(aliensRefPos[0]);
		}
		UpdateEonMode();
	}

	// Snap the new alien to the off-screen ref point, then slide it to the centre ref.
	private void BringIn(Transform from)
	{
		Transform t = aliensTransform[currentAlienIndex];
		t.position = new Vector3(from.position.x, from.position.y, t.position.z);
		t.DOMoveX(aliensRefPos[1].position.x, 0.5f).SetEase(Ease.Flash).OnComplete(delegate
		{
			FindObjectOfType<AudioManager>().Play("Select");
		});
	}

	private void UpdateEonMode()
	{
		if (prototypeRatAnimatinosController != null)
		{
			if (currentAlienIndex == 10)
			{
				prototypeRatAnimatinosController.EnableEonMode();
			}
			else
			{
				prototypeRatAnimatinosController.DisableEonMode();
			}
		}
	}
}
