using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using DG.Tweening;

public class TowerMapForm : BaseUIForm
{
    TextMeshProUGUI nameDesc;
    TextMeshProUGUI goldNum;

    TowerManager towerMgr;
    EventPanel eventPanel;
    RelicPanel relicPanel;

    Transform storeyTra;
    GameObject storeyPf;
    GameObject nodePf;
    List<GameObject> linePfs;

    Slider hp_slider;
    Slider exp_slider;
    TextMeshProUGUI expText;

    Transform loadPanel;
    Slider load_slider;

    //存放地图节点
    List<Transform> storeyList = new List<Transform>();
    List<List<TowerNodeSlot>> nodeMap = new List<List<TowerNodeSlot>>();

    ScrollRect scrollRect;
    Scrollbar scrollbar;
    //地图初始化完毕标记,完毕后 会自动同步层数调整滚动条以显示当前攀爬层数
    bool initMapFlag;

    public static bool RelicEventFlag;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.HideOther;
        ui_type.IsClearStack = false;


        bool isHaveLiuhai = false;
#if UNITY_IPHONE
  		 //通过设备型号判断是否刘海屏
         if (SystemInfo.deviceModel.Contains("iPhone10,3") 
          || SystemInfo.deviceModel.Contains("iPhone10,6")
          || SystemInfo.deviceModel.Contains("iPhone11,2")
          || SystemInfo.deviceModel.Contains("iPhone11,6")
          || SystemInfo.deviceModel.Contains("iPhone11,8"))
        {
            isHaveLiuhai = true;
        }
        //通过屏幕比例判断是否刘海屏
        if ((float)Screen.width / Screen.height > 2)
        {
            isHaveLiuhai = true;
        }
#endif
        if (Screen.height - Screen.safeArea.yMax > 0)
            isHaveLiuhai = true;
        if (isHaveLiuhai)
            GameObject.Find("up").GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 220);




        towerMgr = GameObject.Find("UIManager").GetComponent<TowerManager>();

        scrollRect = GameObject.Find("MapPanel/Scroll View").GetComponent<ScrollRect>();
        scrollbar = GameObject.Find("MapPanel/Scroll View/Scrollbar Vertical").GetComponent<Scrollbar>();
        hp_slider = GameObject.Find("hp").GetComponent<Slider>();
        exp_slider = GameObject.Find("exp").GetComponent<Slider>();
        expText = GameObject.Find("exp/Text (TMP)").GetComponent<TextMeshProUGUI>();

        loadPanel = UIFrameUtil.FindChildNode(this.transform, "loadPanel");
        load_slider = UIFrameUtil.FindChildNode(this.transform, "loadPanel/Slider").GetComponent<Slider>();

        nameDesc = UIFrameUtil.FindChildNode(this.transform, "nameDesc").GetComponent<TextMeshProUGUI>();
        goldNum = UIFrameUtil.FindChildNode(this.transform, "goldNum/Text (TMP)").GetComponent<TextMeshProUGUI>();


        linePfs = new List<GameObject>();
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_1"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_2"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_3"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_4"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_5"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_6"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_7"));
        linePfs.Add(Resources.Load<GameObject>("ui/img/tower/line/mapLine_8"));

        MessageMgr.AddMsgListener("selectMapNode", p =>
        {
            selectCheck((TowerMapNodeData)p.Value);
        });

      

        GetBut(this.transform, "packBut").onClick.AddListener(() => {
            OpenForm("TowerBackPack");
        });

        GetBut(this.transform, "roleBut").onClick.AddListener(() => {
            UIFrameUtil.FindChildNode(this.transform, "RoleStatePanel").gameObject.SetActive(true);
        });
        GetBut(this.transform, "dmgBut").onClick.AddListener(() => {
            UIFrameUtil.FindChildNode(this.transform, "DamagePanel").gameObject.SetActive(true);
        });

        storeyTra = UIFrameUtil.FindChildNode(this.transform, "MapStoreyList").GetComponent<Transform>();
        storeyList = new List<Transform>();

        settlementPanel = UIFrameUtil.FindChildNode(this.transform, "SettlementPanel").GetComponent<SettlementPanel>();
        eventPanel = UIFrameUtil.FindChildNode(this.transform, "EventPanel").GetComponent<EventPanel>();
        relicPanel = UIFrameUtil.FindChildNode(this.transform, "RelicPanel").GetComponent<RelicPanel>();
        eventPanel.mapForm = this;
        relicPanel.mapForm = this;
        /* for (int i = 0; i < storeyTra.childCount; i++) {
            storeyList.Add(storeyTra.GetChild(i));
        }

        storeyPf = storeyList[0].gameObject;
        nodePf = storeyList[0].GetChild(0).gameObject;*/

        //storeyPf = UIFrameUtil.FindChildNode(this.transform, "MapStoreyList").Find("nodeList (1)").gameObject;//Resources.Load<GameObject>("ui/Form/TowerMapForm/nodeList (1)");
        storeyPf = Resources.Load<GameObject>("ui/Form/TowerMapForm/nodeList (1)");
        nodePf = storeyPf.transform.GetChild(0).gameObject;





        GameObject BackPanel = transform.Find("BackPanel").gameObject;
        GetBut(this.transform, "returnBut").onClick.AddListener(() => {
            BackPanel.SetActive(true);
        });
        UIFrameUtil.FindChildNode(BackPanel.transform, "cancelButton").GetComponent<Button>()
           .onClick.AddListener(() => {
               BackPanel.SetActive(false);
           });
        UIFrameUtil.FindChildNode(BackPanel.transform, "confirmButton").GetComponent<Button>()
            .onClick.AddListener(() => {
                //弹出结算画面
                end(3);
            });

        load_slider.value = 0f;
        initEnd = false;
        //章节索引:
        //initTowerMap(DataManager.Get().now_chapterIndex);
    }


    public async void initTowerMap(int chapterIndex) {
        loadPanel.gameObject.SetActive(true);
        await initRole();
       

        //todo  如果要中途回塔功能 就需要这个
        if (DataManager.Get().userData.towerData == null)
            await DataManager.Get().read();

            //检测玩家是否有正在进行中的爬塔
        if (DataManager.Get().userData.towerData == null)
        {
            towerMgr.towerData = new TowerGameData();
            towerMgr.towerData.nowNode = "0-0";
            towerMgr.towerData.nodeMap = towerMgr.creatTowerMap(chapterIndex);
            towerMgr.towerData.nowChapter = TowerFactory.Get().chapterList[chapterIndex];
            TowerManager.DungeonStorey = 0;
            TowerManager.EndStorey = 0;
            DataManager.Get().userData.towerData = towerMgr.towerData;
            //给定初始武器
            DataManager.Get().userData.towerData.skillInfo 
                = new Dictionary<string, int>();
            DataManager.Get().userData.towerData.skillInfo[DataManager.Get().GetWpStr()] = 1;
            //给定测试用的宝物
#if UNITY_EDITOR
            List<RelicConfig> RelicConfigList = TowerFactory.Get().relicList;
            for (int i=0;i< RelicConfigList.Count;i++) {
                for (int j=0;j< RelicConfigList[i].testNum;j++) { 
                    Relic relic = new Relic();
                    relic.configId = RelicConfigList[i].id;
                    relic.level = 0;
                    relic.quality = RelicConfigList[i].quality;
                    DataManager.Get().userData.towerData.relicList.Add(relic);
                }
            }
#endif
            DataManager.Get().save();
        }
        else {
            towerMgr.towerData = DataManager.Get().userData.towerData;
            towerMgr.towerData.nowNode = DataManager.Get().userData.towerData.nowNode;
            towerMgr.towerData.nodeMap = DataManager.Get().userData.towerData.nodeMap;
            TowerManager.nowChapter = DataManager.Get().userData.towerData.nowChapter;
            TowerManager.EndStorey = int.Parse(towerMgr.towerData.nowNode.Split("-")[0]);
        }

        //DataManager.Get().userData.towerData.buffList.Add("buff_event_relic");


        TowerMap chapter = TowerFactory.Get().tmList.Find(x =>
          x.id == TowerFactory.Get().chapterList[DataManager.Get().now_chapterIndex]);
        nameDesc.text = chapter.name;

        RefreshMap();

        if (towerMgr.towerData.selecNode != null && towerMgr.towerData.nowNode != towerMgr.towerData.selecNode)
        {
            string[] kv = towerMgr.towerData.selecNode.Split("-");
            TowerMapNodeData data = towerMgr.towerData
                .nodeMap[int.Parse(kv[0])][int.Parse(kv[1])];
            selectCheck(data,true);
        }


        RoleManager.Get().init(true);
    }

    bool initStart;
    bool initEnd;
    void Update()
    {
        if (!DataManager.Get().saveIngFlag && !initStart) {
            initStart = true;
            initTowerMap(DataManager.Get().now_chapterIndex);
        }

        if (loadPanel.gameObject.activeInHierarchy && load_slider.value<0.9f)
        {
            load_slider.value += Time.deltaTime;
        }

        if (initEnd && !DataManager.Get().saveIngFlag) {
            load_slider.value = 1f;
            loadPanel.gameObject.SetActive(false);
        }
    }
    float timeR;
    bool notf;

    public async Task<bool> initRole() {
        await DataManager.Get().refreshRoleAttributeStr();
        return true;
    }

    public void RefreshMap() {
        storeyPf.gameObject.SetActive(true);
        for (int i = 0; i < towerMgr.towerData.nodeMap.Count; i++)
        {

            nodeMap.Add(new List<TowerNodeSlot>());
            //楼层不足 生成楼层元素
            if (storeyList.Count <= i)
            {
                GameObject storey = Instantiate(storeyPf);
                storeyList.Add(storey.transform);
                storey.transform.parent = storeyTra;
                storey.transform.localScale = new Vector3(1, 1, 1);
                storey.gameObject.SetActive(true);
            }



            for (int j = 0; j < towerMgr.towerData.nodeMap[i].Count; j++)
            {
                GameObject node = null;
                //楼层节点不足  生成节点元素
                if (storeyList[i].childCount <= j)
                {
                    node = Instantiate(nodePf);
                    node.transform.parent = storeyList[i];
                    node.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    node = storeyList[i].GetChild(j).gameObject;
                }

                TowerNodeSlot slot = node.GetComponent<TowerNodeSlot>();

                //赋予节点信息
                slot.data = towerMgr.towerData.nodeMap[i][j];

                //排列点位
                node.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(towerMgr.towerData.nodeMap[i][j].x, towerMgr.towerData.nodeMap[i][j].y);

                //给节点添加图标
                node.transform.Find("icon").GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("ui/img/tower/" + slot.data.type);
                //底部圆圈
                node.transform.Find("Q").GetComponent<Image>().sprite =
                   Resources.Load<Sprite>("ui/img/tower/Q/" + "圈圈 暗");


                if (slot.data.type == "boss") {
                    node.transform.Find("icon").GetComponent<RectTransform>().sizeDelta 
                        = new Vector2(300,300);
                    node.transform.Find("icon").GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(0, 50);
                }
                if (slot.data.type == "深渊")
                {
                    node.transform.Find("icon").GetComponent<RectTransform>().sizeDelta
                        = new Vector2(120, 120);
                }

                //添加节点至集合
                nodeMap[i].Add(slot);
            }
        }

        //CreatLine CreatLine = new CreatLine();

        for (int i = 0; i < nodeMap.Count; i++)
        {
            for (int j = 0; j < nodeMap[i].Count; j++)
            {
                //生成点链接线
                creatLine(i,j);
            }
        }

        //todo 爬塔结束  结算
        if (towerMgr.towerData.nowNode == "10-0")
        {
            end(2);
            return;
        }

        //筛选出可以选择的节点
        RefreshStorey();



        goldNum.text = DataManager.Get().userData.towerData.gold + "";
        hp_slider.value = DataManager.Get().userData.towerData.hpRate;
        exp_slider.value = (DataManager.Get().userData.towerData.exp + 0.0f) / ExpFactory.Get().expMap[DataManager.Get().userData.towerData.level];
        expText.text = "lv."+DataManager.Get().userData.towerData.level;
        initMapFlag = false;
    }

    public void RefreshStorey() {

        if(role_icon==null)
            role_icon = Instantiate(Resources.Load<GameObject>("ui/Form/TowerMapForm/role_icon"));

        //当前可走通的节点
        List<string> walkableNodes = new List<string>();

        for (int i = 0; i < nodeMap.Count; i++)
        {
            for (int j = 0; j < nodeMap[i].Count; j++)
            {
                bool flag = check(nodeMap[i][j].data.nodeStr,
                      towerMgr.towerData.nowNode);

                //校验是否属于可行节点
                if (flag)
                {
                    walkableNodes.Add(nodeMap[i][j].data.nodeStr);
                }
                else { 
                    for (int k = 0; k < walkableNodes.Count; k++) {
                        flag = check(nodeMap[i][j].data.nodeStr,
                          walkableNodes[k]);
                        if (flag) {
                            walkableNodes.Add(nodeMap[i][j].data.nodeStr);
                            break;
                        }
                    }
                }


                nodeMap[i][j].myBut.interactable = flag;

                if (!flag)
                {
                    nodeMap[i][j].transform.Find("icon").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("ui/img/tower/unlock/" + nodeMap[i][j].data.type);
                    nodeMap[i][j].transform.Find("Q").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("ui/img/tower/Q/" + "圈圈 暗");

                    //todo
                    for (int k = 0; k < nodeMap[i][j].transform.childCount; k++)
                    {
                        if (nodeMap[i][j].transform.GetChild(k).name.IndexOf("mapLine") != -1)
                        {
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite
                                = Resources.Load<Sprite>("ui/img/tower/line/LineImg/h/" + nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite.name);
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().color
                                = new Color(1, 1, 1, 0.6f);
                        }
                    }
                }
                else{
                    nodeMap[i][j].transform.Find("icon").GetComponent<Image>().sprite =
                      Resources.Load<Sprite>("ui/img/tower/" + nodeMap[i][j].data.type);
                    //底部圆圈
                    nodeMap[i][j].transform.Find("Q").GetComponent<Image>().sprite =
                       Resources.Load<Sprite>("ui/img/tower/Q/" + "圈圈 亮");

                    //todo
                    for (int k = 0; k < nodeMap[i][j].transform.childCount; k++)
                    {
                        if (nodeMap[i][j].transform.GetChild(k).name.IndexOf("mapLine") != -1)
                        {
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite
                                = Resources.Load<Sprite>("ui/img/tower/line/LineImg/" + nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite.name);
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().color
                               = new Color(1, 1, 1, 0.6f);
                        }
                    }

                }

                //用角色头像 连接线变成高亮
                if (nodeMap[i][j].data.nodeStr == towerMgr.towerData.nowNode)
                {
                    role_icon.transform.parent = nodeMap[i][j].transform;
                    role_icon.GetComponent<RectTransform>().position
                        = nodeMap[i][j].transform.Find("icon").GetComponent<RectTransform>().position + new Vector3(0, 20);

                    /*nodeMap[i][j].transform.Find("icon").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("ui/img/tower/" + "role");*/
                    nodeMap[i][j].transform.Find("Q").GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("ui/img/tower/Q/" + "圈圈 亮");
                    nodeMap[i][j].myBut.interactable = true;
                    /*nodeMap[i][j].transform.Find("icon").GetComponent<RectTransform>().anchoredPosition =
                       new Vector2(0, 20);*/

                    for (int k = 0; k < nodeMap[i][j].transform.childCount; k++)
                    {
                        if (nodeMap[i][j].transform.GetChild(k).name.IndexOf("mapLine") != -1)
                        {
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite
                                = Resources.Load<Sprite>("ui/img/tower/line/LineImg/" + nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().sprite.name);
                            nodeMap[i][j].transform.GetChild(k).GetComponent<Image>().color
                               = new Color(1, 1, 1, 0.6f);
                        }
                    }
                }
                else
                {
                    if(nodeMap[i][j].data.type != "boss")
                        nodeMap[i][j].transform.Find("icon").GetComponent<RectTransform>().anchoredPosition =
                          new Vector2(0, 0);
                }
            }
        }
        hp_slider.value = DataManager.Get().userData.towerData.hpRate;
        initMapFlag = false;

        goldNum.text = DataManager.Get().userData.towerData.gold + "";

        StartCoroutine(positioning());
    }

    public void creatLine(int i,int j) {

        CreatLine CreatLine = new CreatLine();
        //生成点链接线
        string[] nextNodes = null;
        if (nodeMap[i][j].data.nextNodeStrs != null)
        {
            if (nodeMap[i][j].data.nextNodeStrs.IndexOf(";") != -1)
            {
                nextNodes = nodeMap[i][j].data.nextNodeStrs.Split(";");
            }
            else
            {
                nextNodes = new string[1];
                nextNodes[0] = nodeMap[i][j].data.nextNodeStrs;
            }
        }
        else
        {
            nodeMap[i][j].GetComponent<RectTransform>().anchoredPosition =
                   new Vector2(towerMgr.towerData.nodeMap[i][j].x,
                   towerMgr.towerData.nodeMap[i][j].y + 20);
        }

        if (nextNodes != null)
            foreach (string nodeStr in nextNodes)
            {
                //Debug.Log(nodeStr);
                string[] kv = nodeStr.Split("-");
                int key = int.Parse(kv[0]);
                int value = int.Parse(kv[1]);



                //生成楼层节点链接线
                Vector3 headLocalPosition =
                    nodeMap[i][j].transform.InverseTransformPoint
                    (nodeMap[key][value].transform.position);



                Vector2 start = nodeMap[i][j].transform.localPosition
                    + new Vector3(500 -
                    nodeMap[i][j].GetComponent<RectTransform>().anchoredPosition.x + (value - 1) * 25,
                    -nodeMap[i][j].data.y + 50);

                Vector2 end = headLocalPosition
                + new Vector3(0 + (j - 1) * 15 * (j + 0.0f) / nodeMap[i].Count,
                storeyPf.GetComponent<RectTransform>().sizeDelta.y - 50);

                var distance = Vector2.Distance(end, start);


                int index = ((int)distance - 200) / 50;
                index = Mathf.Clamp(index, 0, 7);
                //根据距离
                GameObject line = Instantiate(linePfs[index]);
                line.transform.SetParent(nodeMap[i][j].transform);
                line.transform.localScale = new Vector3(1, 1, 1);
                CreatLine.CreateLine(start, end, line);

                //调整线的弧度方向
                if (((j + 0.0f) / nodeMap[i].Count >
                    (value + 0.0f) / nodeMap[i + 1].Count || value == 0)
                    && value != nodeMap[i + 1].Count - 1)
                    line.transform.localScale =
                        new Vector3(line.transform.localScale.x * -1,
                        line.transform.localScale.y);
            }
    }

    //新手引导相关
    void JC(int index ) {
        //先清理
        GameObject jcPanel = UIFrameUtil.FindChildNode(this.transform, "jcPanel").gameObject;
        for (int i = jcPanel.transform.childCount - 1; i >= 0;i--) {
            if (jcPanel.transform.GetChild(i).name == "role")
                continue;
            Destroy(jcPanel.transform.GetChild(i).gameObject);
        }
        jcPanel.SetActive(true);
        List<TowerNodeSlot> item = nodeMap[index];


        LayoutRebuilder.ForceRebuildLayoutImmediate
         (nodeMap[index][0].transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate
       (nodeMap[index][0].transform.parent.parent.GetComponent<RectTransform>());

        string[] kv = towerMgr.towerData.nowNode.Split("-");
        int k = int.Parse(kv[0]);
        int v = int.Parse(kv[1]);
        GameObject node_0 = Instantiate(nodeMap[k][v].gameObject, jcPanel.transform);
        node_0.transform.position = nodeMap[k][v].transform.position;


        for (int i=0;i< item.Count;i++) {
            //TowerNodeSlot r = Instantiate(nodeMap[index][i], jcPanel.transform,false);
            Debug.Log("------------:" + nodeMap[index][i].transform.position);
            //r.transform.position = nodeMap[index][i].transform.position + new Vector3(500,700);


            GameObject node = null;
            node = Instantiate(nodePf, jcPanel.transform);
            node.transform.localScale = new Vector3(1, 1, 1);
            TowerNodeSlot slot = node.GetComponent<TowerNodeSlot>();
            //赋予节点信息
            slot.data = towerMgr.towerData.nodeMap[index][i];

            //排列点位
            node.transform.position = nodeMap[index][i].transform.position;
            //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null,nodeMap[index][i].transform.position);
            //RectTransform rt = node.transform.parent.GetComponent<RectTransform>();
            //Vector3 globalMousePos;
            //RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, null, out globalMousePos) ;
            //node.transform.position = globalMousePos;

            //给节点添加图标
            node.transform.Find("icon").GetComponent<Image>().sprite =
                Resources.Load<Sprite>("ui/img/tower/" + slot.data.type);
            //底部圆圈
            node.transform.Find("Q").GetComponent<Image>().sprite =
                Resources.Load<Sprite>("ui/img/tower/Q/" + "圈圈 暗");


            if (slot.data.type == "boss")
            {
                node.transform.Find("icon").GetComponent<RectTransform>().sizeDelta
                    = new Vector2(300, 300);
                node.transform.Find("icon").GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(0, 50);
            }
            if (slot.data.type == "深渊")
            {
                node.transform.Find("icon").GetComponent<RectTransform>().sizeDelta
                    = new Vector2(120, 120);
            }
        }

        UIFrameUtil.FindChildNode(this.transform, "jcPanel/role").SetAsLastSibling();

        if (index == 1)
        {
            UIFrameUtil.FindChildNode(this.transform, "jcPanel/role/descPanel/desc").GetComponent<TextMeshProUGUI>()
                .text = "The roads inside the tower are intricate, choose a path carefully to move forward!";
        }
        else if (index == 2)
        {
            UIFrameUtil.FindChildNode(this.transform, "jcPanel/role/descPanel/desc").GetComponent<TextMeshProUGUI>()
                   .text = "Every time you make a choice, it will affect the next available route. Let's move on!";
        }
    }


    IEnumerator positioning()
    {
        yield return new WaitForSeconds(0.05f);

        int nowStorey = int.Parse(towerMgr.towerData.nowNode.Split("-")[0]);
        if (nowStorey == 0)
            scrollbar.value = 0;
        else
            scrollbar.value = (nowStorey+0.0f) / (nodeMap.Count-1.0f);//(nowStorey + 1.0f) / (nodeMap.Count - 1.0f);

        if (DataManager.Get().roleAttrData.nowLevel == 1 && DataManager.Get().roleAttrData.nowLevelExp == 0) { 
            if (TowerManager.EndStorey == 0)
                JC(1);
            else if (TowerManager.EndStorey == 1)
                JC(2);
        }

        initEnd = true;
    }




    //校验该节点与当前节点是否可连接
    public bool check(string checkNode,string nowNode) {
        string[] kv = nowNode.Split("-");
        int k = int.Parse(kv[0]);
        int v = int.Parse(kv[1]);

        TowerNodeSlot slot = nodeMap[k][v];

        bool flag = false;

        //if (slot.data.nextNodeStrs != null) 
        { 
            string[] nodes = slot.data.nextNodeStrs.Split(";");
            foreach (string node in nodes) {
                if (node == checkNode) {
                    flag = true;
                }
            }
        }
        return flag;
    }

    //校验该节点与当前节点是否可连接 
    public bool checkAll(string nodeStr)
    {
        string[] kv = towerMgr.towerData.nowNode.Split("-");
        int k = int.Parse(kv[0]);
        int v = int.Parse(kv[1]);

        TowerNodeSlot slot = nodeMap[k][v];

        bool flag = false;

        string[] nodes = slot.data.nextNodeStrs.Split(";");
        foreach (string node in nodes)
        {
            if (node == nodeStr)
            {
                flag = true;
            }
        }
        return flag;
    }

    public void selectCheck(TowerMapNodeData data,bool reselection = false)
    {
        //data.nodeStr = "10-0";
        //data.type = "boss";
        if (data?.nodeStr == null)
            return;

        //校验是否可以点击
        if (/*true ||*/ check(data.nodeStr, towerMgr.towerData.nowNode) && !selectIng)
        {
            //延迟 播放一段动画 然后触发点击效果
            StartCoroutine(selectAim(data,reselection));
        }
    }
    bool selectIng;
    GameObject role_icon;
    IEnumerator selectAim(TowerMapNodeData data, bool reselection = false)
    {
        if (UIFrameUtil.FindChildNode(this.transform, "jcPanel").gameObject.activeInHierarchy) {
            UIFrameUtil.FindChildNode(this.transform, "jcPanel").gameObject.SetActive(false);
        }


        selectIng = true;
        int x = int.Parse(data.nodeStr.Split("-")[0]);
        int y = int.Parse(data.nodeStr.Split("-")[1]);
        role_icon.transform.parent = nodeMap[x][y].transform;
        role_icon.GetComponent<RectTransform>().DOMove(
           nodeMap[x][y]
           .transform.Find("icon").GetComponent<RectTransform>().position + new Vector3(0, 20), 0.75f);

        //节点动画
        /*nodeMap[0][0].transform.Find("icon").GetComponent<RectTransform>().DOMove(
           nodeMap[0][0].transform.Find("icon").GetComponent<RectTransform>().position + new Vector3(0, 200), 1.2f);
        */


        yield return new WaitForSeconds(1f);
        selectNode(data, reselection);
        selectIng = false;
    }
    void selectNode(TowerMapNodeData data, bool reselection = false) {
        //todo  只有通过了 节点才应该实际变化成此节点
        towerMgr.towerData.selecNode = data.nodeStr;
        //towerMgr.towerData.nowNode = data.nodeStr;
        DataManager.Get().userData.towerData = towerMgr.towerData;
        DataManager.Get().save();

        int index = int.Parse(data.nodeStr.Split("-")[0]);
        TowerMap tm = TowerFactory.Get().tmMap[TowerManager.nowChapter][index];
       
       /* if(data.type != "boss")
        data.type = "宝箱";*/
        
        if (data.type == "事件")
        {

            eventPanel.Refresh(tm.eventRewards);
            /*OpenForm("TowerLevelUpDescForm");
            MessageMgr.SendMsg("LevelUpDescShow", null);
            if (!reselection)
            {
                //升级
                DataManager.Get().userData.towerData.awaitUpgrade += 2;
                DataManager.Get().userData.towerData.eventNodeNum += 1;
                DataManager.Get().save();
            }*/
        }
        else if (data.type == "宝箱")
        {
            //宝物节点
            //relicPanel.Refresh(tm);
            RelicEventFlag = true;

            OpenForm("NewTowerRelicEventForm");
            MessageMgr.SendMsg("BoxRewards",
                  new MsgKV("", tm.boxRewards));
            MessageMgr.SendMsg("ForTowerMapForm",
                  new MsgKV("", this));


            /*OpenForm("TowerLevelUpDescForm");
            MessageMgr.SendMsg("LevelUpDescShow", null);
            //升级
            if (!reselection)
            {
                DataManager.Get().userData.towerData.awaitUpgrade += 2;
                DataManager.Get().userData.towerData.relicNodeNum += 1;
                DataManager.Get().save();
            }*/
        }
        else
        {
            //... 切场景 传递地图数据
            if (data.type == "boss")
            {
                TowerManager.DungeonId = tm.commonId;
                TowerManager.DungeonType = "boss";
            }
            else if (data.type == "精英")
            {
                TowerManager.DungeonId = tm.eliteId;
                TowerManager.DungeonType = "精英";

                if (!reselection)
                {
                    DataManager.Get().userData.towerData.eliteNodeNum += 1;
                    DataManager.Get().save();
                }
            }
            else if (data.type == "深渊")
            {
                TowerManager.DungeonId = tm.abyssId;
                TowerManager.DungeonType = "深渊";
            }
            else
            {
                TowerManager.DungeonId = tm.commonId;
                TowerManager.DungeonType = "普通";
                if (!reselection)
                {
                    DataManager.Get().userData.towerData.normalNodeNum += 1;
                    DataManager.Get().save();
                }
            }
            TowerManager.DungeonStorey = int.Parse(data.nodeStr.Split("-")[0]);
            SceneManager.LoadScene("loading");
            //SceneManager.LoadScene("battle");
        }
    }





    public void nodeEnd() {
        towerMgr.towerData.nowNode = towerMgr.towerData.selecNode;
        DataManager.Get().userData.towerData = towerMgr.towerData;
        TowerManager.EndStorey = int.Parse(towerMgr.towerData.nowNode.Split("-")[0]);
        DataManager.Get().save();
    }


    SettlementPanel settlementPanel;
    /// <summary>
    ///0失败
    ///1小关卡胜利
    ///2boss胜利
    ///3主动退出
    /// </summary>
    /// <param name="state"></param>
    public void end(int state) {
        settlementPanel.settlement(state);
    }
}



