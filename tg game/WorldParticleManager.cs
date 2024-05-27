using System.Collections.Generic;
using UnityEngine;

public class WorldParticleManager : Singleton<WorldParticleManager>
{
	private int maxAttackNormalCount = 30;

	private int nowAttackNormalCount;

	private List<GameObject> listAttackNormalObjects = new List<GameObject>();

	private int maxAttackUniqueCount = 30;

	private int nowAttackUniqueCount;

	private List<GameObject> listAttackUniqueObjects = new List<GameObject>();

	private int maxAttackLegendaryCount = 30;

	private int nowAttackLegendaryCount;

	private List<GameObject> listAttackLegendaryObjects = new List<GameObject>();

	private List<ParticleStartColorChange> listAttackLegendarySystem = new List<ParticleStartColorChange>();

	public void initObjects()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "ParticleObjects";
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Effect/attack_nomal");
		for (int i = 0; i < maxAttackNormalCount; i++)
		{
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.position = new Vector3(0f, 0f, 0f);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.SetActive(value: false);
			listAttackNormalObjects.Add(gameObject2);
		}
		GameObject original2 = Singleton<AssetManager>.Instance.LoadObject("Effect/attack_unique");
		for (int j = 0; j < maxAttackUniqueCount; j++)
		{
			GameObject gameObject3 = Object.Instantiate(original2);
			gameObject3.transform.position = new Vector3(0f, 0f, 0f);
			gameObject3.transform.parent = gameObject.transform;
			gameObject3.SetActive(value: false);
			listAttackUniqueObjects.Add(gameObject3);
		}
		GameObject original3 = Singleton<AssetManager>.Instance.LoadObject("Effect/attack_legendary");
		for (int k = 0; k < maxAttackLegendaryCount; k++)
		{
			GameObject gameObject4 = Object.Instantiate(original3);
			gameObject4.transform.position = new Vector3(0f, 0f, 0f);
			gameObject4.transform.parent = gameObject.transform;
			gameObject4.SetActive(value: false);
			listAttackLegendaryObjects.Add(gameObject4);
			listAttackLegendarySystem.Add(gameObject4.GetComponent<ParticleStartColorChange>());
		}
	}

	public GameObject createItem(ref int max, ref int now, ref List<GameObject> list)
	{
		for (int i = 0; i < max; i++)
		{
			if (now + i >= max)
			{
				now = 0;
			}
			GameObject gameObject = list[now + i];
			if (!gameObject.gameObject.activeSelf)
			{
				now += i;
				gameObject.gameObject.SetActive(value: true);
				return gameObject;
			}
		}
		return null;
	}

	public GameObject createItemLegendary(Color color)
	{
		for (int i = 0; i < maxAttackLegendaryCount; i++)
		{
			if (nowAttackLegendaryCount + i >= maxAttackLegendaryCount)
			{
				nowAttackLegendaryCount = 0;
			}
			GameObject gameObject = listAttackLegendaryObjects[nowAttackLegendaryCount + i];
			listAttackLegendarySystem[nowAttackLegendaryCount + i].settingColor(color);
			if (!gameObject.gameObject.activeSelf)
			{
				nowAttackLegendaryCount += i;
				gameObject.gameObject.SetActive(value: true);
				return gameObject;
			}
		}
		return null;
	}

	public void createAttackParticle(Vector3 position, EquipmentData weaponData)
	{
		GameObject gameObject = null;
		if (weaponData.rank == EquipmentRank.TYPE_NORMAL)
		{
			gameObject = createItem(ref maxAttackNormalCount, ref nowAttackNormalCount, ref listAttackNormalObjects);
		}
		else if (weaponData.rank == EquipmentRank.TYPE_UNIQUE)
		{
			gameObject = createItem(ref maxAttackUniqueCount, ref nowAttackUniqueCount, ref listAttackUniqueObjects);
		}
		else
		{
			Color color = default(Color);
			switch ((int)weaponData.imageIndex)
			{
			case 11:
				color = new Color(1f, 106f / 255f, 0f, 1f);
				break;
			case 12:
				color = new Color(1f, 1f, 1f, 1f);
				break;
			case 13:
				color = new Color(0.8f, 6f / 85f, 6f / 85f, 1f);
				break;
			case 14:
				color = new Color(116f / 255f, 229f / 255f, 1f, 1f);
				break;
			case 15:
				color = new Color(181f / 255f, 0.8f, 1f, 1f);
				break;
			}
			gameObject = createItemLegendary(color);
		}
		if (gameObject != null)
		{
			gameObject.transform.position = position;
		}
	}
}
