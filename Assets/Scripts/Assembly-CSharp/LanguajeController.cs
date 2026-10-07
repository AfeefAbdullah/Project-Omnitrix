using UnityEngine;
using UnityEngine.UI;

public class LanguajeController : MonoBehaviour
{
	public Text[] controls;

	public Text selectWatch;

	public Text selectQuality;

	public Text[] alsoTry;

	public Text[] omniLabMesssage;

	public Text watchAdMessage;

	public Text animationAdMessage;

	public Text[] requiresInternet;

	public Text[] adsCooldownText;

	public Text[] watchAnAd;

	public Text[] backAd;

	private void Start()
	{
		if (Application.systemLanguage == SystemLanguage.Portuguese)
		{
			SetPortugese();
		}
		else if (Application.systemLanguage == SystemLanguage.Spanish)
		{
			SetSpanish();
		}
	}

	private void SetSpanish()
	{
		SetTexts("ELIJE UN RELOJ", "Elige\ncalidad", "¡MIRA UN ANUNCIO PARA\nDESBLOQUEAR ESTOS RELOJES!", "¡MIRA UN ANUNCIO PARA\nDESBLOQUEAR UNA ANIMACIÓN!", "Controles:\nToca el reloj para interactuar\nDesliza para cambiar Alien", "Prueba también:", "¡Crea tu propio reloj!\n¡Búscalo en la tienda!", "(SE NECESITA INTERNET)", "SI YA VISTE UN ANUNCIO Y EL SIGUIENTE\nNO CARGA, DEBERÁS ESPERAR UN RATO.", "VER UN\nANUNCIO", "ATRÁS");
	}

	private void SetPortugese()
	{
		SetTexts("SELECIONE O RELÓGIO", "Selecione\nqualidade", "ASSISTA A UM ANÚNCIO PARA\nDESBLOQUEAR ESTES RELÓGIOS", "ASSISTA A UM ANÚNCIO PARA\nDESBLOQUEAR UMA ANIMAÇÃO!", "Controles:\nToque no relógio para interagir\nDeslize para mudar de Alien", "Experimente também:", "Crie seu próprio relógio!\nEncontre - o na loja!", "(É NECESSÁRIO CONEXÃO COM A INTERNET)", "SE VOCÊ JÁ VIU UM ANÚNCIO E O PRÓXIMO NÃO ESTÁ\nCARREGANDO, ESPERE E TENTE NOVAMENTE MAIS TARDE.", "VER UM\nANÚNCIO", "RETORNAR");
	}

	// The original iterates alsoTry.Length for all paired arrays, then backAd separately.
	private void SetTexts(string watch, string quality, string watchAd, string animationAd, string controlsText, string alsoTryText, string omniLab, string internet, string cooldown, string watchAnAdText, string back)
	{
		selectWatch.text = watch;
		selectQuality.text = quality;
		watchAdMessage.text = watchAd;
		animationAdMessage.text = animationAd;
		for (int i = 0; i < alsoTry.Length; i++)
		{
			controls[i].text = controlsText;
			alsoTry[i].text = alsoTryText;
			omniLabMesssage[i].text = omniLab;
			requiresInternet[i].text = internet;
			adsCooldownText[i].text = cooldown;
			watchAnAd[i].text = watchAnAdText;
		}
		foreach (Text text in backAd)
		{
			text.text = back;
		}
	}
}
