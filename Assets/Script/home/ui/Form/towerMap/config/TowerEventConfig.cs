using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TowerEventConfig 
{
    public string id;
    public string name;
    public string desc;
    //选择 显示
    public string select = "给予|抢夺";
    //选择结果 显示
    public string result = "";
    //选择结果 实际效果
    public string effect = "gold:-200;宝物:1|宝物:1;buff:羞耻" ;
}

public class TowerEventBuffConfig
{
    public string id;
    public string desc;
    public string effect = "gold:-200;宝物:1|宝物:1;buff:羞耻";
    public string icon;
}



[System.Serializable]
public class TowerEvent {
    public TowerEventConfig config;

    public List<string> selectList = new List<string>();
    public List<string> resultList = new List<string>();
    public List<string> effectList = new List<string>();


    public void init() {
        if (config.select.IndexOf("|") != -1)
        {
            string[] selectStrs = config.select.Split("|");
            foreach (string s in selectStrs)
            {
                
                selectList.Add(s.Trim());
            }
        }
        else {
            selectList.Add(config.select);
        }


        if (config.result.IndexOf("|") != -1)
        {
            string[] resultStrs = config.result.Split("|");
            foreach (string s in resultStrs)
            {
                resultList.Add(s.Trim());
            }
        }
        else
        {
            resultList.Add(config.result);
        }


        if (config.effect.IndexOf("|") != -1)
        {
            string[] effectStrs = config.effect.Split("|");
            foreach (string s in effectStrs)
            {
                effectList.Add(s.Trim());
            }
        }
        else
        {
            effectList.Add(config.effect);
        }

    }
}




/*
重生十字: 获得一次复活的机会
狙击手的头巾:对生命值高于90%的敌人造成伤害提高50%
死亡笔记:每次击杀一个单位都会提高0.01%伤害,至多60%
 */

/*
湖水: 饮水(生命值+20%)  沐浴(防御力+10%)
流浪汉:  钱换遗物     唾弃buff(经验值-20%)+遗物

木乃伊  :  血换遗物    什么都不做   

1.你遇到了一个游吟诗人,她欢快的歌声让你接下来技能冷却+20%			
3.你遇到的一口圣泉,喝下泉水后你的所有生命都恢复了,并且最大生命值+30%			



 */


