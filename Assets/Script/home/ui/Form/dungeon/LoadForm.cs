using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadForm : BaseUIForm
{

    Slider load_slider;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;
        
        load_slider = UIFrameUtil.FindChildNode(this.transform, "Slider").GetComponent<Slider>();
       
        MessageMgr.AddMsgListener("HideLoadForm", p =>
        {
            CloseForm();
        });
    }

    private void OnEnable()
    {
        load_slider.value = 0;
    }

    void Update()
    {
        if (load_slider.value < 0.9f)
        {
            load_slider.value += Time.deltaTime;
        }
    }


}
