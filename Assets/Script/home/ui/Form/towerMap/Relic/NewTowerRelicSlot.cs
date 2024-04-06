using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTowerRelicSlot : BaseSlot
{
    TextMeshProUGUI nameText;
    TextMeshProUGUI descText;
    GameObject newTip;
    TextMeshProUGUI numText;

    protected override void Awake()
    {
        init();
    }

    void init() {

        if (initFlag)
            return;

        initFlag = true;
        nameText = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        descText = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        background = UIFrameUtil.FindChildNode(this.transform, "dk").GetComponent<Image>();
        icon = UIFrameUtil.FindChildNode(this.transform, "icon").GetComponent<Image>();
        newTip = UIFrameUtil.FindChildNode(this.transform, "new").gameObject;
        numText = UIFrameUtil.FindChildNode(this.transform, "num").GetComponent<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(() => {
            //((TowerRelicEventForm)mgr).SelectRelic(index);
        });
    }
   

    public void Refresh(Relic relic)
    {
        if (!initFlag)
            init();

        Show();
        RelicConfig now_config = TowerFactory.Get().relicMap[relic.configId];
        icon.sprite = Resources.Load<Sprite>(now_config.icon);
        if(now_config.name == null)
            nameText.text = "未命名";
        else
            nameText.text = now_config.name;

        if(relic.level==0)
            descText.text = now_config.desc_1;
        else if (relic.level == 1)
            descText.text = now_config.desc_2;
        else if (relic.level == 2)
            descText.text = now_config.desc_3;


        List<Relic> relicList = DataManager.Get().userData.towerData.relicList.FindAll(x => x.configId == relic.configId);
        if (relicList == null || relicList.Count == 0)
        {
            newTip.SetActive(true);
            numText.text = "Own: 0";
        }
        else {
            newTip.SetActive(false);
            numText.text = "Own: " + relicList.Count;
        }

        background.sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + relic.quality);
    }
}