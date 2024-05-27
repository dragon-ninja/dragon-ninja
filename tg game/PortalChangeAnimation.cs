using DG.Tweening;
using UnityEngine;

public class PortalChangeAnimation : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public float animationTime;

	private changeDelegate endedCallback;

	public void setAnimationTime(float t)
	{
		animationTime = t;
	}

	public void onPortal()
	{
		base.gameObject.SetActive(value: true);
		spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
		spriteRenderer.DOFade(1f, animationTime).OnComplete(delegate
		{
			endedCallback();
		});
	}

	public void offPortal()
	{
		base.gameObject.SetActive(value: true);
		spriteRenderer.color = new Color(0f, 0f, 0f, 1f);
		spriteRenderer.DOFade(0f, animationTime).OnComplete(delegate
		{
			endedCallback();
		});
	}

	public void setEndedCallback(changeDelegate ended)
	{
		endedCallback = ended;
	}
}
