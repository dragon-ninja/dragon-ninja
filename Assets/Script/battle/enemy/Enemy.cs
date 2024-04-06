using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public DungeonManager mgr;
    public Transform spriteTra;
    public SpriteRenderer sprite;
    Animator animator;
    public EnemyAttr atr;
    Player player;
    //闪击特效
    static GameObject LightningBlue;
    static GameObject boomPf;

    Collider2D myCollider;

    public float speed = 5;
    public int attack = 1;
    public int hp = 20;
    public int hp_now = 20;
    public int exp = 1;

    public float atkRange = 1;
    public Transform target;
    public float atkcd = 5;
    
    //碰撞攻击冷却
    float atkcd_now = 0;
    //远程攻击冷却
    float bullet_atkcd_now = 0;
    bool atkflag = false;
    GameObject bulletPf;
    //硬直参数
    float stiffTime = 0;
    float stiffForce;
    //击退方向
    Vector3 hitDic;
    //不可行动标记
    public bool notAction;
    //不可选取标记
    public bool notTarget;
    //debuff
    bool curseFlag;
    float curseDmg;
    float cureseDelay;

    public bool bleedFlag;
    public float bleedDmg;
    public float bleedInterval;
    public float bleedInterval_now;
    public float bleedTime_now;


    public bool burnFlag;
    public float burnDmg;
    public float burnInterval;
    public float burnInterval_now;
    public float burnTime_now;

    public bool frozenFlag;
    public float frozenTime_now;

    //boss是否进入狂暴状态
    bool rageFlag;

    //受击材质
    public Material SpritesDefault;
    public Material SpritesHit;
    //精英血条
    JY_HPUI hp_ui;

    private void Awake()
    {
        spriteTra = transform.Find("spine");
        sprite = spriteTra.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //SpritesDefault = sprite.material;
       // SpritesHit = Resources.Load<Material>("role/enemy/SpritesHit");

        target = GameObject.Find("role").transform;
        player = target.GetComponent<Player>();
        bulletPf = Resources.Load<GameObject>("skill/" + atr.bulletPfPath);
        myCollider = GetComponent<Collider2D>();
        animator = sprite.GetComponent<Animator>();
    }

    public void init() {
        atkRange = transform.localScale.x * 1.6f;
        atkcd_now = 0;
        bullet_atkcd_now = 0;
        atkflag = false;
        //硬直参数
        stiffTime = 0;
        stiffForce = 0;
        //不可行动标记
        notAction = false;
        //不可选取标记
        notTarget = false;
        //debuff
        curseFlag = false;
        curseDmg = 0;
        cureseDelay = 0;

        bleedFlag = false;
        bleedDmg = 0;
        bleedInterval = 0;
        bleedInterval_now = 0;
        bleedTime_now = 0;

        burnFlag = false;
        burnDmg = 0;
        burnInterval = 0;
        burnInterval_now = 0;
        burnTime_now = 0;
        frozenFlag = false;
        frozenTime_now = 0;

        //boss是否进入狂暴状态
        rageFlag = false;

        dieFlag = false;
        drawFlag = false;

        if (atr.type == "精英") {
            GameObject hp_uiobj = Instantiate(DungeonManager.JY_HP_UI_Pf);
            hp_ui = hp_uiobj.GetComponent<JY_HPUI>();
            hp_ui.enemy = this;
            hp_uiobj.SetActive(true);
            hp_uiobj.transform.parent = GameObject.Find("FightCanvas").transform;
        }
    }


    private void Update()
    {
        die();

        hitAnimUpdate();

        if (atr.type == "boss") {
            DungeonManager.bossHP.value = (hp_now + 0.0f) / hp;
            bossActionEventUpdate();
        }

        if (notAction || dieFlag)
            return;

        atkcd_now -= Time.deltaTime;
        bullet_atkcd_now -= Time.deltaTime;

        if (stiffTime > 0 && atr.type != "boss") {
            stiffTime -= Time.deltaTime;
            transform.position += hitDic * Time.deltaTime * stiffForce * stiffTime;
            return;
        }

        debuffUpdate();

        if (frozenFlag)
            return;

        //持续追踪目标
        if (Vector2.Distance(target.position, transform.position) > 0)
        {
            trackAction();
        }

        if (atkcd_now <= 0)
        {
            attackAction();
        }


        if (atr.bullet_range > 0 &&
            Vector2.Distance(target.position, transform.position) < atr.bullet_range)
        {
            if (bullet_atkcd_now <= 0)
            {
                shot();
            }
        }
    }



    void trackAction() {
        {
            float speedDown = 1;
            //减速光环
            if (RoleManager.Get().entropicAura_Radius > 0) {
                if (Vector2.Distance(transform.position, target.position)
                    < RoleManager.Get().entropicAura_Radius
                    ) {
                    speedDown -= RoleManager.Get().entropicAura_Down;
                }
            }


            transform.position +=
                (target.position - transform.position).normalized
                * Time.deltaTime * speed * 0.07f * speedDown;
        }


        Vector3 theScale = transform.localScale;

        if ((target.position.x > transform.position.x && theScale.x > 0)
            || (target.position.x < transform.position.x && theScale.x < 0))
            theScale.x *= -1;

        transform.localScale = theScale;
    }

    void attackAction()
    {
        atkflag = true;
    }

    void shot() {

        bullet_atkcd_now = atr.bullet_cd;

        //获取 弹体飞行方向
        Vector3 dir = (target.position - transform.position).normalized;

        //生成子弹
        GameObject skillBox = Instantiate(bulletPf);
        skillBox.transform.position = transform.position;
        Bullet box = skillBox.GetComponent<Bullet>();

        box.flyDir = dir;
        box.startPoint = transform.position;
        box.life = atr.bullet_life;
        box.attack = atr.bullet_ack;
        float boxAngle = Vector2.Angle(transform.up, box.flyDir);
        box.transform.localEulerAngles = new Vector3(0, 0, (float)boxAngle * (box.flyDir.x > 0 ? -1 : 1));
    }



    //boss拥有的技能组
    Dictionary<string, EnemySkill> bossSkillList = new Dictionary<string, EnemySkill>();
    List<string> bossSkillNameList = new List<string>();
    List<float> bossSkillCdList = new List<float>();
    //下一次需要执行的技能角标
    int bossSkillIndex = 0;
    //当前执行的技能角标
    int bossSkillIndex_now = 0;
    float bossSkillCd_now = 3;
    float bossSkillCd;
    public bool skillIng;

    void bossActionEventUpdate() {
        //初始化
        if (bossSkillList.Count <= 0)
        {
            foreach (var item in atr.skillMap_1)
            {
                //添加技能组件
                if (!bossSkillList.ContainsKey(item.skill))
                {
                    //SkillAttr satr = SkillAttrFactory.Get().bossSkillMap[item.skill];

                    SkillAttr satr = SkillAttrFactory.Get().bossSkillMap[item.skill];
                    EnemySkill eySkill = this.gameObject.AddComponent<EnemySkill>();
                    eySkill.ey = this;
                    eySkill.atr = satr;
                    eySkill.init();

                    bossSkillList.Add(item.skill, eySkill);
                }
                //添加技能事件
                bossSkillNameList.Add(item.skill);
                bossSkillCdList.Add(item.cd);
            }
        }

        if (skillIng)
        {
            bossSkillList[bossSkillNameList[bossSkillIndex_now]].skillUpdate();
        }
        else {
            bossSkillCd_now -= Time.deltaTime;
            //计算冷却
            if (bossSkillCd_now <= 0)
            {
                bossSkillIndex_now = bossSkillIndex;
                bossSkillCd = bossSkillCdList[bossSkillIndex];
                bossSkillCd_now = bossSkillCd;
                //执行技能
                bossSkillList[bossSkillNameList[bossSkillIndex]].skillStart();
                bossSkillIndex++;
            }
            //重新循环技能组
            if (bossSkillIndex >= bossSkillNameList.Count)
            {
                bossSkillIndex = 0;
            }


            //boss狂暴 重新添加新的技能组  只能在boss没有进行技能释放时切换技能组
            if (!rageFlag)
            {
                if (((hp_now + 0.0f) / hp) < 0.5f)
                {
                    rageFlag = true;
                    bossSkillCd_now = 0;
                    bossSkillIndex = 0;
                    bossSkillList.Clear();
                    bossSkillNameList.Clear();
                    bossSkillCdList.Clear();
                    foreach (var item in atr.skillMap_2)
                    {
                        //添加技能组件
                        if (!bossSkillList.ContainsKey(item.skill))
                        {
                            SkillAttr satr = SkillAttrFactory.Get().bossSkillMap[item.skill];
                            EnemySkill eySkill = this.gameObject.AddComponent<EnemySkill>();
                            eySkill.ey = this;
                            eySkill.atr = satr;
                            eySkill.init();

                            bossSkillList.Add(item.skill, eySkill);
                        }
                        //添加技能事件
                        bossSkillNameList.Add(item.skill);
                        bossSkillCdList.Add(item.cd);
                    }
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "player" && atkflag && hp_now > 0 && stiffTime<=0) {
            atkcd_now = atr.ack_cd;
            atkflag = false;
            HitInfo ht = new HitInfo();
            ht.hitPos = transform.position;
            ht.damage = this.attack;
            player.hurt(ht);
        }
    }

    public void hurt(int dmg,bool mustDie = false)
    {
        if (mustDie)
        {
            mgr.desEnemy(this);
            if (hp_ui != null)
                Destroy(hp_ui.gameObject);
            return;
        }

        this.hp_now -= dmg;

        HitInfo hf = new HitInfo();
        hf.damage = dmg;
        hf.hurtPos = transform.position;

        DamageUIManage.creatureDmgUI(hf);
    }


    //牵引标记 一个目标只能被牵引一次
    bool drawFlag;
    AudioSource _audioSource;
    AudioClip _laserSoundClip;
    public int hurt(HitInfo info)
    {
        //播放音效
        if (_audioSource == null) { 
            _audioSource = this.gameObject.AddComponent<AudioSource>();
            _laserSoundClip = Resources.Load<AudioClip>("audio/hit/1");
        }
        _audioSource.PlayOneShot(_laserSoundClip);

        if (!hit_enlarge) 
        { 
            sprite.material = SpritesHit;
            hit_enlarge = true;
            spriteTra.transform.localScale = new Vector3(
                      spriteTra.transform.localScale.x < 0 ? -1 : 1 * originScaleX * 0.6f,originScaleX * 0.6f,1);
        }

        if (info.critFlag)
        {
            info.damage = (int)(info.damage * info.critDmg);
        }

        //计算子弹剩余伤害
        int yc_dmg = info.surplusDmg - this.hp_now;


        this.hp_now -= info.damage;
        info.hurtPos = transform.position;
        DamageUIManage.creatureDmgUI(info);

        if (info.skillType != null) 
            if (DamageMeters.damageMap.ContainsKey(info.skillType))
                DamageMeters.damageMap[info.skillType] += info.damage;

        

        if (!info.notStiffCover || (info.notStiffCover && stiffTime <= 0))
            if (atr.type != "boss") {
                // todo 根据策划需求暂时屏蔽
                if (info.hitType == "牵引" /*&& !drawFlag*/)
                {
                    drawFlag = true;
                    hitDic = info.hitPos - transform.position;
                    stiffTime = info.stiffTime;
                    stiffForce = info.stiffForce;
                }
                else if (info.hitType == "击退")
                {
                    hitDic = transform.position - info.hitPos;
                    stiffTime = info.stiffTime;
                    stiffForce = info.stiffForce;
                }
                else if (info.hitType != "牵引")
                { 
                    stiffTime = info.stiffTime;
                    stiffForce = info.stiffForce;
                }
            }


        //debuff
        //闪击
        if (info.electricShockFlag && player.electricShockCd_now <= 0) {
            player.electricShockCd_now = RoleManager.Get().electricShockCd;
            List<GameObject> targets = LockUtil.lockTargets(this.transform,10,info.electricShockNum);
            hurt(info.electricDmg);

            if (LightningBlue == null)
                LightningBlue = Resources.Load<GameObject>("skill/LightningBlue");

            GameObject lg = Instantiate(LightningBlue);
            lg.transform.position = this.transform.position;

            foreach (var t in targets) {
                Enemy e = t.GetComponent<Enemy>();
                e.hurt(info.electricDmg);
                GameObject lg2 = Instantiate(LightningBlue);
                lg2.transform.position = e.transform.position;
            }
        }

        //斩杀
        if (info.executeFlag && (float)this.hp / (float)this.hp_now < info.executeHp) {
            this.hp_now -= (int)(this.hp * info.executeHp);
        }

        //诅咒
        if (info.curseFlag) {
            curseFlag = true;
            curseDmg = info.curseDmg;
            cureseDelay = info.cureseDelay;
        }

        //燃烧
        if (info.burnFlag) {
            burnFlag = true;
            burnInterval = info.burnInterval;
            burnInterval_now = burnInterval;
            burnDmg = info.burnDmg;
            burnTime_now = info.burnTime;
        }

        //流血
        if (info.bleedFlag)
        {
            bleedFlag = true;
            bleedInterval = info.bleedInterval;
            bleedInterval_now = bleedInterval;
            bleedDmg = info.bleedDmg;
            bleedTime_now = info.bleedTime;
        }


        //冰冻
        if (info.frozenFlag) {
            frozenFlag = true;
            frozenTime_now = info.frozenTime;
        }
        return yc_dmg;
    }

    void debuffUpdate() {
        if (curseFlag && (cureseDelay -= Time.deltaTime) <= 0) {
            curseFlag = false;
            this.hp_now -= (int)curseDmg;
        }

        if ((burnTime_now -= Time.deltaTime)<=0) {
            burnFlag = false;
        }

        if ((bleedTime_now -= Time.deltaTime) <= 0)
        {
            bleedFlag = false;
        }

        if (bleedFlag && (bleedInterval_now -= Time.deltaTime) <= 0)
        {
            bleedInterval_now = bleedInterval;
            this.hp_now -= (int)bleedDmg;

            HitInfo hf = new HitInfo();
            hf.damage = (int)bleedDmg;
            hf.hurtPos = transform.position;
            DamageUIManage.creatureDmgUI(hf);
        }

        if (burnFlag && (burnInterval_now -= Time.deltaTime)<=0) {
            burnInterval_now = burnInterval;
            this.hp_now -= (int)burnDmg;

            HitInfo hf = new HitInfo();
            hf.damage = (int)burnDmg;
            hf.hurtPos = transform.position;
            DamageUIManage.creatureDmgUI(hf);
        }

        if (frozenFlag && (frozenTime_now -= Time.deltaTime) <= 0)
        {
            frozenFlag = false;
        }
    }

    bool dieFlag;
    float dieTime;

    void die(bool mustDie = false) {
        if (hp_now <= 0 && !dieFlag) {
            dieFlag = true;
            animator.SetTrigger("die");
            if(atr.pf=="e4"|| atr.pf == "e5")
                dieTime = 0.01f;
            dieTime = 0.25f;
        }

        if (dieFlag && (dieTime -= Time.deltaTime) <= 0)
        {
            dieFlag = false;
            animator.SetTrigger("die");

           

            //-------------------触发击杀怪物特效 todo
            if (player.killBoomCd_now<=0 &&
                Random.value<= RoleManager.Get().killBoomProbability){
                player.killBoomCd_now = RoleManager.Get().killBoomCd;

                if (boomPf == null)
                    boomPf = Resources.Load<GameObject>("skill/boomPf");

                GameObject g = Instantiate(boomPf);
                g.transform.position = this.transform.position;
                g.transform.localScale = new Vector3(
                    RoleManager.Get().killBoomSize,
                    RoleManager.Get().killBoomSize, 1);

                List<Transform> targets = LockUtil.lockAll(this.transform, RoleManager.Get().killBoomSize);
                foreach (var t in targets)
                {
                    Enemy e = t.GetComponent<Enemy>();
                    e.hurt((int)(hp * RoleManager.Get().killBoomDmg));
                }

            }
            //----------------end




            if (DungeonManager.zb_mode != 0) {
                if (hp_ui != null)
                    Destroy(hp_ui.gameObject);
                mgr.desEnemy(this);
                return;
            }



            //掉落经验
            if (atr.expRate > Random.Range(0, 10000))
            {
                expCrystalfb exp = mgr.creatExp();
                exp.transform.position = transform.position;
                exp.exp = this.exp;
            }



            //小概率掉落炸弹和磁铁
            int value = Random.Range(0, 500);
            if (this.transform.name.IndexOf("jy") != -1)
            {
                //old 箱子掉落 暂时屏蔽
                //GameObject pf = Instantiate(DungeonManager.luckyBoxPf);
                //pf.transform.position = transform.position + new Vector3(1, 1, 0);

                if (RoleManager.Get().kill_elite_relicRate != 0) {
                    if (RoleManager.Get().kill_elite_relicRate >= Random.value) {
                        mgr.addSelectRelicNum();
                    } 
                }

                mgr.addSelectRelicNum();
                player.addkill("jy");
                if(DungeonManager.zb_mode==0)
                    DataManager.Get().userData.towerData.killNum++;
            }
            else if (this.transform.name.IndexOf("boss") != -1)
            {
                //todo 爬塔改造
                //if (DungeonManager.duration >= 900)
                {
                    mgr.settlement(2);
                    //Time.timeScale = 0;
                }
                DungeonManager.bossDie();

                GameObject pf = Instantiate(DungeonManager.luckyBoxPf);
                pf.transform.position = transform.position + new Vector3(1, 1, 0);

                player.addkill("boss");
            }
            else
            {
                player.addkill();
            }

            foreach (var item in PropFactory.Get().PropMap)
            {
                if (item.Value.flag && 
                    DungeonManager.duration > item.Value.time )
                {
                    int rate = item.Value.rate;

                    //属性效果:增加血包掉落
                    if (item.Value.id == "prop_1") {
                        rate += (int)(RoleManager.Get().killBloodPack * 100);
                    }

                    if (rate > Random.Range(0, 10000)) { 
                        GameObject pf = Instantiate(item.Value.gpf);
                        pf.transform.position = transform.position;
                        break;
                    }
                }
            }



            //增加击杀计数
            DungeonManager.addkill();
            DungeonManager.nowEnemyNum--;

            //增加角色dly能量槽
            if(!player.superAttackIng)
                player.addDlyEs(atr.dlyEs);

            //Destroy(this.gameObject);
            if(hp_ui!=null)
                Destroy(hp_ui.gameObject);

            mgr.desEnemy(this);
        }
    }

    public void hide() {
        this.tag = "Untagged";
        spriteTra.gameObject.SetActive(false);
        notTarget = true;
        myCollider.enabled = false;
    }

    public void show()
    {
        this.tag = "enemy";
        spriteTra.gameObject.SetActive(true);
        notTarget = false;
        myCollider.enabled = true;
    }


    bool hit_enlarge;
    public float originScaleX;

    //受击表现
    void hitAnimUpdate() {
        if (hit_enlarge) {
            if (spriteTra.transform.localScale.x < originScaleX)
            {
                spriteTra.transform.localScale = new Vector3(
                   spriteTra.transform.localScale.x + 0.05f,
                   spriteTra.transform.localScale.y + 0.05f,
                     1
                  );
            }
            else {
                sprite.material = SpritesDefault;
                hit_enlarge = false;
            }
        }
    }
}


[System.Serializable]
public class EnemyAttr {
    public string id;
    public string name;
    public string desc;
    public string type;
    public string pf;
    
    public int hp;
    public float attack;
    public float ack_cd;
    public float speed;

    public int bullet_ack;
    public float bullet_range;
    public float bullet_cd ;
    public string bulletPfPath;
    public float bullet_life;
    public float bullet_speed;

    public float bufferRange;
    public float bufferSpeed;

    public float expRate;
    public int exp;
    public int dlyEs;

    public string skills_1;
    public string skills_2;


    public List<BossSlillInfo> skillMap_1;
    //半血后激活
    public List<BossSlillInfo> skillMap_2;
}


