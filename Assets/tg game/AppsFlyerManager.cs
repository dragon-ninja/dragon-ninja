using Boomlagoon.JSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class AppsFlyerManager : MonoBehaviour
{
	public static AppsFlyerManager Instance;

	private Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();

	private void Awake()
	{
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		AppsFlyer.setAppsFlyerKey("7HW3VK6cCesiqWg4V99uW6");
		AppsFlyer.setAppID("com.percent.wilknight");
		AppsFlyer.init("7HW3VK6cCesiqWg4V99uW6", "AppsFlyerTrackerCallbacks");
	}

	private void JsEval(ref string json)
	{
		for (int i = 0; i < json.Length; i++)
		{
			if (string.Equals("\\", json[i].ToString()))
			{
				json = json.Remove(i, 1);
			}
		}
	}

	public void ValidateReceipt(string publicKey, PurchaseEventArgs args, string price, string category)
	{
		purchaseEvent.Clear();
		purchaseEvent.Add("af_currency", "USD");
		purchaseEvent.Add("af_revenue", price);
		purchaseEvent.Add("af_quantity", "1");
		purchaseEvent.Add("af_content_id", args.purchasedProduct.definition.id);
		purchaseEvent.Add("af_content_type", category);
		string json = JSONObject.Parse(args.purchasedProduct.receipt).GetString("Payload");
		JsEval(ref json);
		JSONObject jSONObject = JSONObject.Parse(json);
		string json2 = jSONObject.GetString("json");
		string json3 = jSONObject.GetString("signature");
		JsEval(ref json2);
		JsEval(ref json3);
		AppsFlyer.validateReceipt("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlqZQwHczKfpPJwxpEoQyf9vXBrZg8QQaSZ1juol6n+AD4J7/+6jf4Gcrqpoe/4ZfcSd9d8A8JD08iXeh4meHZXFZ0ZijtHX1Ug3bEqkg0+AtoBtDUKeQxHEPObcXWuUNKBfYIhih0Aq29BNmf9Bp8Ouzv4BLy7gUuH+bbCWsDhFU40Nnc93EA3VljyUqGh5XTkdzKJVpzj3XmgglfKQ1JAN1ZPylxc6wRRysmiK/MThepDN7FV8+Df3F1dyWYIA9FEtcD41CIMDc7DhOzr/gALks0xKU1jXAD1gLnmFJ7ZCFiihqwvs3A/xUdq1pclR++wwYwk88+ie7hIjZso59mQIDAQAB", json2, json3, price, "USD", purchaseEvent);
	}
}
