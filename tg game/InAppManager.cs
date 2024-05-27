using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppManager : Singleton<InAppManager>, IStoreListener
{
	private static IStoreController storeController;

	private static IExtensionProvider extensionProvider;

	public List<ProductData> listProductDatas = new List<ProductData>();

	private inappDelegate buyCallback;

	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		listProductDatas = new List<ProductData>
		{
			new ProductData("wilknight_card_armor", ProductType.Consumable),
			new ProductData("wilknight_card_helmet", ProductType.Consumable),
			new ProductData("wilknight_card_weapon", ProductType.Consumable),
			new ProductData("wilknight_card_horse", ProductType.Consumable),
			new ProductData("wilknight_gold1", ProductType.Consumable),
			new ProductData("wilknight_gold2", ProductType.Consumable),
			new ProductData("wilknight_gold3", ProductType.Consumable),
			new ProductData("wilknight_pack1", ProductType.NonConsumable),
			new ProductData("wilknight_pack2", ProductType.NonConsumable),
			new ProductData("wilknight_pack3", ProductType.Consumable)
		};
		InitializePurchasing();
	}

	private bool IsInitialized()
	{
		if (storeController != null)
		{
			return extensionProvider != null;
		}
		return false;
	}

	public void InitializePurchasing()
	{
		if (!IsInitialized())
		{
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			string text = "";
			text = "GooglePlay";
			int count = listProductDatas.Count;
			for (int i = 0; i < count; i++)
			{
				configurationBuilder.AddProduct(listProductDatas[i].strID, listProductDatas[i].type, new IDs
				{
					{
						listProductDatas[i].strID,
						text
					}
				});
			}
			UnityEngine.Debug.Log("inapp init");
			configurationBuilder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlqZQwHczKfpPJwxpEoQyf9vXBrZg8QQaSZ1juol6n+AD4J7/+6jf4Gcrqpoe/4ZfcSd9d8A8JD08iXeh4meHZXFZ0ZijtHX1Ug3bEqkg0+AtoBtDUKeQxHEPObcXWuUNKBfYIhih0Aq29BNmf9Bp8Ouzv4BLy7gUuH+bbCWsDhFU40Nnc93EA3VljyUqGh5XTkdzKJVpzj3XmgglfKQ1JAN1ZPylxc6wRRysmiK/MThepDN7FV8+Df3F1dyWYIA9FEtcD41CIMDc7DhOzr/gALks0xKU1jXAD1gLnmFJ7ZCFiihqwvs3A/xUdq1pclR++wwYwk88+ie7hIjZso59mQIDAQAB");
			UnityPurchasing.Initialize(this, configurationBuilder);
		}
	}

	//发起购买请求
	public void BuyProductID(string productId, inappDelegate callback)
	{
		buyCallback = null;
		buyCallback = callback;
		try
		{
			if (IsInitialized())
			{
				Product product = storeController.products.WithID(productId);
				if (product != null && product.availableToPurchase)
				{
					UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
					storeController.InitiatePurchase(product);
				}
				else
				{
					UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}
		catch (Exception arg)
		{
			UnityEngine.Debug.Log("BuyProductID: FAIL. Exception during purchase. " + arg);
		}
	}

	public void RestorePurchase(inappDelegate callback)
	{
		buyCallback = callback;
		if (!IsInitialized())
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
			if (buyCallback != null)
			{
				buyCallback(succssed: false);
			}
			buyCallback = null;
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEngine.Debug.Log("RestorePurchases started ...");
			extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(delegate(bool result)
			{
				UnityEngine.Debug.Log("RestorePurchases continuing: " + result.ToString() + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			if (buyCallback != null)
			{
				buyCallback(succssed: false);
			}
			buyCallback = null;
		}
	}

	public void OnInitialized(IStoreController sc, IExtensionProvider ep)
	{
		UnityEngine.Debug.Log("OnInitialized : PASS");
		storeController = sc;
		extensionProvider = ep;
	}

	public void OnInitializeFailed(InitializationFailureReason reason)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + reason);
	}

	//购买处理逻辑
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
		if (!Singleton<DataManager>.Instance.getLoadState())
		{
			UnityEngine.Debug.Log("DataManager Not Load");
			Singleton<DataManager>.Instance.loadDataAsync();
		}
		switch (args.purchasedProduct.definition.id)
		{
		case "wilknight_card_armor":
			Singleton<DataManager>.Instance.listArmors.Add(new EquipmentData(EquipmentType.TYPE_ARMOR, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			break;
		case "wilknight_card_helmet":
			Singleton<DataManager>.Instance.listHelmets.Add(new EquipmentData(EquipmentType.TYPE_HELMET, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			break;
		case "wilknight_card_weapon":
			Singleton<DataManager>.Instance.listWeapons.Add(new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			break;
		case "wilknight_card_horse":
			Singleton<DataManager>.Instance.listHorses.Add(new EquipmentData(EquipmentType.TYPE_HORSE, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			break;
		case "wilknight_gold1":
		{
			DataManager instance6 = Singleton<DataManager>.Instance;
			instance6.coinCount = (int)instance6.coinCount + 25000;
			instance6.maxCoinCount = (int)instance6.maxCoinCount + 25000;
			break;
		}
		case "wilknight_gold2":
		{
			DataManager instance5 = Singleton<DataManager>.Instance;
			instance5.coinCount = (int)instance5.coinCount + 77000;
					instance5.maxCoinCount = (int)instance5.maxCoinCount + 77000;
					break;
		}
		case "wilknight_gold3":
		{
			DataManager instance4 = Singleton<DataManager>.Instance;
			instance4.coinCount = (int)instance4.coinCount + 132000;
			instance4.maxCoinCount = (int)instance4.maxCoinCount + 132000;
			break;
		}
		case "wilknight_pack1":
			if (!Singleton<DataManager>.Instance.noAds)
			{
				Singleton<DataManager>.Instance.noAds = true;
				DataManager instance2 = Singleton<DataManager>.Instance;
				instance2.eggCount = (int)instance2.eggCount + 10;
				DataManager instance3 = Singleton<DataManager>.Instance;
				instance3.coinCount = (int)instance3.coinCount + 25000;
				instance3.maxCoinCount = (int)instance3.maxCoinCount + 25000;
				Singleton<DataManager>.Instance.listWeapons.Add(new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			}
			break;
		case "wilknight_pack2":
		{
			bool flag = false;
			int count = Singleton<DataManager>.Instance.listPets.Count;
			for (int i = 0; i < count; i++)
			{
				int num = Singleton<DataManager>.Instance.listPets[i].imageIndex;
				if (num == 21 || num == 22 || num == 23)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				PetData data = new PetData(EquipmentRank.TYPE_SUPERLEGENDARY, 0, 21);
				EquipmentObjects.settingRankByStatus(ref data);
				Singleton<DataManager>.Instance.listPets.Add(data);
			}
			break;
		}
		case "wilknight_pack3":
		{
			Singleton<DataManager>.Instance.listArmors.Add(new EquipmentData(EquipmentType.TYPE_ARMOR, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			Singleton<DataManager>.Instance.listHelmets.Add(new EquipmentData(EquipmentType.TYPE_HELMET, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			Singleton<DataManager>.Instance.listWeapons.Add(new EquipmentData(EquipmentType.TYPE_WEAPON, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			Singleton<DataManager>.Instance.listHorses.Add(new EquipmentData(EquipmentType.TYPE_HORSE, EquipmentRank.TYPE_NORMAL, EquipmentGrade.TYPE_D, 10000, 0, 0, 0f, 0, 0, 0f, 0, 0f, 0, 0, 0f));
			DataManager instance = Singleton<DataManager>.Instance;
			instance.coinCount = (int)instance.coinCount + 10000;
			instance.maxCoinCount = (int)instance.maxCoinCount + 10000;
			break;
		}
		}
		Singleton<DataManager>.Instance.saveDataAsync();
		if (buyCallback != null)
		{
			buyCallback(succssed: true, args);
		}
		buyCallback = null;
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
		if (buyCallback != null)
		{
			buyCallback(succssed: false);
		}
		buyCallback = null;
	}
}
