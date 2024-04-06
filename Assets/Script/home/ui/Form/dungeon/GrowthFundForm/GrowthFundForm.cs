using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AppsFlyerSDK;

public class GrowthFundForm : BaseUIForm
{
    GameObject slotPf_1;
    GameObject slotPf_2;
    GameObject slotPf_3;

    RectTransform Content;

    Transform list_1Tra;
    Transform list_2Tra;
    Transform list_3Tra;
    Transform list_1Tra_lock;
    Transform list_2Tra_lock;
    Transform list_3Tra_lock;

    List<GrowthFundSlot> slotList_1;
    List<GrowthFundSlot> slotList_2;
    List<GrowthFundSlot> slotList_3;
    List<GrowthFundSlot> slotList_1_lock;
    List<GrowthFundSlot> slotList_2_lock;
    List<GrowthFundSlot> slotList_3_lock;

    GameObject levelslotPf;
    Transform levellistTra;
    List<GrowthFundLevelSlot> levelList;
    List<GrowthFundData> dataList;

    Button pay1;
    Button pay2;
    TextMeshProUGUI text_pay_1;
    TextMeshProUGUI text_pay_2;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        Content = UIFrameUtil.FindChildNode(this.transform, "Content").GetComponent<RectTransform>();
        text_pay_1 = UIFrameUtil.FindChildNode(this.transform, "Pay_Button (1)/Text (TMP)").GetComponent<TextMeshProUGUI>();
        text_pay_2 = UIFrameUtil.FindChildNode(this.transform, "Pay_Button (2)/Text (TMP)").GetComponent<TextMeshProUGUI>();

        GetBut(this.transform, "returnBut").onClick.AddListener(() => {
            OpenForm("down_menu");
            CloseForm();
        });

        pay1 = GetBut(this.transform, "Pay_Button (1)");
        pay1.onClick.AddListener(async () => {
            nowConfig = PerimeterFactory.Get().GrowthFundPriceConfigList[0];
            payAsync("reward_1");
        });

        pay2 = GetBut(this.transform, "Pay_Button (2)");
        pay2.onClick.AddListener(() => {
            nowConfig = PerimeterFactory.Get().GrowthFundPriceConfigList[1];
            payAsync("reward_2");
        });


        MessageMgr.AddMsgListener("drawGrowthFund", async p =>
        {

            int index = int.Parse(p.Key);
            NetGrowthFundPost post = new NetGrowthFundPost();
            post.level = PerimeterFactory.Get().GrowthFundConfigList[index].level;
            post.type = (int)p.Value;
            string json = JsonConvert.SerializeObject(post);
            string str= await NetManager.post(ConfigCheck.publicUrl+"/data/pub/growthFund/claimGrowthRewards", json, DataManager.Get().getHeader());
            Debug.Log(str);


            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }
            else {
                if ((bool)NetData.data) {

                    List<ItemInfo> itList = new List<ItemInfo>();

                    if (post.type == 0) {
                        itList.Add(PerimeterFactory.Get().GrowthFundConfigList[index].reward_0);
                    }
                    else if (post.type == 1)
                    {
                        itList.Add(PerimeterFactory.Get().GrowthFundConfigList[index].reward_1);
                    }
                    else if (post.type == 2)
                    {
                        itList.Add(PerimeterFactory.Get().GrowthFundConfigList[index].reward_2);
                    }

                    //显示获得奖励
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    MessageMgr.SendMsg("GetReward", new MsgKV("", itList));
                }
            }
            RefreshAsync();
            MessageMgr.SendMsg("RefreshTip", null);
        });


        list_1Tra = UIFrameUtil.FindChildNode(this.transform, "1 unlock");
        slotList_1 = new List<GrowthFundSlot>();
        slotPf_1 = list_1Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_1Tra.childCount; i++)
        {
            GrowthFundSlot slot = list_1Tra.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 0;
            slotList_1.Add(slot);
        }

        list_2Tra = UIFrameUtil.FindChildNode(this.transform, "2 unlock");
        slotList_2 = new List<GrowthFundSlot>();
        slotPf_2 = list_2Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_2Tra.childCount; i++)
        {
            GrowthFundSlot slot = list_2Tra.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 1;
            slotList_2.Add(slot);
        }

        list_3Tra = UIFrameUtil.FindChildNode(this.transform, "3 unlock");
        slotList_3 = new List<GrowthFundSlot>();
        slotPf_3 = list_3Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_3Tra.childCount; i++)
        {
            GrowthFundSlot slot = list_3Tra.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 2;
            slotList_3.Add(slot);
        }

        //-----------lock
        list_1Tra_lock = UIFrameUtil.FindChildNode(this.transform, "1 lock");
        slotList_1_lock = new List<GrowthFundSlot>();
        for (int i = 0; i < list_1Tra_lock.childCount; i++)
        {
            GrowthFundSlot slot = list_1Tra_lock.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 0;
            slotList_1_lock.Add(slot);
        }

        list_2Tra_lock = UIFrameUtil.FindChildNode(this.transform, "2 lock");
        slotList_2_lock = new List<GrowthFundSlot>();
        for (int i = 0; i < list_2Tra_lock.childCount; i++)
        {
            GrowthFundSlot slot = list_2Tra_lock.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 1;
            slotList_2_lock.Add(slot);
        }

        list_3Tra_lock = UIFrameUtil.FindChildNode(this.transform, "3 lock");
        slotList_3_lock = new List<GrowthFundSlot>();
        for (int i = 0; i < list_3Tra_lock.childCount; i++)
        {
            GrowthFundSlot slot = list_3Tra_lock.GetChild(i).GetComponent<GrowthFundSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = 2;
            slotList_3_lock.Add(slot);
        }

        levellistTra = UIFrameUtil.FindChildNode(this.transform, "level");
        levelList = new List<GrowthFundLevelSlot>();
        levelslotPf = levellistTra.GetChild(1).gameObject;
        for (int i = 0; i < levellistTra.childCount; i++)
        {
            GrowthFundLevelSlot slot = levellistTra.GetChild(i).GetComponent<GrowthFundLevelSlot>();
            slot.mgr = this;
            levelList.Add(slot);
        }



        List<GrowthFundConfig> GrowthFundConfigList = PerimeterFactory.Get().GrowthFundConfigList;
        dataList = new List<GrowthFundData>();
        for (int i = 0; i < GrowthFundConfigList.Count; i++) {
            dataList.Add(new GrowthFundData(GrowthFundConfigList[i].id));
        }

        MessageMgr.AddMsgListener("PayEnd", async p =>
        {
            if (!payIng || this.dr == null)
                return;

            string[] strs = p.Value.ToString().Split('|');
            if (strs[0] == this.dr.productId)
            {
                Debug.Log("JJJJ?");
                UIManager.GetUIMgr().showUIForm("AwaitForm");
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "GROWTH_FUNDS";
                d2.orderId = dr.id;
                d2.productId = dr.productId;
                d2.productToken = strs[1];
                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);
                RefreshAsync();
                UIManager.GetUIMgr().closeUIForm("AwaitForm");


                payIng = false;
                this.dr = null;

                Dictionary<string, string> eventValues = new Dictionary<string, string>();
                eventValues.Add(AFInAppEvents.CURRENCY, "USD");
                eventValues.Add(AFInAppEvents.REVENUE, (nowConfig.price / 100.0f).ToString());
                eventValues.Add(AFInAppEvents.QUANTITY, "1");
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "GROWTH_FUNDS_" + nowConfig.id);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });
    }
    


    public override void Show()
    {
        RefreshAsync();
        base.Show();
    }

    NetGrowthFundData data = null;
    async Task RefreshAsync() {


        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/growthFund/getGrowthFund", DataManager.Get().getHeader());
        Debug.Log(str);
        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();

        if (NetData.errorCode != null)
        {
            //显示资源不够
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            return;
        }
        else {
            data = JsonUtil.ReadData<NetGrowthFundData>(str);
        }



        if (data.hasReward1)
        {
            pay1.interactable = false;
            text_pay_1.text = "Purchased";
        }
        else {
            pay1.interactable = true;
            text_pay_1.text = "Buy\n"+ (PerimeterFactory.Get().GrowthFundPriceConfigList[0].price/100.0f) + "$";
        }

        if (data.hasReward2)
        {
            pay2.interactable = false;
            text_pay_2.text = "Purchased";
        }
        else
        {
            pay2.interactable = true;
            text_pay_2.text = "Buy\n" + (PerimeterFactory.Get().GrowthFundPriceConfigList[1].price / 100.0f) + "$";
        }


        List<GrowthFundConfig> GrowthFundConfigList = PerimeterFactory.Get().GrowthFundConfigList;

        for (int i = 0; i < GrowthFundConfigList.Count; i++) {
            if (i >= levelList.Count)
            {
                GameObject g = Instantiate(levelslotPf, levellistTra);
                GrowthFundLevelSlot slot = g.GetComponent<GrowthFundLevelSlot>();
                slot.mgr = this;
                levelList.Insert(levelList.Count - 1, slot);
                g.transform.SetSiblingIndex(levellistTra.childCount - 2);
            }
        }

        //目前只做遍历显示
        for (int i=0;i< GrowthFundConfigList.Count;i++) {
            //数量若不足生成新的ui
            if (i >= slotList_1.Count)
            {
                GameObject g = Instantiate(slotPf_1, list_1Tra);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type =0;
                slotList_1.Add(slot);
            }
            if (i >= slotList_2.Count)
            {
                GameObject g = Instantiate(slotPf_2, list_2Tra);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = 1;
                slotList_2.Add(slot);
            }
            if (i >= slotList_3.Count)
            {
                GameObject g = Instantiate(slotPf_3, list_3Tra);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = 2;
                slotList_3.Add(slot);
            }
            //----------lock
            if (i >= slotList_1_lock.Count)
            {
                GameObject g = Instantiate(slotPf_1, list_1Tra_lock);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = 0;
                slotList_1_lock.Add(slot);
            }
            if (i >= slotList_2_lock.Count)
            {
                GameObject g = Instantiate(slotPf_2, list_2Tra_lock);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = 1;
                slotList_2_lock.Add(slot);
            }
            if (i >= slotList_3_lock.Count)
            {
                GameObject g = Instantiate(slotPf_3, list_3Tra_lock);
                GrowthFundSlot slot = g.GetComponent<GrowthFundSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = 2;
                slotList_3_lock.Add(slot);
            }



            bool unlockFlag = unlockFlag = GrowthFundConfigList[i].level <= DataManager.Get().roleAttrData.nowLevel;


            //data.hasReward1
            //data.hasReward2


            //填充ui内容
            if (unlockFlag)
                slotList_1[i].Refresh(GrowthFundConfigList[i].reward_0, unlockFlag,true, 
                    data.reward0.Find(x=>x.level== GrowthFundConfigList[i].level)!=null);
            else
                slotList_1[i].Hide();
            slotList_1_lock[i].Refresh(GrowthFundConfigList[i].reward_0, false, true,
                    data.reward0.Find(x => x.level == GrowthFundConfigList[i].level) != null);



            if (unlockFlag)
                slotList_2[i].Refresh(GrowthFundConfigList[i].reward_1, unlockFlag, data.hasReward1,
                    data.reward1.Find(x => x.level == GrowthFundConfigList[i].level) != null);
            else
                slotList_2[i].Hide();
            slotList_2_lock[i].Refresh(GrowthFundConfigList[i].reward_1, false, data.hasReward1,
                    data.reward1.Find(x => x.level == GrowthFundConfigList[i].level) != null);




            if (unlockFlag)
                slotList_3[i].Refresh(GrowthFundConfigList[i].reward_2, unlockFlag, data.hasReward2,
                    data.reward2.Find(x => x.level == GrowthFundConfigList[i].level) != null);
            else
                slotList_3[i].Hide();
            slotList_3_lock[i].Refresh(GrowthFundConfigList[i].reward_2, false, data.hasReward2,
                    data.reward2.Find(x => x.level == GrowthFundConfigList[i].level) != null);


            //todo...  已经达到相应等级需要显示等级进度
            levelList[i].Refresh(GrowthFundConfigList[i].level, DataManager.Get().roleAttrData.nowLevel);
        }

        Content.sizeDelta = new Vector2(Content.sizeDelta.x, list_1Tra_lock.GetComponent<RectTransform>().sizeDelta.y);
        StartCoroutine(positioning());
    }

    IEnumerator positioning()
    {
        yield return new WaitForSeconds(0.1f);
        Content.sizeDelta = new Vector2(Content.sizeDelta.x, list_1Tra_lock.GetComponent<RectTransform>().sizeDelta.y);
    }

    NetShopPostDataReturn dr;
    bool payIng;
    GrowthFundPriceConfig nowConfig;
    async Task payAsync(string id) {

        UIManager.GetUIMgr().showUIForm("AwaitForm");
        payIng = true;
        this.dr = null;

        NetShopPostData d1 = new NetShopPostData();
        d1.type = "GROWTH_FUNDS";
        d1.id =  id;

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
        d2.type = "GROWTH_FUNDS";
        d2.orderId = dr.id;
        d2.productId = dr.productId;
        d2.productToken = "testtesttesttest";
        string json2 = JsonConvert.SerializeObject(d2);
        string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

        Debug.Log("trypay:" + str2);
        RefreshAsync();*/
    }
}
public class NetGrowthFundData {
    public bool hasReward1;
    public bool hasReward2;
    public List<ItemInfo2> reward0;
    public List<ItemInfo2> reward1;
    public List<ItemInfo2> reward2;
}

public class NetGrowthFundPost
{
    public int level;
    public int type;
}