using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon 
{
  
}

[System.Serializable]
public class DungeonDesc
{
    public string id;
    public string name;
    public string desc;
    public string mapType;
    public string icon;

    //每次战斗的结算奖励
    public string battleAward;
    public Dictionary<string, int> battleAwardMap;



    public void init() {
        if (battleAward != null && battleAward.Length>0) { 
            battleAwardMap = new Dictionary<string, int>();
        
            string[] strs = battleAward.Split(";");

            foreach (string str in strs)
            {
                string[] ss = str.Split("|");
                battleAwardMap.Add(ss[0], int.Parse(ss[1]));
            }
        }
    }
}


[System.Serializable]
public class DungeonInfo
{
    public string id;
    public float time;
    public string type;
    public string desc;
    public string enemys;
    
    public float cd;
    public float num;
    public float hp_up = 10000;
    public float dmg_up = 10000;
    public float exp_up = 10000;
    public float speed_up = 10000;
}


public class DungeonInfoConfig
{
    public string id;
    public float time;
    public string type;
    public List<DungeonInfoData> datas;
}


public class DungeonInfoData
{
    public string id;
    public float cd;
    public float num;
    public float hp_up = 1;
    public float dmg_up = 1;
    public float exp_up = 1;
    public float speed_up = 1;
}