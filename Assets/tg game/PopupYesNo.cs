using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupYesNo : BaseUI
{
	public GameObject background;

	public Image panel;

	public Button buttonYes;

	public Button buttonNo;

	public TextMeshProUGUI textTitle;

	private PopupDelegate yesCallback;

	private PopupDelegate noCallback;

	public override void onExit()
	{
		offPopup();
		if (noCallback == null)
		{
			if (yesCallback != null)
			{
				yesCallback();
			}
		}
		else
		{
			noCallback();
		}
	}

	public void onYes()
	{
		Debug.Log("onYes");
		offPopup();
		if (yesCallback != null)
		{
			yesCallback();
			yesCallback = null;
		}
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onNo()
	{
		Debug.Log("onNo");
		offPopup();
		if (noCallback != null)
		{
			noCallback();
			noCallback = null;
		}
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onPopup(string text, PopupDelegate yes = null, PopupDelegate no = null)
	{
		base.onStart();
		buttonYes.gameObject.SetActive(value: true);
		buttonNo.gameObject.SetActive(value: true);
		buttonYes.enabled = true;
		buttonNo.enabled = true;
		textTitle.text = text;
		yesCallback = yes;
		noCallback = no;
		Vector3 localPosition = buttonYes.transform.localPosition;
		if (no == null)
		{
			buttonYes.transform.localPosition = new Vector3(0f, localPosition.y, localPosition.z);
			buttonNo.gameObject.SetActive(value: false);
		}
		else
		{
			buttonYes.transform.localPosition = new Vector3(146f, localPosition.y, localPosition.z);
			buttonNo.transform.localPosition = new Vector3(-146f, localPosition.y, localPosition.z);
		}
		base.gameObject.SetActive(value: true);
		background.transform.DOKill();
		panel.DOKill();
		background.transform.localScale = new Vector3(0f, 0f, 0f);
		background.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);
	}

	public void offPopup()
	{
		base.onExit();
		onDelegate();
		buttonYes.enabled = false;
		buttonNo.enabled = false;
		background.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
	}
}
