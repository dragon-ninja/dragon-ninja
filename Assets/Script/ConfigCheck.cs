using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.Text;
using TMPro;
using GoogleMobileAds.Api;

public class ConfigCheck : MonoBehaviour
{

    static string url_new = "http://dev-danke-api.aleelee.net";
    static string url_old = "http://dev_danke_api.nowsharegame.com";
    static string url_new2 = "http://danke-api.g.aleelee.net";

    public static string publicUrl = url_new2;

    public static string filename = "configData";
    string url = ConfigCheck.publicUrl + "/data/pub/excelData/zip";
    public static Text txt;
    public Slider slider;
    public static bool configChangeFlag = false;
    public static string webMd5 = null;
    public Transform playBut;

    public bool initConfigflag;
    LoginPanel loginPanel;


    bool logoutFlag;


    public void Start()
    {

        Debug.Log(Application.identifier);
        GameObject.Find("Canvas/version").GetComponent<TextMeshProUGUI>().text
            = "v:" + Application.version;


        Application.targetFrameRate = 120;
        filename = "configData";
        url = ConfigCheck.publicUrl + "/data/pub/excelData/zip?num=" + UnityEngine.Random.value;

        txt = GameObject.Find("Canvas/Slider/Text (Legacy)").GetComponent<Text>();
        StartCoroutine(Wait_LoadDown(filename, url));
        playBut = GameObject.Find("Canvas").transform.Find("playBut");
        loginPanel = GameObject.Find("Canvas").transform.Find("loginPanel").GetComponent<LoginPanel>();


        //家庭政策 儿童投放广告
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            TagForUnderAgeOfConsent = TagForUnderAgeOfConsent.True
            //TagForChildDirectedTreatment = TagForChildDirectedTreatment.True
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(initStatus => { });
        DontDestroyOnLoad(this.gameObject);


        int flag = PlayerPrefs.GetInt("logout");
        if (flag == 0) {
            logoutFlag = false;
        }
        else if (flag == 1) {
            logout();
            PlayerPrefs.SetInt("logout", 0);
        }
        else if (flag == 2)
        {
            logout();
            WWW www = new WWW("https://papercut.aleelee.net/remove.html");
            Application.OpenURL(www.url);
            PlayerPrefs.SetInt("logout", 0);
        }
    }

    private async void Update()
    {
        if (initConfigflag) {

            initConfigflag = false;

            Debug.Log(DataManager.Get().userData);

            //校验是否有玩家账号
            if (!logoutFlag &&
                DataManager.Get().userData != null &&
                DataManager.Get().userData.settingData != null &&
                DataManager.Get().userData.settingData.a != null && DataManager.Get().userData.settingData.p != null)
            {
               
                string a = Encrypt.DecryptStr(DataManager.Get().userData.settingData.a);
                string p = Encrypt.DecryptStr(DataManager.Get().userData.settingData.p);

                Debug.Log("AAAAA--------------------"+a);

                //尝试自动登录
                bool flag = await loginPanel.login(a, p , DataManager.Get().userData.settingData.hasTempUser);
                if (!flag)
                {
                    loginPanel.Show();
                }
                else {
                    GameObject.Find("Canvas/WelcomePanel").GetComponent<WelcomePanel>().show(DataManager.Get().nickName);
                    playBut.gameObject.SetActive(true);
                }

            }
            else {
                loginPanel.Show();
            }

            //playBut.gameObject.SetActive(true);
            slider.value = 1f;
            txt.text = "Loading succeeded";
        }
    }


    public static string ReadData(String url)
    {
        string readData;
        //读取文件
        using (StreamReader sr = File.OpenText(url))
        {
            //数据保存
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return readData;
    }



    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="ZipID" ZipID的名字，用于存储解压出的每一个Zip文件></param>
    /// <param name="url" Zip下载地址></param>
    /// <returns></returns>
    IEnumerator Wait_LoadDown(string fileName, string url)
    {
        //校验md5
        string md5fileUrl = Application.persistentDataPath +
          "/" + filename + "/ConfigMd5.txt";

        Debug.Log(md5fileUrl);

        md5fileUrl = md5fileUrl.Replace('\\', '/');

        string localMd5 = null;

        if (File.Exists(md5fileUrl))
        {
            localMd5 = ReadData(md5fileUrl);
            Debug.Log("本机md5:" + localMd5);
        }

        //获取网络md5
        {
            string md5Url = ConfigCheck.publicUrl + "/data/pub/excelData/zip/md5?num=" + UnityEngine.Random.value;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(md5Url))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.LogError(webRequest.error + "" + webRequest.downloadHandler.text);
                }
                else
                {
                    webMd5 = webRequest.downloadHandler.text;
                    Debug.Log("获取md5:" + webMd5);
                }
            }
        }
        if (webMd5 == null) {
            txt.text = "Resource loading failed,server exception";
            yield break;
        }



        if (localMd5 == webMd5) {
            Debug.Log("配置未发生改变");
            txt.text = "Resource loading completed";
            ConfigCheck.configChangeFlag = false;
            slider.value = 1f;
            //yield return new WaitForSeconds(0.1f);
            //playBut.gameObject.SetActive(true);
            //slider.gameObject.SetActive(false);
            initConfigflag = true;
            yield break;
        }

        ConfigCheck.configChangeFlag = true;


        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            txt.text = "Download error" + request.error;
            print("当前的下载发生错误" + request.error);
            yield break;
        }
        while (!request.isDone)
        {
            print("当前的下载进度为：" + request.downloadProgress);
            slider.value = request.downloadProgress * 0.5f;
            yield return 0;
        }
        if (request.isDone)
        {
            slider.value = 0.75f;

            Debug.Log("下载成功");
            yield return new WaitForEndOfFrame();
            txt.text = "Download Resources...";

            //直接使用 将byte转换为Stream，省去先保存到本地在解压的过程
            ConfigCheck.SaveZip(fileName, request.downloadHandler.data, null);

            yield return new WaitForEndOfFrame();
            slider.value = 0.9f;

            //写入md5
            File.WriteAllText(md5fileUrl, webMd5);

            //slider.value = 1f;
            //txt.text = "Loading succeeded";

            initConfigflag = true;
        }

    }

    public void Play() {
        //restPlayData();
        SceneManager.LoadScene("home");
    }

    public void logout(){
        logoutFlag = true;
        playBut.gameObject.SetActive(false);
        loginPanel.Show();
}


    public IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            yield return webRequest.SendWebRequest();

            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.LogError(webRequest.error + "" + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }


    public static bool SaveZip(string ZipID, byte[] ZipByte, string password)
    {
        bool result = true;
        FileStream fs = null;
        ZipInputStream zipStream = null;
        ZipEntry ent = null;
        string fileName;

        ZipID = Application.persistentDataPath + "/" + ZipID;
        Debug.Log("ZipID" + ZipID);
        Debug.Log(ZipID);

        if (!Directory.Exists(ZipID))
        {
            Directory.CreateDirectory(ZipID);
        }
        try
        {
            //直接使用 将byte转换为Stream，省去先保存到本地在解压的过程
            Stream stream = new MemoryStream(ZipByte);
            zipStream = new ZipInputStream(stream);

            if (!string.IsNullOrEmpty(password))
            {
                zipStream.Password = password;
            }
            while ((ent = zipStream.GetNextEntry()) != null)
            {
                if (!string.IsNullOrEmpty(ent.Name))
                {
                    fileName = Path.Combine(ZipID, ent.Name);

                    #region      Android
                    fileName = fileName.Replace('\\', '/');
                    Debug.Log(fileName);

                    string dir = fileName.Substring(0, fileName.LastIndexOf("/"));
                    //目录*
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    if (fileName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(fileName);
                        continue;

                    }
                    #endregion
                    fs = File.Create(fileName);

                    int size = 2048;
                    byte[] data = new byte[size];
          
                    while (true)
                    {
                        size = zipStream.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            //fs.Write(data, 0, data.Length);
                            Debug.Log(data.Length);
                            fs.Write(data, 0, size);//解决读取不完整情况
                        }
                        else
                            break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());

            txt.text = "error:" + e.ToString();

            result = false;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
                fs.Dispose();
            }
            if (zipStream != null)
            {
                zipStream.Close();
                zipStream.Dispose();
            }
            if (ent != null)
            {
                ent = null;
            }
            GC.Collect();
            GC.Collect(1);
        }
        return result;
    }


    void restPlayData() {
        DataManager.Get().read();
        DataManager.Get().userData.level = 1;
        DataManager.Get().userData.gold = 3000;
        DataManager.Get().userData.talentList = null;
        //DataManager.Get().userData.towerData = null;


        DataManager.Get().userData.equipmentDataList = new List<EquipmentData>();
        EquipmentData wp1 = new EquipmentData("wp_001", 1, 2);
        //wp1.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(wp1);
        EquipmentData item2 = new EquipmentData("ri_001", 1, 2);
        item2.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(item2);
        EquipmentData item3 = new EquipmentData("be_001", 1, 1);
        item3.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(item3);
        EquipmentData item4 = new EquipmentData("he_001", 1, 4);
        item4.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(item4);
        EquipmentData item5 = new EquipmentData("br_001", 1, 4);
        item5.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(item5);
        EquipmentData item6 = new EquipmentData("sh_001", 1, 5);
        item6.wearing = true;
        DataManager.Get().userData.equipmentDataList
            .Add(item6);

        DataManager.Get().userData.equipmentDataList
           .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.Get().userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
/*        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 4));*/

        DataManager.Get().userData.equipmentDataList
           .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("wp_001", 1, 4));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("wp_001", 1, 5));
        DataManager.Get().userData.equipmentDataList
          .Add(new EquipmentData("wp_001", 1, 8));
        DataManager.Get().userData.equipmentDataList
           .Add(new EquipmentData("wp_001", 1, 10));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("ri_002", 1, 3));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("ri_001", 1, 2));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("be_001", 1, 2));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("he_001", 1, 3));
        DataManager.Get().userData.equipmentDataList
           .Add(new EquipmentData("he_001", 1, 3));
        DataManager.Get().userData.equipmentDataList
           .Add(new EquipmentData("he_001", 1, 3));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("br_001", 1, 3));
        DataManager.Get().userData.equipmentDataList
        .Add(new EquipmentData("br_001", 1, 3));
            DataManager.Get().userData.equipmentDataList
        .Add(new EquipmentData("br_001", 1, 3));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("sh_001", 1, 2));
        DataManager.Get().userData.equipmentDataList
            .Add(new EquipmentData("sh_001", 1, 1));
        DataManager.Get().userData.equipmentDataList
         .Add(new EquipmentData("sh_001", 1, 1));
        DataManager.Get().userData.equipmentDataList
         .Add(new EquipmentData("sh_001", 1, 1));
        DataManager.Get().save();

    }

}
