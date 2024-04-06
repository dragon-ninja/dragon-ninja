using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrForm : BaseUIForm
{
    TextMeshProUGUI desc;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;


        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });

        MessageMgr.AddMsgListener("ErrorDesc", p =>
        {
            desc.text = (string)p.Value;
        });
    }

}
