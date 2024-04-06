using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitBox : MonoBehaviour
{
    public BaseSkill bs;
    //从这里获取属性
    public SkillAttr attr;
    public float now_duration;

    //初始生成点
    public Vector3 startPos;
    //终点
    //public Vector3 endPos;
    //飞行向量
    public Vector3 flyDir;
    //目标落点
    public Vector3 pointVec = Vector3.zero;
    //和目标的向量差
    public Vector3 followVec = Vector3.zero;
    //当前造成伤害次数  --用于泡泡判定
    public int dmgCount_now;
    //同一组技能的数量角标
    public int skillCount;

    SkillAttr exAttr;
    GameObject exPf;
   
    float now_excd;
    CircleCollider2D box;
    //多段伤害技能 此值小于等于0时造成一次判定
    float dmgTime;


    //特殊参数:
    //武士刀:
    public float dmgUp = 1;
    public float sizeUp = 1;

    //十字斩附伤
    public int dmg_additional;

    //攻击原点  主要是召唤物使用
    Transform ackPos = null;
    BaseHitBox parentBs;

    //激光发生器  激光印记特殊处理
    BaseHitBox exBox;
  
    //记录已经打击过的目标
    List<Enemy> hitEnemys = new List<Enemy>();

    void Start()
    {
        cy_dmg = (int)Mathf.Floor((this.attr.getDamage() + dmg_additional) * dmgUp);
        box = GetComponent<CircleCollider2D>();
        //找到圆点  分配位置
        if (attr.skillType == "Roulette" && name.IndexOf("Roulette") != -1) {

            int count = attr.getNum();

            int angle = 360 / count;
            int rotate = 0;

            float dis = 4f;
            if(attr.level==6)
                dis = 5f;
            for (int x = 0; x < count; x++ ) {
                rotate += angle;
                Vector2 vec = LockUtil.RotateAngle(new Vector2(0, 0), new Vector2(0, dis), rotate);

                transform.GetChild(x).localPosition = vec;
                transform.GetChild(x).Rotate(new Vector3(0, 0, rotate - 360 -90));

                BaseHitBox box = transform.GetChild(x).GetComponent<BaseHitBox>();
                box.attr = this.attr.Clone();
                box.attr.ackType = "";
                box.bs = this.bs;
                transform.GetChild(x).gameObject.SetActive(true);
            }
        }

        float scale = attr.getDmgSize() * sizeUp;
        transform.localScale = new Vector3(transform.localScale.x * scale,
            transform.localScale.y * scale,
            transform.localScale.z * scale);


        if (attr.exId != null && attr.exId.Length > 0) {
            string[] exidStrs = attr.exId.Split('_');
            exAttr = SkillAttrFactory.Get().skillMap
                [exidStrs[0]][int.Parse(exidStrs[1])-1];
            exPf = Resources.Load<GameObject>("skill/" + exAttr.pfPath);
        }

        if (attr.ackType.IndexOf("summoned") != -1)
        {
            //获取召唤物攻击原点
            ackPos = transform.Find("ackpos");

            //激光发射器
            if (attr.skillType == "LaserGen" || attr.skillType == "LD") {
                ackPos = transform.GetChild(0).Find("ackpos");
            }
        }

        if (attr.moveType == "line") {
            //lineRenderer = GetComponent<LineRenderer>();
            //lineRenderer.startWidth = 0.2f;
            //lineRenderer.endWidth = 0.2f;
            lineRendererList = new List<LineRenderer>();
            foreach (LineRenderer i in transform.GetComponentsInChildren<LineRenderer>()) {
                lineRendererList.Add(i);
            }
        }

        //激光发生器特殊处理
        if (attr.skillType == "LaserGenEx") {
            //超武
            float dis = float.Parse(attr.exclusiveValue["y"].ToString()); ;
            Vector3 pos = this.ackPos.position + new Vector3(bs.player.facingDirection * dis, 0, 0);

            if (attr.level == 6 ) //todo
            {
                if (pointVec != null && pointVec != Vector3.zero)
                    pos = this.ackPos.position + (pointVec - this.ackPos.position).normalized * dis;
                laserAngleUp = 0.1f * (Random.value > 0.5f ? 1 : -1);
                int angle = 360 / 6;
                int rotate = skillCount * angle;
                //rotate = Random.Range(0,360);
                initLaserEndPos = LockUtil.RotateAngle(this.ackPos.position, pos, rotate);
            }
            else {
                dis = 3;
                if (pointVec != null && pointVec != Vector3.zero)
                    pos = this.ackPos.position + (pointVec - this.ackPos.position).normalized * dis;
                int angle = 360 / attr.num;
                int rotate = skillCount * angle;
                initLaserEndPos = LockUtil.RotateAngle(this.ackPos.position,pos,rotate);
              
            }
            
            initLaserEndPosDir = (initLaserEndPos - this.ackPos.position).normalized;
            laserSpeed = 0;


            //---------生成地面痕迹
            GameObject skillBox = Instantiate(exPf);
            skillBox.transform.position = initLaserEndPos;
            exBox = skillBox.GetComponent<BaseHitBox>();
            exBox.bs = this.bs;
            exBox.attr = this.exAttr;
            exBox.startPos = initLaserEndPos;
            if (attr.level < 6) { 
                exBox.transform.Find("hit").gameObject.SetActive(true);
                if (skillCount != attr.num - 1)
                    transform.Find("start").gameObject.SetActive(false);
            }
        }

        if (attr.skillType == "Shield")
        {
            bs.player.Shield = this;
            //bs.player.es_now = int.Parse(attr.exclusiveValue["hp"].ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DlySkill.dlyIngFlag || DlySkill.dlyReadyEndFlag /*||
            (!bs.mainWeapon && (bs.player.superAttackIng))*/   )
        {
            bs.endFlag = true;
            Destroy(this.gameObject);
            return;
        }

        now_duration += Time.deltaTime;
        dmgTime -= Time.deltaTime;

        if (!DlySkill.dlyIngFlag && !DlySkill.dlyReadyEndFlag 
            && attr.exTriggerType != null 
            && attr.exTriggerType.IndexOf("cd")!=-1) {
            if ((now_excd -= Time.deltaTime) <= 0)
            {
                now_excd = exAttr.getCd();
                if (attr.skillType == "LaserBall")
                    laserBall();
                else
                    StartCoroutine(creatEx());
            }
        }

        if (attr.skillType == "LaserGen")
        {
            Vector3 v = transform.Find("spine").localScale;
            transform.Find("spine").localScale = 
                new Vector3(bs.player.facingDirection * -Mathf.Abs(v.x), 1 * v.y, 1 * v.z);
        }

        if (attr.skillType == "LD")
        {
            Vector3 v = transform.Find("spine").localScale;
            transform.Find("spine").localScale =
                new Vector3(bs.player.facingDirection * -Mathf.Abs(v.x), v.y, v.z);
        }

        if (attr.skillType == "LaserGenEx") {
            laserGenExUpdate();
        }

        if (attr.skillType == "LaserGenExEx")
        {
            laserGenExExUpdate();
        }

        if (attr.skillType == "LaserBallEx")
        {
            laserBallEx();
        }


       if (attr.skillType == "Wind")
       {
            this.transform.Rotate(new Vector3(0, 0, (skillCount % 2 == 0 ? -1 : 1)) 
                * Time.deltaTime * attr.getSpeed() * 5);

            this.transform.GetChild(0).transform.localEulerAngles 
                = -transform.localEulerAngles;
        }


        if (box != null)
        {
            if (!box.enabled)
                box.enabled = true;

            if (attr.skillType != "Roulette" && 
                (attr.dmgCount > 1 || attr.dmgCount == -1) 
                && attr.dmgInterval != 0 && dmgTime <= 0)
            {
                dmgTime = attr.getDmgInterval();
                box.enabled = false;
            }

            //关闭伤害监测
            if (attr.damgaeCheckTime > 0 && 
                now_duration > attr.startCheckTime + attr.damgaeCheckTime)
                box.enabled = false;


            if (this.now_duration < attr.startCheckTime)
                box.enabled = false;
        }
        else {
            //射线类的多段伤害计算
            if ((attr?.dmgCount > 1|| attr?.dmgCount==-1) && dmgTime <= 0)
            {
                dmgTime = attr.getDmgInterval();
                hitEnemys.Clear();
            }
        }
       
        if (attr.moveType.IndexOf("straight")!= -1) 
        {
            if (attr.flyDuration==0 ||(attr.flyDuration > 0 && now_duration < attr.flyDuration))
            { 
                this.transform.position += flyDir.normalized * Time.deltaTime * attr.getSpeed();
            }
        }

        if (name.IndexOf("Roulette") != -1) { 
            this.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * attr.getSpeed()*2);
            transform.GetChild(0).transform.position += flyDir.normalized * Time.deltaTime * 10;
        }
     
        if (followVec != Vector3.zero) {
            this.transform.position = bs.player.transform.position + followVec;
        }

        //计算持续时间
        if (attr.duration!=-1 && now_duration >= attr.getDuration()) {

            if (attr.exTriggerType != null && 
                attr.exTriggerType.IndexOf("destroy") >-1 && 
                attr.exId != null && attr.exId.Length > 0)
            {
                StartCoroutine(creatEx());
            }

            //护盾结束
            if (attr.skillType == "Shield") {
                bs.player.Shield = null;
                bs.player.es_now = 0;
            }

            Destroy(this.gameObject);
            if (attr.skillType.IndexOf("LDEx") != -1 && attr.level == 6)
            {
                parentBs.transform.Find("spine").gameObject.SetActive(true);
            }

            if (attr.skillType == "LaserGenEx" ) {
                if (attr.level == 6)
                    Destroy(exBox.gameObject);
                else {
                    exBox.transform.Find("hit").gameObject.SetActive(false);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (attr.ackType.IndexOf("follow") != -1) { 
            this.transform.position = bs.player.transform.position;
        }

    }


    private LineRenderer lineRenderer;
    List<LineRenderer> lineRendererList = new List<LineRenderer>();
    float laserSpeed = 0;
    //初始激光终点
    Vector3 initLaserEndPos;
    //初始激光终点移动向量
    Vector3 initLaserEndPosDir;
    Vector3 hitPos;
    //圆弧形式激光角度记录
    float laserAngle = 0;
    //圆弧形式激光角度每帧变化
    float laserAngleUp = 0.1f;

    //激光发射器特殊处理
    void laserGenExUpdate() {
        if (this.ackPos == null) { 
            Destroy(this.gameObject);
            return;
        }

        this.transform.position = this.ackPos.transform.position;

        laserSpeed += attr.speed * 240 * Time.deltaTime;

        Vector3 hitpos = initLaserEndPos + initLaserEndPosDir * laserSpeed;

        //超武逻辑
        if (attr.level == 6) 
        { 
            laserAngle += laserAngleUp * 150 *Time.deltaTime ;
            hitpos =  LockUtil.RotateAngle(
                 this.ackPos.position, this.ackPos.position + initLaserEndPosDir * 15
                 , laserAngle);

            if (now_duration > attr.interval) { 
                if (skillCount != attr.num-1)
                    transform.Find("start").gameObject.SetActive(false);
            }
        }

        foreach (LineRenderer line in lineRendererList)
        {
            line.SetPosition(0, this.transform.InverseTransformPoint(this.ackPos.position));
            line.SetPosition(1, this.transform.InverseTransformPoint(hitpos));
        }

        exBox.hitPos = hitpos;
        
        //直接激光伤害
        RaycastHit2D[] hits = Physics2D.RaycastAll(this.ackPos.position,
            (hitpos - this.ackPos.position).normalized, 
            Vector2.Distance(hitpos,this.ackPos.position));

        HitInfo hitInfo = getHitInfo();

        foreach (RaycastHit2D hit in hits) {
            if (hit.transform.tag == "enemy") {
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (!hitEnemys.Contains(e)) {
                    //造成伤害判定
                    e.hurt(hitInfo);
                    hitEnemys.Add(e);
                }
            }
        }
    }

    void laserGenExExUpdate()
    {
        transform.position = hitPos;

        //超武地面痕迹不在造成伤害
        if (attr.level == 6)
            return;


        //印记伤害
        RaycastHit2D[] yj_hits = Physics2D.RaycastAll(startPos,
           (hitPos - startPos).normalized,
           Vector2.Distance(hitPos, startPos));

        HitInfo hitInfo = getHitInfo();

        foreach (RaycastHit2D hit in yj_hits)
        {
            if (hit.transform.tag == "enemy")
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (!hitEnemys.Contains(e))
                {
                    //造成伤害判定
                    e.hurt(hitInfo);
                    hitEnemys.Add(e);
                }
            }
        }
    }


    //链接光球特殊处理
    Transform laserTarget;

    void laserBall() {
        GameObject[] boxs = GameObject.FindGameObjectsWithTag("skillBox");
        List<BaseHitBox> laserBalls = new List<BaseHitBox>();

        //超武 进行球和球之间连线
        if (attr.level == 6) { 
            //找到除了自己之外的所有光球
            foreach (var box in boxs) {
                if (box.name.IndexOf("LaserBall") > -1 && box.gameObject != this.gameObject) {
                    laserBalls.Add(box.GetComponent<BaseHitBox>());
                }
            }

            foreach (BaseHitBox ball in laserBalls) {
                //生成相应的光线 并链接
                GameObject skillBox = Instantiate(exPf);
                skillBox.transform.position = initLaserEndPos;
                BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
                box.bs = this.bs;
                box.attr = this.exAttr;
                box.startPos = this.transform.position;
                box.hitPos = ball.transform.position;
                box.laserTarget = ball.transform;
            }
        }

        //和玩家之间连线
        if (Vector2.Distance(this.transform.position, bs.player.transform.position)<100) { 
            GameObject skillBox1 = Instantiate(exPf);
            skillBox1.transform.position = initLaserEndPos;
            BaseHitBox box1 = skillBox1.GetComponent<BaseHitBox>();
            box1.bs = this.bs;
            box1.attr = this.exAttr;
            box1.startPos = this.transform.position;
            box1.hitPos = bs.player.transform.position;
            box1.laserTarget = bs.player.transform;
        }
    }

    void laserBallEx() {

        if (laserTarget == null) {
            return;
        }

        foreach (LineRenderer line in lineRendererList)
        {
            line.SetPosition(0, this.transform.InverseTransformPoint(startPos));
            line.SetPosition(1, this.transform.InverseTransformPoint(laserTarget.position));
        }



        RaycastHit2D[] hits = Physics2D.RaycastAll(startPos,
          (laserTarget.position - startPos).normalized,
          Vector2.Distance(laserTarget.position, startPos));

        HitInfo hitInfo = getHitInfo();

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.tag == "enemy")
            {
                Enemy e = hit.transform.GetComponent<Enemy>();
                if (!hitEnemys.Contains(e))
                {
                    //造成伤害判定
                    e.hurt(hitInfo);
                    hitEnemys.Add(e);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {

        if (this.now_duration < attr.startCheckTime)
            return;


        if (attr.skillType == "Shield" && Collider.gameObject.tag == "enemySkillBox" )
        {
            Destroy(Collider.gameObject);
            dmgCount_now++;
            if (dmgCount_now >= attr.dmgCount) { 
                bs.endFlag = true;
                bs.player.Shield = null;
                bs.player.es_now = 0;
                Destroy(this.gameObject);
            }
        }

        if (Collider.gameObject.tag == "enemy")
        {
            Enemy e = Collider.gameObject.GetComponent<Enemy>();
            if (!hitEnemys.Contains(e) || (attr.dmgCount > 1 || attr.dmgCount == -1))
            {
                dmgCount_now++;

                //造成伤害判定
                HitInfo hitInfo = getHitInfo();

                cy_dmg = e.hurt(hitInfo);

                //武士刀吸血效果
                if (this.bs.attr.skillType.IndexOf("Katana") !=-1
                    && RoleManager.Get().katanaVampire > 0) {
                    int hphf =  (int)(hitInfo.damage * RoleManager.Get().katanaVampire);
                    this.bs.player.hp_now += hphf;
                    this.bs.player.hp_now = Mathf.Min(this.bs.player.hp_now, this.bs.player.hp_max);
                }


                if (attr.pierceType == 1)
                {
                    if (attr.exTriggerType != null && attr.exTriggerType.IndexOf("destroy") > -1
                        && attr.exId != null && attr.exId.Length > 0)
                    {
                        StartCoroutine(creatEx());
                    }
                    Destroy(this.gameObject);
                }
                if (attr.pierceType == 2 && cy_dmg <= 0)
                {
                    if (attr.exTriggerType != null && attr.exTriggerType.IndexOf("destroy") > -1
                        && attr.exId != null && attr.exId.Length > 0)
                    {
                        StartCoroutine(creatEx());
                    }
                    Destroy(this.gameObject);
                }
                hitEnemys.Add(e);

                //护盾结束
                if (attr.skillType == "Shield" && dmgCount_now >= attr.dmgCount)
                {
                    bs.endFlag = true;
                    bs.player.Shield = null;
                    bs.player.es_now = 0;
                    Destroy(this.gameObject);
                }
            }
        }

        if (Collider.gameObject.tag == "enemySkillBox" && attr.pierceType == 3) {
            Destroy(Collider.gameObject);
        }
    }

    IEnumerator creatEx() {
        //制造相应的衍生物
        int angle = 0;
        int rotate = 0;

        int count = exAttr.getNum();
        angle = exAttr.angle;
        if (count>1 && count % 2 != 0)
        {
            rotate = -angle * (count / 2);
        }
        Transform enemyTra = LockUtil.lockTarget(transform, exAttr.getRange()+1);
        for (int i = 0; i < count; i++) {
            GameObject skillBox = Instantiate(exPf);
            skillBox.transform.position = this.transform.position;
            BaseHitBox box = skillBox.GetComponent<BaseHitBox>();
            box.attr = this.exAttr;

            Vector3 start = transform.position;
            if (ackPos != null)
            {
                box.transform.position = ackPos.position;
                start = ackPos.position;
            }
            var dir = Vector3.zero;
           
            if (enemyTra != null)
                dir = (enemyTra.position - start).normalized;
            else
                dir = new Vector2(bs.player.facingDirection, 0);

            if(exAttr.ackType.IndexOf("random")!=-1)
                dir = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f, 1.0f));

            dir = (Quaternion.AngleAxis(rotate, new Vector3(0, 0, 1)) * dir).normalized;
            rotate += angle;
            box.bs = this.bs;
            box.flyDir = dir;
            box.startPos = transform.position;

            if (exAttr.ackType == "point") {
                if (enemyTra != null)
                    box.transform.position = enemyTra.position;
                else
                    box.transform.position = transform.position +
                        new Vector3(Random.Range(-exAttr.getRange(), exAttr.getRange()),
                                    Random.Range(-exAttr.getRange(), exAttr.getRange()), 0);
            }

            if (attr.skillType.IndexOf("LD") != -1 && attr.level == 6)
            {
                float x = Random.value > 0.5 ? 8 : -8;
                float y = Random.value > 0.5 ? 18 : -18;

                //屏幕的四个角落之一为起点
                skillBox.transform.position = bs.player.transform.position
                    + new Vector3(x, y);
                box.flyDir = new Vector3(-x, -y).normalized;
                transform.Find("spine").gameObject.SetActive(false);
                box.parentBs = this;
            }

            if (box.attr.ackType == "bullet")
            {
                float boxAngle = Vector2.Angle(transform.up, box.flyDir);
                box.transform.localEulerAngles
                    = new Vector3(0, 0, (float)boxAngle * (box.flyDir.x > 0 ? -1 : 1));

                if (attr.skillType.IndexOf("LD") != -1 && attr.level == 6) {
                    Transform DragonSprite = skillBox.transform.Find("大龙");
                    DragonSprite.localPosition =
                        skillBox.transform.InverseTransformPoint(skillBox.transform.position + new Vector3(0, 2, 0));
   
                    DragonSprite.localEulerAngles
                        = new Vector3(0, 0, -box.transform.localEulerAngles.z);
                    DragonSprite.localScale = new Vector3((box.flyDir.x < 0 ? 1 : -1) *
                        Mathf.Abs(DragonSprite.localScale.x),
                        DragonSprite.localScale.y);
                }
            }


            box.skillCount = i;
            box.ackPos = this.ackPos;
            if(enemyTra != null)
                box.pointVec = enemyTra.position;

            yield return new WaitForSeconds(exAttr.getInterval());
        }

        //龙蛋小火球音效处理
        //播放音效
        if (exAttr.pfPath== "Firebomb") 
        { 
            if (_audioSource == null)
            {
                _audioSource = this.gameObject.AddComponent<AudioSource>();
                _laserSoundClip = Resources.Load<AudioClip>("skill/audio/火球");
            }
            _audioSource.PlayOneShot(_laserSoundClip);
        }
    }
    AudioSource _audioSource;
    AudioClip _laserSoundClip;

    //该box的剩余伤害
    int cy_dmg = 0;

    HitInfo getHitInfo() {
        HitInfo info = new HitInfo();
        info.skillType = bs.attr.skillType;
        info.damage = (int)Mathf.Floor((this.attr.getDamage() + dmg_additional) * dmgUp);
        info.surplusDmg = cy_dmg;
       
        info.hitPos = this.transform.position;


        if (this.attr.stiffType == "牵引") {
            if (attr.flyDuration > 0 && now_duration > attr.flyDuration) {
                info.stiffTime = this.attr.stiffTime;
                info.stiffForce = this.attr.stiffForce;
            }
        }
        else { 
            info.stiffTime = this.attr.stiffTime;
            info.stiffForce = this.attr.stiffForce;
        }

        if (this.bs.attr.skillType.IndexOf("Katana") != -1) {
            if (RoleManager.Get().KatanaRepel == 0)
            {
                info.stiffTime = 0;
                info.stiffForce = 0;
            }
        }

        info.hitType = this.attr.stiffType == null ? "" : this.attr.stiffType;
        info.notStiffCover = this.attr.notStiffCover;

        //只有主武器可以触发这些特效
        if (bs.mainWeapon) { 
            //计算特效几率
            float rv = Random.value;

            //触发闪击效果 todo
            if (rv < RoleManager.Get().electricShockProbability ) {
                info.electricShockFlag = true;
                info.electricShockNum = RoleManager.Get().electricShockNum;
                info.electricDmg = (int)(RoleManager.Get().electricDmg);
            }
            //触发击杀爆炸


            //触发诅咒
            if (rv < RoleManager.Get().curseProbability)
            {
                info.curseFlag = true;
                info.cureseDelay = RoleManager.Get().cureseDelay;
                info.curseDmg = (int)(info.damage * RoleManager.Get().curseDmg);
            }

            //触发斩杀
            if (rv < RoleManager.Get().executeProbability)
            {
                info.executeFlag = true;
                info.executeHp = RoleManager.Get().executeHp;
            }

            //触发暴击
            if (rv < RoleManager.Get().critProbability)
            {
                info.critFlag = true;
                info.critDmg = RoleManager.Get().critDmg;
            }

            //触发燃烧
            if (rv < RoleManager.Get().burnProbability)
            {
                info.burnFlag = true;
                info.burnTime = RoleManager.Get().burnTime;
                info.burnInterval = RoleManager.Get().burnInterval;
                info.burnDmg = (int)(info.damage * RoleManager.Get().burnDmg);
            }

            //触发冰冻
            if (rv < RoleManager.Get().frozenProbability)
            {
                info.frozenFlag = true;
                info.frozenTime = RoleManager.Get().frozenTime;
            }
        }

        //造成流血
        if (attr.skillType== "Katana" && dmg_additional > 0) {
            info.bleedFlag = true;
            info.bleedTime = float.Parse(attr.exclusiveValue["bleedTime"].ToString());
            info.bleedInterval = float.Parse(attr.exclusiveValue["bleedInterval"].ToString());
            info.bleedDmg = (int)(info.damage * float.Parse(attr.exclusiveValue["bleedDmg"].ToString()));
        }

        return info;
    }

    int getPositiveOrNegative() {
        return (Random.value < 0.5f ? -1 : 1);
    }

    private void OnDestroy()
    {

        if (attr.skillType == "LaserGenEx")
        {
            if (attr.level == 6)
                Destroy(exBox.gameObject);
            else
            {
                if(exBox!=null)
                    exBox.transform.Find("hit").gameObject.SetActive(false);
            }
        }
    }
}



