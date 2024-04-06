using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPackForm : BaseUIForm
{

    List<ItemSlot> slotList = new List<ItemSlot>();
    Transform itemListNode;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm();
        });
        GetBut(this.transform, "close").onClick.AddListener(() => {
            CloseForm();
        });

        itemListNode = UIFrameUtil.FindChildNode(this.transform, "itemList");

        slotList.Add(UIFrameUtil.FindChildNode(this.transform,
            "itemList/item").GetComponent<ItemSlot>());


        Refresh("all");
    }

    public override void Show()
    {
        base.Show();
        Refresh("all");
    }


    public async void Refresh(string type) {
        //找到玩家背包中不属于装备的道具 显示

        string BackPackStr = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack", DataManager.Get().getHeader());
        DataManager.Get().backPackData = JsonUtil.ReadData<BackPackData>(BackPackStr);


        List<EquipmentData> eqList = DataManager.Get().backPackData.backPackItems;

        Debug.Log("BackPackStr:" + eqList.Count);

        eqList.Sort((a, b) =>
             (b.id + b.quality).CompareTo(a.id + a.quality));

        int index = 0;
        int equipmentNum = 0;
        for (int i = 0; i < eqList.Count; i++)
        {
            if (eqList[i].id == "p10000" || eqList[i].id == "p10001")
                continue;

            //ItemConfig atr = ItemFactory.Get().itemMap[eqList[i].id];
            equipmentNum++;

            if (slotList.Count <= index)
            {
                GameObject slot = Instantiate
                    (slotList[0].gameObject, itemListNode);
                slotList.Add(slot.GetComponent<ItemSlot>());
            }
            ItemInfo info = new ItemInfo();
            info.id = eqList[i].id;
            info.grade = eqList[i].quality;
            info.num = eqList[i].quantity;
            slotList[index++].Refresh(info);
        }

    }

}