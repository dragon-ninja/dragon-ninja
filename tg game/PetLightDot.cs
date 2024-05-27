using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetLightDot : MonoBehaviour
{
	public Image image;

	private List<Sprite> listSprites;

	private Vector3 targetPosition;

	private float widthSize;

	private float heightSize;

	private Sequence sequence;

	public void startAction(List<Sprite> list, Color color, Vector3 pos, float width, float height)
	{
		listSprites = list;
		float num = UnityEngine.Random.Range(0f, 1f);
		base.transform.localScale = new Vector3(num, num, 1f);
		image.color = color;
		targetPosition = pos;
		widthSize = width;
		heightSize = height;
		playAction();
	}

	private void playAction()
	{
		settingRandomPosition();
		settingRandomImage();
		float duration = UnityEngine.Random.Range(1f, 6f);
		if (sequence != null)
		{
			sequence.Kill();
		}
		sequence = DOTween.Sequence();
		sequence.Append(image.DOFade(1f, duration));
		sequence.Append(image.DOFade(0f, duration));
		sequence.AppendCallback(delegate
		{
			playAction();
		});
	}

	private void settingRandomImage()
	{
		image.sprite = listSprites[Random.Range(0, listSprites.Count)];
		image.SetNativeSize();
	}

	private void settingRandomPosition()
	{
		float x = targetPosition.x + UnityEngine.Random.Range(0f - widthSize, widthSize);
		float y = targetPosition.y + UnityEngine.Random.Range(0f - heightSize, heightSize);
		Vector3 localPosition = new Vector3(x, y, 0f);
		base.transform.localPosition = localPosition;
	}
}
