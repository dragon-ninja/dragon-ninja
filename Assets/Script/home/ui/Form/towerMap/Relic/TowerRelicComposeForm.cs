using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerRelicComposeForm : BaseUIForm
{
    TextMeshProUGUI tip;
    List<TowerRelicComposeSlot> slotList;
    TowerRelicComposeSlot resultSlot;


    public Relic r_1 = null;
    public Relic r_2 = null;
    public Relic r_3 = null;

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
        r_3 = null;


        GetBut(this.transform, "close").onClick.AddListener(() => {
            CloseForm();
        });

        tip = UIFrameUtil.FindChildNode(this.transform, "tip").GetComponent<TextMeshProUGUI>();

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
            if (source == "BackPack")
                OpenForm("TowerBackPack");
            else
                OpenForm("TowerRelicEventForm");
        });

        MessageMgr.AddMsgListener("ComposeOrRecastingForBackPack", p =>
        {
            source = "BackPack";
        });
        MessageMgr.AddMsgListener("ComposeOrRecastingForRelicEvent", p =>
        {
            source = "RelicEvent";
        });

        //选择圣物   第一个槽  随便选  不能升级的除外
        //第二个,第三个槽  要匹配第一个已选择的圣物
        //第二个,第三个槽有了东西之后  合成变的可以点击
        //否则弹出提示 至少需要两个相同的圣物

        slotList = new List<TowerRelicComposeSlot>();
        Transform slotTra = UIFrameUtil.FindChildNode(this.transform, "list");
        for (int i = 0; i < slotTra.childCount; i++)
        {
            slotList.Add(slotTra.GetChild(i).GetComponent<TowerRelicComposeSlot>());
            slotList[i].index = i;
            slotList[i].mgr = this;
        }
        resultSlot = UIFrameUtil.FindChildNode(this.transform, "result").GetComponent<TowerRelicComposeSlot>();

        MessageMgr.AddMsgListener("RelicComposeClick", p =>
        {
            nowIndex = (int)p.Value;
            OpenRelicSelect();
        });

        //接收选择的合成圣物
        MessageMgr.AddMsgListener("SelectRelic_hc", p =>
        {
            if (nowIndex == 0) 
                r_1 = (Relic)p.Value;
            if (nowIndex == 1)
                r_2 = (Relic)p.Value;
            if (nowIndex == 2)
                r_3 = (Relic)p.Value;

            slotList[nowIndex].Refresh((Relic)p.Value);
        });

        //合成
        GetBut(this.transform,"Button").onClick.AddListener(() => {
            //校验是否满足合成条件
            int relicNum = 0;
            Relic r = null;
            if (r_1 != null) { 
                relicNum++;r = r_1;
            }
            if (r_2 != null)
            {
                relicNum++; r = r_2;
            }
            if (r_3 != null)
            {
                relicNum++; r = r_3;
            }

            if (relicNum == 2)
            {
                Relic relic_1 =  DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r.configId && x.level==r.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_1);
                Relic relic_2 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r.configId && x.level == r.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_2);

                //获得一个进阶圣物
                Relic newR = new Relic();
                newR.configId = r.configId;
                newR.level = Mathf.Min(r.level + 1,2);
                RelicConfig config = TowerFactory.Get().relicMap[r.configId];
                newR.quality = config.quality;
                DataManager.Get().userData.towerData.relicList.Add(newR);
                DataManager.Get().save();
                CloseForm();
                if (source == "BackPack")
                    OpenForm("TowerBackPack");
                else
                    OpenForm("TowerRelicEventForm");
            }
            if (relicNum == 3)
            {
                Relic relic_1 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r.configId && x.level == r.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_1);
                Relic relic_2 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r.configId && x.level == r.level);
                DataManager.Get().userData.towerData.relicList.Remove(relic_2);
                if (r.level == 0)
                {
                    Relic relic_3 = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == r.configId && x.level == r.level);
                    DataManager.Get().userData.towerData.relicList.Remove(relic_3);
                }

                if (r.level == 2)
                {
                    //提示relic已达到最大等级
                    //tip.text = "relic已达到最大等级";
                    //tip.color = new Color(1,1,1,1);
                }
                else { 
                    //获得一个满阶圣物
                    Relic newR = new Relic();
                    newR.configId = r.configId;
                    newR.level = 2;
                    RelicConfig config = TowerFactory.Get().relicMap[r.configId];
                    newR.quality = config.quality;
                    DataManager.Get().userData.towerData.relicList.Add(newR);
                    DataManager.Get().save();
                    CloseForm();
                    if (source == "BackPack")
                        OpenForm("TowerBackPack");
                    else
                        OpenForm("TowerRelicEventForm");
                }
            }
            else {
                //提示至少需要两个相同的relic
                //tip.text = "至少需要两个相同的relic";
                //tip.color = new Color(1, 1, 1, 1);
            }
        });
    }

    public override void Show() {
        base.Show();
        r_1 = null;
        r_2 = null;
        r_3 = null;
        //tip.color = new Color(1, 1, 1, 0);
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].Refresh();
        }
    }

    public void OpenRelicSelect() {
        OpenForm("TowerBackPackSelectForm");
        MessageMgr.SendMsg("RelicSelectShow_hc",
              new MsgKV("", this));
    }



}
