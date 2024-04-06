using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

public class RegisterPanel : BaseUIPanel
{

    TMP_InputField account;
    TMP_InputField password_1;
    TMP_InputField password_2;

    TextMeshProUGUI desc;
    CanvasGroup group;

    public SettingForm SettingForm;

    LoginPanel loginPanel;
    Transform playBut;

    // Start is called before the first frame update：
    void Start()
    {
        account = UIFrameUtil.FindChildNode(this.transform, "account").GetComponent<TMP_InputField>();
        password_1 = UIFrameUtil.FindChildNode(this.transform, "password_1").GetComponent<TMP_InputField>();
        password_2 = UIFrameUtil.FindChildNode(this.transform, "password_2").GetComponent<TMP_InputField>();
        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        group = desc.GetComponent<CanvasGroup>();

        UIFrameUtil.FindChildNode(this.transform, "cancelBut")
            .GetComponent<Button>().onClick.AddListener(() => {
                Hide();
            });
        UIFrameUtil.FindChildNode(this.transform, "registerBut")
            .GetComponent<Button>().onClick.AddListener(() => {
                tryRegister();
            });

        if (!bindFlag) { 
            loginPanel = GameObject.Find("Canvas").transform.Find("loginPanel").GetComponent<LoginPanel>();
            playBut = GameObject.Find("Canvas").transform.Find("playBut");
        }
    }

    async void tryRegister() {
        if (account.text == null || account.text.Trim().Length <= 0)
        {
            //提示输入帐号
            desc.text = "Please enter an account";
            StartCoroutine(fade());
            return;
        }
        if (password_1.text == null || password_1.text.Trim().Length <= 0)
        {
            //提示输入密码
            desc.text = "Please enter the password";
            StartCoroutine(fade());
            return;
        }

        if (password_2.text == null || password_2.text.Trim().Length <= 0)
        {
            //提示输入密码
            desc.text = "Please confirm the password";
            StartCoroutine(fade());
            return;
        }

        if (password_1.text != password_2.text) {
            //提示两次密码不同
            desc.text = "Two password inputs is dissimilar";
            StartCoroutine(fade());
            return;
        }


        bool flag = await register(account.text, password_1.text);
        if (flag)
        {
            desc.text = "Registration is successful!";
            DOTween.Clear();
            group.alpha = 1;
            /* StartCoroutine(fade());
             await Task.Delay(1000);
             Hide();*/
        }                                                                                                           
    }

    bool bindFlag;
    public void Bind() {
        bindFlag = true;
        Show();
    }


    public async Task<bool> register(string a, string p)
    {

        string url =
            ConfigCheck.publicUrl+"/login/pub/tryLogin";

        if(bindFlag)
            url = ConfigCheck.publicUrl + "/login/pub/tryBind";

        loginPostData data = new loginPostData();
        data.platform = "ANDROID";
        data.deviceId = "test";
        data.packageName = "com.test";
        data.sign = "1233456";
#if UNITY_ANDROID
        data.platform = "ANDROID";
#elif UNITY_IOS
        data.platform = "IOS";
#endif
        data.username = a;
        data.password = p;
        if (bindFlag && DataManager.Get().userData.settingData.t != null) { 
            data.user = DataManager.Get().loginData.data.user;
            //data.token = DataManager.Get().userData.settingData.t;
        }
        string json = JsonConvert.SerializeObject(data);
        string str = null;
        if (bindFlag)
            str = await NetManager.post(url, json, DataManager.Get().getHeader());
        else
            str = await NetManager.post(url, json);
        if (str == null)
        {
            Debug.Log("eer");
            return false;
        }
        JObject obj = (JObject)JsonConvert.DeserializeObject(str);
        loginData loginData = obj.ToObject<loginData>();
        if (loginData.errorCode != null)
        {
            desc.text = "This user already registers";
            StartCoroutine(fade());
            return false;
        }

        DataManager.Get().userData.settingData.a = Encrypt.EncryptStr(a);
        DataManager.Get().userData.settingData.p = Encrypt.EncryptStr(p);
        if (bindFlag)
        {
            DataManager.Get().loginData = loginData;
            DataManager.Get().userData.username = loginData.data.user;
            DataManager.Get().nickName = loginData.data.nikName;

            DataManager.Get().nickName = loginData.data.nikName;
            DataManager.Get().userData.settingData.hasTempUser = false;
            DataManager.Get().userData.settingData.yk_a = null;
            DataManager.Get().userData.settingData.yk_p = null;
            SettingForm.RefreshAsync();
            Hide();
        }
        else {
            //尝试自动登录
            bool flag = await loginPanel.login(a, p, DataManager.Get().userData.settingData.hasTempUser);
            if (!flag)
            {
                loginPanel.Show();
            }
            else
            {
                loginPanel.Hide();
                Hide();
                GameObject.Find("Canvas/WelcomePanel").GetComponent<WelcomePanel>().show(DataManager.Get().nickName);
                playBut.gameObject.SetActive(true);
            }

        }
        DataManager.Get().save();
        DataManager.Get().userData.settingData.t = loginData.data.token;
        return true;

        /* tokenData tokenData = new tokenData();
         tokenData.user = loginData.data.user;
         tokenData.token = loginData.data.token;
         string json1 = JsonConvert.SerializeObject(tokenData);
         //string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack", json1);
         string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/growthFund/getGrowthFund", json1);
         Debug.Log("getBackPack:" + str1);
         //Debug.Log("tokenData:" + tokenData.user+"   "+ tokenData.token);
        */
    }

    IEnumerator fade()
    {
        DOTween.Clear();
        group.alpha = 1;
        yield return new WaitForSeconds(0.7f);
        group.DOFade(0, 1f);
    }
}
