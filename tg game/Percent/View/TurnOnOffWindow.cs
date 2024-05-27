using Percent.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Percent.View
{
	public class TurnOnOffWindow : MonoBehaviour
	{
		public Text title;

		public Text description;

		public Text instruction;

		public RotateTween loading;

		public UUIDLoader uuidLoader;

		public PrivacyPopup privatyPopup;

		public PercentTween butWaitScaleTweener;

		private int index;

		private PercentTween scaleTweener;

		private readonly string[] titleTexts = new string[2]
		{
			"TURN ON",
			"TURN OFF"
		};

		private readonly string[] descriptionTexts = new string[2]
		{
			"You can agree or disagree on our privacy policy. You can change the status anytime if you need to change it.",
			"You can agree or disagree on our privacy policy. You can change the status anytime if you need to change it."
		};

		private readonly string[] instructionTexts = new string[2]
		{
			"Google Setting > Ads > Disable opt out of interest based ads",
			"Google Setting > Ads > Enable opt out of interest based ads"
		};

		private AndroidJavaObject activityContext;

		private AndroidJavaObject className;

		private AndroidJavaObject pluginClass;

		private void Awake()
		{
			scaleTweener = GetComponent<ScaleTween>();
		}

		private void showWindow(bool isOn)
		{
			index = ((!isOn) ? 1 : 0);
			title.text = titleTexts[index];
			description.text = descriptionTexts[index];
			instruction.text = instructionTexts[index];
			loading.enableRotation(isEnable: true);
			InvokeRepeating("checkAdvertisingId", 0f, 0.5f);
		}

		public void showTurnOnWindow()
		{
			showWindow(isOn: true);
			if (!hideOnMatch())
			{
				scaleTweener.play();
			}
		}

		public void showTurnOffWindow()
		{
			showWindow(isOn: false);
			if (!hideOnMatch())
			{
				scaleTweener.play();
			}
		}

		private bool hideOnMatch()
		{
			if (UUIDLoader.isTrackingEnabled)
			{
				if (index == 0)
				{
					UnityEngine.Debug.Log("is on, good to go");
					hideWindow();
					return true;
				}
			}
			else if (index == 1)
			{
				UnityEngine.Debug.Log("is off, good to go");
				hideWindow();
				return true;
			}
			return false;
		}

		private void checkAdvertisingId()
		{
			hideOnMatch();
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					activityContext = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
				if (pluginClass == null)
				{
					pluginClass = new AndroidJavaClass("com.percent.crosspromotion.GetAdvertisingId");
				}
				else
				{
					className = pluginClass.CallStatic<AndroidJavaObject>("getInstance", Array.Empty<object>());
					activityContext.Call("runOnUiThread", (AndroidJavaRunnable)delegate
					{
						className.Call("initialize", activityContext);
						if (pluginClass.GetStatic<bool>("hasFinished"))
						{
							UUIDLoader.advertisingId = pluginClass.GetStatic<string>("strAdid");
							UUIDLoader.isTrackingEnabled = pluginClass.GetStatic<bool>("isTrackingEnabled");
							UnityEngine.Debug.Log(UUIDLoader.isTrackingEnabled);
							hideOnMatch();
						}
					});
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("error: " + ex.Message);
			}
		}

		private void hideWindow(bool hasFinished = true)
		{
			CancelInvoke();
			loading.enableRotation(isEnable: false);
			scaleTweener.startDelay = 0f;
			scaleTweener.playReverse();
			if (hasFinished)
			{
				PlayerPrefs.SetInt(CrossPromotion.PREF_AGREEMENT, 1);
				privatyPopup.invokeHide();
			}
		}

		public void backToPreviousWindow()
		{
			hideWindow(hasFinished: false);
			if (index == 0)
			{
				privatyPopup.showFrameWindow();
			}
			else
			{
				butWaitScaleTweener.play();
			}
		}
	}
}
