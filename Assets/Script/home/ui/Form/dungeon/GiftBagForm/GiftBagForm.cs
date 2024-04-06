using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class GiftBagForm : BaseUIForm
{
    GameObject day;
    GameObject week;
    GameObject month;


    List<GiftBagSlot> daySlotList = new List<GiftBagSlot>();
    List<GiftBagSlot> weekSlotList = new List<GiftBagSlot>();
    List<GiftBagSlot> monthSlotList = new List<GiftBagSlot>();

    TextMeshProUGUI dayDesc;
    TextMeshProUGUI weekDesc;
    TextMeshProUGUI monthDesc;

    List<GiftBagAccumulatedSlot> accumulatedSlotList = new List<GiftBagAccumulatedSlot>();
    GameObject accumulatedSlotPf;
    Transform accumulatedSlotTra;

    RectTransform accumulated_bg;
    RectTransform accumulated_value;
    RectTransform accumulatedContent;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.Normal;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "returnBut").onClick.AddListener(() => {
            OpenForm("down_menu");
            CloseForm("GiftBagForm");
        });


        day = UIFrameUtil.FindChildNode(this.transform, "day").gameObject;
        week = UIFrameUtil.FindChildNode(this.transform, "week").gameObject;
        month = UIFrameUtil.FindChildNode(this.transform, "month").gameObject;

        GetBut(this.transform, "Button_day").onClick.AddListener(() => {
            day.SetActive(true);
            week.SetActive(false);
            month.SetActive(false);
        });
        GetBut(this.transform, "Button_week").onClick.AddListener(() => {
            day.SetActive(false);
            week.SetActive(true);
            month.SetActive(false);
        });                   
        GetBut(this.transform, "Button_month").onClick.AddListener(() => {
            day.SetActive(false);
            week.SetActive(false);
            month.SetActive(true);
        });



        Transform daylist = UIFrameUtil.FindChildNode(this.transform, "day/list");
        for (int i = 0; i < daylist.childCount; i++)
        {
            GiftBagSlot slot = daylist.GetChild(i).GetComponent<GiftBagSlot>();
            slot.mgr = this;
            daySlotList.Add(slot);
        }

        Transform weeklist = UIFrameUtil.FindChildNode(this.transform, "week/list");
        for (int i = 0; i < weeklist.childCount; i++)
        {
            GiftBagSlot slot = weeklist.GetChild(i).GetComponent<GiftBagSlot>();
            slot.mgr = this;
            weekSlotList.Add(slot);
        }

        Transform monthlist = UIFrameUtil.FindChildNode(this.transform, "month/list");
        for (int i = 0; i < monthlist.childCount; i++)
        {
            GiftBagSlot slot = monthlist.GetChild(i).GetComponent<GiftBagSlot>();
            slot.mgr = this;
            monthSlotList.Add(slot);
        }

        dayDesc = UIFrameUtil.FindChildNode(this.transform, "day/desc").GetComponent<TextMeshProUGUI>();
        weekDesc = UIFrameUtil.FindChildNode(this.transform, "week/desc").GetComponent<TextMeshProUGUI>();
        monthDesc = UIFrameUtil.FindChildNode(this.transform, "month/desc").GetComponent<TextMeshProUGUI>();

        accumulated_bg = UIFrameUtil.FindChildNode(this.transform, "AccumulatedContent/bg").GetComponent<RectTransform>();
        accumulated_value = UIFrameUtil.FindChildNode(this.transform, "AccumulatedContent/value").GetComponent<RectTransform>();
        accumulatedContent = UIFrameUtil.FindChildNode(this.transform, "AccumulatedContent").GetComponent<RectTransform>();

        accumulatedSlotTra = UIFrameUtil.FindChildNode(this.transform, "AccumulatedContent/list");
        accumulatedSlotPf = accumulatedSlotTra.GetChild(0).gameObject;
        for (int i = 0; i < accumulatedSlotTra.childCount; i++)
        {
            GiftBagAccumulatedSlot slot = accumulatedSlotTra.GetChild(i).GetComponent<GiftBagAccumulatedSlot>();
            slot.mgr = this;
            accumulatedSlotList.Add(slot);
        }
    }

    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }

    public async Task RefreshAsync() {

        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/giftBag/list", DataManager.Get().getHeader());
        Debug.Log(str);
        NetGiftBagData netData0 = null;
        NetGiftShopData shopData = null;
        NetGiftBagTimeData timeData = null;
        if (str != null) {
            netData0 = JsonUtil.ReadData<NetGiftBagData>(str);
            shopData = netData0.data1;
            timeData = netData0.data2;

            nowSecond_d = timeData.DAILY_SHOP_TIME;
            nowSecond_w = timeData.WEEKLY_SHOP_TIME;
            nowSecond_m = timeData.MONTHLY_SHOP_TIME;
            updateSecond = 0;
        }

        List<GiftBagConfig> configList = PerimeterFactory.Get().GiftBagList;
        List<GiftBagConfig> dayList= configList.FindAll(item => item.id.IndexOf("day") != -1);
        List<GiftBagConfig> weekList = configList.FindAll(item => item.id.IndexOf("week") != -1);
        List<GiftBagConfig> monthList = configList.FindAll(item => item.id.IndexOf("month") != -1);
        List<GiftBagConfig> accumulatedList = configList.FindAll(item => item.id.IndexOf("acc") != -1);
        for (int i = 0; i < daySlotList.Count && i < 6; i++) {
            daySlotList[i].Hide();
        }
        for (int i = 0; i < weekSlotList.Count && i < 6; i++)
        {
            weekSlotList[i].Hide();
        }
        for (int i = 0; i < monthSlotList.Count && i < 6; i++)
        {
            monthSlotList[i].Hide();
        }
        for (int i = 0; i < dayList.Count && i < 6 ; i++) {
            bool bayFlag = false;
            if (shopData.DAILY_SHOP != null) { 
                if (shopData.DAILY_SHOP.Find(x => x.itemId == dayList[i].id) != null)
                    bayFlag = true;
            }

            daySlotList[i].Refresh(dayList[i], bayFlag);
        }

        for (int i = 0; i < weekList.Count && i < 6; i++)
        {
            bool bayFlag = false;
            if (shopData.WEEKLY_SHOP != null)
            {
                if (shopData.WEEKLY_SHOP.Find(x => x.itemId == weekList[i].id) != null)
                    bayFlag = true;
            }

            weekSlotList[i].Refresh(weekList[i], bayFlag);
        }

        for (int i = 0; i < monthList.Count && i < 6; i++)
        {
            bool bayFlag = false;
            if (shopData.MONTHLY_SHOP != null)
            {
                if (shopData.MONTHLY_SHOP.Find(x => x.itemId == monthList[i].id) != null)
                    bayFlag = true;
            }
            monthSlotList[i].Refresh(monthList[i], bayFlag);
        }

        for (int i = 0; i < accumulatedList.Count ; i++)
        {
            if (i >= accumulatedSlotList.Count) {
                GameObject g = Instantiate(accumulatedSlotPf, accumulatedSlotTra); ;
                GiftBagAccumulatedSlot slot = g.GetComponent<GiftBagAccumulatedSlot>();
                slot.mgr = this;
                accumulatedSlotList.Add(slot);
            }
            accumulatedSlotList[i].Refresh(accumulatedList[i].itemList[0],
                accumulatedList[i].day+"");
        }

        accumulated_bg.sizeDelta = new Vector2(200 + 250 * (accumulatedList.Count - 1), accumulated_bg.sizeDelta.y);
        accumulated_value.sizeDelta = new Vector2(200 ,accumulated_value.sizeDelta.y);
        accumulatedContent.sizeDelta = new Vector2(400 + 250 * (accumulatedList.Count - 1), accumulatedContent.sizeDelta.y);
    }

    int nowSecond_d;
    int nowSecond_w;
    int nowSecond_m;
    float updateSecond;
    private void Update()
    {
        if (nowSecond_d > 0)
        {
            updateSecond += Time.deltaTime;
            int second = nowSecond_d - (int)updateSecond;
            int second2 = nowSecond_w - (int)updateSecond;
            int second3 = nowSecond_m - (int)updateSecond;
           
            dayDesc.text = "<size=70> Daily deals </size>\r\ncountdown\r\n<color=#63FF00>" + (second / 3600) + "h: " + (second % 3600 / 60) + "m: " + (second % 3600 % 60) + "s</color>";
            weekDesc.text = "<size=70> Weekly deals </size>\r\ncountdown\r\n<color=#63FF00>" + (second2 / 3600) + "h: " + (second2 % 3600 / 60) + "m: " + (second2 % 3600 % 60) + "s</color>";
            monthDesc.text = "<size=70> Monthly deals </size>\r\ncountdown\r\n<color=#63FF00>" + (second3 / 3600) + "h: " + (second3 % 3600 / 60) + "m: " + (second3 % 3600 % 60) + "s</color>";
        }
    }

}

public class NetGiftBagData {
    public NetGiftShopData data1;
    public NetGiftBagTimeData data2;
}

public class NetGiftShopData
{
    public List<ItemInfo2> DAILY_SHOP;
    public List<ItemInfo2> WEEKLY_SHOP;
    public List<ItemInfo2> MONTHLY_SHOP;
}

public class NetGiftBagTimeData
{
    public int DAILY_REWARDS_TIME;
    public int DAILY_SHOP_TIME;
    public int MONTHLY_SHOP_TIME;
    public int WEEKLY_SHOP_TIME;
}