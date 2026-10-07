using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PotisAltiareAnimations : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private UltimatrixDisplayAliensController ultimatrixDisplayAliensController;

	public FreeCameraController freeCameraController;

	[Header("Screen Animation Elements")]
	public Transform diamond;

	[Header("Watch Parts")]
	public Transform mobilePartsUp;

	public Transform ring;

	public GameObject core;

	public Transform screen;

	[Header("PotisAltiare Parts")]
	public GameObject potisAltiare;

	public Transform top;

	public Transform[] middle;

	public Transform[] wings;

	public Transform[] parts;

	public Transform[] partsPos0;

	public Transform[] partsPos1;

	public Transform[] partsPos2;

	public Transform[] partsPos3;

	public Transform[] partsPos4;

	public Transform[] partsTransformed;

	public Transform[] partsPos2Transformed;

	public Transform[] partsPos4Transformed;

	[Header("Lights")]
	public Light flashLight;

	public Light potisAltiareLight;

	[Header("Effects")]
	public Image flashScreen;

	public Renderer watchScreenRenderer;

	public float defaultScreenIntensity = 0.6f;

	private float currentScreenIntensity;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	private float lightshaftsCoreBlur;

	public SpriteRenderer shineCover;

	private float shinePosition;

	public MeshRenderer lightCone;

	private float lightConeFadeLevel;

	[Header("Extra")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	public GameObject transformationScene;

	private bool controlsEnabled;

	private string watchStatus;

	public bool currentAlienCanUltimate;

	public float coreHeight;

	private bool cameraZoomed;

	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		ultimatrixDisplayAliensController = GetComponent<UltimatrixDisplayAliensController>();
		transformationScene.SetActive(false);
		watchStatus = "normal";
		potisAltiare.SetActive(false);
		parts[5].gameObject.SetActive(false);
		currentScreenIntensity = 0.2f;
		lightConeFadeLevel = 1f;
		CameraIntroA();
	}

	private void Update()
	{
		UpdateScreenEmissionIntensity();
		UpdateShinePosition();
		UpdateLightShafts();
		UpdateLightCone();
	}

	private void UpdateScreenEmissionIntensity()
	{
		float i = currentScreenIntensity;
		watchScreenRenderer.material.SetColor("_EmissionColor", new Color(i, i, i, i));
		watchScreenRenderer.material.EnableKeyword("_EMISSION");
	}

	private void UpdateShinePosition()
	{
		shineCover.material.SetFloat("_ShineLocation", shinePosition);
	}

	private void UpdateLightShafts()
	{
		lightShaftsQuads[0].material.SetFloat("_AppearProgress", lightShaftsProgress);
		lightShaftsQuads[1].material.SetFloat("_AppearProgress", lightShaftsProgress);
		lightShaftsQuads[0].material.SetFloat("_CoreBlur", lightshaftsCoreBlur);
		lightShaftsQuads[1].material.SetFloat("_CoreBlur", lightshaftsCoreBlur);
	}

	private void UpdateLightCone()
	{
		lightCone.material.SetFloat("_FadeStart", lightConeFadeLevel);
	}

	private void ModifyScreenEmissiveness(float startValue, float endValue, float duration, TweenCallback onCompleteCallback = null)
	{
		currentScreenIntensity = startValue;
		DOTween.To(() => currentScreenIntensity, x => currentScreenIntensity = x, endValue, duration).OnComplete(onCompleteCallback);
	}

	private void SnapCamera(Transform target)
	{
		defaultCameraHolder.position = target.position;
		defaultCameraHolder.rotation = target.rotation;
	}

	private Tween MoveCamera(Transform target, float duration)
	{
		defaultCameraHolder.DOMove(target.position, duration);
		return defaultCameraHolder.DORotate(target.rotation.eulerAngles, duration);
	}

	// Moves parts[i] onto targets[i] (position and rotation), optionally scale too.
	private void MoveParts(Transform[] items, Transform[] targets, float duration, Ease ease, bool scale = false)
	{
		for (int i = 0; i < parts.Length; i++)
		{
			items[i].DOMove(targets[i].position, duration).SetEase(ease);
			items[i].DORotate(targets[i].rotation.eulerAngles, duration).SetEase(ease);
			if (scale)
			{
				items[i].DOScale(targets[i].localScale, duration).SetEase(ease);
			}
		}
	}

	private void SnapParts(Transform[] items, Transform[] targets, bool scale)
	{
		for (int i = 0; i < parts.Length; i++)
		{
			items[i].position = targets[i].position;
			items[i].rotation = targets[i].rotation;
			if (scale)
			{
				items[i].localScale = targets[i].localScale;
			}
		}
	}

	private void CameraIntroA()
	{
		SnapCamera(cameraPositions[0]);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 1f).OnComplete(LightActivation);
	}

	private void LightActivation()
	{
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 0.25f, PotisAltiareActivation);
		Audio.Play("DisplayHologram");
	}

	private void PotisAltiareActivation()
	{
		Audio.Play("MustCompleteP");
		StartCoroutine(PotisAltiareAppear());
	}

	private IEnumerator PotisAltiareAppear()
	{
		yield return new WaitForSeconds(2.5f);
		Audio.Play("PotisAltiareCharging");
		Audio.Play("FastChop");
		SnapParts(parts, partsPos0, false);
		potisAltiare.SetActive(true);
		MoveParts(parts, partsPos1, 1f, Ease.Unset);
		StartCoroutine(PotisAltiareAssemble());
	}

	private IEnumerator PotisAltiareAssemble()
	{
		yield return new WaitForSeconds(1.5f);
		Audio.Play("CoreUp");
		Audio.Play("FastChop");
		for (int i = 0; i < parts.Length; i++)
		{
			parts[i].DOMove(partsPos2[i].position, 0.25f).SetEase(Ease.Flash);
			parts[i].DORotate(partsPos2[i].rotation.eulerAngles, 0.25f);
		}
		StartCoroutine(CompleteFlashLight());
		StartCoroutine(CameraIntroB());
	}

	private IEnumerator CompleteFlashLight()
	{
		yield return new WaitForSeconds(0.25f);
		potisAltiareLight.DOIntensity(50f, 0.25f).OnComplete(() => potisAltiareLight.DOIntensity(0f, 1.5f));
		screen.DOLocalRotate(screen.localEulerAngles + new Vector3(0f, -180f, 0f), 0.5f, RotateMode.FastBeyond360);
	}

	private IEnumerator CameraIntroB()
	{
		yield return new WaitForSeconds(1f);
		MoveCamera(cameraPositions[2], 1.5f).SetEase(Ease.OutQuart).OnComplete(() => controlsEnabled = true);
	}

	public void EnableControls(bool status)
	{
		controlsEnabled = status;
	}

	public void Act()
	{
		if (menuController.menuOpen || !controlsEnabled)
		{
			return;
		}
		switch (watchStatus)
		{
		case "normal":
			controlsEnabled = false;
			freeCameraController.MoveFreeCamera(1);
			watchStatus = "activated";
			ActivationAnim();
			break;
		case "activated":
			controlsEnabled = false;
			freeCameraController.MoveFreeCamera(0);
			PushAnim();
			watchStatus = "transformed";
			break;
		case "transformed":
			controlsEnabled = false;
			StartFlash(0.5f, SwitchToNormalScene);
			Audio.Play("TimeOut");
			watchStatus = "normal";
			break;
		default:
			Debug.Log("Invalid Status");
			break;
		}
	}

	public void CheckIfCameraZoomed()
	{
		if (!cameraZoomed)
		{
			CameraZoom();
			cameraZoomed = true;
		}
		else
		{
			controlsEnabled = true;
		}
	}

	private void CameraZoom()
	{
		MoveCamera(cameraPositions[3], 1f).OnComplete(() => controlsEnabled = true);
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Destroy(GameObject.Find("MobileParts"));
		}
	}

	private void PotisAliareActivationAliens()
	{
		MoveParts(parts, partsPos3, 0.2f, Ease.Flash);
	}

	private void ActivationAnim()
	{
		core.SetActive(true);
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y + coreHeight, 0.25f).SetEase(Ease.Flash).OnComplete(DisplayAliens);
		ModifyScreenEmissiveness(defaultScreenIntensity, 1f, 0.25f);
		potisAltiareLight.DOIntensity(35f, 0.2f).SetEase(Ease.Flash);
		diamond.DOScale(1.75f, 0.25f).SetEase(Ease.Flash);
		lightConeFadeLevel = 1f;
		DOTween.To(() => lightConeFadeLevel, x => lightConeFadeLevel = x, 0f, 0.25f).SetEase(Ease.Flash);
		PotisAliareActivationAliens();
		Audio.Play("CoreUp");
	}

	public void DisplayAliens()
	{
		Audio.Play("DisplayAlien");
		ultimatrixDisplayAliensController.DisplayAliens();
	}

	private void PushAnim()
	{
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y - coreHeight, 0.1f).SetEase(Ease.Flash).OnComplete(() => core.SetActive(false));
		DOTween.To(() => lightConeFadeLevel, x => lightConeFadeLevel = x, 1f, 0.1f).SetEase(Ease.Flash);
		StartFlash(0.5f, SwitchToTransformedScene);
		AnimatePotisAltiareTransform();
		Audio.Play("Transformation");
		Audio.Play("Push");
	}

	private void StartFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(100f, duration);
		potisAltiareLight.DOIntensity(100f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(onCompleteMethod);
		ModifyScreenEmissiveness(currentScreenIntensity, 1.2f, duration);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.213f, duration).SetEase(Ease.InQuint);
	}

	private void AnimatePotisAltiareTransform()
	{
		MoveParts(parts, partsPos4, 1f, Ease.Flash, true);
		MoveParts(partsTransformed, partsPos4Transformed, 1f, Ease.Flash, true);
		parts[5].gameObject.SetActive(true);
	}

	private void SwitchToTransformedScene()
	{
		transformationScene.SetActive(true);
		diamond.localScale = Vector3.zero;
		ultimatrixDisplayAliensController.currentAlien.SetActive(false);
		SnapCamera(cameraPositions[4]);
		EndFlash(0.5f, () => controlsEnabled = true);
	}

	private void EndFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(0f, duration);
		potisAltiareLight.DOIntensity(0f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(onCompleteMethod);
		ModifyScreenEmissiveness(1.2f, defaultScreenIntensity, duration);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
		DOTween.To(() => lightshaftsCoreBlur, x => lightshaftsCoreBlur = x, 0.1f, duration).SetEase(Ease.OutQuint);
	}

	private void SwitchToNormalScene()
	{
		transformationScene.SetActive(false);
		SnapCamera(cameraPositions[2]);
		SnapParts(parts, partsPos2, true);
		SnapParts(partsTransformed, partsPos2Transformed, true);
		EndFlash(0.5f, () => controlsEnabled = true);
		cameraZoomed = false;
	}

	public void RotateLeft()
	{
		if (!Rotate(-90f, "left"))
		{
			return;
		}
		if (Application.identifier != "com.KokokatStudios.OmniWatch3D")
		{
			Destroy(Camera.main);
		}
	}

	public void RotateRight()
	{
		Rotate(90f, "right");
	}

	private bool Rotate(float degrees, string direction)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return false;
		}
		controlsEnabled = false;
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.25f).SetEase(Ease.Flash).OnComplete(() => ultimatrixDisplayAliensController.SwitchAlien(direction));
		Audio.Play("Dial");
		return true;
	}
}
