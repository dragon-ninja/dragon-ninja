using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;

public class BackPackForm : BaseUIForm
{

    TextMeshProUGUI attackText;
    TextMeshProUGUI hpText;
    ItemInfoPanel itemInfoPanel;
    Dictionary<string,BackPackSlot> RoleSlotMap = new Dictionary<string, BackPackSlot>();
    List<BackPackSlot> BackPackSlotList = new List<BackPackSlot>();
    Transform itemListNode;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.HideOther;
        ui_type.IsClearStack = false;

        /*GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm("SettingForm");
        });*/

        itemInfoPanel = UIFrameUtil.FindChildNode(this.transform, "ItemInfoPanel").GetComponent<ItemInfoPanel>();
  
        attackText = UIFrameUtil.FindChildNode(this.transform, "roleAtr/attack/value").GetComponent<TextMeshProUGUI>();
        hpText = UIFrameUtil.FindChildNode(this.transform, "roleAtr/hp/value").GetComponent<TextMeshProUGUI>();


        MessageMgr.AddMsgListener("ItemInfoPanelShow", p =>
        {
            itemInfoPanelShow((EquipmentData)p.Value, p.Key == "roleWear");
        });

        MessageMgr.AddMsgListener("wearEquipment", p =>
        {
            wearEquipment((EquipmentData)p.Value);
        });

        MessageMgr.AddMsgListener("removeEquipment", p =>
        {
            removeEquipment((EquipmentData)p.Value);
        });
        MessageMgr.AddMsgListener("upgradeEquipment", p =>
        {
            upgradeEquipment((EquipmentData)p.Value);
        });


        GetBut(this.transform, "FuseBut").onClick.AddListener(()=> {
            OpenForm("FuseForm");
            //CloseForm();
            CloseForm("down_menu");
        });


        //初始化roleitemSlot...
        RoleSlotMap.Add("Weapon", UIFrameUtil.FindChildNode(this.transform,
            "roleItemlist_1/Weapon").GetComponent<BackPackSlot>());
        RoleSlotMap.Add("Helmet", UIFrameUtil.FindChildNode(this.transform,
           "roleItemlist_1/Helmet").GetComponent<BackPackSlot>());
        RoleSlotMap.Add("Ring", UIFrameUtil.FindChildNode(this.transform,
           "roleItemlist_1/Ring").GetComponent<BackPackSlot>());
        RoleSlotMap.Add("Breastplate", UIFrameUtil.FindChildNode(this.transform,
           "roleItemlist_2/Breastplate").GetComponent<BackPackSlot>());
        RoleSlotMap.Add("Belt", UIFrameUtil.FindChildNode(this.transform,
           "roleItemlist_2/Belt").GetComponent<BackPackSlot>());
        RoleSlotMap.Add("Shoe", UIFrameUtil.FindChildNode(this.transform,
           "roleItemlist_2/Shoe").GetComponent<BackPackSlot>());
        foreach (var item in RoleSlotMap) {
            item.Value.roleSlot = true;
        }


        //初始化背包itemSlot...
        BackPackSlotList.Add(UIFrameUtil.FindChildNode(this.transform,
          "itemList").Find("item").GetComponent<BackPackSlot>());
        itemListNode = UIFrameUtil.FindChildNode(this.transform,
          "itemList");
    }

    public void RefreshRoleAtr()
    {
        RoleManager.Get().init();
        attackText.text = RoleManager.Get().attack+"";
        hpText.text = RoleManager.Get().hp + "";
    }




    public async Task Refresh(bool itemInfoPanelHide = true)
    {
        if(itemInfoPanelHide)
            itemInfoPanel.Hide();

        await DataManager.Get().refreshRoleAttributeStr();
        List<EquipmentData> roleEqList = DataManager.Get().roleAttrData.weaponsBackPackItems;
        //角色slot置空 
        foreach (var item in RoleSlotMap)
        {
            RoleSlotMap[item.Key].Refresh(null, null);
        }
        //填充角色装备
        for (int i = 0; i < roleEqList.Count; i++)
        {
            roleEqList[i].wearing = true;
            EquipmentAtr atr = EquipmentFactory.Get().map[roleEqList[i].id];
            if (atr.itemType == "Weapon")
            {
                RoleSlotMap["Weapon"].Refresh(roleEqList[i], atr);
            }
            else
            {
                RoleSlotMap[atr.subType].Refresh(roleEqList[i], atr);
            }
        }


        await DataManager.Get().refreshBackPack();
        List<EquipmentData> eqList = DataManager.Get().backPackData.weaponsBackPackItems;
        //根据种类 品质排序
        eqList.Sort((a, b) =>
        (b.id + b.quality).CompareTo(a.id + a.quality));
        int index = 0;
        int equipmentNum = 0;
        //填充背包装备
        for (int i=0; i< eqList.Count;i++) {
            for (int j = 0; j < eqList[i].num; j++) { 
                EquipmentAtr atr = EquipmentFactory.Get().map[eqList[i].id];
                //统计有多少件装备
                equipmentNum++;

                if (BackPackSlotList.Count <= index) {
                    GameObject slot = Instantiate
                        (BackPackSlotList[0].gameObject, itemListNode);
                    BackPackSlotList.Add(slot.GetComponent<BackPackSlot>());
                }

                Debug.Log("quality:"+eqList[i].quality+ "  "+ eqList[i].seqId);

                BackPackSlotList[index++].Refresh(eqList[i], atr);
            }
        }
   
        //多余的装备solt隐藏 
        for (int i = equipmentNum; i< BackPackSlotList.Count;i++) {
            BackPackSlotList[i].Hide();
        }

        RefreshRoleAtr();
    }

    public override void Show()  {
        base.Show();
        Refresh();
    }

    public void itemInfoPanelShow(EquipmentData eqData,bool wearflag) {
        EquipmentAtr atr = EquipmentFactory.Get().map[eqData.id];
        itemInfoPanel.backPackform = this;
        itemInfoPanel.Show(eqData , atr,true, wearflag);
    }

    public async void removeEquipment(EquipmentData eqData) {
        /*List<EquipmentData> eqList = DataManager.Get().userData.equipmentDataList;
        for (int i = 0; i < eqList.Count; i++) {
            if (eqList[i].seqId == eqData.seqId)
            {
                eqList[i].wearing = false;
            }
        }
        DataManager.Get().save();*/
       
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("user", DataManager.Get().loginData.data.user);
        string json = JsonConvert.SerializeObject(eqData);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/weapons/take", json, dic);

        if (str != null)
            Refresh();
    }

    public async void wearEquipment(EquipmentData eqData) {

        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("user", DataManager.Get().loginData.data.user);
        string json = JsonConvert.SerializeObject(eqData);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/weapons/wear", json, dic);

        if (str!=null)
            Refresh();

        /*EquipmentAtr atr = EquipmentFactory.Get().map[eqData.id];
        string type = atr.itemType == "Weapon"? "Weapon": atr.subType;
        
        string old_id = null;
        if (RoleSlotMap[type].eqData != null) 
            old_id = RoleSlotMap[type].eqData.seqId;

        List <EquipmentData> eqList = DataManager.Get().userData.equipmentDataList;
        for (int i = 0; i < eqList.Count; i++)
        {
            if (eqList[i].seqId == eqData.seqId)
            {
                eqList[i].wearing = true;
            }
            else if (old_id != null && eqList[i].seqId == old_id)
            {
                eqList[i].wearing = false;
            }
        }
        DataManager.Get().save();*/

        //改为和服务器交互的方式则为:
        //向服务器发送要更换的装备 然后这些逻辑在服务器中完成
        //服务器再反过来推送数据 这里再刷新全部装备状态 


    }

    public async void upgradeEquipment(EquipmentData eqData)
    {
        UpgradeFuceDataPush d = new UpgradeFuceDataPush();
        d.wear = eqData.wearing;
        d.mainFuse = eqData;
        string json = JsonConvert.SerializeObject(d);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/weapons/upgrades", json, DataManager.Get().getHeader());

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
       

        if (NetData.errorCode != null)
        {
            Debug.Log(NetData.message);
        }
        else { 
            UpgradeFuceDataGet result = JsonUtil.ReadData<UpgradeFuceDataGet>(str);
            await Refresh(false);
            EquipmentData newData = result.data;
            newData.wearing = eqData.wearing;
            EquipmentAtr atr = EquipmentFactory.Get().map[eqData.id];
            itemInfoPanel.Show(newData, atr);
                MessageMgr.SendMsg("UpMenuRefresh",
                            new MsgKV("", null));
        }
    }
}

//合成时向服务器发送的数据
public class UpgradeFuceDataPush {
    public bool wear;
    public EquipmentData mainFuse;
    public List<EquipmentData> deputyFuse;
}

//升级 合成后返回的数据
public class UpgradeFuceDataGet {
    public EquipmentData data;
    public List<ItemInfo> returned;
}


//public Stack 


/*fuse:{"errorCode":null,"message":null,"data":{"result":true,"msg":"合成成功",
 * "data":{"id":"wp_001","name":"Katana","quality":8,"level":1,"num":1,"seqId":"wp_001_1_8"},
 * "returned":[{"name":"Katana","id":"m10001","num":122},{"name":"Katana","id":"p10001","num":1891000}]}}*/