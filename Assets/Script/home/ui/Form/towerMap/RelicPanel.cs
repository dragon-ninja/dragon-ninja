using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicPanel : BaseUIPanel
{
    public TowerMapForm mapForm;

    TowerManager towerMgr;
    public List<TextMeshProUGUI> infoList;
    bool awakeFlag;

    protected override void Awake()
    {
        if (awakeFlag)
            return;

        awakeFlag = true;

        base.Awake();

        towerMgr = GameObject.Find("UIManager").GetComponent<TowerManager>();

        //接收遗物
        UIFrameUtil.FindChildNode(this.transform, "ReceiveBut").
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Hide();
                mapForm.RefreshStorey();
            });
        //放弃遗物
        UIFrameUtil.FindChildNode(this.transform, "AbandonBut").
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Hide();
                mapForm.RefreshStorey();
            });

        Transform infoTra = UIFrameUtil.FindChildNode(this.transform, "InfoList");

        for (int i = 0; i < infoTra.childCount; i++)
        {
            TextMeshProUGUI text = infoTra.GetChild(i).Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            infoList.Add(text);
        }
    }

    //显示事件内容
    public void Refresh(TowerMap tm)
    {
        Awake();

        //获得金钱
        string[] strs = tm.boxGold.Split("-");
        int min = int.Parse(strs[0]);
        int max = int.Parse(strs[1]);
        int gold = Random.Range(min, max + 1);

        infoList[0].text = gold + " Gold";

        //获得宝物  显示宝物效果
        RelicConfig rc = towerMgr.getRelic();
        infoList[1].text = rc.name;
        infoList[2].text = rc.desc_1;
        base.Show();
    }
}
