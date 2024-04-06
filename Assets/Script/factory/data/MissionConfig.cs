using System.Collections;
using System.Collections.Generic;

//任务
[System.Serializable]
public class MissionConfig
{
    public string id;
    public string type;
    public string desc;
    public string entry;
    //public string prevId;
    //public int level;
    //public List<MissionConditions> conditions;
    public ItemInfo rewards;


    public string unitType;
    public int num;
}

public class MissionConditions
{
    public string flag;
    public int value;
}

//任务进度
public class MissionData
{
    public string id;
    public int nowNum=0;
    public int maxNum=1;
}
