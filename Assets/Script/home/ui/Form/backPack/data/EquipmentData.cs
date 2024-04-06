using System;
using System.Collections;
using System.Collections.Generic;

public class NetData
{
    public string errorCode;
    public string message;
    public Object data;
}


public class RoleAttrData {
    public string _id;
    public List<string> talentList;
    public int exp;
    public strengthInfo strength;
    public int nowLevel;
    public int nowLevelExp;
    public int nextLevel;
    public int nextLevelNeedExp;
    public List<string> successChapter;
    public List<EquipmentData> weaponsBackPackItems;
}

public class strengthInfo
{
    public int strength;
    public int maxStrength;
    public string lastTime;
}



public class BackPackData
{
    public string _id;
    public List<EquipmentData> backPackItems;
    public List<EquipmentData> weaponsBackPackItems;
    public List<EquipmentData> activity;
}

public class EquipmentData
{
    public string seqId;
    public string id;
    public int level;
    public int quality;
    public int num;
    public int quantity;
    public bool wearing;


    public EquipmentData(){
    }

    public EquipmentData(string equipmentId, int level,int grade)
    {
        Guid guid = Guid.NewGuid();
        this.seqId = guid.ToString();
        this.id = equipmentId;
        this.level = level;
        this.quality = grade;
    }
}

public class NetEquipmentData
{
    public string id;
    public string name;
    public string seqId;
    public int level;
    public int quality;

}


public class itemAtr {
    public string id;
    public string name;
    public string desc;
    public string type;
}

[System.Serializable]
public class EquipmentAtr
{
    public string id;
    public string name;
    public string desc;
    public string itemType;
    public string subType;
    public string icon;
    public string mainAtr;
    public int mainAtrValue;
    //根据品质变化
    public string mainAtrValueStr;
    //根据品质变化
    public string mainAtrValueUp;

    public string atr1_id;
    public string atr2_id;
    public string atr3_id;
    public string atr4_id;
    public string atr5_id;
}

[System.Serializable]
public class EquipmentAffix
{
    public string id;
    public string desc_en;
    public string desc_cn;
    public string effect;
}


//强化相关配置
[System.Serializable]
public class EquipmentUpgrade
{
    public int id;
    public int goldNum;
    public int materialNum;
    public int quality;
    public int maxLevel;
    public int fuseNum;
    public bool fuseIdenticalFlag;



    public string itemType;
    public string material_id;
}
