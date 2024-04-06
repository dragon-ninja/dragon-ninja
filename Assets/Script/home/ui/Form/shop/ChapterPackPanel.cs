using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppsFlyerSDK;

public class ChapterPackPanel : BaseSlot
{

    //TextMeshProUGUI name;
    TextMeshProUGUI desc;
    TextMeshProUGUI price;
    TextMeshProUGUI preferentialPrice;
    List<ChapterPackSlot> chapterPackSlotList = new List<ChapterPackSlot>();
    ChapterPackConfig nowConfig;

    NetShopPostDataReturn dr;
    bool payIng;

    protected override void Awake()
    {
        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        desc = UIFrameUtil.FindChildNode(this.transform, "desc/name").GetComponent<TextMeshProUGUI>();
        price = UIFrameUtil.FindChildNode(this.transform, "Button/price").GetComponent<TextMeshProUGUI>();
        preferentialPrice = UIFrameUtil.FindChildNode(this.transform, "Button/preferentialPrice").GetComponent<TextMeshProUGUI>();

        Transform list = UIFrameUtil.FindChildNode(this.transform, "list");

        for (int i = 0; i < list.childCount; i++)
        {
            ChapterPackSlot ChapterPackSlot = list.GetChild(i).GetComponent<ChapterPackSlot>();
            ChapterPackSlot.mgr = this;
            chapterPackSlotList.Add(ChapterPackSlot);
        }

        this.GetComponent<Button>().onClick.AddListener(async () => {
            UIManager.GetUIMgr().showUIForm("AwaitForm");
            payIng = true;
            this.dr = null;


            NetShopPostData d1 = new NetShopPostData();
            d1.type = "CHAPTER_PACK";
            d1.id = nowConfig.id;
            Debug.Log("d1.id:" + d1.id);
            string json = JsonConvert.SerializeObject(d1);
            string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());

            if (str == null)
            {
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                payIng = false;
                this.dr = null;
                //没有网络
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
                return;
            }

            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                payIng = false;
                this.dr = null;
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }

            this.dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
            UIManager.GetUIMgr().closeUIForm("AwaitForm");

            IAPTools.Instance.BuyProductByID(dr.productId);
            return;

            /*NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
            Debug.Log(dr.id + "结果:" + str);
            NetShopPayPostData d2 = new NetShopPayPostData();
            d2.type = "CHAPTER_PACK";
            d2.orderId = dr.id;
            d2.productId = dr.productId;
            d2.productToken = "testtesttesttest";
            string json2 = JsonConvert.SerializeObject(d2);
            string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

            Debug.Log("trypay:" + str2);

            if (str2 != null) {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.itemList));
                MessageMgr.SendMsg("RefreshBoxKey", new MsgKV("", null));
            }*/
        });

        MessageMgr.AddMsgListener("PayEnd", async p =>
        {
            if (!payIng || this.dr == null)
                return;

            string[] strs = p.Value.ToString().Split('|');
            if (strs[0] == this.dr.productId)
            {
                UIManager.GetUIMgr().showUIForm("AwaitForm");
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "CHAPTER_PACK";
                d2.orderId = dr.id;
                d2.productId = dr.productId;
                d2.productToken = strs[1];
                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);

                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                if (str2 != null)
                {
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.itemList));
                    MessageMgr.SendMsg("RefreshBoxKey", new MsgKV("", null));
                }

                payIng = false;
                this.dr = null;

                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add(AFInAppEvents.CURRENCY, "USD");
                eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.preferentialPrice / 100.0f).ToString());
                eventValues.Add(AFInAppEvents.QUANTITY, "1");
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "CHAPTER_PACK_" + nowConfig.id);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });
    }

    public void Refresh(ChapterPackConfig cf) {
        
        nowConfig = cf;

        desc.text = cf.name;
        price.text = Mathf.RoundToInt(cf.price/100.0f) +"$";
        preferentialPrice.text = (cf.preferentialPrice/100.0f) +"$";
        //chapterPackSlotList

        for (int i=0; i< chapterPackSlotList.Count;i++) {
            ItemInfo info = cf.itemList[i];
            chapterPackSlotList[i].Refresh(info);
        }

        GetComponent<Image>().sprite = Resources.Load<Sprite>(cf.imgPath);
    }
}

