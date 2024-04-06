using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuseDescForm : BaseUIForm
{

    ItemSlot it;


    List<ItemSlot> ItemSlotList;
    Transform slotListTra;
    GameObject slotPf;

    TextMeshProUGUI desc2;



    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;


        it = UIFrameUtil.FindChildNode(this.transform, "items/item").GetComponent<ItemSlot>();
        desc2= UIFrameUtil.FindChildNode(this.transform, "desc2").GetComponent<TextMeshProUGUI>();

        slotListTra = UIFrameUtil.FindChildNode(this.transform, "items (1)");
        ItemSlotList = new List<ItemSlot>();
        slotPf = slotListTra.GetChild(0).gameObject;
        for (int i = 0; i < slotListTra.childCount; i++)
        {
            ItemSlot slot = slotListTra.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            ItemSlotList.Add(slot);
        }

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });

        MessageMgr.AddMsgListener("FuseDesc", p =>
        {
            OpenReward((List<ItemInfo>)p.Value);
        });
    }



    public void OpenReward(List<ItemInfo> items)
    {
        it.Refresh(items[0]);

        items.RemoveAt(0);

        if (items.Count>0) {
            desc2.gameObject.SetActive(true);
        }
        else{
            desc2.gameObject.SetActive(false);
        }


        for (int i = 0; i < ItemSlotList.Count; i++)
            ItemSlotList[i].Hide();

        for (int i = 0; i < items.Count; i++)
        {
            if (i >= ItemSlotList.Count)
            {
                GameObject g = Instantiate(slotPf, slotListTra);
                ItemSlot slot = g.GetComponent<ItemSlot>();
                slot.mgr = this;
                ItemSlotList.Add(slot);
            }
            ItemSlotList[i].Refresh(items[i]);
        }
    }

}
