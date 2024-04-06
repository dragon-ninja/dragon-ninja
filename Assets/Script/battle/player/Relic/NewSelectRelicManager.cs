using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSelectRelicManager : MonoBehaviour
{
    List<Relic> now_RelicList;
    List<NewTowerRelicSlot> selectSlotList;
    bool initFlag;


    private void OnEnable()
    {
     
    }

    public void Show() {
        if (!initFlag)
        {
            initFlag = true;
            now_RelicList = new List<Relic>();
            selectSlotList = new List<NewTowerRelicSlot>();
            Transform relicTra = UIFrameUtil.FindChildNode(this.transform, "relicList");
            for (int i = 0; i < relicTra.childCount; i++)
            {
                selectSlotList.Add(relicTra.GetChild(i).GetComponent<NewTowerRelicSlot>());
                selectSlotList[i].mgr = this;
                selectSlotList[i].index = i;
            }
        }
        Time.timeScale = 0;
        this.gameObject.SetActive(true);
        creatRelic(-1);
    }


    public void creatRelic(int Q)
    {
        //获取不重复的宝物
        List<RelicConfig> RelicConfigList = new List<RelicConfig>();
        if (Q != -1)
        {
            foreach (RelicConfig c in TowerFactory.Get().relicList.FindAll(x => x.quality == Q))
            {
                RelicConfigList.Add(c);
            }
        }
        else
        {
            foreach (RelicConfig c in TowerFactory.Get().relicList)
            {
                RelicConfigList.Add(c);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, RelicConfigList.Count);
            RelicConfig config = RelicConfigList[index];
            RelicConfigList.RemoveAt(index);
            Relic relic = new Relic();
            relic.configId = config.id;
            relic.level = 0;
            relic.quality = config.quality;
            now_RelicList.Add(relic);
            selectSlotList[i].Refresh(relic);
        }
    }

    public void selectRelic(int index)
    {
        DataManager.Get().userData.towerData.relicList.Add(now_RelicList[index]);
        DataManager.Get().save();
        now_RelicList.Clear();


        if (DungeonManager.jySelectRelicNum == 0)
            this.gameObject.SetActive(false);

        Time.timeScale = GameSceneManage.nowTimeScale;
    }
}
