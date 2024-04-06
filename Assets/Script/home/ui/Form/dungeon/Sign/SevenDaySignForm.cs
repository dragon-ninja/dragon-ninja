using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SevenDaySignForm : BaseUIForm
{
    List<SevenDaySignSlot> slotList;

    TextMeshProUGUI desc;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        GetBut(this.transform, "close").onClick.AddListener(() => {
                CloseForm();
        });
        GetComponent<Button>().onClick.AddListener(() => {
            CloseForm();
        });
        desc = UIFrameUtil.FindChildNode(this.transform, "descText").GetComponent<TextMeshProUGUI>();
        Transform slotTra = UIFrameUtil.FindChildNode(this.transform, "itemlist");
        slotList = new List<SevenDaySignSlot>();
        for (int i = 0; i < slotTra.childCount; i++)
        {
            SevenDaySignSlot slot = slotTra.GetChild(i).GetComponent<SevenDaySignSlot>();
            slot.mgr = this;
            slotList.Add(slot);
        }

       
    }


    public override void Show()
    {
        base.Show();
        Refresh();
    }

    public async void Refresh()
    {
        string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/sevenDaySign/List", DataManager.Get().getHeader());
        Debug.Log("sevenDaySign:"+ str1);
        SevenDaySignData signdata = JsonUtil.ReadData<SevenDaySignData>(str1);

        Int64 nowTime = 0;

        if (signdata != null)
        {
            nowTime = signdata.currentTime;
            long 剩余 = (signdata.data[0].firstSignTime + (7 * 24 * 60 * 60 * 1000) - nowTime) / 1000 / 60 / 60;
            desc.text = "Novice 7-day limited time gift, daily login can receive rewards! The remaining time of the activity is " + (剩余 / 24) + " days and " + (剩余 % 24 + 1) + " hours";
        }
        else {
            desc.text = "Novice 7-day limited time gift, daily login can receive rewards! ";
        }

        for (int i = 0; i < slotList.Count; i++)
        {

            if (signdata != null && signdata.data.Count > i)
            {
                //说明拿过了
                slotList[i].Refresh(i, true, true, 0);        
            }
            else {
                if (signdata != null && signdata.data.Count == i)
                {
                    //校验是否解锁 是否可领取

                   /* if (i == 2) {
                        Debug.Log(" nowTime >= signdata.data[i - 1].nextSignTime:" + (nowTime >= signdata.data[0].nextSignTime));
                    }*/

                    if (nowTime >= signdata.data[0].nextSignTime)
                    {
                        slotList[i].Refresh(i, false, true, 0);
                    }
                    else {

                        int s = Mathf.RoundToInt((signdata.data[0].nextSignTime - nowTime) / 1000 );
                        slotList[i].Refresh(i, false, false, s);
                    }
                }
                else {
                    if (i == 0)
                    {
                        slotList[i].Refresh(i, false, true, 0);
                    }
                    else { 
                        //没解锁 不能领取
                        slotList[i].Refresh(i, false, false, 0);
                    }
                }
            }
        }

    }
}
/*ItemInfo

       {"errorCode":null,"message":null,"data":{"nextTime":null,"data":null}}

       sevenDaySign: { "errorCode":null,"message":null,
               "data":{ "nextTime":1689078796158,
                                   1688995677
                                   1688995831748
                                   1688996127420

                       "data":[{ "id":"64abfa8c8a381f3e526b41f1","userId":307,
                       "firstSignTime":1688992396158,
                       "signTime":1688992396158,
                       "nextSignTime":1689078796158,
                       "index":1,
                       "createTime":"2023-07-10T12:33:16.158+00:00","expireTime":"2024-07-04T18:33:16.158+00:00"}]} }*/

//DataManager.Get().backPackData = JsonUtil.ReadData<BackPackData>(BackPackStr);


public class SevenDaySignData {
    public long nextTime;
    public long currentTime;
    public List<SevenDaySignDataC> data;
}

public class SevenDaySignDataC
{
    public long firstSignTime;
    public long signTime;
    public long nextSignTime;
    public int index;
}
