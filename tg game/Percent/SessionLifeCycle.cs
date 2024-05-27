using UnityEngine;

namespace Percent
{
	public class SessionLifeCycle : MonoBehaviour
	{
		public enum Status
		{
			INIT,
			UUID_SUCCESS,
			UUID_FAIL,
			ACCESS_SUCCESS,
			ACCESS_FAIL
		}

		internal delegate void OnStateChange(Status state);

		public static SessionLifeCycle instance;

		public CrossPromotion crossPromotion;

		public UUIDLoader uuidLoader;

		public AccessDataLoader accessDataLoader;

		public AndroidReferrerRequester referrerRequester;

		private Status state;

		internal OnStateChange onStateChange;

		private bool isGotUUID;

		private bool isGotReferrer;

		private Status State
		{
			set
			{
				state = value;
				if (onStateChange != null)
				{
					onStateChange(state);
				}
			}
		}

		private void Start()
		{
			instance = this;
		}

		private void initalizeSessionLifeCycle()
		{
			if (Config.didCrossPromotionIDSet())
			{
				State = Status.INIT;
				if (!UUIDLoader.hasUUID())
				{
					uuidLoader.request(onReceiveUUID);
					return;
				}
				uuidLoader.setUUID();
				onSendAccess();
			}
		}

		public static void initializeCrossPromotionSession()
		{
			if (instance == null)
			{
				Logger.error("SessionLifeCycle Null Reference");
			}
			else
			{
				instance.initalizeSessionLifeCycle();
			}
		}

		private void onReceiveUUID(bool isSuccess)
		{
			if (isSuccess)
			{
				State = Status.UUID_SUCCESS;
				isGotUUID = true;
				onSendAccess();
				trySendReferrer();
			}
			else
			{
				State = Status.UUID_FAIL;
			}
		}

		private void trySendReferrer()
		{
			if (isGotUUID && isGotReferrer)
			{
				onSendReferrer();
			}
		}

		public void onGetReferrer()
		{
			isGotReferrer = true;
			trySendReferrer();
		}

		private void onSendReferrer()
		{
			referrerRequester.request();
		}

		private void onSendAccess()
		{
			accessDataLoader.request(onReceiveAccess);
		}

		private void onReceiveAccess(bool isSuccess)
		{
			if (isSuccess)
			{
				State = Status.ACCESS_SUCCESS;
				crossPromotion.onPromotionDataLoad(isSuccess: true);
			}
			else
			{
				State = Status.ACCESS_FAIL;
				crossPromotion.onPromotionDataLoad(isSuccess: false);
			}
		}
	}
}
