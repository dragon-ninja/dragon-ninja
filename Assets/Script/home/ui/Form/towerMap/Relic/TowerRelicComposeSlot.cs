using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerRelicComposeSlot : BaseSlot
{

    List<Transform> stars;
    GameObject tip;

    protected override void Awake()
    {
        base.Awake();

        background = transform.Find("item").GetComponent<Image>();

        icon = transform.Find("item").Find("icon").GetComponent<Image>();

        tip = transform.Find("Text (TMP)").gameObject;

        Transform listTra = UIFrameUtil.FindChildNode(this.transform, "list");
        stars = new List<Transform>();
        for (int i = 0; i < listTra.childCount; i++)
        {
            stars.Add(listTra.GetChild(i));
        }

        this.GetComponent<Button>().onClick.AddListener(() => {
            MessageMgr.SendMsg("RelicComposeClick",
               new MsgKV("", index));
        });
    }

    public void Refresh() {
        Show();
        tip.SetActive(true);
        icon.transform.parent.gameObject.SetActive(false);
    }

    public void Refresh(Relic relic)
    {
        Show();

        tip.SetActive(false);
        icon.transform.parent.gameObject.SetActive(true);
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

        background.sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + now_config.quality);
    }
}
