using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LotteryMachine : MonoBehaviour
{
    public List<Transform> items;
    Transform panel;
    Transform award;
    public List<SkillAttr> lotterySkillList = new List<SkillAttr>();
    bool initFlag;

    int awardIndex;

    // Start is called before the first frame update
    void init()
    {
        if (initFlag)
            return;
        initFlag = true;

        panel =  transform.Find("Panel");
        award = transform.Find("award"); 


        items.Add(panel.GetChild(0));
        items.Add(panel.GetChild(1));
        items.Add(panel.GetChild(2));
        items.Add(panel.GetChild(3));
        items.Add(panel.GetChild(4));
        items.Add(panel.GetChild(9));
        items.Add(panel.GetChild(14));
        items.Add(panel.GetChild(19));
        items.Add(panel.GetChild(24));
        items.Add(panel.GetChild(23));
        items.Add(panel.GetChild(22));
        items.Add(panel.GetChild(21));
        items.Add(panel.GetChild(20));
        items.Add(panel.GetChild(15));
        items.Add(panel.GetChild(10));
        items.Add(panel.GetChild(5));

        //StartCoroutine(lottery());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void show() {
        Time.timeScale = 0;

        init();
        this.gameObject.SetActive(true);
        award.gameObject.SetActive(false);

        initLottery();
    }

    public void startLottery() {
        StartCoroutine(lottery());
    }

    IEnumerator  lottery() {
        //选好某个index  
        //int startIndex = 0;
        int awardIndex = Random.Range(0, 16);
        int 轮数 = Random.Range(3,8);

        for (int i=0; i <= 轮数; i++) {
            int j = 0;
            for (; j < 16; j++) {
                if (i == 轮数 && j == awardIndex)
                {
                    Image img = items[j].gameObject.GetComponent<Image>();
                    img.color = new Color(255, 255, 0, 255);
                    StartCoroutine(selectEnd());
                    break;
                }
                else {
                    StartCoroutine(select(j));
                    yield return new WaitForSecondsRealtime(0.02f);
                }
            }
        }
    }

    IEnumerator select(int index) {
        Image img = items[index].gameObject.GetComponent<Image>();
        img.color = new Color(255,255,0,255);


        yield return new WaitForSecondsRealtime(0.05f);
        img.color = new Color(255, 255, 255, 255);
    }


    IEnumerator selectEnd()
    {
        //弹出奖励显示  
        award.gameObject.SetActive(true);

        award.GetChild(0).Find("name").GetComponent<Text>().text =
            lotterySkillList[awardIndex].itemName;
        award.GetChild(0).Find("desc").GetComponent<Text>().text =
            lotterySkillList[awardIndex].desc;
        award.GetChild(0).Find("level").GetComponent<Text>().text = "level." + 
            (lotterySkillList[awardIndex].level );

        UpLevel.player.addSkill(lotterySkillList[awardIndex]);

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = GameSceneManage.nowTimeScale;
        award.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }


    public void initLottery() {
        //从其中筛选出玩家有的技能
        lotterySkillList.Clear();

        List<SkillLevelInfo> saList = screen();

        //随机分配个每个格子
        for (int i=0;i<16;i++) {
            int index = Random.Range(0, saList.Count);
            lotterySkillList.Add
                (SkillAttrFactory.Get().skillMap[saList[index].name][saList[index].level]); 
            //todo 图标填充
        }
    }

    //筛选出可用技能
    List<SkillLevelInfo> screen() {
        //可选择的技能
        Dictionary<string, SkillLevelInfo> selectItemLevelInfos = new Dictionary<string, SkillLevelInfo>();

        //洗牌算法
        List<SkillLevelInfo> saList = new List<SkillLevelInfo>();

        //角色至少有两个主动技能时  才会刷出被动效果
        //先从角色已有技能中选
        foreach (var item in UpLevel.playerPassiveSkillLevelInfos)
            selectItemLevelInfos.Add(item.Key, item.Value);

        //先从角色已有技能中选
        foreach (var item in UpLevel.playerActiveSkillLevelInfos)
            selectItemLevelInfos.Add(item.Key, item.Value);

        foreach (var item in selectItemLevelInfos)
        {
            //特殊判定:要两个武器组合 而不是武器+道具组合
            if (
                item.Value.level < 5 ||
                (item.Value.level == 5 && item.Value.type == "skill" && !string.IsNullOrEmpty(item.Value.breach)
                && UpLevel.playerPassiveSkillLevelInfos.ContainsKey(item.Value.breach)))
            {
                //buff类型不能超过5级   技能类型不能超过6级
                if ((item.Value.type == "skill" && item.Value.level == 6) ||
                    (item.Value.type == "buff" && item.Value.level == 5))
                {
                    continue;
                }
                saList.Add(item.Value);
            }
        }

        return saList;
    }
}
