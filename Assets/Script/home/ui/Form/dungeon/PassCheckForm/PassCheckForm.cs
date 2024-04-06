using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AppsFlyerSDK;

public class PassCheckForm : BaseUIForm
{

    TextMeshProUGUI levelDesc;



    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        levelDesc = UIFrameUtil.FindChildNode(this.transform, "level_dk/levelDesc").GetComponent<TextMeshProUGUI>();

        GetBut(this.transform, "returnBut").onClick.AddListener(() => {
            OpenForm("down_menu");
            CloseForm();
        });

        GetBut(this.transform, "Token_1/Button").onClick.AddListener(() => {
            //unlockFlag2 = true;
            payAsync("activateToken_1");
        });

        GetBut(this.transform, "Token_2/Button").onClick.AddListener(() => {
            //unlockFlag3 = true;
            payAsync("activateToken_2");
        });


        MessageMgr.AddMsgListener("drawPassCheck", p =>
        {
           /* string type = p.Key;
            int index = (int)p.Value;

            if (type == "1")
                dataList[index].draw1 = true;
            if (type == "2")
                dataList[index].draw2 = true;
            if (type == "3")
                dataList[index].draw3 = true;
            RefreshAsync();*/
        });

        bg_img = UIFrameUtil.FindChildNode(this.transform, "bg_img").GetComponent<RectTransform>();
        level_bg = UIFrameUtil.FindChildNode(this.transform, "level_bg").GetComponent<RectTransform>();
        level_img = UIFrameUtil.FindChildNode(this.transform, "level_img").GetComponent<RectTransform>();
        Content = UIFrameUtil.FindChildNode(this.transform, "Content").GetComponent<RectTransform>();
        token_1_price = UIFrameUtil.FindChildNode(this.transform, "Token_1/Button/Text (TMP)").GetComponent<TextMeshProUGUI>();
        token_2_price = UIFrameUtil.FindChildNode(this.transform, "Token_2/Button/Text (TMP)").GetComponent<TextMeshProUGUI>();


        list_1Tra = UIFrameUtil.FindChildNode(this.transform, "1");
        slotList_1 = new List<PassCheckSlot>();
        slotPf_1 = list_1Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_1Tra.childCount; i++)
        {
            PassCheckSlot slot = list_1Tra.GetChild(i).GetComponent<PassCheckSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = "freeRewards";
            slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[0].id;
            slotList_1.Add(slot);
        }

        list_2Tra = UIFrameUtil.FindChildNode(this.transform, "2");
        slotList_2 = new List<PassCheckSlot>();
        slotPf_2 = list_2Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_2Tra.childCount; i++)
        {
            PassCheckSlot slot = list_2Tra.GetChild(i).GetComponent<PassCheckSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = "activateToken_1";
            slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[0].id;
            slotList_2.Add(slot);
        }

        list_3Tra = UIFrameUtil.FindChildNode(this.transform, "3");
        slotList_3 = new List<PassCheckSlot>();
        slotPf_3 = list_3Tra.GetChild(0).gameObject;
        for (int i = 0; i < list_3Tra.childCount; i++)
        {
            PassCheckSlot slot = list_3Tra.GetChild(i).GetComponent<PassCheckSlot>();
            slot.mgr = this;
            slot.index = i;
            slot.type = "activateToken_2";
            slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[0].id;
            slotList_3.Add(slot);
        }



        levellistTra = UIFrameUtil.FindChildNode(this.transform, "level");
        levelList = new List<PassCheckLevelSlot>();
        levelslotPf = levellistTra.GetChild(1).gameObject;
        for (int i = 0; i < levellistTra.childCount; i++)
        {
            PassCheckLevelSlot slot = levellistTra.GetChild(i).GetComponent<PassCheckLevelSlot>();
            slot.mgr = this;
            levelList.Add(slot);
        }

        MessageMgr.AddMsgListener("PayEnd", async p =>
        {
            if (!payIng || this.dr == null)
                return;

            string[] strs = p.Value.ToString().Split('|');
            if (strs[0] == this.dr.productId)
            {
                UIManager.GetUIMgr().showUIForm("AwaitForm");
                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "BATTLE_PASS";
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
                eventValues.Add(AFInAppEvents.CONTENT_TYPE, "BATTLE_PASS_" + nowConfig.tokenName);
                AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
            }
        });
    }

    GameObject slotPf_1;
    GameObject slotPf_2;
    GameObject slotPf_3;

    Transform list_1Tra;
    Transform list_2Tra;
    Transform list_3Tra;
    Transform list_1Tra_lock;
    Transform list_2Tra_lock;
    Transform list_3Tra_lock;

    List<PassCheckSlot> slotList_1;
    List<PassCheckSlot> slotList_2;
    List<PassCheckSlot> slotList_3;


    GameObject levelslotPf;
    Transform levellistTra;
    List<PassCheckLevelSlot> levelList;
    //List<PassCheckData> dataList;

    //是否解锁了付费奖励
    //bool unlockFlag2;
    //bool unlockFlag3;

    //经验条显示相关
    RectTransform bg_img;
    RectTransform level_bg;
    RectTransform level_img;
    RectTransform Content;

    TextMeshProUGUI token_1_price;
    TextMeshProUGUI token_2_price;

    public override void Show()
    {
        RefreshAsync();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        MessageMgr.SendMsg("RefreshTip", null);
    }

    public async Task RefreshAsync()
    {
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/battlePass/find", DataManager.Get().getHeader());
        Debug.Log(str);

        PassCheckNetData data = null;
        if (str != null)
        {
            data = JsonUtil.ReadData<PassCheckNetData>(str);
        }
        else {
            return;
        }


        List<BattlePassRewardsConfig> PassCheckConfigList = PerimeterFactory.Get().BattlePassRewardsList;
        Debug.Log("PassCheckConfigList[0].itemList.Count:"+PassCheckConfigList[0].itemList.Count);
        //todo 
        int token_level = data.exp / 10;
        levelDesc.text = token_level + "";


        bg_img.sizeDelta = new Vector2(bg_img.sizeDelta.x, 280 * token_level);
        level_bg.sizeDelta = new Vector2(level_bg.sizeDelta.x, 280 * PassCheckConfigList.Count-140);
        Content.sizeDelta = new Vector2(level_bg.sizeDelta.x, 280 * PassCheckConfigList.Count);
        level_img.sizeDelta = new Vector2(level_bg.sizeDelta.x, 280 * token_level - 140);


        tokenList = data.tokenList;
        TokenUnLock t1 = data.tokenList.Find(x => x.tokenName == "activateToken_1");
        TokenUnLock t2 = data.tokenList.Find(x => x.tokenName == "activateToken_2");

        if (t1.has)
        {
            GetBut(this.transform, "Token_1/Button").interactable = false;
            token_1_price.text = "Purchased";
        }
        else {
            GetBut(this.transform, "Token_1/Button").interactable = true;
            token_1_price.text = (t1.price/100.0f)+"$";
        }
        if (t2.has)
        {
            GetBut(this.transform, "Token_2/Button").interactable = false;
            token_2_price.text = "Purchased";
        }
        else
        {
            GetBut(this.transform, "Token_2/Button").interactable = true;
            token_2_price.text = (t2.price / 100.0f) + "$";
        }

        //level条动态显示
        for (int i = 0; i < PassCheckConfigList.Count; i++)
        {
            if (i >= levelList.Count)
            {
                GameObject g = Instantiate(levelslotPf, levellistTra);
                PassCheckLevelSlot slot = g.GetComponent<PassCheckLevelSlot>();
                slot.mgr = this;
                levelList.Insert(levelList.Count - 1, slot);
                g.transform.SetSiblingIndex(levellistTra.childCount - 2);
            }
        }

        //目前只做遍历显示
        for (int i = 0; i < PassCheckConfigList.Count; i++)
        {
            //数量若不足生成新的ui
            if (i >= slotList_1.Count)
            {
                GameObject g = Instantiate(slotPf_1, list_1Tra);
                PassCheckSlot slot = g.GetComponent<PassCheckSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = "freeRewards";
                slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[slotList_1.Count].id;
                slotList_1.Add(slot);
            }
            if (i >= slotList_2.Count)
            {
                GameObject g = Instantiate(slotPf_2, list_2Tra);
                PassCheckSlot slot = g.GetComponent<PassCheckSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = "activateToken_1";
                slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[slotList_2.Count].id;
                slotList_2.Add(slot);
            }
            if (i >= slotList_3.Count)
            {
                GameObject g = Instantiate(slotPf_3, list_3Tra);
                PassCheckSlot slot = g.GetComponent<PassCheckSlot>();
                slot.mgr = this;
                slot.index = i;
                slot.type = "activateToken_2";
                slot.congfig_id = PerimeterFactory.Get().BattlePassRewardsList[slotList_3.Count].id;
                slotList_3.Add(slot);
            }


            bool unlockFlag = false;
            if (i < token_level)
                unlockFlag = true;

            bool drawflag_1 = false;
            bool drawflag_2 = false;
            bool drawflag_3 = false;
            //填充ui内容
            HasRewards hasRewards = data.hasRewardsList.Find(x => x.id == PassCheckConfigList[i].id);
            if (hasRewards != null) {
                if (hasRewards.itemListId.IndexOf("freeRewards") != -1)
                    drawflag_1 = true;
                if (hasRewards.itemListId.IndexOf("activateToken_1") != -1)
                    drawflag_2 = true;
                if (hasRewards.itemListId.IndexOf("activateToken_2") != -1)
                    drawflag_3 = true;
            }





            slotList_1[i].Refresh(new ItemInfo( PassCheckConfigList[i].itemList[0].itemId, PassCheckConfigList[i].itemList[0].num), 
                unlockFlag, true, drawflag_1);
            slotList_2[i].Refresh(new ItemInfo(PassCheckConfigList[i].itemList[1].itemId, PassCheckConfigList[i].itemList[1].num),
              unlockFlag, t1.has, drawflag_2);
            slotList_3[i].Refresh(new ItemInfo(PassCheckConfigList[i].itemList[2].itemId, PassCheckConfigList[i].itemList[2].num),
              unlockFlag, t2.has, drawflag_3);

            levelList[i].Refresh(i+1, token_level);
        }
    }

    NetShopPostDataReturn dr;
    bool payIng;
    TokenUnLock nowConfig;
    List<TokenUnLock> tokenList;

    async Task payAsync(string id)
    {
        if(tokenList == null)
        {
            UIManager.GetUIMgr().closeUIForm("AwaitForm");
            payIng = false;
            this.dr = null;
            //没有网络
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
            return;
        }

        nowConfig = tokenList.Find(x => x.tokenName == id);

        UIManager.GetUIMgr().showUIForm("AwaitForm");
        payIng = true;
        this.dr = null;
        NetShopPostData d1 = new NetShopPostData();
        d1.type = "BATTLE_PASS";
        d1.id = id;

        //activateToken_1
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
            Debug.Log(str);
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
         d2.type = "BATTLE_PASS";
         d2.orderId = dr.id;
         d2.productId = dr.productId;
         d2.productToken = "testtesttesttest";
         string json2 = JsonConvert.SerializeObject(d2);
         string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

         Debug.Log("trypay:" + str2);
         //todo  服务器缺少相关购买接口
         RefreshAsync();*/
    }

}


public class PassCheckNetData
{
    public string battlePassId;
    public int exp;
    //public bool hasActivateToken_1;
    //public bool hasActivateToken_2;
    public string begin;
    public string end;
    public List<TokenUnLock> tokenList;
    public List<HasRewards> hasRewardsList;
}
public class HasRewards {
    public string id;
    public List<string> itemListId;
}

public class PassCheckNetPost
{
    public string rewardsId;
    public string tokenId;
}

public class TokenUnLock {
    public string tokenName;
    public int price;
    public bool has;
}
