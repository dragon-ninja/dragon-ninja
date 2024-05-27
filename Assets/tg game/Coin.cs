using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public List<Sprite> listCoinImages = new List<Sprite>();

	public SpriteRenderer spriteRenderer;

	private Transform cameraTransform;

	private Vector3 addPosition;

	private SoundManager soundManager;

	public void initObject()
	{
		soundManager = Singleton<SoundManager>.Instance;
		cameraTransform = Camera.main.transform;
		addPosition = new Vector3(4.5f, 8.7f, 0f);
	}

	public void onCoin(Vector3 targetpos, int direction, int index)
	{
		spriteRenderer.sprite = listCoinImages[index];
		base.transform.position = targetpos;
		float num = UnityEngine.Random.Range(3f, 7f);
		float jumpPower = UnityEngine.Random.Range(1f, 3f);
		float duration = UnityEngine.Random.Range(0.5f, 1f);
		int numJumps = UnityEngine.Random.Range(1, 3);
		Vector3 endValue = targetpos;
		endValue.x += (float)direction * num;
		base.transform.DOJump(endValue, jumpPower, numJumps, duration).SetEase(Ease.Flash).OnComplete(delegate
		{
			StartCoroutine(updateEndedMove());
		});
	}

	private IEnumerator updateEndedMove()
	{
		while (true)
		{
			Vector3 a = cameraTransform.position + addPosition;
			Vector3 a2 = a - base.transform.position;
			a2.Normalize();
			base.transform.position += a2 * 50f * Time.deltaTime;
			if (Vector3.Distance(a, base.transform.position) <= 1f)
			{
				break;
			}
			yield return null;
		}
		soundManager.playSound("getCoin");
		base.gameObject.SetActive(value: false);
	}
}
