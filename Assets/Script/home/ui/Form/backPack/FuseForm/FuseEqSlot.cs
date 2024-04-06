using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FuseEqSlot : BaseSlot
{
    

    //表示为角色装备栏 0是背包栏   1目标槽  2目标进化槽  3消耗槽1 4消耗槽2 
    public int slotType;

    public EquipmentData eqData;
    public EquipmentAtr eqAtr;
    protected Image typeIcon;

    public List<string> colorList = new List<string>();

    GameObject mask;
    GameObject typeMask;
    GameObject equipFlag;
    GameObject confirm;
    GameObject canFlag;
    //紫12  金123要显示
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI levelText;
    protected Image gradeImg;

    protected override void Awake()
    {
        //base.Awake();

        background = GetComponent<Image>();
        if (transform.Find("icon") != null)
            icon = transform.Find("icon").GetComponent<Image>();
        myBut = GetComponent<Button>();

        init();
    }

    void init() {
        if (initFlag)
            return;

        initFlag = true;

        typeIcon = transform.Find("img").GetComponent<Image>();
        gradeText = UIFrameUtil.FindChildNode(this.transform, "grade/Text (TMP)").GetComponent<TextMeshProUGUI>();
        gradeImg = UIFrameUtil.FindChildNode(this.transform, "grade").GetComponent<Image>();
        if (slotType == 0)
        {
            levelText = UIFrameUtil.FindChildNode(this.transform, "level").GetComponent<TextMeshProUGUI>();
            mask = transform.Find("mask").gameObject;
            typeMask = transform.Find("img/mask").gameObject;
            equipFlag = transform.Find("equipFlag").gameObject;
            canFlag = transform.Find("canFlag").gameObject;
            confirm = transform.Find("confirm").gameObject;
        }
        if (slotType >= 3)
        {
            mask = transform.Find("mask").gameObject;
            typeMask = transform.Find("img/mask").gameObject;
        }


        myBut.onClick.AddListener(() => {
            if (eqData != null)
                tryHandle();
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

    /*flag = 0 常规
     * =1被选中
     * =2不可被选中
     * = 11 消耗槽用于提示需求装备类型
     * = 12 消耗槽显示需求同名装备
     */
    public void Refresh(EquipmentData data, EquipmentAtr atr, int flag = 0)
    {
        Show();

        if (data == null)
        {
            eqData = null;
            eqAtr = null;
            icon.gameObject.SetActive(false);
            background.color = UIFrameUtil.getitemQualityColor("#FFFFFF");
            typeIcon.color = UIFrameUtil.getitemQualityColor("#FFFFFF");
            Show();
            return;
        }

        eqData = data;
        eqAtr = atr;

        if (slotType == 0)
            levelText.text = "lv."+ data.level;


        if (eqAtr.itemType == "Weapon")
        {
            typeIcon.sprite = Resources.Load<Sprite>
                ("ui/icon/item/type/Weapon");
        }
        else
        {
            typeIcon.sprite = Resources.Load<Sprite>
                ("ui/icon/item/type/" + eqAtr.subType);
        }

        if (slotType == 0) {

            if (eqData.wearing)
                equipFlag.SetActive(true);
            else
                equipFlag.SetActive(false);
        }


        if (flag != 11) { 
            icon.sprite = Resources.Load<Sprite>(eqAtr.icon);
            icon.gameObject.SetActive(true);
        }

        Debug.Log("data.quality:" + data.quality + "   " + colorList.Count);

        background.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);
        typeIcon.color = UIFrameUtil.getitemQualityColor(colorList[data.quality]);
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
        showMask(flag);
       
    }



    public void showMask(int i) {
        if (slotType == 0) { 
            //无状态
            if (i == 0) { 
                mask.SetActive(false);
                typeMask.SetActive(false);
                confirm.SetActive(false);
                myBut.interactable = true;
            }
            //已选择
            if (i == 1) {
                mask.SetActive(true);
                typeMask.SetActive(true);
                confirm.SetActive(true);
                myBut.interactable = false;
            }
            //不可用于合成
            if (i == 2)
            {
                mask.SetActive(true);
                typeMask.SetActive(true);
                myBut.interactable = false;
            }
        }
        if (slotType >= 3) {
            //消耗槽显示黑色遮罩,不可交互
            if (i == 11)
            {
                icon.gameObject.SetActive(false);
                mask.SetActive(true);
                typeMask.SetActive(true);
                myBut.interactable = false;
            }
            //消耗槽显示黑色遮罩,不可交互  显示icon表示需要同名装备
            else if (i == 12) {
                mask.SetActive(true);
                typeMask.SetActive(true);
                myBut.interactable = false;
            }
            //消耗槽正常显示 并可交互
            else {
                mask.SetActive(false);
                typeMask.SetActive(false);
                myBut.interactable = true;
            }
        }
    }

    void tryHandle() {
        if (slotType == 0) {
            //选择合成目标
            if (!((FuseForm)mgr).tryFlag)
            {
                MessageMgr.SendMsg("fuseSelectTarget",
                       new MsgKV("", eqData));
            }
            //已有合成目标情况下 选择消耗目标  需要筛选是否符合消耗条件
            else {
                MessageMgr.SendMsg("fuseSelectConsume",
                          new MsgKV("", index));
            }
        }
        if (slotType == 1)
        {
            //卸下合成目标
            MessageMgr.SendMsg("fuseRemoveTarget",
                      new MsgKV("", eqData));
        }
        if (slotType == 2)
        {
            //查看目标装备属性
        }
        if (slotType == 3 || slotType == 4)
        {
            //卸下消耗目标
            MessageMgr.SendMsg("fuseRemoveConsume",
                     new MsgKV(slotType+"" , index));
        }
    }





}
