using System.Collections;
using System.Collections.Generic;

//成长基金
[System.Serializable]
public class GrowthFundConfig 
{
    public string id;
    public int level;
    public ItemInfo reward_0;
    public ItemInfo reward_1;
    public ItemInfo reward_2;
}

public class GrowthFundPriceConfig
{
    public string id;
    public float price;
    public string currency;
    public string type;
}


public class GrowthFundData {
    public string id;
    public bool draw1;
    public bool draw2;
    public bool draw3;
    public GrowthFundData() { 
    }
    public GrowthFundData(string id)
    {
        this.id = id;
    }
}