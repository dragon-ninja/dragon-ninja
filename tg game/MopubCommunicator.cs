using GoogleMobileAds.Api;
using Percent;
using System;
using System.Collections;
using UnityEngine;

public class MopubCommunicator : Singleton<MopubCommunicator>
{
	private string strVideoKey;

	private string strInterstitialKey;

	private string strAppId;

	private string strBannerKey;

	private MopubCallback videoCallback;

	private MopubCallback interstitialCallback;

	private bool bannerNotCreate;

	private bool startLoad;

	private bool initEnded;

	private bool rewardState;

	private bool bannerLoadState;

	private bool rewardLoadState;

	private bool interstitialLoadState;

	private BannerView bannerView;

	private void Start()
	{
		strVideoKey = "b2a8768b2ed54d60acb7c636fd185f1b";
		strInterstitialKey = "78962dfd0c754ce3a0b1338b565282e8";
		strAppId = "ca-app-pub-9932267989523399~8978116834";
		strBannerKey = "ca-app-pub-9932267989523399/2613509728";
		MobileAds.Initialize(strAppId);
		bannerView = new BannerView(strBannerKey, AdSize.Banner, AdPosition.Top);
		MoPubAndroid.InitializeSdk(strVideoKey);
		MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(new string[1]
		{
			strVideoKey
		});
		MoPubAndroid.LoadInterstitialPluginsForAdUnits(new string[1]
		{
			strInterstitialKey
		});
		MoPubAndroid.EnableLocationSupport(CrossPromotion.hasAgreed);
		bannerView.OnAdLoaded += HandleOnAdLoaded;
		bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		bannerView.OnAdOpening += HandleOnAdOpened;
		bannerView.OnAdClosed += HandleOnAdClosed;
		bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void loadVideo()
	{
		if (!rewardLoadState)
		{
			UnityEngine.Debug.Log("loadVideo");
			MoPubAndroid.RequestRewardedVideo(strVideoKey);
			rewardLoadState = true;
		}
	}

	public bool hasVideo()
	{
		return MoPubAndroid.HasRewardedVideo(strVideoKey);
	}

	public void showVideo(MopubCallback callback)
	{
		rewardLoadState = false;
		videoCallback = callback;
		MoPubAndroid.ShowRewardedVideo(strVideoKey);
		UnityEngine.Debug.Log("show call!");
	}

	public void OnRewardedVideoLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLoadedEvent: " + adUnitId);
	}

	private IEnumerator loadEvents()
	{
		yield return new WaitForSeconds(5f);
		settingCallbacks();
	}

	public void OnRewardedVideoFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + error);
		rewardLoadState = false;
	}

	public void OnRewardedVideoExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + adUnitId);
		rewardLoadState = false;
	}

	public void OnRewardedVideoShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoShownEvent: " + adUnitId);
	}

	public void OnRewardedVideoClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
	}

	public void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
	{
		rewardLoadState = false;
	}

	public void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
	{
		UnityEngine.Debug.Log("OnRewardedVideoReceivedRewardEvent for ad unit id " + adUnitId + " currency:" + label + " amount:" + amount);
		rewardState = true;
		if (videoCallback != null)
		{
			videoCallback();
			rewardState = false;
		}
		videoCallback = null;
	}

	public void OnRewardedVideoClosedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClosedEvent: " + adUnitId);
		StartCoroutine(loadVideoDelay());
	}

	private IEnumerator loadVideoDelay()
	{
		yield return new WaitForSeconds(0.1f);
		loadVideo();
	}

	public void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLeavingApplicationEvent: " + adUnitId);
	}

	public void loadInterstitial()
	{
		MoPubAndroid.RequestInterstitialAd(strInterstitialKey);
	}

	public void showInterstitial(MopubCallback callback)
	{
		if (interstitialLoadState)
		{
			interstitialCallback = callback;
			interstitialLoadState = false;
			MoPubAndroid.ShowInterstitialAd(strInterstitialKey);
		}
	}

	public void OnInterstitialLoadedEvent(string adUnitId)
	{
		interstitialCallback = null;
		interstitialLoadState = true;
		UnityEngine.Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
	}

	public void OnInterstitialFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnInterstitialFailedEvent: " + error);
	}

	public void OnInterstitialShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialShownEvent: " + adUnitId);
		if (interstitialCallback != null)
		{
			interstitialCallback();
		}
		interstitialCallback = null;
	}

	public void OnInterstitialClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
	}

	public void OnInterstitialDismissedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
		loadInterstitial();
	}

	public void OnInterstitialExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
		loadInterstitial();
	}

	public void showBanner()
	{
		UnityEngine.Debug.Log("show banner");
		if (!bannerLoadState && bannerView != null)
		{
			AdRequest request = new AdRequest.Builder().Build();
			bannerView.LoadAd(request);
			bannerLoadState = true;
		}
	}

	public void destroyBanner()
	{
		UnityEngine.Debug.Log("destroy banner");
		if (bannerView != null)
		{
			bannerView.Destroy();
		}
		bannerView = null;
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		bannerLoadState = false;
		MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public void OnSdkInitializedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnSdkInitializedEvent: " + adUnitId);
		UnityEngine.Debug.Log("banner startLoad2 : " + startLoad.ToString() + ", initEnded : " + initEnded.ToString());
		if (initEnded)
		{
			return;
		}
		initEnded = true;
		if (startLoad)
		{
			if (!bannerNotCreate)
			{
				showBanner();
			}
			else
			{
				destroyBanner();
			}
			loadVideo();
			loadInterstitial();
		}
	}

	public void startLoadAd(bool notCreate)
	{
		UnityEngine.Debug.Log("banner startLoad1 : " + startLoad.ToString() + ", initEnded : " + initEnded.ToString());
		if (startLoad)
		{
			return;
		}
		bannerNotCreate = notCreate;
		startLoad = true;
		if (initEnded)
		{
			if (!bannerNotCreate)
			{
				showBanner();
			}
			else
			{
				destroyBanner();
			}
			loadVideo();
			loadInterstitial();
		}
	}

	private void settingCallbacks()
	{
	}

	private void OnDisable()
	{
	}
}
