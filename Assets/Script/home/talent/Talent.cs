[System.Serializable]
public class Talent 
{
    public string id;
    public string name;
    public string desc;
    public string type;
    public int level;
    public int value;
    public ItemInfo expend;

    public string talentType;
    public string lastTalentId;

    public string icon;
    public string bgicon;

    public Talent Clone()
    {
        return (Talent)this.MemberwiseClone();
    }
}

public class TalentData {
    //Ìì¸³id
    public string talentId;
    //ÊÇ·ñ½âËø
    public bool flag;

    public TalentData(string id) {
        talentId = id;
    }
}

public class goldinfo { 
    

}