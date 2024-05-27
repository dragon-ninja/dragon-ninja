using UnityEngine;

public class LoadingScene : MonoBehaviour
{
	private void Start()
	{
		Singleton<SceneManager>.Instance.loadingEnded();
	}

	private void Update()
	{
	}

	private void OnApplicationQuit()
	{
		//UCODESHOP TODO
		//GoogleAnalyticsV3.instance.StopSession();
	}
}
