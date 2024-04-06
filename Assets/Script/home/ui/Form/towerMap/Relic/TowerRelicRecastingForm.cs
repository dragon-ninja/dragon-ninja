using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerRelicRecastingForm : BaseUIForm
{
    TextMeshProUGUI tip;
    List<TowerRelicRecastingSlot> slotList;

    public Relic r_1 = null;
    public Relic r_2 = null;

    int nowIndex = 0;

    string source;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        r_1 = null;
        r_2 = null;


        GetBut(this.transform, "close").onClick.AddListener(() => {
            CloseForm();
        });

        tip = UIFrameUtil.FindChildNode(this.transform, "tip").GetComponent<TextMeshProUGUI>();

        slotList = new List<TowerRelicRecastingSlot>();
        Transform slotTra = UIFrameUtil.FindChildNode(this.transform, "list");
        for (int i = 0; i < slotTra.childCount; i++)
        {
            slotList.Add(slotTra.GetChild(i).GetComponent<TowerRelicRecastingSlot>());
            slotList[i].index = i;
            slotList[i].mgr = this;
        }

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
            if (source == "BackPack")
                OpenForm("TowerBackPack");
            else
                OpenForm("TowerRelicEventForm");
        });

        MessageMgr.AddMsgListener("ComposeOrRecastingForBackPack", p =>
        {
            Debug.Log("收到了1");
            source = "BackPack";
        });
        MessageMgr.AddMsgListener("ComposeOrRecastingForRelicEvent", p =>
        {
            Debug.Log("收到了2");
            source = "RelicEvent";
        });

        MessageMgr.AddMsgListener("RelicRecastingClick", p =>
        {
            nowIndex = (int)p.Value;
            OpenRelicSelect();
        });

        //接收选择的合成圣物
        MessageMgr.AddMsgListener("SelectRelic_cz", p =>
        {
            if (nowIndex == 0)
                r_1 = (Relic)p.Value;
            if (nowIndex == 1)
                r_2 = (Relic)p.Value;

            slotList[nowIndex].Refresh((Relic)p.Value);
        });

        //重铸
        GetBut(this.transform, "Button").onClick.AddListener(() => {
            //校验是否满足合成条件
            if (r_1 != null && r_2 != null)
            {
                Relic relic_1 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r_1.configId && x.level == r_1.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_1);
                Relic relic_2 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r_2.configId && x.level == r_2.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_2);

                //获得一个随机更高品质圣物
                List<RelicConfig> clist = TowerFactory.Get().relicList.FindAll
                  (x=> x.quality == Mathf.Min(r_1.quality+1,2));
                RelicConfig config = clist[Random.Range(0, clist.Count)];
                Relic relic = new Relic();
                relic.configId = config.id;
                relic.level = 0;
                relic.quality = config.quality;

                DataManager.Get().userData.towerData.relicList.Add(relic);
                DataManager.Get().save();
                CloseForm();
                if (source == "BackPack")
                {
                    MessageMgr.SendMsg("RefreshTowerBackPack",
                            new MsgKV("", this));
                    OpenForm("TowerBackPack");
                }
                else { 
                    OpenForm("TowerRelicEventForm");
                }
            }
            else
            {
                //提示至少需要两个相同的relic
                //tip.text = "需要两个相同品质的relic";
                //tip.color = new Color(1, 1, 1, 1);
            }
        });
    }

    public override void Show()
    {
        base.Show();
        r_1 = null;
        r_2 = null;

        //tip.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].Refresh();
        }
    }

    public void OpenRelicSelect()
    {
        OpenForm("TowerBackPackSelectForm");
        MessageMgr.SendMsg("RelicSelectShow_cz",
              new MsgKV("", this));
    }



}

