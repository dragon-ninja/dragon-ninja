using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    //��ǰѡ��Ĺؿ�id
    public static string DungeonId = "";
    //��ǰ�ؿ�����
    public static string DungeonType = "";
    //��ǰ�ؿ�����
    public static int DungeonStorey = 0;
    //ͨ���Ĺؿ�����
    public static int EndStorey = 0;
    //�Ƿ�������
    public static bool TowerIng;
    //��������
    public TowerGameData towerData;
    //��ǰ�½�
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


        int ���� = 12453221;
        //���� = 223;
        ���� = Random.Range(100, 100000);
        Random.InitState(����);


        List<string> towerNumList = new List<string>();


        //���ɽڵ�����
        List<List<TowerMapNodeData>> mapList = new List<List<TowerMapNodeData>>();

        //��һ����������
        List<TowerMapNodeData> startStorey = new List<TowerMapNodeData>();
        TowerMapNodeData start = new TowerMapNodeData();
        start.type = "���";
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
                //���ɽڵ�:

                TowerMapNodeData node = new TowerMapNodeData();
                node.nodeStr = i + "-" + j;
                node.type = "��ͨ";
                nodeList.Add(node);

                towerNumList.Add(node.nodeStr);
            }

            mapList.Add(nodeList);
        }


        //ѡ����Ӣ�ؽڵ�
        //ѡ���¼��ڵ�
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
            //ǰ���ز���Ϊ��Ӣ��
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
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "��Ӣ";
        }


        //ѡ���¼��ڵ�
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
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "�¼�";
        }

        //ѡ������ڵ�
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
            //Debug.Log("����node:"+ str);
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "����";
        }
        //ѡ���̵�ڵ�   
        /* for (int i = 0; i < tdd.maxShopNode; i++)
        {
            //�̵�ڵ�������ֲ���Ϊ�ܲ�����һ��
            int nodeNum_0 = Random.Range(towerNumList.Count / 2, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "�̵�";
        }*/
      

        //ѡ����Ԩ�ؽڵ�
       /* for (int i = 0; i < tdd.maxAbyssNode; i++)
        {
            if (towerNumList.Count <= 0)
                break;
            int nodeNum_0 = Random.Range(towerNumList.Count / 2, towerNumList.Count);
            string str = towerNumList[nodeNum_0];
            towerNumList.Remove(str);
            string[] kv = str.Split('-');
            mapList[int.Parse(kv[0])][int.Parse(kv[1])].type = "��Ԩ";
        }*/

        //�յ��Ȼ��boss 
        List<TowerMapNodeData> bossStorey = new List<TowerMapNodeData>();
        TowerMapNodeData bossNode = new TowerMapNodeData();
        bossNode.nodeStr = tdd.maxStorey_clone + "-" + 0;
        bossNode.type = "boss";
        bossNode.x = 500;
        bossStorey.Add(bossNode);
        mapList.Add(bossStorey);

        int leftBoundNode = 0;

       /* mapList[1][0].type = "�¼�";
        mapList[2][1].type = "�¼�";*/

        //�Խڵ�֮���������
        for (int i = 0; i < mapList.Count - 1; i++)
        {
            //�����ӵĽڵ���߽�
            leftBoundNode = 0;
            for (int j = 0; j < mapList[i].Count; j++)
            {
                TowerMapNodeData node = mapList[i][j];

                //���õ�λ
                node.x = 1000.0f / (mapList[i].Count * 2) +
                    j * (1000.0f / mapList[i].Count) + Random.Range(-35, 35);
                node.y = Random.Range(-50, 50);

                //�����2�� �и���1�� С����3��
                int ������ӽڵ��� = Random.value > 0.45 ? 2 : Random.value > 0.3 ? 1 : 2;

                //���Լ��Ǳ������һ��node  һ��Ҫ���ӵ��ϲ���ұ߽�Ϊֹ
                if (j == mapList[i].Count - 1)
                {
                    ������ӽڵ��� = mapList[i + 1].Count - leftBoundNode;
                }

                for (int k = 0; k < ������ӽڵ���; k++)
                {
                    //����߽�С���²�ڵ�����ʱ ��߽�����,���������߽�=�ұ߽� ��������²�������
                    if (k > 0)
                    {
                        //�޷���������
                        if (leftBoundNode < mapList[i + 1].Count - 1)
                        {
                            leftBoundNode += 1;

                            node.nextNodeStrs +=
                                mapList[i + 1][leftBoundNode].nodeStr + ";";
                        }
                    }
                    //������߽�
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

    //����һ�����ظ�������¼�
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
        //�¼�ȫ���ù��� 
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


    //����һ���������
    public RelicConfig getRelic() {
        RelicConfig rc = TowerFactory.Get().relicList[Random.Range(0, TowerFactory.Get().eventList.Count)];
        return rc;
    }


}
