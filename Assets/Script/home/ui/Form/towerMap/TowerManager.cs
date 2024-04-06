using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    //当前选择的关卡id
    public static string DungeonId = "";
    //当前关卡类型
    public static string DungeonType = "";
    //当前关卡层数
    public static int DungeonStorey = 0;
    //通过的关卡层数
    public static int EndStorey = 0;
    //是否爬塔中
    public static bool TowerIng;
    //爬塔数据
    public TowerGameData towerData;
    //当前章节
    public static string nowChapter;

    void Start()
    {
        Application.targetFrameRate = 120;
        DataManager.Get().init();
        UIManager.GetUIMgr().showUIForm("TowerMapForm");
    }

    public List<List<TowerMapNodeData>> creatTowerMap(int chapterIndex)
    {
        TowerManager.nowChapter = TowerFactory.Get().chapterList[chapterIndex];
        TowerMap tdd = TowerFactory.Get().tmMap[nowChapter][0];


        int 种子 = 12453221;
        //种子 = 223;
        种子 = Random.Range(100, 100000);
        Random.InitState(种子);


        List<string> towerNumList = new List<string>();


        //生成节点数据
        List<List<TowerMapNodeData>> mapList = new List<List<TowerMapNodeData>>();

        //第一层是玩家起点
        List<TowerMapNodeData> startStorey = new List<TowerMapNodeData>();
        TowerMapNodeData start = new TowerMapNodeData();
        start.type = "起点";
        start.nodeStr = 0 + "-" + 0;
        startStorey.Add(start);
        mapList.Add(startStorey);


        Debug.Log(TowerManager.nowChapter + "------------" + TowerFactory.Get().tmMap[TowerManager.nowChapter].Count);
        tdd.maxStorey_clone = TowerFactory.Get().tmMap[TowerManager.nowChapter].Count - 1;

        for (int i = 1; i < tdd.maxStorey_clone; i++)
        {
            List<TowerMapNodeData> nodeList = new List<TowerMapNodeData>();
            int nodeNum = Random.Range(tdd.minNode, tdd.maxNode + 1);
            for (int j = 0; j < nodeNum; j++)
            {
                //生成节点:

                TowerMapNodeData node = new TowerMapNodeData();
                node.nodeStr = i + "-" + j;
                node.type = "普通";
                nodeList.Add(node);

                towerNumList.Add(node.nodeStr);
            }

            mapList.Add(nodeList);
        }


        //选出精英关节点
        //选出事件节点
        int EliteNode = 0;
        if (tdd.maxEventNode.Contains("|"))
        {
            EliteNode = Random.Range(int.Parse(tdd.maxEliteNode.Split("|")[0]),
                int.Parse(tdd.maxEliteNode.Split("|")[1]) + 1);
        }
        else
        {
            EliteNode = int.Parse(tdd.maxEliteNode);
        }
        for (int i = 0; i < EliteNode; i++)
        {
            if (towerNumList.Count <= 0)
                break;

            int index = 1;
            //前几关不能为精英关
            for (int j = 0; j < towerNumList.Count; j++)
            {
                string[] kv_0 = towerNumList[j].Split('-');
                if (int.Parse(kv_0[0]) <= 2)
                {
                    index++;
                }
            }

            Debug.Log("index:" + index + "   =====" + towerNumList.Count);

            int nodeNum_0 = Random.Range(index, towerNumList.Count);

            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "精英";
        }


        //选出事件节点
        int EventNode = 0;
        if (tdd.maxEventNode.Contains("|"))
        {
            EventNode = Random.Range(int.Parse(tdd.maxEventNode.Split("|")[0]), 
                int.Parse(tdd.maxEventNode.Split("|")[1]) + 1);
        }
        else {
            EventNode = int.Parse(tdd.maxEventNode);
        }
        for (int i = 0; i < EventNode; i++)
        {
            if (towerNumList.Count <= 0)
                break;

            int nodeNum_0 = Random.Range(towerNumList.Count / 5, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "事件";
        }

        //选出宝箱节点
        int BoxNode = 0;
        if (tdd.maxEventNode.Contains("|"))
        {
            BoxNode = Random.Range(int.Parse(tdd.maxBoxNode.Split("|")[0]),
                int.Parse(tdd.maxBoxNode.Split("|")[1]) + 1);
        }
        else
        {
            BoxNode = int.Parse(tdd.maxBoxNode);
        }
        for (int i = 0; i < BoxNode; i++)
        {
            if (towerNumList.Count <= 0)
                break;

            int nodeNum_0 = Random.Range(towerNumList.Count / 4, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            //Debug.Log("宝箱node:"+ str);
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "宝箱";
        }
        //选出商店节点   
        /* for (int i = 0; i < tdd.maxShopNode; i++)
        {
            //商店节点最早出现层数为总层数的一半
            int nodeNum_0 = Random.Range(towerNumList.Count / 2, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "商店";
        }*/
      

        //选出深渊关节点
       /* for (int i = 0; i < tdd.maxAbyssNode; i++)
        {
            if (towerNumList.Count <= 0)
                break;
            int nodeNum_0 = Random.Range(towerNumList.Count / 2, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "深渊";
        }*/

        //终点必然是boss 
        List<TowerMapNodeData> bossStorey = new List<TowerMapNodeData>();
        TowerMapNodeData bossNode = new TowerMapNodeData();
        bossNode.nodeStr = tdd.maxStorey_clone + "-" + 0;
        bossNode.type = "boss";
        bossNode.x = 500;
        bossStorey.Add(bossNode);
        mapList.Add(bossStorey);

        int leftBoundNode = 0;

       /* mapList[1][0].type = "事件";
        mapList[2][1].type = "事件";*/

        //对节点之间进行连线
        for (int i = 0; i < mapList.Count - 1; i++)
        {
            //可连接的节点左边界
            leftBoundNode = 0;
            for (int j = 0; j < mapList[i].Count; j++)
            {
                TowerMapNodeData node = mapList[i][j];

                //设置点位
                node.x = 1000.0f / (mapList[i].Count * 2) +
                    j * (1000.0f / mapList[i].Count) + Random.Range(-35, 35);
                node.y = Random.Range(-50, 50);

                //大概率2个 中概率1个 小概率3个
                int 最大连接节点数 = Random.value > 0.45 ? 2 : Random.value > 0.3 ? 1 : 2;

                //当自己是本层最后一个node  一定要链接到上层的右边界为止
                if (j == mapList[i].Count - 1)
                {
                    最大连接节点数 = mapList[i + 1].Count - leftBoundNode;
                }

                for (int k = 0; k < 最大连接节点数; k++)
                {
                    //当左边界小于下层节点数量时 左边界增长,否则就是左边界=右边界 这种情况下不再增长
                    if (k > 0)
                    {
                        //无法继续链接
                        if (leftBoundNode < mapList[i + 1].Count - 1)
                        {
                            leftBoundNode += 1;

                            node.nextNodeStrs +=
                                mapList[i + 1][leftBoundNode].nodeStr + ";";
                        }
                    }
                    //链接左边界
                    else
                    {
                        node.nextNodeStrs +=
                            mapList[i + 1][leftBoundNode].nodeStr + ";";
                    }
                }
                node.nextNodeStrs = node.nextNodeStrs.Substring(0, node.nextNodeStrs.Length - 1);
                //Debug.Log(node.nodeStr+"  :  "+node.nextNodeStrs);
            }
        }
        return mapList;
    }

    //返回一个不重复的随机事件
    public TowerEvent getEvent (){

        //DataManager.Get().userData.towerData.eventList
        List<TowerEventConfig> eventConfigs = new List<TowerEventConfig>();
     
        for (int i = 0; i < TowerFactory.Get().eventList.Count; i++) {

            if (DataManager.Get().userData.towerData != null &&
                DataManager.Get().userData.towerData.eventList != null &&
                DataManager.Get().userData.towerData.eventList.Count > 0)
            {
                if (DataManager.Get().userData.towerData.eventList.IndexOf(TowerFactory.Get().eventList[i].id) == -1)
                {
                    eventConfigs.Add(TowerFactory.Get().eventList[i]);
                }
            }
            else {
                eventConfigs.Add(TowerFactory.Get().eventList[i]);
            }
        }
        //事件全被用过了 
        if (eventConfigs.Count == 0) {
            for (int i = 0; i < TowerFactory.Get().eventList.Count; i++)
                eventConfigs.Add(TowerFactory.Get().eventList[i]);
        }

        TowerEventConfig tec = eventConfigs[Random.Range(0, eventConfigs.Count)];
        TowerEvent te = new TowerEvent();
        te.config = tec;
        te.init();

        

        return te;
    }


    //返回一个随机宝物
    public RelicConfig getRelic() {
        RelicConfig rc = TowerFactory.Get().relicList[Random.Range(0, TowerFactory.Get().eventList.Count)];
        return rc;
    }


}
