using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementForm : BaseUIForm
{
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

    }
}

