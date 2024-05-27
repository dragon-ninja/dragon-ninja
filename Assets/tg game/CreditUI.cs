using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : BaseUI
{
	public Transform background;

	public Image panel;

	public override void onStart()
	{
		base.onStart();
		base.gameObject.SetActive(value: true);
		background.localScale = new Vector3(0f, 0f, 0f);
		background.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
		panel.DOFade(0.6f, 0.5f);
	}

	public override void onExit()
	{
		base.onExit();
		onDelegate();
		background.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
		{
			base.gameObject.SetActive(value: false);
		});
		panel.DOFade(0f, 0.5f);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}
}
