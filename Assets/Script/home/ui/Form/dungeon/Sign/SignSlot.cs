using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class SignSlot : BaseSlot
{
    List<ItemSlot> slotList;
    GameObject img;
    string configId;

    protected override void Awake()
    {
        initFlag = true;
        background = GetComponent<Image>();
        myBut = GetComponent<Button>();

        img = UIFrameUtil.FindChildNode(this.transform, "img").gameObject;

        slotList = new List<ItemSlot>();

        //单个类型
        if (this.gameObject.name.IndexOf("7day") == -1)
        {
            ItemSlot slot = UIFrameUtil.FindChildNode(this.transform, "item").GetComponent<ItemSlot>();
            slotList.Add(slot);
        }
        else { 
            //7day 有多个物品
            Transform slotTra = UIFrameUtil.FindChildNode(this.transform, "list");
            for (int i = 0; i < slotTra.childCount; i++)
            {
                ItemSlot slot = slotTra.GetChild(i).GetComponent<ItemSlot>();
                slot.mgr = this;
                slotList.Add(slot);
            }
        }

        this.GetComponent<Button>().onClick.AddListener(async () => {
            /*UIManager.GetUIMgr().showUIForm("RewardForm");
            List<SignConfig> configList = PerimeterFactory.Get().SignList;
            SignConfig config = configList.Find(item => item.id == configId);
            MessageMgr.SendMsg("GetReward", new MsgKV("", config.items));
*/
            bool flag = await trySgin();
            if (flag)
            {
                UIManager.GetUIMgr().showUIForm("RewardForm");
                List<SignConfig> configList = PerimeterFactory.Get().SignList;
                SignConfig config = configList.Find(item => item.id == configId);
                MessageMgr.SendMsg("GetReward", new MsgKV("", config.items));
            }
            ((SignForm)mgr).Refresh();
            MessageMgr.SendMsg("RefreshTip", null);
        });
    }




    async Task<bool> trySgin()
    {
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/sign", DataManager.Get().getHeader());
        Debug.Log("trySgin:" + str);

        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="getFlag">是否領取</param>
    /// <param name="unlock"> 是否解鎖</param>
    public void Refresh(string id, bool getFlag, bool unlock = false)
    {

        if (!initFlag)
            Awake();

        this.configId = id;

        myBut.interactable = !getFlag && unlock;
        img.SetActive(!getFlag && unlock);
        //根据day 填充槽内显示
        List<SignConfig> configList = PerimeterFactory.Get().SignList;
        SignConfig config = configList.Find(item => item.id == configId);


        if (getFlag)
            background.sprite = Resources.Load<Sprite>("ui/img/sign/绿色");
        else
            background.sprite = Resources.Load<Sprite>("ui/img/sign/黄色");


        for (int i = 0; i < slotList.Count;i++)
        {
            if (i >= config.items.Count)
            {
                slotList[i].Hide();
            }
            else { 
                slotList[i].Refresh(config.items[i]);
            }
        }
    }
}
