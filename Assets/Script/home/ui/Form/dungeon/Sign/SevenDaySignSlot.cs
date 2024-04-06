using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SevenDaySignSlot : BaseSlot
{

    TextMeshProUGUI dayText;
    TextMeshProUGUI desc;
    GameObject img;

    List<ItemSlot> slotList;
    Transform slotTra;
    GameObject slotPf;

    int now_index;

    protected override void Awake()
    {
        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        dayText = UIFrameUtil.FindChildNode(this.transform, "day").GetComponent<TextMeshProUGUI>();
        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        img = UIFrameUtil.FindChildNode(this.transform, "img").gameObject;


        slotTra = UIFrameUtil.FindChildNode(this.transform, "list");
        slotList = new List<ItemSlot>();
        slotPf = slotTra.GetChild(0).gameObject;
        for (int i = 0; i < slotTra.childCount; i++)
        {
            ItemSlot slot = slotTra.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            slotList.Add(slot);
        }


        this.GetComponent<Button>().onClick.AddListener(async () => {
            bool flag = await trySgin();
            if (flag) { 
                UIManager.GetUIMgr().showUIForm("RewardForm");

                List<SevenDaySignConfig> configList = PerimeterFactory.Get().SevenDaySignList;
                SevenDaySignConfig config = configList[now_index];
                MessageMgr.SendMsg("GetReward", new MsgKV("", config.items));
            }
            MessageMgr.SendMsg("RefreshTip", null);
            ((SevenDaySignForm)mgr).Refresh();
        });
    }

    async Task<bool> trySgin() {
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/sevenDaySign", DataManager.Get().getHeader());
        Debug.Log("trySgin:"+str);

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode == null)
        {
            return true;
        }
        else {
            return false;
        }
    }

  
    /// <summary>index 是否领取  是否解锁   解锁倒计时(分钟)</summary>
    public void Refresh(int index,bool getFlag,bool unlockFLag,int second)
    {

        if (!initFlag)
            Awake();

        nowSecond = second;
        updateSecond = 0;

        myBut.interactable = false;
        now_index = index;

        background.sprite = Resources.Load<Sprite>("ui/img/SevenDaySign/sevenDay_h");

        dayText.text = (index + 1) + "day";

        if (getFlag)
        {
            img.SetActive(true);
            desc.gameObject.SetActive(false);
        }
        else {
            img.SetActive(false);
            desc.gameObject.SetActive(true);


            if (unlockFLag)
            {
                desc.text = "Get";
                background.sprite = Resources.Load<Sprite>("ui/img/SevenDaySign/sevenDay_l");
                myBut.interactable = true;
            }
            else {
                desc.text = "Unlocked";
                if (second != 0) {
                    Debug.Log(second);
                    desc.text = "countdown:\r\n" + (second / 3600)+ "h:" + (second % 3600 / 60) + "m" + (second % 3600 % 60) + "s";
                }
            }
        }


        //根据day 填充槽内显示
        List<SevenDaySignConfig> configList =  PerimeterFactory.Get().SevenDaySignList;
        SevenDaySignConfig config = configList[index];


        for (int i=0;i< config.items.Count;i++) {
            if (i >= slotList.Count) {
                GameObject g = Instantiate(slotPf, slotTra);
                ItemSlot slot = g.GetComponent<ItemSlot>();
                slot.mgr = this;
                slotList.Add(slot);
            }

            slotList[i].Refresh(config.items[i]);
        }
    }


    int nowSecond;
    float updateSecond;

    private void Update()
    {
        if (nowSecond != 0)
        {
            updateSecond += Time.deltaTime;
            int second = nowSecond - (int)updateSecond;
            if (second >= 0) { 
                desc.text = "countdown:\r\n" + (second / 3600) + "h:" + (second % 3600 / 60) + "m" + (second % 3600 % 60) + "s";
            }
        }
    }

}
