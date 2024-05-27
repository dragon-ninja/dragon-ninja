using System.Collections.Generic;
using UnityEngine;

public class CustomLoadManager : Singleton<CustomLoadManager>
{
	public List<Sprite> listSprites = new List<Sprite>();

	public List<Sprite> listEnemyAttacks = new List<Sprite>();

	public List<GameObject> listEnemyBodys = new List<GameObject>();

	[Space(15f)]
	public Sprite storeSprite;

	public Sprite LoadSpriteBackground(int index)
	{
		if (index >= listSprites.Count)
		{
			return null;
		}
		return listSprites[index];
	}

	public Sprite LoadSpriteEnemyAttack(int index)
	{
		if (index >= listEnemyAttacks.Count)
		{
			return null;
		}
		return listEnemyAttacks[index];
	}

	public GameObject LoadSpriteEnemyBody(int index)
	{
		if (index >= listEnemyBodys.Count)
		{
			return null;
		}
		return listEnemyBodys[index];
	}

	public Sprite getStoreSprite()
	{
		return storeSprite;
	}
}
