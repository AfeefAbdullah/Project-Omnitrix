using UnityEngine;

public class ModifyRenderQueue : MonoBehaviour
{
	public int newRenderQueueValue = 3002;

	private void Start()
	{
		GetComponent<Renderer>().material.renderQueue = newRenderQueueValue;
	}
}
