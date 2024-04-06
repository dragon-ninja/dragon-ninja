using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppsFlyerSDK;

public class FirstChargeForm : BaseUIForm
{
    TextMeshProUGUI desc;
    TextMeshProUGUI price;

    List<ItemSlot> ItemSlotList;
    Transform slotListTra;
    GameObject slotPf;

    NetShopPostDataReturn dr;
    bool payIng;
    ChargeConfig nowConfig;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        price = UIFrameUtil.FindChildNode(this.transform, "button/Text (TMP)").GetComponent<TextMeshProUGUI>();

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });

        slotListTra = UIFrameUtil.FindChildNode(this.transform, "list");
        ItemSlotList = new List<ItemSlot>();
        slotPf = slotListTra.GetChild(0).gameObject;
        for (int i = 0; i < slotListTra.childCount; i++)
        {
            ItemSlot slot = slotListTra.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            ItemSlotList.Add(slot);
        }

        List<ChargeConfig> configList = PerimeterFactory.Get().ChargeList;
        nowConfig = configList[0];

        GetBut(this.transform,"button").onClick.AddListener(async () => {
            UIManager.GetUIMgr().showUIForm("AwaitForm");
            payIng = true;
            this.dr = null;

            NetShopPostData d1 = new NetShopPostData();
            d1.type = "CHARGE";
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


            /* NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
             Debug.Log(dr.id + "结果:" + str);
             NetShopPayPostData d2 = new NetShopPayPostData();
             d2.type = "CHARGE";
             d2.orderId = dr.id;
             d2.productId = dr.productId;
             d2.productToken = "testtesttesttest";
             string json2 = JsonConvert.SerializeObject(d2);
             string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

             Debug.Log("trypay:" + str2);
             if (str2 == null)
             {
                 //没有网络
                 UIManager.GetUIMgr().showUIForm("ErrForm");
                 MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
                 return;
             }

             if (str2 != null)
             {
                 UIManager.GetUIMgr().showUIForm("RewardForm");

                 MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.items));
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
                d2.type = "CHARGE";
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

                UIManager.GetUIMgr().closeUIForm("AwaitForm");
                if (str2 != null)
                {
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    MessageMgr.SendMsg("GetReward", new MsgKV("", nowConfig.items));
                }

                payIng = false;
                this.dr = null;


                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add(AFInAppEvents.CURRENCY, "USD");
                eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.preferentialPrice/100.0f).ToString());
                eventValues.Add(AFInAppEvents.QUANTITY, "1");
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "CHARGE_" + nowConfig.id);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });
    }


    public override void Show()
    {
        base.Show();
        Refresh();
    }



    public void Refresh()
    {

        List<ChargeConfig> configList = PerimeterFactory.Get().ChargeList;
        ChargeConfig config = configList[0];

        desc.text = config.desc;
        price.text = (config.preferentialPrice/100.0f)+ "$";

        for (int i = 0; i < ItemSlotList.Count; i++)
            ItemSlotList[i].Hide();

        for (int i = 0; i < config.items.Count; i++)
        {
            if (i >= ItemSlotList.Count)
            {
                GameObject g = Instantiate(slotPf, slotListTra);
                ItemSlot slot = g.GetComponent<ItemSlot>();
                slot.mgr = this;
                ItemSlotList.Add(slot);
            }
            ItemSlotList[i].Refresh(config.items[i]);
        }
    }
}
