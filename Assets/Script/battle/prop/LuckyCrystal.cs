using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyCrystal : MonoBehaviour
{
    //------------配置参数
    //预期奖励最大数量
    public int awardNum_try = 3;
    //实际可以奖励的最大数量  若角色可升级的技能数量不足 则会出现这种情况
    public int awardNum_real = 0;
    //每次奖励的数量
    public int eachAwardNum = 2;


    List<Vector3> originPosList = new List<Vector3>();
    //奖励槽tras
    List<Transform> itemList = new List<Transform>();
    Transform award;
    Transform crystalTra;
    bool initFlag;

    public void init() {

        if (initFlag)
            return;

        initFlag = true;

        originPosList.Clear();
        itemList.Clear();
        Transform listTra = transform.Find("Panel").Find("list");
        for (int i =0;i<listTra.childCount;i++) {
            itemList.Add(listTra.GetChild(i));
            originPosList.Add(listTra.GetChild(i).transform.localPosition);
        }

        crystalTra = transform.Find("Panel").Find("Image");
        award = transform.Find("award");
    }

    bool showIng;
    float showTime;
    bool hideIng;
    float awardTime;
    bool awardIng;
    //是否弹出技能信息
    bool awardAlertFlag;

    public void show() {
        init();

        this.gameObject.SetActive(true);
        award.gameObject.SetActive(false);
        Time.timeScale = 0f;
        
        awardAlertFlag = false;
        awardIng = false;
        awardTime = 0;
        awardNum_try = 3;
        initLottery();
    }

    float oldtime;

    // Update is called once per frame
    void Update()
    {
        //用于暂停时的动画显示计时
        float nowDt = Time.realtimeSinceStartup - oldtime;
        oldtime = Time.realtimeSinceStartup;
        float speed = 2000 * nowDt;

        if (showIng) {
            bool showEnd = true;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (Vector2.Distance(itemList[i].localPosition, originPosList[i]) >1)
                {
                    itemList[i].localPosition = Vector3.MoveTowards
                        (itemList[i].localPosition, originPosList[i], speed);
                    showEnd = false;
                }
            }
            if (showEnd) {
                showIng = false;
            }
        }
        if (!showIng && showTime > 0) {
            if ((showTime -= nowDt*2 ) <= 0) {
                hideIng = true;
            }
        }


        if (hideIng)
        {
            bool hideEnd = true;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (Vector2.Distance(itemList[i].localPosition, new Vector3(0, 0, 0)) > 1)
                {
                    itemList[i].localPosition = Vector3.MoveTowards
                        (itemList[i].localPosition, new Vector3(0, 0, 0), speed);
                    hideEnd = false;
                }
            }
            if (hideEnd)
            {
                hideIng = false;
                awardTime = 1f;
            }
        }

        if (awardTime > 0)
        {
            if ((awardTime -= nowDt*2) <= 0)
            {
                awardIng = true;
            }
        }

        if (awardIng && awardIndexList.Count > 0 && awardShowIndex_now < awardShowIndex_max) {

            int i = awardIndexList[awardShowIndex_now];
            if (Vector2.Distance(itemList[i].localPosition, originPosList[i]) > 1)
            {
                itemList[i].localPosition = Vector3.MoveTowards
                    (itemList[i].localPosition, originPosList[i], speed);
            }
            else {
                awardShowIndex_now++;
                //弹出下一个奖励
                if (awardShowIndex_now < awardShowIndex_max) { 
                    awardIng = false;
                    awardTime = 1f;
                }
            }
            /*for (int J = 0;J < awardNum_real; J++) { 
                awardShowIndex_now++;
            }*/
        }

        if (!awardAlertFlag && awardShowIndex_now == awardShowIndex_max)
        {
            awardAlertFlag = true;
            StartCoroutine(selectEnd());
        }
    }

    //装填技能的奖励槽 目前一共8个
    public List<SkillAttr> lotterySkillList = new List<SkillAttr>();
    //奖励索引
    List<int> awardIndexList = new List<int>();
    //应弹出的奖励数量
    public int awardShowIndex_max = 0;
    //已弹出的奖励数量
    public int awardShowIndex_now = 0;

    public void initLottery()
    {
        awardShowIndex_now = 0;
        awardShowIndex_max = 0;

        lotterySkillList.Clear();
        //从其中筛选出玩家有的技能
        List<LuckySkillLevelInfo> saList = check();

        if (saList.Count == 0)
        {
            //todo   奖励钱or恢复血量
            end();
            return;
        }

        //存入其中的index不可作为奖励  纯凑数填满槽位
        List<int> notAwardIndexList_1 = new List<int>();
        List<int> notAwardIndexList_2 = new List<int>();


        //筛选出8个可升级的技能  不满8个则任意重复填充,但这几个皆不可作为升级选项
        List<SkillAttr> lotterySkillList_prepare = new List<SkillAttr>();
        for (int i = 0; i < 8; i++)
        {
            bool flag = true;
            //优先加入满足觉醒条件的技能
            for (int j = 0; j < saList.Count; j++) {
                SkillAttr atr = SkillAttrFactory.Get().skillMap[saList[j].sinfo.name][0];
                if (saList[j].superSkillFlag)
                {
                    if (lotterySkillList_prepare.Contains(atr)){
                        continue;
                    }
                    else { 
                        flag = false;
                        lotterySkillList_prepare.Add(atr);
                    }
                }
            }

            if (flag) { 
                for (int j = 0; j < saList.Count; j++) {
                    SkillAttr atr = SkillAttrFactory.Get().skillMap[saList[j].sinfo.name][0];
                    if (lotterySkillList_prepare.Contains(atr) && atr.skillForm != "superSkill")
                    {
                        int num = 0;
                        foreach (SkillAttr sa in lotterySkillList_prepare) {
                            if (sa == atr)
                                num++;
                        }
                        int level = DungeonManager.player.checkSkillLevel(atr);
                        if (level + num < 5) {
                            lotterySkillList_prepare.Add(atr);
                            break;
                        }
                        continue;
                    }
                    else {
                        lotterySkillList_prepare.Add(atr);
                        break;
                    }
                }
            }
        }

        //保证其中有8个元素
        int nowCount = lotterySkillList_prepare.Count;
        for (int i = nowCount; i < 8 ; i++) {
            int index = Random.Range(0, lotterySkillList_prepare.Count);
            lotterySkillList_prepare.Add(lotterySkillList_prepare[index]);
            notAwardIndexList_1.Add(i);
        }

        List<int> prepareIndexList = new List<int>();
        for (int i = 0; i < 8; i++){
            prepareIndexList.Add(i);
        }
        //打乱后分配给格子
        for (int i = 0; i < 8; i++)
        {
            int index = Random.Range(0, prepareIndexList.Count);
            int index2 = prepareIndexList[index];
            SkillAttr atr = lotterySkillList_prepare[index2];
            lotterySkillList.Add(atr);

            //这个索引不能作为奖励  纯凑数
            if (notAwardIndexList_1.Contains(index2)) { 
                notAwardIndexList_2.Add(lotterySkillList.Count-1);
            }
            prepareIndexList.RemoveAt(index);

            //todo 图标填充
            itemList[i].Find("icon").GetComponent<Image>().sprite =
                 Resources.Load<Sprite>("skill/icon/" + atr.icon);
            itemList[i].Find("icon").gameObject.SetActive(true);
        }

        //动画相关
        { 
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].localPosition = new Vector3(0,0,0);
            }
            showIng = true;
            showTime = 2;
        }


        //预定奖励
        { 
            awardIndexList = new List<int>();
            List<int> indexList = new List<int>();
            for (int i = 0; i < 8; i++) {
                if (!notAwardIndexList_2.Contains(i)) { 
                    indexList.Add(i);
                }
            }
            awardNum_real = Mathf.Min(awardNum_try, indexList.Count);

            if (awardNum_real > 2 && awardNum_real % 2 == 0)
            {
                eachAwardNum = 2;
            }
            else {
                eachAwardNum = 1;
            }

            //提前预定好玩家可以抽中的技能:
            //可抽中的技能不能超过该技能的最大等级上限
            for (int i = 0; i < awardNum_try && indexList.Count > 0; i++) {
                awardShowIndex_max++;
                int listIndex = Random.Range(0, indexList.Count);
                int awardIndex = indexList[listIndex];
                awardIndexList.Add(awardIndex);
                indexList.RemoveAt(listIndex);
            }
        }
    }
   


    //筛选出可用技能
    List<LuckySkillLevelInfo> check()
    {
        //可选择的技能
        Dictionary<string, SkillLevelInfo> selectItemLevelInfos = new Dictionary<string, SkillLevelInfo>();

        //洗牌算法
        List<LuckySkillLevelInfo> saList = new List<LuckySkillLevelInfo>();

        //从角色已有被动技能中选
        foreach (var item in UpLevel.playerPassiveSkillLevelInfos)
            selectItemLevelInfos.Add(item.Key, item.Value);
        //从角色已有主动技能中选
        foreach (var item in UpLevel.playerActiveSkillLevelInfos) { 
            selectItemLevelInfos.Add(item.Key, item.Value);
        }

        //特殊判定:组合武器 
        if (UpLevel.playerActiveSkillLevelInfos.ContainsKey("Huo") &&
            UpLevel.playerActiveSkillLevelInfos["Huo"].level == 5 &&
            UpLevel.playerActiveSkillLevelInfos.ContainsKey("Shui") &&
            UpLevel.playerActiveSkillLevelInfos["Shui"].level == 5
            )
        {
            saList.Add(new LuckySkillLevelInfo(UpLevel.allSkillLevelInfos["Wind"],true));
        }

        foreach (var item in selectItemLevelInfos)
        {
            //特殊判定:要两个武器组合 而不是武器+道具组合
            if (item.Value.level < 5)
            {
                saList.Add(new LuckySkillLevelInfo(item.Value,false));
            }
            //若满足觉醒条件 直接加入selectSkill
            else if (item.Value.level == 5
                && item.Value.type == "skill"
                && !string.IsNullOrEmpty(item.Value.breach)
                && UpLevel.playerPassiveSkillLevelInfos.ContainsKey(item.Value.breach))
            {
                saList.Add(new LuckySkillLevelInfo(item.Value,true));
            } 
        }
        return saList;
    }

    
    //弹出奖励详情
    IEnumerator selectEnd()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        
        //弹出奖励显示  
        award.gameObject.SetActive(true);

        for (int i = 0; i < award.Find("list").childCount; i++) {
            award.Find("list").GetChild(i).gameObject.SetActive(false);
        }

        for (int i= 0 ; i < awardShowIndex_max; i++) { 
            //todo  bug1: 需要做等级校验  获取正确等级的技能
            SkillAttr atr_0 = lotterySkillList[awardIndexList[i]];
            int level = DungeonManager.player.checkSkillLevel(atr_0);
            SkillAttr atr = SkillAttrFactory.Get().skillMap[atr_0.skillType][level];

            award.Find("list").GetChild(i).Find("icon").GetComponent<Image>().sprite =
                      Resources.Load<Sprite>("skill/icon/" + atr.icon);
            award.Find("list").GetChild(i).Find("name").GetComponent<Text>().text =
                atr.itemName;
            award.Find("list").GetChild(i).Find("desc").GetComponent<Text>().text =
                atr.desc;

            Transform levelTra = award.Find("list").GetChild(i).Find("list");
            //显示星级
            for (int j=0;j<5;j++) {
                if(atr.level>j)
                    levelTra.GetChild(j).gameObject.SetActive(true);
                else
                    levelTra.GetChild(j).gameObject.SetActive(false);
            }
            award.Find("list").GetChild(i).gameObject.SetActive(true);
            UpLevel.player.addSkill(atr);
        }
    }

    public void end() {
        Time.timeScale = 1f;
        award.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}


//包装SkillLevelInfo 加一个是否是超武升级的参数
public class LuckySkillLevelInfo {
    public SkillLevelInfo sinfo;
    public bool superSkillFlag = false;

    public LuckySkillLevelInfo(SkillLevelInfo info, bool flag = false) {
        this.sinfo = info;
        superSkillFlag = flag;
    }
}