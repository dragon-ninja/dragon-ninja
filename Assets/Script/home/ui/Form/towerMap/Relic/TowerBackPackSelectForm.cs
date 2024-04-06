using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerBackPackSelectForm : BaseUIForm
{
    Transform relicTra;
    public List<TowerBackPackSelectSlot> relicSlots;
    GameObject relicSlotPf;
    GameObject notDesc;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });

        GetBut(this.transform, "close").onClick.AddListener(() => {
            CloseForm();
        });

        MessageMgr.AddMsgListener("SelectRelic_hc", p =>
        {
            CloseForm();
        });
        MessageMgr.AddMsgListener("SelectRelic_cz", p =>
        {
            CloseForm();
        });

        notDesc = UIFrameUtil.FindChildNode(this.transform, "notDesc").gameObject;

        relicSlots = new List<TowerBackPackSelectSlot>();
        relicTra = UIFrameUtil.FindChildNode(this.transform, "relicList");
        relicSlotPf = relicTra.GetChild(0).gameObject;
        for (int i = 0; i < relicTra.childCount; i++)
        {
            relicSlots.Add(relicTra.GetChild(i).GetComponent<TowerBackPackSelectSlot>());
            relicSlots[i].mgr = this;
            relicSlots[i].gameObject.SetActive(false);
        }

        MessageMgr.AddMsgListener("RelicSelectShow_hc", p =>
        {
            Show((TowerRelicComposeForm)p.Value);
        });

        MessageMgr.AddMsgListener("RelicSelectShow_cz", p =>
        {
            Show((TowerRelicRecastingForm)p.Value);
        });
    }


    public void Show(TowerRelicComposeForm compose) {
        if (compose.r_1 == null && compose.r_2 == null && compose.r_3 == null)
        {
            Dictionary<string, int> now_relicList = new Dictionary<string, int>();
            List<Relic> relicList = DataManager.Get().userData.towerData.relicList;
            List<Relic> relicList2 = new List<Relic>();
            for (int i = 0; i < relicList.Count; i++)
            {
                RelicConfig config = TowerFactory.Get().relicMap[relicList[i].configId];
                relicList[i].quality = config.quality;
                if (config.effect_2 != null ) {
                    relicList2.Add(relicList[i]);
                }
            }
            relicList2 = (List<Relic>)(relicList2.OrderBy(o => o.quality).ThenBy(o => o.configId).ThenBy(o => o.level).ToList());
            if (relicList2 != null)
            {
                for (int i = 0; i < relicList2.Count || i < relicSlots.Count; i++)
                {
                    if (i >= relicSlots.Count)
                    {
                        GameObject g = Instantiate(relicSlotPf, relicTra);
                        TowerBackPackSelectSlot slot = g.GetComponent<TowerBackPackSelectSlot>();
                        slot.mgr = this;
                        relicSlots.Add(slot);
                    }
                    relicSlots[i].gameObject.SetActive(false);
                }

                int index = 0;
                foreach (var item in relicList2)
                {
                    relicSlots[index++].Refresh(item);
                }
            }
        }
        else {
            //找到符合configid和相应level的圣物供选择
            Relic r = null;
            int relicNum = 0;
            if (compose.r_1 != null)
            {
                r = compose.r_1;
                relicNum++;
            }
            if (compose.r_2 != null)
            {
                r = compose.r_2;
                relicNum++;
            }
            if (compose.r_3 != null)
            { 
                r = compose.r_3;
                relicNum++;
            }

            Dictionary<string, int> now_relicList = new Dictionary<string, int>();
            List<Relic> relicList_origin = DataManager.Get().userData.towerData.relicList;
            List<Relic> relicList = new List<Relic>();
            
            for (int i = 0; i < relicList_origin.Count; i++)
            {
                RelicConfig config = TowerFactory.Get().relicMap[relicList_origin[i].configId];
                relicList_origin[i].quality = config.quality;
                if (relicList_origin[i].configId == r.configId 
                    && relicList_origin[i].level == r.level) {
                    if (relicNum <= 0)
                    {
                        relicList.Add(relicList_origin[i]);
                    }
                    else {
                        relicNum--;
                    }
                }
            }
            relicList = (List<Relic>)(relicList.OrderBy(o => o.quality).ThenBy(o => o.configId).ThenBy(o => o.level).ToList());
            if (relicList != null)
            {
                for (int i = 0; i < relicList.Count || i < relicSlots.Count; i++)
                {
                    if (i >= relicSlots.Count)
                    {
                        GameObject g = Instantiate(relicSlotPf, relicTra);
                        TowerBackPackSelectSlot slot = g.GetComponent<TowerBackPackSelectSlot>();
                        slot.mgr = this;
                        relicSlots.Add(slot);
                    }
                    relicSlots[i].gameObject.SetActive(false);
                }

                int index = 0;
                foreach (var item in relicList)
                {
                    relicSlots[index++].Refresh(item);
                }
            }

        }
    }

    public void Show(TowerRelicRecastingForm recasting) {

        notDesc.SetActive(false);

        if (recasting.r_1 == null && recasting.r_2 == null)
        {
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
                /*for (int i = 0; i < relicList.Count || i < relicSlots.Count; i++)
                {
                    if (i >= relicSlots.Count)
                    {
                        GameObject g = Instantiate(relicSlotPf, relicTra);
                        TowerBackPackSelectSlot slot = g.GetComponent<TowerBackPackSelectSlot>();
                        slot.mgr = this;
                        relicSlots.Add(slot);
                    }
                    relicSlots[i].gameObject.SetActive(false);
                }

                int index = 0;
                foreach (var item in relicList)
                {
                    relicSlots[index++].Refresh(item);
                }*/
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
                        TowerBackPackSelectSlot slot = g.GetComponent<TowerBackPackSelectSlot>();
                        slot.mgr = this;
                        relicSlots.Add(slot);
                    }
                    relicSlots[i].gameObject.SetActive(false);
                }

                int index = 0;
                foreach (var item in now_relicList)
                {
                    string[] s = item.Key.Split("_");

                    Relic relic = relicList.Find(item =>
                    item.configId == s[0] && item.level == int.Parse(s[1]));

                    relicSlots[index].Refresh(
                         relic, item.Value);
                    index++;
                }
            }
        }
        else
        {
            //找到符合configid和相应level的圣物供选择
            int relicNum_1 = 0;
            int relicNum_2 = 0;
            int Q = 0;
            if (recasting.r_1 != null)
            {
                Q = recasting.r_1.quality;
                relicNum_1++;
            }
            if (recasting.r_2 != null)
            {
                Q = recasting.r_2.quality;
                relicNum_2++;
            }

            //相应被选择的物品 数量减少1
            Dictionary<string, int> now_relicList = new Dictionary<string, int>();
            List<Relic> relicList_origin = DataManager.Get().userData.towerData.relicList;
            List<Relic> relicList = new List<Relic>();

            for (int i = 0; i < relicList_origin.Count; i++)
            {
                RelicConfig config = TowerFactory.Get().relicMap[relicList_origin[i].configId];
                relicList_origin[i].quality = config.quality;
                //这步剔除已放入槽位中的relic
                if (
                     (recasting.r_1 != null &&
                    relicList_origin[i].configId == recasting.r_1.configId &&
                    relicList_origin[i].level == recasting.r_1.level)
                 )
                {
                    if (relicNum_1 <= 0)
                    {
                        relicList.Add(relicList_origin[i]);
                    }
                    else
                    {
                        relicNum_1--;
                    }
                }
                else if ( (recasting.r_2 != null &&
                    relicList_origin[i].configId == recasting.r_2.configId &&
                    relicList_origin[i].level == recasting.r_2.level)) {
                    if (relicNum_2 <= 0)
                    {
                        relicList.Add(relicList_origin[i]);
                    }
                    else
                    {
                        relicNum_2--;
                    }
                }
                else if (relicList_origin[i].quality == Q)
                {
                    relicList.Add(relicList_origin[i]);
                }
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
                        TowerBackPackSelectSlot slot = g.GetComponent<TowerBackPackSelectSlot>();
                        slot.mgr = this;
                        relicSlots.Add(slot);
                    }
                    relicSlots[i].gameObject.SetActive(false);
                }

                int index = 0;
                foreach (var item in now_relicList)
                {
                    string[] s = item.Key.Split("_");
                    Relic relic = relicList.Find(item =>
                    item.configId == s[0] && item.level == int.Parse(s[1]));
                    relicSlots[index].Refresh(
                         relic, item.Value);
                    index++;
                }

                if(relicList.Count == 0) {
                    notDesc.SetActive(true);
                }
            }
        }
    }
}
