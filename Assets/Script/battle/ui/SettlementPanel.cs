using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SettlementPanel : MonoBehaviour
{
    Slider slider;
    bool initFlag;
    AudioSource audioS;

    Image head;
    Animator roleAnim;

    TextMeshProUGUI HeadDesc;

    List<ItemSlot> ItemSlotList = new List<ItemSlot>();
    Transform slotListTra;
    GameObject slotPf;

    Button RewardsButton;


    TextMeshProUGUI TowerName;
    TextMeshProUGUI Final_layer;
    TextMeshProUGUI Kill_moasters;
    TextMeshProUGUI Battle_time;

    void init() {
        if (initFlag)
            return;

        initFlag = true;

        roleAnim = UIFrameUtil.FindChildNode(this.transform, "role").GetComponent<Animator>();

        slider = transform.Find("Panel/Slider").GetComponent<Slider>();
        slider.value = 0;
        /* Transform listTra = transform.Find("Panel/list");
         for ( int i=0;i<listTra.childCount;i++) {
             ItemSlotList.Add(listTra.GetChild(i).GetComponent<ItemSlot>());
         }*/

        HeadDesc = UIFrameUtil.FindChildNode(this.transform, "HeadDesc").GetComponent<TextMeshProUGUI>();

        TowerName = UIFrameUtil.FindChildNode(this.transform, "TowerName").GetComponent<TextMeshProUGUI>();
        Final_layer = UIFrameUtil.FindChildNode(this.transform, "Final_layer").GetComponent<TextMeshProUGUI>();
        Kill_moasters = UIFrameUtil.FindChildNode(this.transform, "Kill_moasters").GetComponent<TextMeshProUGUI>();
        Battle_time = UIFrameUtil.FindChildNode(this.transform, "Battle_time").GetComponent<TextMeshProUGUI>();



        head = UIFrameUtil.FindChildNode(this.transform, "head").GetComponent<Image>();
        slotListTra = UIFrameUtil.FindChildNode(this.transform, "Panel/list");
        ItemSlotList = new List<ItemSlot>();
        slotPf = slotListTra.GetChild(0).gameObject;
        for (int i = 0; i < slotListTra.childCount; i++)
        {
            ItemSlot slot = slotListTra.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            ItemSlotList.Add(slot);
        }


        RewardsButton = UIFrameUtil.FindChildNode(this.transform, "RewardsButton").GetComponent<Button>();
        RewardsButton.onClick.AddListener(() => {
            //退回主页
            //GameObject.Find("GameManager").GetComponent<GameSceneManage>().BackHome();
            Time.timeScale = 1;
            SceneManager.LoadScene("home");
        });
    }

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private async void EndSettlement(int state) {
        init();

        for (int i = 0; i < ItemSlotList.Count; i++)
            ItemSlotList[i].Hide();
        RewardsButton.interactable = false;


        this.gameObject.SetActive(true);
        //float progress = (TowerManager.EndStorey + 0.0f) / (TowerFactory.Get().tmMap[TowerManager.nowChapter].Count - 1);

        if (TowerManager.EndStorey == TowerFactory.Get().tmMap[TowerManager.nowChapter].Count - 1)
        {
            roleAnim.Play("win");
            HeadDesc.text = "Victory";
            head.sprite = Resources.Load<Sprite>("ui/icon/标题_常规");
            audioS.clip = Resources.Load<AudioClip>("audio/战斗胜利");
        }
        else
        {
            roleAnim.Play("loser");
            HeadDesc.text = "Failure";
            head.sprite = Resources.Load<Sprite>("ui/icon/标题_失败");
            audioS.clip = Resources.Load<AudioClip>("audio/战斗失败");
        }
        audioS.Play();

        TowerMap nowChapter = TowerFactory.Get().tmList.Find(x =>
        x.id == TowerFactory.Get().chapterList[DataManager.Get().now_chapterIndex]);
        TowerName.text = nowChapter.chapter;
        string start = nowChapter.name.Substring(0, nowChapter.name.IndexOf("-"));
        string end = nowChapter.name.Substring(nowChapter.name.IndexOf("-") + 1);
        Final_layer.text = "<color=#EA6100>Final layer:</color> "+ (TowerManager.EndStorey + int.Parse(start) - 1) + "/" + end; ;
        Kill_moasters.text = "<color=#EA6100>Kill moasters:</color> " + DataManager.Get().userData.towerData.killNum;

        string timeStr = SpriteNumUtil.zhTime((int)DataManager.Get().userData.towerData.gameTime);
        Battle_time.text = "<color=#EA6100>Battle time:</color> "+ timeStr;


        battleEnd data = new battleEnd();
        //Debug.Log(" TowerManager.nowChapter:" + TowerManager.nowChapter + "   TowerManager.DungeonStorey:" + TowerManager.EndStorey);
        data.chapterId = TowerManager.nowChapter;
        data.storey = TowerManager.EndStorey; //16;
        //data.storey = 16; //测试用

        data.killNum = DataManager.Get().userData.towerData.killNum;
        data.killBossNum = (TowerManager.EndStorey == TowerFactory.Get().tmMap[TowerManager.nowChapter].Count - 1)?1:0;
        data.relicNum = DataManager.Get().userData.towerData.relicList.Count;

        DataManager.Get().userData.towerData = null;
        //await DataManager.Get().save();
        string json1 = JsonConvert.SerializeObject(DataManager.Get().userData.towerData);
        string st = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/battleFlow/save", json1, DataManager.Get().getHeader());

        string json = JsonConvert.SerializeObject(data);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/battleFlow/battleEnd", 
            json, DataManager.Get().getHeader());
        if (str == null)
        {
            //没有网络
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
            this.gameObject.SetActive(false);
            return;
        }

       

        slider.value = (TowerManager.EndStorey+0.0f) / (TowerFactory.Get().tmMap[TowerManager.nowChapter].Count - 1);

     

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

      
        for (int i = 0; i < infoList.Count; i++)
        {
            if (i >= ItemSlotList.Count)
            {
                GameObject g = Instantiate(slotPf, slotListTra);
                ItemSlot slot = g.GetComponent<ItemSlot>();
                slot.mgr = this;
                ItemSlotList.Add(slot);
            }
            if (infoList[i].num != 0) {
                var info = infoList[i];
                ItemSlotList[i].Refresh(new ItemInfo(info.itemId, info.num, info.quality, info.level));
            }
        }




        RewardsButton.interactable = true;
    }

    /*0失败
     *1小关卡胜利
     *2boss胜利
     *3主动退出*/
    public void settlement(int state) {
        init();

        //退回主页  todo 爬塔改动
        if (state == 1)
        {
            TowerManager.EndStorey = int.Parse(DataManager.Get().userData.towerData.nowNode.Split("-")[0]);
            Time.timeScale = 1;
            SceneManager.LoadScene("tower");
        }
        else
        {
            if(state == 2)
                TowerManager.EndStorey = int.Parse(DataManager.Get().userData.towerData.nowNode.Split("-")[0]);

            EndSettlement(state);
        }
        return;
    }
}
