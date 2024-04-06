using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

public class ShopConfirmForm : BaseUIForm
{

    public ShopFactory sf;
    public ItemFactory itemf;
    public EquipmentFactory equipmentf;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        sf = Resources.Load<ShopFactory>("mode/ShopMode");
        sf.init();
        itemf = Resources.Load<ItemFactory>("mode/ItemMode");
        itemf.init();
        equipmentf = Resources.Load<EquipmentFactory>("mode/EquipmentMode");
        equipmentf.init();



        GetComponent<Button>().onClick.AddListener(() => {
            if(!awaitFlag)
                CloseForm();
        });
        GetBut(this.transform, "DailyShopPanel/bt/close").onClick.AddListener(() => {
            if (!awaitFlag) { 
                CloseForm();
            }
        });
        GetBut(this.transform, "GoldPanel/bt/close").onClick.AddListener(() => {
            if (!awaitFlag)
                CloseForm();
        });
        GetBut(this.transform, "DiamondPanel/bt/close").onClick.AddListener(() => {
            if (!awaitFlag)
                CloseForm();
        });

        GetBut(this.transform, "SupplyBoxPanel").onClick.AddListener(() => {
            if (!awaitFlag) { 
                CloseForm();
                MessageMgr.SendMsg("GuideB_OpenBoxEnd", null);
            }
        });
        



        ChapterPackPanelTra = UIFrameUtil.FindChildNode(this.transform, "ChapterPackPanel").gameObject;
        DailyShopPanelTra = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel").gameObject;
        GoldPanelTra = UIFrameUtil.FindChildNode(this.transform, "GoldPanel").gameObject;
        DiamondPanelTra = UIFrameUtil.FindChildNode(this.transform, "DiamondPanel").gameObject;
        SupplyBoxPanelTra = UIFrameUtil.FindChildNode(this.transform, "SupplyBoxPanel").gameObject;



        //------------------------章节礼包
        Transform ChapterPackList = UIFrameUtil.FindChildNode(this.transform, "ChapterPackPanel/list");
        for (int i = 0; i < ChapterPackList.childCount; i++)
        {
            ChapterPackSlot ChapterPackSlot = ChapterPackList.GetChild(i).GetComponent<ChapterPackSlot>();
            ChapterPackSlot.mgr = this;
            chapterPackSlotList.Add(ChapterPackSlot);
        }
        chapterPackPrice = UIFrameUtil.FindChildNode
            (this.transform, "ChapterPackPanel/Button/preferentialPrice")
                .GetComponent<TextMeshProUGUI>();

        MessageMgr.AddMsgListener("buyChapterPack", p =>
        {
            closeAllPanel();
            ChapterPackPanelTra.SetActive(true);
            string id = (string)p.Value;
            ChapterPackConfig config = sf.ChapterPackList.Find(item => item.id == id);
            RefreshChapterPackPanel(config);
        });



        //------------------------每日商店

        DailyShop_itemName = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/bt/name").
          GetComponent<TextMeshProUGUI>();
        DailyShop_icon = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/dk/icon").
            GetComponent<Image>();
        DailyShop_dk = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/dk").
            GetComponent<Image>();
        DailyShop_num = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/dk/Text (TMP)").
            GetComponent<TextMeshProUGUI>();
        DailyShop_priceImg = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/Button/gold/Image").
            GetComponent<Image>();
        DailyShop_price = UIFrameUtil.FindChildNode(this.transform, "DailyShopPanel/Button/gold/value").
            GetComponent<TextMeshProUGUI>();
        MessageMgr.AddMsgListener("buyDailyShop", p =>
        {
            closeAllPanel();
            DailyShopPanelTra.SetActive(true);
            NetDailyShopInfoData data = (NetDailyShopInfoData)p.Value;
            RefreshDailyShopPanel(data);
        });

        GetBut(this.transform, "DailyShopPanel/Button").onClick.AddListener(async () => {
            NetShopPostData d1 = new NetShopPostData();
            d1.type = "DAILY_SHOP";
            d1.id= this.DailyShop_nowData.id;
            d1.shopDailyListId = this.DailyShop_nowData.dailyId;
            string json = JsonConvert.SerializeObject(d1);
            string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json , DataManager.Get().getHeader());


            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null) {
                //显示资源不够
                OpenForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }

            Debug.Log("结果:" + str);
            NetShopPostDataReturn dr  = JsonUtil.ReadData<NetShopPostDataReturn>(str);



            NetShopPayPostData d2 = new NetShopPayPostData();
            d2.type = "DAILY_SHOP";
            d2.orderId = dr.id;
            d2.productId = dr.productId;

            string json2 = JsonConvert.SerializeObject(d2);
            string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

            Debug.Log("trypay:" + str2);

            CloseForm();
            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(new ItemInfo(DailyShop_nowData.itemId, DailyShop_nowData.num, DailyShop_nowData.quality));           
            MessageMgr.SendMsg("GetReward", new MsgKV("", items));
        });




        //------------------------购买钻石
        Diamond_num = UIFrameUtil.FindChildNode(this.transform,
            "DiamondPanel/dk/Text (TMP)").
            GetComponent<TextMeshProUGUI>();
        Diamond_price = UIFrameUtil.FindChildNode(this.transform,
            "DiamondPanel/Button/preferentialPrice").
            GetComponent<TextMeshProUGUI>();

        MessageMgr.AddMsgListener("buyDiamond", p =>
        {
            closeAllPanel();
            DiamondPanelTra.SetActive(true);
            DiamondAndGoldConfig dg = (DiamondAndGoldConfig)p.Value; ;
            RefreshDiamondPanel(dg);
        });



        //------------------------购买金币
        Gold_name = UIFrameUtil.FindChildNode(this.transform,
           "GoldPanel/bt/name").GetComponent<TextMeshProUGUI>();
        Gold_num = UIFrameUtil.FindChildNode(this.transform,
           "GoldPanel/dk/Text (TMP)").GetComponent<TextMeshProUGUI>();
        Gold_price = UIFrameUtil.FindChildNode(this.transform,
            "GoldPanel/Button/gold/value").GetComponent<TextMeshProUGUI>();

        MessageMgr.AddMsgListener("buyGold", p =>
        {
            closeAllPanel();
            GoldPanelTra.SetActive(true);
            DiamondAndGoldConfig dg = (DiamondAndGoldConfig)p.Value; ;
            RefreshGoldPanel(dg);
        });

        //-------------------开箱子
        SupplyBoxList = new List<ItemSlot>();
        Transform SupplyBoxTra = UIFrameUtil.FindChildNode(this.transform, "SupplyBoxPanel/list");
        for (int i = 0; i < SupplyBoxTra.childCount; i++)
        {
            ItemSlot SupplyBoxSlot = SupplyBoxTra.GetChild(i).GetComponent<ItemSlot>();
            SupplyBoxSlot.mgr = this;
            SupplyBoxList.Add(SupplyBoxSlot);
        }



        //开箱子
        MessageMgr.AddMsgListener("openBox", p =>
        {
            openBoxAsync(p.Key, (int)p.Value);
        });



        closeAllPanel();
    }


    //弹出每日
    GameObject ChapterPackPanelTra;
    List<ChapterPackSlot> chapterPackSlotList = new List<ChapterPackSlot>();
    TextMeshProUGUI chapterPackPrice;

    void RefreshChapterPackPanel(ChapterPackConfig cf)
    {
        chapterPackPrice.text = "Pay " + cf.preferentialPrice + "$";
        for (int i = 0; i < chapterPackSlotList.Count; i++)
        {
            ItemInfo info = cf.itemList[i];
            chapterPackSlotList[i].Refresh(info);
        }
    }



    GameObject DailyShopPanelTra;
    TextMeshProUGUI DailyShop_itemName;
    Image DailyShop_dk;
    Image DailyShop_icon;
    TextMeshProUGUI DailyShop_num;
    //货币类型图标
    Image DailyShop_priceImg;
    TextMeshProUGUI DailyShop_price;
    NetDailyShopInfoData DailyShop_nowData;

    //弹出购买界面
    void RefreshDailyShopPanel(NetDailyShopInfoData data)
    {
        DailyShop_nowData = data;
        //根据id找到相应的物品图标

        string iconUrl = null;


        if (data.currency == "gem")
        {
            iconUrl = equipmentf.map[data.itemId].icon;
            DailyShop_itemName.text = equipmentf.map[data.itemId].name;
            DailyShop_num.gameObject.SetActive(false);
        }
        else
        {
            iconUrl = itemf.itemMap[data.itemId].icon;
            DailyShop_itemName.text = itemf.itemMap[data.itemId].name;
            DailyShop_num.text = "x" + data.num;
            DailyShop_num.gameObject.SetActive(true);
        }

        //显示图标
        DailyShop_icon.sprite = Resources.Load<Sprite>(iconUrl);

        //Debug.Log("data.price:"+ data.price+"   "+ data.preferential);

        DailyShop_price.text = "x" + data.discountPrice;
        DailyShop_priceImg.gameObject.SetActive(true);

        if (data.currency == "gem")
            DailyShop_priceImg.sprite = Resources.Load<Sprite>("ui/icon/钻石");
        else if (data.currency == "gold")
            DailyShop_priceImg.sprite = Resources.Load<Sprite>("ui/icon/gold");
        else
        {
            DailyShop_priceImg.gameObject.SetActive(false);
            DailyShop_price.text = "Free";
        }

        DailyShop_dk.sprite = Resources.Load<Sprite>("ui/icon/item/dk/" + data.quality);

        LayoutRebuilder.ForceRebuildLayoutImmediate
            (DailyShop_price.transform.parent.GetComponent<RectTransform>());
    }

    //------------------------
    GameObject GoldPanelTra;
    TextMeshProUGUI Gold_name;
    TextMeshProUGUI Gold_num;
    TextMeshProUGUI Gold_price;
    void RefreshGoldPanel(DiamondAndGoldConfig dg)
    {
        Gold_name.text = dg.desc;
        Gold_num.text = dg.itemList.num + "";
        Gold_price.text = dg.price + "";

        LayoutRebuilder.ForceRebuildLayoutImmediate
          (Gold_price.transform.parent.GetComponent<RectTransform>());
    }


    //------------------------
    GameObject DiamondPanelTra;
    TextMeshProUGUI Diamond_num;
    TextMeshProUGUI Diamond_price;

    void RefreshDiamondPanel(DiamondAndGoldConfig dg) {
        Diamond_num.text = "x" + dg.itemList.num;
        Diamond_price.text = "Pay " + dg.price + "$";
    }


    //------------------------
    GameObject SupplyBoxPanelTra;
    public List<ItemSlot> SupplyBoxList;
    bool awaitFlag;

    async Task openBoxAsync(string boxId, int num) {
        showTime = 0;
        closeAllPanel();
        awaitFlag = true;
        NetShopPostData d1 = new NetShopPostData();
        d1.type = "SUPPLY_CRATE";
        d1.id = boxId;
        d1.isTen = num == 10;
        Debug.Log("d1.id:" + d1.id);
        string json = JsonConvert.SerializeObject(d1);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());


        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode != null)
        {
            //订单创建失败
            awaitFlag = false;
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            CloseForm();
            return;
        }


        NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
        Debug.Log(dr.id + "结果:" + str);
        NetShopPayPostData d2 = new NetShopPayPostData();
        d2.type = "SUPPLY_CRATE";
        d2.orderId = dr.id;
        d2.productId = dr.productId;
        d2.productToken = "Gem pay";
        string json2 = JsonConvert.SerializeObject(d2);
        string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());
        
        Debug.Log("trypay:" + str2);

        if (str2 != null)
        {

            JObject obj2 = (JObject)JsonConvert.DeserializeObject(str2);
            NetData NetData2 = obj2.ToObject<NetData>();

            Debug.Log(NetData2.errorCode+ "NetData2.errorCode:" + (NetData2.errorCode != null /*&& NetData2.errorCode != "null"*/));

            if (NetData2.errorCode != null )
            {
                awaitFlag = false;
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("",  NetData2.message));
                CloseForm();
                return;
            }
            else {
                SupplyBoxPanelTra.SetActive(true);
                for (int i = 0; i < 10; i++)
                    SupplyBoxList[i].Hide();


                JArray obj3 = (JArray)JsonConvert.DeserializeObject(NetData2.data.ToString());
                List<ItemInfo3> infoList = obj3.ToObject<List<ItemInfo3>>();
              
                MessageMgr.SendMsg("UpMenuRefresh",
                         new MsgKV("", null));
                //UIManager.GetUIMgr().showUIForm("RewardForm");
                //MessageMgr.SendMsg("GetReward", new MsgKV("", itemList));
                MessageMgr.SendMsg("RefreshBoxKey", new MsgKV("", null));


                StartCoroutine(showBoxItems(infoList));
                /* int index = 0;
                 foreach (var info in infoList)
                 {
                     SupplyBoxList[index++].Refresh(new ItemInfo(info.equipmentId, 1, info.quality));
                 }*/
                //开箱完之后预加载背包界面
                UIManager.GetUIMgr().preload("BackPackForm");
            }
        }
        else {
            awaitFlag = false;
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "network error"));
            return;
        }
      
        return;



        /*if (type == "EternalBox")
        {

            if (num == 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    SupplyBoxList[i].gameObject.SetActive(false);
                }

                string id = EquipmentFactory.Get().list[Random.Range(0, EquipmentFactory.Get().list.Count)].id;
                EquipmentData wp1 = new EquipmentData(id, 1, Random.Range(1, 4));

                SupplyBoxList[0].Refresh(wp1);
                SupplyBoxList[0].gameObject.SetActive(true);
            }
            //十连
            else {

                for (int i = 0; i < 10; i++) {
                    string id = EquipmentFactory.Get().list[Random.Range(0, EquipmentFactory.Get().list.Count)].id;
                    EquipmentData wp1 = new EquipmentData(id, 1, Random.Range(1, 4));

                    SupplyBoxList[i].Refresh(wp1);
                    SupplyBoxList[i].gameObject.SetActive(true);
                }

            }

        }
        else if (type.IndexOf("seniorBox") != -1)
        {
            for (int i = 0; i < 10; i++)
            {
                SupplyBoxList[i].gameObject.SetActive(false);
            }
            string id = EquipmentFactory.Get().list[Random.Range(0, EquipmentFactory.Get().list.Count)].id;
            EquipmentData wp1 = new EquipmentData(id, 1, Random.Range(1, 4));

            SupplyBoxList[0].Refresh(wp1);
            SupplyBoxList[0].gameObject.SetActive(true);
        }
        else {
            for (int i = 0; i < 10; i++)
            {
                SupplyBoxList[i].gameObject.SetActive(false);
            }

            string id = EquipmentFactory.Get().list[Random.Range(0, EquipmentFactory.Get().list.Count)].id;
            EquipmentData wp1 = new EquipmentData(id, 1, Random.Range(1, 4));
            SupplyBoxList[0].Refresh(wp1);
            SupplyBoxList[0].gameObject.SetActive(true);
        }*/
    }
    IEnumerator showBoxItems(List<ItemInfo3>  infoList)
    {
        yield return new WaitForSeconds(0.5f);
        int index = 0;
        foreach (var info in infoList)
        {
            SupplyBoxList[index++].Refresh(new ItemInfo(info.equipmentId, 1, info.quality));
        }
        awaitFlag = false;
    }




    float showTime;
    /*private void Update()
    {
        showTime += Time.deltaTime;
    }*/




    void closeAllPanel() {
        ChapterPackPanelTra.SetActive(false);
        DailyShopPanelTra.SetActive(false);
        GoldPanelTra.SetActive(false);
        DiamondPanelTra.SetActive(false);
        SupplyBoxPanelTra.SetActive(false);
    }
}
