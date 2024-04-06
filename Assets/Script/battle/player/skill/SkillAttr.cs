using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillAttr 
{
    public string id = "";
    public string itemName = "";
    public string desc = "";
    public string icon;
    public string pfPath = "Sharp";
    public int level = 1;
    public string skillForm;
    public bool mainWeaponFlag;
    //是否为武装技能
    public bool armFlag;
    public bool superAttackFlag;
    public string triggerTyp = null;   // null   move  idle
    public string VTskill;

    public string skillType = "Kunai";
    public string ackType = "bullet";    //弹体  跟随  落点
    public string moveType = "straight";     //straight  rotate  不移动
    public string desType = "hitEnd;timeEnd";
    public string exTriggerType = "";
    public string exId = null;    //衍生物id
    public string breach = null;  //突破组合类型

    //释放延迟 匹配主武器动画
    public float delay = 0;
    public int attack = 0;
    public float dmgRate = 1;
    
    public float cd = 1.5f;
    //扩散角度  多弹片技能之间的角度间隔
    public int angle;
    //弹片  不考虑间隔 一次释放
    public int bulletNum;

    public int num = 1;

    public float startCheckTime = 0;
    public float damgaeCheckTime = 0;

    //持续时间
    public float duration = 3;

    public int dmgCount = 1;
    public float dmgInterval = 0;

    //一次有多发释放时的间隔  0就是没有间隔
    public float interval = 0;

    //射程范围  or 索敌距离  
    public float range = 50;

    //伤害范围   如燃烧瓶,导弹,闪电等具有aoe的技能
    public float dmgSize = 1;

    //飞行速度 rotate速度等
    public float speed = 0;
    //飞行时间,默认0则不设限
    public float flyDuration = 0;

    public float stiffTime;
    public float stiffForce;
    public string stiffType = "击退";
    //默认false为造成硬直覆盖
    public bool notStiffCover = false;

    public string testValue;
    //穿透等级
    public int pierceType;

    public Dictionary<string, object> exclusiveValue;
    public string exclusive;

    public bool notEnabled;

    //boss技能参数
    public string belong;
    public string indicatorType;
    public string animName;
    public float boxMaxX;
    public float boxMaxY;
    public float prepareTime_max;
    public float prepareEndDelay;
    //动态冷却 boss技能专属
    public float dynamicCd;



    public float getInterval() {
        return interval;
    }

    public float getDmgInterval()
    {
        return dmgInterval;
    }


    public int getDamage() {
        float bl = 1;
        //if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_dmgUp"))
        {
            try
            {
                bl =
                    1
                    + (RoleManager.Get().hpDropDmgUp
                    * (1 - DungeonManager.player.hp_now / DungeonManager.player.hp_max))
                    + (armFlag ?  0: RoleManager.Get().weaponDmg)
                    + (armFlag ?  RoleManager.Get().armDmg : 0 )
                    + (superAttackFlag ? 0 : RoleManager.Get().dlyDmg);

                if (DataManager.Get().userData.towerData != null) { 
                    bl += (RoleManager.Get().normalTowerNodeDmgUp * Math.Max(DataManager.Get().userData.towerData.normalNodeNum - 1, 0));
                    bl += (RoleManager.Get().kill_elite_dmgUp * DataManager.Get().userData.towerData.killJYNum);
                }
            }
            catch { 
            }

                //+ UpLevel.playerPassiveSkillLevelInfos["buff_dmgUp"].level * 0.1f;
        }
        dmgRate = dmgRate == 0 ? 1 : dmgRate;
        return (int)Mathf.Round(
            dmgRate * bl * RoleManager.Get().attack + attack );
    }

    public int getNum() {
        return  num==0?1:num;
    }

    public float getDmgSize() {

        float bl = 1;
        //if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_dmgSize"))
        {
            try { 
            bl = 1
                + (mainWeaponFlag ? RoleManager.Get().weaponSize : 0)
                + (mainWeaponFlag ? 0 : RoleManager.Get().armSize);
                //+ UpLevel.playerPassiveSkillLevelInfos["buff_dmgSize"].level * 0.1f;
            }
            catch
            {
            }
        }

        return (dmgSize == 0 ? 1 : dmgSize) * bl;
    }

    public float getCd() {
        float bl = 1;
        //if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_cdUp"))
        {
            bl = 1
                + (mainWeaponFlag ? RoleManager.Get().weaponCd : 0)
                + (mainWeaponFlag ? 0 : RoleManager.Get().armCd);
                //+ UpLevel.playerPassiveSkillLevelInfos["buff_cdUp"].level * 0.08f;
        }

        return cd / bl;
    }

    public float getDuration() {

        float bl = 1;
        if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_timeUp"))
        {
            bl = 1 + UpLevel.playerPassiveSkillLevelInfos["buff_timeUp"].level * 0.1f;
        }
        return (duration == 0 ? 1 : duration) * bl;
    }


    public float getSpeed() {
        float bl = 1;
        if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_bulletSpeed"))
        {
            bl = 1 + UpLevel.playerPassiveSkillLevelInfos["buff_bulletSpeed"].level * 0.1f;
        }
        return speed * bl;
    }

    public float getRange() {
        return range == 0 ? 20 : range;
    }

    public SkillAttr Clone()
    {
        return (SkillAttr)this.MemberwiseClone();
    }
}

