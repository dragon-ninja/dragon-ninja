using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Com.LuisPedroFonseca.ProCamera2D;

public class DungeonManager : MonoBehaviour
{

    public int zbMode = 0;
    [SerializeField]
    public static int zb_mode = 0;

    static GameSceneManage gmr;
    public static SysSettingFactory SysSetting;

    SelectRelicManager selectRelicManager;
    UpLevel upLevel;
    public static int selectRelicNum = 1;
    public static int upLevelNum = 2;
    //击杀精英后立即弹出的三选一relic
    public static int jySelectRelicNum = 0;
    bool endUpStartFlag;
    bool endUpFlag;

    //public DungeonFactory df;
    //public TowerFactory towerf;
    EnemyFactory ef;
    ObstacleFactory obstaclef;
    public string dungeonId;
    SettlementPanel settlementPanel;

    //本次执行的关卡数据
    List<DungeonInfoConfig> infoList;

    public static int killNum;
    public static int goldNum;


    float nowTime;

    //执行中的事件
    List<EventData> nowEventList = new List<EventData>();

    int nowIndex = 0;

    public GameObject expCrystalPf;
    public GameObject expCrystalPf_2;

    public static int duration;
    public static Player player;
    //对象池
    public static List<expCrystalfb> expPoolList = new List<expCrystalfb>();
    //场景中的经验
    public static List<expCrystalfb> expPoolList_cj = new List<expCrystalfb>();
    public static GameObject magnetPf;
    public static GameObject bombPf;
    public static GameObject luckyBoxPf;
    public static GameObject dlyPropPf;

   

    float bossCountdown_now;
    EventData bossEd_now;
    public static GameObject enemyGroupTip;
    public static GameObject bossTip;
    public static Slider bossHP;
    static int bossIndex;
    //玩家传送到boss地图之前的坐标点
    static Vector3 playerSavePos;

    float tipTime;
    TextMeshProUGUI towerText;
    TextMeshProUGUI timeText;
    TextMeshProUGUI killText;
    TextMeshProUGUI KillBossText;
    TextMeshProUGUI goldText;

    Slider progressUi;
    TextMeshProUGUI progressTextUi;

    int defaultCameraSize = 18;
    int enemyGroupCameraSize = 20;
    int CameraSize_now;

    bool enemyGroupFlag;
    bool enemyGroupEndFlag;

    int maxEnemyNum = 500;
    Dictionary<string, GameObject> enemyPfMap;
    public Dictionary<string, List<Enemy>> enemyPoolMap;
    public static int nowEnemyNum = 0;
    public static float dlyCameraSizeUp = 1.5f;
    public static GameObject JY_HP_UI_Pf;
    public static bool superAttackReadyFlag;
    public static bool superAttackReadyEndFlag;

    NewSelectRelicManager NewSelectRelic;

    bool awakeFlag;

    private void Awake()
    {
        DungeonManager.zb_mode = zbMode;

        expPoolList = new List<expCrystalfb>();
        expPoolList_cj = new List<expCrystalfb>();
        bossIndex = 0;
        TimeOrKill = 0;
        gameEndFlag = false;
        killNum = 0;
        goldNum = 0;
        superAttackReadyFlag = false;
        superAttackReadyEndFlag = false;
        Application.targetFrameRate = 120;
        gmr = GetComponent<GameSceneManage>();
        nowEnemyNum = 0;
        nowTime = 0;
        duration = 0;
        Time.timeScale = 1;

        selectRelicManager = GameObject.Find("Canvas").transform.Find("selet_relic").GetComponent<SelectRelicManager>();
        upLevel = GameObject.Find("Canvas").transform.Find("up_level").GetComponent<UpLevel>(); ;

        NewSelectRelic = GameObject.Find("Canvas").transform.Find("NewSelectRelic").GetComponent<NewSelectRelicManager>();
        //NewSelectRelic.SetActive(true);
        SysSetting = Resources.Load<SysSettingFactory>("mode/sysSettingMode");
        SysSetting.init();
        ef = Resources.Load<EnemyFactory>("mode/enemyMode");
        ef.init();



        obstaclef = Resources.Load<ObstacleFactory>("mode/obstacleMode");
        obstaclef.init();
        player = GameObject.Find("role").GetComponent<Player>();
        bossFlag = false;

        bossHP = GameObject.Find("Canvas").transform.Find("bossHP").GetComponent<Slider>();

        enemyGroupTip = GameObject.Find("Canvas").transform.Find("怪潮来袭").gameObject;
        bossTip = GameObject.Find("Canvas").transform.Find("boss来袭").gameObject;
        towerText = GameObject.Find("Canvas").transform.Find("TimePanel").Find("Tower").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find("Canvas").transform.Find("TimePanel").Find("Time").GetComponent<TextMeshProUGUI>();
        killText = GameObject.Find("Canvas").transform.Find("TimePanel").Find("Kill").GetComponent<TextMeshProUGUI>();
        KillBossText = GameObject.Find("Canvas").transform.Find("TimePanel").Find("KillBoss").GetComponent<TextMeshProUGUI>();
        goldText = GameObject.Find("Canvas").transform.Find("gold").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        expCrystalPf = Resources.Load<GameObject>("prop/expCrystal");
        expCrystalPf_2 = Resources.Load<GameObject>("prop/expCrystal_2");
        magnetPf = Resources.Load<GameObject>("prop/Magnet");
        bombPf = Resources.Load<GameObject>("prop/Bomb");
        luckyBoxPf = Resources.Load<GameObject>("prop/LuckyBox");
        dlyPropPf = Resources.Load<GameObject>("prop/dlyProp");

        progressUi = GameObject.Find("Canvas").transform.Find("progressUI").GetComponent<Slider>();
        progressTextUi = progressUi.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        settlementPanel = GameObject.Find("Canvas").transform.Find("SettlementPanel").GetComponent<SettlementPanel>();


        relicNumTra = GameObject.Find("relicNumTip").transform;
        relicNumGPf = relicNumTra.GetChild(0).gameObject;
        relicNumList = new List<GameObject>();
        for (int i = 0; i < relicNumTra.childCount; i++)
        {
            relicNumList.Add(relicNumTra.GetChild(i).gameObject);
            relicNumList[i].SetActive(false);
        }

        //DungeonDesc desc = df.descList[DungeonForm.DungeonIndex];
        //init(desc.id);


        //todo 爬塔改造
        transitionImg = GameObject.Find("Canvas").transform.Find("转场").GetComponent<Image>();
        transitionImg.gameObject.SetActive(false);
        transitionImg.color = new Color(0,0,0,0);


        if (TowerManager.DungeonId.Contains("."))
            TowerManager.DungeonId = TowerManager.DungeonId.Substring(0,TowerManager.DungeonId.IndexOf("."));
        //Debug.Log("============================----------------------------------TowerManager.DungeonId:"+ TowerManager.DungeonId);

        initTower(TowerManager.DungeonId);

        GameObject.Find("mainMap/").transform.Find("d001").gameObject.SetActive(true);
        GameObject.Find("bossMap1/").transform.Find("d001").gameObject.SetActive(true);
        GameObject.Find("bossMap2/").transform.Find("d001").gameObject.SetActive(true);
        GameObject.Find("bossMap3/").transform.Find("d001").gameObject.SetActive(true);
        boomMask = GameObject.Find("Main Camera").transform.Find("endMask").GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 适配世界摄像机 竖屏宽度适配
    /// </summary>
    public void MainCameraAdjust()
    {
        float ratio = GameSetting.Instance.Width * 1f / GameSetting.Instance.Height / (Screen.width * 1f / Screen.height);
        if (Camera.main.orthographic)
        {
            // Camera.main.orthographicSize = Mathf.Max(GameSetting.Instance.CameraAdjust, ratio * GameSetting.Instance.CameraAdjust);
            Camera.main.orthographicSize = (18 * ratio- 0.5f);
            ProCamera2D.Instance.UpdateScreenSize((18 * ratio - 0.5f));
            //ProCamera2D.Instance.Reset();
        }
        else
        {
           // Camera.main.fieldOfView = Mathf.Max(GameSetting.Instance.CameraAdjust, ratio * GameSetting.Instance.CameraAdjust);
        }
    }

    //当前章节数据
    TowerMap nowChapter;                                                                                                                                        
    public void initTower(string id)
    {
        //DungeonManager.jySelectRelicNum = 15;
           nowChapter = TowerFactory.Get().tmList.Find(x =>
        x.id == TowerFactory.Get().chapterList[DataManager.Get().now_chapterIndex]);

        string start = nowChapter.name.Substring(0,nowChapter.name.IndexOf("-"));
        string end = nowChapter.name.Substring(nowChapter.name.IndexOf("-") + 1);
        towerText.text = "Tower"+ (TowerManager.DungeonStorey+int.Parse(start)-1) + "/"+ end; 


        //地图样式
        GameObject.Find("mainMap/").transform.Find("d001").GetComponent<SpriteRenderer>().sprite =
           Resources.Load<Sprite>("image/map/" + nowChapter.mapImg);
        GameObject.Find("bossMap1/").transform.Find("d001").GetComponent<SpriteRenderer>().sprite =
           Resources.Load<Sprite>("image/map/" + nowChapter.mapImg);
        GameObject.Find("bossMap2/").transform.Find("d001").GetComponent<SpriteRenderer>().sprite =
           Resources.Load<Sprite>("image/map/" + nowChapter.mapImg);
        GameObject.Find("bossMap3/").transform.Find("d001").GetComponent<SpriteRenderer>().sprite =
           Resources.Load<Sprite>("image/map/" + nowChapter.mapImg);

        GameObject.Find("mainMap/").transform.Find("上下_box").gameObject.SetActive(false);
        GameObject.Find("mainMap/").transform.Find("无限_box").gameObject.SetActive(false);
        GameObject.Find("mainMap/").transform.Find("方形_box").gameObject.SetActive(false);

        Debug.Log("Camera.main.orthographicSize:"+Camera.main.orthographicSize);
        Debug.Log("Camera.main.sensorSize:" + Camera.main.sensorSize);
        Debug.Log("Camera.main.fieldOfView:" + Camera.main.fieldOfView);
        MainCameraAdjust();

        int v = Random.Range(0, 10);
        //v = 0;
        //地图边界
        if (v <= 3 && v > 0 /*nowChapter.boundType=="上下"*/) 
        {
            ProCamera2D.Instance.AdjustCameraTargetInfluence(player.transform,0,1);
            GameObject.Find("mainMap/").transform.Find("上下_box").gameObject.SetActive(true);
        }
        else if (v == 0/*nowChapter.boundType == "方形"*/)
        {
            ProCamera2D.Instance.AdjustCameraTargetInfluence(player.transform, 1, 1);
            GameObject.Find("mainMap/").transform.Find("方形_box").gameObject.SetActive(true);
        }
        else 
        {
            ProCamera2D.Instance.AdjustCameraTargetInfluence(player.transform, 1, 1);
            GameObject.Find("mainMap/").transform.Find("无限_box").gameObject.SetActive(true);
        }


        dungeonId = id;
        infoList = TowerFactory.Get().tdMap[dungeonId==""? "1001" : dungeonId];


       

        if (TowerManager.DungeonType == "普通") {
            //maxBattleTime =
            List<KeyValue> s = TowerFactory.Get().tmMap[nowChapter.id][TowerManager.DungeonStorey].commonEnd;
            maxBattleTime = s.Find(x => x.key == "time").value;
            maxKillNum = s.Find(x => x.key == "kill").value;
        }
        else if (TowerManager.DungeonType == "精英") {
            List<KeyValue> s = TowerFactory.Get().tmMap[nowChapter.id][TowerManager.DungeonStorey].eliteEnd;
            maxBattleTime = s.Find(x => x.key == "time").value;
            maxKillNum = s.Find(x => x.key == "kill").value;
        }


        enemyPfMap = new Dictionary<string, GameObject>();
        enemyPoolMap = new Dictionary<string, List<Enemy>>();
        expPoolList = new List<expCrystalfb>();
        expPoolList_cj = new List<expCrystalfb>();
        JY_HP_UI_Pf = GameObject.Find("FightCanvas").transform.Find("JY_HP_UI").gameObject;
        CameraSize_now = defaultCameraSize;
        bossIndex = 0;
        endUpStartFlag = false;

        //开局刷新一定经验值
       /* for (int i = 0; i < 10; i++)
        {
            expCrystalfb exp = creatExp();
            exp.transform.position = player.transform.position + new Vector3(
                    Random.Range(-7, 7), Random.Range(-10, 10), 0
                );
            exp.exp = 50;
        }*/

        PropFactory propFactory = Resources.Load<PropFactory>("mode/propMode");
        propFactory.init();

        /* if (infoList[infoList.Count - 1].type == "end")
            maxBattleTime = infoList[infoList.Count - 1].time;
        else {
            maxBattleTime = 100;
        }*/

       
        TransitionCountdown_now = TransitionCountdown;

        if (TowerManager.DungeonType == "boss") {

            //隐藏时间和进度条显示
            timeText.gameObject.SetActive(false);
            progressUi.gameObject.SetActive(false);
            //传送到boss地图
            bossIndex += 1;
            player.transform.position = new Vector2(1000 * bossIndex + 20000, 20000);
        }

        //事件增益
        upLevelNum = RoleManager.Get().battleEndSkillUp;
        selectRelicNum = 1;

        if (Random.value <= RoleManager.Get().relicNumUp) {
            selectRelicNum++;
        }
        TimeOrKill = Random.Range(0, 2);
        //TimeOrKill = 0;
        KillBossText.gameObject.SetActive(false);
        if (TimeOrKill == 0)
        {
            timeText.gameObject.SetActive(true);
            killText.gameObject.SetActive(false);
        }
        else
        {
            timeText.gameObject.SetActive(false);
            killText.gameObject.SetActive(true);
        }
        if (TowerManager.DungeonType == "boss") {
            timeText.gameObject.SetActive(false);
            killText.gameObject.SetActive(false);
            KillBossText.gameObject.SetActive(true);
        }

        //maxBattleTime = 1;
        //nowTime = 88; 
    }


    /*结算 todo  
     *1.这里副本奖励物品需求不明 奖励规则不清楚 这里只填了最基本的钱和经验
     *2.副本结算这种影响物资的行为应该由服务器校验,不能进行本地计算奖励 否则会被作弊
     *
     *爬塔改造  
     *0失败
     *1小关卡胜利
     *2boss胜利
     *3主动退出
     */
    public void settlement(int state) {
        //保存角色等级 经验 血量百分比状态
        if (state == 1)
        {
            DataManager.Get().userData.towerData.level = player.level;
            DataManager.Get().userData.towerData.exp = player.exp_now;
            DataManager.Get().userData.towerData.hpRate = (player.hp_now + 0.0f) / (player.hp_max + 0.0f);
            DataManager.Get().userData.towerData.eyRate = (player.move_dlyEsNow + 0.0f) / (player.move_dlyEsMax + 0.0f);
            DataManager.Get().userData.towerData.damageMap = DamageMeters.damageMap;
            DataManager.Get().userData.towerData.nowNode = DataManager.Get().userData.towerData.selecNode;
            DataManager.Get().userData.towerData.killNum += killNum;
            DataManager.Get().userData.towerData.gameTime += nowTime;
            DataManager.Get().save(true);
        }
      /*  else if (state == 2)
        {
            DataManager.Get().userData.towerData.level = player.level;
            DataManager.Get().userData.towerData.exp = player.exp_now;
            DataManager.Get().userData.towerData.hpRate = (player.hp_now + 0.0f) / (player.hp_max + 0.0f);
            DataManager.Get().userData.towerData.eyRate = (player.move_dlyEsNow + 0.0f) / (player.move_dlyEsMax + 0.0f);
            DataManager.Get().userData.towerData.damageMap = DamageMeters.damageMap;
            DataManager.Get().userData.towerData.nowNode = DataManager.Get().userData.towerData.selecNode;
            DataManager.Get().userData.towerData.killNum += killNum;
            DataManager.Get().userData.towerData.gameTime += nowTime;
        }*/
        else {
            DataManager.Get().userData.towerData.level = player.level;
            DataManager.Get().userData.towerData.exp = player.exp_now;
            DataManager.Get().userData.towerData.hpRate = (player.hp_now + 0.0f) / (player.hp_max + 0.0f);
            DataManager.Get().userData.towerData.eyRate = (player.move_dlyEsNow + 0.0f) / (player.move_dlyEsMax + 0.0f);
            DataManager.Get().userData.towerData.damageMap = DamageMeters.damageMap;
            DataManager.Get().userData.towerData.nowNode = DataManager.Get().userData.towerData.selecNode;
            DataManager.Get().userData.towerData.killNum += killNum;
            DataManager.Get().userData.towerData.gameTime += nowTime;
        }

        settlementPanel.settlement(state);
    }

    int maxKillNum = 200;
    float maxBattleTime = 180;
    float TransitionCountdown_now = 3;
    float TransitionCountdown = 3f;
    Image transitionImg;
    SpriteRenderer boomMask;


    int TimeOrKill = 0;
    public bool gameEndFlag = false;

    private void Update()
    {
        expUpdate();

        cameraUpdate();

        if (DungeonManager.zb_mode == 2) {

            getEy();
            return;
        }


        killText.text = "<size=50>Kill</size>\r\n" + killNum + "/" + maxKillNum;

        //test
        /*maxBattleTime = 1;
        TimeOrKill = 0;*/

        if ((TimeOrKill == 0 && duration == maxBattleTime)||
        (TimeOrKill == 1 && killNum >= maxKillNum)) 
        {
            gameEndFlag = true;
        }
        

        //todo 爬塔改造  倒计时结束
        if (gameEndFlag)
        {
            //触发一次
            if (TransitionCountdown_now == TransitionCountdown) {
                transitionImg.gameObject.SetActive(true);
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
                foreach (var e in enemys)
                {
                    e.GetComponent<Enemy>().hurt(0, true);
                }
                //闪白
                boomMask.gameObject.SetActive(true);
                //消除精英怪血条

                //根据关卡类型触发相关特殊效果
                if (TowerManager.DungeonType == "普通") {
                    selectRelicNum += RoleManager.Get().win_common_relic;
                }
               
                DataManager.Get().userData.towerData.level = player.level;
                DataManager.Get().userData.towerData.exp = player.exp_now;
                DataManager.Get().userData.towerData.hpRate = (player.hp_now + 0.0f) / (player.hp_max + 0.0f);
                DataManager.Get().userData.towerData.eyRate = (player.move_dlyEsNow + 0.0f) / (player.move_dlyEsMax + 0.0f);
                DataManager.Get().userData.towerData.damageMap = DamageMeters.damageMap;
                DataManager.Get().userData.towerData.nowNode = DataManager.Get().userData.towerData.selecNode;
                DataManager.Get().userData.towerData.killNum += killNum;
                //DataManager.Get().save();
                //todo  这里存在时间差  可能还没存过去就读了  就有问题了
            }

            if (TransitionCountdown_now <= 2) {
                endUpFlag = true;
            }

            if (TransitionCountdown_now <= 1)
            { 
                transitionImg.color = new Color(0, 0, 0, 
                    1 - TransitionCountdown_now);
            }

            TransitionCountdown_now -= Time.deltaTime;
            if (TransitionCountdown_now <= 0) {
                settlement(1);
            }
            return;
        }



        if (bossFlag) {
            startBossBattle();
            return;
        }

        nowTime += Time.deltaTime;
        duration = (int)Mathf.Floor(nowTime);
        string timeStr = SpriteNumUtil.zhTime((int)maxBattleTime - duration);


        timeText.text = "<size=50>Survival Time</size>\r\n" + timeStr;
        progressUi.value = duration / maxBattleTime;
        progressTextUi.text = SpriteNumUtil.zhjindu(Mathf.FloorToInt(duration / maxBattleTime * 100));

       

        if (tipTime > 0) {
            tipTime -= Time.deltaTime;
            if (tipTime <= 0) { 
                enemyGroupTip.SetActive(false);
                bossTip.SetActive(false);
            }
        }


        if (nowIndex < infoList.Count && 
            nowTime >= infoList[nowIndex].time) {
            //加入待执行事件
            DungeonInfoConfig cf = infoList[nowIndex++];
            if(cf.datas!=null && cf.datas.Count>0)
                foreach (DungeonInfoData data in cf.datas) { 
                    EventData ed = new EventData();
                    ed.info = new DungeonInfo();
                    ed.info.enemys = data.id;
                    ed.info.cd = data.cd;
                    ed.info.num = data.num;
                    ed.info.hp_up = data.hp_up;
                    ed.info.dmg_up = data.dmg_up;
                    ed.info.exp_up = data.exp_up;
                    ed.info.speed_up = data.speed_up;
               
                    if (cf.type == "boss")
                    {
                        tipTime = 2;
                        bossCountdown_now = 0.001f;
                        bossTip.SetActive(true);
                        bossFlag = true;
                        GameObject[] enemys = GameObject.FindGameObjectsWithTag("enemy");
                        foreach (var e in enemys)
                        {
                            e.GetComponent<Enemy>().hurt(0,true);
                        }
                        enemyGroupFlag = false;
                        enemyGroupEndFlag = true;
                        bossEd_now = ed;
                        nowEventList.Clear();
                        //爬塔改造
                        startBossBattle();
                        return;
                    }
                    else
                    {
                        nowEventList.Add(ed); 
                    }
                }
        }


         if (!bossFlag) { 
             foreach (EventData d in nowEventList){
                 d.cd -= Time.deltaTime;
                 //持续执行
                 if (d.info.cd > 0)
                 {
                     if (d.cd <= 0)
                     {
                         d.cd = d.info.cd;
                         for (int i = 0;i< d.info.num; i++) {
                             creatEnemy(d.info.enemys, d.info);
                         }
                     }
                 }
                 //只执行一次
                 else if(!d.flag) {
                     d.flag = true;

                    if (d.info.enemys.Contains("obstacle")) { 
                        //StartCoroutine(creatObstacle(d.info.enemys));
                    }
                    else
                        creatEnemy(d.info.enemys, d.info);
                 }
             }
         }
    }

    void FixedUpdate() {

        if (DungeonManager.zb_mode != 0)
            return;


        if (endUpFlag && (upLevelNum > 0 || selectRelicNum > 0)) {
            endUpFlag = endUp();
        }

        if (jySelectRelicNum > 0) {
            jySelectRelicNum -= 1;
            NewSelectRelic.Show();
        }
    }



    



    bool endUp() {
       
        if (upLevelNum > 0)
        {
            upLevelNum -= 1;
            upLevel.up();
            return true;
        }
        else
        {
            if (selectRelicNum > 0)
            {
                selectRelicNum -= 1;
                selectRelicManager.show();
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    void startBossBattle() {
        if (bossCountdown_now > 0)
        {
            bossCountdown_now -= Time.deltaTime;
            if (bossCountdown_now <= 0)
            {
                bossTip.SetActive(false);

                ProCamera2D.Instance.RemoveCameraTarget(player.transform);
                ProCamera2D.Instance.ResetMovement();
                ProCamera2D.Instance.MoveCameraInstantlyToPosition(new Vector2(1000 * bossIndex + 20000, 20000));

                //传送到boss地图 爬塔改造
                //playerSavePos = player.transform.position;
                //player.transform.position = new Vector2(1000 * bossIndex + 20000, 20000);

                //生成boss
                Enemy ey = creatEnemy(bossEd_now.info.enemys, bossEd_now.info);
                ey.transform.position = new Vector2(1000 * bossIndex + 20000, 20005);

                //todo 暂时让相机不跟随玩家
                //Camera.main.transform.parent = null;
                bossHP.gameObject.SetActive(true);
            }
        }
    }

    void cameraUpdate() {
        if (enemyGroupFlag && Camera.main.orthographicSize < enemyGroupCameraSize)
        {
            Camera.main.orthographicSize += 0.025f;
            CameraSize_now = enemyGroupCameraSize;
            if (Camera.main.orthographicSize >= enemyGroupCameraSize)
            {
                enemyGroupFlag = false;
            }
        }
        if (enemyGroupEndFlag && Camera.main.orthographicSize > defaultCameraSize)
        {
            Camera.main.orthographicSize -= 0.025f;
            CameraSize_now = defaultCameraSize;
            if (Camera.main.orthographicSize <= defaultCameraSize)
            {
                enemyGroupEndFlag = false;
            }
        }
        if (!enemyGroupFlag && !enemyGroupEndFlag)
        {
            if (superAttackReadyFlag)
            {
                if (Camera.main.orthographicSize > CameraSize_now - dlyCameraSizeUp)
                    Camera.main.orthographicSize -= 0.01f;
                else
                    superAttackReadyFlag = false;
            }
            if (superAttackReadyEndFlag)
            {
                if (Camera.main.orthographicSize < CameraSize_now)
                    Camera.main.orthographicSize += 0.05f;
                else
                    superAttackReadyEndFlag = false;
            }
        }
    }

    Enemy creatEnemy(string enemyid , DungeonInfo info)
    {
        if (nowEnemyNum >= maxEnemyNum) {
            return null;
        }
        nowEnemyNum++;

        EnemyAttr eatr = ef.eyMap[enemyid];

        if (!enemyPfMap.ContainsKey(enemyid)) {
            enemyPfMap.Add(enemyid, 
                Resources.Load<GameObject>("role/enemy/" + eatr.pf));
        }
       

        GameObject pf = null;
        pf = enemyPfMap[enemyid];
        //todo 压力测试
        //pf = e1Pf;

      
        GameObject ey = null;
        Enemy enemy = null;
        if (eatr.type.IndexOf("boss") == -1 &&
            enemyPoolMap.ContainsKey(enemyid) && 
            enemyPoolMap[enemyid].Count > 0)
        {
            enemy = enemyPoolMap[enemyid][0];
            ey = enemy.gameObject;
            enemyPoolMap[enemyid].RemoveAt(0);
            enemy.spriteTra.transform.localPosition = pf.transform.Find("spine").transform.localPosition;
            enemy.spriteTra.transform.localScale = pf.transform.Find("spine").transform.localScale;
        }
        else { 
            ey = Instantiate(pf);
            enemy = ey.GetComponent<Enemy>();
            enemy.originScaleX = pf.transform.Find("spine").transform.localScale.x;
        }


        Vector3 vec = player.transform.position;
        enemy.atr = eatr;
        enemy.hp = Mathf.Max(1, (int)(enemy.atr.hp * (info.hp_up / 10000.0f) * (nowChapter.hpUp / 10000.0f)));
        enemy.hp_now = Mathf.Max(1, (int)(enemy.atr.hp * (info.hp_up / 10000.0f) * (nowChapter.hpUp / 10000.0f)));

        /* enemy.hp = 400;
         enemy.hp_now = 400;*/

        

        enemy.attack = Mathf.Max(1,(int)(enemy.atr.attack * (info.dmg_up / 10000.0f) * (nowChapter.dmgUp / 10000.0f)));
        enemy.exp = Mathf.Max(1, (int)(enemy.atr.exp * (info.exp_up / 10000.0f)));
        enemy.speed = Mathf.Max(1, (int)(enemy.atr.speed * (info.speed_up / 10000.0f)));
        enemy.mgr = this;
        enemy.init(); 

        float offsetX = Random.Range(0, 20f) * randomValue();
        float offsetY = Random.Range(0f, 30f) * randomValue();
        //当y绝对值小于25时  x值小于12时 x绝对值一定要大于12
        if (Mathf.Abs(offsetY) < 25 && Mathf.Abs(offsetY) < 12)
        { 
            if (randomValue()>0)
                offsetX = Random.Range(12, 20f) * randomValue();
            else
                offsetY = Random.Range(25f, 30f) * randomValue();
        }
     
        ey.transform.position = 
            new Vector3(vec.x + offsetX, vec.y + offsetY, 0);

        ey.SetActive(true);

        return enemy;
    }

    public static List<Obstacle> nowObstacleList = new List<Obstacle>();
    public static IEnumerator creatObstacle(string id,GameObject g = null) {
        gmr.tryDlyCourse();

        nowObstacleList.Clear();
        ObstacleAttr atr = ObstacleFactory.atrMap[id];
        List<float> xlist = new List<float>();
        List<float> ylist = new List<float>();
        int num = Mathf.Min(Random.Range(atr.minNum, atr.maxNum + 1), 8);
        int angle = 360 / num;
        int rotate = 0;

        List<Obstacle> obstacleList = new List<Obstacle>();

        Vector3 startPos = player.transform.position;

        for (int i = 0; i < num; i++)
        {
            GameObject obstacle_1 = Instantiate(Resources.Load<GameObject>
                ("prop/obs/obstacle_" + Random.Range(10, 20)));

            Obstacle obs = obstacle_1.GetComponent<Obstacle>();
            nowObstacleList.Add(obs);
            obstacleList.Add(obs);

            if (i > 0) { 
                obs.lastObstacle = obstacleList[i-1];

                if(i < num)
                    obstacleList[i - 1].nextObstacle = obs;
            }


            obs.index = i;
            obs.maxNum = num;

            obs.attr = atr;
            obs.startPos = startPos;
            rotate += angle;
            Vector3 vec = LockUtil.RotateAngle(new Vector2(0, 0), new Vector2(0, 7), rotate);
            float x1 = Random.Range(0.0f, 2.0f) * (vec.x < 0 ? 1 : -1);
            float y1 = Random.Range(0.0f, 2.0f) * (vec.y < 0 ? -1 : 1);
            vec += new Vector3(x1,y1);
            obstacle_1.transform.position = player.transform.position + vec;

            if (i == num - 1) { 
                obs.endObstacle = true;
            }
            obs.obstacleList = obstacleList;

            yield return new WaitForSeconds(atr.interval);
        }
        if(g!=null)
            Destroy(g);
    }

    public void desEnemy(Enemy ey) {
        ey.gameObject.SetActive(false);
        if (!enemyPoolMap.ContainsKey(ey.atr.id)) {
            enemyPoolMap.Add(ey.atr.id, new List<Enemy>());
        }
        enemyPoolMap[ey.atr.id].Add(ey);
    }

    int randomValue()
    {
        return (Random.value < 0.5f ? -1 : 1);
    }

    public static bool bossFlag;

    public static void bossDie()
    {
        bossHP.gameObject.SetActive(false);
        //todo 生成一个传送门 让玩家回去
        GameObject p = Instantiate(Resources.Load<GameObject>("prop/Portal"));
        p.transform.position = new Vector3(1000 * bossIndex+20000, 20000, 0); 
    }

    public static void bossEnd() {
        bossFlag = false;
        player.transform.position = playerSavePos;
        ProCamera2D.Instance.AddCameraTarget(player.transform);
        ProCamera2D.Instance.MoveCameraInstantlyToPosition(player.transform.position);

    }

    public expCrystalfb creatExp()
    {
        expCrystalfb exp = null;
        if (expPoolList.Count > 0)
        {
            exp = expPoolList[0];
            expPoolList.RemoveAt(0);
        }
        else
        {
            GameObject expCrystal = null;//Instantiate(expCrystalPf);
            if (Random.value >= 0.5f)
            {
                expCrystal = Instantiate(expCrystalPf);
            }
            else {
                expCrystal = Instantiate(expCrystalPf_2);
            }

            exp = new expCrystalfb();
            exp.obj = expCrystal;
            exp.mgr = this;
            expPoolList_cj.Add(exp);
        }
        exp.init();
        exp.obj.SetActive(true);
        return exp;
    }

    public static bool MagnetFlag;

    public void expUpdate() {
        if (MagnetFlag) {
            MagnetFlag = false;
            foreach (expCrystalfb exp in expPoolList_cj)
            {
                exp.absorb();
            }
        }

        foreach (expCrystalfb exp in expPoolList_cj) {
            exp.Update();
        }
    }

    public static void desExp(expCrystalfb exp)
    {
        expPoolList.Add(exp);
        exp.obj.SetActive(false);
    }

    static Transform relicNumTra;
    static List<GameObject> relicNumList;
    static GameObject relicNumGPf;
    public void addSelectRelicNum() {
        jySelectRelicNum++;
    }

    public static void addkill()
    {
        killNum++;
        //killTxt.text = SpriteNumUtil.zhInt(killNum);
    }

    public void setGold(int value) {
        goldText.text = value + "";
    }







    //----------------直播 弹幕礼物产生怪物
    void getEy() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
            for (int i = 0; i < 5; i++)
                creatEnemy("e1", new DungeonInfo());
        if (Input.GetKeyDown(KeyCode.Alpha2))
            for (int i = 0; i < 5; i++)
                creatEnemy("e2", new DungeonInfo());
        if (Input.GetKeyDown(KeyCode.Alpha3))
            creatEnemy("jy1", new DungeonInfo());
        if (Input.GetKeyDown(KeyCode.Alpha4))
            creatEnemy("boss1", new DungeonInfo());
    }
}


public class EventData
{
    public DungeonInfo info;
    public float cd;
    public bool flag;
}
public class GameSetting
{
    public static GameSetting Instance {get{ return new GameSetting(); }}

    public  int Width = 10;
    public  int Height = 20;
}