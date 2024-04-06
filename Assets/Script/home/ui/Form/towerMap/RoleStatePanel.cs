using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoleStatePanel : BaseUIPanel
{
    List<GameObject> attrObjList;
    Transform attrListTra;
    GameObject attrPf;



    List<GameObject> buffObjList;
    Transform buffListTra;
    GameObject buffPf;


    // Start is called before the first frame update£º
    protected override void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            gameObject.SetActive(false);
        });

        UIFrameUtil.FindChildNode(this.transform, "returnBut").
            GetComponent<Button>().onClick.AddListener(() => {
                gameObject.SetActive(false);
            });


        attrListTra = UIFrameUtil.FindChildNode(this.transform, "attrList");
        attrPf = attrListTra.GetChild(0).gameObject;
        attrObjList = new List<GameObject>();
        for (int i = 0; i<attrListTra.childCount;i++) {
            attrObjList.Add(attrListTra.GetChild(i).gameObject);
        }


        buffListTra = UIFrameUtil.FindChildNode(this.transform, "buffList");
        buffPf = buffListTra.GetChild(0).gameObject;
        buffObjList = new List<GameObject>();
        buffObjList.Add(buffListTra.GetChild(0).gameObject);
    }

    private void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        attrObjList[0].GetComponent<TextMeshProUGUI>().text =
            "Level:" + (DataManager.Get().userData.towerData.level);

        int exp =  DataManager.Get().userData.towerData.exp;
        int extraExp = DataManager.Get().userData.towerData.extraExp;
        int expMax = ExpFactory.Get().expMap[DataManager.Get().userData.towerData.level];
        string expStr = exp > 1000 ? (exp / 1000)+"."+ (exp % 1000 / 100) + "k" : exp+"";
        string extraExpStr = extraExp > 1000 ? (extraExp / 1000) + "." + (extraExp % 1000 / 100) + "k" : extraExp + "";
        string expMaxStr = expMax > 1000 ? (expMax / 1000) + "." + (expMax % 1000 / 100) + "k" : expMax + "";

        attrObjList[1].GetComponent<TextMeshProUGUI>().text =
           "Exp:" + (expStr) +"/"+(expMaxStr);
        if (extraExp > 0) {
            attrObjList[1].GetComponent<TextMeshProUGUI>().text =
              "Exp:" + (expStr) +"(+"+ (extraExpStr) + ")" + "/" + (expMaxStr);
        }

        attrObjList[1].GetComponent<TextMeshProUGUI>().text =
          "Exp:" + Mathf.Round((exp+0.0f) / (expMax + 0.0f)* 1000)/10 + "%";
        if (extraExp > 0)
            attrObjList[1].GetComponent<TextMeshProUGUI>().text =
                "Exp:" + Mathf.Round((exp + 0.0f) / (expMax + 0.0f) * 1000) / 10
                + "%(+"+ extraExpStr +")";


        int hp_max = RoleManager.Get().hp;
        int hp_now = Mathf.Min(hp_max, Mathf.FloorToInt(hp_max * DataManager.Get().userData.towerData.hpRate));
        

        attrObjList[2].GetComponent<TextMeshProUGUI>().text =
          "Hp:" + hp_now +"/" + hp_max;

        attrObjList[3].GetComponent<TextMeshProUGUI>().text =
         "Attack:" + (RoleManager.Get().attack);

        attrObjList[4].GetComponent<TextMeshProUGUI>().text =
       "Damage:" + ((RoleManager.Get().weaponDmg + 1) *100)+"%";

        attrObjList[5].GetComponent<TextMeshProUGUI>().text =
         "Defense:" + (RoleManager.Get().defense);


        foreach (GameObject g in buffObjList) {
            g.SetActive(false);
        }

        int j = 0;
        for (int i=0; i < DataManager.Get().userData.towerData.buffList.Count;i++)
        {
            string configid = DataManager.Get().userData.towerData.buffList[i];
            TowerEventBuffConfig c = TowerFactory.Get().eventBuffList.Find(x => x.id == configid);
            if (c != null) {
                if (buffObjList.Count <= j) {
                    GameObject item = Instantiate(buffPf, buffListTra);
                    buffObjList.Add(item);
                }
                buffObjList[i].transform.Find("desc").GetComponent<TextMeshProUGUI>()
                    .text = c.desc;
                if (c.icon != null)
                {
                    buffObjList[i].transform.Find("icon").gameObject.SetActive(true);
                    buffObjList[i].transform.Find("icon").GetComponent<Image>()
                       .sprite = Resources.Load<Sprite>(c.icon);
                }
                else {
                    buffObjList[i].transform.Find("icon").gameObject.SetActive(false);
                }
                buffObjList[i].SetActive(true);
                j++;
            }

        }
    }
}
