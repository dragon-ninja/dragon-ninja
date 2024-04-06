using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTowerRelicEventSlot : BaseSlot
{
    
    
    List<Transform> stars;
    Relic now_relic;
    TextMeshProUGUI nameText;
    TextMeshProUGUI descText;

    protected override void Awake()
    {
        base.Awake();

        nameText = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        descText = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();

        background = UIFrameUtil.FindChildNode(this.transform, "item").GetComponent<Image>();
        icon = UIFrameUtil.FindChildNode(this.transform, "icon").GetComponent<Image>();
        Transform listTra = UIFrameUtil.FindChildNode(this.transform, "list");
        stars = new List<Transform>();
        for (int i = 0; i < listTra.childCount; i++)
        {
            stars.Add(listTra.GetChild(i));
        }

        GetComponent<Button>().onClick.AddListener(() => {
            ((TowerRelicEventForm)mgr).SelectRelic(index);
        });
    }

   

    public void Refresh(Relic relic)
    {
        Show();

        now_relic = relic;
        RelicConfig now_config = TowerFactory.Get().relicMap[relic.configId];
        icon.sprite = Resources.Load<Sprite>(now_config.icon);
        if(now_config.name==null)
            nameText.text = "未命名";
        else
            nameText.text = now_config.name;

        if(relic.level==0)
            descText.text = now_config.desc_1;
        else if (relic.level == 1)
            descText.text = now_config.desc_2;
        else if (relic.level == 2)
            descText.text = now_config.desc_3;

        for (int i = 0; i < stars.Count; i++)
        {
            if (relic.level >= i)
                stars[i].gameObject.SetActive(true);
            else
                stars[i].gameObject.SetActive(false);
        }
        stars[0].parent.gameObject.SetActive(false);

        background.sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + now_relic.quality);
    }
}