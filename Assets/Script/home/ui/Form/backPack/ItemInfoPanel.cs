using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoPanel : BaseUIPanel
{
    public BackPackForm backPackform;

    public List<string> colorList = new List<string>();

    TextMeshProUGUI nameText;
    TextMeshProUGUI levelText;
    TextMeshProUGUI mainAtrText;
    TextMeshProUGUI goldText;
    TextMeshProUGUI materialText;
    Image dk;
    Image icon;
    Image materialImg;
    Transform atrListNode;
    //紫12  金123要显示
    public TextMeshProUGUI gradeText;
    protected Image gradeImg;

    List<TextMeshProUGUI> atrList = new List<TextMeshProUGUI>();
    //List<> atrColorList;
    List<Transform> atrlockList = new List<Transform>();

    Button equipBut;
    Button removeBut;
    Button upgradeBut;

    public EquipmentData nowShow_eqData;

    protected override void Awake()
    {
        initFlag = true;

        base.Awake();

        GetComponent<Button>().onClick.AddListener(() => {
            Hide();
        });

        UIFrameUtil.FindChildNode(this.transform, "close").GetComponent<Button>().onClick.AddListener(() => {
            Hide();
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

        equipBut = UIFrameUtil.FindChildNode(this.transform, "Equip").GetComponent<Button>();
        removeBut = UIFrameUtil.FindChildNode(this.transform, "Remove").GetComponent<Button>();
        upgradeBut = UIFrameUtil.FindChildNode(this.transform, "Upgrade").GetComponent<Button>();

        //新手引导
        MessageMgr.AddMsgListener("GuideB_Equip", p =>
        {
            MessageMgr.SendMsg("wearEquipment",
                   new MsgKV("", nowShow_eqData));
            Hide();
        });


        equipBut.onClick.AddListener(() => {
            MessageMgr.SendMsg("wearEquipment",
                     new MsgKV("", nowShow_eqData));
            Hide();
         });
        removeBut.onClick.AddListener(() => {
            MessageMgr.SendMsg("removeEquipment",
                    new MsgKV("", nowShow_eqData));
            Hide();
        });
        upgradeBut.onClick.AddListener(() => {

            upgradeBut.interactable = false;

            MessageMgr.SendMsg("upgradeEquipment",
                    new MsgKV("", nowShow_eqData));
        });


        nameText = UIFrameUtil.FindChildNode(this.transform, "name/Text (TMP)").GetComponent<TextMeshProUGUI>();
        levelText = UIFrameUtil.FindChildNode(this.transform, "level").GetComponent<TextMeshProUGUI>();

        goldText = UIFrameUtil.FindChildNode(this.transform, "gold/Text (TMP)").GetComponent<TextMeshProUGUI>();
        materialText = UIFrameUtil.FindChildNode(this.transform, "material/Text (TMP)").GetComponent<TextMeshProUGUI>();
        materialImg = UIFrameUtil.FindChildNode(this.transform, "material").GetComponent<Image>();

        mainAtrText = UIFrameUtil.FindChildNode(this.transform, "mainAtr").GetComponent<TextMeshProUGUI>();
        dk = UIFrameUtil.FindChildNode(this.transform, "img").GetComponent<Image>();
        icon = UIFrameUtil.FindChildNode(this.transform, "img/icon").GetComponent<Image>();
        gradeText = UIFrameUtil.FindChildNode(this.transform, "grade/Text (TMP)").GetComponent<TextMeshProUGUI>();
        gradeImg = UIFrameUtil.FindChildNode(this.transform, "grade").GetComponent<Image>();
        atrListNode = UIFrameUtil.FindChildNode(this.transform, "atrList");

        for (int i=1;i< atrListNode.childCount;i++) {
            Transform atrNode = atrListNode.GetChild(i);
            atrList.Add(atrNode.Find("Text (TMP)").GetComponent<TextMeshProUGUI>());
            atrlockList.Add(atrNode.Find("Text (TMP)/Image/lock"));
        }
    }

    public void Show(EquipmentData data, EquipmentAtr atr,bool backPackflag = true,bool wearflag = false) {
        base.Show();

        upgradeBut.interactable = true;

        //----------------------------------属性展示
        nowShow_eqData = data;
        nameText.text = atr.name;


        string[] mainAtrValues = atr.mainAtrValueStr.Split('|');
        int value = int.Parse(mainAtrValues[data.quality]);

        string[] mainAtrValueUps = atr.mainAtrValueUp.Split('|');
        int valueUp = int.Parse(mainAtrValueUps[data.quality]);

        //等级上限配置
        int maxLevel = EquipmentFactory.Get().upgradeMap[10000 + data.quality].maxLevel;
        levelText.text = "Level  " + data.level + "/" + maxLevel;

        mainAtrText.text = atr.mainAtr+ "  "+
            (value + (data.level - 1) * valueUp);
        dk.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);
        icon.sprite = Resources.Load<Sprite>(atr.icon);
        gradeImg.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);
        //紫12 金123品质的装备
        gradeText.transform.parent.gameObject.SetActive(false);
        if (data.quality == 4 || data.quality == 5 ||
            data.quality == 7 || data.quality == 8 || data.quality == 9
            )
        {
            if (data.quality > 6)
            {
                gradeText.text = data.quality - 6 + "";
            }
            else
            {
                gradeText.text = data.quality - 3 + "";
            }
            gradeText.transform.parent.gameObject.SetActive(true);
        }

        atrList[0].text = EquipmentFactory.Get().affixMap[atr.atr1_id].desc_en;//atr.atr_1_desc;
        atrList[1].text = EquipmentFactory.Get().affixMap[atr.atr2_id].desc_en;//atr.atr_2_desc;
        atrList[2].text = EquipmentFactory.Get().affixMap[atr.atr3_id].desc_en;//atr.atr_3_desc;
        atrList[3].text = EquipmentFactory.Get().affixMap[atr.atr4_id].desc_en;//atr.atr_4_desc;
        atrList[4].text = EquipmentFactory.Get().affixMap[atr.atr5_id].desc_en;//atr.atr_5_desc;

        //强制刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate
           (atrList[0].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
           (atrList[1].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
           (atrList[2].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
           (atrList[3].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
           (atrList[4].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
          (atrListNode.GetComponent<RectTransform>());

        for (int i=0;i<3;i++) {
            if (data.quality > i)
            {
                atrlockList[i].gameObject.SetActive(false);
            }
            else {
                atrlockList[i].gameObject.SetActive(true);
            }
        }

        if (data.quality >= 6)
        {
            atrlockList[3].gameObject.SetActive(false);
        }
        else {
            atrlockList[3].gameObject.SetActive(true);
        }
        if (data.quality >= 10) {
            atrlockList[4].gameObject.SetActive(false);
        }
        else
        {
            atrlockList[4].gameObject.SetActive(true);
        }



        //----------------------------------升级资源判定

        EquipmentData gd = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10001");
        int gold = 0;
        if (gd != null)
            gold = DataManager.Get().backPackData.backPackItems.Find(x => x.id == "p10001").quantity;
        //需要的材料id
        int ruleId = 0;
        if (atr.itemType == "Weapon")
            ruleId = 20000;
        else if (atr.subType == "Ring")
            ruleId = 20001;
        else if (atr.subType == "Helmet")
            ruleId = 20002;
        else if (atr.subType == "Belt")
            ruleId = 20003;
        else if (atr.subType == "Breastplate")
            ruleId = 20004;
        else if (atr.subType == "Shoe")
            ruleId = 20005;

        EquipmentUpgrade upd = EquipmentFactory.Get().upgradeMap[ruleId];
        string material_id = upd.material_id;

        int materialNum = 0;
        EquipmentData materialData = DataManager.Get().backPackData.backPackItems.Find(x => x.id == material_id);

        ItemConfig materialC = ItemFactory.Get().itemMap[material_id];
        if (materialData != null)
        {
            materialNum = materialData.quantity;
        }

        if (backPackflag)
        {
            //if (data.wearing)
            if (wearflag)
            {
                //显示脱下按钮
                equipBut.gameObject.SetActive(false);
                removeBut.gameObject.SetActive(true);
            }
            else
            {
                //显示装备按钮
                equipBut.gameObject.SetActive(true);
                removeBut.gameObject.SetActive(false);
            }
        }


         if (data.level == maxLevel)
        {
            upgradeBut.interactable = false;
            materialText.transform.parent.gameObject.SetActive(false);
            goldText.transform.parent.gameObject.SetActive(false);
            return;
        }
        else {
            materialText.transform.parent.gameObject.SetActive(true);
            goldText.transform.parent.gameObject.SetActive(true);
        }

        //当前强化需求金额材料
        EquipmentUpgrade upgrade = EquipmentFactory.Get().upgradeMap[data.level+1];
       
        goldText.text = NumUtil.getNumk(upgrade.goldNum) + "/" +
             NumUtil.getNumk(gold);

        materialImg.sprite = Resources.Load<Sprite>(materialC.icon);
        materialText.text = upgrade.materialNum + "/" + materialNum;

        if (backPackflag) {
            upgradeBut.interactable = true;

            //资源充足可以升级
            if (gold >= upgrade.goldNum )
            {
                goldText.color = Color.white;
            }
            else {
                goldText.color = Color.red;
                upgradeBut.interactable = false;
            }


            if (materialNum >= upgrade.materialNum){
                materialText.color = Color.white;
            }
            else{
                materialText.color = Color.red;
                upgradeBut.interactable = false;
            }

            if (data.level >= maxLevel) {
                upgradeBut.interactable = false;
            }
        }
    }
}





