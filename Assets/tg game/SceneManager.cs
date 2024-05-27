using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
	private ObscuredInt nowStage = 0;

	private int oldStage;

	private string changeSceneName = "";

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	public void changeScene(int stage)
	{
		nowStage = stage;
		if (stage == 0)
		{
			changeSceneName = "Village";
		}
		else
		{
			changeSceneName = "Game" + stage.ToString();
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
	}

	public void loadingEnded()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(changeSceneName);
	}

	public int getStage()
	{
		return nowStage;
	}

	public int getOldState()
	{
		return oldStage;
	}

	public void setOldStage(int index)
	{
		oldStage = index;
	}
}
