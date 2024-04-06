using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class EnemyFactory : ScriptableObject
{
    public string md5;

    public List<EnemyAttr> eyList = new List<EnemyAttr>();

    public Dictionary<string, EnemyAttr> eyMap = new Dictionary<string, EnemyAttr>();

    public void init()
    {
        if (eyMap.Count > 0)
            return;

        //if (this.md5 != ConfigCheck.webMd5 || eyList.Count<1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string skillAttrJsonUrl = Application.persistentDataPath +
                   "/" + ConfigCheck.filename + "/battle/Enemy.json";
            skillAttrJsonUrl = skillAttrJsonUrl.Replace('\\', '/');
            string skillAttrJson = ConfigCheck.ReadData(skillAttrJsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(skillAttrJson);
            eyList = obj.ToObject<List<EnemyAttr>>();
        }

        foreach (EnemyAttr ea in eyList)
        {
            //json
            if (ea.skills_1 != null && ea.skills_1.IndexOf("{") != -1)
            {
                JArray t = (JArray)JsonConvert.DeserializeObject(ea.skills_1);
                List<BossSlillInfo> t1 = t.ToObject<List<BossSlillInfo>>();

                ea.skillMap_1 = new List<BossSlillInfo>();
                foreach (var item in t1)
                {
                    ea.skillMap_1.Add(item);
                }
            }

            if (ea.skills_2 != null && ea.skills_2.IndexOf("{") != -1)
            {
                JArray t = (JArray)JsonConvert.DeserializeObject(ea.skills_2);
                List<BossSlillInfo> t1 = t.ToObject<List<BossSlillInfo>>();

                ea.skillMap_2 = new List<BossSlillInfo>();
                foreach (var item in t1)
                {
                    ea.skillMap_2.Add(item);
                }
            }

            eyMap.Add(ea.id, ea);
        }
    }

}
public class BossSlillInfo {
    public string skill;
    public float cd;
}
