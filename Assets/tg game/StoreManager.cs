using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
{
	private List<Vector3> listStorePositions = new List<Vector3>();

	private List<Transform> listStoreTransforms = new List<Transform>();

	private int lastCrashIndex;

	public void initObjects(int level, List<Vector3> pos)
	{
		if (level != 0)
		{
			listStorePositions = pos;
			GameObject gameObject = new GameObject();
			gameObject.name = "Store Objects";
			Sprite storeSprite = Singleton<CustomLoadManager>.Instance.getStoreSprite();
			int count = listStorePositions.Count;
			for (int i = 0; i < count; i++)
			{
				Vector3 position = listStorePositions[i];
				GameObject gameObject2 = new GameObject();
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.position = position;
				listStoreTransforms.Add(gameObject2.transform);
				gameObject2.AddComponent<SpriteRenderer>().sprite = storeSprite;
			}
		}
	}

	public Transform crashObject(Vector3 playerPosition)
	{
		int count = listStorePositions.Count;
		for (int i = 0; i < count; i++)
		{
			if (Mathf.Abs(listStorePositions[i].x - playerPosition.x) <= 3f)
			{
				lastCrashIndex = i;
				return listStoreTransforms[i];
			}
		}
		return null;
	}

	public int getCrashStoreIndex()
	{
		return lastCrashIndex;
	}
}
