using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Relic
{
    //public string id;
    public int level;
    public string configId;

    //用于参与排序
    public int quality;

    public Relic() { 
    }
}



[System.Serializable]
public class RelicConfig
{
    public string id;
    public string name;
    public string desc_1 = "对生命值高于90%的敌人造成伤害提高50%";
    public string desc_2;
    public string desc_3;
    public bool notUpFlag;
    //普通 稀有 传说
    public int quality = 0;
    // 武器 武装 角色
    public string type = "";
    public string effect_1 = "";
    public string effect_2 = "";
    public string effect_3 = "";
    public string icon;

    //用于测试
    public int testNum;
}


