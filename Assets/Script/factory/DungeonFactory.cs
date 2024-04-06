using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class DungeonFactory : ScriptableObject
{
    public string md5;
    public List<DungeonDesc> descList = new List<DungeonDesc>();
    public List<DungeonInfo> infoList = new List<DungeonInfo>();
    
    public Dictionary<string, List<DungeonInfo>> dungeonMap = new Dictionary<string, List<DungeonInfo>>();
    public Dictionary<string, DungeonDesc> dungeonDescMap = new Dictionary<string, DungeonDesc>();

    public void init()
    {
        if (dungeonMap.Count > 0)
            return;


        //if (this.md5 != ConfigCheck.webMd5 || infoList.Count<1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string skillAttrJsonUrl = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/battle/Dungeon.json";
            skillAttrJsonUrl = skillAttrJsonUrl.Replace('\\', '/');
            string skillAttrJson = ConfigCheck.ReadData(skillAttrJsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(skillAttrJson);
            infoList = obj.ToObject<List<DungeonInfo>>();


            string skillAttrJsonUrl2 = Application.persistentDataPath +
               "/" + ConfigCheck.filename + "/battle/DungeonList.json";
            skillAttrJsonUrl2 = skillAttrJsonUrl2.Replace('\\', '/');
            string skillAttrJson2 = ConfigCheck.ReadData(skillAttrJsonUrl2);
            JArray obj2 = (JArray)JsonConvert.DeserializeObject(skillAttrJson2);
            descList = obj2.ToObject<List<DungeonDesc>>();
        }



        foreach (DungeonInfo ea in infoList)
        {
            if (!dungeonMap.ContainsKey(ea.id))
                dungeonMap[ea.id] = new List<DungeonInfo>();

            dungeonMap[ea.id].Add(ea);
        }


        foreach (DungeonDesc ea in descList)
        {
            dungeonDescMap.Add(ea.id,ea);
            dungeonDescMap[ea.id].init();
        }
    }
}
