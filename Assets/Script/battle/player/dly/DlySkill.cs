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
    //������β
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
        //todo  ���ֲ�ͬdly
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
        //todo ԭ�߼�
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

        //��������
        dlyMask.SetActive(true);
        //�ӻ�ʱ��
        GameSceneManage.nowTimeScale = 0.1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.002f;

        //����ҡ��ui
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
        //��������
        dlyMask.SetActive(true);

        //�ӻ�ʱ��
        GameSceneManage.nowTimeScale = 0.1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.002f;

        //����ҡ��ui
        jy.SetActive(false);

        dlyUI.gameObject.SetActive(true);
        dlyUI.value = 1;
        dlyReadyEndFlag = false;
        dlyIngFlag = true;

        //todo ���� dlyAtr.duration = 20;

        nowDuration = dlyAtr.duration;

        //player.hide();
        creatObstacleLine();
    }

    public void creatObstacleLine() {
        startIndex = -1;
        nextIndex = -1;

        //�ҵ���Ļ�������ϰ��� ��������
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

        //�ر�����
        dlyMask.SetActive(false);

        //�ӻ�ʱ��
        GameSceneManage.nowTimeScale = 1f;
        Time.timeScale = GameSceneManage.nowTimeScale;
        Time.fixedDeltaTime = 0.02f;

        //����ҡ��ui
        if (DungeonManager.zb_mode == 0)
            jy.SetActive(true);

        dlyUI.gameObject.SetActive(false);

        dlyIngFlag = false;

        nowDuration = 0;

        if(skillBox!=null)
            Destroy(skillBox);
    }

    //�������ϰ�������
    int obstaclesCount;
    //��ʼ�����ϰ���Ǳ�
    public int startIndex;
    //��һ���ɻ����ϰ���Ǳ�
    public int nextIndex;

    //--------�ϰ����������
    Transform qualifled;
    Transform excellent;
    Transform perfect;



    //������x   ָ���ϰ���  
    public void Katana_dlyAckUpdate() {

        nowDuration -= Time.deltaTime * 10;
        dlyUI.value = nowDuration / dlyAtr.duration;
        if (nowDuration <= 0) {
            int maxNum = 0;
            //�ҵ����б�������ϰ��� ����ɾ��
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
                //����obstaclesCount���һ��ȫ���˺�
                ObstacleAttr atr = obstacles[0].GetComponent<Obstacle>().attr;
                int dmg = (int)(atr.dmg * obstaclesCount * atr.rate * obstaclesCount);
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
                foreach (var e in enemys)
                {
                    e.GetComponent<Enemy>().hurt(dmg);
                }
                //������
                boomMask.gameObject.SetActive(true);
                //����������������
                
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

        //������ָ���� ������ֱ�ӵ���  ���ƶ��ٶ�����

        skillBox.transform.position += (Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)
            ) - skillBox.transform.position).normalized * 100 * Time.deltaTime;
    }

}


