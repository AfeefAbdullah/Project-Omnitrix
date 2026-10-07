using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecalibratedAnimationsController : MonoBehaviour
{
	[Header("Scripts References")]
	public MenuController menuController;

	private RecalibratedDisplayAliensController recalibratedDisplayAliensController;

	public FreeCameraController freeCameraController;

	[Header("Screen Animation Elements")]
	public Transform[] screenDiamondsTransforms;

	public SpriteRenderer[] screenDiamondsSprites;

	public SpriteRenderer screenBackgroundSprite;

	public GameObject selfDestructCanvas;

	public Image selfDestructCounter;

	[Header("Watch Parts")]
	public Transform mobilePartsUp;

	public Transform ring;

	public Transform scanRing;

	public Transform[] buttonsA;

	public Transform[] buttonsB;

	[Header("Lights")]
	public Light flashLight;

	public Light coreLight;

	[Header("Camera")]
	public Transform defaultCameraHolder;

	public Transform[] cameraPositions;

	public Transform cameraFollowPoint;

	[Header("Effects")]
	public Image flashScreen;

	public ParticleSystem explosionSmoke;

	public Transform scanRay;

	public Renderer watchScreenRenderer;

	public float defaultScreenIntensity = 0.6f;

	private float currentScreenIntensity;

	public MeshRenderer[] lightShaftsQuads;

	private float lightShaftsProgress;

	private float lightShaftsCoreBlur;

	public SpriteRenderer shineCover;

	private float shinePosition;

	public MeshRenderer plasmaTubeQuad;

	private float plasmaTubeZoom;

	private float plasmaTubeOrbSize;

	public MeshRenderer fluidCellsQuad;

	public GameObject fluidCellsElements;

	public MeshRenderer xRayFlashQuad;

	public MeshRenderer electricOrbQuad;

	private float electricOrbSize;

	private float electricOrbOpacity;

	public MeshRenderer lightCone;

	private float lightConeFadeLevel;

	[Header("Extra")]
	public GameObject transformationScene;

	public float coreHeight = 0.55f;

	public string secretCode;

	public bool transformAnimation;

	private Color screenColor;

	[HideInInspector]
	public string watchStatus;

	private Quaternion originalRingRotation;

	private bool controlsEnabled;

	private bool cameraFollowWatch;

	public int animationsCounter;

	private bool inverseScreenColors;

	private bool yellowScreen;

	// The original looks the AudioManager up on every call.
	private static AudioManager Audio => FindObjectOfType<AudioManager>();

	private void Start()
	{
		screenColor = screenBackgroundSprite.color;
		secretCode = "";
		currentScreenIntensity = defaultScreenIntensity;
		originalRingRotation = ring.rotation;
		recalibratedDisplayAliensController = GetComponent<RecalibratedDisplayAliensController>();
		transformationScene.SetActive(false);
		watchStatus = "normal";
		cameraFollowWatch = true;
		CameraIntro();
		plasmaTubeZoom = 0.85f;
		lightConeFadeLevel = 1f;
		electricOrbSize = 0.5f;
		electricOrbOpacity = 0f;
		lightShaftsCoreBlur = 0.1f;
	}

	private void Update()
	{
		UpdateScreenEmissionIntensity();
		UpdateShinePosition();
		UpdateLightShafts();
		UpdatePlasmaTube();
		UpdateElectricOrb();
		UpdateLightCone();
	}

	private void LateUpdate()
	{
		if (cameraFollowWatch)
		{
			defaultCameraHolder.LookAt(cameraFollowPoint.position);
		}
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
		lightShaftsQuads[0].material.SetFloat("_CoreBlur", lightShaftsCoreBlur);
		lightShaftsQuads[1].material.SetFloat("_CoreBlur", lightShaftsCoreBlur);
	}

	private void UpdatePlasmaTube()
	{
		plasmaTubeQuad.material.SetFloat("_Zoom", plasmaTubeZoom);
		plasmaTubeQuad.material.SetFloat("_OrbSize", plasmaTubeOrbSize);
	}

	private void UpdateElectricOrb()
	{
		electricOrbQuad.material.SetFloat("_TotalOpacity", electricOrbOpacity);
		electricOrbQuad.material.SetFloat("_CoreSize", electricOrbSize);
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

	// OutQuad is DOTween's default ease, i.e. what the original gets when it sets none.
	private Tween MoveCamera(Transform target, float duration, Ease ease = Ease.OutQuad)
	{
		defaultCameraHolder.DOMove(target.position, duration).SetEase(ease);
		return defaultCameraHolder.DORotate(target.rotation.eulerAngles, duration).SetEase(ease);
	}

	private Tween TweenFade(float endValue, float duration)
	{
		return DOTween.To(() => lightConeFadeLevel, x => lightConeFadeLevel = x, endValue, duration).SetEase(Ease.Flash);
	}

	// Slides the side buttons back home after activation pushed them out.
	private void ReturnSideButtons()
	{
		buttonsB[0].DOLocalMove(Vector3.zero, 0.5f);
		buttonsB[1].DOLocalMove(Vector3.zero, 0.5f);
	}

	private void PushSideButtons(float offset, float duration)
	{
		buttonsB[0].DOMoveZ(buttonsB[0].position.z + offset, duration);
		buttonsB[1].DOMoveZ(buttonsB[1].position.z - offset, duration);
	}

	private void CoreDown(float duration)
	{
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y - 0.55f, duration).SetEase(Ease.Flash);
	}

	private void CameraIntro()
	{
		SnapCamera(cameraPositions[0]);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 2f).SetEase(Ease.OutQuad).OnComplete(() => EnableControls(true));
	}

	public void EnableControls(bool status)
	{
		controlsEnabled = status;
		cameraFollowWatch = false;
	}

	public void ToggleTransformAnimation(bool newState)
	{
		transformAnimation = newState;
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
			EnableControls(false);
			freeCameraController.MoveFreeCamera(1);
			CameraZoom();
			watchStatus = "activated";
			StartCoroutine(WaitToButtonsAnim());
			return;
		case "activated":
			EnableControls(false);
			freeCameraController.MoveFreeCamera(0);
			PushAnim();
			watchStatus = "transformed";
			return;
		case "transformed":
			EnableControls(false);
			StartFlash(0.2f, SwitchToNormalScene);
			Audio.Play("TimeOut");
			break;
		case "destroyed":
			EnableControls(false);
			ReactivateWatch();
			break;
		case "repairDna":
			EnableControls(false);
			RepairDnaAnimB();
			break;
		default:
			Debug.Log("Invalid Status");
			return;
		}
		watchStatus = "normal";
	}

	private void CameraZoom()
	{
		MoveCamera(cameraPositions[2], 0.5f);
	}

	private IEnumerator WaitToButtonsAnim()
	{
		yield return new WaitForSeconds(0.25f);
		PressButtonsAnim();
	}

	public void PressButtonsAnim()
	{
		buttonsA[0].DOLocalMoveX(buttonsA[0].position.x + 0.26f, 0.15f).OnComplete(ActivationAnim);
		buttonsA[1].DOLocalMoveX(buttonsA[1].position.x - 0.26f, 0.15f).OnComplete(ReturnButtonsAnim);
	}

	private void ReturnButtonsAnim()
	{
		buttonsA[0].DOLocalMoveX(0f, 0.15f);
		buttonsA[1].DOLocalMoveX(0f, 0.15f);
	}

	private void ActivationAnim()
	{
		secretCode = "";
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y + coreHeight, 0.25f).SetEase(Ease.Flash).OnComplete(DisplayAliens);
		ModifyScreenEmissiveness(defaultScreenIntensity, 1f, 0.25f);
		coreLight.DOIntensity(20f, 0.2f).SetEase(Ease.Flash);
		flashLight.DOIntensity(35f, 0.2f).SetEase(Ease.Flash);
		PushSideButtons(0.25f, 0.25f);
		Audio.Play("CoreUp");
		ScreenActivationAnim();
	}

	public void ScreenActivationAnim()
	{
		screenDiamondsTransforms[0].DOLocalMoveX(0f, 0.25f).SetEase(Ease.Flash);
		screenDiamondsTransforms[1].DOLocalMoveX(0f, 0.25f).SetEase(Ease.Flash);
		StartCoroutine(ScreenActivationA());
	}

	private IEnumerator ScreenActivationA()
	{
		yield return new WaitForSeconds(0.05f);
		screenDiamondsTransforms[2].DOScale(1.5f, 0.2f).SetEase(Ease.Flash).OnComplete(ScreenActivationB);
		lightConeFadeLevel = 1f;
		TweenFade(0f, 0.2f);
	}

	private void ScreenActivationB()
	{
		screenDiamondsTransforms[0].DOLocalMoveX(-1f, 0f);
		screenDiamondsTransforms[1].DOLocalMoveX(1f, 0f);
	}

	public void DisplayAliens()
	{
		Audio.Play("DisplayHologram0");
		recalibratedDisplayAliensController.DisplayAliens();
	}

	private void StartFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(100f, duration);
		flashScreen.DOFade(1f, duration).SetEase(Ease.InExpo).OnComplete(onCompleteMethod);
		ModifyScreenEmissiveness(currentScreenIntensity, 1.2f, duration);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 1f, duration).SetEase(Ease.InQuint);
		DOTween.To(() => lightShaftsCoreBlur, x => lightShaftsCoreBlur = x, 0.213f, duration).SetEase(Ease.InQuint);
	}

	private void EndFlash(float duration, TweenCallback onCompleteMethod)
	{
		flashLight.DOIntensity(10f, duration);
		flashScreen.DOFade(0f, duration).SetEase(Ease.InQuart).OnComplete(onCompleteMethod);
		ModifyScreenEmissiveness(1.2f, defaultScreenIntensity, duration);
		DOTween.To(() => lightShaftsProgress, x => lightShaftsProgress = x, 0f, duration);
		DOTween.To(() => lightShaftsCoreBlur, x => lightShaftsCoreBlur = x, 0.1f, duration).SetEase(Ease.OutQuint);
	}

	private void PushAnim()
	{
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y - coreHeight, 0.1f).SetEase(Ease.Flash);
		TweenFade(1f, 0.1f);
		Audio.Play("Transform");
		if (!transformAnimation)
		{
			StartFlash(0.5f, SwitchToTransformedScene);
			return;
		}
		Audio.Play("TransformationTheme");
		StartFlash(0.5f, TransformingSequence);
		menuController.ShowLetterboxing(true);
	}

	private void TransformingSequence()
	{
		EndFlash(0.5f, null);
		coreLight.DOIntensity(7f, 0.2f).SetEase(Ease.Flash);
		screenDiamondsTransforms[2].localScale = Vector3.zero;
		recalibratedDisplayAliensController.currentAlien.SetActive(false);
		ReturnSideButtons();
		plasmaTubeQuad.gameObject.SetActive(true);
		plasmaTubeZoom = 0.85f;
		plasmaTubeOrbSize = 0.21f;
		DOTween.To(() => plasmaTubeZoom, x => plasmaTubeZoom = x, 1.5f, 2.5f).SetEase(Ease.InCubic);
		DOTween.To(() => plasmaTubeOrbSize, x => plasmaTubeOrbSize = x, 0.32f, 2.3f).SetEase(Ease.InCubic).OnComplete(() =>
			StartFlash(0.2f, () =>
			{
				plasmaTubeQuad.gameObject.SetActive(false);
				fluidCellsQuad.gameObject.SetActive(true);
				fluidCellsElements.SetActive(true);
				SnapCamera(cameraPositions[6]);
				defaultCameraHolder.DOMove(cameraPositions[3].position, 3f).SetEase(Ease.OutSine).OnComplete(TransformingSequenceA);
				defaultCameraHolder.DORotate(cameraPositions[3].rotation.eulerAngles, 3f).SetEase(Ease.OutSine);
				EndFlash(0.2f, null);
			}));
	}

	private void TransformingSequenceA()
	{
		electricOrbQuad.gameObject.SetActive(true);
		electricOrbOpacity = 0f;
		DOTween.To(() => electricOrbOpacity, x => electricOrbOpacity = x, 0.75f, 1.7f);
		electricOrbSize = 0.5f;
		DOTween.To(() => electricOrbSize, x => electricOrbSize = x, 0.2f, 1.7f).OnComplete(TransformSequenceB);
	}

	private void TransformSequenceB()
	{
		DOTween.To(() => electricOrbOpacity, x => electricOrbOpacity = x, 0f, 0.25f).OnComplete(() => electricOrbQuad.gameObject.SetActive(false));
		xRayFlashQuad.gameObject.SetActive(true);
		StartCoroutine(TransformationSequenceC());
	}

	private IEnumerator TransformationSequenceC()
	{
		yield return new WaitForSeconds(1.5f);
		StartFlash(0.25f, null);
		Camera.main.DOFieldOfView(50f, 0.25f).SetEase(Ease.InCubic).OnComplete(SwitchToTransformedScene);
	}

	private void SwitchToTransformedScene()
	{
		menuController.ShowLetterboxing(false);
		coreLight.DOIntensity(7f, 0.2f).SetEase(Ease.Flash);
		screenDiamondsTransforms[2].localScale = Vector3.zero;
		ReturnSideButtons();
		recalibratedDisplayAliensController.currentAlien.SetActive(false);
		Camera.main.fieldOfView = 60f;
		fluidCellsQuad.gameObject.SetActive(false);
		fluidCellsElements.SetActive(false);
		xRayFlashQuad.gameObject.SetActive(false);
		transformationScene.SetActive(true);
		SnapCamera(cameraPositions[4]);
		EndFlash(0.5f, () =>
		{
			shinePosition = 0f;
			DOTween.To(() => shinePosition, x => shinePosition = x, 1f, 0.6f).OnComplete(() => EnableControls(true));
		});
	}

	private void SwitchToNormalScene()
	{
		transformationScene.SetActive(false);
		SnapCamera(cameraPositions[1]);
		EndFlash(0.5f, () => EnableControls(true));
	}

	public void RotateLeft()
	{
		Rotate(-90f, "L", "left");
	}

	public void RotateRight()
	{
		Rotate(90f, "R", "right");
	}

	private void Rotate(float degrees, string code, string direction)
	{
		if (!controlsEnabled || watchStatus != "activated" || menuController.menuOpen)
		{
			return;
		}
		EnableControls(false);
		UpdateSecretCode(code);
		CheckForSecretCode();
		ring.rotation = originalRingRotation;
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, degrees, 0f), 0.2f).SetEase(Ease.Flash).OnComplete(() => recalibratedDisplayAliensController.SwitchAlien(direction));
		Audio.Play("Dial");
	}

	private void UpdateSecretCode(string newDirection)
	{
		if (secretCode.Length > 5)
		{
			secretCode = secretCode.Remove(0, 1);
		}
		secretCode += newDirection;
	}

	public void CheckForSecretCode()
	{
		IEnumerator command;
		switch (secretCode)
		{
		case "RRLLRL":
			command = MasterControlAnimA();
			break;
		case "LRLRLL":
			command = ScanModeAnimA();
			break;
		case "RLRLRR":
			command = SelfDestructAnimA();
			break;
		case "LLRRLR":
			command = RepairDnaAnimA();
			break;
		default:
			return;
		}
		watchStatus = "command";
		StartCoroutine(CommandAcceptedAnimation());
		StartCoroutine(command);
	}

	private void ResetScreenDiamonds()
	{
		screenDiamondsTransforms[2].localScale = Vector3.zero;
		Vector3 left = screenDiamondsTransforms[0].localPosition;
		screenDiamondsTransforms[0].localPosition = new Vector3(-1.65f, left.y, left.z);
		Vector3 right = screenDiamondsTransforms[1].localPosition;
		screenDiamondsTransforms[1].localPosition = new Vector3(1.65f, right.y, right.z);
		screenDiamondsTransforms[2].localScale = Vector3.zero;
	}

	private IEnumerator CommandAcceptedAnimation()
	{
		yield return new WaitForSeconds(0.75f);
		freeCameraController.MoveFreeCamera(0);
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 0.25f);
		TweenFade(1f, 0.1f);
		ResetScreenDiamonds();
		screenDiamondsTransforms[0].DOLocalMoveX(-1f, 0.4f).SetEase(Ease.Flash);
		screenDiamondsTransforms[1].DOLocalMoveX(1f, 0.4f).SetEase(Ease.Flash);
		MoveCamera(cameraPositions[5], 1f).OnComplete(() => Audio.Play("ActivationBeep"));
		Audio.Play("CommandAccepted");
	}

	private IEnumerator MasterControlAnimA()
	{
		yield return new WaitForSeconds(1.75f);
		CoreDown(0.1f);
		StartFlash(0.5f, MasterControlAnimB);
		Audio.Play("Transformation");
	}

	private void MasterControlAnimB()
	{
		SnapCamera(cameraPositions[1]);
		recalibratedDisplayAliensController.currentAlien.SetActive(false);
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 0.5f);
		coreLight.DOIntensity(7f, 0.2f);
		ReturnSideButtons();
		EndFlash(0.5f, () => StartCoroutine(MasterControlAnimC()));
		MasterControlAnimD();
	}

	private IEnumerator MasterControlAnimC()
	{
		yield return new WaitForSeconds(0.55f);
		Audio.Play("MasterControlUnlocked");
		yield return new WaitForSeconds(2f);
		Audio.Play("SamplesAvailable");
	}

	public void MasterControlAnimD()
	{
		if (animationsCounter < 6)
		{
			animationsCounter++;
			Audio.Play("Beep5");
			ResetScreenDiamonds();
			screenDiamondsTransforms[0].DOLocalMoveX(0f, 0.8f).SetEase(Ease.Flash);
			screenDiamondsTransforms[1].DOLocalMoveX(0f, 0.8f).SetEase(Ease.Flash);
			StartCoroutine(MasterControlAnimE());
			return;
		}
		animationsCounter = 0;
		watchStatus = "normal";
		ResetScreenDiamonds();
		screenDiamondsTransforms[0].DOLocalMoveX(-1f, 0.6f).SetEase(Ease.Flash);
		screenDiamondsTransforms[1].DOLocalMoveX(1f, 0.6f).SetEase(Ease.Flash).OnComplete(() => EnableControls(true));
	}

	private IEnumerator MasterControlAnimE()
	{
		yield return new WaitForSeconds(0.43f);
		screenDiamondsTransforms[2].DOScale(1.5f, 0.53f).SetEase(Ease.Flash).OnComplete(MasterControlAnimD);
	}

	private IEnumerator SelfDestructAnimA()
	{
		yield return new WaitForSeconds(2.5f);
		InvokeRepeating("SwitchScreenColors", 0f, 0.35f);
		Audio.Play("DestructSequence");
	}

	private void SwitchScreenColors()
	{
		if (animationsCounter < 8)
		{
			Audio.Play("Beep8");
			Color black = new Color(0f, 0f, 0f, 1f);
			Color diamonds = inverseScreenColors ? black : screenColor;
			screenDiamondsSprites[0].DOColor(diamonds, 0.15f);
			screenDiamondsSprites[1].DOColor(diamonds, 0.15f);
			screenBackgroundSprite.DOColor(inverseScreenColors ? screenColor : black, 0.15f);
			inverseScreenColors = !inverseScreenColors;
			animationsCounter++;
			return;
		}
		animationsCounter = 10;
		CancelInvoke("SwitchScreenColors");
		screenBackgroundSprite.color = new Color(0f, 0f, 0f, 1f);
		selfDestructCanvas.SetActive(true);
		InvokeRepeating("SelfDestructCounter", 0f, 0.0416f);
		Audio.Play("SelfDestructPowerCharge");
		Audio.Play("Detonation");
		Audio.Play("Beep1");
	}

	private void SelfDestructCounter()
	{
		if (selfDestructCounter.fillAmount > 0.0416f)
		{
			selfDestructCounter.fillAmount -= 0.0416f;
			return;
		}
		animationsCounter--;
		selfDestructCounter.fillAmount = 1f;
		if (animationsCounter == 5)
		{
			Audio.Play("CountDown");
		}
		if (animationsCounter < 1)
		{
			CancelInvoke("SelfDestructCounter");
			SelfDestructiExplosionA();
			return;
		}
		selfDestructCounter.fillAmount -= 0.0416f;
		Audio.Play("Beep1");
	}

	private void SelfDestructiExplosionA()
	{
		Audio.Play("Beep3");
		ModifyScreenEmissiveness(defaultScreenIntensity, 1.2f, 1.5f, SelfDestructiExplosionB);
		Audio.Play("SelfDestructPowerCharge2");
	}

	private void SelfDestructiExplosionB()
	{
		StartFlash(0.5f, SelfDestructiExplosionC);
		Audio.Stop("Beep3");
		Audio.Play("SelfDestructExplosion");
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 0.5f);
	}

	private void SelfDestructiExplosionC()
	{
		explosionSmoke.Play();
		Vector3 position = mobilePartsUp.position;
		mobilePartsUp.position = new Vector3(position.x, position.y - 0.55f, position.z);
		coreLight.intensity = 0f;
		ReturnSideButtons();
		selfDestructCanvas.SetActive(false);
		watchStatus = "destroyed";
		EndFlash(2.5f, () => EnableControls(true));
	}

	private void ReactivateWatch()
	{
		MoveCamera(cameraPositions[1], 1f);
		explosionSmoke.Stop();
		screenBackgroundSprite.DOColor(screenColor, 1f);
		coreLight.DOIntensity(7f, 1f).OnComplete(() => EnableControls(true));
		Audio.Play("Reactivate");
	}

	private IEnumerator ScanModeAnimA()
	{
		yield return new WaitForSeconds(2.5f);
		InvokeRepeating("YellowScreen", 0f, 0.25f);
		CoreDown(1f);
		PushSideButtons(-0.25f, 0.5f);
		Audio.Play("Beep7");
		Audio.Play("DnaDetected");
	}

	private void YellowScreen()
	{
		if (animationsCounter > 6)
		{
			animationsCounter = 0;
			yellowScreen = false;
			CancelInvoke("YellowScreen");
			StartCoroutine(CoreRotateUp(1f));
			return;
		}
		if (!yellowScreen)
		{
			screenBackgroundSprite.color = new Color(1f, 0.9215686f, 0.01568628f, 1f);
		}
		else
		{
			screenBackgroundSprite.DOColor(screenColor, 0.15f);
		}
		yellowScreen = !yellowScreen;
		animationsCounter++;
	}

	private IEnumerator CoreRotateUp(float duration)
	{
		yield return new WaitForSeconds(0.5f);
		ModifyScreenEmissiveness(currentScreenIntensity, 1.2f, duration);
		mobilePartsUp.DOMoveY(mobilePartsUp.position.y + 0.55f, duration).SetEase(Ease.Flash);
		ring.rotation = originalRingRotation;
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, 405f, 0f), duration, RotateMode.FastBeyond360).SetEase(Ease.Flash).OnComplete(() => StartCoroutine(PrepareRay()));
		Audio.Play("Beep6");
	}

	private IEnumerator PrepareRay()
	{
		Audio.Play("Beep2");
		ring.gameObject.SetActive(false);
		scanRing.gameObject.SetActive(true);
		MoveCamera(cameraPositions[6], 0.75f, Ease.InSine);
		yield return new WaitForSeconds(0.5f);
		ShowRay();
	}

	private void ShowRay()
	{
		scanRay.DOScaleX(0.05f, 0.5f).SetEase(Ease.Flash);
		scanRay.DOScaleZ(1.4154f, 0.5f).OnComplete(() => scanRay.DOScaleX(1.4154f, 0.75f).SetEase(Ease.Flash).OnComplete(ScanA));
		Audio.Play("ScanRay");
		Audio.Play("Beep5");
	}

	private void TiltRay(float degrees, float duration, TweenCallback onComplete)
	{
		scanRay.DORotate(scanRay.rotation.eulerAngles + new Vector3(degrees, 0f, 0f), duration).OnComplete(onComplete);
	}

	private void ScanA()
	{
		TiltRay(-15f, 0.7f, ScanB);
	}

	private void ScanB()
	{
		TiltRay(30f, 1.4f, ScanC);
	}

	private void ScanC()
	{
		TiltRay(-15f, 0.7f, HideRay);
	}

	private void HideRay()
	{
		scanRay.DOScaleX(0.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
			scanRay.DOScaleZ(0f, 0.2f).OnComplete(() => scanRay.DOScaleX(0f, 0.2f)));
		Audio.Play("DnaAcquired");
		StartCoroutine(ScanCompleted());
	}

	private IEnumerator ScanCompleted()
	{
		MoveCamera(cameraPositions[5], 1f);
		yield return new WaitForSeconds(2.25f);
		defaultCameraHolder.DOMove(cameraPositions[1].position, 1.2f).SetEase(Ease.Flash);
		defaultCameraHolder.DORotate(cameraPositions[1].rotation.eulerAngles, 1.2f);
		screenBackgroundSprite.DOColor(screenColor, 1.2f);
		Audio.Play("ScanCompleted");
		CoreRotateDown(1.2f);
	}

	private void CoreRotateDown(float duration)
	{
		ring.gameObject.SetActive(true);
		scanRing.gameObject.SetActive(false);
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, duration);
		CoreDown(duration);
		ring.DORotate(ring.localEulerAngles + new Vector3(0f, -405f, 0f), duration, RotateMode.FastBeyond360).OnComplete(() => EnableControls(true));
		Audio.Play("Beep7");
		watchStatus = "normal";
	}

	private IEnumerator RepairDnaAnimA()
	{
		yield return new WaitForSeconds(2.5f);
		watchStatus = "repairDna";
		Audio.Play("GeneticDamageDetected");
		CoreDown(0.5f);
		PushSideButtons(-0.25f, 0.5f);
		ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 0.5f);
		coreLight.DOIntensity(7f, 0.5f);
		yield return new WaitForSeconds(2.25f);
		Audio.Play("AttemptToRepair");
		MoveCamera(cameraPositions[1], 1f).OnComplete(() => EnableControls(true));
		InvokeRepeating("RepairDnaBlink", 0f, 0.6f);
		InvokeRepeating("RepairDnaBeep", 0f, 1.2f);
	}

	private void RepairDnaBlink()
	{
		coreLight.intensity = 10f;
		coreLight.DOIntensity(0f, 0.5f).SetEase(Ease.Flash);
		screenBackgroundSprite.color = new Color(0.7254902f, 1f, 0.5294118f, 1f);
		screenBackgroundSprite.DOColor(screenColor, 0.5f).SetEase(Ease.Flash);
	}

	private void RepairDnaBeep()
	{
		Audio.Play("Beep4");
	}

	private void RepairDnaAnimB()
	{
		CancelInvoke("RepairDnaBlink");
		CancelInvoke("RepairDnaBeep");
		StartFlash(1f, RepairFlash);
		ModifyScreenEmissiveness(currentScreenIntensity, 1.2f, 0.5f);
		Audio.Play("DnaRepair");
	}

	private void RepairFlash()
	{
		if (animationsCounter > 3)
		{
			animationsCounter = 0;
			ModifyScreenEmissiveness(currentScreenIntensity, defaultScreenIntensity, 1.5f);
			EndFlash(1.5f, () => Audio.Play("GeneticDamageRepaired"));
			StartCoroutine(WaitToEnableControls());
			return;
		}
		animationsCounter++;
		flashScreen.DOFade(0.75f, 0.2f).SetEase(Ease.Flash).OnComplete(() =>
			flashScreen.DOFade(0f, 0.25f).SetEase(Ease.Flash).OnComplete(RepairFlash));
	}

	private IEnumerator WaitToEnableControls()
	{
		yield return new WaitForSeconds(3.2f);
		EnableControls(true);
	}
}
