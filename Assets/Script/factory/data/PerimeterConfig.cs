using System.Collections;
using System.Collections.Generic;

//月卡
[System.Serializable]
public class MonthlyCardConfig
{
    public string id;
    public string name;
    public string desc;
    public ItemInfo buyReward;
    public ItemInfo dailyReward;
    public float price;
    public float patrolReward;
    public int patrolNum;
    public int strength;
}


//7日签到
[System.Serializable]
public class SevenDaySignConfig
{
    public string id;
    public List<ItemInfo> items;
}


//签到
[System.Serializable]
public class SignConfig
{
    public string id;
    public int day;
    public List<ItemInfo> items;

}


//挂机收益
[System.Serializable]
public class PatrolConfig
{
    public string id;
    //概率参数  基础值:章节增幅值
    public PatrolInfo gold;
    public PatrolInfo exp;
    public PatrolInfo item;
    public PatrolInfo material;
}

public class PatrolInfo {
    public float baseValue;
    public float up;
}



//充值奖励
[System.Serializable]
public class ChargeConfig
{
    public string id;
    //概率参数  基础值:章节增幅值
    public string desc;
    public List<ItemInfo> items;

    public float price;
    public float preferentialPrice;

}

//通行证奖励
[System.Serializable]
public class PassCheckConfig
{
    public string id;
    public int activity;
    public ItemInfo reward_0;
    public ItemInfo reward_1;
    public ItemInfo reward_2;
}

public class PassCheckData
{
    public string id;
    public bool draw1;
    public bool draw2;
    public bool draw3;
}

public class BattlePassRewardsConfig {
    public string id;
    public int lv;
    public int exp;
    public List<ItemInfo2> itemList;
}



//每日/周/月折扣商店
[System.Serializable]
public class GiftBagConfig
{
    public string id;
    public int day;
    public List<ItemInfo> itemList;
    public float price;
    public float preferentialPrice;
}




//体力
[System.Serializable]
public class StrengthShopConfig
{
    public string id;
    public string desc;
    public int num;
    public string type;
    public int max;
}


//等级解锁功能
public class LevelUnlockConfig
{
    public string id;
    public int level;
}