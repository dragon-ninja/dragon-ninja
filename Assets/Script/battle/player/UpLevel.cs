using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;


public class SkillLevelInfo {
    public string name;
    public int level;
    public string breach;
    public string type;
    public SkillAttr atr;

    public SkillLevelInfo(SkillAttr atr,string name,int level,string breach = null,string type="skill") {
        this.atr = atr;
        this.name = name;
        this.level = level;
        this.breach = breach;
        this.type = type;
    }

}


public class SelectObj {
    public string type; //skill  or  relic
    public SkillAttr skill;
    public Relic relic;
}


public class UpLevel : MonoBehaviour
{

    public static Player player;
    Transform skillList;

    //所有技能
    public static Dictionary<string, SkillLevelInfo> allSkillLevelInfos;
    //角色现有的主动技能组
    public static Dictionary<string, SkillLevelInfo> playerActiveSkillLevelInfos;
    //角色现有的被动技能组
    public static Dictionary<string, SkillLevelInfo> playerPassiveSkillLevelInfos = new Dictionary<string, SkillLevelInfo>();
    //从可选技能中最终筛选后的技能组  用于三选一
    public List<SkillAttr> selectSkillList;

    //新逻辑   混合存放了技能/宝物,选择后获取其中指定索引的技能or宝物
    public List<SelectObj> selectList;

    bool initFlag = false;

    //显示已激活的技能
    public List<Image> ActiveIconList;
    public List<Image> PassiveIconList;


    //public static int wpSkillNum;
    //public static int armSkillNum;

    bool awaitUpgradeFlag;

    public void up(bool awaitUpgradeFlag = false)
    {
        this.init();
        this.awaitUpgradeFlag = awaitUpgradeFlag;
        //显示已有技能
        {
            int index = 0;
            foreach (var item in playerActiveSkillLevelInfos) {
                SkillAttr atr = SkillAttrFactory.Get().skillMap[item.Value.name][0];
                ActiveIconList[index].gameObject.SetActive(true);
                ActiveIconList[index].sprite 
                    = Resources.Load<Sprite>("skill/icon/" + atr.icon);
                index++;

                if (index == 5)
                    break;
            }
            for (int i = index; i < ActiveIconList.Count;i++) {
                ActiveIconList[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < PassiveIconList.Count; i++)
            {
                PassiveIconList[i].gameObject.SetActive(false);
                if (i == 4)
                {
                    PassiveIconList[i].transform.parent.GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("ui/uplevel/边缘不加粗");
                    PassiveIconList[i].transform.parent.GetComponent<RectTransform>().sizeDelta 
                        = new Vector2(100, 100);
                }
                else {
                    PassiveIconList[i].transform.parent.GetComponent<Image>().sprite
                       = Resources.Load<Sprite>("ui/uplevel/不加粗");
                }
            }

            index = 0;
            foreach (var item in playerPassiveSkillLevelInfos)
            {
                SkillAttr atr = SkillAttrFactory.Get().skillMap[item.Value.name][0];
                PassiveIconList[index].gameObject.SetActive(true);
                PassiveIconList[index].sprite
                    = Resources.Load<Sprite>("skill/icon/" + atr.icon);

                if (atr.skillForm == "weaponBuff") {
                    if (index == 4) { 
                        PassiveIconList[index].transform.parent.GetComponent<Image>().sprite
                            = Resources.Load<Sprite>("ui/uplevel/边缘加粗");
                        PassiveIconList[index].transform.parent.GetComponent<RectTransform>().sizeDelta
                            = new Vector2(103.2f, 103);
                    }
                    else 
                        PassiveIconList[index].transform.parent.GetComponent<Image>().sprite
                           = Resources.Load<Sprite>("ui/uplevel/加粗");
                }
                index++;

                if (index == 5)
                    break;
            }
         
        }


        //所有可选择的技能
        Dictionary<string, SkillLevelInfo> selectItemLevelInfos = new Dictionary<string, SkillLevelInfo>();

        //用于洗牌算法
        List<string> saList = new List<string>();

        //最终筛选出的技能放置在这里
        selectSkillList = new List<SkillAttr>();

        //第一次筛选  筛选出角色已有的技能   若不足,从全技能库中抽取,若数量已够,则不再获取新技能
        { 
            //先从角色已有技能中选
            /*foreach (var item in playerPassiveSkillLevelInfos)
                selectItemLevelInfos.Add(item.Key, item.Value);
            //限制数量  从全部技能库中选
            if (playerPassiveSkillLevelInfos.Count < 5)
            {
                foreach (var item in allSkillLevelInfos)
                    if (item.Value.type == "buff" && !selectItemLevelInfos.ContainsKey(item.Key))
                        selectItemLevelInfos.Add(item.Key, item.Value);
            }*/
            //先从角色已有技能中选
            foreach (var item in playerActiveSkillLevelInfos)
                selectItemLevelInfos.Add(item.Key, item.Value);
            //限制数量 从全部技能库中选
            if (playerActiveSkillLevelInfos.Count < 6)
            {
                foreach (var item in allSkillLevelInfos) {

                    if (item.Value.type == "skill" && !selectItemLevelInfos.ContainsKey(item.Key))
                        selectItemLevelInfos.Add(item.Key, item.Value);

                    /*//加入武装技能
                    if (item.Value.atr.armFlag && UpLevel.armSkillNum < 3)
                    {
                        if (item.Value.type == "skill" && !selectItemLevelInfos.ContainsKey(item.Key))
                            selectItemLevelInfos.Add(item.Key, item.Value);
                    }
                    //加入武器技能
                    else if (!item.Value.atr.armFlag && UpLevel.wpSkillNum < 3)
                    {
                        if (item.Value.type == "skill" && !selectItemLevelInfos.ContainsKey(item.Key))
                            selectItemLevelInfos.Add(item.Key, item.Value);
                    }*/
                }
            }
        }
        

        //如果包含融合超武  不在加入原先的两种基础武器
        if (playerActiveSkillLevelInfos.ContainsKey("Wind")) {
            selectItemLevelInfos.Remove("Huo");
            selectItemLevelInfos.Remove("Shui");
        }


        //特殊判定:火水两个武器组合 组成风暴 暂时剔除龙卷风技能
       /* if (playerActiveSkillLevelInfos.ContainsKey("Huo") &&
            playerActiveSkillLevelInfos["Huo"].level == 5 &&
            playerActiveSkillLevelInfos.ContainsKey("Shui") &&
            playerActiveSkillLevelInfos["Shui"].level == 5 
            && selectSkillList.Count < 3 )
        {
            selectSkillList.Add(SkillAttrFactory.Get().skillMap["Wind"][0]);
        }*/


        //第二次筛选   选出还能继续升级的技能
        foreach (var item in selectItemLevelInfos)
        {
            Relic r = DataManager.Get().userData.towerData.relicList.Find(x => x.configId == item.Value.breach);
            if (item.Value.level < 5)
            {
                //buff类型不能超过5级   技能类型不能超过6级
                if ((item.Value.type == "skill" && item.Value.level == 6) ||
                    (item.Value.type == "buff" && item.Value.level == 5) ||
                    item.Value.atr.skillType == "Wind"
                    )
                {
                    continue;
                }

                //剔除 不是当前携带的主武器
                if (item.Value.atr.mainWeaponFlag) {
                    if (!playerActiveSkillLevelInfos.ContainsKey(item.Value.atr.skillType)) {
                        continue;
                    }
                }
                saList.Add(item.Value.name);
            }
            //若满足觉醒条件 直接加入selectSkill
            else if (selectSkillList.Count < 3
                && item.Value.level == 5 
                && item.Value.type == "skill"
                && !string.IsNullOrEmpty(item.Value.breach)   //第三版逻辑  超武关联获取的遗物
                && r!=null
                //&& RoleManager.Get().superNum > 0 //新逻辑 需要超武进化道具   该逻辑取消
                /* && !string.IsNullOrEmpty(item.Value.breach)    old逻辑   主动 + 被动组合成超武
                 && playerPassiveSkillLevelInfos.ContainsKey(item.Value.breach)*/
                ) 
            {
                int level = selectItemLevelInfos[item.Value.name].level;
                selectSkillList.Add(SkillAttrFactory.Get().skillMap[item.Value.name][level]);
            }
        }


        //第三次筛选  从可以升级的技能中根据权重随机筛选出三个技能
        if (selectSkillList.Count < 3) { 
            Dictionary<string, int> skillWeight = new Dictionary<string, int>();
            WeightInfoForLevel weightForLevel = null;
            //计算权重
            if (SkillAttrFactory.Get().skillWeightMap.ContainsKey(player.level)
                && SkillAttrFactory.Get().skillWeightMap[player.level] != null) {
                 weightForLevel = SkillAttrFactory.Get().skillWeightMap[player.level];
            }
            //test selectSkill.Add(SkillAttrFactory.Get().skillMap["LaserGen"][selectItemLevelInfos["LaserGen"].level]);
            //当有权重数据时
            if (weightForLevel != null)
            {
                for (int i=0; i < 3 && saList.Count > 0 && selectSkillList.Count<=3; i++) {
                    int maxNum = 0;
                    List<WeightInfo> newWeightInfoList = new List<WeightInfo>();
                    foreach (WeightInfo info in weightForLevel.skillWeightInfo)
                    {
                        if (saList.IndexOf(info.skillType) != -1) {
                            maxNum += info.weight;
                            WeightInfo wi = new WeightInfo();
                            wi.skillType = info.skillType;
                            wi.weight = maxNum;
                            newWeightInfoList.Add(wi);
                        }
                    }

                    int value = Random.Range(0, maxNum + 1);
                    string skillType = null;
                    foreach (WeightInfo info in newWeightInfoList) {
                        if (info.weight >= value) {
                            skillType = info.skillType;
                            int level = selectItemLevelInfos[skillType].level;
                            selectSkillList.Add(SkillAttrFactory.Get().skillMap[skillType][level]);
                            break;
                        }
                    }
                    saList.Remove(skillType);
                }
            }
            //没有权重时
            else {
                for (int i = 1; i < saList.Count; i++)
                {
                    string s = saList[i];
                    int index = Random.Range(0, i);
                    saList[i] = saList[index];
                    saList[index] = s;
                }

                for (int i = 0; i < saList.Count && selectSkillList.Count < 3; i++)
                {
                    //优先选择主动技能  保证里面必定有一个主动
                    if (selectItemLevelInfos[saList[i]].type == "skill")
                    {
                        int level = selectItemLevelInfos[saList[i]].level;
                        selectSkillList.Add(SkillAttrFactory.Get().skillMap[saList[i]][level]);
                    }
                    if (selectSkillList.Count == 2) { break; }
                }

                //二次筛选 加入被动
                for (int i = 0; i < saList.Count && selectSkillList.Count<3; i++)
                {
                    int level = selectItemLevelInfos[saList[i]].level;
                    if (selectSkillList.IndexOf(SkillAttrFactory.Get().skillMap[saList[i]][level]) == -1)
                    {
                        selectSkillList.Add(SkillAttrFactory.Get().skillMap[saList[i]][level]);
                    }
                }
            }
        }


        //if (selectSkillList.Count > 0)
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }

        skillList.GetChild(0).gameObject.SetActive(false);
        skillList.GetChild(1).gameObject.SetActive(false);
        skillList.GetChild(2).gameObject.SetActive(false);

        for (int i = 0; i < selectSkillList.Count && i < 3; i++)
        {
            //RefreshSkill(i, selectSkillList[i]);

           /* skillList.GetChild(i).GetComponent<UpSkill>().ul = this;
            skillList.GetChild(i).GetComponent<UpSkill>().index = i;
            skillList.GetChild(i).gameObject.SetActive(true);
            skillList.GetChild(i).GetComponent<Button>().interactable = true;*/
        }


        //混入宝物
        int relicNum = 3 - selectSkillList.Count;
        if (relicNum == 0) {
            relicNum = Random.Range(0, 2);
        }
        selectList = new List<SelectObj>();

        if (player.level < 5)
            relicNum = 0;


        List<Relic> relicList = creatRelic(relicNum, -1);
        List<int> intList = new List<int>();
        intList.Add(0);
        intList.Add(1);
        intList.Add(2);

        selectList.Add(new SelectObj());
        selectList.Add(new SelectObj());
        selectList.Add(new SelectObj());

        for (int i = 0; i < 3; i++) {


            int index = Random.Range(0, intList.Count);
            int value = intList[index];
            intList.RemoveAt(index);
            /*selectList[value] = new SelectObj();
            selectList[value].type = "relic";*/


            if (relicList.Count > 0)
            {
                selectList[value].type = "relic";
                selectList[value].relic = relicList[0];
                relicList.RemoveAt(0);
            }
            else if (selectSkillList.Count > 0)
            {
                selectList[value].type = "skill";
                selectList[value].skill = selectSkillList[0];
                selectSkillList.RemoveAt(0);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (selectList[i].type == "skill")
            {
                RefreshSkill(i, selectList[i].skill);
            }
            else
            {
                RefreshRelic(i, selectList[i].relic);
            }

            skillList.GetChild(i).GetComponent<UpSkill>().ul = this;
            skillList.GetChild(i).GetComponent<UpSkill>().index = i;
            skillList.GetChild(i).gameObject.SetActive(true);
            skillList.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }

    private void Awake()
    {
        this.init();
    }

    public void init() {
        if (initFlag)
            return;

        initFlag = true;

        player = GameObject.Find("role").GetComponent<Player>();
        skillList = transform.Find("skillList");

        allSkillLevelInfos = new Dictionary<string, SkillLevelInfo>();

        foreach (var item in SkillAttrFactory.Get().skillMap)
        {
            if (item.Value[0].id == null || item.Value[0].id=="" || item.Value[0].notEnabled)
                continue;

            if (item.Value[0].skillType.IndexOf("buff_") != -1)
            {
                allSkillLevelInfos.Add(item.Value[0].skillType,
                    new SkillLevelInfo(item.Value[0],item.Value[0].skillType, 0, item.Value[0].breach, "buff"));
            }
            else if (item.Value[0].skillType.IndexOf("Ex") == -1
                && item.Value[0].skillType.IndexOf("VT") == -1
                 && item.Value[0].skillType.IndexOf("DLY") == -1
                ) {
                //todo  应该传入一个数组  数组内的技能类型才能被加入技能库

                //Debug.Log(item.Value[0].mainWeaponFlag+"=======" +DataManager.Get().GetWpStr()+"======="+ item.Value[0].skillType);

                //不是装备的主武器技能不加入
                if (item.Value[0].mainWeaponFlag && DataManager.Get().GetWpStr() != item.Value[0].skillType) {
                    continue;
                }

                allSkillLevelInfos.Add(item.Value[0].skillType,
                    new SkillLevelInfo(item.Value[0],item.Value[0].skillType, 0, item.Value[0].breach));
            }
        }


        ActiveIconList = new List<Image>();
        Transform ActiveListTra = transform.Find("ActiveList");
        for (int i=1;i < ActiveListTra.childCount ; i++) {
            ActiveIconList.Add(ActiveListTra.GetChild(i).GetChild(0).GetComponent<Image>());
        }
        PassiveIconList = new List<Image>();
        Transform PassiveListTra = transform.Find("PassiveList");
        for (int i = 1; i < PassiveListTra.childCount; i++)
        {
            PassiveIconList.Add(PassiveListTra.GetChild(i).GetChild(0).GetComponent<Image>());
        }
    }

    public static List<int> GenerateRandomList(int length, int min, int max)
    {
        List<int> randomList = new List<int>();
        if (length <= (max - min))
        {
            for (var i = 0; i < length; i++)
            {
                int random = Random.Range(min, max);
                if (randomList.Contains(random))
                {
                    i--;
                    continue;
                }
                else
                {
                    randomList.Add(random);
                }
            }
        }
        return randomList;
    }

    public static void setNum(SkillAttr sa) {

/*        Debug.Log(UpLevel.playerActiveSkillLevelInfos.Count+"  "+(UpLevel.playerActiveSkillLevelInfos.ContainsKey(sa.skillType)));

        if (!UpLevel.playerActiveSkillLevelInfos.ContainsKey(sa.skillType))
        {
            if (sa.armFlag)
                UpLevel.armSkillNum += 1;
            else
                UpLevel.wpSkillNum += 1;
        }*/
    }

    public void selectSkill(int index) {

        if (selectList[index].type == "skill")
        {
            UpLevel.player.addSkill(selectList[index].skill);
        }
        else {
            DataManager.Get().userData.towerData.relicList.Add(selectList[index].relic);
            DataManager.Get().save();
        }


        player.levelSkillUnm++;
        gameObject.SetActive(false);
        if (this.awaitUpgradeFlag && player.awaitlevelSkillUnm > player.levelSkillUnm)
        {
            gameObject.SetActive(true);
            if(player.awaitlevelSkillUnm == player.levelSkillUnm)
                gameObject.SetActive(false);
        }
        Player.levelUpIng = false;
        for (int i = 0; i < selectSkillList.Count && i < 3; i++)
            skillList.GetChild(i).GetComponent<Button>().interactable = false;
        Time.timeScale = GameSceneManage.nowTimeScale;
    }

    List<Relic> creatRelic(int num,int Q) {

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
        List<Relic> relicList = new List<Relic>();
        for (int i = 0; i < num; i++)
        {
            int index = Random.Range(0, RelicConfigList.Count);
            RelicConfig config = RelicConfigList[index];
            RelicConfigList.RemoveAt(index);
            Relic relic = new Relic();
            relic.configId = config.id;
            relic.level = 0;
            relic.quality = config.quality;
            relicList.Add(relic);
        }
        return relicList;
    }


    void RefreshSkill(int i,SkillAttr skill) {
        skillList.GetChild(i).transform.Find("dk").Find("icon").GetComponent<Image>().sprite =
                 Resources.Load<Sprite>("skill/icon/" + skill.icon);
        skillList.GetChild(i).transform.Find("name").GetComponent<Text>().text =
            skill.itemName;
        skillList.GetChild(i).transform.Find("desc").GetComponent<Text>().text =
            skill.desc;
        skillList.GetChild(i).transform.Find("dk").GetComponent<Image>().sprite = Resources.Load<Sprite>
         ("ui/img/tower/towerBackPack/skill");
        skillList.GetChild(i).transform.Find("num").gameObject.SetActive(false);
        GameObject breachObj =
            skillList.GetChild(i).transform.Find("breach_dk/breach").gameObject;
        breachObj.transform.parent.gameObject.SetActive(false);
        //显示超武组合
        /*if (skill.skillType.IndexOf("buff_") != -1 )
        {
            foreach (var item in SkillAttrFactory.Get().skillMap) {
                if (item.Value[0].breach != null && item.Value[0].breach == skill.skillType && !item.Value[0].notEnabled) { 
                    SkillAttr breach = item.Value[0];
                    breachObj.GetComponent<Image>().sprite =
                            Resources.Load<Sprite>("skill/icon/" + breach.icon);
                    breachObj.transform.parent.gameObject.SetActive(true);
                    break;
                }
            }
        }*/
        if (skill.breach != null && skill.breach.Length > 0)
        {
            RelicConfig r = TowerFactory.Get().relicList.Find(x => x.id == skill.breach);
            breachObj.GetComponent<Image>().sprite =
                            Resources.Load<Sprite>(r.icon);
            breachObj.transform.parent.gameObject.SetActive(true);
        }


        Transform levelList = skillList.GetChild(i).transform.Find("list");
        levelList.gameObject.SetActive(true);
        for (int j = 0; j < levelList.childCount; j++)
        {
            if (skill.skillForm == "SuperSkill")
                levelList.GetChild(j).GetComponent<Image>().color =
                    UIFrameUtil.getitemQualityColor("#FFFFFF");
            else
                levelList.GetChild(j).GetComponent<Image>().color =
                    UIFrameUtil.getitemQualityColor("#353535");
        }
        for (int j = 0; j < levelList.childCount && j < skill.level; j++)
        {
            levelList.GetChild(j).GetComponent<Image>().color =
              UIFrameUtil.getitemQualityColor("#FFFFFF");
        }

        if (skill.level > 1)
            skillList.GetChild(i).transform.Find("new").gameObject.SetActive(false);
        else
            skillList.GetChild(i).transform.Find("new").gameObject.SetActive(true);


        //底框
        if (skill.skillType.IndexOf("buff_") != -1)
        {
            if (skill.skillForm == "weaponBuff")
                skillList.GetChild(i).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("ui/uplevel/wqbd");
            else
                skillList.GetChild(i).GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("ui/uplevel/bd");
        }
        else
        {
            if (skill.level == 6 || skill.skillType == "Wind")
                skillList.GetChild(i).GetComponent<Image>().sprite =
                      Resources.Load<Sprite>("ui/uplevel/cw");
            else
                skillList.GetChild(i).GetComponent<Image>().sprite =
                      Resources.Load<Sprite>("ui/uplevel/pt");
        }
    }

    void RefreshRelic(int i, Relic relic) {
        RelicConfig now_config = TowerFactory.Get().relicMap[relic.configId];

        skillList.GetChild(i).transform.Find("dk").Find("icon").GetComponent<Image>().sprite = 
              Resources.Load<Sprite>(now_config.icon);
        skillList.GetChild(i).transform.Find("name").GetComponent<Text>().text =
            now_config.name;
        skillList.GetChild(i).transform.Find("desc").GetComponent<Text>().text =
            now_config.desc_1;

        GameObject breachObj =
            skillList.GetChild(i).transform.Find("breach_dk/breach").gameObject;
        breachObj.transform.parent.gameObject.SetActive(false);


        List<Relic> relicList2 = DataManager.Get().userData.towerData.relicList.FindAll(x => x.configId == relic.configId);
        if (relicList2 == null || relicList2.Count == 0)
        {
            skillList.GetChild(i).transform.Find("new").gameObject.SetActive(true);
            //skillList.GetChild(i).transform.Find("num").GetComponent<TextMeshProUGUI>().text = "Own: 0";
            skillList.GetChild(i).transform.Find("num").gameObject.SetActive(false);
        }
        else
        {
            skillList.GetChild(i).transform.Find("new").gameObject.SetActive(false);
            skillList.GetChild(i).transform.Find("num").GetComponent<TextMeshProUGUI>().text = "Own: " + relicList2.Count;
            skillList.GetChild(i).transform.Find("num").gameObject.SetActive(true);
        }

        skillList.GetChild(i).transform.Find("dk").GetComponent<Image>().sprite = Resources.Load<Sprite>
           ("ui/img/tower/towerBackPack/" + now_config.quality);
        Transform levelList = skillList.GetChild(i).transform.Find("list");
        levelList.gameObject.SetActive(false);

        skillList.GetChild(i).GetComponent<Image>().sprite =
                  Resources.Load<Sprite>("ui/uplevel/wqbd");
    }
}
