using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class MissionSlot : BaseSlot
{
    TextMeshProUGUI desc;
    TextMeshProUGUI rewardNum;
    Slider Slider;
    TextMeshProUGUI SliderDesc;
    Image buttonImg;
    Image getImg;
    TextMeshProUGUI buttonDesc;

    protected override void Awake()
    {
        base.Awake();


        desc = UIFrameUtil.FindChildNode(this.transform, "desc/Text (TMP)").GetComponent<TextMeshProUGUI>();
        icon = transform.Find("box/icon").GetComponent<Image>();
        rewardNum = UIFrameUtil.FindChildNode(this.transform, "box/num").GetComponent<TextMeshProUGUI>();
        Slider = UIFrameUtil.FindChildNode(this.transform, "Slider").GetComponent<Slider>();
        SliderDesc = UIFrameUtil.FindChildNode(this.transform, "Slider/Text (TMP)").GetComponent<TextMeshProUGUI>();

        buttonImg = UIFrameUtil.FindChildNode(this.transform, "Button").GetComponent<Image>();
        getImg = UIFrameUtil.FindChildNode(this.transform, "getImg").GetComponent<Image>();
        buttonDesc = UIFrameUtil.FindChildNode(this.transform, "Button/Text (TMP)").GetComponent<TextMeshProUGUI>();

        UIFrameUtil.FindChildNode(this.transform, "Button").GetComponent<Button>().onClick.AddListener(async () => {
            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/task/receive?taskId="+nowData.taskId,  DataManager.Get().getHeader());
            Debug.Log(str);
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();
            if (NetData.errorCode != null)
            {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
                return;
            }
            Debug.Log(NetData.data);
            NetMissionData data = JsonUtil.ReadData<NetMissionData>(str);
            Debug.Log(data.data.id);
            if (data != null)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<ItemInfo> items = new List<ItemInfo>();
                items.Add(new ItemInfo(data.data.id, data.data.num));
                MessageMgr.SendMsg("GetReward", new MsgKV("", items));
            }

            ((MissionForm)mgr).RefreshAsync(nowData.taskType);
            MessageMgr.SendMsg("RefreshTip", null);
        });
    }

    NetTaskInfo nowData;

    public void Refresh(NetTaskInfo data)
    {
        Show();

        nowData = data;

        MissionConfig config = PerimeterFactory.Get().MissionConfigList.Find(item => item.id == data.taskId);
        Debug.Log("config.rewards[0]:"+ config.rewards);
        rewardNum.text = config.rewards.num + "";
        desc.text = config.desc;
       
        Slider.value = (data.num+0.0f) / (data.targetNum + 0.0f) ;
        SliderDesc.text = data.num + " / "+ data.targetNum;

        icon.sprite = Resources.Load<Sprite>(ItemFactory.Get().itemMap[config.rewards.id].icon);

        getImg.gameObject.SetActive(false);
        if (data.hasReceive) {
         
            buttonImg.gameObject.SetActive(false);
            getImg.gameObject.SetActive(true);
        }
        else if (data.num == data.targetNum)
        {
            buttonDesc.text = "Receive";
            buttonImg.gameObject.SetActive(true);
        }
        else {
            buttonImg.gameObject.SetActive(false);
            buttonDesc.text = "Go to";
        }

    }
}


public class NetMissionData
{
    public bool result;
    public ItemInfo data;
}