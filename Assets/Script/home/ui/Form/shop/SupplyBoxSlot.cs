using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBoxSlot : BaseSlot
{
    public void Refresh(EquipmentData info)
    {
        if (!initFlag)
            Awake();

        //根据id找到相应的物品图标
        string iconUrl = EquipmentFactory.Get().map[info.id].icon;
        icon.sprite = null;
        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);

        //显示品级
        //012  345=紫  6789=金  10=红

        int quality = info.quality;
        if (info.quality >= 3 && info.quality <= 5)
            quality = 3;
        if (info.quality >= 6 && info.quality <= 9)
            quality = 4;
        if (info.quality == 10)
            quality = 5;

        background.sprite = Resources.Load<Sprite>("ui/icon/item/dk/" + quality);
    }
}
