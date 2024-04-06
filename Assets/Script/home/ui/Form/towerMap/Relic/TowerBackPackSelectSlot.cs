using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerBackPackSelectSlot : BaseSlot
{
    TextMeshProUGUI num;
    List<Transform> stars;
    Relic now_relic;

    protected override void Awake()
    {
        base.Awake();
        num = UIFrameUtil.FindChildNode(this.transform, "num").
            GetComponent<TextMeshProUGUI>();

        Transform listTra = UIFrameUtil.FindChildNode(this.transform, "list");
        stars = new List<Transform>();
        for (int i = 0; i < listTra.childCount; i++){
            stars.Add(listTra.GetChild(i));
        }

        GetComponent<Button>().onClick.AddListener(() => {
            MessageMgr.SendMsg("SelectRelic_hc",
                    new MsgKV("", now_relic));
        });

        GetComponent<Button>().onClick.AddListener(() => {
            MessageMgr.SendMsg("SelectRelic_cz",
                    new MsgKV("", now_relic));
        });
    }

    public void Refresh(Relic relic,int relicNum = 0)
    {
        Show();

        now_relic = relic;
        RelicConfig now_config = TowerFactory.Get().relicMap[relic.configId];
        icon.sprite = Resources.Load<Sprite>
            (now_config.icon);

        for (int i = 0; i < stars.Count; i++)
        {
            if (relic.level >= i)
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
        else
        {
            this.num.gameObject.SetActive(false);
        }

        background.sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + now_relic.quality);
    }
}
