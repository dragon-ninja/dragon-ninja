using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetLightLine : MonoBehaviour
{
	public Image image;

	private List<Sprite> listSprites;

	private Sequence sequence;

	public void startAction(List<Sprite> list, Color color)
	{
		listSprites = list;
		base.transform.localScale = new Vector3(0f, 0f, 1f);
		image.color = color;
		playAction();
	}

	private void playAction()
	{
		settingRandomImage();
		float duration = UnityEngine.Random.Range(3f, 7f);
		float z = UnityEngine.Random.Range(0f, 360f);
		base.transform.rotation = Quaternion.Euler(0f, 0f, z);
		if (sequence != null)
		{
			sequence.Kill();
		}
		sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOScale(new Vector3(1f, 1f, 1f), duration));
		sequence.Join(image.DOFade(1f, duration));
		sequence.Append(base.transform.DOScale(new Vector3(0.7f, 0.7f, 1f), duration));
		sequence.Join(image.DOFade(0f, duration));
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
}
