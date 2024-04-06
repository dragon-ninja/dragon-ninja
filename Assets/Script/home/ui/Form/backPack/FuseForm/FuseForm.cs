using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class FuseForm : BaseUIForm
{
    //尝试合成中
    public bool tryFlag;
    //待合成的装备槽
    FuseEqSlot target_slot;
    //合成后的装备槽
    FuseEqSlot target_slot_up;
    //合成消耗的装备槽
    FuseEqSlot consume_slot1;
    FuseEqSlot consume_slot2;
    Transform itemListNode;

    TextMeshProUGUI infoName;
    TextMeshProUGUI info1;
    TextMeshProUGUI info2;
    TextMeshProUGUI info3;

    List<FuseEqSlot> FuseEqSlotList = new List<FuseEqSlot>();

    //需要消耗的装备数量/已填充了装备数量
    int needConsumeNum = 0;
    int haveConsumeNum = 0;
    Button fuseBut;

    ItemInfoPanel itemInfoPanel;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "returnBut").onClick.AddListener(() => {
            OpenForm("BackPackForm");
            OpenForm("down_menu");
            CloseForm();
        });

        fuseBut = GetBut(this.transform, "fuseBut");
        fuseBut.onClick.AddListener(() => {
            tryFuse();
        });
        
        itemInfoPanel = UIFrameUtil.FindChildNode(this.transform, "ItemInfoPanel").GetComponent<ItemInfoPanel>();

        infoName = UIFrameUtil.FindChildNode(this.transform,
           "info/name").GetComponent<TextMeshProUGUI>();
        info1 = UIFrameUtil.FindChildNode(this.transform,
          "info/info1").GetComponent<TextMeshProUGUI>();
        info2 = UIFrameUtil.FindChildNode(this.transform,
          "info/info2").GetComponent<TextMeshProUGUI>();
        info3 = UIFrameUtil.FindChildNode(this.transform,
          "info/info3").GetComponent<TextMeshProUGUI>();

        target_slot = UIFrameUtil.FindChildNode(this.transform,
           "targetEq/item_1").GetComponent<FuseEqSlot>();
        target_slot_up = UIFrameUtil.FindChildNode(this.transform,
          "targetEq/item_0").GetComponent<FuseEqSlot>();
        consume_slot1 = UIFrameUtil.FindChildNode(this.transform,
          "consumeEq/consume_slot1").GetComponent<FuseEqSlot>();
        consume_slot2 = UIFrameUtil.FindChildNode(this.transform,
         "consumeEq/consume_slot2").GetComponent<FuseEqSlot>();
        target_slot.slotType = 1;
        target_slot_up.slotType = 2;
        consume_slot1.slotType = 3;
        consume_slot2.slotType = 4;
        target_slot.mgr = this;
        target_slot_up.mgr = this;
        consume_slot1.mgr = this;
        consume_slot2.mgr = this;

        //初始化背包itemSlot...
        FuseEqSlotList.Add(UIFrameUtil.FindChildNode(this.transform,
          "itemList").Find("item").GetComponent<FuseEqSlot>());
        FuseEqSlotList[0].mgr = this;
        FuseEqSlotList[0].index = 0;
        itemListNode = UIFrameUtil.FindChildNode(this.transform,
          "itemList");


        MessageMgr.AddMsgListener("fuseSelectTarget", p =>
        {
            selectTarget((EquipmentData)p.Value);
        });

        MessageMgr.AddMsgListener("fuseRemoveTarget", p =>
        {
            Refresh();
        });

        MessageMgr.AddMsgListener("fuseSelectConsume", p =>
        {
            selectConsume((int)p.Value);
        });

        MessageMgr.AddMsgListener("fuseRemoveConsume", p =>
        {
            removeConsume(int.Parse(p.Key));
        });

        //initRole();
    }


    public async Task<bool> initData()
    {
        await DataManager.Get().refreshBackPack();
        await DataManager.Get().refreshRoleAttributeStr();
        return true;
    }

    public async void Refresh()
    {
        tryFlag = false;
        DataManager.Get().init();

        target_slot.Refresh(null, null);
        //合成后的装备槽
        target_slot_up.Refresh(null, null);
        //合成消耗的装备槽
        consume_slot1.Refresh(null, null);
        consume_slot2.Refresh(null, null);


        infoName.text = "";
        info1.text = "";
        info2.text = "";
        info3.text = "";
        target_slot.gameObject.SetActive(false);
        target_slot_up.gameObject.SetActive(false);
        consume_slot1.gameObject.SetActive(false);
        consume_slot2.gameObject.SetActive(false);
        haveConsumeNum = 0;
        consume_slot1_EqIndex = -1;
        consume_slot2_EqIndex = -1;
        fuseBut.gameObject.SetActive(false);

        //await initRole();

        List<EquipmentData> eqList = new List<EquipmentData>();
        foreach (EquipmentData info in DataManager.Get().backPackData.weaponsBackPackItems)
        {
            info.wearing = false;
            eqList.Add(info);
        }
        foreach (EquipmentData info in DataManager.Get().roleAttrData.weaponsBackPackItems)
        {
            info.wearing = true;
            eqList.Add(info);
        }

        //装备排序....
        eqList.Sort((a, b) => 
         (b.wearing+ b.id + b.quality).CompareTo
         (a.wearing + a.id + a.quality));


        int index = 0;
        int equipmentNum = 0;
        for (int i = 0; i < eqList.Count; i++)
        {
            int num = 0;
            if (eqList[i].wearing)
                num = 1;
            else
                num = eqList[i].num;

            for (int j = 0; j < num; j++)
            {
                if (eqList[i].quality < 10) { 
                    //填充角色装备
                    EquipmentAtr atr = EquipmentFactory.Get().map[eqList[i].id];
                    //统计有多少件装备
                    equipmentNum++;
                    if (FuseEqSlotList.Count <= index)
                    {
                        GameObject slot = Instantiate
                            (FuseEqSlotList[0].gameObject, itemListNode);
                        FuseEqSlot eqSlot = slot.GetComponent<FuseEqSlot>();
                        FuseEqSlotList.Add(eqSlot);
                        eqSlot.mgr = this;
                        eqSlot.index = FuseEqSlotList.Count-1;
                    }
                    FuseEqSlotList[index++].Refresh(eqList[i], atr);
                }
            }
        }

        //多余的装备solt隐藏 
        for (int i = equipmentNum; i < FuseEqSlotList.Count; i++)
        {
            FuseEqSlotList[i].Hide();
        }
    }

    public override void Show()
    {
        base.Show();
        Refresh();
    }


    //选择要合成的目标装备
    public async void selectTarget(EquipmentData eqData) {

        //await DataManager.Get().refreshBackPack();
        //await DataManager.Get().refreshRoleAttributeStr();

        tryFlag = true;

        //改变target_slot
        EquipmentAtr atr = EquipmentFactory.Get().map[eqData.id];
        target_slot.Refresh(eqData, atr);

        EquipmentData new_eqData = new EquipmentData
            (eqData.id, eqData.level, eqData.quality+1);
        target_slot_up.Refresh(new_eqData, atr);

        //改变背包栏
        //筛选出可用于合成的装备
        //将目标装备变暗打钩
        //将其他不可以参与合成的装备变暗
        //穿戴中的装备可以参与合成  但不能作为材料
        //List<EquipmentData> eqList = DataManager.Get().userData.equipmentDataList;


        List<EquipmentData> eqList = new List<EquipmentData>();
        foreach (EquipmentData info in DataManager.Get().backPackData.weaponsBackPackItems)
        {
            info.wearing = false;
            eqList.Add(info);
        }
        foreach (EquipmentData info in DataManager.Get().roleAttrData.weaponsBackPackItems)
        {
            info.wearing = true;
            eqList.Add(info);
        }


        List<EquipmentData> eqList1 = new List<EquipmentData>();
        List<EquipmentData> eqList2 = new List<EquipmentData>();
        //合成规则
        EquipmentUpgrade upgrade = 
            EquipmentFactory.Get().upgradeMap[10000+eqData.quality];
        needConsumeNum = upgrade.fuseNum;

        bool meflag = false;
         
        //筛选出可以用于合成和不能用于合成的装备
        for (int i = 0; i < eqList.Count; i++)
        {
            int num = 0;
            if (eqList[i].wearing)
                num = 1;
            else
                num = eqList[i].num;

            for (int j = 0; j < num; j++) { 
                if (eqList[i].quality < 10) { 
                    
                    if (!meflag && eqData.seqId == eqList[i].seqId &&((eqData.wearing && eqList[i].wearing) || (!eqData.wearing && !eqList[i].wearing)))
                    {
                        meflag = true;
                    }
                    else if (!eqData.wearing && eqList[i].wearing) {
                        eqList2.Add(eqList[i]);
                    }
                    else {
                        EquipmentAtr atr1 = EquipmentFactory.Get().map[eqList[i].id];
                        //需要同名装备
                        if (upgrade.fuseIdenticalFlag)
                        {
                            if (eqData.id == eqList[i].id
                                && eqData.quality == eqList[i].quality)
                            {
                                //同名装备
                                eqList1.Add(eqList[i]);
                            }
                            else {
                                //不可用于合成的装备
                                eqList2.Add(eqList[i]);
                            }
                        }
                        else {
                            //同部位装备
                            if (((atr.itemType == "weapon" 
                                && atr1.itemType == "weapon")
                                || atr.subType == atr1.subType)
                                && eqData.quality == eqList[i].quality
                                ) {
                                eqList1.Add(eqList[i]);
                            }
                            else{
                                //不可用于合成的装备
                                eqList2.Add(eqList[i]);
                            }
                        }
                    }
                }
            }
        }
        int equipmentNum = 0;
        for (int i = 0; i < eqList1.Count; i++)
        {
            FuseEqSlotList[i].Refresh
                (eqList1[i], EquipmentFactory.Get().map[eqList1[i].id]);
            equipmentNum++;
        }

        FuseEqSlotList[equipmentNum++].Refresh
                (eqData, EquipmentFactory.Get().map[eqData.id],1);


        Debug.Log(eqList.Count+ "----:" +(eqList2.Count + eqList1.Count) + "----:" + (eqList2.Count));

        for (int i = 0; i < eqList2.Count; i++)
        {
            //Debug.Log()
            FuseEqSlotList[equipmentNum++].Refresh
                (eqList2[i], EquipmentFactory.Get().map[eqList2[i].id],2);
        }


        //多余的装备solt隐藏 
        for (int i = eqList1.Count + eqList2.Count+1; i < FuseEqSlotList.Count; i++)
        {
            FuseEqSlotList[i].Hide();
        }

        //------------------------
        //显示装备合成后的属性


        infoName.text = atr.name;

        EquipmentUpgrade newUpgrade =
          EquipmentFactory.Get().upgradeMap[10000 + eqData.quality+1];
        int nowMaxLevel = upgrade.maxLevel;
        int newMaxLevel = newUpgrade.maxLevel;

        string[] mainAtrValues = atr.mainAtrValueStr.Split('|');
        int nowAtrValue = int.Parse(mainAtrValues[eqData.quality]);
        int newAtrValue = int.Parse(mainAtrValues[eqData.quality+1]);


        info1.text = "Max Level            " +
            nowMaxLevel+"  <color=#11FF00>>  " +
            newMaxLevel+"</color>";
        info2.text = "Attack            " +
            nowAtrValue+"  <color=#11FF00>>  " +
            newAtrValue+"</color>";

        if (eqData.quality + 1 == 1)
        {
            info3.text = EquipmentFactory.Get().affixMap[atr.atr1_id].desc_en;
        }
        else if (eqData.quality + 1 == 2)
        {
            info3.text = EquipmentFactory.Get().affixMap[atr.atr2_id].desc_en;
        }
        else if (eqData.quality + 1 == 3)
        {
            info3.text = EquipmentFactory.Get().affixMap[atr.atr3_id].desc_en;
        }
        else if (eqData.quality + 1 == 6)
        {
            info3.text = EquipmentFactory.Get().affixMap[atr.atr4_id].desc_en;
        }
        else if (eqData.quality + 1 == 10)
        {
            info3.text = EquipmentFactory.Get().affixMap[atr.atr5_id].desc_en;
        }
        else { 
            info3.text = "";
        }

        //显示需要几个装备用于合成
        //显示是否需要同名装备  
        if (upgrade.fuseNum == 1)
        {
            consume_slot1.gameObject.SetActive(true);
            consume_slot2.gameObject.SetActive(false);
            consume_slot1.Refresh(eqData,
                EquipmentFactory.Get().map[eqData.id], 
                upgrade.fuseIdenticalFlag ? 12:11);
        }
        else {
            consume_slot1.gameObject.SetActive(true);
            consume_slot2.gameObject.SetActive(true);
            consume_slot1.Refresh(eqData,
                EquipmentFactory.Get().map[eqData.id], 
                upgrade.fuseIdenticalFlag ? 12 : 11);
            consume_slot2.Refresh(eqData,
                EquipmentFactory.Get().map[eqData.id], 
                upgrade.fuseIdenticalFlag ? 12 : 11);
        }

        target_slot.gameObject.SetActive(true);
        target_slot_up.gameObject.SetActive(true);

    }

    //选择消耗装备
    public void selectConsume(int index) {
        if (haveConsumeNum < needConsumeNum) { 
            haveConsumeNum++;

            EquipmentData eqData = FuseEqSlotList[index].eqData;

            if (consume_slot1_EqIndex == -1)
            {
                consume_slot1_EqIndex = index;
                consume_slot1.Refresh(eqData,
                    EquipmentFactory.Get().map[eqData.id]);
            }
            else if(consume_slot2_EqIndex == -1) {
                consume_slot2_EqIndex = index;
                consume_slot2.Refresh(eqData,
                   EquipmentFactory.Get().map[eqData.id]);
            }

            FuseEqSlotList[index].Refresh(eqData, EquipmentFactory.Get().map[eqData.id], 1);

            if(haveConsumeNum == needConsumeNum)
                fuseBut.gameObject.SetActive(true);
        }
    }

    //卸除消耗装备
    int consume_slot1_EqIndex;
    int consume_slot2_EqIndex;
    public void removeConsume(int type) {

        EquipmentData eqData;
        haveConsumeNum--;

        fuseBut.gameObject.SetActive(false);

        if (type == 3)
        {
            eqData = FuseEqSlotList[consume_slot1_EqIndex].eqData;
            FuseEqSlotList[consume_slot1_EqIndex].
               Refresh(eqData, EquipmentFactory.Get().map[eqData.id]);
            consume_slot1_EqIndex = -1;

            EquipmentUpgrade upgrade =
                EquipmentFactory.Get().upgradeMap[10000 + eqData.quality];
            consume_slot1.showMask(upgrade.fuseIdenticalFlag ? 12 : 11);
        }
        else {
            eqData = FuseEqSlotList[consume_slot2_EqIndex].eqData;
            FuseEqSlotList[consume_slot2_EqIndex].
                Refresh(eqData, EquipmentFactory.Get().map[eqData.id]);
            consume_slot2_EqIndex = -1;

            EquipmentUpgrade upgrade =
                EquipmentFactory.Get().upgradeMap[10000 + eqData.quality];
            consume_slot2.showMask(upgrade.fuseIdenticalFlag ? 12 : 11);
        }

    }

    async void tryFuse() {

     

        //EquipmentData eq =  DataManager.Get().userData.equipmentDataList.
        //    Find(item => item.seqId == target_slot.eqData.seqId);

        //target_slot.eqData.seqId

        EquipmentUpgrade upgrade = EquipmentFactory.Get().upgradeMap[10000+ target_slot.eqData.quality];

        if (upgrade.fuseNum == 1 ) {
            if (consume_slot1_EqIndex == -1)
                return;
        }
        if (upgrade.fuseNum == 2)
        {
            if (consume_slot1_EqIndex == -1 || consume_slot2_EqIndex == -1)
                return;
        }

        /*if (consume_slot1_EqIndex != -1)
            DataManager.Get().userData.equipmentDataList.Remove(
            DataManager.Get().userData.equipmentDataList.
            Find(item => item.seqId == consume_slot1.eqData.seqId)
            );
        if (consume_slot2_EqIndex != -1)
            DataManager.Get().userData.equipmentDataList.Remove(
            DataManager.Get().userData.equipmentDataList.
            Find(item => item.seqId == consume_slot2.eqData.seqId)
            );
        eq.quality += 1;
        DataManager.Get().save();

        itemInfoPanel.Show(eq,
            EquipmentFactory.Get().map[eq.id],false);
        */

        Debug.Log("target_slot.eqData.wearing:"+ target_slot.eqData.wearing);

        UpgradeFuceDataPush data = new UpgradeFuceDataPush();
        data.wear = target_slot.eqData.wearing;
        data.mainFuse = target_slot.eqData;
        data.deputyFuse = new List<EquipmentData>();

        if (consume_slot1_EqIndex != -1) 
            data.deputyFuse.Add(consume_slot1.eqData);
        
        if (consume_slot2_EqIndex != -1) 
            data.deputyFuse.Add(consume_slot2.eqData);
        
        string json = JsonConvert.SerializeObject(data);


        //Debug.Log(json);

        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/weapons/fuse", json, DataManager.Get().getHeader());
        Debug.Log("fuse:"+ str);

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode != null)
        {
            Debug.Log(NetData.message);
            MessageMgr.SendMsg("ErrorDesc",
                         new MsgKV("", NetData.message));
        }
        else {
            UpgradeFuceDataGet fuseReturnData = JsonUtil.ReadData<UpgradeFuceDataGet>(str);

            ItemInfo iteminfo = new ItemInfo(fuseReturnData.data.id, fuseReturnData.data.num, fuseReturnData.data.quality, fuseReturnData.data.level);

            fuseReturnData.returned.Insert(0,iteminfo);

            //显示材料

            UIManager.GetUIMgr().showUIForm("FuseDescForm");
            MessageMgr.SendMsg("FuseDesc",
                         new MsgKV("", fuseReturnData.returned));
            await initData();
            Refresh();
        }

    }
}

