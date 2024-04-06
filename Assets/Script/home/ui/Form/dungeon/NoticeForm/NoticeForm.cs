using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class NoticeForm : BaseUIForm
{
    public List<Image> imgList;
    GameObject imgPf;
    Transform imgListTra;

    TextMeshProUGUI DescContent;

    int nowIndex = 0;
    int maxIndex = 5;

    public override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        initAsync();
    }

    public void initAsync() {
        if (initFlag)
            return;

        initFlag = true;

        canvasGroup.alpha = 1;
        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.Normal;
        ui_type.IsClearStack = false;

        DescContent = UIFrameUtil.FindChildNode(this.transform, "DescContent").GetComponent<TextMeshProUGUI>();

        imgListTra = UIFrameUtil.FindChildNode(this.transform, "list");
        imgList = new List<Image>();
        imgPf = imgListTra.GetChild(0).gameObject;
        for (int i = 0; i < imgListTra.childCount; i++)
        {
            Image slot = imgListTra.GetChild(i).GetComponent<Image>();
            imgList.Add(slot);
        }

        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            if (UIManager.GetUIMgr() != null)
                CloseForm();
            else
                Hide();
        });

        GetBut(this.transform, "button_left").onClick.AddListener(() => {
            left();
        });

        GetBut(this.transform, "button_right").onClick.AddListener(() => {
            right();
        });


     
    }
    List<NoticeNetData> infoList;

    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }

    public async Task RefreshAsync() {

        string str = await NetManager.get(ConfigCheck.publicUrl + "/data/pub/notice/getNotice", DataManager.Get().getHeader());
        Debug.Log(str);
        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        NetData NetData = obj.ToObject<NetData>();
        if (NetData.errorCode != null)
        {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", NetData.message));
            return;
        }
        JArray obj1 = (JArray)JsonConvert.DeserializeObject(NetData.data.ToString());
        infoList = obj1.ToObject<List<NoticeNetData>>();

        string desc = "";
        
        foreach (string ss in infoList[nowIndex].content) {
            desc += ss + "\r\n";
        }

        Debug.Log(infoList[nowIndex].title);
        
        DescContent.text = "           <size=100>"+infoList[nowIndex].title + "</size>\r\n" + desc;


        /*DescContent.text = "           <size=100>活动预告" + nowIndex + "</size>\r\n" +
            "1.xxxxxxxxxxxx\r\n2.xxxxxxxxxxxx\r\n3.xxxxxxxxxxxx";*/



        maxIndex = Mathf.Max(0, infoList.Count - 1);

        for (int i = 0; i < infoList.Count; i++)
        {
            if (i >= imgList.Count)
            {
                GameObject g = Instantiate(imgPf, imgListTra);
                imgList.Add(g.GetComponent<Image>());
            }

            if (i == nowIndex)
                imgList[i].sprite = Resources.Load<Sprite>("ui/img/mail/y_l");
            else
                imgList[i].sprite = Resources.Load<Sprite>("ui/img/mail/y_h");
        }
    }


    void left() {
        nowIndex -= 1;
        nowIndex = Mathf.Max(0, nowIndex);

        RefreshAsync();
    }
    void right() {
        nowIndex += 1;
        nowIndex = Mathf.Min(nowIndex, maxIndex);

        RefreshAsync();
    }


    private float fingerActionSensitivity = Screen.width * 0.05f;
    private float fingerBeginX;
    private float fingerBeginY;
    private float fingerCurrentX;
    private float fingerCurrentY;
    private float fingerSegmentX;
    private float fingerSegmentY;

    private int fingerTouchState;
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;

    void Start()
    {
        fingerActionSensitivity = Screen.width * 0.05f;
        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;
        fingerTouchState = FINGER_STATE_NULL;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (fingerTouchState == FINGER_STATE_NULL)
            {
                fingerTouchState = FINGER_STATE_TOUCH;
                fingerBeginX = Input.mousePosition.x;
                fingerBeginY = Input.mousePosition.y;
            }
        }
        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            fingerCurrentX = Input.mousePosition.x;
            fingerCurrentY = Input.mousePosition.y;
            fingerSegmentX = fingerCurrentX - fingerBeginX;
            fingerSegmentY = fingerCurrentY - fingerBeginY;

        }
        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;

            if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
            {
                toAddFingerAction();
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            fingerTouchState = FINGER_STATE_NULL;
        }
    }

    private void toAddFingerAction()
    {
        fingerTouchState = FINGER_STATE_ADD;
        if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        {
            fingerSegmentY = 0;
        }
        else
        {
            fingerSegmentX = 0;
        }

        if (fingerSegmentX == 0)
        {
            if (fingerSegmentY > 0)
            {
                //Debug.Log("up");
            }
            else
            {
                //Debug.Log("down");
            }
        }
        else if (fingerSegmentY == 0)
        {
            if (fingerSegmentX > 0)
            {
                //Debug.Log("right");
                left();
            }
            else
            {
                right();
                //Debug.Log("laft");
            }
        }
    }



}

public class NoticeData {
    public string name = "TestMail";
    public string desc;
    public string time = "2023.1.1";
    public string countdown = "10";
    public bool readFlag;
    public bool receiveFlag;

}
public class NoticeNetData
{
    public string title ;
    public List<string> content;
}
