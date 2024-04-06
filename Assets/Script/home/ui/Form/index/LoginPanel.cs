using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine.Networking;
using DG.Tweening;
using System;
using AppsFlyerSDK;

class battleEnd
{
    public int gold;
    public string chapterId;
    public int storey;
    public int exp;


    //新参数
    public int killNum;
    public int killBossNum;
    public int relicNum;
}

public class LoginPanel : BaseUIPanel
{
    //TextMeshProUGUI account;
    //TextMeshProUGUI password;

    RegisterPanel registerP;

    TMP_InputField account;
    TMP_InputField password;

    TextMeshProUGUI desc;
    CanvasGroup group;


    Toggle read_toggle;
    GameObject PolicyPanel;

    bool initflag;

    // Start is called before the first frame update：
    void Start()
    {
        read_toggle = UIFrameUtil.FindChildNode(this.transform, "Toggle").GetComponent<Toggle>();
        PolicyPanel = UIFrameUtil.FindChildNode(this.transform, "PolicyPanel").gameObject;
        account = UIFrameUtil.FindChildNode(this.transform, "account").GetComponent<TMP_InputField>();
        password = UIFrameUtil.FindChildNode(this.transform, "password").GetComponent<TMP_InputField>();
        desc = UIFrameUtil.FindChildNode(this.transform, "desc").GetComponent<TextMeshProUGUI>();
        group = desc.GetComponent<CanvasGroup>();
        group.alpha = 0;
        desc.text = "please login";
        registerP = GameObject.Find("Canvas").transform.Find("registerPanel").GetComponent<RegisterPanel>();

        FBManager = GetComponent<FBManager>();

        account.text = "";
        password.text = "";
        /*UIFrameUtil.FindChildNode(this.transform, "backBut")
           .GetComponent<Button>().onClick.AddListener(() => {
               Hide();
           });*/

        GetComponent<Button>().onClick.AddListener(() => {
            PolicyPanel.SetActive(false);
            Hide();
           });
        UIFrameUtil.FindChildNode(this.transform, "Close")
          .GetComponent<Button>().onClick.AddListener(() => {
              PolicyPanel.SetActive(false);
              Hide();
          });
        UIFrameUtil.FindChildNode(this.transform, "PolicyPanelClose")
         .GetComponent<Button>().onClick.AddListener(() => {
             PolicyPanel.SetActive(false);
         });
        UIFrameUtil.FindChildNode(this.transform, "PolicyPanel")
         .GetComponent<Button>().onClick.AddListener(() => {
             PolicyPanel.SetActive(false);
         });

        UIFrameUtil.FindChildNode(this.transform, "AgreeBut")
         .GetComponent<Button>().onClick.AddListener(() => {
             read_toggle.isOn = true;
             PolicyPanel.SetActive(false);
         });
        UIFrameUtil.FindChildNode(this.transform, "Tourist mode")
        .GetComponent<Button>().onClick.AddListener(() => {
            TouristLogin();
        });


        UIFrameUtil.FindChildNode(this.transform, "FB_login")
       .GetComponent<Button>().onClick.AddListener(() => {
           FB_login();
       });



        UIFrameUtil.FindChildNode(this.transform, "loginBut")
            .GetComponent<Button>().onClick.AddListener(() => {
                if (read_toggle.isOn)
                {
                    tryLogin();
                }
                else {
                    desc.text = "Please agree to the privacy policy";
                    StartCoroutine(fade());
                }

            });
        UIFrameUtil.FindChildNode(this.transform, "registerBut")
            .GetComponent<Button>().onClick.AddListener(() => {
                registerP.Show();
            });

        UIFrameUtil.FindChildNode(this.transform, "ToggleDesc")
           .GetComponent<Button>().onClick.AddListener(() => {
               PolicyPanel.SetActive(true);
           });


        if (DataManager.Get().userData != null &&
            DataManager.Get().userData.settingData != null &&
            DataManager.Get().userData.settingData.a != null && DataManager.Get().userData.settingData.p != null)
            read_toggle.isOn = true;


        //login("heixiaoma", "123456");
        //StartCoroutine(get());
        initflag = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Show()
    {
        base.Show();

        if (!initflag)
            return;

        PolicyPanel.SetActive(false);
        group.alpha = 0;
        desc.text = "please login";
    }

    async void tryLogin() {

        if (account.text == null || account.text.Trim().Length <= 0) {
            desc.text = "Please enter your account";
            StartCoroutine(fade());
            //提示输入帐号
            return;
        }
        if (password.text == null || password.text.Trim().Length <= 0) {
            desc.text = "Please enter your password";
            StartCoroutine(fade());
            //提示输入密码
            return;
        }

        bool flag = await login(account.text, password.text);
        if (flag) {
            group.alpha = 0;
            GameObject.Find("Canvas/WelcomePanel").
                GetComponent<WelcomePanel>().show(DataManager.Get().nickName);
            Hide();
        }
    }


    public async Task<bool> login(string a,string p,bool hasTempUser=false) {

        try {
            if (desc != null) { 
                desc.text = "login...";
                group.alpha = 1;
            }

           // Debug.Log("deviceId:" + DeviceUtil.DeviceIdentifier);
           // Debug.Log("deviceModel:" + DeviceUtil.DeviceModel);

            string url =
                ConfigCheck.publicUrl+"/login/pub/tryLogin";

            loginPostData data  = new loginPostData();
            data.platform= "ANDROID";
            data.deviceId = "";
            data.deviceModel = "";
            //data.deviceId = DeviceUtil.DeviceIdentifier;
            //data.deviceModel = DeviceUtil.DeviceModel;
            data.packageName = Application.identifier;
            data.sign = "1233456";
            data.hasTempUser = hasTempUser;
    #if UNITY_ANDROID
            data.platform = "ANDROID";
            //data.deviceId = AndroidInterface.GetInstance.GetAndroidPhoneUnid();
            //data.sign = SignatureVerify.getAndroidSigna();
    #elif UNITY_IOS
            data.platform = "IOS";
            //data.deviceId = AndroidInterface.GetInstance.GetAndroidPhoneUnid();
            //data.sign = SignatureVerify.getAndroidSigna();
    #endif
            data.username = a;
            data.password = p;
            string json = JsonConvert.SerializeObject(data);
            string str = await NetManager.post(url, json);
            if (str == null)
            {
                if (desc != null)
                    desc.text = "Error";
                Debug.Log("eer");
                return false;
            }
      
            
            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            loginData loginData = obj.ToObject<loginData>();
            if (loginData.errorCode != null) {
                Debug.Log("loginData.errorCode :"+loginData.errorCode);
                desc.text =  "invalid password";
                StartCoroutine(fade());
                return false;
            }


            Debug.Log("aa:" + str);
            Debug.Log("loginData.data.token:" + loginData.data.token);

            DataManager.Get().loginData = loginData;
            DataManager.Get().userData.settingData.a = Encrypt.EncryptStr(a);
            DataManager.Get().userData.settingData.p = Encrypt.EncryptStr(p);
            DataManager.Get().userData.settingData.hasTempUser = hasTempUser;
            DataManager.Get().userData.username = loginData.data.user;
            DataManager.Get().nickName = loginData.data.nikName;
            if (hasTempUser) {
                //记录游客账号
                DataManager.Get().userData.settingData.yk_a = Encrypt.EncryptStr(a);
                DataManager.Get().userData.settingData.yk_p = Encrypt.EncryptStr(p);
            }
            DataManager.Get().save();
            DataManager.Get().userData.settingData.t = loginData.data.token;

            GameObject.Find("Canvas").transform.Find("playBut").gameObject.SetActive(true);


            AppsFlyer.setCustomerUserId(data.username);
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(AFInAppEvents.CURRENCY, "USD");
            AppsFlyer.sendEvent("af_login", null);
        }
        catch (Exception ex)
        {
            if (desc != null)
                desc.text = "Error: " + ex;
        }
        /*
       Dictionary<string, string> dic = new Dictionary<string, string>();
       dic.Add("user", loginData.data.user);

       //string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack", dic);
       //string str1 = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/growthFund/getGrowthFund", dic);
       //Debug.Log("getBackPack:" + str1);

       Debug.Log("=========================");
       lsdata id = new lsdata();
       id.user = "1";
       id.mainFuse = new itData();
       id.mainFuse.id = "wp_001";
       id.mainFuse.quality = 1;
       id.mainFuse.level = 3;
       id.mainFuse.seqId = "wp_001_3_1";
       string json2 = JsonConvert.SerializeObject(id);
       string str2 = await NetManager.post(ConfigCheck.publicUrl+"/data/pub/weapons/upgrades", json2);
       Debug.Log("item:"+ str2);
      */
        /*
        string PackageName = Application.identifier;  //包名
        string APPversion = Application.version;     //APK版本号
        string ProductName = Application.productName;   //产品名
        string CompanyName = Application.companyName;   //公司名称

        Debug.Log("PackageName:" + PackageName);
        Debug.Log("ProductName:" + ProductName); */
        return true;
    }


    //token登录  谷歌 fb等
    public async Task<bool> login_token(string userid , string token)
    {

        try
        {
            if (desc != null)
            {
                desc.text = "login...";
                group.alpha = 1;
            }

            // Debug.Log("deviceId:" + DeviceUtil.DeviceIdentifier);
            // Debug.Log("deviceModel:" + DeviceUtil.DeviceModel);

            string url =
                ConfigCheck.publicUrl + "/login/pub/tryLogin";

            loginPostData data = new loginPostData();
            data.platform = "ANDROID";
            data.deviceId = "";
            data.deviceModel = "";
            //data.deviceId = DeviceUtil.DeviceIdentifier;
            //data.deviceModel = DeviceUtil.DeviceModel;
            data.packageName = Application.identifier;
            data.sign = "1233456";
#if UNITY_ANDROID
            data.platform = "ANDROID";
            //data.deviceId = AndroidInterface.GetInstance.GetAndroidPhoneUnid();
            //data.sign = SignatureVerify.getAndroidSigna();
#elif UNITY_IOS
            data.platform = "IOS";
            //data.deviceId = AndroidInterface.GetInstance.GetAndroidPhoneUnid();
            //data.sign = SignatureVerify.getAndroidSigna();
#endif
            //data.username = a;
            //data.password = p;
            data.idToken = token;
            string json = JsonConvert.SerializeObject(data);
            string str = await NetManager.post(url, json);
            if (str == null)
            {
                if (desc != null)
                    desc.text = "Error";
                Debug.Log("eer");
                return false;
            }


            JObject obj = (JObject)JsonConvert.DeserializeObject(str);
            loginData loginData = obj.ToObject<loginData>();
            if (loginData.errorCode != null)
            {
                Debug.Log("loginData.errorCode :" + loginData.errorCode);
                desc.text = "invalid password";
                StartCoroutine(fade());
                return false;
            }


            Debug.Log("aa:" + str);
            Debug.Log("loginData.data.token:" + loginData.data.token);

            DataManager.Get().loginData = loginData;
            DataManager.Get().userData.settingData.a = null;
            DataManager.Get().userData.settingData.p = null;
            //DataManager.Get().userData.settingData.a = Encrypt.EncryptStr(a);
            //DataManager.Get().userData.settingData.p = Encrypt.EncryptStr(p);
            //DataManager.Get().userData.settingData.hasTempUser = false;
            DataManager.Get().userData.username = loginData.data.user;
            DataManager.Get().nickName = loginData.data.nikName;
           
            DataManager.Get().save();
            DataManager.Get().userData.settingData.t = loginData.data.token;

            GameObject.Find("Canvas").transform.Find("playBut").gameObject.SetActive(true);


            AppsFlyer.setCustomerUserId(data.username);
            Dictionary<string, string> eventValues = new Dictionary<string, string>();
            eventValues.Add(AFInAppEvents.CURRENCY, "USD");
            AppsFlyer.sendEvent("af_login", null);


            Hide();
            GameObject.Find("Canvas/WelcomePanel").GetComponent<WelcomePanel>().show(userid);


        }
        catch (Exception ex)
        {
            if (desc != null)
                desc.text = "Error: " + ex;
        }
        return true;
    }

    async void TouristLogin() {
        string a = null;
        string p = null;
        //检查是否有游客账号密码
        if (DataManager.Get().userData != null &&
            DataManager.Get().userData.settingData != null &&
            DataManager.Get().userData.settingData.yk_a != null && DataManager.Get().userData.settingData.yk_p != null)
        {
            a = Encrypt.DecryptStr(DataManager.Get().userData.settingData.yk_a);
            p = Encrypt.DecryptStr(DataManager.Get().userData.settingData.yk_p);


            print(a + "---" + p + "===") ;

        }
        else { 
            a= "yk_" + SnowFlake.NewId();
            p = "123456";
        }

        bool flag = await login(a, p, true);

        if (flag)
        {
            group.alpha = 0;
            GameObject.Find("Canvas/WelcomePanel").
                //GetComponent<WelcomePanel>().show(DataManager.Get().userData.username);
            GetComponent<WelcomePanel>().show(DataManager.Get().nickName);
           
            Hide();
        }
    }

    void FB_login() {
        FBManager.fbLogin();
    }

    FBManager FBManager;


    /// <summary>
    /// GET请求数据
    /// </summary>
    /// <param name="url">请求数据的URL地址</param>
    /// <param name="token">token验证的参数，此处为authorization</param>
    /// <returns></returns>
    IEnumerator GetUrl()
    {
        Debug.Log("we");
        string url = ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack";
        //string token = "1";
        using UnityWebRequest webRequest = new UnityWebRequest(url, "GET");
        //byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(token);
        //webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("user", "1");//请求头文件内容

        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("zzz:" + webRequest.downloadHandler.text);
        }
    }


    IEnumerator get() {

        string md5Url = ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack?user=1";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(md5Url))
        {
            //webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            webRequest.SetRequestHeader("user", "1");


            yield return webRequest.SendWebRequest();
            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "" + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("获取====:" + webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator fade()
    {
        DOTween.Clear();
        group.alpha = 1;
        yield return new WaitForSeconds(1.5f);
        group.DOFade(0, 1f);
    }

    /*
     aa:{"errorCode":null,"message":null,"data":{"user":1,"token":"03a45ce493b36c814a52cb11d942b8c7",
    "userBackPack":{"version":"261031926","versionTime":1688556347538,"_id":1,
        "backPackItems":[{"id":"p10000","name":"钻石","quantity":1300},{"id":"p10003","name":"金钥匙","quantity":9},{"id":"p10001","name":"金币","quantity":20000},{"id":"m10000","name":"随机图纸","quantity":4}],
        "weaponsBackPackItems":[{"id":"wp_001","name":"Katana","mainAtr":"attack","weaponQuality":0,"quantity":1,"wearing":false}],
    "lastModify":"2023-06-25T09:33:00.085+00:00"},"socketIO":"ws://localhost:10240"}}

    aa:{"timestamp":"2023-07-05T11:08:04.654+00:00","status":400,"error":"Bad Request","path":"/pub/tryLogin"}

     */
}



public class loginPostData
{
    public string platform;
    public string deviceId;
    public string deviceModel;
    public string packageName;
    public string sign;
    public string username;
    public string password;
    public bool hasTempUser;
    public string token;
    public string user;

    public string idToken;
}

public class loginData {
    public string errorCode;
    public string message;
    public userData data;
}

public class userData {
    public string user;
    public string token;
    public string nikName;
}


/*public class AndroidInterface
{
    public static AndroidInterface GetInstance
    {
        get
        {
            return new AndroidInterface();
        }
    }

    public AndroidJavaObject currentActivity
    {
        get
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }
    }


    public string GetAndroidPhoneUnid()
    {
        string ID = "";
        AndroidJavaClass javaClass = new AndroidJavaClass("com.example.getuuid.GetAndroidphoneId");
        ID = javaClass.CallStatic<string>("GetSerial", currentActivity);
        return ID;
    }
}*/

/*public class SignatureVerify {

    public static string getAndroidSigna() {
#if UNITY_EDITOR
         return "test";
#endif
// 获取Android的PackageManager
        AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

        // 获取当前Android应用的包名
        string packageName = Activity.Call<string>("getPackageName");

        // 调用PackageManager的getPackageInfo方法来获取签名信息数组
        int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
        AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
        AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");
        int hashCode = Signatures[0].Call<int>("hashCode");

        byte[] bytes =  Signatures[0].Call<byte[]>("toByteArray");

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}*/

 
//获取设备标识符
/*public class DeviceUtil
{
    //获取设备标识符
    public static string DeviceIdentifier
    {
        get
        {
            return SystemInfo.deviceUniqueIdentifier;
        }
    }

    //获取设备型号
    public static string DeviceModel
    {
        get
        {
#if !UNITY_EDITOR && UNITY_IPHONE
        return UnityEngine.iOS.Device.generation.ToString();
#else
            return SystemInfo.deviceModel;
#endif
        }
    }

}*/