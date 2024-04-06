using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingForm : BaseUIForm
{
    bool soundFlag;
    bool musicFlag;
    bool shockFlag;
    Slider soundSlider;
    Slider musicSlider;
    Slider shockSlider;

    GameObject RenamePanel;

    TMP_InputField RenameText;

    TextMeshProUGUI nameText;
    TextMeshProUGUI accountText;

    [SerializeField]
    AudioMixer audioMixer;

    RegisterPanel registerP;


    public override void Awake()
    {
        base.Awake();
        canvasGroup.alpha = 1;

        ui_type.ui_FormType = UIformType.PopUp;
        ui_type.ui_ShowType = UIformShowMode.ReverseChange;
        ui_type.IsClearStack = false;

        registerP = UIFrameUtil.FindChildNode(this.transform, "registerPanel").GetComponent<RegisterPanel>();
        registerP.SettingForm = this;

        RenamePanel = UIFrameUtil.FindChildNode(this.transform, "Rename").gameObject;
        soundSlider = UIFrameUtil.FindChildNode(this.transform, "sound/Slider").GetComponent<Slider>();
        musicSlider = UIFrameUtil.FindChildNode(this.transform, "music/Slider").GetComponent<Slider>();
        shockSlider = UIFrameUtil.FindChildNode(this.transform, "shock/Slider").GetComponent<Slider>();
        RenameText = UIFrameUtil.FindChildNode(this.transform, "input_name").GetComponent<TMP_InputField>();

        nameText = UIFrameUtil.FindChildNode(this.transform, "name/Text (TMP)").GetComponent<TextMeshProUGUI>();
        accountText = UIFrameUtil.FindChildNode(this.transform, "name/Text (TMP) (1)").GetComponent<TextMeshProUGUI>();
        soundFlag = DataManager.Get().userData.settingData.soundFlag;
        musicFlag = DataManager.Get().userData.settingData.musicFlag;
        shockFlag = DataManager.Get().userData.settingData.shockFlag;
        soundSlider.value = soundFlag ? 1 : 0;
        musicSlider.value = musicFlag ? 1 : 0;
        shockSlider.value = shockFlag ? 1 : 0;

        GetBut(this.transform, "Rename").onClick.AddListener(() => {
            RenamePanel.SetActive(false);
        });

/*        GetBut(this.transform, "Rename/Panel/Button").onClick.AddListener(() => {
            RenamePanel.SetActive(false);
        });*/

        GetBut(this.transform, "name/Button").onClick.AddListener(() => {
            RenamePanel.SetActive(true);
        });

        //¸ÄÃû
        GetBut(this.transform, "RenameButton").onClick.AddListener(async () => {

            if (RenameText.text != null)
            {
                string json = "{\"newNikName\": \""+ RenameText.text + "\"}";
      
                string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/userData/nikName",
                    json, DataManager.Get().getHeader());

                SettingNetData data = JsonUtil.ReadData<SettingNetData>(str);

                if (data == null)
                {
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                    MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "NetWork Erre"));
                }
                else {
                    DataManager.Get().nickName = data.nickname;
                    RenamePanel.SetActive(false);
                    RefreshAsync();
                }

            }
            else {
                UIManager.GetUIMgr().showUIForm("ErrForm");
                MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Please enter a nickname"));
            }
        });

        


        GetBut(this.transform, "Panel").onClick.AddListener(() => {
            CloseForm("SettingForm");
        });


        GetBut(this.transform, "Close").onClick.AddListener(() => {
            CloseForm("SettingForm");
        });

        GetBut(this.transform, "Logout").onClick.AddListener(() => {
            SceneManager.LoadScene("index");
            //PlayerPrefs.SetInt("logout", 1);
        });

        GetBut(this.transform, "Dele").onClick.AddListener(() => {
            SceneManager.LoadScene("index");
            //É¾³ýÕËºÅ
            PlayerPrefs.SetInt("logout", 2);
        });

        GetBut(this.transform, "Bind").onClick.AddListener(() => {
            //°ó¶¨ÕËºÅ
            registerP.Bind();
        });

        if (DataManager.Get().userData.settingData.hasTempUser)
        {
            UIFrameUtil.FindChildNode(this.transform, "Bind").gameObject.SetActive(true);
        }
        else {
            UIFrameUtil.FindChildNode(this.transform, "Bind").gameObject.SetActive(false);
        }



        GetBut(this.transform, "sound/Slider/Button").onClick.AddListener(() => {
            soundFlag = !soundFlag;
            soundSlider.value = soundFlag ? 1 : 0;

            DataManager.Get().userData.settingData.soundFlag = soundFlag;
            DataManager.Get().save();

            audioMixer.SetFloat("vSound", DataManager.Get().userData.settingData.soundFlag ? 0 : -100f);
        });

        GetBut(this.transform, "music/Slider/Button").onClick.AddListener(() => {
            musicFlag = !musicFlag;
            musicSlider.value = musicFlag ? 1 : 0;

            DataManager.Get().userData.settingData.musicFlag = musicFlag;
            DataManager.Get().save();
            audioMixer.SetFloat("vMusic", DataManager.Get().userData.settingData.musicFlag ? 0 : -100f);
        });

        GetBut(this.transform, "shock/Slider/Button").onClick.AddListener(() => {
            shockFlag = !shockFlag;
            shockSlider.value = shockFlag ? 1 : 0;

            DataManager.Get().userData.settingData.shockFlag = shockFlag;
            DataManager.Get().save();
        });
    }

    public override void Show()
    {
        base.Show();
        RefreshAsync();
    }

    public void RefreshAsync() {

        nameText.text = DataManager.Get().nickName;
        accountText.text = "UserID: "+DataManager.Get().loginData.data.user;

        if (DataManager.Get().userData.settingData.hasTempUser)
        {
            UIFrameUtil.FindChildNode(this.transform, "Bind").gameObject.SetActive(true);
        }
        else
        {
            UIFrameUtil.FindChildNode(this.transform, "Bind").gameObject.SetActive(false);
        }

        MessageMgr.SendMsg("UpMenuRefresh",
                       new MsgKV("", null));

        /*string json = "{\"newNikName\": \"testname\"}";
        string str = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/userData/nikName", 
            json, DataManager.Get().getHeader());
        Debug.Log(str);*/
    }

    
}
public class SettingNetData
{
    public string username;
    public string nickname;
}