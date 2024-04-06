using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public interface IConfigMgr
{
    public Dictionary<string, string> AppSetting { get; }

    int getAppSetting();
}

[System.Serializable]
internal class KeyValueInfo
{
    public List<KeyValueNode> ConfigInfo = null;

    void CS()
    {

    }
}

[System.Serializable]
internal class KeyValueNode
{
    public string key;
    public string value;
}
