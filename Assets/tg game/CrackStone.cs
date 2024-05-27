using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackStone : MonoBehaviour
{
	public List<Sprite> listStoneImages = new List<Sprite>();

	public SpriteRenderer render;

	private Vector3 targetPosition;

	private IEnumerator actionUpdate;

	private bool offUpdate;

	private void Start()
	{
		base.gameObject.SetActive(value: false);
		actionUpdate = updateStoneAction();
	}

	public void onAction(Vector3 pos)
	{
		base.gameObject.SetActive(value: true);
		targetPosition = pos;
		offUpdate = false;
		StartCoroutine(actionUpdate);
	}

	public void offAction()
	{
		offUpdate = true;
	}

	private void endedObject()
	{
		StopCoroutine(actionUpdate);
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator updateStoneAction()
	{
		do
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.5f));
			render.color = new Color(1f, 1f, 1f, 1f);
			int num = UnityEngine.Random.Range(0, listStoneImages.Count);
			render.sprite = listStoneImages[num];
			float num2 = UnityEngine.Random.Range(-3f, 3f);
			float num3 = (!(num2 < 0f)) ? 1 : (-1);
			Vector3 vector = new Vector3(targetPosition.x + num2, targetPosition.y, 0f);
			float num4 = num3 * UnityEngine.Random.Range(2f, 4f);
			float seconds = UnityEngine.Random.Range(0.5f, 1.5f);
			if (num == 0)
			{
				seconds = 0.5f;
			}
			base.transform.position = vector;
			Sequence sequence = DOTween.Sequence();
			sequence.Append(base.transform.DOLocalJump(new Vector3(vector.x + num4 * 0.6f, vector.y, vector.z), 2f, 1, 0.3f).SetEase(Ease.Flash));
			sequence.Append(base.transform.DOLocalJump(new Vector3(vector.x + num4 * 0.8f, vector.y, vector.z), 1f, 1, 0.2f).SetEase(Ease.Flash));
			sequence.Append(base.transform.DOLocalJump(new Vector3(vector.x + num4 * 1f, vector.y, vector.z), 0.5f, 1, 0.1f).SetEase(Ease.Flash));
			sequence.Play();
			yield return new WaitForSeconds(seconds);
			render.color = new Color(1f, 1f, 1f, 0f);
		}
		while (!offUpdate);
	}
}
