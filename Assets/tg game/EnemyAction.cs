using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
	public bool easeOut;

	public List<MovePart> listFiles = new List<MovePart>();

	public void onAction()
	{
		StartCoroutine(actionActive());
	}

	private IEnumerator actionActive()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
		int count = listFiles.Count;
		for (int i = 0; i < count; i++)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.Append(listFiles[i].fileTransform.DOLocalMoveY(0f - listFiles[i].moveDistance, 1f).SetRelative(isRelative: true));
			sequence.Append(listFiles[i].fileTransform.DOLocalMoveY(listFiles[i].moveDistance, 1f).SetRelative(isRelative: true));
			if (!easeOut)
			{
				sequence.SetEase(Ease.InOutExpo);
			}
			sequence.SetLoops(-1);
			sequence.Play();
		}
	}
}
