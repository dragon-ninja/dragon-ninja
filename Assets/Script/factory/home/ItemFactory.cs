using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[CreateAssetMenu(menuName = "data/ItemFactory", fileName = "ItemMode")]
public class ItemFactory : ScriptableObject
{
    static ItemFactory itemF;


    public string md5;
    public List<ItemConfig> itemList = new List<ItemConfig>();
    public Dictionary<string, ItemConfig> itemMap = new Dictionary<string, ItemConfig>();

    public void init()
    {
        //if (this.md5 != ConfigCheck.webMd5 || infoList.Count<1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/common/ItemConfig.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string Json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(Json);
            itemList = obj.ToObject<List<ItemConfig>>();
        }


        foreach (ItemConfig ic in itemList)
        {
            itemMap[ic.id] = ic;
        }
    }

    public static ItemFactory Get() {
        if (itemF == null) { 
            itemF = Resources.Load<ItemFactory>("mode/ItemMode");
            itemF.init();
        }

        return itemF;
    }

}




