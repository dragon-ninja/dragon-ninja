using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailSlot: BaseSlot
{
    TextMeshProUGUI mailName;
    TextMeshProUGUI time;

    MailInfo nowData;

    protected override void Awake()
    {
        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        mailName = UIFrameUtil.FindChildNode(this.transform, "bt").GetComponent<TextMeshProUGUI>();
        time = UIFrameUtil.FindChildNode(this.transform, "time").GetComponent<TextMeshProUGUI>();

        this.GetComponent<Button>().onClick.AddListener(() => {
            MessageMgr.SendMsg("OpenMail", new MsgKV("", nowData));
        });
    }

    public void Refresh(MailInfo data)
    {
        Show();
        nowData = data;
        mailName.text = data.title;
        time.text = data.content;

        if(data.readStatus== "NO_READ")
            background.sprite = Resources.Load<Sprite>("ui/img/mail/未读");
        else
            background.sprite = Resources.Load<Sprite>("ui/img/mail/已读");
    }
}
