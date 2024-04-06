using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class PassCheckSlot : BaseSlot
{
    public string type;
    TextMeshProUGUI num;
    GameObject mask;
    GameObject lockImg;
    TextMeshProUGUI received;
    public string congfig_id;

    protected override void Awake()
    {
        base.Awake();
        received = UIFrameUtil.FindChildNode(this.transform, "Received/ReceivedText").GetComponent<TextMeshProUGUI>();
        num = UIFrameUtil.FindChildNode(this.transform, "num").GetComponent<TextMeshProUGUI>();
        mask = UIFrameUtil.FindChildNode(this.transform, "mask").gameObject;
        lockImg = UIFrameUtil.FindChildNode(this.transform, "lock").gameObject;


        this.GetComponent<Button>().onClick.AddListener(async () => {
            PassCheckNetPost post = new PassCheckNetPost();
            post.rewardsId = congfig_id;
            post.tokenId = type;
            string json = JsonConvert.SerializeObject(post);
            string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/battlePass/save", json, DataManager.Get().getHeader());
            Debug.Log(str);

            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();

            if (NetData.errorCode != null)
            {
                //显示资源不够
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }

           
            UIManager.GetUIMgr().showUIForm("RewardForm");
            List<ItemInfo> items = new List<ItemInfo>();
            items.Add(nowItemInfo);
            MessageMgr.SendMsg("GetReward", new MsgKV("", items));
            ((PassCheckForm)mgr).RefreshAsync();
            MessageMgr.SendMsg("RefreshTip", null);
        });
    }

    ItemInfo nowItemInfo;

    ///等级是否解锁  是否已购买  是否已领取
    public void Refresh(ItemInfo info, bool unlockFlag,bool buyFlag,bool drawFlag = false) {
        Show();
        nowItemInfo = info;
        string iconUrl = ItemFactory.Get().itemMap[info.id].icon;
        icon.sprite = Resources.Load<Sprite>(iconUrl);
        num.text = "x" + info.num;

        if (unlockFlag)
            mask.gameObject.SetActive(false);
        else
            mask.gameObject.SetActive(true);

        if (buyFlag)
            lockImg.gameObject.SetActive(false);
        else
            lockImg.gameObject.SetActive(true);


        if (drawFlag)
        {
            received.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            received.transform.parent.gameObject.SetActive(false);
        }

    }

}
