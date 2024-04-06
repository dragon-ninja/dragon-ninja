using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GoldSlot : BaseSlot
{
    TextMeshProUGUI num;
    TextMeshProUGUI desc;
    //货币类型图标
    Image priceImg;
    TextMeshProUGUI price;
    DiamondAndGoldConfig nowConfig;

    protected override void Awake()
    {
        base.Awake();
        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        num = UIFrameUtil.FindChildNode(this.transform, "name").GetComponent<TextMeshProUGUI>();
        price = UIFrameUtil.FindChildNode(this.transform, "price").GetComponent<TextMeshProUGUI>();
        priceImg = UIFrameUtil.FindChildNode(this.transform, "priceShow/Image").GetComponent<Image>();

        this.GetComponent<Button>().onClick.AddListener(async () => {
            //MessageMgr.SendMsg("buyGold", new MsgKV("", nowConfig));

            NetShopPostData d1 = new NetShopPostData();
            d1.type = "DIAMOND_GOLD";
            d1.id = nowConfig.id;
            Debug.Log("d1.id:" + d1.id);
            string json = JsonConvert.SerializeObject(d1);
            string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/pay", json, DataManager.Get().getHeader());


            Debug.Log("-------------------------------------------");
            Debug.Log(str);

            if (str == null) {
                //没有网络
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
                return;
            }


            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            Debug.Log(NetData);
            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }


            NetShopPostDataReturn dr = JsonUtil.ReadData<NetShopPostDataReturn>(str);
            Debug.Log(dr.id + "结果:" + str);
            NetShopPayPostData d2 = new NetShopPayPostData();
            d2.type = "DIAMOND_GOLD";
            d2.orderId = dr.id;
            d2.productId = dr.productId;
            d2.productToken = "Gem pay";
            string json2 = JsonConvert.SerializeObject(d2);
            string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/mall/payEnd", json2, DataManager.Get().getHeader());
           
            JObject obj2 = (JObject)JsonConvert.DeserializeObject(str2);
            NetData NetData2 = obj2.ToObject<NetData>();
            Debug.Log("trypay:" + str2);
            if (NetData2.errorCode != null) {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData2.message));
                return;
            }
            else if (str2 != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<ItemInfo> items = new List<ItemInfo>();
                items.Add(new ItemInfo(nowConfig.itemList.id, nowConfig.itemList.num)); 
                MessageMgr.SendMsg("GetReward", new MsgKV("", items));
            }

        });
    }

    public void Refresh(DiamondAndGoldConfig dg)
    {
        nowConfig = dg;
        num.text = dg.itemList.num+"";
        desc.text = dg.desc;
        price.text = "x"+ dg.price + "";

        LayoutRebuilder.ForceRebuildLayoutImmediate
           (price.transform.parent.GetComponent<RectTransform>());
    }
}
