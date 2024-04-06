using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


[CreateAssetMenu(menuName = "data/PerimeterFactory", fileName = "PerimeterMode")]
public class PerimeterFactory : ScriptableObject
{
    static PerimeterFactory myFactory;

    public List<GrowthFundConfig> GrowthFundConfigList = new List<GrowthFundConfig>();
    public List<GrowthFundPriceConfig> GrowthFundPriceConfigList = new List<GrowthFundPriceConfig>();
    
    public List<MissionConfig> MissionConfigList = new List<MissionConfig>();

    public List<MonthlyCardConfig> MonthlyCardConfigList = new List<MonthlyCardConfig>();

    public List<SevenDaySignConfig> SevenDaySignList = new List<SevenDaySignConfig>();

    public List<SignConfig> SignList = new List<SignConfig>();

    public List<PatrolConfig> PatrolList = new List<PatrolConfig>();

    public List<ChargeConfig> ChargeList = new List<ChargeConfig>();

    //public List<PassCheckConfig> PassCheckList = new List<PassCheckConfig>();

    public List<BattlePassRewardsConfig> BattlePassRewardsList = new List<BattlePassRewardsConfig>(); 


    public List<GiftBagConfig> GiftBagList = new List<GiftBagConfig>();

    public List<SupplyCrateConfig> SupplyCrateList = new List<SupplyCrateConfig>();

    public List<LevelUnlockConfig> LevelUnlockList = new List<LevelUnlockConfig>();


    public static PerimeterFactory Get()
    {
        if (myFactory == null)
        {
            myFactory = Resources.Load<PerimeterFactory>("mode/PerimeterMode");
            myFactory.init();
        }

        return myFactory;
    }

    public void init() {
        MissionConfigList = getJson("/mission/Mission.json").
           ToObject<List<MissionConfig>>();

        GrowthFundConfigList = getJson("/perimeter/GrowthFund.json").
            ToObject<List<GrowthFundConfig>>();

        GrowthFundPriceConfigList = getJson("/perimeter/GrowthFundPrice.json").
            ToObject<List<GrowthFundPriceConfig>>();

        MonthlyCardConfigList = getJson("/perimeter/MonthlyCard.json").
           ToObject<List<MonthlyCardConfig>>();

        SevenDaySignList = getJson("/perimeter/SevenDaySign.json").
           ToObject<List<SevenDaySignConfig>>();

        SignList = getJson("/perimeter/Sign.json").
           ToObject<List<SignConfig>>();

        PatrolList = getJson("/perimeter/Patrol.json").
           ToObject<List<PatrolConfig>>();

        ChargeList = getJson("/perimeter/Charge.json").
           ToObject<List<ChargeConfig>>();

        /* PassCheckList = getJson("/perimeter/PassCheck.json").
            ToObject<List<PassCheckConfig>>();*/

        BattlePassRewardsList = getJson("/mission/BattlePassRewards.json").
            ToObject<List<BattlePassRewardsConfig>>();

        GiftBagList = getJson("/perimeter/GiftBag.json").
           ToObject<List<GiftBagConfig>>();

        SupplyCrateList = getJson("/shop/SupplyCrate.json").
           ToObject<List<SupplyCrateConfig>>();

        LevelUnlockList = getJson("/perimeter/LevelUnlock.json").
           ToObject<List<LevelUnlockConfig>>();
    }


    public JArray getJson(string path) {
        string JsonUrl = Application.persistentDataPath +
            "/" + ConfigCheck.filename + path;
        JsonUrl = JsonUrl.Replace('\\', '/');
        string Json = ConfigCheck.ReadData(JsonUrl);
        JArray obj = (JArray)JsonConvert.DeserializeObject(Json);
        return obj;
    }

}
