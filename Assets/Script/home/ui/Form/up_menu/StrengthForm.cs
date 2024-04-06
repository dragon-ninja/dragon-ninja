using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class StrengthForm : BaseUIForm
{
    TextMeshProUGUI priceText_1;
    TextMeshProUGUI priceText_2;

    TextMeshProUGUI buyNumText_1;
    TextMeshProUGUI buyNumText_2;
    TextMeshProUGUI timeDesc;
    Dictionary<string, ItemInfo> infosMap = new Dictionary<string, ItemInfo>();

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm();
        });


        priceText_1 = UIFrameUtil.FindChildNode(this.transform, "buy1/gold/value").GetComponent<TextMeshProUGUI>();
        priceText_2 = UIFrameUtil.FindChildNode(this.transform, "buy2/gold/value").GetComponent<TextMeshProUGUI>();

        buyNumText_1 = UIFrameUtil.FindChildNode(this.transform, "buy1/buyNum").GetComponent<TextMeshProUGUI>();
        buyNumText_2 = UIFrameUtil.FindChildNode(this.transform, "buy2/buyNum").GetComponent<TextMeshProUGUI>();

        timeDesc = UIFrameUtil.FindChildNode(this.transform, "timeDesc").GetComponent<TextMeshProUGUI>();

        GetBut(this.transform, "buy1").onClick.AddListener(async () => {
            buyAsync("s_001");
        });
        GetBut(this.transform, "buy2").onClick.AddListener(async () => {
            buyAsync("f_001");
        });
    }

    public override void Show()
    {
        base.Show();
        refreshAsync();
    }

    async Task refreshAsync()
    {
        int second = 0;
        updateSecond = 0;
        nowSecond = second;




        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/strength/getShopStrength", DataManager.Get().getHeader());
        Debug.Log(str);
        if (str == null)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "network error"));
            return;
        }
        else{

            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();

            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }

            GetShopStrengthData d = JsonUtil.ReadData<GetShopStrengthData>(str);
            if (DataManager.Get().roleAttrData.strength.strength >= DataManager.Get().roleAttrData.strength.maxStrength) {
                timeDesc.gameObject.SetActive(false);
            }
            else 
            {
                timeDesc.gameObject.SetActive(true);
                second = d.restRecoveryTime; //(DataManager.Get().roleAttrData.strength.maxStrength - DataManager.Get().roleAttrData.strength.strength) * 60 * 60;

                nowSecond = second;
                timeDesc.text = "Countdown to physical recovery:\r\n" +
                    (second / 3600) + "h:" + (second % 3600 / 60) + "m" + (second % 3600 % 60) + "s";
                if(nowSecond==0)
                    timeDesc.gameObject.SetActive(false);
            }

            foreach (var info in d.data)
            {
                Debug.Log(info.id +": "+info.price +"   ===" + info.num);

                infosMap[info.id] = info;

                if (info.id == "f_001")
                {
                    //priceText_1.text = info.price + "";
                    buyNumText_2.text = "Purchased times:" + info.num;
                }
                else {
                    priceText_1.text = info.price + "";
                    buyNumText_1.text = "Purchased times:" + info.num;
                }
            }
        }

       
    }

    int nowSecond;
    float updateSecond;
    private void Update()
    {
        if (nowSecond > 0)
        {
            updateSecond += Time.deltaTime;
            int second = nowSecond - (int)updateSecond;
            timeDesc.text = "Countdown to physical recovery:\r\n" + (second / 3600) + "h:" + (second % 3600 / 60) + "m:" + (second % 3600 % 60) + "s";
        }
    }


    //防止重复点击导致多次购买
    bool buyIngFlag;

    async Task buyAsync(string id) {

        if (buyIngFlag)
            return;

        buyIngFlag = true;

        Debug.Log("买体力");

        NetShopPostData d1 = new NetShopPostData();
        d1.type = "SHOP_STRENGTH";
        d1.id = id;
        Debug.Log("d1.id:" + d1.id);
        string json = JsonConvert.SerializeObject(d1);
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());

        if (str == null)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Error"));
            buyIngFlag = false;
            return;
        }

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode != null)
        {
            //显示资源不够
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            buyIngFlag = false;
            return;
        }

        NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
        Debug.Log(dr.id + "结果:" + str);

        NetShopPayPostData d2 = new NetShopPayPostData();
        d2.type = "SHOP_STRENGTH";
        d2.orderId = dr.id;
        d2.productId = dr.productId;
        d2.productToken = "Gem pay";

        string json2 = JsonConvert.SerializeObject(d2);
        string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

        Debug.Log("trypay:" + str2);




        if (str2 != null)
        {

            JObject obj2 = (JObject)JsonConvert.DeserializeObject(str2);
            NetData NetData2 = obj2.ToObject<NetData>();
            if (NetData2.errorCode != null) {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData2.message));
                buyIngFlag = false;
                return;
            }


            Debug.Log(ShopFactory.Get().StrengthShopList.Count+"trypay-----:" + id);
            StrengthShopConfig config = ShopFactory.Get().StrengthShopList.Find(x => x.id == id);
            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(new ItemInfo("p10009", config.num));            
            MessageMgr.SendMsg("GetReward", new MsgKV("", items));
        }

        buyIngFlag = false;
        await refreshAsync();
    }
}


//pub/strength/getShopStrength接口返回参数
public class GetShopStrengthData
{
    public List<ItemInfo> data;
    public int restRecoveryTime;
}