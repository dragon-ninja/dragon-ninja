using UnityEngine;
using UnityEngine.Events;

namespace Percent
{
	public class TextureLifeCycle : MonoBehaviour
	{
		public enum Status
		{
			INIT,
			LOAD,
			LOAD_ALL
		}

		internal delegate void OnStateChange(Status state);

		public CrossPromotion crossPromotion;

		private Status state;

		internal OnStateChange onStateChange;

		private bool invokeFailAction;

		private int restJob;

		private UnityAction<bool> onAllTextureLoad;

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

		private void Awake()
		{
			onAllTextureLoad = crossPromotion.onTextureLoad;
		}

		internal void load()
		{
			int num = restJob = PromotionData.crossPromotionData.resourceUrl.Length;
			State = Status.LOAD;
		}

		internal void onEachTextureLoad(bool isSuccess)
		{
			if (isSuccess)
			{
				if (isAllJobDone(--restJob))
				{
					State = Status.LOAD_ALL;
					onAllTextureLoad(arg0: true);
				}
			}
			else if (!invokeFailAction)
			{
				onAllTextureLoad(arg0: false);
				invokeFailAction = true;
			}
		}

		private bool isAllJobDone(int restJob)
		{
			if (restJob.Equals(0))
			{
				return true;
			}
			return false;
		}
	}
}
