using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
	private int maxCount = 100;

	private int nowCount;

	private List<Item> listItemObjects = new List<Item>();

	private List<Item> listLiveItems = new List<Item>();

	public void initObjects()
	{
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Prefebs/ItemObject");
		GameObject gameObject = new GameObject();
		gameObject.name = "ItemObjects";
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < maxCount; i++)
		{
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.position = new Vector3(0f, 0f, 0f);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.SetActive(value: false);
			Item component = gameObject2.GetComponent<Item>();
			component.settingItem();
			listItemObjects.Add(component);
		}
	}

	public Item createItem()
	{
		for (int i = 0; i < maxCount; i++)
		{
			if (nowCount + i >= maxCount)
			{
				nowCount = 0;
			}
			Item item = listItemObjects[nowCount + i];
			if (!item.gameObject.activeSelf)
			{
				nowCount += i;
				item.gameObject.SetActive(value: true);
				return item;
			}
		}
		return null;
	}

	public void createRandomItem(Vector3 targetPos, float direction)
	{
		Item item = createItem();
		if (!(item == null))
		{
			item.onItem((ItemState)Random.Range(0, 3), 
				new EquipmentData(EquipmentType.TYPE_ARMOR,
				EquipmentRank.TYPE_NORMAL,
				EquipmentGrade.TYPE_D, 0, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			item.onActionJump(targetPos, direction);
			listLiveItems.Add(item);
		}
	}

	public void createEquipment(Vector3 targetPos, float direction, EquipmentData data)
	{
		Item item = createItem();
		if (!(item == null))
		{
			item.onItem(ItemState.TYPE_EQUIPMENT, data);
			item.onActionJump(targetPos, direction);
			listLiveItems.Add(item);
		}
	}

	public void createEgg(Vector3 targetPos, float direction)
	{
		Item item = createItem();
		if (!(item == null))
		{
			item.onItem(ItemState.TYPE_EGG, new EquipmentData(EquipmentType.TYPE_ARMOR, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 0, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			item.onActionJump(targetPos, direction);
			listLiveItems.Add(item);
		}
	}

	public Item crashCheck(Vector3 playerPos)
	{
		Item result = null;
		int count = listLiveItems.Count;
		for (int i = 0; i < count; i++)
		{
			if (listLiveItems[i].isLife() && Mathf.Abs(listLiveItems[i].gameObject.transform.position.x - playerPos.x) < 1.5f)
			{
				result = listLiveItems[i];
			}
		}
		return result;
	}

	public void removeItem(Item item)
	{
		item.removeItem();
		listLiveItems.Remove(item);
	}
}
