using System.Collections.Generic;
using UnityEngine;

public class AssetManager : Singleton<AssetManager>
{
	public Dictionary<string, GameObject> dicLoadObjectFiles = new Dictionary<string, GameObject>();

	public Dictionary<string, Sprite> dicLoadSpriteFiles = new Dictionary<string, Sprite>();

	public List<string> listObjectKeys = new List<string>();

	public List<GameObject> listObjects = new List<GameObject>();

	public List<string> listSpriteKeys = new List<string>();

	public List<Sprite> listSprites = new List<Sprite>();

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
		dicLoadObjectFiles.Clear();
		dicLoadSpriteFiles.Clear();
		int count = listObjectKeys.Count;
		for (int i = 0; i < count; i++)
		{
			dicLoadObjectFiles.Add(listObjectKeys[i], listObjects[i]);
		}
		int count2 = listSpriteKeys.Count;
		for (int j = 0; j < count2; j++)
		{
			dicLoadSpriteFiles.Add(listSpriteKeys[j], listSprites[j]);
		}
	}

	public GameObject LoadObject(string key)
	{
		if (dicLoadObjectFiles.ContainsKey(key))
		{
			return dicLoadObjectFiles[key];
		}
		return null;
	}

	public Sprite LoadSprite(string key)
	{
		if (dicLoadSpriteFiles.ContainsKey(key))
		{
			return dicLoadSpriteFiles[key];
		}
		return null;
	}
}
