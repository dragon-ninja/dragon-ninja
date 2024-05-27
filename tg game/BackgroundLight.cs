using DG.Tweening;
using UnityEngine;

public class BackgroundLight : MonoBehaviour
{
	private Sequence sequence;

	private SpriteRenderer render;

	private float sequenceTime;

	private bool fadeOut;

	public void initObject(Sprite image, float time, string sortingName)
	{
		render = base.gameObject.AddComponent<SpriteRenderer>();
		render.sortingLayerName = sortingName;
		render.sprite = image;
		sequenceTime = time;
		playAction();
	}

	private void playAction()
	{
		sequence = DOTween.Sequence();
		sequence.Append(render.DOFade(0f, sequenceTime / 2f));
		sequence.Append(render.DOFade(1f, sequenceTime / 2f));
		sequence.SetLoops(-1);
		sequence.Play();
	}

	public void fadeAction(float endValue, float time)
	{
		if (endValue <= 0.1f)
		{
			fadeOut = true;
		}
		else
		{
			fadeOut = false;
		}
		sequence.Kill();
		render.DOKill();
		render.DOFade(endValue, time).OnComplete(delegate
		{
			if (!fadeOut)
			{
				playAction();
			}
		});
	}
}
