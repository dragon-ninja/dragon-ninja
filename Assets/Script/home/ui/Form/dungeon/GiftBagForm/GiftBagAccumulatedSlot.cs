using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiftBagAccumulatedSlot : BaseSlot
{
    TextMeshProUGUI num;
    TextMeshProUGUI day;
    ItemInfo nowItem = null;

    protected override void Awake()
    {
        base.Awake();

        num = UIFrameUtil.FindChildNode(this.transform, "num").
            GetComponent<TextMeshProUGUI>();

        day = UIFrameUtil.FindChildNode(this.transform, "dayNum").
            GetComponent<TextMeshProUGUI>();

        this.GetComponent<Button>().onClick.AddListener(() => {
           /* UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> list = new List<ItemInfo>();
            list.Add(nowItem);
            MessageMgr.SendMsg("GetReward", new MsgKV("", list));*/
        });
    }

    public void Refresh(ItemInfo info,string dayText)
    {
        Show();

        if (!initFlag)
            Awake();

        nowItem = info;

        day.text = dayText;


        //根据id找到相应的物品图标
        string iconUrl = null;
        int quality = 0;
        if (ItemFactory.Get().itemMap.ContainsKey(info.id))
        {
            iconUrl = ItemFactory.Get().itemMap[info.id].icon;
            quality = ItemFactory.Get().itemMap[info.id].quality;
            num.text = "x" + info.num;
            num.gameObject.SetActive(true);
        }
        else
        {
            iconUrl = EquipmentFactory.Get().map[info.id].icon;
            //显示品级
            //012  345=紫  6789=金  10=红
            if (info.grade >= 3 && info.grade <= 5)
                quality = 3;
            if (info.grade >= 6 && info.grade <= 9)
                quality = 4;
            if (info.grade == 10)
                quality = 5;

            num.gameObject.SetActive(false);
        }




        icon.sprite = null;
        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);

        background.sprite = Resources.Load<Sprite>("ui/icon/item/dk/" + quality);
    }
}
