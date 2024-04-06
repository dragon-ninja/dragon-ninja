using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoleManager 
{
    static RoleManager mgr;

    //基础属性
    public int attack;
    public int hp;
    public int defense;
    public int recovery;
    public int getGold;
    public int speed;


    //--------------特殊参数
    public int superNum; //可用于超武进化的道具数量
    public float attackUp;
    public float hpUp;
  

    //武士刀参数
    public int katanaNumUp;
    public int KatanaRepel;
    public float katanaVampire;



    //x范围内敌人速度降低  entropicAura_RadiusAndDown
    public float entropicAura_Radius;//:10/0.05
    public float entropicAura_Down;
    //击杀精英/boss获得护盾
    public float killElite_Shield;
    //击杀x数量敌人获得速度提升 killEySpeedUpNum
    public int killEySpeedUpNum;
    public float killEySpeedUp;
    //通过普通关卡后增加伤害
    public float normalTowerNodeDmgUp;

    //------------new 

    //食物回复效果提升5%	
    public float Food_RecoveryUp;
    //基本移动速度+1	
    public float Basic_Movement_Speed;

    //击杀精英或者boss，获得10%增伤，受击移除
    public float killElite_Damage_Increased;

    //累计击杀300怪物，获得15%移速加成，受击移除
    public int killEyMoveSpeedUpNum;
    public float killEyMoveSpeedUp;

    //每完成一次事件，伤害增加10%	
    public float event_Damage_Increased;

    //3%概率秒杀小怪
    public float Probability_of_killing;

    //死亡免费复活1次，伤害增加10%
    //复活次数
    public int reviveNum;
    //复活后攻击力加成
    public float revive_attackUp;
    //复活后速度加成
    public float revive_speedUp;

    //击杀精英或者boss，50%概率额外掉落1张铭牌
    public float killElite_getNameplateProbability;

    //累计击杀300怪物，恢复5%生命上限
    public float killEy_Hp_restoreNum;
    public float killEy_Hp_restore;
    //每完成一次精英关卡，伤害增加10%	
    public float EliteEnd_Damage_Increased;



    //生命高于50%，增伤10%	
    public float Hp_above_Damage_Increased_key;
    public float Hp_above_Damage_Increased;

    //获取食物时伤害增加0.5%，可累加
    public float food_Damage_Increased;

    //击杀精英或者boss，每5秒恢复2%血量，受击移除   key=间隔  value=恢复量
    public float killElite_RestoreHp_key;
    public float killElite_RestoreHp_value;

    //累计击杀300怪物，发射一次10m半径冲击波，造成攻击力10%伤害
    public float killEy_shockWaveNum;
    public float killEy_shockWaveRadius;
    public float killEy_shockWaveAttack;

    //每前进一层，伤害增加10%
    public float goupstairs_Damage_Increased;


    //生命低于50%，免伤10%	
    public float Hp_above_DR_key;
    public float Hp_above_DR_value;

    //对精英或boss怪增伤10%
    public float Elite_Damage_Increased;

    //击杀精英或者boss，攻速提高10%，受击移除 
    public float killElite_Attack_Speed;
    //累计击杀300怪物，立即恢复随机1个技能冷却 
    public float killEy_Skill_cooldownNum_key;
    public float killEy_Skill_cooldownNum_value;
    //每进入一次商店，伤害增加10%	
    public float box_Damage_Increased;






    //---------------------爬塔事件增益
    //击杀精英后伤害增加
    public float kill_elite_dmgUp;
    //击杀精英怪物额外概率获取随机宝物
    public float kill_elite_relicRate;
    //通关普通怪物关卡获取额外数量随机宝物
    public int win_common_relic;
    //战斗结束随机技能升级
    public int battleEndSkillUp;
    //怪物伤害提高
    public float enemyDmgUp;




    //--------------遗物参数

    //武器类
    public float weaponSize;
    public float weaponDmg;
    public float weaponCd;
    //闪击  
    public float electricShockProbability;
    public int electricShockNum;
    public float electricDmg;
    public float electricShockCd = 0.5f;

    //诅咒
    public float curseProbability;
    public float cureseDelay;
    public float curseDmg;

    //斩杀 概率 生命百分比
    public float executeProbability;
    public float executeHp;

    //暴击
    public float critProbability;
    public float critDmg;

    //燃烧 
    public float burnProbability;
    public float burnDmg;
    public float burnInterval;
    public float burnTime;

    //冰冻
    public float frozenProbability;
    public float frozenTime;

    //爆炸
    public float killBoomProbability;
    public float killBoomSize = 1;
    public float killBoomDmg = 0.25f;
    public float killBoomCd = 0.5f;

    //武装类
    public float armSize;
    public float armDmg;
    public float armCd;

    //技能类
    public int dlyFlag;  // 0/1
    public float dlyTime;
    public float dlyDmg;
    public float dlyEy;


    public int killBoomLevel = 0;
    public int paopaoLevel = 0;

    //角色类
    public float expUp = 0;
    public float goldUp = 0;
    public float moveSpeedUp = 0;
    public float magnet = 0;
    //每5秒回复
    public float recovery5s = 0;
    //击杀掉落血包
    public float killBloodPack = 0;
    //升级回血
    public float upgradeRecover = 0;
    //血少增伤
    public float hpDropDmgUp = 0;

    //概率胜利额外宝物
    public float relicNumUp = 0;
    //概率提升品质
    public float relicQUp = 0;
    //--------------end



    public static RoleManager Get()
    {
        if (RoleManager.mgr == null)
            RoleManager.mgr = new RoleManager();
       
        return RoleManager.mgr;
    }

    void initialization() {
        superNum = 0;
        attackUp = 1;
        hpUp = 1;
        defense = 0;
        recovery = 0;
        getGold = 0;
        speed = 0;
        reviveNum = 0;
        revive_attackUp = 0;
        revive_speedUp = 0;
        //角色类
        expUp = 0;
        goldUp = 0;
        moveSpeedUp = 0;
        recovery5s = 0;
        killBloodPack = 0;
        magnet = 0;
        upgradeRecover = 0;
        hpDropDmgUp = 0;
        relicNumUp = 0;
        relicQUp = 0;

        //武士刀
        katanaNumUp = 0;
        KatanaRepel = 0;
        katanaVampire = 0;



        //装备词条
        //x范围内敌人速度降低  entropicAura_RadiusAndDown
        entropicAura_Radius = 0;
        entropicAura_Down = 0;
        //击杀精英/boss获得护盾
        killElite_Shield = 0;
        //击杀x数量敌人获得速度提升 killEySpeedUpNum
        killEySpeedUpNum = 0;
        killEySpeedUp = 0;
        //通过普通关卡后增加伤害
        normalTowerNodeDmgUp = 0;


        //---------new 装备词条
        //食物回复效果提升5%	
        Food_RecoveryUp=0;
        //基本移动速度+1	
        Basic_Movement_Speed = 0;
        //击杀精英或者boss，获得10%增伤，受击移除
        killElite_Damage_Increased = 0;
        //累计击杀300怪物，获得15%移速加成，受击移除
        killEyMoveSpeedUpNum = 0;
        killEyMoveSpeedUp = 0;
        //每完成一次事件，伤害增加10%	
        event_Damage_Increased = 0;
        //3%概率秒杀小怪
        Probability_of_killing = 0;
        //死亡免费复活1次，伤害增加10%
        //复活次数
        reviveNum = 0;
        //复活后攻击力加成
        revive_attackUp = 0;
        //复活后速度加成
        revive_speedUp = 0;
        //击杀精英或者boss，50%概率额外掉落1张铭牌
        killElite_getNameplateProbability = 0;
        //累计击杀300怪物，恢复5%生命上限
        killEy_Hp_restoreNum = 0;
        killEy_Hp_restore = 0;
        //每完成一次精英关卡，伤害增加10%	
        EliteEnd_Damage_Increased = 0;
        //生命高于50%，增伤10%	
        Hp_above_Damage_Increased_key = 0;
        Hp_above_Damage_Increased = 0;
        //获取食物时伤害增加0.5%，可累加
        food_Damage_Increased = 0;
        //击杀精英或者boss，每5秒恢复2%血量，受击移除   key=间隔  value=恢复量
        killElite_RestoreHp_key = 0;
        killElite_RestoreHp_value = 0;
        //累计击杀300怪物，发射一次10m半径冲击波，造成攻击力10%伤害
        killEy_shockWaveNum = 0;
        killEy_shockWaveRadius = 0;
        killEy_shockWaveAttack = 0;
        //每前进一层，伤害增加10%
        goupstairs_Damage_Increased = 0;
        //生命低于50%，免伤10%	
        Hp_above_DR_key = 0;
        Hp_above_DR_value = 0;
        //对精英或boss怪增伤10%
        Elite_Damage_Increased = 0;
        //击杀精英或者boss，攻速提高10%，受击移除 
        killElite_Attack_Speed = 0;
        //累计击杀300怪物，立即恢复随机1个技能冷却 
        killEy_Skill_cooldownNum_key = 0;
        killEy_Skill_cooldownNum_value = 0;
        //每进入一次商店，伤害增加10%	
        box_Damage_Increased = 0;



        //武器类----------------------------------
        weaponSize = 0;
        weaponDmg = 0;
        weaponCd = 0;
        //shanji
        electricShockProbability = 0;
        electricShockNum = 0;
        electricDmg = 0;
        electricShockCd = 0;
        //诅咒
        curseProbability = 0;
        cureseDelay = 0;
        curseDmg = 0;
        //斩杀 概率 生命百分比
        executeProbability = 0;
        executeHp = 0;
        //暴击
        critProbability = 0;
        critDmg = 0;
        //燃烧 
        burnProbability = 0;
        burnDmg = 0;
        burnInterval = 0;
        burnTime = 0;
        //冰冻
        frozenProbability = 0;
        frozenTime = 0;

        //击杀爆炸
        killBoomProbability = 0;
        killBoomSize = 0;
        killBoomDmg = 0;

        //武装类-------------------------------
        armSize = 0;
        armDmg = 0;
        armCd = 0;

        //技能类---------------------------------
        dlyFlag = 1;
        dlyTime = 0;
        dlyDmg = 0;
        dlyEy = 0;


        kill_elite_dmgUp = 0 ;
        //击杀精英怪物额外概率获取随机宝物
        kill_elite_relicRate = 0;
        //通关普通怪物关卡获取额外数量随机宝物
        win_common_relic = 0;
        //战斗结束随机技能升级
        battleEndSkillUp = 0;
        enemyDmgUp = 0;


        paopaoLevel = 0;
    }


    //计算所有属性
    public void init(bool forGame=false) {
        initialization();
        compute();

        if (forGame) {
            List<Relic> relicList = DataManager.Get().userData.towerData.relicList;
            //计算遗物属性
            for (int i = 0; i < relicList.Count; i++)
            {
                RelicConfig config = TowerFactory.Get().relicMap[relicList[i].configId];
                if (relicList[i].level == 0) 
                    analysisAffix(config.effect_1);
                if (relicList[i].level == 1)
                    analysisAffix(config.effect_2);
                if (relicList[i].level == 2)
                    analysisAffix(config.effect_3);
            }


            //计算爬塔buff
            List<string> buffList = DataManager.Get().userData.towerData.buffList;
            for (int i = 0; i < buffList.Count; i++) {
                TowerEventBuffConfig c = TowerFactory.Get().eventBuffList.Find(x => x.id == buffList[i]);
                if (c != null) { 
                    Debug.Log("buff:   " + c.effect.Replace("buff_", ""));
                    analysisAffix(c.effect.Replace("buff_",""));
                }
            }
            //todo
            DungeonManager.upLevelNum = battleEndSkillUp;
        }

        attack = (int)(attack * attackUp);
        hp = (int)(hp * hpUp);
    }

    //计算属性  弹幕互动模式
    public void init_zb() {
        initialization();

        attack = 100;
        hp = 10000;

        attack = (int)(attack * attackUp);
        hp = (int)(hp * hpUp);
    }

    public void compute() {
        attack = RoleFactory.Get().roleAttrMap[DataManager.Get().roleAttrData.nowLevel].attack;
        hp = RoleFactory.Get().roleAttrMap[DataManager.Get().roleAttrData.nowLevel].hp;
        speed = RoleFactory.Get().roleAttrMap[DataManager.Get().roleAttrData.nowLevel].speed;


        if (DataManager.Get().roleAttrData == null)
            return;

        //计算天赋属性
        if (DataManager.Get().roleAttrData.talentList != null)
        {
            for (int i = 0; i < DataManager.Get().roleAttrData.talentList.Count; i++)
            {
                string talentId = DataManager.Get().roleAttrData.talentList[i];
                if (TalentFactory.Get().talentMap.ContainsKey(talentId))
                {
                    Talent t = TalentFactory.Get().talentMap[talentId];
                    if (t.type != null && t.type == "attack")
                    {
                        attack += t.value;
                    }
                    else if (t.type != null && t.type == "hp")
                    {
                        hp += t.value;
                    }
                    else if (t.type != null && t.type == "defense")
                    {
                        defense += t.value;
                    }
                    else if (t.type != null && t.type == "recovery")
                    {
                        recovery += t.value;
                    }
                    else if (t.type != null && t.type == "gold")
                    {
                        getGold += t.value;
                    }
                }
            }
        }

        //计算装备属性
        for (int i = 0; i < DataManager.Get().roleAttrData.weaponsBackPackItems.Count; i++)
        {
           
            EquipmentData ed = DataManager.Get().roleAttrData.weaponsBackPackItems[i];

            //EquipmentData ed = DataManager.Get().userData.equipmentDataList[i];
            //if (ed.wearing)
            {
                //... todo
                EquipmentAtr atr = EquipmentFactory.Get().map[ed.id];

                string[] mainAtrValues = atr.mainAtrValueStr.Split('|');
                int value = int.Parse(mainAtrValues[ed.quality]);

                string[] mainAtrValueUps = atr.mainAtrValueUp.Split('|');
                int valueUp = int.Parse(mainAtrValueUps[ed.quality]);

                if (atr.mainAtr == "attack")
                {
                    attack += value + (ed.level - 1) * valueUp;
                }
                else if (atr.mainAtr == "hp")
                {
                    hp += value + (ed.level-1) * valueUp;
                }


                //...todo   根据品质附加属性
                //绿
                if (ed.quality >= 1)
                {
                    EquipmentAffix affix = EquipmentFactory.Get().affixMap[atr.atr1_id];
                    analysisAffix(affix.effect);
                }
                //蓝
                if (ed.quality >= 2)
                {
                    EquipmentAffix affix = EquipmentFactory.Get().affixMap[atr.atr2_id];
                    analysisAffix(affix.effect);
                }
                //紫
                if (ed.quality >= 3)
                {
                    EquipmentAffix affix = EquipmentFactory.Get().affixMap[atr.atr3_id];
                    analysisAffix(affix.effect);
                }
                //橙
                if (ed.quality >= 6)
                {
                    EquipmentAffix affix = EquipmentFactory.Get().affixMap[atr.atr4_id];
                    analysisAffix(affix.effect);
                }
                //红
                if (ed.quality >= 10)
                {
                    EquipmentAffix affix = EquipmentFactory.Get().affixMap[atr.atr5_id];
                    analysisAffix(affix.effect);
                }
            }
        }
    }

    public void analysisAffix(string effect) {

        if (effect == null || effect.Length == 0)
            return;

        string[] effects = effect.Split('|');

        for (int i=0;i< effects.Length;i++) {
            string[] kv = effects[i].Split(':');

            string key = kv[0];
            float value = 0;
            if (kv[1].IndexOf("/") == -1) 
            {
                value = float.Parse(kv[1]);
            }

            if (key == "attackUp")
            {
                attackUp += value;
            }
            else if (key == "hpUp")
            {
                hpUp += value;
            }
            else if (key == "revive")
            {
                reviveNum += (int)value;
            }
            else if (key == "revive_attackUp")
            {
                revive_attackUp += value;
            }
            else if (key == "revive_speedUp")
            {
                revive_speedUp += value;
            }
            else if (key == "expUp")
            {
                expUp += value;
            }
            else if (key == "goldUp")
            {
                goldUp += value;
            }
            else if (key == "moveSpeedUp")
            {
                moveSpeedUp += value;
            }
            else if (key == "recovery5s")
            {
                recovery5s += value;
            }
            else if (key == "killBloodPack")
            {
                killBloodPack += value;
            }
            else if (key == "magnet")
            {
                magnet += value;
            }
            else if (key == "upgradeRecover")
            {
                upgradeRecover += value;
            }
            else if (key == "hpDropDmgUp")
            {
                hpDropDmgUp += value;
            }
            else if (key == "relicNumUp")
            {
                relicNumUp += value;
            }
            else if (key == "relicQUp")
            {
                relicQUp += value;
            }
            else if (key == "weaponSize")
            {
                weaponSize += value;
            }
            else if (key == "weaponDmg")
            {
                weaponDmg += value;
            }
            else if (key == "weaponCd")
            {
                weaponCd += value;
            }
            else if (key == "electricShockProbability")
            {
                electricShockProbability += value;
            }
            else if (key == "electricShockNum")
            {
                electricShockNum += (int)value;
            }
            else if (key == "electricDmg")
            {
                electricDmg += value;
            }
            else if (key == "electricShockCd")
            {
                electricShockCd = value;
            }
            //诅咒
            else if (key == "curseProbability")
            {
                curseProbability += value;
            }
            else if (key == "cureseDelay")
            {
                cureseDelay = value;
            }
            else if (key == "curseDmg")
            {
                curseDmg += value;
            }
            //斩杀 概率 生命百分比
            else if (key == "executeProbability")
            {
                executeProbability += value;
            }
            else if (key == "executeHp")
            {
                executeHp = value;
            }
            //暴击
            else if (key == "critProbability")
            {
                critProbability += value;
            }
            else if (key == "critDmg")
            {
                critDmg = value;
            }
            //燃烧 
            else if (key == "burnProbability")
            {
                burnProbability += value;
            }
            else if (key == "burnDmg")
            {
                burnDmg += value;
            }
            else if (key == "burnInterval")
            {
                burnInterval = value;
            }
            else if (key == "burnTime")
            {
                burnTime = value;
            }
            //冰冻
            else if (key == "frozenProbability")
            {
                frozenProbability = value;
            }
            else if (key == "frozenTime")
            {
                frozenTime = value;
            }
            else if (key == "killBoomProbability")
            {
                killBoomProbability = value;
            }
            else if (key == "killBoomSize")
            {
                killBoomSize = value;
            }
            else if (key == "killBoomDmg")
            {
                killBoomDmg = value;
            }
            else if (key == "armSize")
            {
                armSize += value;
            }
            else if (key == "armDmg")
            {
                armDmg += value;
            }
            else if (key == "armCd")
            {
                armCd += value;
            }
            else if (key == "dlyFlag")
            {
                dlyFlag = (int)value;
            }
            else if (key == "dlyTime")
            {
                dlyTime += value;
            }
            else if (key == "dlyDmg")
            {
                dlyDmg += dlyDmg;
            }
            else if (key == "dlyEy")
            {
                dlyEy += value;
            }
            else if (key == "paopaoLevel")
            {
                paopaoLevel = (int)value;
            }

            else if (key == "entropicAura_RadiusAndDown") {
                string[] strs = kv[1].Split("/");
                entropicAura_Radius = float.Parse(strs[0]);
                entropicAura_Down += float.Parse(strs[1]);
            }
            else if (key == "killEySpeedUp")
            {
                string[] strs = kv[1].Split("/");
                killEySpeedUpNum = int.Parse(strs[0]);
                killEySpeedUp += float.Parse(strs[1]);
            }
            else if (key == "killElite_Shield")
            {
                killElite_Shield += value;
            }
            else if (key == "normalTowerNodeDmgUp")
            {
                normalTowerNodeDmgUp += value;
            }
            else if (key == "katanaNumUp")
            {
                katanaNumUp += (int)value;
            }
            else if (key == "KatanaRepel")
            {
                KatanaRepel = 1;
            }
            else if (key == "katanaVampire")
            {
                katanaVampire += value;
            }
            else if (key == "super") {
                superNum += 1;
            }
            else if (key == "kill_elite_dmgUp")
            {
                kill_elite_dmgUp += value;
            }
            else if (key == "kill_elite_relicRate")
            {
                kill_elite_relicRate += value;
            }
            else if (key == "win_common_relic")
            {
                win_common_relic += (int)value;
            }
            else if (key == "battleEndSkillUp")
            {
                battleEndSkillUp += (int)value;
            }
            else if (key == "enemyDmgUp")
            {
                enemyDmgUp += value;
            }
            else if (key == "Food_RecoveryUp")
            {
                Food_RecoveryUp += value;
            }
            else if (key == "Basic_Movement_Speed")
            {
                Basic_Movement_Speed += value;
            }
            else if (key == "killElite_Damage_Increased")
            {
                killElite_Damage_Increased += value;
            }
            else if (key == "killEyMoveSpeedUp")
            {
                string[] strs = kv[1].Split("/");
                killEyMoveSpeedUpNum = int.Parse(strs[0]);
                killEyMoveSpeedUp += float.Parse(strs[1]);
            }
            else if (key == "event_Damage_Increased")
            {
                event_Damage_Increased += value;
            }
            else if (key == "Probability_of_killing")
            {
                Probability_of_killing += value;
            }
            else if (key == "reviveNum")
            {
                reviveNum += (int)(value);
            }
            else if (key == "revive_attackUp")
            {
                revive_attackUp += value;
            }
            else if (key == "revive_speedUp")
            {
                revive_speedUp += value;
            }
            else if (key == "killElite_getNameplateProbability")
            {
                killElite_getNameplateProbability += value;
            }
            else if (key == "killEy_Hp_restore")
            {
                //累计击杀300怪物，恢复5 % 生命上限
                string[] strs = kv[1].Split("/");
                killEyMoveSpeedUpNum = int.Parse(strs[0]);
                killEyMoveSpeedUp += float.Parse(strs[1]);
            }
            else if (key == "EliteEnd_Damage_Increased")
            {
                EliteEnd_Damage_Increased += value;
            }
            else if (key == "Hp_above_Damage_Increased")
            {
                //生命高于50%，增伤10%	
                string[] strs = kv[1].Split("/");
                Hp_above_Damage_Increased_key = float.Parse(strs[0]);
                Hp_above_Damage_Increased += float.Parse(strs[1]);
            }
            else if (key == "food_Damage_Increased")
            {
                food_Damage_Increased += value;
            }
            else if (key == "killElite_RestoreHp")
            { //击杀精英或者boss，每5秒恢复2%血量，受击移除   key=间隔  value=恢复量
                string[] strs = kv[1].Split("/");
                killElite_RestoreHp_key = float.Parse(strs[0]);
                killElite_RestoreHp_value += float.Parse(strs[1]);
            }
            else if (key == "killEy_shockWave")
            { //累计击杀300怪物，发射一次10m半径冲击波，造成攻击力10%伤害
                string[] strs = kv[1].Split("/");
                killEy_shockWaveNum = float.Parse(strs[0]);
                killEy_shockWaveRadius = float.Parse(strs[1]);
                killEy_shockWaveAttack = float.Parse(strs[2]);
            }
            else if (key == "goupstairs_Damage_Increased")
            {
                goupstairs_Damage_Increased += value;
            }
            else if (key == "Hp_above_DR")
            {
                //生命低于50%，免伤10%	
                string[] strs = kv[1].Split("/");
                Hp_above_DR_key = float.Parse(strs[0]);
                Hp_above_DR_value += float.Parse(strs[1]);
            }
            else if (key == "Elite_Damage_Increased")
            {
                Elite_Damage_Increased += value;
            }
            else if (key == "killElite_Attack_Speed")
            {
                killElite_Attack_Speed += value;
            }
            else if (key == "killEy_Skill_cooldownNum")
            {
                //生命低于50%，免伤10%	
                string[] strs = kv[1].Split("/");
                killEy_Skill_cooldownNum_key = float.Parse(strs[0]);
                killEy_Skill_cooldownNum_value += float.Parse(strs[1]);
            }
            else if (key == "box_Damage_Increased")
            {
                box_Damage_Increased += value;
            }
        }
    }
}
