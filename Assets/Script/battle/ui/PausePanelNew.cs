using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PausePanelNew : MonoBehaviour
{

    Transform weaponTra;
    Transform armTra;
    Transform relicTra;
    public List<TowerBackPackSlot> weaponSlots = new List<TowerBackPackSlot>();
    //public List<TowerBackPackSlot> armSlots = new List<TowerBackPackSlot>();
    public List<TowerBackPackSlot> relicSlots = new List<TowerBackPackSlot>();

    GameObject relicSlotPf;

    public TextMeshProUGUI desc;
    public TextMeshProUGUI num_1;
    public TextMeshProUGUI num_2;
    public TextMeshProUGUI num_3;

    GameSceneManage gameSceneManage;
    GameObject BackPanel;
    GameObject DamagePanel;

    void Awake()
    {

        DamagePanel = transform.parent.Find("DamagePanel").gameObject;
        UIFrameUtil.FindChildNode(this.transform.parent, "But_Dmg").GetComponent<Button>()
           .onClick.AddListener(() => {
               DamagePanel.SetActive(true);
           });


        BackPanel = transform.parent.Find("BackPanel").gameObject;
        UIFrameUtil.FindChildNode(this.transform.parent, "But_OpenBack").GetComponent<Button>()
           .onClick.AddListener(() => {
               BackPanel.SetActive(true);
           });
        UIFrameUtil.FindChildNode(this.transform.parent, "cancelButton").GetComponent<Button>()
           .onClick.AddListener(() => {
               BackPanel.SetActive(false);
           });
        UIFrameUtil.FindChildNode(this.transform.parent, "confirmButton").GetComponent<Button>()
            .onClick.AddListener(() => {
                //弹出结算画面
                GameObject.Find("GameManager").GetComponent<DungeonManager>().settlement(3);
            });

        gameSceneManage = GameObject.Find("GameManager").GetComponent<GameSceneManage>();
        GetComponent<Button>().onClick.AddListener(() => {
            gameSceneManage.EndPauseGame();
        });


        weaponTra = UIFrameUtil.FindChildNode(this.transform, "weaponList");
        armTra = UIFrameUtil.FindChildNode(this.transform, "armList");
        relicTra = UIFrameUtil.FindChildNode(this.transform, "relicList");
        for (int i = 0; i < weaponTra.childCount; i++)
        {
            weaponSlots.Add(weaponTra.GetChild(i).GetComponent<TowerBackPackSlot>());
            weaponSlots[i].type = "weapon_battle";
            weaponSlots[i].mgr = this;
        }
        for (int i = 0; i < armTra.childCount; i++)
        {
            weaponSlots.Add(armTra.GetChild(i).GetComponent<TowerBackPackSlot>());
            weaponSlots[weaponSlots.Count - 1].type = "weapon_battle";
            weaponSlots[weaponSlots.Count - 1].mgr = this;
            /*armSlots.Add(armTra.GetChild(i).GetComponent<TowerBackPackSlot>());
            armSlots[i].type = "arm_battle";
            armSlots[i].mgr = this;*/
        }
        for (int i = 0; i < relicTra.childCount; i++)
        {
            relicSlots.Add(relicTra.GetChild(i).GetComponent<TowerBackPackSlot>());
            relicSlots[i].type = "relic_battle";
            relicSlots[i].mgr = this;
        }
        relicSlotPf = relicTra.GetChild(0).gameObject;

        desc = UIFrameUtil.FindChildNode(this.transform, "descText").GetComponent<TextMeshProUGUI>();
        num_1 = UIFrameUtil.FindChildNode(this.transform, "list/1/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_2 = UIFrameUtil.FindChildNode(this.transform, "list/2/Text (TMP)").GetComponent<TextMeshProUGUI>();
        num_3 = UIFrameUtil.FindChildNode(this.transform, "list/3/Text (TMP)").GetComponent<TextMeshProUGUI>();
        Refresh();
    }

    public void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {

        num_1.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_1;
        num_2.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_2;
        num_3.text = "" + DataManager.Get().userData.towerData.relicEssenceNum_3;



        for (int i = 0; i < weaponSlots.Count; i++)
        {
            weaponSlots[i].Refresh();
        }
        /*for (int i = 0; i < armSlots.Count; i++)
        {
            armSlots[i].Refresh();
        }*/

        //int armNum = 0;
        int wpNum = 1;
        if (DataManager.Get().userData.towerData.skillInfo != null){
            foreach (var item in DataManager.Get().userData.towerData.skillInfo)
            {
                SkillAttr skillAttr = SkillAttrFactory.Get().skillMap[item.Key][item.Value - 1];
                if (skillAttr.mainWeaponFlag)
                {
                    weaponSlots[0].Refresh(skillAttr);
                }
                /*else if (skillAttr.armFlag)
                {
                    armSlots[armNum++].Refresh(skillAttr);
                }*/
                else
                {
                    weaponSlots[wpNum++].Refresh(skillAttr);
                }
            }
        }

        Dictionary<string, int> now_relicList = new Dictionary<string, int>();
        List<Relic> relicList = DataManager.Get().userData.towerData.relicList;
        for (int i = 0; i < relicList.Count; i++)
        {
            RelicConfig config = TowerFactory.Get().relicMap[relicList[i].configId];
            relicList[i].quality = config.quality;
        }
        relicList = (List<Relic>)(relicList.OrderBy(o => o.quality).ThenBy(o => o.level).ToList());
        if (relicList != null)
        {
            for (int i = 0; i < relicList.Count; i++)
            {
                if (now_relicList.ContainsKey(relicList[i].configId + "_" + relicList[i].level))
                {
                    now_relicList
                        [relicList[i].configId + "_" + relicList[i].level]
                        += 1;
                }
                else
                {
                    now_relicList
                            [relicList[i].configId + "_" + relicList[i].level]
                            = 1;
                }
            }
            //建立对应的槽位  多余的隐藏
            for (int i = 0; i < now_relicList.Count || i < relicSlots.Count; i++)
            {
                if (i >= relicSlots.Count)
                {
                    GameObject g = Instantiate(relicSlotPf, relicTra);
                    TowerBackPackSlot slot = g.GetComponent<TowerBackPackSlot>();
                    slot.mgr = this;
                    slot.type = "relic_battle";
                    relicSlots.Add(slot);
                }
                relicSlots[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach (var item in now_relicList)
            {
                string[] s = item.Key.Split("_");
                relicSlots[index].Refresh(s[0], int.Parse(s[1]), item.Value);
                index++;
            }


            //应策划要求改成每个遗物单独一个槽显示,不堆叠
            /*for (int i = 0; i < relicList.Count
               || i < relicSlots.Count; i++)
            {
                if (i >= relicSlots.Count)
                {
                    GameObject g = Instantiate(relicSlotPf, relicTra);
                    TowerBackPackSlot slot = g.GetComponent<TowerBackPackSlot>();
                    slot.mgr = this;
                    slot.type = "relic_battle";
                    relicSlots.Add(slot);
                }
                relicSlots[i].gameObject.SetActive(false);
            }

            int index = 0;
            foreach (var item in relicList)
            {
                relicSlots[index++].Refresh(item.configId, item.level);
            }*/
        }

    }

    public void ShowInfo(string str)
    {
        desc.text = str;
    }


}
