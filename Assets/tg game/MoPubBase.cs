using MoPubInternal.ThirdParty.MiniJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoPubBase
{
	public enum AdPosition
	{
		TopLeft,
		TopCenter,
		TopRight,
		Centered,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public static class Consent
	{
		public enum Status
		{
			Unknown,
			Denied,
			DoNotTrack,
			PotentialWhitelist,
			Consented
		}

		private static class Strings
		{
			public const string ExplicitYes = "explicit_yes";

			public const string ExplicitNo = "explicit_no";

			public const string Unknown = "unknown";

			public const string PotentialWhitelist = "potential_whitelist";

			public const string Dnt = "dnt";
		}

		public static Status FromString(string status)
		{
			if (!(status == "explicit_yes"))
			{
				if (!(status == "explicit_no"))
				{
					if (!(status == "dnt"))
					{
						if (!(status == "potential_whitelist"))
						{
							if (status == "unknown")
							{
								return Status.Unknown;
							}
							try
							{
								return (Status)Enum.Parse(typeof(Status), status);
							}
							catch
							{
								UnityEngine.Debug.LogError("Unknown consent status string: " + status);
								return Status.Unknown;
							}
						}
						return Status.PotentialWhitelist;
					}
					return Status.DoNotTrack;
				}
				return Status.Denied;
			}
			return Status.Consented;
		}
	}

	public enum BannerType
	{
		Size320x50,
		Size300x250,
		Size728x90,
		Size160x600
	}

	public enum LogLevel
	{
		MPLogLevelAll = 0,
		MPLogLevelTrace = 10,
		MPLogLevelDebug = 20,
		MPLogLevelInfo = 30,
		MPLogLevelWarn = 40,
		MPLogLevelError = 50,
		MPLogLevelFatal = 60,
		MPLogLevelOff = 70
	}

	public struct SdkConfiguration
	{
		public string AdUnitId;

		public AdvancedBidder[] AdvancedBidders;

		public MediationSetting[] MediationSettings;

		public RewardedNetwork[] NetworksToInit;

		public string AdvancedBiddersString
		{
			get
			{
				if (AdvancedBidders == null)
				{
					return string.Empty;
				}
				return string.Join(",", (from b in AdvancedBidders
					select b.ToString()).ToArray());
			}
		}

		public string MediationSettingsJson
		{
			get
			{
				if (MediationSettings == null)
				{
					return string.Empty;
				}
				return Json.Serialize(MediationSettings);
			}
		}

		public string NetworksToInitString
		{
			get
			{
				if (NetworksToInit == null)
				{
					return string.Empty;
				}
				return string.Join(",", (from b in NetworksToInit
					select b.ToString()).ToArray());
			}
		}
	}

	public class MediationSetting : Dictionary<string, object>
	{
		public class AdColony : MediationSetting
		{
			public AdColony()
				: base("AdColony")
			{
			}
		}

		public class AdMob : MediationSetting
		{
			public AdMob()
				: base("GooglePlayServices", "MPGoogle")
			{
			}
		}

		public class Chartboost : MediationSetting
		{
			public Chartboost()
				: base("Chartboost")
			{
			}
		}

		public class Vungle : MediationSetting
		{
			public Vungle()
				: base("Vungle")
			{
			}
		}

		public MediationSetting(string adVendor)
		{
			Add("adVendor", adVendor);
		}

		public MediationSetting(string android, string ios)
			: this(android)
		{
		}
	}

	public struct Reward
	{
		public string Label;

		public int Amount;

		public override string ToString()
		{
			return $"\"{Amount} {Label}\"";
		}

		public bool IsValid()
		{
			if (!string.IsNullOrEmpty(Label))
			{
				return Amount > 0;
			}
			return false;
		}
	}

	public abstract class ThirdPartyNetwork
	{
		private readonly string _name;

		protected ThirdPartyNetwork(string name, string suffix)
		{
			_name = "com.mopub.mobileads." + name + suffix;
		}

		protected ThirdPartyNetwork(string android, string ios, string suffix)
			: this(android, suffix)
		{
		}

		public override string ToString()
		{
			return _name;
		}
	}

	public class AdvancedBidder : ThirdPartyNetwork
	{
		private const string suffix = "AdvancedBidder";

		public static readonly AdvancedBidder AdColony = new AdvancedBidder("AdColony");

		public static readonly AdvancedBidder AdMob = new AdvancedBidder("GooglePlayServices", "MPGoogleAdMob");

		public static readonly AdvancedBidder AppLovin = new AdvancedBidder("AppLovin");

		public static readonly AdvancedBidder Facebook = new AdvancedBidder("Facebook");

		public static readonly AdvancedBidder OnebyAOL = new AdvancedBidder("Millennial", "MPMillennial");

		public static readonly AdvancedBidder Tapjoy = new AdvancedBidder("Tapjoy");

		public static readonly AdvancedBidder Unity = new AdvancedBidder("Unity", "UnityAds");

		public static readonly AdvancedBidder Vungle = new AdvancedBidder("Vungle");

		public AdvancedBidder(string name)
			: base(name, "AdvancedBidder")
		{
		}

		public AdvancedBidder(string android, string ios)
			: base(android, ios, "AdvancedBidder")
		{
		}
	}

	public class RewardedNetwork : ThirdPartyNetwork
	{
		private const string suffix = "RewardedVideo";

		public static readonly RewardedNetwork AdColony = new RewardedNetwork("AdColony");

		public static readonly RewardedNetwork AdMob = new RewardedNetwork("GooglePlayServices", "MPGoogleAdMob");

		public static readonly RewardedNetwork AppLovin = new RewardedNetwork("AppLovin");

		public static readonly RewardedNetwork Chartboost = new RewardedNetwork("Chartboost");

		public static readonly RewardedNetwork Facebook = new RewardedNetwork("Facebook");

		public static readonly RewardedNetwork IronSource = new RewardedNetwork("IronSource");

		public static readonly RewardedNetwork OnebyAOL = new RewardedNetwork("Millennial", "MPMillennial");

		public static readonly RewardedNetwork Tapjoy = new RewardedNetwork("Tapjoy");

		public static readonly RewardedNetwork Unity = new RewardedNetwork("Unity", "UnityAds");

		public static readonly RewardedNetwork Vungle = new RewardedNetwork("Vungle");

		public RewardedNetwork(string name)
			: base(name, "RewardedVideo")
		{
		}

		public RewardedNetwork(string android, string ios)
			: base(android, ios, "RewardedVideo")
		{
		}
	}

	public const double LatLongSentinel = 99999.0;

	public static readonly string moPubSDKVersion = new MoPubSDKVersion().Number;

	private static string _pluginName;

	public static string ConsentLanguageCode
	{
		get;
		set;
	}

	public static string PluginName => _pluginName ?? (_pluginName = "MoPub Unity Plugin v" + moPubSDKVersion);

	protected static void ValidateAdUnitForSdkInit(string adUnitId)
	{
		if (string.IsNullOrEmpty(adUnitId))
		{
			UnityEngine.Debug.LogError("A valid ad unit ID is needed to initialize the MoPub SDK.");
		}
	}

	protected static void ReportAdUnitNotFound(string adUnitId)
	{
		UnityEngine.Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
	}

	protected static Uri UrlFromString(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return null;
		}
		try
		{
			return new Uri(url);
		}
		catch
		{
			UnityEngine.Debug.LogError("Invalid URL: " + url);
			return null;
		}
	}

	protected static void InitManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		MoPubManager component = new GameObject("MoPubManager", typeFromHandle).GetComponent<MoPubManager>();
		if (MoPubManager.Instance != component)
		{
			UnityEngine.Debug.LogWarning("It looks like you have the " + typeFromHandle.Name + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}

	protected MoPubBase()
	{
	}
}
