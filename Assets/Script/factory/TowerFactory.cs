using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



[CreateAssetMenu(menuName = "data/TowerFactory", fileName = "towerMode")]
public class TowerFactory : ScriptableObject
{
    public string md5;
    public List<TowerMap> tmList = new List<TowerMap>();
    //public List<DungeonInfo> tdList = new List<DungeonInfo>();
    public List<DungeonInfoConfig> tdList = new List<DungeonInfoConfig>();

    public List<TowerEventConfig> eventList = new List<TowerEventConfig>();
    public List<TowerEventBuffConfig> eventBuffList = new List<TowerEventBuffConfig>();
    public List<RelicConfig> relicList = new List<RelicConfig>();




    //list中0是父节点信息  其他是子节点信息
    public Dictionary<string, List<TowerMap>> tmMap = new Dictionary<string, List<TowerMap>>();
    //public Dictionary<string, List<DungeonInfo>> tdMap = new Dictionary<string, List<DungeonInfo>>();
    public Dictionary<string, List<DungeonInfoConfig>> tdMap = new Dictionary<string, List<DungeonInfoConfig>>();
    public List<string> chapterList = new List<string>();


    public Dictionary<string, TowerEvent> eventMap = new Dictionary<string, TowerEvent>();
    public Dictionary<string, RelicConfig> relicMap = new Dictionary<string, RelicConfig>();

    static TowerFactory myFactory;

    public static TowerFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<TowerFactory>("mode/towerMode");
            myFactory.init();
        }

        return myFactory;
    }

    public void init()
    {
        //if (this.md5 != ConfigCheck.webMd5 || infoList.Count<1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;

            tdList = getJson("/battle/TowerDungeon.json").ToObject<List<DungeonInfoConfig>>();

            tmList = getJson("/battle/TowerMap.json").ToObject<List<TowerMap>>();

            eventList = getJson("/battle/TowerEvent.json").ToObject<List<TowerEventConfig>>();

            relicList = getJson("/battle/Relic.json").ToObject<List<RelicConfig>>();

            eventBuffList = getJson("/battle/TowerEventBuff.json").
                ToObject<List<TowerEventBuffConfig>>();
        }

        foreach (RelicConfig ea in relicList)
        {
            relicMap[ea.id] =  ea;
        }
        foreach (TowerEventConfig ea in eventList)
        {
            TowerEvent te = new TowerEvent();
            te.config = ea;
            te.init();
            eventMap[ea.id] = te;
        }



        foreach (DungeonInfoConfig ea in tdList)
        {
            if (!tdMap.ContainsKey(ea.id))
                tdMap[ea.id] = new List<DungeonInfoConfig>();

            tdMap[ea.id].Add(ea);
        }


        foreach (TowerMap ea in tmList)
        {
            if (ea.fatherId == null)
            {
                if (!tmMap.ContainsKey(ea.id))
                    tmMap[ea.id] = new List<TowerMap>();

                tmMap[ea.id].Add(ea);
                chapterList.Add(ea.id);
            }
            else {
                tmMap[ea.fatherId].Add(ea);
            }
        }
    }


    public JArray getJson(string path)
    {
        string JsonUrl = Application.persistentDataPath +
            "/" + ConfigCheck.filename + path;
        JsonUrl = JsonUrl.Replace('\\', '/');
        string Json = ConfigCheck.ReadData(JsonUrl);
        JArray obj = (JArray)JsonConvert.DeserializeObject(Json);
        return obj;
    }
}
