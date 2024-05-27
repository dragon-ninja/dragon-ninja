using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class DownloadDataHandle
{
    public int code;
    public string msg;
    public object data;
}
public class MNull
{
}
public static class HttpPlus
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task)tcs.Task).GetAwaiter();
    }
}

public class NetManager
{

    public static async Task<string> get(string url, Dictionary<string, string> headerMap = null)
    {
        string data = null;
        if (CheckNetworkAccessibility() == "NotNet")
        {
            //没有网络 通知弹窗
            return data;
        }

        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        www.downloadHandler = downloadHandler;
        www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        if(headerMap != null && headerMap.Count>0)
            foreach (var item in headerMap)
            {
                www.SetRequestHeader(item.Key, item.Value);
            }
        //UnityWebRequest www2 = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        await www.SendWebRequest();
        //if (www.result == UnityWebRequest.Result.Success)
        if (string.IsNullOrWhiteSpace(www.error)) {
            Debug.Log("--------------------------------");
            Debug.Log("data:" + www.downloadHandler.text);
            data = www.downloadHandler.text;
        }
        else
            Debug.Log("err:" + www.error);

        return data;
    }

    public static async Task<string> post(string url, string bodyJson, Dictionary<string, string> headerMap = null)
    {
        string data = null;

        if (CheckNetworkAccessibility() == "NotNet")
            return data;

        UnityWebRequest www = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        www.downloadHandler = downloadHandler;
        www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJson);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);

        if (headerMap != null)
            foreach (var item in headerMap)
            {
                www.SetRequestHeader(item.Key, item.Value);
            }

        await www.SendWebRequest();
        //if (www.result == UnityWebRequest.Result.Success)
        if (string.IsNullOrWhiteSpace(www.error))
            data = www.downloadHandler.text;
        else
            Debug.Log("err:" + www.error);

        return data;
    }


    private static string CheckNetworkAccessibility()
    {
        string type = null;
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                //Debug.Log("网络不可用");
                type = "NotNet";
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                //Debug.Log("通过移动数据网络连接");
                break;
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                //Debug.Log("通过WiFi或有线网络连接");
                break;
        }

        return type;
    }

    public Dictionary<string, string> getHeader()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        //dic.Add("user", DataManager.Get().loginData.data.user);
        return dic;
    }
}