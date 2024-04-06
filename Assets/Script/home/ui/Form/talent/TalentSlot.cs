using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentSlot : BaseSlot
{

    //槽位连线图片
    protected Image line;
    public Talent talent;
    bool line_h_flag;

    //最后一个节点隐藏自己的线
    public bool endFlag;

    protected override void Awake()
    {
        initFlag = true;
        background = transform.Find("img").GetComponent<Image>();
        icon = transform.Find("icon").GetComponent<Image>();
        line = transform.Find("line").GetComponent<Image>();

        line_h_flag = line.sprite.name.IndexOf("_横") != -1;

        this.GetComponent<Button>().onClick.AddListener(() => {
            MessageMgr.SendMsg("TalentInfoShow",
                    new MsgKV(talent.talentType, index));
        });
    }

    public void Refresh(bool unlock)
    {
        if (!initFlag)
            Awake();

        if (endFlag)
            line.gameObject.SetActive(false);
        else
            line.gameObject.SetActive(true);

        if (unlock)
        {
            if(line_h_flag)
                line.sprite = Resources.Load<Sprite>("ui/img/talent/line_横");
            else
                line.sprite = Resources.Load<Sprite>("ui/img/talent/line");

            //background.color = UIFrameUtil.getitemQualityColor("#D7FFAE");

            background.sprite = Resources.Load<Sprite>(talent.bgicon);
            icon.sprite =  Resources.Load<Sprite>(talent.icon);
        }
        else
        {
            if (line_h_flag)
                line.sprite = Resources.Load<Sprite>("ui/img/talent/line_h_横");
            else
                line.sprite = Resources.Load<Sprite>("ui/img/talent/line_h");

            //background.color = UIFrameUtil.getitemQualityColor("#6F6F6F");

            background.sprite = Resources.Load<Sprite>("ui/icon/talent/底框/灰色");
            icon.sprite = Resources.Load<Sprite>(talent.icon+"_未解锁");
        }
        icon.gameObject.SetActive(true);
    }
}
