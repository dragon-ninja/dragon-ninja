using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerBackPackSlot :  BaseSlot
{
    //weapon arm relic
    public string type;
    TextMeshProUGUI num;
    List<Transform> stars;

    SkillAttr now_skillAttr;
    RelicConfig now_config;
    int now_relicLevel;

    protected override void Awake()
    {
        base.Awake();

        if(type.IndexOf("relic")!=-1)
            num = UIFrameUtil.FindChildNode(this.transform, "num").
                GetComponent<TextMeshProUGUI>();

        Transform listTra = UIFrameUtil.FindChildNode(this.transform, "list");
        stars = new List<Transform>();
        for (int i=0;i<listTra.childCount;i++) {
            stars.Add(listTra.GetChild(i));
        }

        this.GetComponent<Button>().onClick.AddListener(() => {
            if (type == "relic_ForRelicEvent") {
                if (now_relicLevel == 2)
                    ((TowerRelicEventForm)mgr).ShowInfo(now_config.desc_3);
                else if (now_relicLevel == 1)
                    ((TowerRelicEventForm)mgr).ShowInfo(now_config.desc_2);
                else
                    ((TowerRelicEventForm)mgr).ShowInfo(now_config.desc_1);
            }
            else if (type.IndexOf("_battle") != -1)
            {
                if (type.IndexOf("relic") != -1)
                {
                    if (now_relicLevel == 2)
                        ((PausePanelNew)mgr).ShowInfo(now_config.desc_3);
                    else if (now_relicLevel == 1)
                        ((PausePanelNew)mgr).ShowInfo(now_config.desc_2);
                    else
                        ((PausePanelNew)mgr).ShowInfo(now_config.desc_1);
                }
                else if (now_skillAttr != null)
                    ((PausePanelNew)mgr).ShowInfo(now_skillAttr.desc);
            }
            else if (type == "relic") {
                if (now_relicLevel == 2)
                    ((TowerBackPack)mgr).ShowInfo(now_config.desc_3);
                else if (now_relicLevel == 1)
                    ((TowerBackPack)mgr).ShowInfo(now_config.desc_2);
                else
                    ((TowerBackPack)mgr).ShowInfo(now_config.desc_1);
            }
            else if(now_skillAttr != null)
                ((TowerBackPack)mgr).ShowInfo(now_skillAttr.desc);
        });
    }

    //清空
    public void Refresh() {
        Show();

        icon.gameObject.SetActive(false);
        for (int i = 0; i < stars.Count; i++)
            stars[i].gameObject.SetActive(false);


       /* if (type.IndexOf("weapon")!=-1)
        {
            background.sprite = Resources.Load<Sprite>
                ("ui/img/tower/towerBackPack/" + 2);
        }
        else*/ {
            background.sprite = Resources.Load<Sprite>
                   ("ui/img/tower/towerBackPack/" + 1);
        }
    }

    public void Refresh(SkillAttr skillAttr)
    {
        Show();

        now_skillAttr = skillAttr;

        //显示图标
        icon.sprite = Resources.Load<Sprite>
            ("skill/icon/" + skillAttr.icon);
        icon.gameObject.SetActive(true);

        for (int i = 0; i < stars.Count; i++)
        {
            if (skillAttr.level > i)
                stars[i].gameObject.SetActive(true);
            else
                stars[i].gameObject.SetActive(false);
        }

      

        /*if (type.IndexOf("weapon") != -1)
        {
            background.sprite = Resources.Load<Sprite>
                ("ui/img/tower/towerBackPack/" + 2);
        }
        else*/
        {
            background.sprite = Resources.Load<Sprite>
                   ("ui/img/tower/towerBackPack/" + 1);
        }

        if (skillAttr.level > 5 || skillAttr.skillForm == "SuperSkill")
        {
            background.sprite = Resources.Load<Sprite>
               ("ui/img/tower/towerBackPack/" + 3);
        }
    }

    public void Refresh(string configId,int level,int relicNum = 0)
    {
        Show();

        now_config = TowerFactory.Get().relicMap[configId];
        now_relicLevel = level;
        //显示图标
        icon.sprite = Resources.Load<Sprite>
            (now_config.icon);

        for (int i = 0; i < stars.Count; i++) {
            if (level >= i)
                stars[i].gameObject.SetActive(true);
            else
                stars[i].gameObject.SetActive(false);
        }
        stars[0].parent.gameObject.SetActive(false);

        if (relicNum > 1)
        {
            this.num.text = "x" + relicNum;
            this.num.gameObject.SetActive(true);
        }
        else {
            this.num.gameObject.SetActive(false);
        }

        background.sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + now_config.quality);
    }
}
