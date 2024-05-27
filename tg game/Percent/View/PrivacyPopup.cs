using Boomlagoon.JSON;
using Percent.Http;
using Percent.Tween;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Percent.View
{
	public class PrivacyPopup : MonoBehaviour
	{
		public ColorTween showBackgroundColorTweener;

		public PercentTween showFrameScaleTweener;

		public PercentTween showWaitFrameScaleTweener;

		public GameObject waitPopupGO;

		public TurnOnOffWindow turnOnOffWindow;

		public UUIDLoader uuidLoader;

		public Text descriptionText;

		public TextLoader textLoader;

		public RectTransform scrollMe;

		public GameObject thirdParties;

		private TextTool textTool;

		private Scrollbar scrollBar;

		public int privacyPolicyVersion;

		public bool isSendTracker = true;

		private readonly string PRIVACY_POLICY_VERSION = "PrivacyPolicyVersion";

		private readonly string PRIVACY_POLICY = "privacyPolicy";

		public static readonly string[] EU_CODES = new string[32]
		{
			"AT",
			"BE",
			"BG",
			"HR",
			"CY",
			"CZ",
			"DK",
			"EE",
			"FI",
			"FR",
			"DE",
			"GR",
			"HU",
			"IE",
			"IT",
			"LV",
			"LT",
			"LU",
			"MT",
			"NL",
			"PL",
			"PT",
			"RO",
			"SK",
			"SI",
			"ES",
			"SE",
			"AE",
			"BGN",
			"HRK",
			"EUR",
			"RON"
		};

		private AndroidJavaObject activityContext;

		private AndroidJavaObject className;

		private AndroidJavaObject pluginClass;

		private void Start()
		{
			Application.RequestAdvertisingIdentifierAsync(delegate(string advertisingId, bool trackingEnabled, string error)
			{
				UUIDLoader.advertisingId = advertisingId;
				UUIDLoader.isTrackingEnabled = trackingEnabled;
			});
			if (PlayerPrefs.HasKey(PRIVACY_POLICY_VERSION))
			{
				privacyPolicyVersion = PlayerPrefs.GetInt(PRIVACY_POLICY_VERSION);
			}
			textTool = new TextTool();
			replaceDescription(textLoader.readDeafultDescription());
			hideSelfWithoutThirdparties();
		}

		public void showWindow()
		{
			textLoader.request(onReceieveText);
		}

		private void playShowWindow()
		{
			CancelInvoke();
			CrossPromotion.hasAgreed = true;
			scrollBar = showFrameScaleTweener.transform.Find("InnerTextBox").Find("Scrollbar").GetComponent<Scrollbar>();
			showFrameScaleTweener.onEnd = delegate
			{
				scrollBar.GetComponent<ScrollToTop>().repositionScrollbar();
			};
			showBackgroundColorTweener.play();
			showFrameScaleTweener.play();
		}

		private void onReceieveText(bool isSuccess)
		{
			if (isSuccess)
			{
				if (CrossPromotion.isEURegion)
				{
					playShowWindow();
				}
			}
			else
			{
				hideSelf();
			}
		}

		public void hideWindowWithAgreement()
		{
			InvokeRepeating("getAndroidAdvertisingId", 0f, 0.1f);
		}

		private void getAndroidAdvertisingId()
		{
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					activityContext = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
				if (pluginClass != null)
				{
					className = pluginClass.CallStatic<AndroidJavaObject>("getInstance", Array.Empty<object>());
					activityContext.Call("runOnUiThread", (AndroidJavaRunnable)delegate
					{
						className.Call("initialize", activityContext);
						if (pluginClass.GetStatic<bool>("hasFinished"))
						{
							UUIDLoader.advertisingId = pluginClass.GetStatic<string>("strAdid");
							UUIDLoader.isTrackingEnabled = pluginClass.GetStatic<bool>("isTrackingEnabled");
							UnityEngine.Debug.Log("android uuid enabled? " + UUIDLoader.isTrackingEnabled.ToString());
							checkTrackability();
							CancelInvoke();
						}
					});
				}
				else
				{
					pluginClass = new AndroidJavaClass("com.percent.crosspromotion.GetAdvertisingId");
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("error: " + ex.Message);
			}
		}

		private void checkTrackability()
		{
			if (UUIDLoader.isTrackingEnabled)
			{
				CrossPromotion.hasAgreed = true;
				invokeHide();
			}
			else
			{
				turnOnOffWindow.showTurnOnWindow();
				showFrameScaleTweener.startDelay = 0f;
				showFrameScaleTweener.playReverse();
			}
		}

		public void invokeHide()
		{
			showFrameScaleTweener.startDelay = 0f;
			showFrameScaleTweener.playReverse();
			PercentTween percentTween = showFrameScaleTweener;
			percentTween.onEnd = (PercentTween.OnTweenEnd)Delegate.Combine(percentTween.onEnd, new PercentTween.OnTweenEnd(hideSelf));
			showBackgroundColorTweener.startDelay = 0f;
			showBackgroundColorTweener.playReverse();
		}

		public void hideSelf()
		{
			PlayerPrefs.SetInt(CrossPromotion.PREF_AGREEMENT, 1);
			hideSelfWithoutThirdparties();
			if (!thirdParties.activeSelf)
			{
				thirdParties.SetActive(value: true);
			}
			SessionLifeCycle.initializeCrossPromotionSession();
			Invoke("inactivateSelf", 0.5f);
		}

		private void inactivateSelf()
		{
			base.gameObject.SetActive(value: false);
			if (isSendTracker)
			{
				bool hasAgreed = CrossPromotion.hasAgreed;
			}
			else
			{
				isSendTracker = true;
			}
		}

		private void hideSelfWithoutThirdparties()
		{
			turnOnOffWindow.GetComponent<RectTransform>().localScale = Vector3.zero;
			PercentTween percentTween = showFrameScaleTweener;
			percentTween.onEnd = (PercentTween.OnTweenEnd)Delegate.Remove(percentTween.onEnd, new PercentTween.OnTweenEnd(hideSelf));
		}

		public void showWaitWindow()
		{
			waitPopupGO.SetActive(value: true);
			showWaitFrameScaleTweener.play();
			showFrameScaleTweener.playReverse();
			CrossPromotion.hasAgreed = false;
		}

		public void showFrameWindow()
		{
			showFrameScaleTweener.play();
		}

		public void parseData(JSONObject json)
		{
			CrossPromotion.isEURegion = true;
			if (json.ContainsKey("success"))
			{
				CrossPromotion.isEURegion &= json.GetBoolean("success");
			}
			if (json.ContainsKey("accepted"))
			{
				CrossPromotion.isEURegion &= json.GetBoolean("accepted");
			}
			if (!CrossPromotion.isEURegion)
			{
				hideSelf();
			}
			else
			{
				extractDescription(json);
			}
		}

		private void extractDescription(JSONObject json)
		{
			if (!json.ContainsKey("version"))
			{
				return;
			}
			int num = (int)json.GetNumber("version");
			if (num == privacyPolicyVersion)
			{
				string text = textTool.loadTextFromCache(PRIVACY_POLICY);
				if (!text.Equals(string.Empty))
				{
					replaceDescription(text);
				}
				return;
			}
			privacyPolicyVersion = num;
			if (json.ContainsKey(PRIVACY_POLICY))
			{
				string @string = json.GetString(PRIVACY_POLICY);
				if (!@string.Equals(string.Empty))
				{
					replaceDescription(@string);
					textTool.saveText(PRIVACY_POLICY, @string);
					PlayerPrefs.SetInt(PRIVACY_POLICY_VERSION, num);
				}
				else
				{
					PlayerPrefs.SetInt(PRIVACY_POLICY_VERSION, 0);
				}
			}
		}

		private void replaceDescription(string newDescription)
		{
			descriptionText.text = Regex.Unescape(newDescription);
			scrollMe.sizeDelta = new Vector2(scrollMe.sizeDelta.x, descriptionText.GetComponent<RectTransform>().sizeDelta.y + 500f);
		}
	}
}
