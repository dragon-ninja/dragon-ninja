using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[CreateAssetMenu(menuName = "data/obstacleFactory", fileName = "obstacleMode")]
public class ObstacleFactory : ScriptableObject
{
    public string md5;

    public List<ObstacleAttr> list = new List<ObstacleAttr>();

    public static Dictionary<string, ObstacleAttr> atrMap = new Dictionary<string, ObstacleAttr>();

    public void init()
    {
        if (atrMap.Count > 0)
            return;

        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/battle/ObstacleAttr.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            list = obj.ToObject<List<ObstacleAttr>>();
        }


        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].id == null)
                continue;

            ObstacleAttr p = list[i];
            atrMap.Add(p.id, p);
        }
    }
}

[System.Serializable]
public struct ObstacleAttr
{
    public string id;
    public int minNum;
    public int maxNum;
    public int dmg;
    public float rate;
    public float time;
    public float speed;
    public float size;
    public float interval;
}
