namespace GooglePlayGames
{
	public static class GameInfo
	{
		private const string UnescapedApplicationId = "APP_ID";

		private const string UnescapedIosClientId = "IOS_CLIENTID";

		private const string UnescapedWebClientId = "WEB_CLIENTID";

		private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";

		public const string ApplicationId = "516713422846";

		public const string IosClientId = "__IOS_CLIENTID__";

		public const string WebClientId = "";

		public const string NearbyConnectionServiceId = "";

		public static bool ApplicationIdInitialized()
		{
			if (!string.IsNullOrEmpty("516713422846"))
			{
				return !"516713422846".Equals(ToEscapedToken("APP_ID"));
			}
			return false;
		}

		public static bool IosClientIdInitialized()
		{
			if (!string.IsNullOrEmpty("__IOS_CLIENTID__"))
			{
				return !"__IOS_CLIENTID__".Equals(ToEscapedToken("IOS_CLIENTID"));
			}
			return false;
		}

		public static bool WebClientIdInitialized()
		{
			if (!string.IsNullOrEmpty(""))
			{
				return !"".Equals(ToEscapedToken("WEB_CLIENTID"));
			}
			return false;
		}

		public static bool NearbyConnectionsInitialized()
		{
			if (!string.IsNullOrEmpty(""))
			{
				return !"".Equals(ToEscapedToken("NEARBY_SERVICE_ID"));
			}
			return false;
		}

		private static string ToEscapedToken(string token)
		{
			return $"__{token}__";
		}
	}
}
