using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePanelSlot : MonoBehaviour
{
    TextMeshProUGUI nameTxt;
    TextMeshProUGUI jdValue;
    Image jdimg;
    Image icon;
    bool initFlag;

    // Start is called before the first frame update：
    void Awake()
    {
        initFlag = true;
        icon = UIFrameUtil.FindChildNode(this.transform, "icon")
            .GetComponent<Image>();
        jdimg = UIFrameUtil.FindChildNode(this.transform, "jd/img")
          .GetComponent<Image>();
        nameTxt =  UIFrameUtil.FindChildNode(this.transform, "name")
            .GetComponent<TextMeshProUGUI>();
        jdValue = UIFrameUtil.FindChildNode(this.transform, "jd/value")
            .GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(string skillType, float value, float value2) {

        if (!initFlag)
            Awake();

        SkillAttr skillAttr = SkillAttrFactory.Get().skillMap[skillType][0];
        nameTxt.text = Mathf.RoundToInt(value2) + "";
        icon.sprite = Resources.Load<Sprite>
            ("skill/icon/" + skillAttr.icon);

        value = Mathf.Clamp(value, 0, 1);
        jdimg.fillAmount = value;
        jdValue.text = Mathf.RoundToInt(value * 100) +"%";
        gameObject.SetActive(true);
    }
}
