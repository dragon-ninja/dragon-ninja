using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//保存玩家对局数据
public class TowerGameData {

    public string nowChapter;
    public List<List<TowerMapNodeData>> nodeMap; 
    
    //当前玩家所处节点   但该节点不一定通过了  需要校验
    public string nowNode;
    public string selecNode;

    //通过的节点记录
    public string oldNodes;
    public int normalNodeNum;
    public int eliteNodeNum; 
    public int eventNodeNum;
    public int relicNodeNum;

    public int killNum;
    public int killJYNum;

    public float gameTime;

    //因为选择事件和宝箱节点累计的升级
    public int awaitUpgrade;

    //伤害记录数据
    public Dictionary<string, int> damageMap;

    //是否播放过解锁dly动效  
    public bool dlyDOTweenFlag;

    //玩家状态
    public float hpRate = 1;
    public float eyRate = 0;
    public int gold;
    public int level=1;
    public int exp;
    public int extraExp;
    //-------技能
    public Dictionary<string, int> skillInfo;

    //-------战利品
    public List<Relic> relicList = new List<Relic>();

    //-------事件
    public List<string> eventList = new List<string>();

    //------buff
    public List<string> buffList = new List<string>();

    //------宝物精华
    public int relicEssenceNum_1;
    public int relicEssenceNum_2;
    public int relicEssenceNum_3;
}


//节点内容
public class TowerMapNodeData
{
    //节点编号
    public string nodeStr;
    //节点坐标偏移
    public float x;
    public float y;
    //连接的下层节点编号
    public string nextNodeStrs;
    //类型  普通/精英/深渊/宝箱/事件/商店....
    public string type;
    //节点状态  0未解锁  1可挑战  2已通过;
    public bool state;
}


[System.Serializable]
//总览
public class TowerMap
{
    public string id;
    //防止策划填错 计算赋值
    public int maxStorey_clone = 10;
    public int maxNode = 4;
    public int minNode = 2;

    public string maxBoxNode = "3|5";
    public string maxEventNode = "3|5";
    public string maxAbyssNode = "0";
    public string maxEliteNode = "3|5";
    public string maxShopNode = "0";

    public string chapter;
    public string name;
    public string boundType;
    public string mapImg;

    public string fatherId;
    public int storey;
    //对应到具体每层使用的数据模版
    public string commonId;
    public string eliteId;
    public string abyssId;
    public float difficultyRatio;
    //宝箱给予的金币数量范围
    public string boxGold;
    public List<ItemInfo> rewards;

    //万分比
    public int hpUp = 10000;
    public int dmgUp = 10000;

    public List<KeyValue> commonEnd;
    public List<KeyValue> eliteEnd;


    public List<KeyValue> boxRewards;
    public List<KeyValue> eventRewards;
    //宝箱节点经验  事件节点经验
    //public int boxExp;
    //public int eventExp;

}

public class KeyValue {
    public string key;
    public int value;
} 



/*//节点内容
public class TowerMapNodeConfig
{
    public string id;
    public string type;
    public string name;
    public string icon;
}*/




