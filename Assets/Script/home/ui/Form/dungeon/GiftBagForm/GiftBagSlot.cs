using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppsFlyerSDK;

public class GiftBagSlot : BaseSlot
{
    TextMeshProUGUI price;
    List<ItemSlot> itemSlotList = new List<ItemSlot>();

    GiftBagConfig nowConfig;

    Transform maskTra;
    Transform superValueTra;
    Transform imgTra;

    NetShopPostDataReturn dr;
    bool payIng;

    protected override void Awake()
    {
        init();
    }

    private void init()
    {
        if (initFlag)
            return;

        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        price = UIFrameUtil.FindChildNode(this.transform, "price").GetComponent<TextMeshProUGUI>();
        maskTra = UIFrameUtil.FindChildNode(this.transform, "mask");
        superValueTra = UIFrameUtil.FindChildNode(this.transform, "super value");
        imgTra = UIFrameUtil.FindChildNode(this.transform, "img");

        Transform list = UIFrameUtil.FindChildNode(this.transform, "list");

        for (int i = 0; i < list.childCount; i++)
        {
            ItemSlot slot = list.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            itemSlotList.Add(slot);
        }

        this.GetComponent<Button>().onClick.AddListener(async () => {

            UIManager.GetUIMgr().showUIForm("AwaitForm");
            payIng = true;
            this.dr = null;

            NetShopPostData d1 = new NetShopPostData();
            d1.type = "GIFT_BAG";
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
            //paper_cut_gid_0
            //Debug.Log("<<<<<<<<<<<<<<<<    " + dr.productId);
            if (dr.productId == "paper_cut_gid_0")
            {
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "GIFT_BAG";
                d2.orderId = dr.id;
                d2.productId = dr.productId;
                d2.productToken = "free";
                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);
                if (str2 == null)
                {
                    UIManager.GetUIMgr().closeUIForm("AwaitForm");
                    payIng = false;
                    this.dr = null;
                    //没有网络
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                    MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
                    return;
                }
                ((GiftBagForm)mgr).RefreshAsync();
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                if (str2 != null)
                {
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.itemList));
                }

                payIng = false;
                this.dr = null;

                ((GiftBagForm)mgr).RefreshAsync();
                MessageMgr.SendMsg("RefreshTip", null);
            }
            else { 
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                IAPTools.Instance.BuyProductByID(dr.productId);
                return;
            }


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
                d2.type = "GIFT_BAG";
                d2.orderId = dr.id;
                d2.productId = dr.productId;
                d2.productToken = strs[1];
                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);
                if (str2 == null)
                {
                    UIManager.GetUIMgr().closeUIForm("AwaitForm");
                    payIng = false;
                    this.dr = null;
                    //没有网络
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                    MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
                    return;
                }
                ((GiftBagForm)mgr).RefreshAsync();
                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                if (str2 != null)
                {
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.itemList));
                }

                payIng = false;
                this.dr = null;

                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add(AFInAppEvents.CURRENCY, "USD");
                eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.preferentialPrice / 100.0f).ToString());
                eventValues.Add(AFInAppEvents.QUANTITY, "1");
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "GIFT_BAG_" + nowConfig.id);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });

    }

    public void Refresh(GiftBagConfig cf,bool buyFlag = false)
    {
        Show();

        if (buyFlag)
        {
            maskTra.gameObject.SetActive(true);
            if(superValueTra!=null)
                superValueTra.gameObject.SetActive(false);
            if (imgTra != null)
                imgTra.gameObject.SetActive(false);
            this.GetComponent<Button>().interactable = false;
        }
        else {
            maskTra.gameObject.SetActive(false);
            if (superValueTra != null)
                superValueTra.gameObject.SetActive(true);
            if (imgTra != null)
                imgTra.gameObject.SetActive(true);
            this.GetComponent<Button>().interactable = true;
        }


        nowConfig = cf;
        price.text = (cf.preferentialPrice/100.0f) + "$";
        for (int i = 0; i < itemSlotList.Count; i++)
        {
            ItemInfo info = cf.itemList[i];
            itemSlotList[i].Refresh(info);
        }
    }
}
