using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.UI;

public class DungeonForm : BaseUIForm
{
    //Dictionary<string, BaseUIPanel> childPanelMap = new Dictionary<string, BaseUIPanel>();
    //Dictionary<string, BaseUIPanel> EsayAndHardPanelMap = new Dictionary<string, BaseUIPanel>();
    //Transform DungeonButton_1;
    //Transform DungeonButton_2;

    Transform selectStoreyTra;
    Transform lockTra;

    TextMeshProUGUI StoreyDescText;
    Image towerImg;
    TextMeshProUGUI selectStoreyDescText;

    //防止重复点击
    bool towerButFlag;

    public override void Awake()
    {
        base.Awake();
        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.HideOther;
        ui_type.IsClearStack = false;

        //nameText = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();

        //DungeonButton_1 = UIFrameUtil.FindChildNode(this.transform, "MainPanel/DungeonButton/Button");
        //DungeonButton_2 = UIFrameUtil.FindChildNode(this.transform, "SelectPanel/DungeonButton");

        /* EsayAndHardPanelMap.Add("EsayPanel",
                UIFrameUtil.FindChildNode(this.transform, "EsayPanel").GetComponent<BaseUIPanel>());
         EsayAndHardPanelMap.Add("HardPanel",
             UIFrameUtil.FindChildNode(this.transform, "HardPanel").GetComponent<BaseUIPanel>());
         childPanelMap.Add("MainPanel",
             UIFrameUtil.FindChildNode(this.transform, "MainPanel").GetComponent<BaseUIPanel>());
         childPanelMap.Add("SelectPanel",
             UIFrameUtil.FindChildNode(this.transform, "SelectPanel").GetComponent<BaseUIPanel>());*/


        loadEnd = false;

        selectStoreyTra = UIFrameUtil.FindChildNode(this.transform, "selectStorey");
        StoreyDescText = UIFrameUtil.FindChildNode(this.transform, "StoreyDesc").GetComponent<TextMeshProUGUI>();
        towerImg = UIFrameUtil.FindChildNode(this.transform, "towerImg").GetComponent<Image>();
        selectStoreyDescText = UIFrameUtil.FindChildNode(this.transform, "selectStoreyDesc").GetComponent<TextMeshProUGUI>();
        lockTra = UIFrameUtil.FindChildNode(this.transform, "selectStoreyDesc/lock");

        GetBut(this.transform, "towerPanel/button")
           .onClick.AddListener(async () => {

               //先校验
               if (towerButFlag)
                   return;

               towerButFlag = true;
               string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/battleFlow/flowIn", DataManager.Get().getHeader());
               Debug.Log(str);

               JObject obj = (JObject)JsonConvert.DeserializeObject(str);
               NetData NetData = obj.ToObject<NetData>();

               if ((bool)NetData.data)
               {
                   //todo 爬塔
                   towerButFlag = false;
                   SceneManager.LoadScene("tower");
               }
               else { 
                   //显示资源不够
                   UIManager.GetUIMgr().showUIForm("ErrForm");
                   MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Insufficient physical strength"));
                   towerButFlag = false;
               }
           });

        GetBut(this.transform, "towerPanel/button/StoreyDesc").onClick.AddListener(() => {

            //selectStoreyTra.GetComponent<CanvasGroup>().DOFade(1, 0.25f);
            //StopCoroutine("InActive");
            selectStoreyTra.gameObject.SetActive(true);
        });
        GetBut(this.transform, "selectStorey/Panel").onClick.AddListener(() => {
            RefreshAsync();
            selectStoreyTra.gameObject.SetActive(false);

            //selectStoreyTra.GetComponent<CanvasGroup>().DOFade(0, 0.1f);
            //StartCoroutine(InActive(selectStoreyTra.gameObject,0.1f));
        });

        GetBut(this.transform, "UpSelectBut").onClick.AddListener(() => {
            selectStorey(1);
        });
        GetBut(this.transform, "DownSelectBut").onClick.AddListener(() => {
            selectStorey(-1);
        });

        GetBut(this.transform, "Button_GrowthFund").onClick.AddListener(() => {
            OpenForm("GrowthFundForm");
            CloseForm("down_menu");
        });
        GetBut(this.transform, "Button_GiftBag").onClick.AddListener(() => {
            OpenForm("GiftBagForm");
            CloseForm("down_menu");
        });
        GetBut(this.transform, "Button_MonthlyCard").onClick.AddListener(() => {
            OpenForm("MonthlyCardForm");
            CloseForm("down_menu");
        });
        GetBut(this.transform, "Button_PassCheck").onClick.AddListener(() => {
            OpenForm("PassCheckForm");
            CloseForm("down_menu");
        });
        GetBut(this.transform, "Button_Achievement").onClick.AddListener(() => {
            OpenForm("AchievementForm");
        });
        GetBut(this.transform, "Button_Mission").onClick.AddListener(() => {
            OpenForm("MissionForm");
        });
        GetBut(this.transform, "Button_Pack").onClick.AddListener(() => {
            OpenForm("MyPackForm");
        });
        GetBut(this.transform, "Button_SevenDaySign").onClick.AddListener(() => {
            OpenForm("SevenDaySignForm");
        });
        GetBut(this.transform, "Button_Sign").onClick.AddListener(() => {
            OpenForm("SignForm");
        });
        GetBut(this.transform, "Button_FirstCharge").onClick.AddListener(() => {
            OpenForm("FirstChargeForm");
        });
        GetBut(this.transform, "Button_Mail").onClick.AddListener(() => {
            OpenForm("MailForm");
        });
        GetBut(this.transform, "Button_Notice").onClick.AddListener(() => {
            OpenForm("NoticeForm");
        });
        GetBut(this.transform, "fire").onClick.AddListener(() => {
            OpenForm("PatrolForm");
        });


        MessageMgr.AddMsgListener("PlayerLevelUp", p =>{
            (int, int) a = ((int, int))p.Value;
            showLockFrom(a.Item1,a.Item2);
        });
        MessageMgr.AddMsgListener("UnLockFormPopEnd", p =>
        {
            popFlag = false;
        });

        MessageMgr.AddMsgListener("RefreshTip", p =>
        {
            showTipAsync();
        });

       
        

        //选择关卡
        /*GetBut(this.transform, "SelectPanel/DungeonButton/Button").onClick.AddListener(() => {
            confirmSelect();
        });

        GetBut(this.transform, "SelectPanel/UpButton").onClick.AddListener(() => {
            selectDungeon(-1);
        });
        GetBut(this.transform, "SelectPanel/DownButton").onClick.AddListener(() => {
            selectDungeon(1);
        });

        MainPanelDungeonName = UIFrameUtil.FindChildNode
            (this.transform,"MainPanel/Name").GetComponent<TextMeshProUGUI>();
        SelectPanelDungeonName = UIFrameUtil.FindChildNode
            (this.transform, "SelectPanel/Name").GetComponent<TextMeshProUGUI>();*/


        //df = Resources.Load<DungeonFactory>("mode/dungeonMode");
        //df.init();

        //DungeonIndex = 0;
        //DungeonDesc desc = df.descList[DungeonIndex];
        //MainPanelDungeonName.text = desc.name;

    }

    private void Start()
    {
        //todo 爬塔改动
        //childPanelMap["MainPanel"].Show();
        //EsayAndHardPanelMap["EsayPanel"].Show();
        //DungeonButton_1.DOScale(0.97f, 1).SetLoops(-1, LoopType.Yoyo);
        //DungeonButton_2.DOScale(0.97f, 1).SetLoops(-1, LoopType.Yoyo);
        OpenForm("LoadForm");
        StoreyDescText.transform.DOScale(1.2f, 1.25f).SetLoops(-1, LoopType.Yoyo);
    }

    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }
   

    async Task init()
    {
        await DataManager.Get().refreshRoleAttributeStr();
        if (DataManager.Get().roleAttrData.successChapter.Count > 0) { 
            DataManager.Get().now_chapterIndex = DataManager.Get().roleAttrData.successChapter.Count;
            selectTowerIndex = DataManager.Get().now_chapterIndex;
        }
        return;
    }

    bool startFlag;

    async Task RefreshAsync() {
        loadEnd = false;
        GuideAing = false;

        RoleAttrData rd = await DataManager.Get().refreshRoleAttributeStr();
        //if(rd.nowLevel>)

        if (!startFlag) {
            startFlag = true;
            //await init();
            if (DataManager.Get().roleAttrData.successChapter.Count > 0)
            {
                DataManager.Get().now_chapterIndex = DataManager.Get().roleAttrData.successChapter.Count;
                selectTowerIndex = DataManager.Get().now_chapterIndex;
            }
        }

        if (DataManager.Get().roleAttrData.nowLevel == 1 && DataManager.Get().roleAttrData.nowLevelExp == 0)
        {
            OpenForm("GuideAFrom");
            DataManager.Get().GuideAFlag = true;
            GuideAing = true;
        }


        TowerMap chapter = TowerFactory.Get().tmList.Find(x => 
            x.id == TowerFactory.Get().chapterList[DataManager.Get().now_chapterIndex]);
        StoreyDescText.text = chapter.chapter + "\r\n <size=45>tower layers:" + chapter.name;
        towerImg.sprite = Resources.Load<Sprite>("image/tower/" + chapter.chapter);

        showTowerIndex = DataManager.Get().now_chapterIndex;
        selectStorey(0);

        UIFrameUtil.FindChildNode(this.transform, "fire").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_GrowthFund").gameObject.SetActive(false);
       
        UIFrameUtil.FindChildNode(this.transform, "Button_Mission").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_GiftBag").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_MonthlyCard").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_PassCheck").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_Sign").gameObject.SetActive(false);
        Button_FirstCharge_Unlock = false;

        //功能解锁相关
        for (int i = 0; i < PerimeterFactory.Get().LevelUnlockList.Count; i++)
        {
            if (DataManager.Get().now_level >= PerimeterFactory.Get().LevelUnlockList[i].level)
            {
                //弹出相关提示
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "Patrol")
                    UIFrameUtil.FindChildNode(this.transform, "fire").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "GrowthFund")
                    UIFrameUtil.FindChildNode(this.transform, "Button_GrowthFund").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "FirstCharge")
                    Button_FirstCharge_Unlock = true;
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "Mission")
                    UIFrameUtil.FindChildNode(this.transform, "Button_Mission").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "GiftBag")
                    UIFrameUtil.FindChildNode(this.transform, "Button_GiftBag").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "MonthlyCard")
                    UIFrameUtil.FindChildNode(this.transform, "Button_MonthlyCard").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "PassCheck")
                    UIFrameUtil.FindChildNode(this.transform, "Button_PassCheck").gameObject.SetActive(true);
                if (PerimeterFactory.Get().LevelUnlockList[i].id == "Sign")
                    UIFrameUtil.FindChildNode(this.transform, "Button_Sign").gameObject.SetActive(true);
            }
        }

        await showTipAsync(true);

        loadEnd = true;
        MessageMgr.SendMsg("HideLoadForm", null);
    }

    int showTowerIndex;
    int selectTowerIndex;
    void selectStorey(int i) {
        //不能超过
        showTowerIndex = Mathf.Clamp(showTowerIndex + i , 0, TowerFactory.Get().chapterList.Count-1) ;

        TowerMap chapter = TowerFactory.Get().tmList.Find(x =>
           x.id == TowerFactory.Get().chapterList[showTowerIndex]);

        if (DataManager.Get().roleAttrData.successChapter.Count < showTowerIndex)
        {
            lockTra.gameObject.SetActive(true);
        }
        else { 
            lockTra.gameObject.SetActive(false);
            selectTowerIndex = showTowerIndex;
            DataManager.Get().now_chapterIndex = selectTowerIndex;
            StoreyDescText.text = chapter.chapter + "\r\n <size=45>tower layers:" + chapter.name;
            towerImg.sprite = Resources.Load<Sprite>("image/tower/" + chapter.chapter);
        }
        selectStoreyDescText.text = chapter.chapter + "\r\n <size=45>tower layers:" + chapter.name;
    }


    bool popFlag;
    bool loadEnd;
    bool GuideAing;
    //解锁相关   可能一级解锁多个
    void showLockFrom(int level,int oldLevel) {
        List<string> strlist = new List<string>(); 
        for (int i=0;i < PerimeterFactory.Get().LevelUnlockList.Count;i++) {
            if (oldLevel < PerimeterFactory.Get().LevelUnlockList[i].level && 
                level >= PerimeterFactory.Get().LevelUnlockList[i].level) {
                //弹出相关提示
                strlist.Add(PerimeterFactory.Get().LevelUnlockList[i].id);
            } 
        }
        if (strlist.Count > 0) {
            popFlag = true;
            OpenForm("UnLockForm");
            MessageMgr.SendMsg("LevelUnLcokShow", new MsgKV(null,strlist));
        }
    }

    async void Update()
    {
        if (!loadEnd)
            return;

        if (loadEnd && !popFlag && !GuideAing && 
            DataManager.Get().GuideAFlag && 
            !DataManager.Get().GuideBFlag)
        {
            DataManager.Get().GuideBFlag = true;
            int gem = 0;
            EquipmentData goldD = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10000");
            if (goldD != null)
                gem = goldD.quantity;
            string str = await NetManager.get(ConfigCheck.publicUrl + "/data/pub/mall/dailyShop", DataManager.Get().getHeader());
            NetDailyShopData dailyShopdata = JsonUtil.ReadData<NetDailyShopData>(str);
            if (dailyShopdata.dailyInfoList[0].payedNum < dailyShopdata.dailyInfoList[0].buyCount || gem >= 80)
            {
                OpenForm("GuideBFrom");
            }
        }

        //OpenForm("GuideBFrom");

        if (UIManager.GetUIMgr().checkUIForm("GuideBFrom")) {
            return;
        }

        if (pop_Sign && !pop_Sign_flag)
        {
            pop_Sign_flag = true;
            pop_Sign = false;
            OpenForm("SignForm");
            return;
        }

    }

    bool pop_Sign;
    bool pop_Sign_flag; //只弹一次 弹过就不在弹了
    bool pop_SevenDaySign;
    bool pop_MonthlyCard;
    bool Button_FirstCharge_Unlock;

    public async Task showTipAsync(bool init = false) {
        //提示新的可交互模块
        //Debug.Log(" selectTowerIndex:" + DataManager.Get().now_chapterIndex);
        string unclaimedScenesStr = await NetManager.get(ConfigCheck.publicUrl + "/data/pub/base/unclaimedScenes", DataManager.Get().getHeader());
        Debug.Log("----------------unclaimedScenesStr:" + unclaimedScenesStr);

        UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign").gameObject.SetActive(true);
        UIFrameUtil.FindChildNode(this.transform, "Button_FirstCharge").gameObject.SetActive(false);

        UIFrameUtil.FindChildNode(this.transform, "Button_Mail/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "fire/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_Mission/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_MonthlyCard/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_GrowthFund/tip").gameObject.SetActive(false);


        UIFrameUtil.FindChildNode(this.transform, "Button_GiftBag/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_PassCheck/tip").gameObject.SetActive(false);
        UIFrameUtil.FindChildNode(this.transform, "Button_Sign/tip").gameObject.SetActive(false);

        if (unclaimedScenesStr != null)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(unclaimedScenesStr);
            NetData NetData = obj.ToObject<NetData>();
            JArray obj1 = (JArray)JsonConvert.DeserializeObject(NetData.data.ToString());
            List<string> infoList = obj1.ToObject<List<string>>();
            if (infoList != null && infoList.Count > 0)
            {
                if (infoList.Contains("MAILBOX"))
                    UIFrameUtil.FindChildNode(this.transform, "Button_Mail/tip").gameObject.SetActive(true);

                if (infoList.Contains("HANG_UP"))
                    UIFrameUtil.FindChildNode(this.transform, "fire/tip").gameObject.SetActive(true);

                if (infoList.Contains("TASK"))
                    UIFrameUtil.FindChildNode(this.transform, "Button_Mission/tip").gameObject.SetActive(true);

                if (infoList.Contains("MONTH_CARD")) { 
                    UIFrameUtil.FindChildNode(this.transform, "Button_MonthlyCard/tip").gameObject.SetActive(true);
                    //需要主动弹窗
                    if (UIFrameUtil.FindChildNode(this.transform, "Button_MonthlyCard").gameObject.activeInHierarchy)
                        if (init) {
                            pop_MonthlyCard = true;
                        }
                }

                if (infoList.Contains("GRPWTH_FUNDS"))
                    UIFrameUtil.FindChildNode(this.transform, "Button_GrowthFund/tip").gameObject.SetActive(true);

                if (infoList.Contains("NEW_SIGN_TRUE"))
                {
                    UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign/tip").gameObject.SetActive(true);
                    //需要主动弹窗
                    if (UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign").gameObject.activeInHierarchy)
                        if (init)
                        {
                            pop_SevenDaySign = true;
                        }
                }
                else if (infoList.Contains("NEW_SIGN_FALSE")) { 
                    UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign/tip").gameObject.SetActive(false);
                }
                //校验功能是否解锁
                if (UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign").gameObject.activeInHierarchy) { 
                    if (!infoList.Contains("NEW_SIGN_FALSE") && !infoList.Contains("NEW_SIGN_TRUE"))
                        UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign").gameObject.SetActive(false);
                    else
                        UIFrameUtil.FindChildNode(this.transform, "Button_SevenDaySign").gameObject.SetActive(true);
                }

                if (infoList.Contains("FIRST_CHARGE_PACK"))
                {
                    DataManager.Get().FIRST_CHARGE_PACK = true;
                }
                else {
                    DataManager.Get().FIRST_CHARGE_PACK = false;
                }

                //校验功能是否解锁
                if (Button_FirstCharge_Unlock) 
                { 
                    if (DataManager.Get().FIRST_CHARGE_PACK)
                    {
                        UIFrameUtil.FindChildNode(this.transform, "Button_FirstCharge").gameObject.SetActive(true);
                    }
                    else {
                        UIFrameUtil.FindChildNode(this.transform, "Button_FirstCharge").gameObject.SetActive(false);
                    }
                }


            
              


                if (infoList.Contains("DAILY_SIGN")) { 
                    UIFrameUtil.FindChildNode(this.transform, "Button_Sign/tip").gameObject.SetActive(true);
                    //需要主动弹窗
                    if (UIFrameUtil.FindChildNode(this.transform, "Button_Sign").gameObject.activeInHierarchy)
                        if (init)
                        {
                            pop_Sign = true;
                        }
                }

                if (infoList.Contains("BUNDLE_SHOP"))
                    UIFrameUtil.FindChildNode(this.transform, "Button_GiftBag/tip").gameObject.SetActive(true);
                if (infoList.Contains("PASS"))
                    UIFrameUtil.FindChildNode(this.transform, "Button_PassCheck/tip").gameObject.SetActive(true);
            }
        }
        return;
    }

   /* IEnumerator InActive(GameObject g,float time)
    {
        yield return new WaitForSeconds(time);
        g.SetActive(false);
    }*/
}

