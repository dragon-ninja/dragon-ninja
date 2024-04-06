using System.Collections;
using System.Collections.Generic;

//每日商店配置
[System.Serializable]
public class DailyShopConfig
{
    public string id;
    public string itemId;
    //货币类型
    public string currency;
    //权重参数
    public int weight;
    public List<weight> qualityWeight;
    public List<weight> priceWeight;
    public List<weight> numWeight;
    public List<weight> preferentialWeight;

    //免费资源参数  数量 购买cd 购买次数
    public int num;
    public int cd;
    public int buyCount;

}
//权重
public class weight
{
    public int key;
    public int value;
}
//每日商店实际数据
public class DailyShopData
{
    public string id;
    public string itemId;
    //货币类型
    public string currency;
    public int quality;
    public int price;
    public int num;
    public int preferential;
}


public class NetDailyShopData
{
    public List<NetDailyShopInfoData> dailyInfoList;
}
public class NetDailyShopInfoData
{
    public string dailyId;
    public int serialNumber;
    public string id;
    public string itemId;
    public string type;
    public string currency;
    public int num;
    public int price;
    public float discountRate;
    public int discountPrice;
    public int quality;
    public int buyCount;
    public int payedNum;
}
