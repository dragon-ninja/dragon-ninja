using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDemeScene : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("DemoScene");
	}
}
