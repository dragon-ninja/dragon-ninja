using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Boomlagoon.JSON;

public class JsonUtil
{
    public static T ReadData<T>(string str)
    {
         JObject obj = (JObject)JsonConvert.DeserializeObject(str);
         NetData NetData = obj.ToObject<NetData>();
         if (NetData.data == null)
             return default(T);

         JObject obj1 = (JObject)JsonConvert.DeserializeObject(NetData.data.ToString());

         try
         {
             return obj1.ToObject<T>();
         }
         catch (Exception ex)
         {
             Debug.LogWarning(ex);
             return default(T);
         }

    }

}


public class NetData
{
    public string msg;
    public string code;
    public System.Object data;
}