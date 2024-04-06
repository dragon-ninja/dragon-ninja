using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkill : MonoBehaviour
{
    public bool mainWeapon = false;

    //技能参数
    public SkillAttr attr;
    //一形态  
    public SkillAttr attr_move;
    //二形态
    public SkillAttr attr_idle;
    //二形态无能量
    public SkillAttr attr_idle_notEnergy;
    //当前使用的形态参数
    public SkillAttr nowAttr;

    public Player player;
    //狂暴模式触发延迟时间
    public float superAttackDelayTime_now;
    public float superAttackDelayTime_max = 1.5f;
    //狂暴模式读条准备时间
    public float superAttackReadyTime_now;
    public float superAttackReadyTime_max = 1.5f;
    //超级攻击模式标记  替代原来的dly系统
    public bool superAttackFlag;
    public float superAttackTime_now;
    public float superAttackTime_max = 10;

    public float now_cd = 2;
    public float now_cd_idle = 2;
    public float now_cd_move = 2;

    public GameObject pf;
    public GameObject pf_move;
    public GameObject pf_idle;
    public GameObject pf_idle_notEnergy;

    //十字斩
    public GameObject szz_pf;

    public bool endFlag = true;
    GameObject skillBox;
    RectTransform cd_rtra_idle;
    RectTransform cd_rtra_move;
    Transform EYUI;
    Image cd_img;
    List<Image> EYs;
    int maxEy = 4;
    int attackCount = 0;


    //武士刀特殊处理参数:
    int energy;
    float[] dmgUps;
    float[] sizeUps;
    //融合十字斩 相关的特殊判定用参数 
    public bool skill_Released;
    public bool skill_Skip;
    //复合枪专属参数
    public float reloadLastTime;



    public void init() {
        pf = Resources.Load<GameObject>("skill/" + attr.pfPath);
        szz_pf = Resources.Load<GameObject>("skill/CrossChop");
        player = transform.GetComponent<Player>();

        if (mainWeapon) {
            maxEy = 5;
            energy = maxEy;


            cd_rtra_idle = GameObject.Find("FightCanvas").
                transform.Find("HP_UI").Find("UI_List").Find("CD_UI").
                 Find("value").GetComponent<RectTransform>();
            cd_rtra_move = GameObject.Find("FightCanvas").
                transform.Find("HP_UI").Find("UI_List").Find("CD_UI").
                 Find("value_move").GetComponent<RectTransform>();


            cd_img = cd_rtra_idle.GetComponent<Image>();

            EYUI = GameObject.Find("FightCanvas").
                transform.Find("HP_UI").Find("UI_List").Find("EY_UI");

            EYs = new List<Image>();
            for (int c = 0; c < EYUI.childCount; c++) {
                EYs.Add(EYUI.GetChild(c).gameObject.GetComponent<Image>());
                EYs[c].gameObject.SetActive(false);
                EYs[c].enabled = false;
            }
            for (int c = 0; c < maxEy; c++)
            {
                EYs[c].gameObject.SetActive(true);
            }



            //Debug.Log()

            attr_move = attr;
            attr_idle = SkillAttrFactory.Get().skillMap[attr.VTskill][attr.level-1];
            //attr_idle_notEnergy = SkillAttrFactory.Get().skillMap[attr_idle.VTskill][attr.level - 1];

            pf_move = Resources.Load<GameObject>("skill/" + attr_move.pfPath);
            pf_idle = Resources.Load<GameObject>("skill/" + attr_idle.pfPath);
            //pf_idle_notEnergy = Resources.Load<GameObject>("skill/" + attr_idle_notEnergy.pfPath);
            superAttackTime_max = float.Parse(attr_idle.exclusiveValue["time"].ToString()) * (1 + RoleManager.Get().dlyTime);
            superAttackDelayTime_max = float.Parse(attr_idle.exclusiveValue["delay"].ToString());
            superAttackReadyTime_max = float.Parse(attr_idle.exclusiveValue["ready"].ToString());
            DungeonManager.dlyCameraSizeUp = float.Parse(attr_idle.exclusiveValue["cameraReduce"].ToString());

            player.idle_dlyEsMax = int.Parse(attr_idle.exclusiveValue["energy"].ToString());
            player.move_dlyEsMax = int.Parse(attr_idle.exclusiveValue["energy"].ToString());

            player.move_dlyEsNow = Mathf.Min(
                player.move_dlyEsMax,
               Mathf.FloorToInt(player.move_dlyEsMax * DataManager.Get().userData.towerData.eyRate)
               );
            player.idle_dlyEsNow = Mathf.Min(
                player.idle_dlyEsMax,
               Mathf.FloorToInt(player.idle_dlyEsMax * DataManager.Get().userData.towerData.eyRate)
               );
            player.addDlyEs(0);
        }

        //武士刀特殊处理   根据积攒能量,提高伤害和尺寸
        /*if (attr_idle != null && attr_idle.skillType == "KatanaVT"
            && attr_idle.exclusive!= null)
        {
            string[] strs = attr_idle.exclusive.Split(';');
            string dmg = strs[0];
            string size = strs[1];

            string[] dmgStrs = dmg.Split(':');
            string[] sizeStrs = size.Split(':');

            string[] dmgValues = dmgStrs[1].Split(',');
            string[] sizeValues = sizeStrs[1].Split(',');

            dmgUps = new float[5];
            sizeUps = new float[5];
            for (int i = 0; i < 5; i++)
            {
                dmgUps[i] = float.Parse(dmgValues[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                sizeUps[i] = float.Parse(sizeValues[i]);
            }
            print(dmgUps);
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        if (DlySkill.dlyIngFlag || DlySkill.dlyReadyEndFlag )
            return;

        if (mainWeapon )
        {
            //强力攻击期间 
            if (player.superAttackIng)
            {
                if (superAttackTime_now > 0)
                {
                    superAttackTime_now -= Time.deltaTime;
                    player.superAttackWDTime -= Time.deltaTime;

                    player.idle_dlyEsNow = player.idle_dlyEsMax *
                        superAttackTime_now / superAttackTime_max;
                    player.move_dlyEsNow = player.move_dlyEsMax *
                        superAttackTime_now / superAttackTime_max;
                    player.addDlyEs(0);

                    if (superAttackTime_now <= 0)
                    {
                        player.idle_dlyEsNow = 0;
                        player.move_dlyEsNow = 0;
                        player.addDlyEs(0);
                        superAttackFlag = false;
                    }
                }
                //强力攻击结束
                if (endFlag && !superAttackFlag)
                {
                    player.superAttackIng = false;
                    player.superAttackTiredTime = 5;
                    cd_rtra_idle.gameObject.SetActive(false);
                    //cd_rtra_move.gameObject.SetActive(true);
                    player.ctr.animator_move.gameObject.SetActive(true);
                    player.ctr.animator_idle.gameObject.SetActive(false);
                    player.ctr.idle_change_flag = false;
                    player.ctr.move_change_flag = true;
                    player.ctr.BQ.transform.localScale = new Vector3(1.5f, 1.5f, 1);
                    pf = pf_move;
                    attr = attr_move;
                    now_cd_move = attr.getCd();
                }
            }
            //强力攻击读条期间
            if (player.superAttackReady) {
                superAttackReadyTime_now -= Time.deltaTime;

                cd_rtra_idle.sizeDelta = new Vector2(1.2f * 
                    (1 - Mathf.Max(superAttackReadyTime_now,0)/ superAttackReadyTime_max), 0.1f);
                if (superAttackReadyTime_now<=0) {
                    player.superAttackReady = false;
                    GetComponent<Rigidbody2D>().mass = 2;
                    //开启强力攻击模式
                    player.superAttackEnergyReady = false;
                    superAttackFlag = true;
                    player.superAttackIng = true;
                    //狂暴攻击持续时间
                    superAttackTime_max = float.Parse(attr_idle.exclusiveValue["time"].ToString()) * (1 + RoleManager.Get().dlyTime);
                    superAttackTime_now = superAttackTime_max;
                        //superAttackTime_max * (1 + RoleManager.Get().dlyTime);
                    player.superAttackWDTime = 1;
                    attr = attr_idle;
                    pf = pf_idle;
                    now_cd_idle = attr.getCd();

                    player.ctr.animator_move.gameObject.SetActive(false);
                    player.ctr.animator_idle.gameObject.SetActive(true);
                    player.ctr.idle_change_flag = true;
                    player.ctr.move_change_flag = false;
                    DungeonManager.superAttackReadyFlag = false;
                    DungeonManager.superAttackReadyEndFlag = true;
                    cd_rtra_idle.gameObject.SetActive(false);
                    cd_rtra_idle.transform.parent.gameObject.SetActive(false);

                    //能量充满  否则复合枪可能无法攻击
                    if (attr.skillType.IndexOf("Gun") != -1)
                    {
                        energy = maxEy;
                        reloadLastTime = 0;
                        for (int c = 0; c < energy; c++)
                            EYs[c].enabled = true;
                    }
                }
                if (player.ctr.moveState == 1) { 
                    player.superAttackReady = false;
                    GetComponent<Rigidbody2D>().mass = 2;
                    DungeonManager.superAttackReadyFlag = false;
                    DungeonManager.superAttackReadyEndFlag = true;
                    cd_rtra_idle.gameObject.SetActive(false);
                    cd_rtra_idle.transform.parent.gameObject.SetActive(false);
                }

                return;
            }

            if (player.superAttackEnergyReady
                && player.ctr.moveState == 0 
                && !player.superAttackDelay 
                && !player.superAttackReady) {
                player.superAttackDelay = true;
                superAttackDelayTime_now = superAttackDelayTime_max;
            }
            //读条前延迟判定
            if (player.superAttackDelay)
            {
                if (player.ctr.moveState == 1)
                {
                    player.superAttackDelay = false;
                    return;
                }

                superAttackDelayTime_now -= Time.deltaTime;
                if (superAttackDelayTime_now <= 0)
                {
                    player.superAttackDelay = false;
                    //准备开启强力攻击模式
                    player.superAttackReady = true;
                    GetComponent<Rigidbody2D>().mass = 100;
                    superAttackReadyTime_now = 
                        superAttackReadyTime_max;
                    //todo 播放动画
                    player.ctr.animator_move.Play("baoqi");
                    DungeonManager.superAttackReadyFlag = true;
                    cd_rtra_idle.sizeDelta = new Vector2(0, 0.1f);
                    cd_rtra_idle.gameObject.SetActive(true);
                    cd_rtra_idle.transform.parent.gameObject.SetActive(true);
                }
            }
            
            //换弹相关
            if (attr.skillType.IndexOf("Gun") != -1 && energy <=0 ) {
                if (reloadLastTime==0)
                    reloadLastTime = Time.time;
                if (attr.exclusiveValue != null && attr.exclusiveValue.ContainsKey("reload") &&
                    (Time.time - reloadLastTime) > float.Parse(attr.exclusiveValue["reload"].ToString())) {
                    energy = maxEy;
                    reloadLastTime = 0;

                    for (int c = 0; c < energy; c++)
                    {
                        EYs[c].enabled = true;
                    }
                }
                return;
            }

            if (endFlag) {
                if (player.superAttackIng)
                {
                    now_cd_idle -= Time.deltaTime;
                    now_cd = Mathf.Max(now_cd_idle, 0);
                    //cd_rtra_idle.sizeDelta = new Vector2(1.2f * (1 - now_cd / attr.cd), 0.1f);
                }
                else
                {
                    now_cd_move -= Time.deltaTime;
                    now_cd = Mathf.Max(now_cd_move,0);
                    //cd_rtra_move.sizeDelta = new Vector2(1.2f * (1 - now_cd / attr.cd), 0.1f);
                }

           
                if ((attr.getCd() > 0 && now_cd <= 0) || (attr.getCd() <= 0 && endFlag))
                {
                    if (player.superAttackIng)
                    {

                        if (attr.skillType.IndexOf("Gun") != -1)
                        {
                            player.ctr.animatorUtil_idle.TrySet("attack", null);
                            player.ctr.animatorUtil_move.TrySet("attack", null);
                        }
                        else { 
                            player.ctr.animatorUtil_idle.TrySet("attack_idle", null);
                            player.ctr.animatorUtil_move.TrySet("attack_idle", null);
                        }
                    }
                    else {
                        player.ctr.animatorUtil_idle.TrySet("attack", null);
                        player.ctr.animatorUtil_move.TrySet("attack", null);
                    }
                    tryStartSkill();
                }
            }
        }
        else if(/*!player.superAttackIng &&*/ !player.superAttackReady)
        {
            if (endFlag) 
            { 
                if ((attr.getCd() > 0 && (now_cd -= Time.deltaTime) <= 0)
                    || (attr.getCd() <= 0 && endFlag)) {
                    tryStartSkill();
                }
            }
        }
    }

    void tryStartSkill() {
        if (attr.getCd()!=0 ||(attr.getCd() <= 0 && skillBox == null))
        {
            startSkill();
        }
    }

    //持续时间较长的技能 升级时立即刷新技能
    public void refreshSkill() {
        if (attr.duration == -1 /*|| attr.getDuration() >= 5*/) {
            Destroy(skillBox);
            StopCoroutine("follow_attack");
            startSkill();
        }
    }

    //技能升级后立即刷新 重新释放技能  主要用于持续类技能
    void startSkill() {

        if (pf == null)
            init();

        specialHandle();

        now_cd = attr.getCd();
      
        if (mainWeapon ) {
            if (player.superAttackIng)
            {
                now_cd_idle = attr.getCd();
            }
            else {
                now_cd_move = attr.getCd();
            }
        }

        endFlag = false;

        if (attr.ackType.IndexOf("bullet") != -1) {
            StartCoroutine(buttle_attack());
        }
        else if (attr.skillType.IndexOf("Huo") != -1 || attr.skillType.IndexOf("Shui") != -1)
            StartCoroutine(hs_point_attack());
        else if (attr.ackType.IndexOf("follow") != -1)
            StartCoroutine("follow_attack");
        else if (attr.ackType.IndexOf("point") != -1)
            StartCoroutine(point_attack());
        else if (attr.ackType.IndexOf("line") != -1)
            StartCoroutine(line_attack());
    }


    public void specialHandle() {


        if (attr.skillType.IndexOf("Gun") != -1 &&(attr.exclusiveValue != null && attr.exclusiveValue.ContainsKey("reload")))
        {
            for (int c = 0; c < maxEy; c++)
            {
                EYs[c].enabled = false;
            }

            energy -= 1;
            for (int c = 0; c < energy; c++)
            {
                EYs[c].enabled = true;
            }
        }


        //todo 原双形态逻辑
        return;


        if (attr.skillType == "Katana" && attr_idle != null && energy < maxEy) {
            attackCount += 1;
            pf = pf_move;
            if (attackCount == 5) {
                attackCount = 0;
                energy += 1;
                for (int c = 0; c < energy; c++) 
                {
                    EYs[c].enabled = true;
                }
            }
        }
        if (attr.skillType == "KatanaVT" || attr.skillType == "KatanaVTNotEy") {
            if (energy <= 0) {
                pf = pf_idle_notEnergy;
                attr = attr_idle_notEnergy;
            }
            else {
                pf = pf_idle;
                attr = attr_idle;
            }
        }
    }



    IEnumerator buttle_attack() {
        GameObject pfcl = pf;
        SkillAttr attrcl = this.attr;
        yield return new WaitForSeconds(attr.delay);
        
        int count = attr.getNum();
        int angle = 0;
        int rotate = 0;
        if (attr.skillType == "Firebomb")
        {
            angle = 360 / count;
        }
        else {
            angle = attr.angle;
        }
        if (attr.skillType == "Gun" && count > 1 && count % 2 != 0)
        {
            rotate = -angle * (count/2);
        }
       



        Vector3 start = player.transform.position;
        Vector3 dir = Vector3.zero;
        Transform enemyTra = LockUtil.lockTarget(player.transform, attr.getRange());
        if (enemyTra != null)
            dir = (enemyTra.position - start).normalized;
        else
            dir = player.ctr.moveVec; //new Vector2(player.facingDirection, 0);

        //限制技能角度
        if (attr.skillType == "Katana" || attr.skillType== "Gun")
        {
            int i = (dir.x > 0 ? 1 : -1);
            float boxAngle = (Vector2.Angle(transform.up, dir))
                 * (player.facingDirection == i ? 1 : -1);
            if (boxAngle >180 || boxAngle < 0) { 
                dir = player.ctr.moveVec;
            }
        }

        int num = attr.getNum();
        if (this.attr.skillType.IndexOf("Katana") != -1) {
            num += RoleManager.Get().katanaNumUp;
        }


        //调整枪口方向
        if (attr.skillType.IndexOf("Gun") != -1)
            player.ctr.setShotDir(dir);

        Vector2 start_dir = dir;
        for (int i = 0; i < num; i++) {

            skill_Released = true;

            if(attr.skillType == "Katana" && attr.skillType == "Chop")
                yield return new WaitForSeconds(0.1f);

            skill_Released = false;

            if (skill_Skip) {
                skill_Skip = false;
                yield return new WaitForSeconds(attr.getInterval());
                continue;
            }

            bool szz_flag = false;
            //获取技能主导权 todo 方案1
            /*  if (attr.skillType == "Katana" && player.bsMap.ContainsKey("Chop"))
            {
                int sn = player.bsMap["Chop"].attr.num;
                if (sn > i) {
                    szz_flag = true;
                    pfcl = szz_pf;
                }
                else
                    pfcl = pf;
            }*/
            //todo 方案2
            pfcl = pf;
            if (player.bsMap.ContainsKey("Chop")) {
                if (player.bsMap["Chop"].skill_Released)
                {
                    szz_flag = true;
                    pfcl = szz_pf;
                    player.bsMap["Chop"].skill_Skip = true;
                }
            }


            dir = (Quaternion.AngleAxis(rotate, 
                new Vector3(0, 0, 1)) * start_dir).normalized;

            rotate += angle;
            GameObject skillBox = Instantiate(pfcl);
            BaseHitBox box = initBox(skillBox);
            box.attr = attrcl;
            skillBox.transform.position = transform.position;
            box.flyDir = dir;
            box.startPos = transform.position;

            if (szz_flag) {
                box.dmg_additional = player.bsMap["Chop"].attr.getDamage();
            }

            float boxAngle = Vector2.Angle(transform.up, box.flyDir);
            box.transform.localEulerAngles = new Vector3(0, 0, (float)boxAngle * (box.flyDir.x > 0 ? -1 : 1));
            box.skillCount = i;
            yield return new WaitForSeconds(attr.getInterval());
        }
        endFlag = true;
    }

    IEnumerator follow_attack() {

        yield return new WaitForSeconds(attr.delay);
        skillBox = Instantiate(pf);
        BaseHitBox box = initBox(skillBox);
        skillBox.transform.position = player.transform.position;
      

        if (attr.duration != -1) { 
            yield return new WaitForSeconds(attr.getDuration());
            //if (attr.getDuration() < 9999) 
            {
                endFlag = true;
                Destroy(skillBox);
            }
        }
    }

    IEnumerator point_attack() {

        yield return new WaitForSeconds(attr.delay);

       
        int angle = 360 / attr.getNum();
        int rotate = 0;

        List<GameObject> targets = new List<GameObject>();

        if (attr.skillType.IndexOf("Gun") != -1) { 
            targets = LockUtil.lockTargets(this.transform, attr.getRange(), attr.getNum());
            //调整枪口方向
             player.ctr.setShotDir(new Vector3(0.75f, 0.75f, 0));
        }


        for (int i = 0; i < attr.getNum(); i++)
        {
            Vector3 start = player.transform.position;
            Transform enemyTra = LockUtil.lockTarget(player.transform, attr.getRange(), 
                attr.skillType == "Lightning");
            if (enemyTra != null && attr.ackType.IndexOf("random") == -1)
                start = enemyTra.position;
            else
                //没有目标  角色自身周围随机点
                start = player.transform.position +
                    new Vector3(Random.Range(-attr.getRange(), attr.getRange()),
                                Random.Range(-attr.getRange(), attr.getRange()), 0);

            if (attr.skillType.IndexOf("Gun") != -1)
            {
                Vector3 dir = (Quaternion.AngleAxis(rotate, new Vector3(0, 0, 1))
                        * new Vector3(0, 1, 0)).normalized * attr.getRange();
                start = player.transform.position + dir;
                rotate += angle;

                if (targets.Count > i) {
                    start = targets[i].transform.position;
                } 
                else if (targets.Count>0) {
                    start = targets[targets.Count-1].transform.position;
                }
            }


            //链接光球超武特殊处理
            if (attr.skillType == "LaserBall" && attr.level==6) {

                Vector3 dir = (Quaternion.AngleAxis(rotate, new Vector3(0, 0, 1))
                        * new Vector3(0, 1, 0)).normalized * 20;
                start = player.transform.position + dir;
                rotate += angle;
            }


            GameObject skillBox = Instantiate(pf);
            skillBox.transform.position = start;
            BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
            box.bs = this;
            box.attr = this.attr;
            box.skillCount = i;
            box.startPos = start;

            if (attr.skillType == "Wind")
            {
                skillBox.transform.position = player.transform.position;
                BaseHitBox boxC = box.transform.GetChild(0).GetComponent<BaseHitBox>();
                boxC.attr = this.attr.Clone();
                boxC.attr.ackType = "";
                boxC.attr.skillType = "";
                boxC.bs = this;
                float scale = attr.getDmgSize();
                boxC.transform.localPosition = new Vector3(0,attr.range);
                boxC.transform.localScale = new Vector3(transform.localScale.x * scale,
                    transform.localScale.y * scale,
                    transform.localScale.z * scale);
            }


            //相关赋值
            box.pointVec = start;

            yield return new WaitForSeconds(attr.getInterval());
        }
        endFlag = true;
    }

    IEnumerator line_attack()
    {
        yield return new WaitForSeconds(attr.delay);
        Transform enemyTra = LockUtil.lockTarget(player.transform, 20);
        for (int i = 0; i < attr.getNum(); i++) {
            //生成线
            GameObject skillBox = Instantiate(pf);
            skillBox.transform.position = player.transform.position;
            BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
            box.pointVec = enemyTra.position;
            box.skillCount = i;
            box.bs = this;
            box.attr = this.attr;
            yield return new WaitForSeconds(attr.getInterval());
        }
        endFlag = true;
    }

    //火神之力  海神之力特殊处理
    IEnumerator hs_point_attack()
    {
        yield return new WaitForSeconds(attr.delay);
        //固定点 绕圈  以角色正上方为起点   和以角色正下方为起点

        int angle = attr.angle;
        float rotate = 0;
        Vector3 playerStart = player.transform.position;
        Vector3 skillStart = player.transform.position;

        playerStart = player.transform.position;
        skillStart = player.transform.position +
                    new Vector3(0, attr.range, 0);

        for (int i = 0; i < attr.getNum(); i++)
        {
           
           
            //todo
           /* {
                Transform enemyTra = LockUtil.lockTarget(player.transform, attr.range+2, false);
                if (enemyTra != null)
                    skillStart = player.transform.position + (enemyTra.position - playerStart).normalized * 5;
            }*/

            Vector3 pos = LockUtil.RotateAngle
                (playerStart, skillStart, rotate);
            rotate += angle;
            GameObject skillBox = Instantiate(pf);
            skillBox.transform.position = pos;
            BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
            box.bs = this;
            box.skillCount = i;
            box.attr = this.attr;
            //box.transform.parent = player.transform;
            //box.followVec = skillBox.transform.position - player.transform.position;
            //相关赋值
            box.pointVec = pos;

            yield return new WaitForSeconds(attr.getInterval());
        }
        endFlag = true;
    }








    BaseHitBox initBox(GameObject skillBox) {
        BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
        box.attr = this.attr;
        box.bs = this;
        return box;
    }
}
