using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoDescForm : BaseUIForm
{

    TextMeshProUGUI nameText;
    TextMeshProUGUI descText;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        nameText = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        descText = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        MessageMgr.AddMsgListener("ItemInfoDescShow", p =>
        {
            Refresh((ItemInfoDescData)p.Value);
        });

       /* GetComponent<Button>().onClick.AddListener(() =>
        {
            CloseForm();
        });*/
    }

    public override void Show()
    {
        base.Show();
        
    }


    public void Refresh(ItemInfoDescData data) {
        nameText.text = data.name;
        descText.text = data.desc;
        this.transform.position = data.t.position + new Vector3(0,40,0);

        LayoutRebuilder.ForceRebuildLayoutImmediate
         (descText.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
         (transform.GetComponent<RectTransform>());
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            CloseForm();
        }

        

    }

}