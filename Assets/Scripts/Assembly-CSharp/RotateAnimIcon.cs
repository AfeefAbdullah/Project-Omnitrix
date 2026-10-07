using UnityEngine;

public class RotateAnimIcon : MonoBehaviour
{
	private void Update()
	{
		transform.Rotate(Vector3.back * Time.deltaTime * 200f);
	}
}
