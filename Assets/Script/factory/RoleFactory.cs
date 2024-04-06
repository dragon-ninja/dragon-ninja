using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(menuName = "data/roleFactory", fileName = "roleMode")]
public class RoleFactory : ScriptableObject
{
    public string md5;

    public List<RoleAttr> roleList = new List<RoleAttr>();

    public Dictionary<int, RoleAttr> roleAttrMap = new Dictionary<int, RoleAttr>();

    static RoleFactory myFactory;
    public static RoleFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<RoleFactory>("mode/roleMode");
            myFactory.init();
        }

        return myFactory;
    }

    public void init()
    {

        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/battle/RoleAttr.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            roleList = obj.ToObject<List<RoleAttr>>();
        }


        foreach (RoleAttr sa in roleList)
        {
            if (sa.id == null)
                continue;

            roleAttrMap.Add(sa.id,sa);
        }
    }
}
