using DG.Tweening;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
	public GameObject head;

	public GameObject body;

	private void Start()
	{
		StartCoroutine(startAction());
	}

	private IEnumerator startAction()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));
		Sequence sequence = DOTween.Sequence();
		sequence.Append(head.transform.DOLocalMoveY(-0.05f, 1f).SetRelative(isRelative: true));
		sequence.Append(head.transform.DOLocalMoveY(0.05f, 1f).SetRelative(isRelative: true));
		sequence.SetEase(Ease.InOutExpo);
		sequence.SetLoops(-1);
		sequence.Play();
	}
}
