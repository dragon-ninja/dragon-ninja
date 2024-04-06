using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackPackSlot : BaseSlot
{
    //表示为角色装备栏
    public bool roleSlot;

    public EquipmentData eqData;
    public EquipmentAtr eqAtr;
    protected Image typeIcon;
    protected Image gradeImg;

    //紫12  金123要显示
    public TextMeshProUGUI gradeText;

    //等级
    public TextMeshProUGUI levelText;

    public List<string> colorList = new List<string>();

    protected override void Awake()
    {
        base.Awake();

        typeIcon = transform.Find("img").GetComponent<Image>();
        gradeText = UIFrameUtil.FindChildNode(this.transform, "grade/Text (TMP)").GetComponent<TextMeshProUGUI>();
        gradeImg = UIFrameUtil.FindChildNode(this.transform, "grade").GetComponent<Image>();
        levelText = UIFrameUtil.FindChildNode(this.transform, "level").GetComponent<TextMeshProUGUI>();
        this.GetComponent<Button>().onClick.AddListener(() => {
            if(eqData!=null)
                MessageMgr.SendMsg("ItemInfoPanelShow",
                    new MsgKV(roleSlot?"roleWear":"", eqData));
        });

        colorList.Add("#FFFFFF");
        colorList.Add("#91FF4C");
        colorList.Add("#5A97FF");
        //紫0,1,2
        colorList.Add("#C13FDD");
        colorList.Add("#C13FDD");
        colorList.Add("#C13FDD");
        //橙0,1,2,3
        colorList.Add("#FF9438");
        colorList.Add("#FF9438");
        colorList.Add("#FF9438");
        colorList.Add("#FF9438");
        //红
        colorList.Add("#FF1800");
    }

    public void Refresh(EquipmentData data, EquipmentAtr atr)
    {
        Show();
        gradeText.transform.parent.gameObject.SetActive(false);
        if (data == null) {
            eqData = null;
            eqAtr = null;
            icon.gameObject.SetActive(false);
            background.color = UIFrameUtil.getitemQualityColor("#FFFFFF");
            typeIcon.color = UIFrameUtil.getitemQualityColor("#FFFFFF");
            gradeImg.color = UIFrameUtil.getitemQualityColor("#FFFFFF");
            levelText.gameObject.SetActive(false);
            Show();
            return;
        }

        eqData = data;
        eqAtr = atr;

        levelText.text = "lv."+data.level;
        levelText.gameObject.SetActive(true);

        if (!roleSlot) { 
            if (eqAtr.itemType == "Weapon")
            {
                typeIcon.sprite = Resources.Load<Sprite>
                   ("ui/icon/item/type/Weapon");
            }
            else { 
                typeIcon.sprite = Resources.Load<Sprite>
                    ("ui/icon/item/type/"+ eqAtr.subType);
            }
        }

        icon.sprite = Resources.Load<Sprite>(eqAtr.icon);
        icon.gameObject.SetActive(true);


        Debug.Log("bkdata.quality="+ data.quality);

        background.color =  UIFrameUtil.getitemQualityColor(colorList[data.quality]);
        typeIcon.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);
        gradeImg.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);

        //紫12 金123品质的装备
        
        if (data.quality == 4 || data.quality == 5 ||
            data.quality == 7 || data.quality == 8 || data.quality == 9
            ) {

            if (data.quality > 6) {
                gradeText.text = data.quality - 6+"";
            }
            else {
                gradeText.text = data.quality - 3+"";
            }
            gradeText.transform.parent.gameObject.SetActive(true);
        }


      
    }

}
