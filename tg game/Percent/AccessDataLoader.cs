using Boomlagoon.JSON;
using Percent.Http;
using UnityEngine.Events;

namespace Percent
{
	public class AccessDataLoader : HttpsClient
	{
		private UnityAction<bool> onReceiveAccess;

		internal void request(UnityAction<bool> onReceiveAccess)
		{
			this.onReceiveAccess = onReceiveAccess;
			sendGETRequest(getUrl(), isResponseJson: true);
		}

		private string getUrl()
		{
			string url = HttpsClient.resolve(PercentHttpConfig.GET_REQ_LOG);
			HttpsClient.addResource(ref url, Config.GAME_ID);
			HttpsClient.addResource(ref url, UUIDLoader.uuid);
			HttpsClient.addResource(ref url, PercentHttpConfig.REQ_RESOURCE_ACCESS);
			return url;
		}

		internal override void onGETResponseSuccess(JSONObject json)
		{
			PromotionData.setData(json);
			base.onGETResponseSuccess(json);
			if (!PromotionData.isPromotionDataNull())
			{
				if (onReceiveAccess != null)
				{
					onReceiveAccess(arg0: true);
				}
			}
			else if (onReceiveAccess != null)
			{
				onReceiveAccess(arg0: false);
			}
		}

		internal override void onGETResponseFail()
		{
			base.onGETResponseFail();
			if (onReceiveAccess != null)
			{
				onReceiveAccess(arg0: false);
			}
		}
	}
}
