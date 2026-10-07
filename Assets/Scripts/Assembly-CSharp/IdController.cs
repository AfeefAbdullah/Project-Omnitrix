using UnityEngine;

public class IdController : MonoBehaviour
{
	// Tamper check from the original game: sabotages the scene when the package name differs.
	public static void Check()
	{
		if (Application.identifier == "com.KokokatStudios.OmniWatch3D")
		{
			return;
		}
		Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
		camera.fieldOfView = 179f;
		Destroy(GameObject.Find("MobileParts"));
		camera.backgroundColor = Color.green;
		Destroy(GameObject.Find("AudioManager"));
	}
}
