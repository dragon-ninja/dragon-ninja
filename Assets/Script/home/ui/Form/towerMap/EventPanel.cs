using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventPanel : BaseUIPanel
{
    public TowerMapForm mapForm;
    TowerManager towerMgr;

    public TowerEventConfig nowEventConfig;
    public TowerEvent nowEvent;

    SelectRelicManager selectRelicManager;


    //当前的选择索引
    public int nowSelectIndex;
    public TextMeshProUGUI desc;
    public List<EventSlot> butList;

    //表示已经选择过了 再次选择就是关闭面板
    bool selectEndFlag;
    bool awakeFlag;

    GameObject itemInfo;

    protected override void Awake()
    {
        if (awakeFlag)
            return;

        awakeFlag = true;

        base.Awake();

        towerMgr = GameObject.Find("UIManager").GetComponent<TowerManager>();
        selectRelicManager = transform.parent.Find("selet_relic").GetComponent<SelectRelicManager>();

        MessageMgr.AddMsgListener("selectEvent", p =>
        {
            Select((int)p.Value);
        });


        desc = UIFrameUtil.FindChildNode(this.transform, "Desc").GetComponent<TextMeshProUGUI>();
        itemInfo = UIFrameUtil.FindChildNode(this.transform, "itemInfo").gameObject;
        Transform ButList = UIFrameUtil.FindChildNode(this.transform, "ButList");

        for (int i=0;i< ButList.childCount;i++) {
            EventSlot slot = ButList.GetChild(i).GetComponent<EventSlot>();
            slot.index = i;
            butList.Add(slot); 
        }
    }

    //显示事件内容
    public void Refresh(List<KeyValue> data) {
        Awake();

        boxRewards = data;
        itemInfo.SetActive(false);
        selectEndFlag = false;
        nowRelic = null;
        nowBuff = null;

        nowEvent = towerMgr.getEvent();

        DataManager.Get().userData.towerData.eventList.Add(nowEvent.config.id);

        desc.text = nowEvent.config.desc;

        for (int i = 0; i < butList.Count; i++) { 
            butList[i].Hide();
            if (i < nowEvent.selectList.Count) {
                EventSlot slot = butList[i];
                slot.Refresh(nowEvent.selectList[i]);
            }
        }

        base.Show();

        //有此增益获得一个随机宝物
        foreach (string s in DataManager.Get().userData.towerData.buffList) {
            Debug.Log("sssssssssssss      "+ s);
        }
        if (DataManager.Get().userData.towerData.buffList.Contains("buff_3")) //buff_event_relic:1
        {
            selectRelicManager.show(getRelic(-1), false);
        }

       
    }

    Relic nowRelic;
    string nowBuff;
    List<KeyValue> boxRewards;

    //选择事件产生影响
    public void Select(int index) {

        if (selectEndFlag) {
            //直接关闭panel
            mapForm.nodeEnd();
            Hide();
            mapForm.RefreshStorey();

            if (nowRelic != null) {
               //selectRelicManager.show(nowRelic, false);
            }

            return;
        }
        
        selectEndFlag = true;

        //结果显示
        string result = nowEvent.resultList[index];
        //实际效果
        string effect = nowEvent.effectList[index];

        if (effect == "null")
        {
        }
        else {
            string[] effectStrs = null;
            if (effect.IndexOf(";") != -1)
            {
                effectStrs = effect.Split(";");
            }
            else {
                effectStrs = new string[]{effect};
            }
            for (int i = 0;i < effectStrs.Length;i++) {

                if (effectStrs[i].Contains(":"))
                {
                    string[] kv = effectStrs[i].Split(":");
                    string key = kv[0];
                    string value = kv[1];

                    if (key == "nowHp")
                    {
                        if (float.Parse(value) < 0)
                        {
                            //扣血以当前血量为判定
                            DataManager.Get().userData.towerData.hpRate =
                                Mathf.Clamp(DataManager.Get().userData.towerData.hpRate +
                                DataManager.Get().userData.towerData.hpRate * float.Parse(value), 0.05f, 1);
                        }
                        else { 
                            //加血以最大学历为判定
                            DataManager.Get().userData.towerData.hpRate =
                                Mathf.Clamp(DataManager.Get().userData.towerData.hpRate + float.Parse(value), 0.05f, 1);
                        }

                    }
                    else if (key == "maxHp")
                    {

                    }
                   /* else if (key.Contains("buff_"))
                    {
                        //DataManager.Get().userData.towerData.buffList.Add(effectStrs[i]);
                    }*/
                    else if (key == "relic")
                    {
                        if (value == "random")
                        {
                            nowRelic = getRelic(-1);
                            // selectRelicManager.show(getRelic(-1), false);
                        }
                        else if (value == "random_common")
                        {
                            nowRelic = getRelic(0);
                            // selectRelicManager.show(getRelic(0), false);
                        }
                        else if (value == "random_rare")
                        {
                            nowRelic = getRelic(1);
                            // selectRelicManager.show(getRelic(1), false);
                        }
                        else if (value == "random_legend")
                        {
                            nowRelic = getRelic(2);
                            // selectRelicManager.show(getRelic(2), false);
                        }
                        else
                        {
                            //特定宝物
                        }
                    }
                }
                else if(effectStrs[i].Contains("buff_")) {
                     DataManager.Get().userData.towerData.buffList.Add(effectStrs[i]);
                     nowBuff = effectStrs[i];
                }
            }
        }

        if (result == "null")
        {
            //直接关闭panel
            Hide();
        }
        else {
            //显示说明
            ShowResult(result);
        }
    }

    public void ShowResult(string str) {
       
        //显示结果信息
        desc.text = str;

        if (nowRelic != null)
        {
            itemInfo.SetActive(true);
            RelicConfig config = TowerFactory.Get().relicMap[nowRelic.configId];

            //显示遗物的属性
            itemInfo.transform.Find("icon").GetComponent<Image>()
                   .sprite = Resources.Load<Sprite>(config.icon);
            itemInfo.transform.Find("desc").GetComponent<TextMeshProUGUI>().text
                = config.name + "\r\n" + config.desc_1;
            itemInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/tower/towerBackPack/" + config.quality);
            itemInfo.transform.Find("icon").GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            DataManager.Get().userData.towerData.relicList.Add(nowRelic);
            DataManager.Get().save();
        }
        else if (nowBuff != null) {
            itemInfo.SetActive(true);
            TowerEventBuffConfig config = TowerFactory.Get().eventBuffList.Find(x => x.id == nowBuff);
            //显示遗物的属性
            itemInfo.transform.Find("icon").GetComponent<Image>()
                   .sprite = Resources.Load<Sprite>(config.icon);
            itemInfo.transform.Find("desc").GetComponent<TextMeshProUGUI>().text
                = config.desc;
            itemInfo.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/img/tower/towerBackPack/Event");

            itemInfo.transform.Find("icon").GetComponent<RectTransform>().sizeDelta = new Vector2(120,120);
        }

        //显示离去按钮
        for (int i = 0; i < butList.Count; i++)
        {
            butList[i].Hide();
            if (i < 1)
            {
                int exp = (boxRewards.Find(x => x.key == "exp").value);
                string expStr = exp+"";
                if (exp > 1000) {
                    expStr = (exp/1000) + "k";
                }
                DataManager.Get().userData.towerData.extraExp += exp;
                butList[i].Refresh("leave(Exp+" + expStr+")");
            }
        }
    }





    Relic getRelic(int Q)
    {
        List<RelicConfig> RelicConfigList = null;
        if (Q == -1) {
            RelicConfigList = TowerFactory.Get().relicList;
        }
        else {
            RelicConfigList =
            TowerFactory.Get().relicList.FindAll(x => x.quality == Q);
        } 


        RelicConfig config = RelicConfigList[Random.Range(0, RelicConfigList.Count)];
        Relic relic = new Relic();
        relic.configId = config.id;
        relic.level = 0;
        relic.quality = config.quality;

        return relic;
    }
}
