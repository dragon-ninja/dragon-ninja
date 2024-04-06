using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrowthFundLevelSlot : BaseSlot
{

    TextMeshProUGUI leveltext;
    Image img;

    protected override void Awake()
    {
        leveltext = UIFrameUtil.FindChildNode(this.transform, "level/Text (TMP)").GetComponent<TextMeshProUGUI>();
        img = UIFrameUtil.FindChildNode(this.transform, "s").GetComponent<Image>();

    }

    public void Refresh(int level,int playerLevel)
    {      
        Show();
        leveltext.text = level+"";
        
        if (playerLevel >= level) 
            img.color = new Color(1, 1, 1);
        else
            img.color = new Color(0, 0, 0);
    }
}
