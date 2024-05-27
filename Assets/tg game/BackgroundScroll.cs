using System.Collections;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
	private Transform thisTransform;

	private float moveSpeed;

	private float resetPos;

	private bool hold;

	private Camera mainCamera;

	private float imageWidth;

	public void initObject(Sprite image, float speed, Vector3 startPosition, bool h, string sortingName)
	{
		SpriteRenderer spriteRenderer = base.gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sortingLayerName = sortingName;
		spriteRenderer.sprite = image;
		moveSpeed = speed;
		base.transform.localPosition = startPosition;
		thisTransform = base.transform;
		mainCamera = Camera.main;
		imageWidth = image.rect.width / image.pixelsPerUnit * 2f;
		UnityEngine.Debug.Log("imagewidth : " + imageWidth);
		hold = h;
		StartCoroutine(move());
	}

	private IEnumerator move()
	{
		while (true)
		{
			thisTransform.localPosition -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
			if (mainCamera.WorldToViewportPoint(thisTransform.position).x < 0f)
			{
				Vector3 localPosition = thisTransform.localPosition;
				if (hold)
				{
					if (-0.1f <= resetPos && resetPos <= 0.1f)
					{
						resetPos = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x + imageWidth / 2f;
					}
					localPosition.x = resetPos;
				}
				else
				{
					localPosition.x = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x + imageWidth;
				}
				thisTransform.localPosition = localPosition;
			}
			yield return null;
		}
	}
}
