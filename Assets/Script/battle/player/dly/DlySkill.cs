using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DlySkill : MonoBehaviour
{
    public Player player;
    public SkillAttr dlyAtr;
    public static bool dlyIngFlag;
    public static bool dlyReadyEndFlag;
    public static bool dlyReadyFlag;

    GameObject dlyMask;
    SpriteRenderer boomMask;
    GameObject jy;

    GameObject skillBoxPf;
    GameObject skillBox;
    //刀光拖尾
    TrailRenderer tr;

    float maxDuration = 3;
    public float nowDuration;

    Slider dlyUI;
    GameObject dly_Ready;
    Button playerHead;

    private void Awake()
    {
        dlyMask =  GameObject.Find("Main Camera").transform.Find("dlyMask").gameObject;
        boomMask = GameObject.Find("Main Camera").transform.Find("boomMask").GetComponent<SpriteRenderer>();
        dly_Ready = GameObject.Find("lz_Canvas").transform.Find("dly_Ready").gameObject;
        dly_Ready.SetActive(false);

        //playerHead = GameObject.Find("playerHead").GetComponent<Button>();
        //playerHead.interactable = false;
        //playerHead.onClick.AddListener(dlyAckStart);

        dlyMask.SetActive(false);

        if (DungeonManager.zb_mode==0)
            jy = GameObject.Find("Joystick-Left");

        dlyUI = GameObject.Find("Canvas").transform.Find("dlyUI").GetComponent<Slider>();

        player = GetComponent<Player>();

        dlyIngFlag = false;
        dlyReadyEndFlag = false;
        dlyReadyFlag = false;

        qualifled = GameObject.Find("Canvas").transform.Find("dlyScore").Find("qualifled");
        excellent = GameObject.Find("Canvas").transform.Find("dlyScore").Find("excellent");
        perfect = GameObject.Find("Canvas").transform.Find("dlyScore").Find("perfect");


        skillBoxPf = Resources.Load<GameObject>("skill/dly/KatanaDly");

    }

    private void Start()
    {
        //dlyAckStart();
    }

    public void init() {
        //todo  区分不同dly
        if (true)
        {
            skillBoxPf = Resources.Load<GameObject>("skill/dly/KatanaDly"/* + dlyAtr.pfPath*/);
            //skillBox = GameObject.Find("KatanaDly");
            //tr = skillBox.GetComponent<TrailRenderer>();
        }
    }

    private void Update()
    {
        if (dlyIngFlag)
            Katana_dlyAckUpdate();
    }

    public void dlyReady() {
        //dlyReadyFlag = true;
        //dly_Ready.SetActive(true);
        //todo 原逻辑
        //playerHead.interactable = true;
    }


    public void dlyAckStart() {
        //dly_Ready.SetActive(false);
        //playerHead.interactable = false;
        //player.idle_dlyEsNow = 0;
        //player.move_dlyEsNow = 0;
        //player.addDlyEs(0);
        //dlyReadyEndFlag = true;
        //player.hideFlag = true;
        //player.ctr.animator_move.gameObject.SetActive(false);
        //player.ctr.animator_idle.gameObject.SetActive(true);
        //player.ctr.animator_idle.Play("baoqi");




        //StartCoroutine(dlyStart());
        obstaclesCount = 0;

        //开启遮罩
        dlyMask.SetActive(true);
        //延缓时间
        GameSceneManage.nowTimeScale = 0.1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.002f;

        //隐藏摇杆ui
        if (DungeonManager.zb_mode == 0)
            jy.SetActive(false);
        dlyUI.gameObject.SetActive(true);
        dlyUI.value = 1;
        dlyReadyEndFlag = false;
        dlyIngFlag = true;
        nowDuration = dlyAtr.duration;
        creatObstacleLine();
    }


    IEnumerator dlyStart()
    {
        obstaclesCount = 0;
        yield return new WaitForSeconds(0/*0.7f*/);
        //开启遮罩
        dlyMask.SetActive(true);

        //延缓时间
        GameSceneManage.nowTimeScale = 0.1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.002f;

        //隐藏摇杆ui
        jy.SetActive(false);

        dlyUI.gameObject.SetActive(true);
        dlyUI.value = 1;
        dlyReadyEndFlag = false;
        dlyIngFlag = true;

        //todo 测试 dlyAtr.duration = 20;

        nowDuration = dlyAtr.duration;

        //player.hide();
        creatObstacleLine();
    }

    public void creatObstacleLine() {
        startIndex = -1;
        nextIndex = -1;

        //找到屏幕里所有障碍物 依次连线
        //GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        foreach (var obstacle in DungeonManager.nowObstacleList)
        {
            //Obstacle obs = obstacle.GetComponent<Obstacle>();
            obstacle.creatLine();
        }
    }

    public void dlyAckEnd()
    {
        //player.show();

        //关闭遮罩
        dlyMask.SetActive(false);

        //延缓时间
        GameSceneManage.nowTimeScale = 1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.02f;

        //隐藏摇杆ui
        if (DungeonManager.zb_mode == 0)
            jy.SetActive(true);

        dlyUI.gameObject.SetActive(false);

        dlyIngFlag = false;

        nowDuration = 0;

        if(skillBox!=null)
            Destroy(skillBox);
    }

    //滑过的障碍物数量
    int obstaclesCount;
    //开始滑的障碍物角标
    public int startIndex;
    //下一个可滑的障碍物角标
    public int nextIndex;

    //--------障碍物评价相关
    Transform qualifled;
    Transform excellent;
    Transform perfect;



    //刀光类x   指滑障碍物  
    public void Katana_dlyAckUpdate() {

        nowDuration -= Time.deltaTime * 10;
        dlyUI.value = nowDuration / dlyAtr.duration;
        if (nowDuration <= 0) {
            int maxNum = 0;
            //找到所有被激活的障碍物 进行删除
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
            foreach (var obstacle in obstacles) {
                Obstacle o = obstacle.GetComponent<Obstacle>();
                o.hideLine();
                if (o.activeFlag) {
                    maxNum = o.maxNum;
                    obstaclesCount++;
                    Destroy(o.gameObject);
                }
            }

            if (obstaclesCount > 0) {
                //根据obstaclesCount造成一次全屏伤害
                ObstacleAttr atr = obstacles[0].GetComponent<Obstacle>().attr;
                int dmg = (int)(atr.dmg * obstaclesCount * atr.rate * obstaclesCount);
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
                foreach (var e in enemys)
                {
                    e.GetComponent<Enemy>().hurt(dmg);
                }
                //闪白屏
                boomMask.gameObject.SetActive(true);
                //根据数量弹出评价
                
                if (obstaclesCount == maxNum)
                {
                    perfect.gameObject.SetActive(true);
                }
                else if ((obstaclesCount+0.0f) / maxNum < 0.5f) 
                {
                    qualifled.gameObject.SetActive(true);
                }
                else
                {
                    excellent.gameObject.SetActive(true);
                }
            }
            dlyAckEnd();
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            skillBox = Instantiate(skillBoxPf);
            DlySkillBox dsb = skillBox.GetComponent<DlySkillBox>();
            dsb.attack = dlyAtr.getDamage();
            dsb.dlyskill = this;

            //skillBox.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            Destroy(skillBox);
        }
        if(skillBox != null)
            skillBox.transform.position = Camera.main.ScreenToWorldPoint(
               new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

    }

    public void dlyAckUpdate2()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            skillBox.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            skillBox.SetActive(false);
        }

        //跟随手指向量 而不是直接等于  有移动速度限制

        skillBox.transform.position += (Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)
            ) - skillBox.transform.position).normalized * 100 * Time.deltaTime;
    }

}


