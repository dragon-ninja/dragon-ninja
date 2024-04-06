using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentLevelSlot : BaseSlot
{

    protected override void Awake()
    {
        base.Awake();
    }


    public void Refresh(bool unlock)
    {
        Show();

        if (unlock)
            icon.sprite = Resources.Load<Sprite>("ui/img/talent/数字背景");
        else
            icon.sprite = Resources.Load<Sprite>("ui/img/talent/数字背景灰");
    }
}
