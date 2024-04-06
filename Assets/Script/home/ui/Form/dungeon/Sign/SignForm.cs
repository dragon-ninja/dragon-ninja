using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SignForm : BaseUIForm
{
    List<SignAccrueSlot> accrueSlotList;
    List<SignSlot> slotList;


    GameObject dayList_obj1;
    GameObject dayList_obj2;
    TextMeshProUGUI loadingDesc_obj;

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

        dayList_obj1 = UIFrameUtil.FindChildNode(this.transform, "dayList").gameObject;
        dayList_obj2 = UIFrameUtil.FindChildNode(this.transform, "7day").gameObject;
        loadingDesc_obj = UIFrameUtil.FindChildNode(this.transform, "loadingDesc").GetComponent<TextMeshProUGUI>();

        Transform accrueSlotListTra = UIFrameUtil.FindChildNode(this.transform, "boxList/list");
        accrueSlotList = new List<SignAccrueSlot>();
        for (int i = 0; i < accrueSlotListTra.childCount; i++)
        {
            SignAccrueSlot slot = accrueSlotListTra.GetChild(i).GetComponent<SignAccrueSlot>();
            slot.mgr = this;
            accrueSlotList.Add(slot);
        }

        Transform slotTra = UIFrameUtil.FindChildNode(this.transform, "dayList");
        slotList = new List<SignSlot>();
        for (int i = 0; i < slotTra.childCount; i++)
        {
            SignSlot slot = slotTra.GetChild(i).GetComponent<SignSlot>();
            slot.mgr = this;
            slotList.Add(slot);
        }
        slotList.Add(
            UIFrameUtil.FindChildNode(this.transform, "7day")
            .GetComponent<SignSlot>()
            );

        
    }

    public override void Show()
    {
        base.Show();
        Refresh();
    }


    public async void Refresh()
    {
        dayList_obj1.SetActive(false);
        dayList_obj2.SetActive(false);
        loadingDesc_obj.gameObject.SetActive(true);

        string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/sign/list", DataManager.Get().getHeader());

        Debug.Log("Sign:" + str1);

        SevenDaySignData signdata = null;
        Int64 nowTime = 0;
        if (str1 == null)
        {
            //Debug.Log("Signxx");
        }
        else { 
        
            //Debug.Log("Sign:" + str1);
            signdata = JsonUtil.ReadData<SevenDaySignData>(str1);
            //Debug.Log("Sign---:" + signdata.data[0].nextSignTime);
            if (signdata != null)
            {
                nowTime = signdata.currentTime;
            }
        }



        List<SignConfig> configList = PerimeterFactory.Get().SignList;
       
        for (int i = 0; i < slotList.Count; i++)
        {
            if (signdata != null && signdata.data.Count > i)
            {
                slotList[i].Refresh(configList[i].id, true, true);
            }
            else {
                if (signdata != null && signdata.data.Count == i)
                {
                    if (nowTime >= signdata.data[0].nextSignTime)
                    {
                        slotList[i].Refresh(configList[i].id, false, true);
                    }
                    else
                    {
                        slotList[i].Refresh(configList[i].id, false, false);
                    }
                }
                else {
                    if (i == 0)
                    {
                        slotList[i].Refresh(configList[i].id, false, true);
                    }
                    else { 
                        slotList[i].Refresh(configList[i].id, false, false);
                    }
                }
            }

        }

        dayList_obj1.SetActive(true);
        dayList_obj2.SetActive(true);
        loadingDesc_obj.gameObject.SetActive(false);

        /* Sign:{"errorCode":null,"message":null,"data":{"nextTime":1689163847412,"currentTime":1689166659637,"
         * data":[{"id":"64ad46c70a2365009873b2a4",
         * "userId":307,"platform":"ANDROID",
         * "signTime":1689077447412,
         * "nextSignTime":1689163847412,"day":1,"coiled":1}]}}*/



        //--------------------------------------------------
        //accrueSlotList[0].Refresh("dayShowFlag");
        /*for (int i = 1; i < accrueSlotList.Count; i++)
        {
            accrueSlotList[i].Refresh(configList[i+6].id,false);
         }*/
    }

    float loadingtime;
    private void Update()
    {
        if (loadingDesc_obj.gameObject.activeInHierarchy) {

            loadingtime += Time.deltaTime;
           
            if (loadingtime <= 0.25f)
                loadingDesc_obj.text = "loading";
            else if (loadingtime<=0.5f)
                loadingDesc_obj.text = "loading.";
            else if (loadingtime <= 0.75f)
                loadingDesc_obj.text = "loading..";
            else if (loadingtime <= 1f)
                loadingDesc_obj.text = "loading...";
            else
                loadingtime = 0;
        }

    }
}
