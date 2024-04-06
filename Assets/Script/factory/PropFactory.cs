using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[CreateAssetMenu(menuName = "data/propFactory", fileName = "propMode")]
public class PropFactory : ScriptableObject
{
    public string md5;

    public List<Prop> PropList = new List<Prop>();

    public Dictionary<string, Prop> PropMap = new Dictionary<string, Prop>();

    public void init()
    {
        if (PropMap.Count > 0)
            return;

        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/battle/Prop.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            PropList = obj.ToObject<List<Prop>>();
        }


        for (int i=0;i< PropList.Count;i++)
        {
            if (PropList[i].id == null)
                continue;

            Prop p = PropList[i];

            p.gpf = Resources.Load<GameObject>("prop/" + p.pf);

            PropMap.Add(p.id, p);
        }
    }

    static PropFactory myFactory;

    public static PropFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<PropFactory>("mode/propMode");
            myFactory.init();
        }

        return myFactory;
    }

}

[System.Serializable]
public struct Prop {
    public string id;
    public string name;
    public string pf;
    public int rate;
    public bool flag;
    public GameObject gpf;
    public float time;
    public int effect;
}