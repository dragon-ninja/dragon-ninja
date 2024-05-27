using System.Collections.Generic;
using UnityEngine;

public class StartTitle : MonoBehaviour
{
	public Transform playerTransform;

	public List<GameObject> listSubObjects = new List<GameObject>();

	private bool start;

	private void Update()
	{
		if (!start && Mathf.Abs(base.transform.position.x - playerTransform.position.x) < 5f)
		{
			base.gameObject.GetComponent<Rigidbody2D>().gravityScale = 5f;
			int count = listSubObjects.Count;
			for (int i = 0; i < count; i++)
			{
				listSubObjects[i].GetComponent<Rigidbody2D>().gravityScale = 5f;
			}
			start = true;
		}
	}
}
