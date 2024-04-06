using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppsFlyerSDK;

public class DiamondSlot : BaseSlot
{

    //public ShopForm mgr;

    TextMeshProUGUI num;
    TextMeshProUGUI desc;
    TextMeshProUGUI price;
    DiamondAndGoldConfig nowConfig;

    NetShopPostDataReturn dr;
    bool payIng;

    protected override void Awake()
    {
        base.Awake();
        desc= UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        num = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        price = UIFrameUtil.FindChildNode(this.transform, "price").GetComponent<TextMeshProUGUI>();

        this.GetComponent<Button>().onClick.AddListener(async () => {
            UIManager.GetUIMgr().showUIForm("AwaitForm");
            payIng = true;
            this.dr = null;

            //MessageMgr.SendMsg("buyDiamond", new MsgKV("", nowConfig));
            NetShopPostData d1 = new NetShopPostData();
            d1.type = "DIAMOND_GOLD";
            d1.id = nowConfig.id;
            Debug.Log("d1.id:" + d1.id);
            string json = JsonConvert.SerializeObject(d1);
            string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());

            Debug.Log("-------------------------------------------");
            Debug.Log(str);

            if (str == null) {
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                payIng = false;
                this.dr = null;
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Error"));
                return;
            }

            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                payIng = false;
                this.dr = null;
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }


            this.dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
            UIManager.GetUIMgr().closeUIForm("AwaitForm");
            IAPTools.Instance.BuyProductByID(dr.productId);
            return;


            /*Debug.Log(dr.id+ "结果:" + str);
            NetShopPayPostData d2 = new NetShopPayPostData();
            d2.type = "DIAMOND_GOLD";
            d2.orderId = dr.id;
            d2.productId = dr.productId; //价格id
            d2.productToken = "testtesttesttest";

            string json2 = JsonConvert.SerializeObject(d2);
            string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

            Debug.Log("trypay:" + str2);

            if (str2 != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<ItemInfo> items = new List<ItemInfo>();
                items.Add(new ItemInfo(nowConfig.itemList.id, nowConfig.itemList.num))
    ;           MessageMgr.SendMsg("GetReward", new MsgKV("", items));
            }
            payIng = false;*/
        });

        MessageMgr.AddMsgListener("PayEnd", async p =>
        {
            if (!payIng || this.dr == null)
                return;

            string[] strs = p.Value.ToString().Split('|');
            if (strs[0] == this.dr.productId) {
                Debug.Log("JJJJ?");
                UIManager.GetUIMgr().showUIForm("AwaitForm");
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "DIAMOND_GOLD";
                d2.orderId = this.dr.id;
                d2.productId = this.dr.productId;
                d2.productToken = strs[1];

                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                if (str2 != null)
                {
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    List<ItemInfo> items = new List<ItemInfo>();
                    items.Add(new ItemInfo(nowConfig.itemList.id, nowConfig.itemList.num))
        ;           MessageMgr.SendMsg("GetReward", new MsgKV("", items));
                }

                payIng = false;
                this.dr = null;

                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add(AFInAppEvents.CURRENCY, "USD");
                eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.price / 100.0f).ToString());
                eventValues.Add(AFInAppEvents.QUANTITY, "1");
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "DIAMOND_GOLD_" + nowConfig.id);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });

    }

    public void Refresh(DiamondAndGoldConfig dg)
    {
        nowConfig = dg;
        num.text = dg.itemList.num + "";
        desc.text = dg.desc;
        price.text = (dg.price/100.0f) + "$";
    }
}
