using UnityEngine;

public class MopubEventObject : MonoBehaviour
{
	private MopubCommunicator mopubCommunicator;

	private void Start()
	{
		mopubCommunicator = Singleton<MopubCommunicator>.Instance;
	}

	private void OnRewardedVideoLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLoadedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoLoadedEvent(adUnitId);
		}
	}

	private void OnRewardedVideoFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + error);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoFailedEvent(adUnitId, error);
		}
	}

	private void OnRewardedVideoExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoExpiredEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoExpiredEvent(adUnitId);
		}
	}

	private void OnRewardedVideoShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoShownEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoShownEvent(adUnitId);
		}
	}

	private void OnRewardedVideoClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClickedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoClickedEvent(adUnitId);
		}
	}

	private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
	{
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoFailedToPlayEvent(adUnitId, error);
		}
	}

	private void OnRewardedVideoReceivedRewardEvent(string adUnitId, string label, float amount)
	{
		UnityEngine.Debug.Log("OnRewardedVideoReceivedRewardEvent for ad unit id " + adUnitId + " currency:" + label + " amount:" + amount);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoReceivedRewardEvent(adUnitId, label, amount);
		}
	}

	private void OnRewardedVideoClosedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoClosedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoClosedEvent(adUnitId);
		}
	}

	private void OnRewardedVideoLeavingApplicationEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnRewardedVideoLeavingApplicationEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnRewardedVideoLeavingApplicationEvent(adUnitId);
		}
	}

	private void OnInterstitialLoadedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialLoadedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialLoadedEvent(adUnitId);
		}
	}

	private void OnInterstitialFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnInterstitialFailedEvent: " + error);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialFailedEvent(adUnitId, error);
		}
	}

	private void OnInterstitialShownEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialShownEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialShownEvent(adUnitId);
		}
	}

	private void OnInterstitialClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialClickedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialClickedEvent(adUnitId);
		}
	}

	private void OnInterstitialDismissedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialDismissedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialDismissedEvent(adUnitId);
		}
	}

	private void OnInterstitialExpiredEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnInterstitialExpiredEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnInterstitialExpiredEvent(adUnitId);
		}
	}

	private void OnAdLoadedEvent(string adUnitId, float height)
	{
		UnityEngine.Debug.Log("OnAdLoadedEvent: " + adUnitId + " height: " + height);
	}

	private void OnAdFailedEvent(string adUnitId, string error)
	{
		UnityEngine.Debug.Log("OnAdFailedEvent: " + adUnitId + " height: " + error);
	}

	private void OnAdClickedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdClickedEvent: " + adUnitId);
	}

	private void OnAdExpandedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdExpandedEvent: " + adUnitId);
	}

	private void OnAdCollapsedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnAdCollapsedEvent: " + adUnitId);
	}

	private void OnSdkInitializedEvent(string adUnitId)
	{
		UnityEngine.Debug.Log("OnSdkInitializedEvent: " + adUnitId);
		if (mopubCommunicator != null)
		{
			mopubCommunicator.OnSdkInitializedEvent(adUnitId);
		}
	}

	private void OnEnable()
	{
		MoPubManager.OnSdkInitializedEvent += OnSdkInitializedEvent;
		MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
		MoPubManager.OnAdFailedEvent += OnAdFailedEvent;
		MoPubManager.OnAdClickedEvent += OnAdClickedEvent;
		MoPubManager.OnAdExpandedEvent += OnAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent += OnAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent += OnInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent += OnInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent += OnInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent += OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent += OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent += OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent += OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent += OnRewardedVideoLeavingApplicationEvent;
	}

	private void OnDisable()
	{
		MoPubManager.OnSdkInitializedEvent -= OnSdkInitializedEvent;
		MoPubManager.OnAdLoadedEvent -= OnAdLoadedEvent;
		MoPubManager.OnAdFailedEvent -= OnAdFailedEvent;
		MoPubManager.OnAdClickedEvent -= OnAdClickedEvent;
		MoPubManager.OnAdExpandedEvent -= OnAdExpandedEvent;
		MoPubManager.OnAdCollapsedEvent -= OnAdCollapsedEvent;
		MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
		MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
		MoPubManager.OnInterstitialShownEvent -= OnInterstitialShownEvent;
		MoPubManager.OnInterstitialClickedEvent -= OnInterstitialClickedEvent;
		MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;
		MoPubManager.OnInterstitialExpiredEvent -= OnInterstitialExpiredEvent;
		MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
		MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
		MoPubManager.OnRewardedVideoExpiredEvent -= OnRewardedVideoExpiredEvent;
		MoPubManager.OnRewardedVideoShownEvent -= OnRewardedVideoShownEvent;
		MoPubManager.OnRewardedVideoClickedEvent -= OnRewardedVideoClickedEvent;
		MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
		MoPubManager.OnRewardedVideoReceivedRewardEvent -= OnRewardedVideoReceivedRewardEvent;
		MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;
		MoPubManager.OnRewardedVideoLeavingApplicationEvent -= OnRewardedVideoLeavingApplicationEvent;
	}
}
