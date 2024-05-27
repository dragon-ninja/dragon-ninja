using System.Collections.Generic;

public class AppCustomEventManager : Singleton<AppCustomEventManager>
{
	private string strGameTitle = "WilKnight";

	private bool settingLog;

	public static string getRewardVideoUSD()
	{
		return "0.011";
	}

	public static string getInterstitialUSD()
	{
		return "0.0024";
	}

	public void settingEvent(bool state)
	{
		settingLog = state;
	}

	public void pushEvent(string strMainEvent, string strSubEvent)
	{
		if (settingLog)
		{
			AppsFlyer.trackRichEvent(strGameTitle + "_" + strMainEvent + "_" + strSubEvent, new Dictionary<string, string>());
		}
	}

	public void pushEvent(string strMainEvent)
	{
		if (settingLog)
		{
			AppsFlyer.trackRichEvent(strGameTitle + "_" + strMainEvent, new Dictionary<string, string>());
		}
	}

	public void pushEventPurchase(string strMainEvent, string strSubEvent, string strId, string strType, string strValue)
	{
		if (settingLog)
		{
			AppsFlyer.trackRichEvent(strGameTitle + "_" + strMainEvent + "_" + strSubEvent, new Dictionary<string, string>
			{
				{
					"af_content_id",
					strId
				},
				{
					"af_content_type",
					strType
				},
				{
					"af_revenue",
					strValue
				},
				{
					"af_currency",
					"USD"
				}
			});
		}
	}

	public void pushEventPurchase(string strMainEvent, string strId, string strType, string strValue)
	{
		if (settingLog)
		{
			AppsFlyer.trackRichEvent(strGameTitle + "_" + strMainEvent, new Dictionary<string, string>
			{
				{
					"af_content_id",
					strId
				},
				{
					"af_content_type",
					strType
				},
				{
					"af_revenue",
					strValue
				},
				{
					"af_currency",
					"USD"
				}
			});
		}
	}

	public void pushEventStage(string strMainEvent, string strSubEvent, string value)
	{
		if (settingLog)
		{
			AppsFlyer.trackRichEvent(strGameTitle + "_" + strMainEvent + "_" + strSubEvent, new Dictionary<string, string>
			{
				{
					"value",
					value
				}
			});
		}
	}
}
