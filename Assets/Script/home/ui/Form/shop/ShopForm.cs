using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopForm : BaseUIForm
{
    public ShopFactory sf;
    public ItemFactory itemf;
    public EquipmentFactory equipmentf;
    Scrollbar scrollbar;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.HideOther;
        ui_type.IsClearStack = false;

        UIManager.GetUIMgr().preload("ShopConfirmForm");

        sf = Resources.Load<ShopFactory>("mode/ShopMode");
        sf.init();
        itemf = Resources.Load<ItemFactory>("mode/ItemMode");
        itemf.init();
        equipmentf = Resources.Load<EquipmentFactory>("mode/EquipmentMode");
        equipmentf.init();

        scrollbar = UIFrameUtil.FindChildNode
            (this.transform, "Scrollbar Vertical").GetComponent<Scrollbar>();
        MessageMgr.AddMsgListener("jumpGem", p =>
        {
            scrollbar.value = 0.2f;
        });
        MessageMgr.AddMsgListener("jumpGold", p =>
        {
            scrollbar.value = 0;
        });


        MessageMgr.AddMsgListener("Guide_Button_OpenShop", p =>
        {
            scrollbar.value = 1f;
        });
        MessageMgr.AddMsgListener("Guide_OpenBox", p =>
        {
            scrollbar.value = 0.45f;
        });



        //购买章节礼包
        MessageMgr.AddMsgListener("buyChapterPack", p =>
        {
            OpenForm("ShopConfirmForm");
        });

        //购买每日商店资源
        MessageMgr.AddMsgListener("buyDailyShop", p =>
        {
            OpenForm("ShopConfirmForm");
        });

        //更新箱子钥匙显示
        MessageMgr.AddMsgListener("RefreshBoxKey", p =>
        {
            RefreshSupplyBoxPanelAsync();
        });

        MessageMgr.AddMsgListener("RefreshDailyShop", p =>
        {
            RefreshDailyShop();
        });


        //单抽
        GetBut(this.transform, "Eternal Supply Crate/Button (1)").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc003", 1));
        });
        //十连
        GetBut(this.transform, "Eternal Supply Crate/Button (10)").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc003", 10));
        });
        //普通箱子
        GetBut(this.transform, "Supply box/panel/box1/AdBut").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc001", 1));
        });
        GetBut(this.transform, "Supply box/panel/box1/BuyBut").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc001", 1));
        });
        //高级箱子
        GetBut(this.transform, "Supply box/panel/box2/AdBut").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc002", 1));
        });
        GetBut(this.transform, "Supply box/panel/box2/BuyBut").onClick.AddListener(() => {
            OpenForm("ShopConfirmForm");
            MessageMgr.SendMsg("openBox", new MsgKV("sc002", 1));
        });


        //观看广告开箱
        MessageMgr.AddMsgListener("AdSupplyBox", p =>
        {
            //宝箱类型
            string type = (string)p.Value;
            //校验是否在广告冷却倒计时期间
        });
        //购买开箱
        MessageMgr.AddMsgListener("BuySupplyBox", p =>
        {
            //宝箱类型
            string type = (string)p.Value;
            //校验是否有充足开箱资源
        });

        //购买钻石
        MessageMgr.AddMsgListener("buyDiamond", p =>
        {
            OpenForm("ShopConfirmForm");
        });
        //购买金币
        MessageMgr.AddMsgListener("buyGold", p =>
        {
            OpenForm("ShopConfirmForm");
        });


        //------------------ui初始化

        ChapterPackContent = UIFrameUtil.FindChildNode(this.transform, "ChapterPackContent");
        ChapterPackNode = ChapterPackContent.GetChild(0).gameObject;
        chapterPackList = new List<ChapterPackPanel>();
        for (int i = 0; i < ChapterPackContent.childCount; i++)
        {
            ChapterPackPanel ChapterPackPanel = ChapterPackContent.GetChild(i).GetComponent<ChapterPackPanel>();
            ChapterPackPanel.mgr = this;
            chapterPackList.Add(ChapterPackPanel);
        }


        Transform DailyShopList = UIFrameUtil.FindChildNode(this.transform, "DailyShopList");
        dailyShopSlotList = new List<DailyShopSlot>();
        for (int i = 0; i < DailyShopList.childCount; i++)
        {
            DailyShopSlot DailyShopSlot = DailyShopList.GetChild(i).GetComponent<DailyShopSlot>();
            DailyShopSlot.mgr = this;
            dailyShopSlotList.Add(DailyShopSlot);
        }


        Transform DiamondList = UIFrameUtil.FindChildNode(this.transform, "Diamond/list");
        diamondSlotList = new List<DiamondSlot>();
        for (int i = 0; i < DiamondList.childCount; i++)
        {
            DiamondSlot DiamondSlot = DiamondList.GetChild(i).GetComponent<DiamondSlot>();
            DiamondSlot.mgr = this;
            diamondSlotList.Add(DiamondSlot);
        }

        Transform GoldList = UIFrameUtil.FindChildNode(this.transform, "Gold/list");
        goldSlotList = new List<GoldSlot>();
        for (int i = 0; i < GoldList.childCount; i++)
        {
            GoldSlot GoldSlot = GoldList.GetChild(i).GetComponent<GoldSlot>();
            GoldSlot.mgr = this;
            goldSlotList.Add(GoldSlot);
        }

    }

    public override void Show()
    {
        base.Show();
        scrollbar.value = 1;
        Refresh();
    }

    void Refresh() {
        RefreshChapterPack();
        RefreshDailyShop();
        RefreshEternalSupplyCratePanel();
        RefreshSupplyBoxPanelAsync();
        RefreshDiamondPanel();
        RefreshGoldPanel();
    }


    //---------------------Chapter pack
    List<ChapterPackPanel> chapterPackList;
    Transform ChapterPackContent;
    GameObject ChapterPackNode;

    //更新章节礼包状态
    void RefreshChapterPack() {
        //遍历已解锁的章节礼包 填充   ui模块不足则生成
        for (int i = 0; i < sf.ChapterPackList.Count; i++)
        {
            //生成新的ui
            if (i >= chapterPackList.Count) {
                GameObject g = Instantiate(ChapterPackNode, ChapterPackContent);
                ChapterPackPanel ChapterPackPanel = g.GetComponent<ChapterPackPanel>();
                ChapterPackPanel.mgr = this;
                chapterPackList.Add(ChapterPackPanel);
            }
            chapterPackList[i].Refresh(sf.ChapterPackList[i]);
        }
    }



    //---------------------Daily shop
    List<DailyShopSlot> dailyShopSlotList;
    //购买免费资源后需等待的时间
    int dailyShop_BuyFreeTime;
    //可购买几次
    int dailyShop_BuyFreeNum;
    //上次宝石购买的时间  超过5分钟才能再次购买
    int dailyShop_BuyFreeTimeNow;
    //今日购买了几次
    int dailyShop_BuyFreeNumNow;


    async void RefreshDailyShop()
    {
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/mall/dailyShop", DataManager.Get().getHeader());
        NetDailyShopData dailyShopdata = JsonUtil.ReadData<NetDailyShopData>(str);
        Debug.Log("dailyShop:" + str);
         
        //更新ui槽位
        for (int i = 0; i < dailyShopdata.dailyInfoList.Count; i++)
        {
            dailyShopSlotList[i].Refresh(dailyShopdata.dailyInfoList[i]);
        }

        //List<DailyShopData> dataList = getDailyShopDataList();
        // DailyShopConfig freeConfig = null;

        /*  foreach (DailyShopConfig dsc in sf.DailyShopConfigList) {
              if (dsc.currency == "free") {
                  freeConfig = dsc;
              }
          }
          DailyShopData freeData = new DailyShopData();
          freeData.itemId = freeConfig.itemId;
          freeData.currency = "free";
          freeData.num = freeConfig.num;
          freeData.quality = 3;
          dailyShop_BuyFreeTime = freeConfig.cd;
          dailyShop_BuyFreeNum = freeConfig.buyCount;*/

        //FF38FF 紫色代码

        //第一栏一定是宝石  且宝石分不同状态
        //dailyShopSlotList[0].Refresh(freeData);


        /*     //更新ui槽位
             for (int i = 0; i < dailyShopSlotList.Count; i++)
             {
                 dailyShopSlotList[i].Refresh(dataList[i-1]);
             }*/
    }

    //本地计算权重给出每日商品
    /*List<DailyShopData> getDailyShopDataList() {
        //不能每次都变化  在一定时间内只刷新一次
        List<DailyShopConfig> DailyShopConfigList = sf.DailyShopConfigList;

        //用于权重计算
        Dictionary<int, DailyShopConfig> DailyShopConfigMap = new Dictionary<int, DailyShopConfig>();

        //用于存放被选中的配置
        List<DailyShopConfig> selectDSList = new List<DailyShopConfig>();
        //用于存放计算各项参数之后的实际数据
        List<DailyShopData> dataList = new List<DailyShopData>();

        //总权重计数
        int weight = 0;
        foreach (DailyShopConfig dsc in sf.DailyShopConfigList)
        {
            if (dsc.weight != 0) { 
                //计算权重
                weight += dsc.weight;
                DailyShopConfigMap.Add(weight, dsc);
            }
        }

        //根据权重选出物品
        for (int i = 0; i < 5; i++)
        {
            int num = Random.Range(0, weight);
            DailyShopConfig nowC = null;
            foreach (var item in DailyShopConfigMap)
            {
                //计算权重
                if (item.Key > num)
                {
                    nowC = item.Value;
                    break;
                }
            }
            selectDSList.Add(nowC);
        }

        //后面是铭牌或者装备
        for (int i = 0; i < 5; i++)
        {
            DailyShopData data = new DailyShopData();
            data.itemId = selectDSList[i].itemId;
            data.currency = selectDSList[i].currency;

            //进行品质  数量  价格  折扣的权重计算

            if (selectDSList[i].currency == "gem")
            {
                //品质权重
                data.quality = getValueForWeight(selectDSList[i].qualityWeight);
                //原价  --品质决定
                foreach (weight w in selectDSList[i].priceWeight)
                {
                    if (w.key == data.quality)
                    {
                        data.price = w.value;
                        break;
                    }
                }
            }
            else
            {
                data.quality = 0;

                //数量权重
                data.num = getValueForWeight(selectDSList[i].numWeight);

                //原价 --数量决定
                foreach (weight w in selectDSList[i].priceWeight)
                {
                    if (w.key == data.num)
                    {
                        data.price = w.value;
                        break;
                    }
                }
            }
            //折扣权重...
            data.preferential = getValueForWeight(selectDSList[i].preferentialWeight);
            dataList.Add(data);
        }

        return dataList;
    }*/

    int getValueForWeight(List<weight> weightList) {
        Dictionary<int, int> weightDic = new Dictionary<int, int>();
        int maxWeightNum = 0;
        foreach (weight w in weightList) {
            maxWeightNum += w.value;
            weightDic.Add(maxWeightNum, w.key);
        }

        int num = Random.Range(0, maxWeightNum);
        int value = -1;
        foreach (var item in weightDic)
        {
            //计算权重
            if (item.Key > num)
            {
                value = item.Value;
                break;
            }
        }
        return value;
    }





    //---------------------Eternal Supply Crate

    void RefreshEternalSupplyCratePanel() { 



    }


    public void openBox() { 
        
    }



    //---------------------Supply box
    GameObject supplyBox1;
    GameObject supplyBox2;
    TextMeshProUGUI boxKeyNum_1;
    TextMeshProUGUI boxKeyNum_2;
    Image boxKeyImg_1;
    Image boxKeyImg_2;

    async Task RefreshSupplyBoxPanelAsync()
    {
        //广告倒计时等
        //钥匙数量
        await DataManager.Get().refreshBackPack();

        int ys_ys = 0;
        int js_ys = 0;
        EquipmentData ys_ysD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10002");
        if (ys_ysD != null)
            ys_ys = ys_ysD.quantity;
        EquipmentData js_ysD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10003");
        if (js_ysD != null)
            js_ys = js_ysD.quantity;

        if (boxKeyNum_1 == null)
            boxKeyNum_1 = UIFrameUtil.FindChildNode(this.transform, "boxKeyNum_1").GetComponent<TextMeshProUGUI>();

        if (boxKeyNum_2 == null)
            boxKeyNum_2 = UIFrameUtil.FindChildNode(this.transform, "boxKeyNum_2").GetComponent<TextMeshProUGUI>();

        if (boxKeyImg_1 == null)
            boxKeyImg_1 = UIFrameUtil.FindChildNode(this.transform, "boxKeyImg_1").GetComponent<Image>();

        if (boxKeyImg_2 == null)
            boxKeyImg_2 = UIFrameUtil.FindChildNode(this.transform, "boxKeyImg_2").GetComponent<Image>();


        SupplyCrateConfig scc1 = PerimeterFactory.Get().SupplyCrateList.Find(x => x.id == "sc001");
        SupplyCrateConfig scc2 = PerimeterFactory.Get().SupplyCrateList.Find(x => x.id == "sc002");


        if (ys_ys == 0 && scc1.price > 0)
        {
            boxKeyImg_1.sprite = Resources.Load<Sprite>("ui/icon/钻石");
            boxKeyNum_1.text = ""+ scc1.price;
        }
        else {
            boxKeyImg_1.sprite = Resources.Load<Sprite>("ui/icon/银钥匙");
            boxKeyNum_1.text = "1/" + ys_ys;
        }


        if (js_ys == 0 && scc2.price>0)
        {
            boxKeyImg_2.sprite = Resources.Load<Sprite>("ui/icon/钻石");
            boxKeyNum_2.text = "" + scc2.price;
        }
        else
        {
            boxKeyImg_2.sprite = Resources.Load<Sprite>("ui/icon/金钥匙");
            boxKeyNum_2.text = "1/" + js_ys;
        }


    }



    //---------------------Diamond
    List<DiamondSlot> diamondSlotList;

    void RefreshDiamondPanel() {

        int index = 0;
        foreach (DiamondAndGoldConfig dg in sf.DiamondAndGoldConfigList) {
            if (dg.currency == "$") {
                diamondSlotList[index++].Refresh(dg);
            }
        }

       /* for (int i = 0; i < diamondSlotList.Count; i++)
        {
            diamondSlotList[i].Refresh();
        }*/

    }


    //---------------------Gold
    List<GoldSlot> goldSlotList;
    void RefreshGoldPanel()
    {
        int index = 0;
        foreach (DiamondAndGoldConfig dg in sf.DiamondAndGoldConfigList)
        {
            if (dg.currency == "gem")
            {
                goldSlotList[index++].Refresh(dg);
            }
        }
    }
}


public class NetShopPostData
{
    public string type;
    public string id;
    public string shopDailyListId;
    public bool isTen;
    public string orderType = "GOOGLE_PLAY";

    public  NetShopPostData() {
#if UNITY_IOS
        orderType = "APPLE_PAY";
#else
        orderType = "GOOGLE_PLAY";
#endif
    }
}
public class NetShopPostDataReturn
{
    public string id;           //":"64afc64995073e2ed148de73",
    public string thirdOid;     //":null,
    public string userId;       // ":307,
    public string nickName;     //   ":"123456",
    public string platform;     //   ":"ANDROID",
    public string currencyType; //"  :"GOLD",
    public string orderType;    //  ":"SYSTEM",
    public string orderStats;   // ":"NOT_PAY",
    public string productId;    //  ":null,
  //public string productData
}

public class NetShopPayPostData
{
    public string type;
    public string orderId;
    public string packageName = Application.identifier;
    public string productId;
    public string productToken;
}





/*  dailyShop:{"errorCode":null,"message":null,
      "data":{"id":"54be3309-6c4c-43d9-8899-d5bedd907802","infoDate":"2023-07-13",
      "dailyInfoList":[
      {"dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802",
              "serialNumber":1,"id":"00001","itemId":"p10000","type":"free","currency":"free","num":80,"price":0,"discountRate":100.0,
              "discountPrice":0,"quality":0,"buyCount":3,"payedNum":0},
     { "dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802",
              "serialNumber":2,"id":"20005","itemId":"m10005","type":"dogtag","currency":"gold","num":0,"price":20000,"discountRate":50.0,
              "discountPrice":10000,"quality":0,"buyCount":999,"payedNum":0},
{ "dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802","serialNumber":3,"id":"20004","itemId":"m10004","type":"dogtag","currency":"gold","num":0,"price":20000,"discountRate":70.0,"discountPrice":14000,"quality":0,"buyCount":999,"payedNum":0},
{ "dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802","serialNumber":4,"id":"20002","itemId":"m10002","type":"dogtag","currency":"gold","num":0,"price":10000,"discountRate":60.0,"discountPrice":6000,"quality":0,"buyCount":999,"payedNum":0},{ "dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802","serialNumber":5,"id":"10001","itemId":"wp_001","type":"equipment","currency":"gem","num":1,"price":500,"discountRate":90.0,"discountPrice":450,"quality":2,"buyCount":999,"payedNum":0},{ "dailyId":"54be3309-6c4c-43d9-8899-d5bedd907802","serialNumber":6,"id":"10002","itemId":"ri_001","type":"equipment","currency":"gem","num":1,"price":500,"discountRate":50.0,"discountPrice":250,"quality":2,"buyCount":999,"payedNum":0}]}} 
 */