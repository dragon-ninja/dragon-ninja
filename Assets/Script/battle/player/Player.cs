using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using Com.LuisPedroFonseca.ProCamera2D;
using System;
using Unity.Burst.Intrinsics;

public class Player : MonoBehaviour
{
    public Dictionary<string, BaseSkill> bsMap = new Dictionary<string, BaseSkill>();
    Dictionary<string, SkillAttr> passiveSkillMap = new Dictionary<string, SkillAttr>();

    public List<Relic> relicList = new List<Relic>();

    public bool TestFlag;
    [SerializeField]
    public List<SkillTest> skills = new List<SkillTest>();

    [HideInInspector]
    public PlayerController ctr;
    [HideInInspector]
    public DungeonManager dungeonManager;
    [HideInInspector]
    public float drawSize = 2;
    [HideInInspector]
    public BaseSkill[] skAry;
    [HideInInspector]
    public SkeletonMecanim skm;
    [HideInInspector]
    public SpriteRenderer sp;
   
    Transform drawSizeBox;
    Image expUi;
    Slider dly_idleEsUi;
    Slider dly_moveEsUi;
    Image energyValueImage;
    Image energyValueImage_2;
    TextMeshProUGUI levelUi;


    public float speed = 100f;
    public int hp = 20;
    [HideInInspector]
    public float speed_now = 100;
    [HideInInspector]
    public int hp_max = 20;
    [HideInInspector]
    public int hp_now = 20;
    [HideInInspector]
    public int es_now = 0;
    [HideInInspector]
    public int level = 0;
    [HideInInspector]
    public int exp_now;
    float recovery = 0;
    float recoveryTime = 0;
    [HideInInspector]
    public float idle_dlyEsNow = 0;
    [HideInInspector]
    public int idle_dlyEsMax = 100;
    [HideInInspector]
    public float move_dlyEsNow = 0;
    [HideInInspector]
    public int move_dlyEsMax = 100;
    [HideInInspector]
    public int facingDirection = -1;
    [HideInInspector]
    public int updownDirection = -1;

    Animator levelUpAnim;
    bool colorReset = true;
    float colorTime = 0;
    UpLevel ul;
    //DlySkill dlySkill;
    GameObject HpUI;
    [HideInInspector]
    public BaseHitBox Shield;
    //闪击cd
    public float electricShockCd_now;
    //击杀爆炸cd
    public float killBoomCd_now;

    //硬直参数
    [HideInInspector] public float stiffTime = 0;
    float stiffForce;
    //击退方向
    Vector3 hitDic;

    [HideInInspector] public bool hideFlag;
    //能量已满标记
    [HideInInspector] public bool superAttackEnergyReady;
    [HideInInspector] public bool superAttackIng;
    //应策划需求的延迟时间标记 延迟完了才读条
    [HideInInspector] public bool superAttackDelay;
    //狂暴模式读条标记
    [HideInInspector] public bool superAttackReady;
    //开启狂暴模式后短暂无敌
    [HideInInspector] public float superAttackWDTime;
    //狂暴模式结束后x秒内不能继续获得能量
    [HideInInspector] public float superAttackTiredTime;


    //直播参数
    Image zb_hpUi;
    TextMeshProUGUI zb_hpUi_value;
    public List<EntourageZB> EntourageList = new List<EntourageZB>();

    private void Awake()
    {
       
        ctr = GetComponent<PlayerController>();
        UpLevel.playerPassiveSkillLevelInfos = new Dictionary<string, SkillLevelInfo>();
        UpLevel.playerActiveSkillLevelInfos = new Dictionary<string, SkillLevelInfo>();
        skAry = GetComponents<BaseSkill>();

        if (DungeonManager.zb_mode == 0)
        {
            expUi = GameObject.Find("EXP_UI/value").GetComponent<Image>();
            levelUi = GameObject.Find("level/levelValue").GetComponent<TextMeshProUGUI>();
            HpUI = GameObject.Find("HP_UI");
        }
        else {

            zb_hpUi = GameObject.Find("HP_UI_zb/value").GetComponent<Image>();
            zb_hpUi_value = GameObject.Find("HP_UI_zb/value/Text (TMP)").GetComponent<TextMeshProUGUI>();
        }

        sp = this.GetComponent<SpriteRenderer>();
        ul = GameObject.Find("Canvas").transform.Find("up_level").GetComponent<UpLevel>();
        drawSizeBox = transform.Find("drawSize");
        dungeonManager = GameObject.Find("GameManager").GetComponent<DungeonManager>();
        drawSize = 3;

        levelUpAnim = GameObject.Find("levelUp").GetComponent<Animator>();

        Vibration.Init();
    }

    private void Start()
    {
        try { 
            killNum = 0;
            UpLevel.playerActiveSkillLevelInfos = new Dictionary<string, SkillLevelInfo>();
            relicList = DataManager.Get()?.userData?.towerData?.relicList;
            try
            {
                if(DungeonManager.zb_mode == 0)
                    RoleManager.Get().init(true);
                else
                    RoleManager.Get().init_zb();
            }
            catch (Exception ex)
            {
            }
            ul.init();
            drawSize = 3 * (1 + RoleManager.Get().magnet);
            speed_now = (speed * (1 + RoleManager.Get().moveSpeedUp));
            hp_max = RoleManager.Get().hp;
            hp_now = RoleManager.Get().hp;
            energyValueImage = GameObject.Find("superAckEnergy").GetComponent<Image>();
            energyValueImage_2 = energyValueImage.transform.Find("value").GetComponent<Image>();
            //读取爬塔信息
            if (DataManager.Get()?.userData?.towerData != null)
            {
                level = DataManager.Get().userData.towerData.level;
                exp_now = DataManager.Get().userData.towerData.exp;
                hp_now = Mathf.Min(hp_max,Mathf.FloorToInt(hp_max * DataManager.Get().userData.towerData.hpRate));

                levelUi.text = this.level+""; //SpriteNumUtil.zhInt(this.level);
                expUi.fillAmount = (this.exp_now + 0.0f) / ExpFactory.Get().expMap[this.level];

                if (DataManager.Get().userData.towerData.skillInfo != null)
                    foreach (var item in DataManager.Get().userData.towerData.skillInfo)
                    {
                        addSkill(SkillAttrFactory.Get().skillMap[item.Key][item.Value - 1], false);
                    }
                else {
                    foreach (var item in skills)
                    {
                        if (item.level > -1)
                        {
                            int level = Mathf.Min(item.level, 5);
                            addSkill(SkillAttrFactory.Get().skillMap[item.name][level]);
                        }
                    }
                }

                expUp(DataManager.Get().userData.towerData.extraExp);
                DataManager.Get().userData.towerData.extraExp = 0;

                dungeonManager.setGold(DataManager.Get().userData.towerData.gold);
            }

    #if UNITY_EDITOR
            if (TestFlag) {
                foreach (var v in bsMap) { 
                    Destroy(v.Value);
                }

                bsMap.Clear();

                foreach (var item in skills)
                {
                    if (item.level > -1)
                    {
                        int level = Mathf.Min(item.level, 5);
                        addSkill(SkillAttrFactory.Get().skillMap[item.name][level]);
                    }
                }
            }
    #endif


            //根据宝物赋予技能
            /*if (RoleManager.Get()?.paopaoLevel > 0) {
                addSkill(SkillAttrFactory.Get().skillMap["Shield"][RoleManager.Get().paopaoLevel-1],false,true);
            }*/

            changeDlyEsUI();

            if (bsMap.ContainsKey("Katana")) { 
                ctr.initSpine("KatanaSpine");
            }
            else if (bsMap.ContainsKey("Gun")) { 
                ctr.initSpine("GunSpine");
            }
            //addSkill(SkillAttrFactory.Get().skillMap["Wind"][0]);


            if (DungeonManager.zb_mode != 0)
            {
                hpUpdate();
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Errer:"+ex);
            /*GameObject.Find("ErrDesc").GetComponent<TextMeshProUGUI>().text =
                "start:" + ex.ToString();*/
        }
        //expUp(2000);
    }

    public static bool levelUpIng;


    private void Update()
    {
        if (awaitlevelSkillUnm > levelSkillUnm && !levelUpIng) {
            if (selectSkillAwaitTime > 0)
            {
                selectSkillAwaitTime -= Time.deltaTime;
            }
            else { 
                levelUpIng = true;

                if(DungeonManager.zb_mode == 0)
                    ul.up(true);
            }
        }


        try { 
            if (hideFlag)
                return;
            superAttackTiredTime -= Time.deltaTime;

            if (stiffTime > 0)
            {
                stiffTime -= Time.deltaTime;
                transform.position += 
                    hitDic * 
                    Time.deltaTime * stiffForce * stiffTime;
                return;
            }

            if (!colorReset && (colorTime -= Time.deltaTime) <=0) {
                colorReset = true;
                /*if (ctr.moveState == 0)
                {
                    skm.skeleton.SetColor(new Color32(255, 255, 255, 255));
                }
                else {
                    skm.skeleton.SetColor(new Color32(255, 100, 255, 255));
                }*/
            }

            if ((recoveryTime += Time.deltaTime) > 5) {
                recoveryTime = 0;
                hp_now = Mathf.Min(hp_now + 
                     (int)(hp_max * recovery / 100)
                    +(int)(hp_max * RoleManager.Get().recovery5s / 100),
                    hp_max);
            }

            this.electricShockCd_now -= Time.deltaTime;
            this.killBoomCd_now -= Time.deltaTime;

        }catch (Exception ex)
        {
           /* GameObject.Find("ErrDesc").GetComponent<TextMeshProUGUI>().text =
                "update:" + ex.ToString();*/
        }
    }

    void hpUpdate() {

        zb_hpUi.fillAmount = Mathf.Clamp((hp_now + 0.0f) / (hp_max),0,1);
        zb_hpUi_value.text = "HP:" + hp_now + "/" + hp_max;
    }

    public void expUp(int exp)
    {
        try
        {
            this.exp_now += (int)(exp * (1 + RoleManager.Get().expUp));
            if (this.exp_now >= ExpFactory.Get().expMap[this.level])
            {
                this.exp_now = this.exp_now - ExpFactory.Get().expMap[this.level];
                //this.exp_now = 0;
                Upgrade();
                if (this.exp_now > 0)
                    expUp(0);
            }
            expUi.fillAmount = (this.exp_now + 0.0f) / ExpFactory.Get().expMap[this.level];

        }
        catch (Exception ex)
        {
            /*GameObject.Find("ErrDesc").GetComponent<TextMeshProUGUI>().text =
                "expup:" + ex.ToString();*/
        }
    }

    public int levelSkillUnm; 
    public int awaitlevelSkillUnm;
    float selectSkillAwaitTime;


    void Upgrade(bool awaitUpgradeFlag = true)
    {

        levelUpAnim.Play("levelUp");
        this.level += 1;
        awaitlevelSkillUnm++;

        if (selectSkillAwaitTime <= 0) {
            selectSkillAwaitTime = 0.5f;
        }

        //战斗结束后吃经验立即升级
        if(dungeonManager.gameEndFlag)
            selectSkillAwaitTime = 0f;

        //升级回血
        hp_now = Mathf.Min(hp_max,
            (int)(hp_now + hp_max * RoleManager.Get().upgradeRecover));
        //DungeonManager.upLevelNum += 1;
        levelUi.text = this.level + ""; //SpriteNumUtil.zhInt(this.level);
    }


    public BaseSkill addSkill(SkillAttr sa,bool save = true,bool itemAdd = false) {

        try { 
        bool mainWeapon = false;
        if (sa.skillType == "Katana" || sa.skillType == "Gun") {
            mainWeapon = true;
        }
        //暂时剔除龙卷风技能
       /* if (sa.skillType == "Wind") {
            //移除火/水技能
            UpLevel.playerActiveSkillLevelInfos.Remove("Huo");
            UpLevel.playerActiveSkillLevelInfos.Remove("Shui");
            Destroy(bsMap["Huo"]);
            Destroy(bsMap["Shui"]);
        }*/

        BaseSkill bs1 = null;


        if (sa.skillType.IndexOf("buff_") != -1)
        {
            if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey(sa.skillType))
                UpLevel.playerPassiveSkillLevelInfos[sa.skillType].level = sa.level;
            else
                UpLevel.playerPassiveSkillLevelInfos.Add(sa.skillType,
                   new SkillLevelInfo(sa,sa.skillType, sa.level, sa.breach, "buff"));
            
            passiveSkillMap[sa.skillType] = sa;

            if (sa.skillType == "buff_hpUp") {
                int oldmax = hp_max;
                hp_max = (int)(hp * (1 + sa.level * 0.2f));
                hp_now = (int)(hp_now * hp_max / oldmax);
            }
            if (sa.skillType == "buff_moveSpeed")
            {
                this.speed_now = (speed * (1 + sa.level * 0.1f));
            }
            if (sa.skillType == "buff_magnet")
            {
                drawSizeBox.localScale = new Vector3(sa.level + 1, sa.level + 1, sa.level + 1);
                drawSize =  sa.level * 2 + 3;
            }
            if (sa.skillType == "buff_recovery")
            {
                recovery = sa.level * 1;
            }
            if (sa.skillType == "buff_ldhp")
            {
                RoleManager.Get().electricShockProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().electricShockNum = int.Parse(sa.exclusiveValue["数量"].ToString());
                RoleManager.Get().electricDmg = float.Parse(sa.exclusiveValue["伤害"].ToString());
            }
            if (sa.skillType == "buff_eyhp")
            {
                RoleManager.Get().curseProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().cureseDelay = float.Parse(sa.exclusiveValue["延迟"].ToString());
                RoleManager.Get().curseDmg = float.Parse(sa.exclusiveValue["伤害"].ToString());
            }
            if (sa.skillType == "buff_tlhp")
            {
                RoleManager.Get().executeProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().executeHp = float.Parse(sa.exclusiveValue["斩杀线"].ToString());
            }
            if (sa.skillType == "buff_fnhp")
            {
                RoleManager.Get().critProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().critDmg = float.Parse(sa.exclusiveValue["伤害"].ToString());
            }
            if (sa.skillType == "buff_hyhp")
            {
                RoleManager.Get().burnProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().burnTime = float.Parse(sa.exclusiveValue["时长"].ToString());
                RoleManager.Get().burnInterval = float.Parse(sa.exclusiveValue["间隔"].ToString());
                RoleManager.Get().burnDmg = float.Parse(sa.exclusiveValue["伤害"].ToString());
            }
            if (sa.skillType == "buff_hshp")
            {
                RoleManager.Get().frozenProbability = float.Parse(sa.exclusiveValue["概率"].ToString());
                RoleManager.Get().frozenTime = float.Parse(sa.exclusiveValue["时长"].ToString());
            }
        }
        else {
            if (!itemAdd) { 
                UpLevel.setNum(sa);

                if (!DamageMeters.damageMap.ContainsKey(sa.skillType))
                    DamageMeters.damageMap[sa.skillType] = 0;

                if (UpLevel.playerActiveSkillLevelInfos.ContainsKey(sa.skillType))
                    UpLevel.playerActiveSkillLevelInfos[sa.skillType].level = sa.level;
                else
                    UpLevel.playerActiveSkillLevelInfos.Add(sa.skillType,
                       new SkillLevelInfo(sa,sa.skillType, sa.level, sa.breach));
            }

            if (bsMap.ContainsKey(sa.skillType))
            {
                bsMap[sa.skillType].attr = sa;
                bsMap[sa.skillType].init();
            }
            else
            {
                bs1 = this.gameObject.AddComponent<BaseSkill>();
                bs1.attr = sa;
                bsMap.Add(sa.skillType, bs1);
                bs1.mainWeapon = mainWeapon;
                bs1.init();

            }
            bsMap[sa.skillType].refreshSkill();
        }
 

        //保存技能和等级信息
        if (save && DataManager.Get().userData.towerData!=null) {

            if (DataManager.Get().userData.towerData.skillInfo == null)
                DataManager.Get().userData.towerData.skillInfo = new Dictionary<string, int>();

            DataManager.Get().userData.towerData.
                skillInfo[sa.skillType] = sa.level;
            DataManager.Get().save();
        }
        return bs1;

        }catch (Exception ex)
        {
            /*GameObject.Find("ErrDesc").GetComponent<TextMeshProUGUI>().text =
                "addskill:" + ex.ToString();*/
        }

        return null;
    }

    public int checkSkillLevel(SkillAttr sa) {
        if (sa.skillType.IndexOf("buff_") != -1 && passiveSkillMap.ContainsKey(sa.skillType))
        {
            return passiveSkillMap[sa.skillType].level;
        }
        else if (bsMap.ContainsKey(sa.skillType))
        {
            return bsMap[sa.skillType].attr.level;
        }
        
        return 0;
    }


    public void hurt(HitInfo ht) {

        if (hideFlag || superAttackReady || superAttackWDTime>0)
            return;

        //取消各种受击之后会消失的buff
        {
            speed_now = (speed * (1 +
                    RoleManager.Get().moveSpeedUp 
                    ));
        }


        float dmgRate = 1;
        if (UpLevel.playerPassiveSkillLevelInfos.ContainsKey("buff_deUp"))
            dmgRate = 1 - UpLevel.playerPassiveSkillLevelInfos["buff_deUp"].level * 0.1f;

        if (es_now > 0) {
            es_now -= (int)(ht.damage * dmgRate);


            //如果是超武 可以将敌人击退
            if (Shield.bs.attr.level == 6) {
               //todo  还需要这个效果吗?
            }


            if (es_now <= 0 && Shield != null) {
                Shield.bs.endFlag = true;
                //护盾破碎
                Destroy(Shield.gameObject);
            }
            return;
        }

        Debug.Log("------------hp_now:" + hp_now+ "          ht.damage:" + ht.damage+ "       dmg:"+((int)(ht.damage * dmgRate * (1 + RoleManager.Get().enemyDmgUp))));

        //hp_now -= (int)MathF.Max((int)(ht.damage * dmgRate * (1 + RoleManager.Get().enemyDmgUp)),1);
        hp_now -= (int)(ht.damage * dmgRate * (1 + RoleManager.Get().enemyDmgUp));

        if (hp_now <= 0) {
            //死亡 跳出结算画面
            Time.timeScale = 0;
            dungeonManager.settlement(0);
            return;
        }

        //被击退
        stiffTime = 0.15f;
        stiffForce = 50;
        hitDic = (transform.position - ht.hitPos).normalized;
        ProCamera2DShake.Instance.Shake("PlayerHit");

        //震动
        //Handheld.Vibrate();
        if(DataManager.Get().userData.settingData.shockFlag)
            Vibration.VibratePeek(); 

        if (colorReset) {
            colorReset = false;
            colorTime = 0.05f;
            //改变透明度
            //skm.skeleton.SetColor(new Color32(255, 73, 37, 100));
        }



        if (DungeonManager.zb_mode != 0) {
            hpUpdate();
        }
    }

    public void addDlyEs(int es , bool maxFlag = false) {
        if (RoleManager.Get().dlyFlag == 0)
            return;

        if (DlySkill.dlyIngFlag || superAttackTiredTime > 0)
            return;

        if (maxFlag)
        {
            this.idle_dlyEsNow += this.idle_dlyEsMax;
            this.move_dlyEsNow += this.move_dlyEsMax;
        }
        else {
            this.move_dlyEsNow += es * (1 + RoleManager.Get().dlyEy);
            this.idle_dlyEsNow += es * (1 + RoleManager.Get().dlyEy);
            /* if (this.ctr.moveState == 0)
                 this.idle_dlyEsNow += es;
             else
                 this.move_dlyEsNow += es;*/
        }

        this.idle_dlyEsNow = Mathf.Min(this.idle_dlyEsNow, this.idle_dlyEsMax);
        this.move_dlyEsNow = Mathf.Min(this.move_dlyEsNow, this.move_dlyEsMax);

        if (this.idle_dlyEsNow>= this.idle_dlyEsMax &&
            this.move_dlyEsNow >= this.move_dlyEsMax)
        {
            superAttackEnergyReady = true;
        }

        //dly_idleEsUi.value = this.idle_dlyEsNow / this.idle_dlyEsMax;
        //dly_moveEsUi.value = this.move_dlyEsNow / this.move_dlyEsMax;

        energyValueImage.fillAmount = this.idle_dlyEsNow / this.idle_dlyEsMax * 0.85f + 0.15f;
        if(energyValueImage.fillAmount>=1)
            energyValueImage_2.gameObject.SetActive(true);
        else
            energyValueImage_2.gameObject.SetActive(false);
    }

    public void changeDlyEsUI() {
       
        if (RoleManager.Get().dlyFlag == 0)
        {
            energyValueImage.gameObject.SetActive(false);
        }
        else {
            //播放动画 但是只播放一次
     /*       if (!DataManager.Get().userData.towerData.dlyDOTweenFlag)
            {
                DataManager.Get().userData.towerData.dlyDOTweenFlag = true;
                DataManager.Get().save();
                GameObject.Find("Canvas").transform.Find("openDly")
                        .gameObject.SetActive(true);
            }else*/
                energyValueImage.gameObject.SetActive(true);
        }
    }

    public void hide() {
        hideFlag = true;
        ctr.animator_move.gameObject.SetActive(false);
        ctr.animator_idle.gameObject.SetActive(false);
        HpUI.SetActive(false);
        ProCamera2D.Instance.RemoveCameraTarget(this.transform);
        ProCamera2D.Instance.ResetMovement();
    }

    public void show() {
        hideFlag = false;
        ctr.animator_idle.gameObject.SetActive(true);
        HpUI.SetActive(true);
        ProCamera2D.Instance.AddCameraTarget(this.transform);
        ProCamera2D.Instance.MoveCameraInstantlyToPosition(this.transform.position);
        foreach (var item in bsMap) {
            item.Value.refreshSkill();
        }
    }


    //击杀计数,记录用于触发各种类似装备词缀的效果
    [HideInInspector] public int killNum;
    [HideInInspector] public int killEySpeedUpNum;
    //击杀敌人
    public void addkill(string type = null) {
        killNum++;

        if (RoleManager.Get().killEySpeedUpNum > 0) {
            killEySpeedUpNum++;
            if (killEySpeedUpNum > RoleManager.Get().killEySpeedUpNum) {
                killEySpeedUpNum = 0;
                //提升速度 直到受击
                speed_now = (speed * (1 + 
                    RoleManager.Get().moveSpeedUp +
                    RoleManager.Get().killEySpeedUp
                    ));
            }
        }

        if (RoleManager.Get().killElite_Shield > 0 && (type=="jy" || type=="boss")) {
            es_now += (int)(hp_max * RoleManager.Get().killElite_Shield);
        }
    }

}



[System.Serializable]
public struct SkillTest {
    public string name;
    public int level;

}

