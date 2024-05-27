using Percent;
using UnityEngine;

public class PrivacyButton : MonoBehaviour
{
	private void Start()
	{
		base.transform.gameObject.SetActive(!CrossPromotion.isNOTEURegion());
	}

	public void showPrivacy()
	{
		CrossPromotion.showPrivacyWindow();
	}
}
