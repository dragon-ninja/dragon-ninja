using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



[CreateAssetMenu(menuName = "data/ShopFactory", fileName = "ShopMode")]
public class ShopFactory : ScriptableObject
{
    public string md5;
    public List<ChapterPackConfig> ChapterPackList = new List<ChapterPackConfig>();
    public List<DailyShopConfig> DailyShopConfigList = new List<DailyShopConfig>();

    public List<DiamondAndGoldConfig> DiamondAndGoldConfigList = new List<DiamondAndGoldConfig>();
    public List<StrengthShopConfig> StrengthShopList = new List<StrengthShopConfig>();

    static ShopFactory myFactory;

    public static ShopFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<ShopFactory>("mode/ShopMode");
            myFactory.init();
        }

        return myFactory;
    }

    public void init()
    {
        //if (this.md5 != ConfigCheck.webMd5 || infoList.Count<1 || ConfigCheck.configChangeFlag) 
        {
            md5 = ConfigCheck.webMd5;
            string JsonUrl = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/shop/ChapterPack.json";
            JsonUrl = JsonUrl.Replace('\\', '/');
            string Json = ConfigCheck.ReadData(JsonUrl);
            JArray obj = (JArray)JsonConvert.DeserializeObject(Json);
            ChapterPackList = obj.ToObject<List<ChapterPackConfig>>();


            string JsonUrl2 = Application.persistentDataPath +
                "/" + ConfigCheck.filename + "/shop/DailyShop.json";
            JsonUrl2 = JsonUrl2.Replace('\\', '/');
            string Json2 = ConfigCheck.ReadData(JsonUrl2);
            JArray obj2 = (JArray)JsonConvert.DeserializeObject(Json2);
            DailyShopConfigList = obj2.ToObject<List<DailyShopConfig>>();


            string JsonUrl3 = Application.persistentDataPath +
               "/" + ConfigCheck.filename + "/shop/DiamondAndGold.json";
            JsonUrl3 = JsonUrl3.Replace('\\', '/');
            string Json3 = ConfigCheck.ReadData(JsonUrl3);
            JArray obj3 = (JArray)JsonConvert.DeserializeObject(Json3);
            DiamondAndGoldConfigList = obj3.ToObject<List<DiamondAndGoldConfig>>();


            StrengthShopList = getJson("/shop/StrengthShop.json").
                 ToObject<List<StrengthShopConfig>>();


            /*  Debug.Log("ChapterPackList.Count:"+ ChapterPackList.Count);
              Debug.Log("ChapterPackList.0:" + ChapterPackList[0].itemList.Count);
              Debug.Log(ChapterPackList[0].itemList[0].id+"   "+ ChapterPackList[0].itemList[0].num);*/
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

