using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
//using GoogleMobileAds.Api;

public class HomeManager : MonoBehaviour
{

    public AudioMixer audioMixer;

    void Start()
    {
        audioMixer.SetFloat("vSound", DataManager.Get().userData.settingData.soundFlag ? 0 : -100f);
        audioMixer.SetFloat("vMusic", DataManager.Get().userData.settingData.musicFlag ? 0 : -100f);

        Application.targetFrameRate = 120;
        DataManager.Get().init();
        UIManager.GetUIMgr().showUIForm("up_menu");
        UIManager.GetUIMgr().showUIForm("down_menu");
        UIManager.GetUIMgr().preload("TalentForm");
        UIManager.GetUIMgr().preload("ShopForm");
        UIManager.GetUIMgr().preload("BackPackForm");
        UIManager.GetUIMgr().showUIForm("DungeonForm");
        //UIManager.GetUIMgr().showUIForm("LoadForm");
        //UIManager.GetUIMgr().preload("MissionForm");

        //DataManager.Get().userData.towerData = null;
        //DataManager.Get().save();

        IAPTools.Instance.InitUnityPurchase();
        GoogleAdsManager.Instance.LoadRewardedAd();
    }
}
