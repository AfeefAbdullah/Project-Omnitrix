using UnityEngine;
using UnityEngine.UI;

public class fpsCounter : MonoBehaviour
{
	[SerializeField]
	private Text _fpsText;

	[SerializeField]
	private float _hudRefreshRate;

	private float _timer;

	private void Update()
	{
	}
}
