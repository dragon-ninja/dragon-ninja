using UnityEngine;

public class TutorialGameTouch : MonoBehaviour
{
	public GameObject finger;

	public void onTouch()
	{
		base.gameObject.SetActive(value: true);
		TutorialManager.fingerAction(finger);
	}
}
