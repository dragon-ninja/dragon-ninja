using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnLockForm : BaseUIForm
{
    Image icon;
    TextMeshProUGUI desc;
    List<string> types;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        icon = UIFrameUtil.FindChildNode(this.transform, "icon").GetComponent<Image>();
        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();

        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
            if (types.Count > 0)
            {
                OpenForm("UnLockForm");
                Open();
            }
            else {
                MessageMgr.SendMsg("UnLockFormPopEnd", null);
            }
        });

        MessageMgr.AddMsgListener("LevelUnLcokShow", p =>
        {
            types = (List<string>)p.Value;
            Open() ;
        });
    }


    public void Open()
    {
        icon.sprite = Resources.Load<Sprite>("ui/icon/menu/" + types[0]);
        desc.text = types[0] + " Unlocked";
        types.RemoveAt(0);
    }


}
