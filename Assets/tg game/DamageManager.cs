using System.Collections.Generic;
using UnityEngine;

public class DamageManager : Singleton<DamageManager>
{
	private List<Numbers> listNumbers = new List<Numbers>();

	public int maxNumberCount;

	private int nowIndex;

	public void initObjects()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "NumberUIs";
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Objects/Number/Numbers");
		for (int i = 0; i < maxNumberCount; i++)
		{
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.parent = gameObject.transform;
			Numbers component = gameObject2.GetComponent<Numbers>();
			component.initNumbers();
			listNumbers.Add(component);
		}
	}

	public void createActionDamage(int index, Color color, Vector3 position, float scale = 1f)
	{
		Vector3 position2 = new Vector3(position.x, position.y + 2f, -8f);
		int num = 0;
		Numbers numbers;
		while (true)
		{
			if (num < maxNumberCount)
			{
				if (nowIndex + num >= maxNumberCount)
				{
					nowIndex = 0;
				}
				numbers = listNumbers[nowIndex + num];
				if (!numbers.gameObject.activeSelf)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		numbers.gameObject.SetActive(value: true);
		numbers.transform.position = position2;
		numbers.transform.localScale = new Vector3(scale, scale, 1f);
		numbers.setIndex(index, color);
		numbers.onDamageAction();
		nowIndex += num;
	}
}
