using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DailyShopSlot : BaseSlot
{
    TextMeshProUGUI itemName;
    //提醒玩家购买的图标  
    Image img;
    Image dk;
    TextMeshProUGUI num;
    //货币类型图标
    Image priceImg;
    TextMeshProUGUI price;
    TextMeshProUGUI preferential;
    DailyShopData nowData;

    GameObject selloutTra;

    bool lookAdsIng;

    protected override void  Awake(){
        base.Awake();

        itemName = UIFrameUtil.FindChildNode(this.transform, "name").
            GetComponent<TextMeshProUGUI>();

        img = UIFrameUtil.FindChildNode(this.transform, "img").
          GetComponent<Image>();

        icon = UIFrameUtil.FindChildNode(this.transform, "dk/icon").
            GetComponent<Image>();

        dk = UIFrameUtil.FindChildNode(this.transform, "dk").
            GetComponent<Image>();


        num = UIFrameUtil.FindChildNode(this.transform, "dk/Text (TMP)").
            GetComponent<TextMeshProUGUI>();

        priceImg = UIFrameUtil.FindChildNode(this.transform, "gold/Image").
            GetComponent<Image>();
        
        price = UIFrameUtil.FindChildNode(this.transform, "gold/value").
            GetComponent<TextMeshProUGUI>();

        selloutTra = UIFrameUtil.FindChildNode(this.transform, "sell out").gameObject;

        preferential = UIFrameUtil.FindChildNode(this.transform, "preferential/value").
            GetComponent<TextMeshProUGUI>();


        this.GetComponent<Button>().onClick.AddListener(async () => {

            if (data.currency != "free")
            {
                MessageMgr.SendMsg("buyDailyShop", new MsgKV("", data));
            }
            else {
                if (data.payedNum > 0 && data.payedNum < 3) { 
                    GoogleAdsManager.Instance.testAd();
                    lookAdsIng = true;
                    return;
                }

                NetShopPostData d1 = new NetShopPostData();
                d1.type = "DAILY_SHOP";
                d1.id = this.data.id;
                d1.shopDailyListId = this.data.dailyId;
                string json = JsonConvert.SerializeObject(d1);
                string str = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/pay", json, DataManager.Get().getHeader());


                JObject obj = (JObject)JsonConvert.DeserializeObject(str);
                NetData NetData = obj.ToObject<NetData>();
                if (NetData.errorCode != null)
                {
                    //显示资源不够
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                    MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                    return;
                }

                Debug.Log("结果:" + str);
                NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);

                NetShopPayPostData d2 = new NetShopPayPostData();
                d2.type = "DAILY_SHOP";
                d2.orderId = dr.id;
                d2.productId = dr.productId;
                string json2 = JsonConvert.SerializeObject(d2);
                string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

                Debug.Log("trypay:" + str2);


                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<ItemInfo> items = new List<ItemInfo>();
                items.Add(new ItemInfo(data.itemId, data.num, data.quality))
    ;           MessageMgr.SendMsg("GetReward", new MsgKV("", items));

                MessageMgr.SendMsg("RefreshDailyShop", new MsgKV(null, null));
            }
        });

        //广告观看奖励
        MessageMgr.AddMsgListener("lookAdsEnd", async p =>
        {
            if (data.currency != "free")
                return;
            if (!lookAdsIng)
                return;

            lookAdsIng = false;

            NetShopPostData d1 = new NetShopPostData();
            d1.type = "DAILY_SHOP";
            d1.id = this.data.id;
            d1.shopDailyListId = this.data.dailyId;
            string json = JsonConvert.SerializeObject(d1);
            string str = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/pay", json, DataManager.Get().getHeader());


            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }

            Debug.Log("结果:" + str);
            NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);

            NetShopPayPostData d2 = new NetShopPayPostData();
            d2.type = "DAILY_SHOP";
            d2.orderId = dr.id;
            d2.productId = dr.productId;
            string json2 = JsonConvert.SerializeObject(d2);
            string str2 = await NetManager.post(ConfigCheck.publicUrl + "/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());

            Debug.Log("trypay:" + str2);


            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(new ItemInfo(data.itemId, data.num, data.quality))
;           MessageMgr.SendMsg("GetReward", new MsgKV("", items));

            MessageMgr.SendMsg("RefreshDailyShop", new MsgKV(null, null));

        });

    }

    public void Refresh(DailyShopData data)
    {
        nowData = data;

        ShopForm shopForm = (ShopForm)mgr;
        
        //根据id找到相应的物品图标
        
        string iconUrl = null;
      

        if (data.currency == "gem")
        {
            iconUrl = shopForm.equipmentf.map[data.itemId].icon;
            itemName.text = shopForm.equipmentf.map[data.itemId].name;
            num.gameObject.SetActive(false);
        }
        else {
            iconUrl = shopForm.itemf.itemMap[data.itemId].icon;
            itemName.text = shopForm.itemf.itemMap[data.itemId].name;
            num.text = "x" + data.num;
            num.gameObject.SetActive(true);
        }



        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);

        //Debug.Log("data.price:"+ data.price+"   "+ data.preferential);

        price.text = "x" + Mathf.FloorToInt(data.price * (data.preferential / 10.0f));
        priceImg.gameObject.SetActive(true);
       
        if (data.currency == "gem")
            priceImg.sprite = Resources.Load<Sprite>("ui/icon/钻石");
        else if (data.currency == "gold")
            priceImg.sprite = Resources.Load<Sprite>("ui/icon/gold");
        else { 
            priceImg.gameObject.SetActive(false);
            price.text = "Free";
        }

        preferential.text = (data.preferential * 10) +"%";


        dk.sprite = Resources.Load<Sprite>("ui/icon/item/dk/" + data.quality);

        LayoutRebuilder.ForceRebuildLayoutImmediate
            (price.transform.parent.GetComponent<RectTransform>());
    }


    NetDailyShopInfoData data;

    public void Refresh(NetDailyShopInfoData data)
    {
        this.data = data;

        ShopForm shopForm = (ShopForm)mgr;

        //根据id找到相应的物品图标

        string iconUrl = null;


        if (data.currency == "gem")
        {
            iconUrl = shopForm.equipmentf.map[data.itemId].icon;
            itemName.text = shopForm.equipmentf.map[data.itemId].name;
            num.gameObject.SetActive(false);
        }
        else
        {
            iconUrl = shopForm.itemf.itemMap[data.itemId].icon;
            itemName.text = shopForm.itemf.itemMap[data.itemId].name;
            num.text = "x" + data.num;
            num.gameObject.SetActive(true);
        }

        if (data.currency == "free") {
            img.gameObject.SetActive(true);
        }

        //达到最大购买次数
        if (data.payedNum >= data.buyCount)
        {
            img.gameObject.SetActive(false);
            selloutTra.SetActive(true);
            price.transform.parent.gameObject.SetActive(false);
        }
        else {

            selloutTra.SetActive(false);
            price.transform.parent.gameObject.SetActive(true);
        }

        //显示图标
        icon.sprite = Resources.Load<Sprite>(iconUrl);

        //Debug.Log("data.price:"+ data.price+"   "+ data.preferential);

        price.text = "x" + Mathf.FloorToInt(data.discountPrice);
        priceImg.gameObject.SetActive(true);

        if (data.currency == "gem")
            priceImg.sprite = Resources.Load<Sprite>("ui/icon/钻石");
        else if (data.currency == "gold")
            priceImg.sprite = Resources.Load<Sprite>("ui/icon/gold");
        else
        {
            priceImg.gameObject.SetActive(false);
            price.text = "Free";
        }

        preferential.text = (data.discountRate) + "%";


        dk.sprite = Resources.Load<Sprite>("ui/icon/item/dk/" + data.quality);

        LayoutRebuilder.ForceRebuildLayoutImmediate
            (price.transform.parent.GetComponent<RectTransform>());
    }
}
