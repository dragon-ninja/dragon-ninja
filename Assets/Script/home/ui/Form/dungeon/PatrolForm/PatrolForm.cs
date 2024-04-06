using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PatrolForm : BaseUIForm
{
    GameObject QuickPatrolPanel;
    //GameObject RewardPanel;
    TextMeshProUGUI goldText;
    TextMeshProUGUI expText;
    TextMeshProUGUI timeText;

    GameObject slot_pf;

    List<ItemSlot> ItemSlotList;
    List<ItemSlot> quick_ItemSlotList;
    //List<ItemSlot> reward_ItemSlotList;

    Transform slotTra_1;
    Transform slotTra_2;

    Button GetBut;
    Button buyBut;
    TextMeshProUGUI buyButText;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        QuickPatrolPanel = transform.Find("QuickPatrolPanel").gameObject;

        GetBut(this.transform, "PatrolPanel/close").onClick.AddListener(() => {
            CloseForm();
        });

        GetBut(this.transform, "QuickPatrolPanel/Panel/close").onClick.AddListener(() => {
            QuickPatrolPanel.SetActive(false);
        });

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });

        GetBut(this.transform, "QuickBut").onClick.AddListener(()=> {
            //打开快速巡逻面板
            openQuickPatrolPanelAsync();
        });
        
        GetBut(this.transform, "QuickPatrolPanel").onClick.AddListener(() => {
            //关闭快速巡逻面板
            QuickPatrolPanel.SetActive(false);
        });

        GetBut = GetBut(this.transform, "GetBut");
        GetBut(this.transform, "freeBut").onClick.AddListener(async () => {
            //看广告-------todo
            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/patrol/receiveFastPatrol", DataManager.Get().getHeader());
            Debug.Log(str);
            PatrolNetData data = JsonUtil.ReadData<PatrolNetData>(str);
            if (data != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");

                List<ItemInfo> infos = new List<ItemInfo>();

                infos.Add(new ItemInfo("p10001", data.gold));
                infos.Add(new ItemInfo("p10008", data.exp));

                foreach (var item in data.material)
                {
                    infos.Add(item);
                }
                foreach (var item in data.specialMaterial)
                {
                    infos.Add(item);
                }
                MessageMgr.SendMsg("GetReward", new MsgKV("", infos));
                openQuickPatrolPanelAsync();
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("audio/金币获取音效"), Vector3.zero, 1.0f);
                MessageMgr.SendMsg("RefreshTip", null);
            }
        });
        buyBut = GetBut(this.transform, "buyBut");
        buyButText = buyBut.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        buyBut.onClick.AddListener(async () => {
            //扣除体力------todo
            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/patrol/receiveFastPatrol", DataManager.Get().getHeader());
            Debug.Log(str);
            PatrolNetData data = JsonUtil.ReadData<PatrolNetData>(str);
            if (data != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");

                List<ItemInfo> infos = new List<ItemInfo>();

                infos.Add(new ItemInfo("p10001", data.gold));
                infos.Add(new ItemInfo("p10008", data.exp));

                foreach (var item in data.material)
                {
                    infos.Add(item);
                }
                foreach (var item in data.specialMaterial)
                {
                    infos.Add(item);
                }
                MessageMgr.SendMsg("GetReward", new MsgKV("", infos));
                openQuickPatrolPanelAsync();
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("audio/金币获取音效"), Vector3.zero, 1.0f);
                MessageMgr.SendMsg("RefreshTip", null);
            }
        });
        GetBut.onClick.AddListener(async () => {
            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/patrol/receivePatrol", DataManager.Get().getHeader());
            Debug.Log(str);
            PatrolNetData data = JsonUtil.ReadData<PatrolNetData>(str);
            if (data != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");

                List<ItemInfo> infos = new List<ItemInfo>();

                infos.Add(new ItemInfo("p10001", data.gold));
                infos.Add(new ItemInfo("p10008", data.exp));

                foreach (var item in data.material) {
                    infos.Add(item);
                }
                foreach (var item in data.specialMaterial)
                {
                    infos.Add(item);
                }
                MessageMgr.SendMsg("GetReward", new MsgKV("", infos));
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("audio/金币获取音效"), Vector3.zero, 1.0f);
                MessageMgr.SendMsg("RefreshTip", null);
            }
            RefreshAsync();
        });

        MessageMgr.AddMsgListener("RefreshStrength", p =>
        {
            setStrength();
        });


        goldText = UIFrameUtil.FindChildNode(this.transform, "gold/Image (1)/Text (TMP)").GetComponent<TextMeshProUGUI>();
        expText = UIFrameUtil.FindChildNode(this.transform, "exp/Image (1)/Text (TMP)").GetComponent<TextMeshProUGUI>();
        timeText = UIFrameUtil.FindChildNode(this.transform, "time").GetComponent<TextMeshProUGUI>();


        slotTra_1 = UIFrameUtil.FindChildNode(this.transform, "PatrolPanel/items");
        ItemSlotList = new List<ItemSlot>();
        slot_pf = slotTra_1.GetChild(0).gameObject;
        for (int i = 0; i < slotTra_1.childCount; i++)
        {
            ItemSlot slot = slotTra_1.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            ItemSlotList.Add(slot);
        }


        slotTra_2 = UIFrameUtil.FindChildNode(this.transform, "QuickPatrolPanel/Panel/items");
        quick_ItemSlotList = new List<ItemSlot>();
        for (int i = 0; i < slotTra_2.childCount; i++)
        {
            ItemSlot slot = slotTra_2.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            quick_ItemSlotList.Add(slot);
        }

        if (1 >= ItemSlotList.Count)
        {
            GameObject g = Instantiate(slot_pf, slotTra_1);
            ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
            ItemSlot.mgr = this;
            ItemSlotList.Add(ItemSlot);
        }
    }


    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }

    PatrolNetData nowData;
    float times;

    public async Task RefreshAsync()
    {
        nowData = null;
        times = 0;
        timeText.text = "Patrol Time: ";
        goldText.text = "";
        expText.text = "";
        for (int i = 0; i < ItemSlotList.Count; i++)
            ItemSlotList[i].Hide();


        //挂机轮训
        string st = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/hungUp/push", DataManager.Get().getHeader());

        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/patrol/getInfo", DataManager.Get().getHeader());

        Debug.Log(str);

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode != null) {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            return;
        }
         
        nowData = JsonUtil.ReadData<PatrolNetData>(str);
        if (nowData == null) {
            //没有网络
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
            return;
        }


        timeText.text = "Patrol Time: "+ NumUtil.getTime(nowData.hookTimeSeconds);

        //显示玩家每小时的金钱 经验收益
        goldText.text = nowData.goldMinute + " /m";
        expText.text = nowData.expMinute + " /m";

        int index = 0;
        GetBut.interactable = false;
        if (nowData.gold > 0) {
            index++;
            GetBut.interactable = true;
            ItemSlotList[0].Refresh(new ItemInfo("p10001", nowData.gold));
        }
        if (nowData.exp > 0) {
            index++;
            ItemSlotList[1].Refresh(new ItemInfo("p10008", nowData.exp));
        }
        for (int i = 0;i < nowData.material.Count; i++) {
            //槽位不足则增加  多出则隐藏
            if (i+ index >= ItemSlotList.Count) {
                GameObject g = Instantiate(slot_pf, slotTra_1);
                ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
                ItemSlot.mgr = this;
                ItemSlotList.Add(ItemSlot);
            }
            ItemSlotList[i+ index].RefreshForPatrol(nowData.material[i]);
        }
        for (int i = 0; i < nowData.specialMaterial.Count; i++)
        {
            //槽位不足则增加  多出则隐藏
            if ((nowData.material.Count + i+ index) >= ItemSlotList.Count)
            {
                GameObject g = Instantiate(slot_pf, slotTra_1);
                ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
                ItemSlot.mgr = this;
                ItemSlotList.Add(ItemSlot);
            }
            ItemSlotList[nowData.material.Count + i + index].RefreshForPatrol(nowData.specialMaterial[i]);
        }
    }

    //打开快速巡逻面板
    public async Task openQuickPatrolPanelAsync() {

        for (int i = 0; i < quick_ItemSlotList.Count; i++)
            quick_ItemSlotList[i].Hide();

        QuickPatrolPanel.SetActive(false);
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/patrol/getFastInfo", DataManager.Get().getHeader());
        Debug.Log(str);

        QuickPatrolPanel.SetActive(true);

        PatrolNetData data = JsonUtil.ReadData<PatrolNetData>(str);
        if (data == null)
        {
            //没有网络
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
            return;
        }

        setStrength();

        quick_ItemSlotList[0].Refresh(new ItemInfo("p10001", data.gold));
        if (1 >= quick_ItemSlotList.Count)
        {
            GameObject g = Instantiate(slot_pf, slotTra_2);
            ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
            ItemSlot.mgr = this;
            quick_ItemSlotList.Add(ItemSlot);
        }
        quick_ItemSlotList[1].Refresh(new ItemInfo("p10008", data.exp));

        for (int i = 0; i < data.material.Count; i++)
        {
            //槽位不足则增加  多出则隐藏
            if (i+2 >= quick_ItemSlotList.Count)
            {
                GameObject g = Instantiate(slot_pf, slotTra_2);
                ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
                ItemSlot.mgr = this;
                quick_ItemSlotList.Add(ItemSlot);
            }
            quick_ItemSlotList[i+2].RefreshForPatrol(data.material[i]);
        }
        for (int i = 0; i < data.specialMaterial.Count; i++)
        {
            //槽位不足则增加  多出则隐藏
            if ((data.material.Count + i+2) >= quick_ItemSlotList.Count)
            {
                GameObject g = Instantiate(slot_pf, slotTra_2);
                ItemSlot ItemSlot = g.GetComponent<ItemSlot>();
                ItemSlot.mgr = this;
                quick_ItemSlotList.Add(ItemSlot);
            }
            quick_ItemSlotList[data.material.Count + i+2].RefreshForPatrol(data.specialMaterial[i]);
        }
    }


    void setStrength() {
        buyButText.text = "10/" + DataManager.Get().roleAttrData.strength.strength;
        if (DataManager.Get().roleAttrData.strength.strength < 10)
        {
            buyBut.interactable = false;
        }
        else
        {
            buyBut.interactable = true;
        }

    }


    public void Update()
    {
        if (nowData != null) {
            if (nowData.hookTimeSeconds < 86400) { 
                times +=Time.deltaTime;
                timeText.text = "Patrol Time: " + NumUtil.getTime(nowData.hookTimeSeconds + Mathf.RoundToInt(times));
            }
        }
    }

    
}

public class PatrolNetData
{ 
    public int hookTimeSeconds;
    public int exp;
    public int gold;
    public int expMinute;
    public int goldMinute;
    public List<ItemInfo> material;
    public List<ItemInfo> specialMaterial;
}   