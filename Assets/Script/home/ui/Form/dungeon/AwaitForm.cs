using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AwaitForm : BaseUIForm
{
    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;
        
        MessageMgr.AddMsgListener("HideLoadForm", p =>
        {
            CloseForm();
        });
    }
}
