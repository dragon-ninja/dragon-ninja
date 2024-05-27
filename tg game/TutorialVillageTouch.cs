using UnityEngine;

public class TutorialVillageTouch : MonoBehaviour
{
	public GameObject finger;

	public void onDungeon()
	{
		Debug.LogError("开始教程");
		base.gameObject.SetActive(value: true);
		TutorialManager.fingerAction(finger);
	}
}
