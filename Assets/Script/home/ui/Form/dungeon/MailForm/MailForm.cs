using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class MailForm : BaseUIForm
{
    List<MailSlot> slotList;
    GameObject slotPf;
    Transform slotListTra;
    GameObject mailPanel;

    //邮件详情面板
    TextMeshProUGUI mailBt;
    TextMeshProUGUI mailDescContent;
    List<ItemSlot> itemSlotList;
    Transform itemSlotListTra;
    GameObject itemSlotPf;
    RectTransform ScrollbarVerticalTra;
    RectTransform ViewportTra;

    MailInfo nowInfo;
    Button Button_receive;

    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        mailPanel = UIFrameUtil.FindChildNode(this.transform, "mailPanel").gameObject;
        mailBt = UIFrameUtil.FindChildNode(this.transform, "MailBt/Text (TMP)").GetComponent<TextMeshProUGUI>();
        mailDescContent = UIFrameUtil.FindChildNode(this.transform, "MailDescContent").GetComponent<TextMeshProUGUI>();
        ViewportTra = UIFrameUtil.FindChildNode(this.transform,
            "MailScroll View/Viewport").GetComponent<RectTransform>();
        ScrollbarVerticalTra = UIFrameUtil.FindChildNode(this.transform,
           "MailScroll View/Scrollbar Vertical").GetComponent<RectTransform>();


        slotListTra = UIFrameUtil.FindChildNode(this.transform, "MailContent");
        slotList = new List<MailSlot>();
        slotPf = slotListTra.GetChild(0).gameObject;
        for (int i = 0; i < slotListTra.childCount; i++)
        {
            MailSlot slot = slotListTra.GetChild(i).GetComponent<MailSlot>();
            slot.mgr = this;
            slotList.Add(slot);
        }

        itemSlotListTra = UIFrameUtil.FindChildNode(this.transform, "itemList");
        itemSlotList = new List<ItemSlot>();
        itemSlotPf = itemSlotListTra.GetChild(0).gameObject;
        for (int i = 0; i < itemSlotListTra.childCount; i++)
        {
            ItemSlot slot = itemSlotListTra.GetChild(i).GetComponent<ItemSlot>();
            slot.mgr = this;
            itemSlotList.Add(slot);
        }

        GetBut(this.transform, "Panel/Image/close").onClick.AddListener(() => {
            CloseForm();
        });
        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm();
        });
        GetBut(this.transform, "mailPanel/panel/close").onClick.AddListener(() => {
            mailPanel.SetActive(false);
            RefreshAsync();
        });

        GetBut(this.transform, "mailPanel").onClick.AddListener(() => {
            mailPanel.SetActive(false);
            RefreshAsync();
        });
        Button_receive = GetBut(this.transform, "Button_receive");
        Button_receive.onClick.AddListener(() => {
            receiveAsync();
        });

        GetBut(this.transform, "Button_deleteAll").onClick.AddListener(async () => {
            string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/mail/deleteAllRead", DataManager.Get().getHeader());
            Debug.Log(str);
            RefreshAsync();
        });
       

        MessageMgr.AddMsgListener("OpenMail", p =>
        {
            OpenMailAsync((MailInfo)p.Value);
        });

    }


    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }


    public async Task RefreshAsync() {
        for (int i = 0; i < slotList.Count; i++) {
            slotList[i].Hide();
        }

        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/mail/getList?page=0&pageSize=100", DataManager.Get().getHeader());
        Debug.Log(str);
   
       

        MailNetData datas = JsonUtil.ReadData<MailNetData>(str);
        if (datas != null) {

            datas.content.Sort((a, b) =>
                (a.readStatus).CompareTo(b.readStatus));

            for (int i = 0; i < datas.content.Count; i++)
            {
                if (i >= slotList.Count)
                {
                    GameObject g = Instantiate(slotPf, slotListTra);
                    MailSlot slot = g.GetComponent<MailSlot>();
                    slot.mgr = this;
                    slotList.Add(slot);
                }
                slotList[i].Refresh(datas.content[i]);
            }
        }
    }


    public async Task OpenMailAsync(MailInfo data) {
        mailPanel.SetActive(true);

        nowInfo = data;

        mailBt.text = data.title;
        mailDescContent.text = data.content;

       

        if (data.readStatus != "NO_READ")
            Button_receive.gameObject.SetActive(false);
        else { 
            Button_receive.gameObject.SetActive(true);
            //没有可领资源 自动触发领取接口
            if (nowInfo.material.Count == 0 && nowInfo.consumables.Count == 0) {
                await NetManager.get(ConfigCheck.publicUrl+"/data/pub/mail/receive?id=" + (nowInfo.id), DataManager.Get().getHeader());
                Button_receive.gameObject.SetActive(false);
            }
        }


        List<ItemInfo> items = new List<ItemInfo>();
        foreach (ItemInfo it in data.material) {
            items.Add(new ItemInfo(it.id, it.num, it.quality, it.level));
        }
        foreach (ItemInfo it in data.consumables)
        {
            items.Add(new ItemInfo(it.id, it.num, it.quality, it.level));
        }


        if (items != null && items.Count > 0)
        {
            itemSlotListTra.gameObject.SetActive(true);
            ViewportTra.offsetMin = new Vector2(ViewportTra.offsetMin.x, 200);
            ScrollbarVerticalTra.offsetMin = new Vector2(ScrollbarVerticalTra.offsetMin.x, 205);

            for (int i = 0; i < itemSlotList.Count; i++)
                itemSlotList[i].Hide();

            for (int i = 0; i < items.Count; i++)
            {
                if (i >= itemSlotList.Count)
                {
                    GameObject g = Instantiate(itemSlotPf, itemSlotListTra);
                    ItemSlot slot = g.GetComponent<ItemSlot>();
                    slot.mgr = this;
                    itemSlotList.Add(slot);
                }
                itemSlotList[i].Refresh(items[i]);
                itemSlotList[i].mask.gameObject.SetActive(false);
                if (data.readStatus != "NO_READ") {
                    itemSlotList[i].mask.gameObject.SetActive(true);
                }
            }
        }
        else {
            itemSlotListTra.gameObject.SetActive(false);
            ViewportTra.offsetMin = new Vector2(ViewportTra.offsetMin.x, 50);
            ScrollbarVerticalTra.offsetMin = new Vector2(ScrollbarVerticalTra.offsetMin.x, 55);
        }

        MessageMgr.SendMsg("RefreshTip", null);
    }

    public async Task receiveAsync(){
   
        string str = await NetManager.get
            (ConfigCheck.publicUrl+"/data/pub/mail/receive?id="+(nowInfo.id), DataManager.Get().getHeader());

        if (str != null)
        {
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            NetData NetData = obj.ToObject<NetData>();

            if (NetData.errorCode == null)
            {
                if (nowInfo.material.Count>0 || nowInfo.consumables.Count > 0) { 
                    UIManager.GetUIMgr().showUIForm("RewardForm");
                    List<ItemInfo> items = new List<ItemInfo>();
                    foreach (ItemInfo it in nowInfo.material)
                    {
                        items.Add(new ItemInfo(it.id, it.num, it.quality, it.level));
                    }
                    foreach (ItemInfo it in nowInfo.consumables)
                    {
                        items.Add(new ItemInfo(it.id, it.num, it.quality, it.level));
                    }
                    MessageMgr.SendMsg("GetReward", new MsgKV("", items));

                    nowInfo.readStatus = "READ";
                    OpenMailAsync(nowInfo);
                }
            }
            else {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            }
        }
        else {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Error"));
        }
    }
}


public class MailData {
    public string name = "TestMail";
    public string desc;
    public string time = "2023.1.1";
    public string countdown = "10";
    public List<ItemInfo> items;

    public bool readFlag;
    public bool receiveFlag;
}
public class MailNetData
{
    public int pageNumber;
    public int pageSize;
    public int totalElements;
    public List<MailInfo> content;
    public bool first;
    public bool last;
    public int totalPages;
}
public class MailInfo
{
    public string id;
    public string userId;
    public string title;
    public string content;
    public string createTime;
    public List<ItemInfo> material;
    public List<ItemInfo> consumables;
    public string readStatus; // NO_READ",
    public bool delete;
    
}