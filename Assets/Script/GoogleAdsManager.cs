using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class GoogleAdsManager : MonoSingleton<GoogleAdsManager>
{

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    //private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    //pub-3662538546838552

      private string _adUnitId = "ca-app-pub-3662538546838552";
#elif UNITY_IPHONE
      //private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
      private string _adUnitId = "ca-app-pub-3662538546838552";
#else
      private string _adUnitId = "unused";
#endif



    private RewardedAd _rewardedAd;


    public void testAd() {
        ShowRewardedAd();
    }


    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        _adUnitId = "ca-app-pub-3940256099942544/5224354917";  //测试id

        //_adUnitId = "ca-app-pub-5365218020985119/7224448891"; //我们自己的id
        //_adUnitId = "ca-app-pub-5365218020985119/2445128454"; //激励广告

        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("ads-------------------Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,(RewardedAd ad, LoadAdError error) =>
            {
  
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("ads-------------------Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    UIManager.GetUIMgr().showUIForm("ErrForm");
                    MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "ads with error: " + error));
                    return;
                }
                _rewardedAd = ad;

                //UIManager.GetUIMgr().showUIForm("ErrForm");
                //MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "ads-------------------带响应的奖励广告:" + _rewardedAd.GetResponseInfo()));
                Debug.Log("ads-------------------Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
            });
    }


    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
#if UNITY_EDITOR
                adsShowEnd = true;
#endif
            });
        }
        else {
            UIManager.GetUIMgr().showUIForm("ErrForm");
            MessageMgr.SendMsg("ErrorDesc", new MsgKV("", "Rewarded ad failed to load"));
        }

        RegisterEventHandlers(_rewardedAd);
    }


    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
#if UNITY_ANDROID
            adsShowEnd = true;
#endif
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.广告打开
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.广告关闭
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadRewardedAd();
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadRewardedAd();
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };

        //_rewardedAd.Destroy();
        //RegisterReloadHandler(_rewardedAd);
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>{
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    bool adsShowEnd;
    private void Update()
    {
        if (adsShowEnd) {
            adsShowEnd = false;
            MessageMgr.SendMsg("lookAdsEnd", new MsgKV(null, null));
        }
    }
}
