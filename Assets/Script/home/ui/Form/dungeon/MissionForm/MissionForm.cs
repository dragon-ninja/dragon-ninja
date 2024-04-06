using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

public class MissionForm : BaseUIForm
{

    Transform MissionPanel;
    Transform AchievementPanel;
    GameObject MissionSlotPf;
    GameObject AchievementSlotPf;
    Transform MissionTra;
    Transform AchievementTra;

    RectTransform boxListTra_1;
    RectTransform boxListTra_2;

    List<MissionSlot> MissionSlotList = new List<MissionSlot>();
    List<MissionSlot> AchievementSlotList = new List<MissionSlot>();
    TextMeshProUGUI MissionName;

    TextMeshProUGUI activityNumDesc;

    List<Button> boxButs = new List<Button>();

    string nowType;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;


        GetBut(this.transform, "Mission/close").onClick.AddListener(() => {
            CloseForm();
        });
        GetBut(this.transform, "Achievement/close").onClick.AddListener(() => {
            CloseForm();
        });

        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm();
        });

        MissionPanel = UIFrameUtil.FindChildNode(this.transform, "Mission");
        AchievementPanel = UIFrameUtil.FindChildNode(this.transform, "Achievement");


        boxListTra_1 = UIFrameUtil.FindChildNode(this.transform, "boxList (1)").GetComponent<RectTransform>();
        boxListTra_2 = UIFrameUtil.FindChildNode(this.transform, "boxList (2)").GetComponent<RectTransform>();

        MissionName = UIFrameUtil.FindChildNode(this.transform,
          "Mission/name/Text (TMP)").GetComponent<TextMeshProUGUI>();
        activityNumDesc = UIFrameUtil.FindChildNode(this.transform,
            "activityNumDesc").GetComponent<TextMeshProUGUI>();


        //UIFrameUtil.FindChildNode(this.transform, "boxList (2)").GetComponent<RectTransform>();
        //boxList(2)

        int index = 0;

        for (int i = 0; i < boxListTra_2.childCount;i++) 
            boxButs.Add(boxListTra_2.GetChild(i).GetComponent<Button>());

        boxListTra_2.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
            receiveBoxAsync(nowType == "DAILY_TASK" ? "box_101" : "box_201");
        });
        boxListTra_2.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
            receiveBoxAsync(nowType == "DAILY_TASK" ? "box_102" : "box_202");
        });
        boxListTra_2.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {
            receiveBoxAsync(nowType == "DAILY_TASK" ? "box_103" : "box_203");
        });
        boxListTra_2.GetChild(3).GetComponent<Button>().onClick.AddListener(() => {
            receiveBoxAsync(nowType == "DAILY_TASK" ? "box_104" : "box_204");
        });
        boxListTra_2.GetChild(4).GetComponent<Button>().onClick.AddListener(() => {
            receiveBoxAsync(nowType == "DAILY_TASK" ? "box_105" : "box_205");
        });



        GetBut(this.transform, "buttonList/daily").onClick.AddListener(() => {
            MissionName.text = "Daily";
            MissionPanel.gameObject.SetActive(true);
            AchievementPanel.gameObject.SetActive(false);
            RefreshAsync("DAILY_TASK");
        });
        GetBut(this.transform, "buttonList/weekly").onClick.AddListener(() => {
            MissionName.text = "Weekly";
            MissionPanel.gameObject.SetActive(true);
            AchievementPanel.gameObject.SetActive(false);
            RefreshAsync("WEEKLY_TASK");
        });
        GetBut(this.transform, "buttonList/achievement").onClick.AddListener(() => {
            MissionPanel.gameObject.SetActive(false);
            AchievementPanel.gameObject.SetActive(true);
            RefreshAsync("ACHIEVEMENT_TASK");
        });

      

        MissionTra = UIFrameUtil.FindChildNode(this.transform, "Mission/Scroll View/Viewport/Content");
        MissionSlotPf = MissionTra.Find("item").gameObject;
        MissionSlot slot = MissionTra.Find("item").GetComponent<MissionSlot>();
        slot.mgr = this;
        MissionSlotList.Add(slot);
       /* for (int i = 0; i < MissionTra.childCount; i++)
        {
        }*/
        Debug.Log("___________"+ (MissionSlotPf == null)+"  "+ MissionSlotList.Count);

        AchievementTra = UIFrameUtil.FindChildNode(this.transform, "Achievement/Scroll View/Viewport/Content");
        AchievementSlotPf = AchievementTra.Find("item").gameObject;
        MissionSlot slot2 = AchievementTra.Find("item").GetComponent<MissionSlot>();
        slot.mgr = this;
        AchievementSlotList.Add(slot2);
        /*for (int i = 0; i < AchievementTra.childCount; i++)
        {
        }*/
    }

    public override void Show()
    {
        base.Show();
        RefreshAsync("DAILY_TASK");
    }

    public async Task RefreshAsync(string type) {

        nowType = type;

        List<NetTaskInfo> taskInfos = null;
        List<NetActivityInfo> activityInfos = null;
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/task/list", DataManager.Get().getHeader());
        Debug.Log(str);

      /*  string saveUrl = Application.persistentDataPath +
       "/userData" + "/testData.json";
        File.WriteAllText(saveUrl, str);*/

        if (str != null)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }
            else
            {
                Debug.Log("NetData.errorCode != null:" + (NetData.errorCode != null));
                JObject obj2 = (JObject)JsonConvert.DeserializeObject(NetData.data.ToString());
                NetTaskData itemList = obj2.ToObject<NetTaskData>();


                Debug.Log(" itemList.activityInfos:" + itemList.activity);
                activityInfos = itemList.activity;
                taskInfos = itemList.taskInfos.FindAll(x => x.taskType == type);
                taskInfos.Sort(Comp);
  
               /* taskInfos.Sort((a, b) =>
                       ((b.num + 0.0f) / (b.targetNum + 0.0f)).CompareTo((a.num + 0.0f) / (a.targetNum + 0.0f))
                );*/

                Debug.Log("----------");
                foreach (var v in taskInfos) {
                    
                    Debug.Log(v.taskId+ "  num:"+ v.num + "  targetNum:"+ v.targetNum + "  hasReceive:" + v.hasReceive);
                }
            }
        }
        else
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "network error"));
            return;
        }

        List<MissionConfig> list = null;
        list = PerimeterFactory.Get().MissionConfigList.FindAll(item => item.type == type);

        if (type == "ACHIEVEMENT_TASK")
        {
            MissionPanel.gameObject.SetActive(false);
            AchievementPanel.gameObject.SetActive(true);

            for (int i = 0; i < AchievementSlotList.Count; i++)
            {
                AchievementSlotList[i].Hide();
            }
            for (int i = 0; i < taskInfos.Count; i++)
            {
                //生成新的ui
                if (i >= AchievementSlotList.Count)
                {
                    GameObject g = Instantiate(AchievementSlotPf, AchievementTra);
                    MissionSlot slot = g.GetComponent<MissionSlot>();
                    slot.mgr = this;
                    AchievementSlotList.Add(slot);
                }
                MissionData data = new MissionData();
                data.id = list[i].id;
                AchievementSlotList[i].Refresh(taskInfos[i]);
            }

        }
        else {
            if (type == "DAILY_TASK")
                MissionName.text = "Daily";
            else 
                MissionName.text = "Weekly";
            MissionPanel.gameObject.SetActive(true);
            AchievementPanel.gameObject.SetActive(false);

            activityNumDesc.text = "0";
            //填充   ui模块不足则生成  多余则隐藏
            for (int i = 0; i < MissionSlotList.Count; i++)
            {
                MissionSlotList[i].Hide();
            }
            Debug.Log(taskInfos.Count);
            for (int i = 0; i < taskInfos.Count; i++)
            {
                //生成新的ui
                if (i >= MissionSlotList.Count)
                {
                    GameObject g = Instantiate(MissionSlotPf, MissionTra);
                    MissionSlot slot = g.GetComponent<MissionSlot>();
                    slot.mgr = this;
                    MissionSlotList.Add(slot);
                }
                MissionSlotList[i].Refresh(taskInfos[i]);
            }
        }

        //活跃宝箱
        if (type != "ACHIEVEMENT_TASK")
        {
            string hydid = "p10006";
            if (type == "WEEKLY_TASK")
                hydid = "p10007";

            int hyd = 0;

            await DataManager.Get().refreshBackPack();

            EquipmentData ed = DataManager.Get().backPackData.activity.Find(x => x.id == hydid);

            if (ed != null)
                hyd = ed.quantity;

            Debug.Log(
                (DataManager.Get().backPackData.activity.Find(x => x.id == "p10006") == null) + " " +
                (ed != null) + "hyd::::" + hyd);

            boxListTra_1.sizeDelta = new Vector2(160 * Mathf.Min((hyd / 20), 5), boxListTra_1.sizeDelta.y);


            for (int i = 0;i< boxButs.Count;i++)
            {
                if ((hyd / 20) > i ) {
                    boxButs[i].image.sprite = Resources.Load<Sprite>("ui/icon/活跃度宝箱发光");
                }
                else
                    boxButs[i].image.sprite = Resources.Load<Sprite>("ui/icon/活跃度宝箱");
            }

            NetActivityInfo info = null;
            if (activityInfos.Find(x=> x.id == hydid) != null) {
                info = activityInfos.Find(x => x.id == hydid);

                activityNumDesc.text = info.quantity + "";


                foreach (string s in info.boxIds) {
                    int index = int.Parse(s.Substring(s.Length - 1))-1;
                    boxButs[index].image.sprite = Resources.Load<Sprite>("ui/icon/活跃度宝箱打开");
                }
                //info.boxIds;

            }
         
        }
    }
    private int Comp(NetTaskInfo a, NetTaskInfo b) {

        MissionConfig config1 = PerimeterFactory.Get().MissionConfigList.Find(item => item.id == a.taskId);
        MissionConfig config2 = PerimeterFactory.Get().MissionConfigList.Find(item => item.id == b.taskId);

        if (b.hasReceive.CompareTo(a.hasReceive)!=0) {
            return -b.hasReceive.CompareTo(a.hasReceive);
        }
        else if ( ((b.num+0.0f) / (b.targetNum + 0.0f)).CompareTo((a.num + 0.0f) / (a.targetNum + 0.0f)) != 0 ) { 
            return ((b.num + 0.0f) / (b.targetNum + 0.0f)).CompareTo((a.num + 0.0f) / (a.targetNum + 0.0f));
        }
        else
        {
            return - b.taskId.CompareTo(a.taskId);
        }
    }


    //领取活跃度奖励
    public async Task receiveBoxAsync(string boxId) {

            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/task/receiveBox?boxId=" + boxId, DataManager.Get().getHeader());
            Debug.Log(str);
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }
            Debug.Log(NetData.data);

            JArray obj1 = (JArray)JsonConvert.DeserializeObject(NetData.data.ToString());
            List<ItemInfo2> infoList = obj1.ToObject<List<ItemInfo2>>();

            if (infoList != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<ItemInfo> items = new List<ItemInfo>();

                foreach (var info in infoList)
                    items.Add(new ItemInfo(info.itemId, info.num,info.quality,info.level));

                MessageMgr.SendMsg("GetReward", new MsgKV("", items));
            }
            RefreshAsync(nowType);
            MessageMgr.SendMsg("RefreshTip", null);
        }

}

public class NetTaskData
{
    public List<NetTaskInfo> taskInfos;
    public List<NetActivityInfo> activity;
}



public class NetTaskInfo
{
    public string taskId;
    public string taskType;
    public int num;
    public int  targetNum;
    public bool  hasReceive;
}

public class NetActivityInfo
{
    public string id;
    public string activityType;
    public int quantity;
    public List<string> boxIds;
}