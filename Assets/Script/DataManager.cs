using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

//临时数据控制脚本
public class DataManager 
{
    public static DataManager mgr;

    string saveUrl = Application.persistentDataPath +
        "/userData" + "/userData.json";

    bool initFlag;

    public loginData loginData;
    public BackPackData backPackData;
    public RoleAttrData roleAttrData;
    public UserData userData;
    public string nickName;
    public int now_chapterIndex;
    public int now_level = -1;

    public bool GuideAFlag;
    public bool GuideBFlag;

    //是否完成了首充
    public bool FIRST_CHARGE_PACK;

    public static DataManager Get() {
        if (mgr == null)
            mgr = new DataManager();

        mgr.init();

        return mgr;
    }

    public Dictionary<string, string> getHeader()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("user", DataManager.Get().loginData.data.user);
        return dic;
    }

    public string GetWpStr()
    {
        if(DataManager.Get().roleAttrData != null && DataManager.Get().roleAttrData.weaponsBackPackItems!=null)
        foreach (EquipmentData data in DataManager.Get().roleAttrData?.weaponsBackPackItems) {
            EquipmentAtr atr = EquipmentFactory.Get().map[data.id];
            if (atr.itemType == "Weapon") {
                return atr.subType;
            }
        }

        return "Katana";
    }
    public async Task<RoleAttrData> refreshRoleAttributeStr() {

        string roleAttributeStr = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/base/roleAttribute", getHeader());
        DataManager.Get().roleAttrData = JsonUtil.ReadData<RoleAttrData>(roleAttributeStr);
        Debug.Log("roleAttributeStr:"+roleAttributeStr);

        if (now_level == -1) {
            this.now_level = roleAttrData.nowLevel;
        }
        //通知升级
        else if (roleAttrData.nowLevel > this.now_level) {
            int old_Level = now_level;
            this.now_level = roleAttrData.nowLevel;
            MessageMgr.SendMsg("PlayerLevelUp", new MsgKV("", (this.now_level,old_Level)));
        }

        return DataManager.Get().roleAttrData;
    }
    public async Task<BackPackData> refreshBackPack()
    {
        string BackPackStr = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/backPack/getBackPack", getHeader());
        DataManager.Get().backPackData = JsonUtil.ReadData<BackPackData>(BackPackStr);
        Debug.Log("BackPackStr:"+BackPackStr);
        return DataManager.Get().backPackData;
    }

    public void init() {
        if (!initFlag) {
            initFlag = true;
            now_level = -1;
            read();
            refreshRoleAttributeStr();
            refreshBackPack();
        }
    }

    //修改数据
    public void set() { 
    }


    public bool saveIngFlag;

    //保存数据到本地
    public async Task save(bool forGame = false) {
        if (forGame)
            saveIngFlag = true;
       
        string json1 = JsonConvert.SerializeObject(DataManager.Get().userData.towerData);
        await NetManager.post(ConfigCheck.publicUrl+"/data/pub/battleFlow/save", json1, DataManager.Get().getHeader());
       
        if(forGame)
            saveIngFlag = false;
        
        if (!Directory.Exists(Application.persistentDataPath +"/userData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath +"/userData");
        }
        Debug.Log(saveUrl);
        string json = JsonConvert.SerializeObject(userData);
        File.WriteAllText(saveUrl, json);
        return;
    }

    //读取本地数据到游戏
    public async Task read() {
        saveUrl = saveUrl.Replace('\\', '/');

        if (File.Exists(saveUrl))
        {
            string json = ConfigCheck.ReadData(saveUrl);
            JObject obj = (JObject)JsonConvert.DeserializeObject(json);
            userData = obj.ToObject<UserData>();
            userData.towerData = null;
            if (userData.settingData == null)
                userData.settingData = new SettingData();
        }
        else {
            userData = new UserData();
            userData.level = 1;
            userData.gold = 100;
            if (userData.settingData == null)
                userData.settingData = new SettingData();
            /*save();*/
        }

        TowerGameData tData = null;
        string str = await NetManager.get(ConfigCheck.publicUrl+"/data/pub/battleFlow/find", DataManager.Get().getHeader());

        if (str != null)
            tData = JsonUtil.ReadData<TowerGameData>(str);
        
        userData.towerData = tData;
        
        return;
    }
}


public class UserData {
    public string username;
    public int level;
    public int gold;
    public int exp;
    public int gem;
    public int strength;
    public List<string> talentList;
    public List<EquipmentData> equipmentDataList;
    public SettingData settingData = new SettingData();

    //当前爬塔数据  为null 则当前没有正在爬的塔
    public TowerGameData towerData = null;
}


public class SettingData {
    public bool soundFlag = true;
    public bool musicFlag = true;
    public bool shockFlag = true;

    //存放本地的加密后的账号密码 用于自动登录
    public string a;
    public string p;
    public string t;
    public bool hasTempUser;

    //游客模式账号密码
    public string yk_a;
    public string yk_p;

    //教程相关
    public bool courseFlag_1 = false;
    public bool courseFlag_2 = false;

}