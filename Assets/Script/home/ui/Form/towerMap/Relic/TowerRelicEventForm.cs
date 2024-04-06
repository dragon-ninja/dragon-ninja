using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class TowerRelicEventForm : BaseUIForm
{
    List<Relic> now_RelicList;
    List<TowerRelicEventSlot> selectSlotList;
    //int selectNum_1;
    //int selectNum_2;
    //int selectNum_3;
    int nowSelectQ;

    Transform relicTra;
    List<TowerBackPackSlot> relicSlots;
    GameObject relicSlotPf;

    TextMeshProUGUI desc;
    TextMeshProUGUI num_1;
    TextMeshProUGUI num_2;
    TextMeshProUGUI num_3;

    TextMeshProUGUI notMoreTip;

    bool basicRewardsFlag;
    TowerMapForm mapForm;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        notMoreTip = UIFrameUtil.FindChildNode(this.transform, "notMoreTip").GetComponent<TextMeshProUGUI>();

        MessageMgr.AddMsgListener("ForTowerMapForm", p =>
        {
            mapForm = (TowerMapForm)p.Value;

            //提示升级
        });

        GetBut(this.transform, "close").onClick.AddListener(() => {
            if (!basicRewardsFlag)
            {
                mapForm.nodeEnd();
                CloseForm();
                //刷新楼层显示
                mapForm.RefreshStorey();
            }
            else {
                //todo 提示
            }
        });

        GetComponent<Button>().onClick.AddListener(() => {
            //不能随意关闭,至少领取完一次基础奖励才能关闭
            if (!basicRewardsFlag)
            {
                mapForm.nodeEnd();
                CloseForm();
                //刷新楼层显示
                mapForm.RefreshStorey();
            }
            else { 
                //todo 提示
            }
        });

        GetBut(this.transform, "Button_hc").onClick.AddListener(() => {
            OpenForm("TowerRelicComposeForm");
            CloseForm();

            MessageMgr.SendMsg("ComposeOrRecastingForRelicEvent",
                     new MsgKV("", null));
        });
        GetBut(this.transform, "Button_cz").onClick.AddListener(() => {
            OpenForm("TowerRelicRecastingForm");
            CloseForm();

            MessageMgr.SendMsg("ComposeOrRecastingForRelicEvent",
                    new MsgKV("", null));
        });

        now_RelicList = new List<Relic>();
        selectSlotList = new List<TowerRelicEventSlot>();
        Transform selectTra = UIFrameUtil.FindChildNode(this.transform, "selectList");
        for (int i = 0; i < selectTra.childCount; i++)
        {
            selectSlotList.Add(selectTra.GetChild(i).GetComponent<TowerRelicEventSlot>());
            selectSlotList[i].mgr = this;
            selectSlotList[i].index = i;
        }

        relicSlots = new List<TowerBackPackSlot>();
        relicTra = UIFrameUtil.FindChildNode(this.transform, "relicList");
        for (int i = 0; i < relicTra.childCount; i++)
        {
            relicSlots.Add(relicTra.GetChild(i).GetComponent<TowerBackPackSlot>());
            relicSlots[i].type = "relic_ForRelicEvent";
            relicSlots[i].mgr = this;
        }
        relicSlotPf = relicTra.GetChild(0).gameObject;

        desc = UIFrameUtil.FindChildNode(this.transform, "descText").GetComponent<TextMeshProUGUI>();
        num_1 = UIFrameUtil.FindChildNode(this.transform, "list/1/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_2 = UIFrameUtil.FindChildNode(this.transform, "list/2/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_3 = UIFrameUtil.FindChildNode(this.transform, "list/3/Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public override void Show()
    {
        //基础奖励
        basicRewardsFlag = TowerMapForm.RelicEventFlag ? true : basicRewardsFlag;
        TowerMapForm.RelicEventFlag = false;

        base.Show();
        Refresh();
    }

    public void Refresh()
    {
        num_1.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_1;
        num_2.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_2;
        num_3.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_3;

        Dictionary<string, int> now_relicList = new Dictionary<string, int>();
        List<Relic> relicList = DataManager.Get().userData.towerData.relicList;
        for (int i = 0; i < relicList.Count; i++)
        {
            RelicConfig config = TowerFactory.Get().relicMap[relicList[i].configId];
            relicList[i].quality = config.quality;
        }
        relicList = (List<Relic>)(relicList.OrderBy(o => o.quality).ThenBy(o => o.level).ToList());
        if (relicList != null)
        {
            for (int i = 0; i < relicList.Count; i++)
            {
                if (now_relicList.ContainsKey(relicList[i].configId + "_" + relicList[i].level))
                {
                    now_relicList
                        [relicList[i].configId + "_" + relicList[i].level]
                        += 1;
                }
                else
                {
                    now_relicList
                            [relicList[i].configId + "_" + relicList[i].level]
                            = 1;
                }
            }
            //建立对应的槽位  多余的隐藏
            for (int i = 0; i < now_relicList.Count || i < relicSlots.Count; i++)
            {
                if (i >= relicSlots.Count)
                {
                    GameObject g = Instantiate(relicSlotPf, relicTra);
                    TowerBackPackSlot slot = g.GetComponent<TowerBackPackSlot>();
                    slot.mgr = this;
                    slot.type = "relic_ForRelicEvent";
                    relicSlots.Add(slot);
                }
                relicSlots[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach (var item in now_relicList)
            {
                string[] s = item.Key.Split("_");
                relicSlots[index].Refresh(s[0], int.Parse(s[1]), item.Value);
                index++;
            }



            /*for (int i = 0; i < relicList.Count || i < relicSlots.Count; i++)
            {
                if (i >= relicSlots.Count)
                {
                    GameObject g = Instantiate(relicSlotPf, relicTra);
                    TowerBackPackSlot slot = g.GetComponent<TowerBackPackSlot>();
                    slot.mgr = this;
                    slot.type = "relic_ForRelicEvent";
                    relicSlots.Add(slot);
                }
                relicSlots[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach (var item in relicList)
            {
                relicSlots[index++].Refresh(item.configId, item.level);
            }*/
        }

        notMoreTip.gameObject.SetActive(false);
        if (basicRewardsFlag) {
            //基础奖励
            creatRelic(2);
        }
        else if (DataManager.Get().userData.towerData.relicEssenceNum_1 > 0)
        {
            creatRelic(0);
        }
        else if (DataManager.Get().userData.towerData.relicEssenceNum_2 > 0)
        {
            creatRelic(1);
        }
        else if (DataManager.Get().userData.towerData.relicEssenceNum_3 > 0)
        {
            creatRelic(2);
        }
        else {
            notMoreTip.gameObject.SetActive(true);
        }
    }

    public void ShowInfo(string str)
    {
        desc.text = str;
    }


    public void creatRelic(int Q) {
        //不刷新 显示之前的
        if (now_RelicList != null && now_RelicList.Count > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                selectSlotList[i].Refresh(now_RelicList[i]);
            }
        }
        else {
            nowSelectQ = Q;
            List<RelicConfig> RelicConfigList = 
                TowerFactory.Get().relicList.FindAll(x=> x.quality == Q);

            for (int i=0;i<3;i++) {
                RelicConfig config = RelicConfigList[Random.Range(0, RelicConfigList.Count)];
                Relic relic = new Relic();
                relic.configId = config.id;
                relic.level = 0;
                relic.quality = config.quality;
                now_RelicList.Add(relic);
                selectSlotList[i].Refresh(relic);
            }
        }
    }


    public void SelectRelic(int index) {
        //选择之后  次数--   now_RelicList清空

        if (nowSelectQ == 0) { 
            //selectNum_1--;
            if (!basicRewardsFlag)
            {
                DataManager.Get().userData.towerData.relicEssenceNum_1
                    = Mathf.Max(0, DataManager.Get().userData.towerData.relicEssenceNum_1 - 1);
            }
        }
        if (nowSelectQ == 1) { 
            //selectNum_2--;
            if (!basicRewardsFlag)
            {
                DataManager.Get().userData.towerData.relicEssenceNum_2
                    = Mathf.Max(0, DataManager.Get().userData.towerData.relicEssenceNum_2 - 1);
            }
        }
        if (nowSelectQ == 2) { 
            //selectNum_3--;
            if (!basicRewardsFlag)
            {
                DataManager.Get().userData.towerData.relicEssenceNum_3
                    = Mathf.Max(0, DataManager.Get().userData.towerData.relicEssenceNum_3 - 1);
            }
        }
        if (basicRewardsFlag) { 
            basicRewardsFlag = false;

        }


        for (int i = 0; i < 3; i++)
        {
            selectSlotList[i].Hide();
        }

        //DataManager.Get().userData.towerData.relicEssenceNum_1 = 2;
        //DataManager.Get().userData.towerData.relicEssenceNum_2 = 2;
        //DataManager.Get().userData.towerData.relicEssenceNum_3 = 2;

        DataManager.Get().userData.towerData.relicList.Add(now_RelicList[index]);
        DataManager.Get().save();

        now_RelicList.Clear();
        Refresh();
    }
}