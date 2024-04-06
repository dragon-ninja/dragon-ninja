using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : BaseSlot
{
    TextMeshProUGUI num;
    public Transform mask;

    string congfig_name;
    string congfig_desc;


    //紫12  金123要显示
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI levelText;
    protected Image gradeImg;

    public List<string> colorList = new List<string>();

    float fontSize;

    protected override void Awake()
    {
        base.Awake();

        if(UIFrameUtil.FindChildNode(this.transform, "grade/Text (TMP)")!=null)
            gradeText = UIFrameUtil.FindChildNode(this.transform, "grade/Text (TMP)").GetComponent<TextMeshProUGUI>();
        if (UIFrameUtil.FindChildNode(this.transform, "level") != null)
            levelText = UIFrameUtil.FindChildNode(this.transform, "level").GetComponent<TextMeshProUGUI>();
        if (UIFrameUtil.FindChildNode(this.transform, "grade") != null)
            gradeImg = UIFrameUtil.FindChildNode(this.transform, "grade").GetComponent<Image>();


        num = UIFrameUtil.FindChildNode(this.transform, "num").
            GetComponent<TextMeshProUGUI>();
        fontSize = num.fontSize;

        if (UIFrameUtil.FindChildNode(this.transform, "mask")!=null) {
            mask = UIFrameUtil.FindChildNode(this.transform, "mask");
        }

        if(GetComponent<Button>()!=null)
        GetComponent<Button>().onClick.AddListener(() =>
        {

            ItemInfoDescData data  = new ItemInfoDescData();
            data.t = this.transform;
            data.name = congfig_name;
            data.desc = congfig_desc;
            UIManager.GetUIMgr().closeUIForm("ItemInfoDescForm");
            UIManager.GetUIMgr().showUIForm("ItemInfoDescForm");
            MessageMgr.SendMsg("ItemInfoDescShow", new MsgKV("", data));
        });

        colorList.Add("#FFFFFF");
        colorList.Add("#91FF4C");
        colorList.Add("#5A97FF");
        //紫0,1,2
        colorList.Add("#C13FDD");
        //colorList.Add("#C13FDD");
        //colorList.Add("#C13FDD");
        //橙0,1,2,3
        colorList.Add("#FF9438");
        //colorList.Add("#FF9438");
        //colorList.Add("#FF9438");
        //colorList.Add("#FF9438");
        //红
        colorList.Add("#FF1800");
    }

    public void Refresh(ItemInfo info,bool changeBackgroundFlag = true)
    {
        Show();

        if (!initFlag)
            Awake();

        //根据id找到相应的物品图标
        string iconUrl = null;
        int quality = 0;
        if (ItemFactory.Get().itemMap.ContainsKey(info.id))
        {
            iconUrl = ItemFactory.Get().itemMap[info.id].icon;
            quality = ItemFactory.Get().itemMap[info.id].quality;
            /*  num.text = "x" + NumUtil.getNumk(info.num);
              num.gameObject.SetActive(true);*/

            congfig_name = ItemFactory.Get().itemMap[info.id].name;
            congfig_desc = ItemFactory.Get().itemMap[info.id].desc;
        }
        else { 
            iconUrl = EquipmentFactory.Get().map[info.id].icon;
            //显示品级
            //012  345=紫  6789=金  10=红
            if (info.grade >= 3 && info.grade <= 5)
                quality = 3;
            else if (info.grade >= 6 && info.grade <= 9)
                quality = 4;
            else if (info.grade == 10)
                quality = 5;
            else
                quality = info.grade;

            congfig_name = EquipmentFactory.Get().map[info.id].name;
            congfig_desc = EquipmentFactory.Get().map[info.id].desc;
        }
        if (info.num > 1)
        {
            num.text = "x" + NumUtil.getNumk(info.num);
            num.gameObject.SetActive(true);
        }
        else { 
            num.gameObject.SetActive(false);
        }
        num.fontSize = fontSize;
        icon.sprite = null;
        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);
        icon.gameObject.SetActive(true);

        if (changeBackgroundFlag) { 
            background.sprite = Resources.Load<Sprite>("ui/icon/item/dk/0" /*+ quality*/);
            background.color = UIFrameUtil.getitemQualityColor(colorList[quality]);
        }


        if (gradeImg != null) {
            gradeImg.color = UIFrameUtil.getitemQualityColor(colorList[quality]);
            //紫12 金123品质的装备
            gradeImg.gameObject.SetActive(false);
            if (info.grade == 4 || info.grade == 5 ||
                info.grade == 7 || info.grade == 8 || info.grade == 9
                )
            {

                if (info.grade > 6)
                {
                    gradeText.text = info.grade - 6 + "";
                }
                else
                {
                    gradeText.text = info.grade - 3 + "";
                }
                gradeImg.gameObject.SetActive(true);
            }
        }
    }


    public void RefreshForPatrol(ItemInfo info, bool changeBackgroundFlag = true)
    {
        Show();

        if (!initFlag)
            Awake();

        //根据id找到相应的物品图标
        string iconUrl = null;
        int quality = 0;
        if (ItemFactory.Get().itemMap.ContainsKey(info.id))
        {
            iconUrl = ItemFactory.Get().itemMap[info.id].icon;
            quality = ItemFactory.Get().itemMap[info.id].quality;
            /*  num.text = "x" + NumUtil.getNumk(info.num);
              num.gameObject.SetActive(true);*/

            congfig_name = ItemFactory.Get().itemMap[info.id].name;
            congfig_desc = ItemFactory.Get().itemMap[info.id].desc;
        }
        else
        {
            iconUrl = EquipmentFactory.Get().map[info.id].icon;
            //显示品级
            //012  345=紫  6789=金  10=红
            if (info.grade >= 3 && info.grade <= 5)
                quality = 3;
            else if (info.grade >= 6 && info.grade <= 9)
                quality = 4;
            else if (info.grade == 10)
                quality = 5;
            else
                quality = info.grade;

            congfig_name = EquipmentFactory.Get().map[info.id].name;
            congfig_desc = EquipmentFactory.Get().map[info.id].desc;
        }
        if (info.num > 1)
        {
            num.text = "x" + NumUtil.getNumk(info.num);
            num.gameObject.SetActive(true);
        }
        else
        {
            num.gameObject.SetActive(false);
        }

        //巡逻面板独有
        num.text = "?";
        num.gameObject.SetActive(true);
        num.fontSize = 50;
        icon.sprite = null;
        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);
        icon.gameObject.SetActive(true);

        if (changeBackgroundFlag)
        {
            background.sprite = Resources.Load<Sprite>("ui/icon/item/dk/0" /*+ quality*/);
            background.color = UIFrameUtil.getitemQualityColor(colorList[quality]);
        }


        if (gradeImg != null)
        {
            gradeImg.color = UIFrameUtil.getitemQualityColor(colorList[quality]);
            //紫12 金123品质的装备
            gradeImg.gameObject.SetActive(false);
            if (info.grade == 4 || info.grade == 5 ||
                info.grade == 7 || info.grade == 8 || info.grade == 9
                )
            {

                if (info.grade > 6)
                {
                    gradeText.text = info.grade - 6 + "";
                }
                else
                {
                    gradeText.text = info.grade - 3 + "";
                }
                gradeImg.gameObject.SetActive(true);
            }
        }
    }
}


public class ItemInfoDescData {
    public Transform t;
    public string name;
    public string desc;
}
