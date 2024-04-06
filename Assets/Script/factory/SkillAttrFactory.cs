using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class SkillAttrFactory : ScriptableObject
{
    static SkillAttrFactory myFactory;
    //public List<SkillAttr> skillAttrList = new List<SkillAttr>();
    public string md5;

    public List<SkillAttr> skillList = new List<SkillAttr>();


    public List<SkillAttr> KunaiList = new List<SkillAttr>();
    public List<SkillAttr> NukeList = new List<SkillAttr>();
    public List<SkillAttr> FirebombList = new List<SkillAttr>();
    public List<SkillAttr> RouletteList = new List<SkillAttr>();
    public List<SkillAttr> FieldList = new List<SkillAttr>();
    public List<SkillAttr> LightningList = new List<SkillAttr>();
    public List<SkillAttr> buffList = new List<SkillAttr>();

    public List<SkillAttr> NukeExList = new List<SkillAttr>();
    public List<SkillAttr> FirebombExList = new List<SkillAttr>();

    public List<SkillAttr> KunaiVTList = new List<SkillAttr>();
    public List<SkillAttr> BlackHoleList = new List<SkillAttr>();

    public List<SkillAttr> KatanaList = new List<SkillAttr>();
    public List<SkillAttr> KatanaVTList = new List<SkillAttr>();
    

    public Dictionary<string, List<SkillAttr>> skillMap = new Dictionary<string, List<SkillAttr>>();
    public Dictionary<string, SkillAttr> bossSkillMap = new Dictionary<string, SkillAttr>();

    public List<SkillWeight> skillWeightList = new List<SkillWeight>();

    public Dictionary<int, WeightInfoForLevel> skillWeightMap = new Dictionary<int, WeightInfoForLevel>();



    public static SkillAttrFactory Get() {
        if (myFactory == null)
        {
            myFactory = Resources.Load<SkillAttrFactory>("mode/skillAttrMode");
            myFactory.init();
        }

        return myFactory;
    } 


    public void init() {
        /*if (skillMap.Count>0)
            return;*/

        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string skillAttrJsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/battle/SkillAttr.json";
            skillAttrJsonUrl = skillAttrJsonUrl.Replace('\\', '/');
            string skillAttrJson = ConfigCheck.ReadData(skillAttrJsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(skillAttrJson);
            skillList = obj.ToObject<List<SkillAttr>>();
        }


        foreach (SkillAttr sa in skillList)
        {
            if (sa.id == null)
                continue;

            //json
            if (sa.exclusive != null && sa.exclusive.IndexOf("{") != -1) {
                JObject t = (JObject)JsonConvert.DeserializeObject(sa.exclusive);
                Dictionary<string, object> t1 = t.ToObject<Dictionary<string, object>>();

                sa.exclusiveValue = new Dictionary<string, object>();
                foreach (var item in t1)
                {
                    sa.exclusiveValue.Add(item.Key, item.Value);
                }
            }

            if (sa.belong == "boss")
            {
                bossSkillMap.Add(sa.id, sa);
            }
            else { 
                List<SkillAttr> list = null;
                if (!skillMap.ContainsKey(sa.skillType) || skillMap[sa.skillType] == null)
                {
                    list = new List<SkillAttr>();
                    skillMap.Add(sa.skillType, list);
                    skillMap[sa.skillType].Add(sa);
                }
                else
                {
                    list = skillMap[sa.skillType];
                    skillMap[sa.skillType].Add(sa);
                }
            }
        }

        initWeight();
    }

    public void initWeight() {
        string jsonUrl = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/battle/SkillWeight.json";
        jsonUrl = jsonUrl.Replace('\\', '/');
        string json = ConfigCheck.ReadData(jsonUrl);
        JArray obj = (JArray)JsonConvert.DeserializeObject(json);
        skillWeightList = obj.ToObject<List<SkillWeight>>();

        //skillWeightMap = new Dictionary<int, string>();
        foreach (SkillWeight s in skillWeightList) {
            if (!skillWeightMap.ContainsKey(s.id) && s.weight != null) 
            {
                //解析权重
                string weight = s.weight;
                string[] strs = weight.Split(';');
                List<WeightInfo> infoList = new List<WeightInfo>();


                int maxNum = 0;
                WeightInfoForLevel weightInfoForLevel = new WeightInfoForLevel();
                foreach (string str in strs)
                {
                    if (str.IndexOf("|")==-1) 
                        continue;
                    
                    string[] weightStr = str.Split('|');
    
                    int i = int.Parse(weightStr[1]);
                    maxNum += i;

                    WeightInfo wi = new WeightInfo();
                    wi.skillType = weightStr[0];
                    wi.weight = i;
                    infoList.Add(wi);
                }
                weightInfoForLevel.max = maxNum;
                weightInfoForLevel.skillWeightInfo = infoList;
                skillWeightMap.Add(s.id, weightInfoForLevel);
            }
        }
    }

    public void 权重计算() { 
        
    }
}

[System.Serializable]
public struct SkillWeight {
    public int id;
    public string weight;
}

//单个技能的权重信息
public struct WeightInfo
{
    public string skillType;
    public int weight;
}

//某一等级的权重信息
public class WeightInfoForLevel {
    public int max;
    public List<WeightInfo> skillWeightInfo; 
}


