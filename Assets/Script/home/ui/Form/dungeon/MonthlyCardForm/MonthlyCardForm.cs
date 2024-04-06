using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using AppsFlyerSDK;

public class MonthlyCardForm : BaseUIForm
{
    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            OpenForm("down_menu");
            CloseForm();
        });

        m1_Button = GetBut(this.transform, "Monthly Card_1/Button");
        m2_Button = GetBut(this.transform, "Monthly Card_2/Button");

        m1_Button.onClick.AddListener(() => {
            buyOrGetAsync(0);
            //m1_buyFlag = true;
            //Refresh();
        });
        m2_Button.onClick.AddListener(() => {
            buyOrGetAsync(1);
            //m2_buyFlag = true;
            //Refresh();
        });


        m1_buyReward = UIFrameUtil.FindChildNode
            (this.transform, "Monthly Card_1/desc1/dk/num").GetComponent<TextMeshProUGUI>();
        m1_dailyReward = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_1/desc2/dk/num").GetComponent<TextMeshProUGUI>();
        m1_ButtonImg = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_1/Button").GetComponent<Image>();
        m1_ButtonDesc = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_1/Button/desc").GetComponent<TextMeshProUGUI>();
        m1_price = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_1/Button/price").GetComponent<TextMeshProUGUI>();
        m1_getReward = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_1/Button/reward/value").GetComponent<TextMeshProUGUI>();


        m2_buyReward = UIFrameUtil.FindChildNode
          (this.transform, "Monthly Card_2/desc1/dk/num").GetComponent<TextMeshProUGUI>();
        m2_dailyReward = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_2/desc2/dk/num").GetComponent<TextMeshProUGUI>();
        m2_ButtonImg = UIFrameUtil.FindChildNode
         (this.transform, "Monthly Card_2/Button").GetComponent<Image>();
        m2_ButtonDesc = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_2/Button/desc").GetComponent<TextMeshProUGUI>();
        m2_price = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_2/Button/price").GetComponent<TextMeshProUGUI>();
        m2_getReward = UIFrameUtil.FindChildNode
           (this.transform, "Monthly Card_2/Button/reward/value").GetComponent<TextMeshProUGUI>();


        MessageMgr.AddMsgListener("PayEnd", async p =>
        {
            if (!payIng || this.dr == null)
                return;

            string[] strs = p.Value.ToString().Split('|');
            if (strs[0] == this.dr.productId) {
                UIManager.GetUIMgr().showUIForm("AwaitForm");
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "MONTHLY_CARD";
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
                    List<ItemInfo> items = new List<ItemInfo>();
                    items.Add(new ItemInfo("p10000", nowConfig.buyReward.num));
                    MessageMgr.SendMsg("GetReward", new MsgKV("", items));
                }
                Refresh();
            }
            payIng = false;
            this.dr = null;

            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(AFInAppEvents.CURRENCY, "USD");
            eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.price / 100.0f).ToString());
            eventValues.Add(AFInAppEvents.QUANTITY, "1");
            eventValues.Add(AFInAppEvents.CONTENT_TYPE, "MONTHLY_CARD_" + nowConfig.id);
            AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);

        });
    }

    NetShopPostDataReturn dr;
    bool payIng;
    MonthlyCardConfig nowConfig;

    Button m1_Button;
    Button m2_Button;

    TextMeshProUGUI m1_buyReward;
    TextMeshProUGUI m1_dailyReward;
    Image m1_ButtonImg;
    TextMeshProUGUI m1_ButtonDesc;
    TextMeshProUGUI m1_price;
    TextMeshProUGUI m1_getReward;
    /*TextMeshProUGUI m1_patrolReward;
    TextMeshProUGUI m1_patrolNum;*/

    TextMeshProUGUI m2_buyReward;
    TextMeshProUGUI m2_dailyReward;
    Image m2_ButtonImg;
    TextMeshProUGUI m2_ButtonDesc;
    TextMeshProUGUI m2_price;
    TextMeshProUGUI m2_getReward;
    /*TextMeshProUGUI m2_patrolReward;
    TextMeshProUGUI m2_patrolNum;
    TextMeshProUGUI m2_strength;*/

    MonthlyCardNetData c1;
    MonthlyCardNetData c2;

    public override void Show()
    {
        base.Show();
        Refresh();
    }


    async Task Refresh() {

        this.c1 = null;
        this.c2 = null;

        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/monthlyCard/list", DataManager.Get().getHeader());
        Debug.Log("monthlyCard:" + str);

        if (str != null) {
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            Debug.Log(NetData.data.ToString().Length);
            if (NetData.data.ToString().Trim().Length < 5 )
            {
                //没买月卡
                //Debug.Log("无monthlyCard");
            }
            else {
                JArray obj1 = (JArray)JsonConvert.DeserializeObject(NetData.data.ToString());
                List<MonthlyCardNetData> netDataList = obj1.ToObject<List<MonthlyCardNetData>>();
                //DateTime d = DateTime.Parse(netDataList[0].expTime);
                //DateTime d2 = DateTime.Parse(netDataList[0].createTime);
                //int remainDays = new TimeSpan(d.Ticks - d2.Ticks).Days;
                //Debug.Log("expTime:" + (d - d2)+"  "+ remainDays+"天");

                foreach (MonthlyCardNetData dt in netDataList ) {
                    if (dt.cardId == "1") {
                        this.c1 = dt;
                    }
                    if (dt.cardId == "2") {
                        this.c2 = dt;
                    }
                }
            }
        }
        //未购买时  点击后 向服务器发起购买请求

        //购买成功,给一次性奖励  并显示可领取状态

        //已购买时   显示领取状态,已领取则演示已领取  并禁用button

        MonthlyCardConfig config_1 = PerimeterFactory.Get().MonthlyCardConfigList[0];
        MonthlyCardConfig config_2 = PerimeterFactory.Get().MonthlyCardConfigList[1];

        Debug.Log("月卡：---------------");
        m1_buyReward.text = "x" + config_1.buyReward.num;
        m1_dailyReward.text = "x" + config_1.dailyReward.num;
        if (this.c1 == null)
        {
            m1_ButtonDesc.text = "Buy";
            m1_price.text = (config_1.price/100.0f) + "$";
            m1_price.gameObject.SetActive(true);
            m1_getReward.transform.parent.gameObject.SetActive(false);
            m1_ButtonImg.sprite = Resources.Load<Sprite>("ui/icon/黄色按钮");
        }
        else {
            m1_ButtonDesc.text = "Get";
            m1_price.gameObject.SetActive(false);
            m1_getReward.text = "x"+ config_1.dailyReward.num;
            m1_getReward.transform.parent.gameObject.SetActive(true);
            m1_ButtonImg.sprite = Resources.Load<Sprite>("ui/icon/绿色按钮");

            LayoutRebuilder.ForceRebuildLayoutImmediate(
                 m1_getReward.transform.parent.GetComponent<RectTransform>());

            //判断今天的有没有领取过
            if (c1.hasReward) {
                m1_Button.interactable = true;
            }else
                m1_Button.interactable = false;
        }


        m2_buyReward.text = "x" + config_2.buyReward.num;
        m2_dailyReward.text = "x" + config_2.dailyReward.num;
        if (this.c2 == null)
        {
            m2_ButtonDesc.text = "Buy";
            m2_price.text = (config_2.price/100.0f) + "$";
            m2_price.gameObject.SetActive(true);
            m2_getReward.transform.parent.gameObject.SetActive(false);
            m2_ButtonImg.sprite = Resources.Load<Sprite>("ui/icon/黄色按钮");
        }
        else
        {
            m2_ButtonDesc.text = "Get";
            m2_price.gameObject.SetActive(false);
            m2_getReward.text = "x" + config_2.dailyReward.num;
            m2_getReward.transform.parent.gameObject.SetActive(true);
            m2_ButtonImg.sprite = Resources.Load<Sprite>("ui/icon/绿色按钮");

            LayoutRebuilder.ForceRebuildLayoutImmediate(
                 m2_getReward.transform.parent.GetComponent<RectTransform>());

            if (c2.hasReward)
            {
                m2_Button.interactable = true;
            }
            else
                m2_Button.interactable = false;
        }
    }


    async Task buyOrGetAsync(int index) {

        if (index == 0)
        {
            if (this.c1 == null)
                buy(index);
            else
                get(index);
        }
        else {
            if (this.c2 == null)
                buy(index);
            else
                get(index);
        }
        
    }

    async Task buy(int index)
    {
        UIManager.GetUIMgr().showUIForm("AwaitForm");
        payIng = true;
        this.dr = null;

        MonthlyCardConfig nowConfig = PerimeterFactory.Get().MonthlyCardConfigList[index];
        this.nowConfig = nowConfig;
        NetShopPostData d1 = new NetShopPostData();
        d1.type = "MONTHLY_CARD";
        d1.id = nowConfig.id;
        Debug.Log("d1.id:" + d1.id);
        string json = JsonConvert.SerializeObject(d1);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());

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



        /*NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
        Debug.Log(dr.id + "结果:" + str);
        NetShopPayPostData d2 = new NetShopPayPostData();
        d2.type = "MONTHLY_CARD";
        d2.orderId = dr.id;
        d2.productId = dr.productId;
        d2.productToken = "testtesttesttest";
        string json2 = JsonConvert.SerializeObject(d2);
        string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

        Debug.Log("trypay:" + str2);


        if (str2 != null)
        {
            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(new ItemInfo("p10000", nowConfig.buyReward.num));
            MessageMgr.SendMsg("GetReward", new MsgKV("", items));
        }
        Refresh();*/
    }
    async Task get(int index)
    {
        MonthlyCardConfig nowConfig = PerimeterFactory.Get().MonthlyCardConfigList[index];

        string str2 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/monthlyCard/claimDailyRewards?cardId="+(index+1), DataManager.Get().getHeader());
        Debug.Log("trypay:" + str2);

        if (str2 != null)
        {
            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(new ItemInfo("p10000", nowConfig.dailyReward.num));
            MessageMgr.SendMsg("GetReward", new MsgKV("", items));
        }
        Refresh();
        MessageMgr.SendMsg("RefreshTip", null);
    }
}



public class MonthlyCardNetData
{ 
    public string id;     //": "64b0b6ba9826f70ea771ce9b",
    public string userId; // 227,
    /* "card": {
    "id": "1",
    "name": "Monthly Card",
    "desc": "patrol incame +5% (including gold coins and experience);\nFast patrol times +1",
    "buyReward": 500,
    "dailyReward": 150,
    "price": 6,
    "patrolReward": 0.05,
    "patrolNum": 1,
    "strength": 0
    },*/
    public string cardId;     // "1",
    public string lastRewardTime;
    public bool hasReward;
    public string createTime; // "2023-07-14T02:45:14.721+00:00",
    public string expTime;    // "2023-10-14T02:45:14.721+00:00"
}