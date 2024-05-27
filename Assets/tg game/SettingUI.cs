using DG.Tweening;
using Percent;
using Percent.Event;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BaseUI
{
	public Sprite imageBGMon;

	public Sprite imageBGMoff;

	public Sprite imageSOUNDon;

	public Sprite imageSOUNDoff;

	public Image bgmImage;

	public Image soundImage;

	public Transform background;

	public Image panel;

	public GameObject homePanel;

	private DataManager dataManager;

	private SoundManager soundManager;

	private PercentTracker tracker;

	public override void onStart()
	{
		base.onStart();
		if (tracker == null)
		{
			tracker = base.gameObject.AddComponent<PercentTracker>();
		}
		dataManager = Singleton<DataManager>.Instance;
		soundManager = Singleton<SoundManager>.Instance;
		base.gameObject.SetActive(value: true);
		refreshBGM();
		refreshSound();
		background.localScale = new Vector3(0f, 0f, 0f);
		background.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);

		transform.Find("Canvas/Background/ButtonCredit").gameObject.SetActive(false);
		transform.Find("Canvas/Background/TextDelete").gameObject.SetActive(false);
		transform.Find("Canvas/Background/TextWarning").gameObject.SetActive(false);
		transform.Find("Canvas/Background/IconWarning").gameObject.SetActive(false);
	}

	public override void onExit()
	{
		base.onExit();
		onDelegate();
		background.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onBgmToggle()
	{
		bool flag = dataManager.bgmState;
		flag = !flag;
		dataManager.bgmState = flag;
		soundManager.setBGMState(flag);
		dataManager.saveDataAsync();
		refreshBGM();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onSoundToggle()
	{
		bool flag = dataManager.soundState;
		flag = !flag;
		dataManager.soundState = flag;
		soundManager.setSoundState(flag);
		dataManager.saveDataAsync();
		refreshSound();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onHome()
	{
		homePanel.SetActive(value: true);
		dataManager.saveDataAsync();
		bool flag = false;
		if (Singleton<SceneManager>.Instance.getStage() != 0)
		{
			flag = true;
			//GoogleAnalyticsV3.instance.LogEvent("GAME", "STAGE", "STAGE_GOHOME_" + Singleton<SceneManager>.Instance.getStage().ToString(), 1L);
		}
		Singleton<SceneManager>.Instance.changeScene(0);
		Singleton<SoundManager>.Instance.playSound("uiClick");
		if (flag && (int)dataManager.clearStage >= 1 && CrossPromotion.isInterstitialShowable())
		{
			if (!dataManager.noAds)
			{
				Singleton<MopubCommunicator>.Instance.showInterstitial(delegate
				{
					tracker.triggerSeeAdsInterstitial();
					Singleton<AppCustomEventManager>.Instance.pushEvent("watchIS", "interstitial");
					DataManager obj = dataManager;
					obj.interstitialWatchCount = (int)obj.interstitialWatchCount + 1;
				});
			}
			CrossPromotion.reportShowInterstitial();
		}
	}

	public void onCredit()
	{
		Singleton<UIControlManager>.Instance.onCreditUI(delegate
		{
		});
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	private void refreshBGM()
	{
		bgmImage.sprite = (dataManager.bgmState ? imageBGMon : imageBGMoff);
		bgmImage.SetNativeSize();
	}

	private void refreshSound()
	{
		soundImage.sprite = (dataManager.soundState ? imageSOUNDon : imageSOUNDoff);
		soundImage.SetNativeSize();
	}
}
