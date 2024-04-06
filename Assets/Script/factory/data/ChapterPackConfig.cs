using System.Collections;
using System.Collections.Generic;

//章节礼包
[System.Serializable]
public class ChapterPackConfig
{
    public string id;
    public string name;
    public List<ItemInfo> itemList;
    public float price;
    public float preferentialPrice;
    public string imgPath;
}
[System.Serializable]
public class ItemInfo
{
    public string id;
    //和id完全一样 只是服务器有时候会传itemId这个字段
    public string itemId;
    public int num;
    //如果是装备 则有品级参数
    public int grade;       //配置里的品质参数
    public int quality;     //服务器传来的品质参数
    public int level;

    public int price;

    public ItemInfo() { 
    }

    public ItemInfo(string id,int num, int grade = 0, int level = 0)
    {
        this.id = id;
        this.num = num;
        this.grade = grade;
        this.level = level;
    }
}

public class ItemInfosData {
    public List<ItemInfo2> data;
}
public class ItemInfo2 {
    public string id;
    public string itemId;
    public int num;
    public int quality;
    public int level;
}


public class ItemInfo3
{
    public string equipmentId;
    public int quality;
}