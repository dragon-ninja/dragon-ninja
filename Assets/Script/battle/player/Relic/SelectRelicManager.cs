using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectRelicManager : MonoBehaviour
{
    bool initflag;

    public static Player player;

    List<RelicConfig> RelicConfigList;

    public Image dk;
    public Image icon;
    TextMeshProUGUI towerDesc;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI num_1;
    public TextMeshProUGUI num_2;
    public TextMeshProUGUI num_3;
    Relic now_relic;
    bool forGame;

    public void init() {

        if (GameObject.Find("role") != null)
            player = GameObject.Find("role").GetComponent<Player>();

        initflag = true;
        RelicConfigList = TowerFactory.Get().relicList;
        dk = transform.Find("item/dk").GetComponent<Image>();
        icon = transform.Find("item/icon").GetComponent<Image>();
        desc = transform.Find("GameObject/desc").GetComponent<TextMeshProUGUI>();
        num_1 = transform.Find("list/1/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_2 = transform.Find("list/2/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_3 = transform.Find("list/3/Text (TMP)").GetComponent<TextMeshProUGUI>();

        transform.Find("GameObject/getButton").GetComponent<Button>().onClick.AddListener(() => {
            get();
        });
        transform.Find("recoveryButton").GetComponent<Button>().onClick.AddListener(() => {
            recovery();
        });

        if(transform.Find("towerDesc")!=null)
            towerDesc = transform.Find("towerDesc").GetComponent<TextMeshProUGUI>();
    }

    public void show(Relic relic = null, bool forGame = true) {
        this.forGame = forGame;

        if (!initflag)
            init();

        if (towerDesc!=null) { 
            TowerMap nowChapter = TowerFactory.Get().tmList.Find(x =>
                x.id == TowerFactory.Get().chapterList[DataManager.Get().now_chapterIndex]);

            string start = nowChapter.name.Substring(0, nowChapter.name.IndexOf("-"));
            string end = nowChapter.name.Substring(nowChapter.name.IndexOf("-") + 1);
            towerDesc.text = nowChapter.chapter+"\r\n" + (TowerManager.DungeonStorey + int.Parse(start)-1) + "/" + end+ "\r\n<size=70><color=#FFFF00>complete";
        }

       

        Time.timeScale = 0;

        //num_1.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_1;
        //num_2.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_2;
        //num_3.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_3;


        //获取一个随机遗物
        if (relic != null)
        {
            now_relic = relic;
        }
        else {
            now_relic = creatRelic();
        }

        RelicConfig config = TowerFactory.Get().relicMap[now_relic.configId];

        //显示遗物的属性
        icon.sprite = Resources.Load<Sprite>(config.icon);
        dk.sprite = Resources.Load<Sprite>("ui/img/tower/towerBackPack/" + config.quality);
        desc.text = config.name + "\r\n" + config.desc_1;
        this.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate
          (desc.transform.parent.GetComponent<RectTransform>());
    }

    public void get() {
        DataManager.Get().userData.towerData.relicList.Add(now_relic);
        DataManager.Get().save();

        Time.timeScale = GameSceneManage.nowTimeScale;
        if (DungeonManager.selectRelicNum == 0 || !this.forGame)
            this.gameObject.SetActive(false);

        if (player != null)
            player.changeDlyEsUI();
    }

    public void recovery() {

        RelicConfig config = TowerFactory.Get().relicMap[now_relic.configId];

        if (config.quality == 0)
            DataManager.Get().userData.towerData.relicEssenceNum_1 += 1;
        else if (config.quality == 1)
            DataManager.Get().userData.towerData.relicEssenceNum_2 += 1;
        else if (config.quality == 2)
            DataManager.Get().userData.towerData.relicEssenceNum_3 += 1;

        DataManager.Get().save();

        Time.timeScale = GameSceneManage.nowTimeScale;
        if (DungeonManager.selectRelicNum == 0 || !this.forGame)
            this.gameObject.SetActive(false);
    }


    public Relic creatRelic() {
        //获取一个随机遗物
        RelicConfig config = RelicConfigList
            [Random.Range(0, RelicConfigList.Count)];

        //品质相关:
        if (Random.value<= RoleManager.Get().relicQUp) { 
        
        }

        Relic relic = new Relic();
        //relic.id = config.id + "_" + player.relicList.Count;
        relic.configId = config.id;
        relic.level = 0;
        relic.quality = config.quality;
        return relic;
    }
}
