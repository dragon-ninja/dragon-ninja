using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{

    public SkillAttr atr;

    LayerMask playerMask;
    public Enemy ey;
    
    //当前冷却
    float skillCd_now = 0;
    //当前蓄力时间
    float prepareTime_now;
    //当前释放时间
    float releaseTime_now;
    //目标向量
    Vector3 targetVec;
    //目标坐标
    Vector3 targetPos;

    //蓄力准备中
    bool prepareIngflag;
    //蓄力准备结束
    bool prepareEndFlag;
    //释放中
    bool releaseFlag;
    //技能指示器
    Indicator indicator;

    //技能主体
    bool skillMain;

    public void init() {
        playerMask = 1 << LayerMask.NameToLayer("player");
        skillMain = true;
    }

    public void FixedUpdate()
    {
        //子类地刺调用自身更新
        if (atr.skillType == "lurker" && !skillMain)
            lurkerSkillUpdate();


        //调用子类更新
        if (childList.Count > 0)
        {
            foreach (EnemySkill es in childList)
            {
                es.FixedUpdate();
            }
        }
    }

    public void skillStart()
    {
        if (indicator == null)
        {
            if(atr.indicatorType != null && atr.indicatorType.IndexOf("Circle")>-1)
                indicator = 
                    Instantiate(Resources.Load<GameObject>("indicator/Circle")).GetComponent<Indicator>();

            if (atr.indicatorType != null 
                && atr.indicatorType.IndexOf("Square")>-1)
            {
                indicator = 
                    Instantiate(Resources.Load<GameObject>("indicator/Square")).GetComponent<Indicator>();
            }
        }

        ey.skillIng = true;
        ey.notAction = true;
        //skillCd_now = cd;
        prepareIngflag = true;
        prepareEndFlag = false;
        releaseFlag = false;

        prepareTime_now = 0;
        releaseTime_now = 0;

        targetPos = ey.target.position;

        if (atr.indicatorType != null) { 
            if (atr.indicatorType.IndexOf("follow") > -1)
            {
                indicator.transform.parent = ey.transform;
                indicator.transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                indicator.transform.position = targetPos;
            }
            //indicator.gameObject.SetActive(true);
        }

        //todo 这里播放动画更合适
        if (atr.skillType == "drop") { 
            ey.transform.position = targetPos + new Vector3(0, 10, 0);
            ey.hide();
        }

        if (atr.skillType == "lurker") {
            //ey.notAction = true;
            //ey.skillIng = true;
            ey.notAction = false;
            if (skillMain)
            {
                indicator.hide();
                StartCoroutine(lurkerSkillAttack());
            }
        }
    }

    public void skillUpdate() {
        /*if (!prepareIngflag && !releaseFlag && skillCd_now >= 0)
        {
            skillCd_now -= Time.deltaTime;
            if (skillCd_now <= 0)
            {
                skillStart();
            }
        }*/

        if (atr.skillType == "charge") 
            chargeUpdate();

        if (atr.skillType == "drop")
            dropSkillUpdate();

        if (atr.skillType == "smash")
            smashSkillUpdate();

        if (atr.skillType == "laser")
            laserSkillUpdate();

        
    }


    #region 冲锋技能
    void chargeUpdate()
    {
        if (prepareIngflag)
        {
            //开始蓄力
            if (prepareTime_now < atr.prepareTime_max)
            {

                prepareTime_now += Time.deltaTime;
                //chargePrepareDis_now = prepareTime_now / prepareTime_max * skillBoxY_max;
                targetVec = (ey.target.position - ey.transform.position).normalized;

                float boxAngle = Vector2.Angle(new Vector3(0,1,0), targetVec) 
                    * (targetVec.x > 0 ? -1 : 1);
                indicator.transform.localEulerAngles 
                    = new Vector3(0, 0,(float)boxAngle
                    * (ey.transform.localScale.x > 0 ? 1 : -1));
                indicator.showSquare(prepareTime_now / atr.prepareTime_max,
                    atr.boxMaxX, atr.boxMaxY);

            }
            //蓄力完成 ~ 冲锋  缓冲阶段
            else if (prepareTime_now < atr.prepareTime_max + atr.prepareEndDelay)
            {
                //todo 是否有必要?
                if (!prepareEndFlag)
                {
                    //todo 特效处理  动画处理...
                }
                prepareTime_now += Time.deltaTime;
                prepareEndFlag = true;
            }
            //开始冲锋
            else
            {
                indicator.hide();
                //开始charge...
                prepareIngflag = false;
                releaseFlag = true;
            }
        }

        if (releaseFlag)
        {
            if (releaseTime_now < atr.duration)
            {
                releaseTime_now += Time.deltaTime;

                ey.transform.position +=
                    targetVec * Time.deltaTime * atr.speed;
            }
            else
            {
                skillEnd();
                ey.notAction = false;
            }
        }
    }

    #endregion

    #region 砸地技能
    void smashSkillUpdate()
    {
        if (prepareIngflag)
        {
            //开始蓄力   指示器放大
            if (prepareTime_now < atr.prepareTime_max)
            {
                prepareTime_now += Time.deltaTime;
                indicator.showCircle(prepareTime_now / atr.prepareTime_max,
                    atr.boxMaxX, atr.boxMaxY);

            }
            //蓄力完成 ~  缓冲阶段
            else if (prepareTime_now < atr.prepareTime_max + atr.prepareEndDelay)
            {
                if (!prepareEndFlag)
                {
                    indicator.hide();
                    //todo 特效处理  动画处理...
                }
                prepareTime_now += Time.deltaTime;
                prepareEndFlag = true;
            }
            else
            {
                indicator.hide();
                skillEnd();
                ey.notAction = false;

                //计算判定
                Collider2D c = Physics2D.OverlapCircle
                    (ey.transform.position, atr.boxMaxX / 2.0f, playerMask);

                if (c != null)
                {
                    HitInfo ht = new HitInfo();
                    ht.hitPos = transform.position;
                    ht.damage = atr.attack;
                    c.GetComponent<Player>().hurt(ht);
                }

                //生成障碍物
            }
        }
    }
    #endregion

    #region 坠落技能

    void dropSkillUpdate()
    {
        if (prepareIngflag)
        {
            //开始蓄力   指示器放大
            if (prepareTime_now < atr.prepareTime_max)
            {
                prepareTime_now += Time.deltaTime;
                
                indicator.showCircle(prepareTime_now / atr.prepareTime_max,
                    atr.boxMaxX, atr.boxMaxY);

            }
            //蓄力完成 ~  缓冲阶段
            else if (prepareTime_now < atr.prepareTime_max + atr.prepareEndDelay)
            {
                if (!prepareEndFlag)
                {
                    //todo 特效处理  动画处理...
                }
                prepareTime_now += Time.deltaTime;
                prepareEndFlag = true;
            }
            //开始下坠
            else
            {
                ey.spriteTra.gameObject.SetActive(true);

                indicator.hide();
                //todo 播放坠落动画

                //开始charge...
                prepareIngflag = false;
                releaseFlag = true;
            }
        }


        //播放落地过程
        if (releaseFlag)
        {
            if (ey.transform.position.y > targetPos.y)
            {
                ey.transform.position +=
                    new Vector3(0, -1, 0) * Time.deltaTime * atr.speed;
            }
            else {
                //到达地面 完全重新激活boss
                ey.show();

                skillEnd();

                ey.notAction = false;


                //计算判定

                Collider2D c = Physics2D.OverlapCircle
                    (ey.transform.position, atr.boxMaxX / 2.0f, playerMask);

                if (c != null)
                {
                    HitInfo ht = new HitInfo();
                    ht.hitPos = transform.position;
                    ht.damage = atr.attack;
                    c.GetComponent<Player>().hurt(ht);
                }
            }
            

        }
    }

    #endregion

    #region 激光技能

    List<LineRenderer> lineList = new List<LineRenderer>();
    List<bool> lineDmgList = new List<bool>();

    void laserSkillUpdate()
    {
        if (prepareIngflag)
        {
            //开始蓄力   指示器放大
            if (prepareTime_now < atr.prepareTime_max)
            {
                prepareTime_now += Time.deltaTime;

                indicator.showCircle(prepareTime_now / atr.prepareTime_max,
                    atr.boxMaxX, atr.boxMaxY);

            }
            //蓄力完成 ~  缓冲阶段
            else if (prepareTime_now < atr.prepareTime_max + atr.prepareEndDelay)
            {
                if (!prepareEndFlag)
                {
                    //todo 特效处理  动画处理...
                }
                prepareTime_now += Time.deltaTime;
                prepareEndFlag = true;
            }
            else
            {
                prepareIngflag = false;
                releaseFlag = true;
                if (lineList.Count < 2) { 
                    lineList.Add(Instantiate(Resources.Load<GameObject>
                        ("skill/enemy/BossLaser")).GetComponent<LineRenderer>());
                    lineList.Add(Instantiate(Resources.Load<GameObject>
                        ("skill/enemy/BossLaser")).GetComponent<LineRenderer>());

                    lineDmgList.Add(false);
                    lineDmgList.Add(false);
                }

                lineDmgList[0] = false;
                lineDmgList[1] = false;
            }
        }

        //激光:
        if (releaseFlag)
        {
            if (releaseTime_now < atr.duration)
            {
                float f = 1 - (atr.duration - releaseTime_now)/ atr.duration;

                lineList[0].gameObject.SetActive(true);
                releaseTime_now += Time.deltaTime;
                lineList[0].SetPosition(0, ey.transform.position);
                Vector2 endPos = ey.transform.position +
                    new Vector3(-30 + (30 * f) ,-50,0);
                lineList[0].SetPosition(1, endPos);


                lineList[1].gameObject.SetActive(true);
                releaseTime_now += Time.deltaTime;
                lineList[1].SetPosition(0, ey.transform.position);
                Vector2 endPos2 = ey.transform.position +
                    new Vector3(30 + (-30 * f), -50, 0);
                lineList[1].SetPosition(1, endPos2);


                if (!lineDmgList[0]) { 
                    RaycastHit2D c = Physics2D.Linecast
                       (ey.transform.position, endPos,playerMask);
                    if (c)
                    {
                        HitInfo ht = new HitInfo();
                        ht.hitPos = transform.position;
                        ht.damage = atr.attack;
                        c.collider.GetComponent<Player>().hurt(ht);
                        lineDmgList[0] = true;
                    }
                }

                if (!lineDmgList[1]) { 
                    RaycastHit2D c2 = Physics2D.Linecast
                       (ey.transform.position, endPos2, playerMask);
                    if (c2)
                    {
                        HitInfo ht = new HitInfo();
                        ht.hitPos = transform.position;
                        ht.damage = atr.attack;
                        c2.collider.GetComponent<Player>().hurt(ht);
                        lineDmgList[1] = true;
                    }
                }

            }
            else
            {
                skillEnd();
                ey.notAction = false;
                lineList[0].gameObject.SetActive(false);
                lineList[1].gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region 地刺技能

    //public List<float> prepareTime_nowList = new List<float>();
    //public List<Indicator> indicatorList = new List<Indicator>();

    void lurkerSkillUpdate()
    {
        if (prepareIngflag)
        {
            //开始蓄力   指示器放大
            if (prepareTime_now < atr.prepareTime_max)
            {
                prepareTime_now += Time.deltaTime;
                indicator.showCircle(prepareTime_now / atr.prepareTime_max,
                    atr.boxMaxX, atr.boxMaxY);

            }
            //蓄力完成 ~  缓冲阶段
            else if (prepareTime_now < atr.prepareTime_max + atr.prepareEndDelay)
            {
                if (!prepareEndFlag)
                {
                    indicator.hide();
                    //todo 特效处理  动画处理...
                }
                prepareTime_now += Time.deltaTime;
                prepareEndFlag = true;
            }
            else
            {
                indicator.hide();
                prepareIngflag = false;
                releaseFlag = false;

                //计算判定
                Collider2D c = Physics2D.OverlapCircle
                    (targetPos, atr.boxMaxX / 2.0f, playerMask);

                if (c != null)
                {
                    HitInfo ht = new HitInfo();
                    Debug.Log(targetPos);
                    ht.hitPos = targetPos;
                    ht.damage = atr.attack;

                    c.GetComponent<Player>().hurt(ht);
                }
            }
        }
    }

    List<EnemySkill> childList = new List<EnemySkill>();

    IEnumerator lurkerSkillAttack() {
        //todo 待优化
        childList.Clear();
        for (int i = 0; i < 3; i++) {
            EnemySkill es = new EnemySkill();
            es.init();
            es.skillMain = false;
            es.ey = this.ey;
            es.atr = this.atr;
            es.skillStart();
            childList.Add(es);
            yield return new WaitForSeconds(0.35f);
        }
        ey.notAction = false;
        skillEnd();
    }

    #endregion

    void skillEnd() {
        prepareIngflag = false;
        releaseFlag = false;
        ey.skillIng = false;
    }
}


