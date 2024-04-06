using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(menuName = "data/sysSettingFactory", fileName = "sysSettingMode")]
public class SysSettingFactory : ScriptableObject
{
    public string md5;

    public List<SysSetting> list = new List<SysSetting>();


    public void init()
    {
        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/battle/SysSetting.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            list = obj.ToObject<List<SysSetting>>();
        }
    }
}

[System.Serializable]
public struct SysSetting {
    public string id;
    public string dmgUiThreshold;
}
