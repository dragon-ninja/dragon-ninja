using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GrowthFundSlot : BaseSlot
{



    public int type;
    TextMeshProUGUI num;
    GameObject mask;
    GameObject lockImg;
    TextMeshProUGUI received;


    bool unlockFlag_now;
    bool buyFlag_now;

    public GrowthFundConfig config;

    protected override void Awake()
    {
        base.Awake();
        received = UIFrameUtil.FindChildNode(this.transform, "Received/ReceivedText").GetComponent<TextMeshProUGUI>();
        num = UIFrameUtil.FindChildNode(this.transform, "num").GetComponent<TextMeshProUGUI>();
        mask = UIFrameUtil.FindChildNode(this.transform, "mask").gameObject;
        lockImg = UIFrameUtil.FindChildNode(this.transform, "lock").gameObject;


        this.GetComponent<Button>().onClick.AddListener(() => {
            if (unlockFlag_now && buyFlag_now)
                MessageMgr.SendMsg("drawGrowthFund", new MsgKV(index.ToString(), type));
        });
    }


    ///info  等级是否解锁  是否已购买  是否已领取
    public void Refresh(ItemInfo info, bool unlockFlag,bool buyFlag,bool drawFlag = false) {
        Show();
        unlockFlag_now = unlockFlag;
        buyFlag_now = buyFlag;

        string iconUrl = ItemFactory.Get().itemMap[info.id].icon;
        icon.sprite = Resources.Load<Sprite>(iconUrl);

        num.text = "x" + info.num;



        if (unlockFlag)
            mask.gameObject.SetActive(false);
        else
            mask.gameObject.SetActive(true);

        if (buyFlag)
            lockImg.gameObject.SetActive(false);
        else
            lockImg.gameObject.SetActive(true);


        if (drawFlag)
        {
            received.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            received.transform.parent.gameObject.SetActive(false);
        }

    }

}
