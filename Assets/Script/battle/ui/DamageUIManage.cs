using System.Collections.Generic;
using UnityEngine;


public class DamageUIManage : MonoBehaviour
{
    public static Transform fightCanvas;
    public static GameObject dmgPf_1;
    public static GameObject dmgPf_2;
    public static GameObject dmgPf_3;
    public static GameObject dmgPf_4;
    //治疗数字
    public static GameObject dmgPf_99;
    public static List<DamageUI> DamageUIList_1;
    public static List<DamageUI> DamageUIList_2;
    public static List<DamageUI> DamageUIList_3;
    public static List<DamageUI> DamageUIList_4;
    public static List<DamageUI> DamageUIList_99;


    static int dmgUICount_1 = 0;
    static int dmgUICount_2 = 0;
    static int dmgUICount_3 = 0;
    static int dmgUICount_4 = 0;
    static int dmgUICount_99 = 0;

    static List<int> dmgUiThresholdList;

    private void Awake()
    {
        fightCanvas = GameObject.Find("FightCanvas").transform;
        dmgPf_1 = Resources.Load<GameObject>("ui/dmg_1");
        dmgPf_2 = Resources.Load<GameObject>("ui/dmg_2");
        dmgPf_3 = Resources.Load<GameObject>("ui/dmg_3");
        dmgPf_4 = Resources.Load<GameObject>("ui/dmg_4");
        dmgPf_99 = Resources.Load<GameObject>("ui/cure_1");
        DamageUIList_1 = new List<DamageUI>();
        DamageUIList_2 = new List<DamageUI>();
        DamageUIList_3 = new List<DamageUI>();
        DamageUIList_4 = new List<DamageUI>();
        DamageUIList_99 = new List<DamageUI>();

        dmgUICount_1 = 0;
        dmgUICount_2 = 0;
        dmgUICount_3 = 0;
        dmgUICount_4 = 0;
        dmgUICount_99 = 0;
    }

    private void Start()
    {
        dmgUiThresholdList = new List<int>();
        string thresholdStr = DungeonManager.SysSetting.list[0].dmgUiThreshold;
        string[] ss = thresholdStr.Split(";");
        foreach (string s in ss) {
            dmgUiThresholdList.Add(int.Parse(s));
        }
    }

    public static void DmgUI(HitInfo hf)
    {
        int lv = 0;
        List<DamageUI> DamageUIList = null;
        int dmgCount = 0;
        GameObject dmgPf = null;

        if (hf.cureFlag)
        {
            lv = 99;
            dmgPf = dmgPf_99;
            DamageUIList = DamageUIList_99;
            dmgCount = dmgUICount_99;
        }
        else if (hf.damage < dmgUiThresholdList[0]) {
            lv = 1;
            dmgPf = dmgPf_1;
            DamageUIList = DamageUIList_1;
            dmgCount = dmgUICount_1;
        }
        else if (hf.damage < dmgUiThresholdList[1])
        {
            lv = 2;
            dmgPf = dmgPf_2;
            DamageUIList = DamageUIList_2;
            dmgCount = dmgUICount_2;
        }
        else if (hf.damage < dmgUiThresholdList[2])
        { 
            lv = 3;
            dmgPf = dmgPf_3;
            DamageUIList = DamageUIList_3;
            dmgCount = dmgUICount_3;
        }
        else 
        {
            lv = 4;
            dmgPf = dmgPf_4;
            DamageUIList = DamageUIList_4;
            dmgCount = dmgUICount_4;
        }
      

        //当激活的伤害数字ui大于x时 不再生成新的ui
        DamageUI dmgui = null;
        if (DamageUIList.Count > 0)
        {
            dmgui = DamageUIList[DamageUIList.Count - 1];
            DamageUIList.RemoveAt(DamageUIList.Count - 1);
        }
        else if (dmgCount < 30)
        {
            GameObject dmgobj = Instantiate(dmgPf);
            dmgui = dmgobj.GetComponent<DamageUI>();
           
            if (lv == 1) 
                dmgUICount_1++;
            if (lv == 2)
                dmgUICount_2++;
            if (lv == 3)
                dmgUICount_3++;
            if (lv == 4)
                dmgUICount_4++;
            if (lv == 99)
                dmgUICount_99++;
        }
        if (dmgui != null)
        {
            dmgui.transform.parent = fightCanvas;
            dmgui.lv = lv;
            dmgui.init(hf);
            dmgui.gameObject.SetActive(true);
        }
    }

    //限制数量
    public static void creatureDmgUI(HitInfo hf)
    {
        DmgUI(hf);
    }

    public static void destroyDmgUI(DamageUI ui,int lv) {
        
        ui.gameObject.SetActive(false);

        if (lv == 1)
            DamageUIList_1.Add(ui);
        if (lv == 2)
            DamageUIList_2.Add(ui);
        if (lv == 3)
            DamageUIList_3.Add(ui);
        if (lv == 4)
            DamageUIList_4.Add(ui);
        if (lv == 99)
            DamageUIList_99.Add(ui);
    }
}
