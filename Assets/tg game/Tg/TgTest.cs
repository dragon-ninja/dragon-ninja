using Boomlagoon.JSON;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TgTest : MonoBehaviour
{
   /* [DllImport("__Internal")]
    private static extern void Test();
    [DllImport("__Internal")]
    private static extern void TestArgument(string message);
    [DllImport("__Internal")]
    private static extern void TestGetCookie();


    [DllImport("__Internal")]
    private static extern void GetUserInfo();*/


    public Text gg;

 

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            TestArgument("测试Unity调用JS传参方法");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TestGetCookie();
            */
    }

    public void OnCookie_Callback(string cookie)
    {
        gg.text = "收到js参数:" + cookie;
        Debug.Log("收到js参数:" + cookie);
    }

   /* public void saveUserId(string id) {
        DataManager.userId = id;
    }*/
}

/*
 * 
 <script src="https://telegram.org/js/telegram-web-app.js"></script>

function loadFunction() {
            window.Telegram.WebApp.ready();
             window.Telegram.WebApp.expand();
        }
function GetUserId()
{
    alert(JSON.stringify(window.Telegram.WebApp.initDataUnsafe.user.id));
    gameInstance.SendMessage("StartScene", "saveUserId", JSON.stringify(window.Telegram.WebApp.initDataUnsafe.user.id));
};
function OpenUrl(url)
{
    window.Telegram.WebApp.openTelegramLink(url);
}
function GetfromId()
{
    gameInstance.SendMessage("StartScene", "saveFromId", JSON.stringify(window.Telegram.WebApp.initDataUnsafe.start_param));
};

 */
