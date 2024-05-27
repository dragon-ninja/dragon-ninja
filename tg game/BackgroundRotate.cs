using System.Collections;
using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
	private float speed;

	public void initObject(Sprite image, float s, string sortingName)
	{
		SpriteRenderer spriteRenderer = base.gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingLayerName = sortingName;
		spriteRenderer.sprite = image;
		speed = s;
		StartCoroutine(rotateUpdate());
	}

	private IEnumerator rotateUpdate()
	{
		while (true)
		{
			base.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * speed));
			yield return null;
		}
	}
}
