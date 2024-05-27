using UnityEngine;

public class rotate : MonoBehaviour
{
	public float speed;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * speed));
	}
}
