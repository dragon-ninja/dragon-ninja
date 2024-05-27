using UnityEngine;

public class ETFXSceneManager : MonoBehaviour
{
	public bool GUIHide;

	public bool GUIHide2;

	public bool GUIHide3;

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.L))
		{
			GUIHide = !GUIHide;
			if (GUIHide)
			{
				GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>().enabled = false;
			}
			else
			{
				GameObject.Find("CanvasSceneSelect").GetComponent<Canvas>().enabled = true;
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.J))
		{
			GUIHide2 = !GUIHide2;
			if (GUIHide2)
			{
				GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
			}
			else
			{
				GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.H))
		{
			GUIHide3 = !GUIHide3;
			if (GUIHide3)
			{
				GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>().enabled = false;
			}
			else
			{
				GameObject.Find("ParticleSysDisplayCanvas").GetComponent<Canvas>().enabled = true;
			}
		}
	}
}
