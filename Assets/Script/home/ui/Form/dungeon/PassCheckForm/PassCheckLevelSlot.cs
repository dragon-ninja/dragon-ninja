using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassCheckLevelSlot : BaseSlot
{

    TextMeshProUGUI leveltext;
    Image levelImg;

    protected override void Awake()
    {
        leveltext = UIFrameUtil.FindChildNode(this.transform, "level/Text (TMP)").GetComponent<TextMeshProUGUI>();
        levelImg = UIFrameUtil.FindChildNode(this.transform, "level")
            .GetComponent<Image>();

    }

    public void Refresh(int level,int playerLevel)
    {      
        Show();
        leveltext.text = level+"";

        if(level <= playerLevel)
            levelImg.sprite = Resources.Load<Sprite>("ui/img/PassCheck/Í·Ïñ¿ò");
        else
            levelImg.sprite = Resources.Load<Sprite>("ui/img/PassCheck/Í·Ïñ¿ò_h");
    }
}
