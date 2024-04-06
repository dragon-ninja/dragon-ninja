using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(fileName = "expMode", menuName = "data/ExpFactory")]
public class ExpFactory : ScriptableObject
{

    public string md5;
    public List<UpgradeExp> upgradeExpList = new List<UpgradeExp>();
    public Dictionary<int, int> expMap = new Dictionary<int, int>();

    static ExpFactory myFactory;
    public static ExpFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<ExpFactory>("mode/expMode");
            myFactory.init();
        }

        return myFactory;
    }

    public void init()
    {
        //if (this.md5 != ConfigCheck.webMd5 || upgradeExpList.Count < 1 || ConfigCheck.configChangeFlag)
        {
            md5 = ConfigCheck.webMd5;
            string skillAttrJsonUrl = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/battle/Exp.json";
            skillAttrJsonUrl = skillAttrJsonUrl.Replace('\\', '/');
            string skillAttrJson = ConfigCheck.ReadData(skillAttrJsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(skillAttrJson);
            upgradeExpList = obj.ToObject<List<UpgradeExp>>();
        }


        foreach (UpgradeExp ea in upgradeExpList)
        {
                expMap[ea.id] = ea.exp;
        }
    }
}


[System.Serializable]
public class UpgradeExp
{
    public int id;
    public int exp;
}
