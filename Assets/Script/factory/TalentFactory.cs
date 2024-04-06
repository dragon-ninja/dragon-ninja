using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(menuName = "data/TalentFactory", fileName = "TalentMode")]
public class TalentFactory : ScriptableObject
{
    static TalentFactory myFactory;
    public string md5;

    public List<Talent> list = new List<Talent>();


    public List<Talent> talentList = new List<Talent>();
    public List<Talent> superTalentList = new List<Talent>();

    public Dictionary<string,Talent> talentMap = new Dictionary<string, Talent>();
    public Dictionary<string, Talent> superTalentMap = new Dictionary<string, Talent>();

    public void init()
    {
        if (talentMap.Count > 0)
            return;


        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/common/Talent.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            list = obj.ToObject<List<Talent>>();
        }

        foreach (Talent item in list)
        {
            if (item.id == null)
                continue;

            if (item.talentType.IndexOf("super") != -1) {
                superTalentMap.Add(item.id, item);
                superTalentList.Add(item);
            }
            else { 
                talentMap.Add(item.id, item);
                talentList.Add(item);
            }
        }
    }


    public static TalentFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<TalentFactory>("mode/TalentMode");
            myFactory.init();
        }

        return myFactory;
    }
}

