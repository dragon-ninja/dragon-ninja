using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
	public float waveSize = 100f;

	public float enemyDistance;

	public float waveStart;

	public float storeSize;

	public float bossSize;

	private List<List<Enemy>> listEnemys = new List<List<Enemy>>();

	private List<Vector3> listStorePositions = new List<Vector3>();

	private int killBossCount = 1;

	public bool checkStageClear(int index)
	{
		return killBossCount <= index;
	}

	public List<Vector3> getListStorePositions()
	{
		return listStorePositions;
	}

	public void initObjects(int stage, int level, float maxWidthSize, float startY)
	{
		switch (stage)
		{
		case 0:
		case 8:
			break;
		case 7:
			waveSize = 27f;
			enemyDistance = 5.4f;
			killBossCount = 7;
			createCrackEnemys(level, maxWidthSize, startY);
			break;
		default:
			createStageEnemys(stage, level, maxWidthSize, startY);
			break;
		}
	}

	public void createStageEnemys(int stage, int level, float maxWidthSize, float startY)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		gameObject.name = "Enemys";
		float num = maxWidthSize - bossSize;
		float num2 = waveStart;
		float num3 = 0f;
		int num4 = 0;
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Enemy");
		listEnemys.Add(new List<Enemy>());
		int num5 = 0;
		while (true)
		{
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = new Vector3(num2, startY, 0.1f * (float)num5);
			EnemyType randomEnemyType = getRandomEnemyType(num4);
			Enemy component = gameObject2.GetComponent<Enemy>();
			component.initEnemy(randomEnemyType, level, gameObject2.transform.position, stage, maxWidthSize, (int)(randomEnemyType - 1));
			listEnemys[num4].Add(component);
			num3 += enemyDistance;
			if (num3 >= waveSize)
			{
				listStorePositions.Add(new Vector3(num2 + storeSize / 2f, startY, 100f));
				num2 += storeSize;
				num3 = 0f;
				listEnemys.Add(new List<Enemy>());
				num4++;
			}
			else
			{
				num2 += enemyDistance;
			}
			if (num2 >= num)
			{
				break;
			}
			num5++;
		}
		GameObject gameObject3 = Object.Instantiate(original);
		gameObject3.transform.parent = gameObject.transform;
		gameObject3.transform.position = new Vector3(maxWidthSize - bossSize * 0.333f, startY, 0f);
		Enemy component2 = gameObject3.GetComponent<Enemy>();
		component2.initEnemy(EnemyType.TYPE_BOSS, level, gameObject3.transform.position, stage, maxWidthSize, 4);
		listEnemys[num4].Add(component2);
	}

	public void createCrackEnemys(int level, float maxWidthSize, float startY)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		gameObject.name = "Enemys";
		float num = maxWidthSize - bossSize;
		float num2 = waveStart;
		float num3 = 0f;
		int num4 = 0;
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Prefebs/Enemy");
		listEnemys.Add(new List<Enemy>());
		int num5 = 0;
		while (true)
		{
			float x = num2;
			if (num5 % 5 == 4)
			{
				x = num2 - enemyDistance * 0.4f;
			}
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.position = new Vector3(x, startY, 0.1f * (float)num5);
			int num6 = (num5 + 1) % 5;
			num6 = ((num6 == 0) ? 5 : num6);
			EnemyType t = (EnemyType)num6;
			Enemy component = gameObject2.GetComponent<Enemy>();
			component.initEnemy(t, level, gameObject2.transform.position, 7, maxWidthSize, num5);
			listEnemys[num4].Add(component);
			num3 += enemyDistance * 0.8f;
			if (num5 % 5 == 3)
			{
				num3 += enemyDistance;
				num2 += enemyDistance;
			}
			if (num3 >= waveSize)
			{
				listStorePositions.Add(new Vector3(num2 + storeSize / 2f, startY, 0f));
				num2 += storeSize;
				num3 = 0f;
				listEnemys.Add(new List<Enemy>());
				num4++;
			}
			else
			{
				num2 += enemyDistance;
			}
			if (num2 >= num || num5 >= 29)
			{
				break;
			}
			num5++;
		}
		GameObject gameObject3 = Object.Instantiate(original);
		gameObject3.transform.parent = gameObject.transform;
		gameObject3.transform.position = new Vector3(maxWidthSize - bossSize * 0.333f, startY, 0f);
		Enemy component2 = gameObject3.GetComponent<Enemy>();
		component2.initEnemy(EnemyType.TYPE_BOSS, level, gameObject3.transform.position, 7, maxWidthSize, 30);
		listEnemys[num4].Add(component2);
	}

	public Enemy createKnightEnemy(int level, float maxWidthSize, float startY)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		gameObject.name = "Enemys";
		float bossSize2 = bossSize;
		float waveStart2 = waveStart;
		listEnemys.Add(new List<Enemy>());
		GameObject gameObject2 = Object.Instantiate(Singleton<AssetManager>.Instance.LoadObject("Prefebs/Enemy"));
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.position = new Vector3(76.08f, startY, 0f);
		Enemy component = gameObject2.GetComponent<Enemy>();
		component.initEnemy(EnemyType.TYPE_BOSS, level, gameObject2.transform.position, 8, maxWidthSize, 0);
		listEnemys[0].Add(component);
		return component;
	}

	private EnemyType getRandomEnemyType(int waveCount)
	{
		float num = (waveCount - 1) * 4;
		float num2 = waveCount * 8;
		float num3 = waveCount * 13;
		float num4 = Random.Range(0f, 90f);
		if (num4 <= num)
		{
			return EnemyType.TYPE_4;
		}
		if (num4 <= num2)
		{
			return EnemyType.TYPE_3;
		}
		if (num4 <= num3)
		{
			return EnemyType.TYPE_2;
		}
		return EnemyType.TYPE_1;
	}

	public int getPlaceIndex(Vector3 pos)
	{
		int count = listStorePositions.Count;
		for (int i = 0; i < count; i++)
		{
			if (listStorePositions[i].x > pos.x)
			{
				return i;
			}
		}
		if (count > 0 && listStorePositions[count - 1].x < pos.x)
		{
			return count;
		}
		return 0;
	}

	public Enemy crashOldPosition(Vector3 playerPos, Vector3 playerOldPos, List<Enemy> list)
	{
		float x;
		float x2;
		if (playerPos.x > playerOldPos.x)
		{
			x = playerPos.x;
			x2 = playerOldPos.x;
		}
		else
		{
			x = playerOldPos.x;
			x2 = playerPos.x;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i].isLife())
			{
				Vector3 position = list[i].transform.position;
				if (x2 < position.x && position.x < x)
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public List<Enemy> crashOldPositionMultiple(Vector3 playerPos, Vector3 playerOldPos, List<Enemy> list)
	{
		List<Enemy> list2 = new List<Enemy>();
		float x;
		float x2;
		if (playerPos.x > playerOldPos.x)
		{
			x = playerPos.x;
			x2 = playerOldPos.x;
		}
		else
		{
			x = playerOldPos.x;
			x2 = playerPos.x;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i].isLife())
			{
				Vector3 position = list[i].transform.position;
				if (x2 < position.x && position.x < x)
				{
					list2.Add(list[i]);
				}
			}
		}
		return list2;
	}

	public List<Enemy> crashOldPositionMultiple(Vector3 playerPos, Vector3 playerOldPos)
	{
		int placeIndex = getPlaceIndex(playerPos);
		if (listEnemys.Count <= placeIndex)
		{
			return new List<Enemy>();
		}
		List<Enemy> list = listEnemys[placeIndex];
		return crashOldPositionMultiple(playerPos, playerOldPos, list);
	}

	public Enemy crashDistance(Vector3 playerPos, float distance, List<Enemy> list)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i].isLife() && Mathf.Abs(playerPos.x - list[i].transform.position.x) < distance + ((list[i].GetEnemyType() == EnemyType.TYPE_BOSS) ? distance : 0f))
			{
				return list[i];
			}
		}
		return null;
	}

	public Enemy crashCheck(Vector3 playerPos, Vector3 playerOldPos, float distance)
	{
		int placeIndex = getPlaceIndex(playerPos);
		if (listEnemys.Count <= placeIndex)
		{
			return null;
		}
		List<Enemy> list = listEnemys[placeIndex];
		Enemy enemy = crashOldPosition(playerPos, playerOldPos, list);
		if (enemy == null)
		{
			enemy = crashDistance(playerPos, distance, list);
		}
		return enemy;
	}

	public Enemy crashCheck(Vector3 playerPos, float distance)
	{
		int placeIndex = getPlaceIndex(playerPos);
		if (listEnemys.Count <= placeIndex)
		{
			return null;
		}
		List<Enemy> list = listEnemys[placeIndex];
		return crashDistance(playerPos, distance, list);
	}

	public void resetEnemyState(int crashStoreIndex, bool frontReset)
	{
		int count = listEnemys.Count;
		if (frontReset)
		{
			if (count <= crashStoreIndex + 1)
			{
				return;
			}
			bool flag = false;
			List<Enemy> list = listEnemys[crashStoreIndex + 1];
			int count2 = list.Count;
			for (int i = 0; i < count2; i++)
			{
				if (list[i].isLife())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				resetEnemys(list, flip: false);
			}
		}
		else
		{
			if (count > crashStoreIndex + 1)
			{
				resetEnemys(listEnemys[crashStoreIndex + 1], flip: false);
			}
			if (count > crashStoreIndex)
			{
				resetEnemys(listEnemys[crashStoreIndex], flip: true);
			}
		}
	}

	public Enemy getVectorDistanceEnemy(Vector3 pos, float distance)
	{
		int placeIndex = getPlaceIndex(pos);
		if (listEnemys.Count <= placeIndex)
		{
			return null;
		}
		Vector2 a = new Vector2(pos.x, pos.y);
		List<Enemy> list = listEnemys[placeIndex];
		Enemy result = null;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (list[i].isLife())
			{
				Vector2 b = new Vector2(list[i].transform.position.x, list[i].transform.position.y);
				if (Vector2.Distance(a, b) < distance)
				{
					result = list[i];
					break;
				}
			}
		}
		return result;
	}

	public List<Enemy> getDistanceEnemys(Vector3 pos, float distance)
	{
		List<Enemy> list = new List<Enemy>();
		int placeIndex = getPlaceIndex(pos);
		if (listEnemys.Count <= placeIndex)
		{
			return list;
		}
		List<Enemy> list2 = listEnemys[placeIndex];
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			if (list2[i].isLife() && Mathf.Abs(pos.x - list2[i].transform.position.x) < distance + ((list2[i].GetEnemyType() == EnemyType.TYPE_BOSS) ? distance : 0f))
			{
				list.Add(list2[i]);
			}
		}
		return list;
	}

	public List<Enemy> getDistanceEnemys(List<Vector3> pos, float distance)
	{
		List<Enemy> list = new List<Enemy>();
		int count = listEnemys.Count;
		for (int i = 0; i < count; i++)
		{
			int count2 = listEnemys[i].Count;
			for (int j = 0; j < count2; j++)
			{
				if (!listEnemys[i][j].isLife())
				{
					continue;
				}
				int count3 = pos.Count;
				for (int k = 0; k < count3; k++)
				{
					if (Mathf.Abs(pos[k].x - listEnemys[i][j].transform.position.x) < distance + ((listEnemys[i][j].GetEnemyType() == EnemyType.TYPE_BOSS) ? distance : 0f))
					{
						list.Add(listEnemys[i][j]);
						break;
					}
				}
			}
		}
		return list;
	}

	private void resetEnemys(List<Enemy> enemys, bool flip)
	{
		int count = enemys.Count;
		for (int i = 0; i < count; i++)
		{
			enemys[i].restart(flip);
		}
	}
}
