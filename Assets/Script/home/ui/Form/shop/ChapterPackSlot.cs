using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChapterPackSlot : BaseSlot
{
    TextMeshProUGUI num;

    protected override void Awake()
    {
        initFlag = true;
        base.Awake();
        num = UIFrameUtil.FindChildNode(this.transform, "Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(ItemInfo info)
    {
        if (!initFlag)
            Awake();

        //根据id找到相应的物品图标
        string iconUrl = ItemFactory.Get().itemMap[info.id].icon;
        icon.sprite = null;
        //显示图标和数量
        icon.sprite = Resources.Load<Sprite>(iconUrl);
        num.text = "x"+ info.num;
    }

}
