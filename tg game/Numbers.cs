using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour
{
	private List<Number> listNumbers = new List<Number>();

	private int index;

	private float distance;

	private Color color;

	public void initNumbers()
	{
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Objects/Number/Number");
		for (int i = 0; i < 7; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			gameObject.SetActive(value: false);
			gameObject.transform.parent = base.transform;
			Number component = gameObject.GetComponent<Number>();
			listNumbers.Add(component);
		}
		base.gameObject.SetActive(value: false);
	}

	public void resetNumbers()
	{
		int count = listNumbers.Count;
		for (int i = 0; i < count; i++)
		{
			listNumbers[i].gameObject.SetActive(value: false);
		}
		base.gameObject.SetActive(value: false);
	}

	public void setIndex(int i, Color col, float dis = 0.4f)
	{
		index = i;
		distance = dis;
		color = col;
		settingIndexNumbers();
	}

	public void onDamageAction()
	{
		float playTime = 2f;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOMove(new Vector3(0f, 1f, 0f), playTime).SetRelative(isRelative: true));
		sequence.InsertCallback(playTime * 0.5f, delegate
		{
			int count = listNumbers.Count;
			for (int i = 0; i < count; i++)
			{
				listNumbers[i].render.DOFade(0f, playTime * 0.5f);
			}
			StartCoroutine(damageActionEnd(playTime * 0.5f));
		});
		sequence.Play();
	}

	private IEnumerator damageActionEnd(float i)
	{
		yield return new WaitForSeconds(i);
		resetNumbers();
	}

	private void settingIndexNumbers()
	{
		int[] array = digitArr(index);
		int num = array.Length;
		float num2 = 0f;
		if (num != 1)
		{
			num2 = ((num % 2 != 0) ? (num2 - (float)((num - 1) / 2) * distance) : (num2 - (distance / 2f + (float)(num / 2) * distance)));
		}
		for (int i = 0; i < num; i++)
		{
			Number number = listNumbers[i];
			number.setNumber(array[i], color);
			number.transform.localPosition = new Vector3(num2, 0f, 0f);
			number.gameObject.SetActive(value: true);
			num2 += distance;
		}
	}

	private static int numDigits(int n)
	{
		if (n < 10)
		{
			return 1;
		}
		if (n < 100)
		{
			return 2;
		}
		if (n < 1000)
		{
			return 3;
		}
		if (n < 10000)
		{
			return 4;
		}
		if (n < 100000)
		{
			return 5;
		}
		if (n < 1000000)
		{
			return 6;
		}
		if (n < 10000000)
		{
			return 7;
		}
		if (n < 100000000)
		{
			return 8;
		}
		if (n < 1000000000)
		{
			return 9;
		}
		return 10;
	}

	private static int[] digitArr(int n)
	{
		int[] array = new int[numDigits(n)];
		for (int num = array.Length - 1; num >= 0; num--)
		{
			array[num] = n % 10;
			n /= 10;
		}
		return array;
	}
}
