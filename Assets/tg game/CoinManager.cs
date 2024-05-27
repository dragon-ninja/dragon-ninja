using System.Collections.Generic;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
	private int coinCut1 = 1;

	private int coinCut2 = 5;

	private int coinCut3 = 10;

	private int coinCut4 = 50;

	private int coinCut5 = 100;

	private int maxCount = 100;

	private int nowCount;

	private List<Coin> listCoinObjects = new List<Coin>();

	public void initObjects()
	{
		GameObject original = Singleton<AssetManager>.Instance.LoadObject("Prefebs/CoinObject");
		GameObject gameObject = new GameObject();
		gameObject.name = "CoinObjects";
		gameObject.transform.position = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < maxCount; i++)
		{
			GameObject gameObject2 = Object.Instantiate(original);
			gameObject2.transform.position = new Vector3(0f, 0f, 0f);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.SetActive(value: false);
			Coin component = gameObject2.GetComponent<Coin>();
			component.initObject();
			listCoinObjects.Add(component);
		}
	}

	public Coin createCoin()
	{
		for (int i = 0; i < maxCount; i++)
		{
			if (nowCount + i >= maxCount)
			{
				nowCount = 0;
			}
			Coin coin = listCoinObjects[nowCount + i];
			if (!coin.gameObject.activeSelf)
			{
				nowCount += i;
				coin.gameObject.SetActive(value: true);
				return coin;
			}
		}
		return null;
	}

	public void onCoins(Vector3 targetPos, int direction, int coin)
	{
		int createCount = 10;
		int coin2 = coin;
		createTypeByCoins(targetPos, direction, nextCoinCount(ref coin2, coinCut5), 4, ref createCount);
		createTypeByCoins(targetPos, direction, nextCoinCount(ref coin2, coinCut4), 3, ref createCount);
		createTypeByCoins(targetPos, direction, nextCoinCount(ref coin2, coinCut3), 2, ref createCount);
		createTypeByCoins(targetPos, direction, nextCoinCount(ref coin2, coinCut2), 1, ref createCount);
		createTypeByCoins(targetPos, direction, nextCoinCount(ref coin2, coinCut1), 0, ref createCount);
	}

	private int nextCoinCount(ref int coin, int target)
	{
		int result = coin / target;
		coin %= target;
		return result;
	}

	private void createTypeByCoins(Vector3 targetPos, int direction, int count, int type, ref int createCount)
	{
		if (createCount <= 0)
		{
			return;
		}
		createCount -= count;
		if (createCount < 0)
		{
			count += createCount;
		}
		for (int i = 0; i < count; i++)
		{
			Coin coin = createCoin();
			if (coin != null)
			{
				coin.onCoin(targetPos, direction, type);
			}
		}
	}
}
