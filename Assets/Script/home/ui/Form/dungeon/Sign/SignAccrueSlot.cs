using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignAccrueSlot : BaseSlot
{
    TextMeshProUGUI desc;
    string configId;

    //该槽位只显示天数信息 不用于交互
    bool dayShowFlag;

    protected override void Awake()
    {
        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        desc = UIFrameUtil.FindChildNode(this.transform, "Text (TMP)").GetComponent<TextMeshProUGUI>();

        this.GetComponent<Button>().onClick.AddListener(() => {
            if (!dayShowFlag) {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<SignConfig> configList = PerimeterFactory.Get().SignList;
                SignConfig config = configList.Find(item => item.id == configId);
                MessageMgr.SendMsg("GetReward", new MsgKV("", config.items));
                MessageMgr.SendMsg("RefreshTip", null);
            } 
        });
    }


    ///id 是否解鎖
    public void Refresh(string id, bool unlock = false)
    {
        if (!initFlag)
            Awake();

        this.configId = id;

        if (id == "dayShowFlag")
        {
            dayShowFlag = true;
            myBut.interactable = false;
            desc.text = 1 + "\r\nday";
        }
        else { 
            myBut.interactable = unlock;
            List<SignConfig> configList = PerimeterFactory.Get().SignList;
            SignConfig config = configList.Find(item => item.id == configId);
            desc.text = config.day + "";
        }
    }
}
