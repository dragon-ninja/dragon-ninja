using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class NewTowerRelicEventForm : BaseUIForm
{
    TextMeshProUGUI RewardNumText;
    TextMeshProUGUI expReward;
    TextMeshProUGUI goldReward;
    List<Button> selectButs;


    TowerMapForm mapForm;

    List<Relic> now_RelicList;
    List<NewTowerRelicSlot> selectSlotList;

    GameObject selectRelicPanel;
    int maxRewardsNum;
    int rewardsNum;
    List<KeyValue> boxRewards;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;


        selectRelicPanel = UIFrameUtil.FindChildNode(this.transform, "selectRelic").gameObject;
        RewardNumText = UIFrameUtil.FindChildNode(this.transform, "RewardNumText").GetComponent<TextMeshProUGUI>();
        expReward = UIFrameUtil.FindChildNode(this.transform, "expReward/desc").GetComponent<TextMeshProUGUI>();
        goldReward = UIFrameUtil.FindChildNode(this.transform, "goldReward/desc").GetComponent<TextMeshProUGUI>();

        selectButs = new List<Button>();
        Transform selectTra = UIFrameUtil.FindChildNode(this.transform, "list");
        for (int i = 0; i < selectTra.childCount; i++)
        {
            selectButs.Add(selectTra.GetChild(i).GetComponent<Button>());
        }
        selectButs[0].onClick.AddListener(() => {
            select(0);
        });
        selectButs[1].onClick.AddListener(() => {
            select(1);
        });
        selectButs[2].onClick.AddListener(() => {
            select(2);
        });


        now_RelicList = new List<Relic>();
        selectSlotList = new List<NewTowerRelicSlot>();
        Transform relicTra = UIFrameUtil.FindChildNode(this.transform, "relicList");
        for (int i = 0; i < relicTra.childCount; i++)
        {
            selectSlotList.Add(relicTra.GetChild(i).GetComponent<NewTowerRelicSlot>());
            selectSlotList[i].mgr = this;
            selectSlotList[i].index = i;
        }

        MessageMgr.AddMsgListener("BoxRewards", p =>
        {
            maxRewardsNum = 2;
            rewardsNum = 0;
            this.boxRewards = (List<KeyValue>)p.Value;


            selectButs[0].gameObject.SetActive(true);
            selectButs[1].gameObject.SetActive(true);
            selectButs[2].gameObject.SetActive(true);
            Refresh();

            RewardNumText.text = "take away a treasures\r\n(" + rewardsNum + " / " + maxRewardsNum + ")";

        });
        MessageMgr.AddMsgListener("ForTowerMapForm", p =>
        {
            mapForm = (TowerMapForm)p.Value;
        });

        GetComponent<Button>().onClick.AddListener(() => {
            //不能随意关闭 需领完奖励
            if (rewardsNum == maxRewardsNum)
            {
                close();
            }
            else
            {
                //todo 提示
            }
        });
    }

    public override void Show()
    {
        //基础奖励
        base.Show();
        //Refresh();
    }

    public void Refresh()
    {
        int exp = (boxRewards.Find(x => x.key == "exp").value);
        string expStr = exp + "";
        if (exp > 1000)
        {
            expStr = (exp / 1000) + "k";
        }

        int gold = (boxRewards.Find(x => x.key == "gold").value);
        string goldStr = gold + "";
        if (gold > 1000)
        {
            goldStr = (gold / 1000) + "k";
        }

        expReward.text = "Exp +" + expStr;
        goldReward.text = "Gold +" + goldStr;
    }

    public void select(int index) {
        if (index == 0)
        {
            DataManager.Get().userData.towerData.extraExp += (boxRewards.Find(x => x.key == "exp").value);
            DataManager.Get().save();

            rewardsNum++;
            selectButs[0].gameObject.SetActive(false);
        }
        if (index == 1)
        {
            DataManager.Get().userData.towerData.gold += (boxRewards.Find(x => x.key == "gold").value);
            DataManager.Get().save();
            rewardsNum++;
            selectButs[1].gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("audio/金币获取音效"), Vector3.zero, 1.0f);
        }
        if (index == 2) {
            selectButs[2].gameObject.SetActive(false);
            openSelectRelic();
        }

        RewardNumText.text = "take away a treasures\r\n(" + rewardsNum + " / " + maxRewardsNum + ")";

        if (rewardsNum == maxRewardsNum) {
            close();
        }
    }


    public void openSelectRelic() {

        selectRelicPanel.SetActive(true);
        creatRelic(-1);
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
            //获取不重复的宝物
            List<RelicConfig> RelicConfigList = new List<RelicConfig>();
            if (Q != -1)
            {
                foreach (RelicConfig c in TowerFactory.Get().relicList.FindAll(x => x.quality == Q)) {
                    RelicConfigList.Add(c);
                }
            }
            else {
                foreach (RelicConfig c in TowerFactory.Get().relicList)
                {
                    RelicConfigList.Add(c);
                }
            }

            for (int i=0;i<3;i++) {
                int index = Random.Range(0, RelicConfigList.Count);
                RelicConfig config = RelicConfigList[index];
                RelicConfigList.RemoveAt(index);
                Relic relic = new Relic();
                relic.configId = config.id;
                relic.level = 0;
                relic.quality = config.quality;
                now_RelicList.Add(relic);
                selectSlotList[i].Refresh(relic);
            }
        }
    }

    public void selectRelic(int index) {
        rewardsNum++;
        RewardNumText.text = "take away a treasures\r\n(" + rewardsNum + " / " + maxRewardsNum + ")";
        if (rewardsNum == maxRewardsNum)
        {
            close();
        }
        selectRelicPanel.SetActive(false);

        DataManager.Get().userData.towerData.relicList.Add(now_RelicList[index]);
        DataManager.Get().save();
        now_RelicList.Clear();
    }


    void close() {
        mapForm.nodeEnd();
        CloseForm();
        //刷新楼层显示
        mapForm.RefreshStorey();
    }

}