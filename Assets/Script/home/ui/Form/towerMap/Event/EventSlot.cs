using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventSlot : BaseSlot
{
    TextMeshProUGUI desc;

    bool awakeFlag;

    protected override void Awake()
    {
        if (awakeFlag)
            return;

        awakeFlag = true;

        base.Awake();
        this.GetComponent<Button>().onClick.AddListener(() => {
                MessageMgr.SendMsg("selectEvent",
                    new MsgKV("", index));
        });

        desc = UIFrameUtil.FindChildNode(this.transform, "Text (TMP)").GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(string str)
    {
        Awake();
        desc.text = str;
        Show();
    }
}
