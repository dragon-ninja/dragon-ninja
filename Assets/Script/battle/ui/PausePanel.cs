using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    Transform mianWeapon;
    List<Image> itemIconList_active;
    List<Image> itemIconList_passive;
    bool initFlag;

    Sprite xxpf;
    Sprite xxhpf;
    Sprite xxheipf;

    GameObject BackPanel;

    public void init() {

        BackPanel = transform.Find("BackPanel").gameObject;

        UIFrameUtil.FindChildNode(this.transform, "cancelButton").GetComponent<Button>()
            .onClick.AddListener(() => {
                cancelBack();
            });
        UIFrameUtil.FindChildNode(this.transform, "confirmButton").GetComponent<Button>()
            .onClick.AddListener(() => {
                //弹出结算画面
                //todo 爬塔改造
                GameObject.Find("GameManager").GetComponent<DungeonManager>().settlement(3);
            });


        mianWeapon = transform.Find("Panel").Find("Panel").Find("playerInfo").Find("mianWeapon");

        itemIconList_active = new List<Image>();
        Transform list1 = transform.Find("Panel").Find("Panel").Find("items_active").Find("list");
        for(int i =0;i<4;i++) {
            Transform item = list1.GetChild(i);
            itemIconList_active.Add(item.Find("icon").GetComponent<Image>());
        }

        itemIconList_passive = new List<Image>();
        Transform list2 = transform.Find("Panel").Find("Panel").Find("items_passive").Find("list");
        for (int i = 0; i < 5; i++)
        {
            Transform item = list2.GetChild(i);
            itemIconList_passive.Add(item.Find("icon").GetComponent<Image>());
        }
        initFlag = true;

        xxpf = Resources.Load<Sprite>("ui/pause/xx");
        xxhpf = Resources.Load<Sprite>("ui/pause/xx_h");
        xxheipf = Resources.Load<Sprite>("ui/pause/xx_minh");
    }

    private void OnEnable()
    {
        if (!initFlag)
            init();

        foreach (Image i in itemIconList_active) { 
            i.gameObject.SetActive(false);
            i.transform.parent.Find("list").gameObject.SetActive(false);
        }

        foreach (Image i in itemIconList_passive)
        {
            i.gameObject.SetActive(false);
            i.transform.parent.Find("list").gameObject.SetActive(false);
        }

        int index = 0;
        foreach (var item in UpLevel.playerActiveSkillLevelInfos)
        {

            SkillAttr atr = SkillAttrFactory.Get().skillMap[item.Value.name][item.Value.level - 1];
            if (atr.skillType == "Katana")
            {
                for (int i = 0; i < 5; i++)
                {
                    mianWeapon.GetChild(i).gameObject.SetActive(true);
                    if (atr.level > i)
                        mianWeapon.GetChild(i).GetComponent<Image>().sprite = xxpf;
                    else
                        mianWeapon.GetChild(i).GetComponent<Image>().sprite = xxhpf;
                }

            }
            else { 
                itemIconList_active[index].sprite = Resources.Load<Sprite>("skill/icon/" + atr.icon);
                itemIconList_active[index].gameObject.SetActive(true);
                Transform levellist = itemIconList_active[index].transform.parent.Find("list");
                levellist.gameObject.SetActive(true);


                Debug.Log(item.Value.type+":" + item.Value.level);

                if (item.Value.level == 6 || atr.skillType == "Wind")
                {
                    itemIconList_active[index].transform.parent.
                        GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/pause/" + (index+1) + "_red");
                }
                else {
                    itemIconList_active[index].transform.parent.
                       GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/pause/" + (index + 1));
                }


                for (int i = 0; i < 5; i++)
                {
                    levellist.GetChild(i).gameObject.SetActive(true);
                    if (atr.level > i || atr.skillType == "Wind")
                        levellist.GetChild(i).GetComponent<Image>().sprite = xxpf;
                    else
                        levellist.GetChild(i).GetComponent<Image>().sprite = xxhpf;
                }
                index++;
            }
        }

        index = 0;
        foreach (var item in UpLevel.playerPassiveSkillLevelInfos)
        {
            SkillAttr atr = SkillAttrFactory.Get().skillMap[item.Value.name][item.Value.level - 1];
            itemIconList_passive[index].sprite = Resources.Load<Sprite>("skill/icon/" + atr.icon);
            itemIconList_passive[index].gameObject.SetActive(true);
            Transform levellist = itemIconList_passive[index].transform.parent.Find("list");
            levellist.gameObject.SetActive(true);
            for (int i = 0; i < 5; i++)
            {
                levellist.GetChild(i).gameObject.SetActive(true);
                if (atr.level > i)
                    levellist.GetChild(i).GetComponent<Image>().sprite = xxpf;
                else
                    levellist.GetChild(i).GetComponent<Image>().sprite = xxheipf;
            }
            index++;
        }

    }

    public void show() {
       
           
    }


    public void openBack() {
        BackPanel.SetActive(true);
    }

    public void cancelBack() {
        BackPanel.SetActive(false);
    }
}
