using Percent.Http;

namespace Percent.Event
{
	public class ClickTracker : InHouseTracker
	{
		protected override string getUrl(PromotionType type, int clickedGameId)
		{
			if (!UUIDLoader.hasUUID())
			{
				Logger.error("Can NOT send click event. UUID is NOT exist.");
				return string.Empty;
			}
			string url = HttpsClient.resolve(PercentHttpConfig.GET_REQ_LOG);
			HttpsClient.addResource(ref url, Config.GAME_ID);
			HttpsClient.addResource(ref url, UUIDLoader.uuid);
			HttpsClient.addResource(ref url, PercentHttpConfig.REQ_RESOURCE_CLICK);
			HttpsClient.addResource(ref url, getResourceOf(type));
			HttpsClient.addResource(ref url, clickedGameId);
			return url;
		}

		protected override string addParamater(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			Parameter parameter = new Parameter(url);
			parameter.addParamater(PercentHttpConfig.REQ_RESOURCE_ID, PromotionData.crossPromotionData.rId);
			return parameter.getUrl();
		}
	}
}
