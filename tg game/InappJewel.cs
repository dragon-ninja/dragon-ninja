using DG.Tweening;
using System.Collections;
using UnityEngine;

public class InappJewel : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(startAction());
	}

	private IEnumerator startAction()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
		float duration = UnityEngine.Random.Range(2f, 3f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOLocalMoveY(50f, duration).SetRelative(isRelative: true));
		sequence.Append(base.transform.DOLocalMoveY(-50f, duration).SetRelative(isRelative: true));
		sequence.SetLoops(-1);
		sequence.Play();
	}
}
