using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(menuName = "data/EquipmentFactory", fileName = "EquipmentMode")]
public class EquipmentFactory : ScriptableObject
{
    public static EquipmentFactory factory;

    public string md5;

    public List<EquipmentAtr> list = new List<EquipmentAtr>();
    public List<EquipmentAffix> affixList = new List<EquipmentAffix>();
    public List<EquipmentUpgrade> upgradeList = new List<EquipmentUpgrade>();

    public Dictionary<string, EquipmentAtr> map = new Dictionary<string, EquipmentAtr>();
    public Dictionary<string, EquipmentAffix> affixMap = new Dictionary<string, EquipmentAffix>();
    public Dictionary<int, EquipmentUpgrade> upgradeMap = new Dictionary<int, EquipmentUpgrade>();
    public Dictionary<string, string> materialMap = new Dictionary<string, string>();

    public void init()
    {
        if (map.Count > 0)
            return;

        //if (this.md5 != ConfigCheck.webMd5 || skillList.Count < 1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                    "/" + ConfigCheck.filename + "/common/Equipment.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(json);
            list = obj.ToObject<List<EquipmentAtr>>();


            string JsonUrl2 = Application.persistentDataPath +
                   "/" + ConfigCheck.filename + "/common/EquipmentAffix.json";
            JsonUrl2 = JsonUrl2.Replace('\\', '/');
            string json2 = ConfigCheck.ReadData(JsonUrl2);
            JArray obj2 = (JArray)JsonConvert.DeserializeObject(json2);
            affixList = obj2.ToObject<List<EquipmentAffix>>();


            string JsonUrl3 = Application.persistentDataPath +
                 "/" + ConfigCheck.filename + "/common/EquipmentUpgrade.json";
            JsonUrl3 = JsonUrl3.Replace('\\', '/');
            string json3 = ConfigCheck.ReadData(JsonUrl3);
            JArray obj3 = (JArray)JsonConvert.DeserializeObject(json3);
            upgradeList = obj3.ToObject<List<EquipmentUpgrade>>();

        }

        foreach (EquipmentAtr item in list)
        {
            if (item.id == null || map.ContainsKey(item.id))
                continue;
            map.Add(item.id, item);
        }

        foreach (EquipmentAffix item in affixList)
        {
            if (item.id == null || affixMap.ContainsKey(item.id))
                continue;
            affixMap.Add(item.id, item);
        }

        foreach (EquipmentUpgrade item in upgradeList)
        {
            if (upgradeMap.ContainsKey(item.id))
                continue;

            upgradeMap.Add(item.id, item);
            if (item.id >= 20000 && !materialMap.ContainsKey(item.itemType)) {
                materialMap.Add(item.itemType, item.material_id);
            }
        }
    }

    public static EquipmentFactory Get() {
        if (factory == null) { 
            factory = Resources.Load<EquipmentFactory>("mode/EquipmentMode");
            factory.init();
        }
        return factory;
    }
}


