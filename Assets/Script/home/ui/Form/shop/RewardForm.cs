using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardForm : BaseUIForm
{
    List<ItemSlot> ItemSlotList;
    Transform slotListTra;
    GameObject slotPf;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        slotListTra = UIFrameUtil.FindChildNode(this.transform, "items");
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
            MessageMgr.SendMsg("GuideB_ShopGem", null);
        });

        MessageMgr.AddMsgListener("GetReward", p =>
        {
            OpenReward((List<ItemInfo>)p.Value);

            MessageMgr.SendMsg("UpMenuRefresh",
                           new MsgKV("", null));
        });
    }



    public void OpenReward(List<ItemInfo> items)
    {
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
