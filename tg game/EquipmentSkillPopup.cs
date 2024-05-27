using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSkillPopup : BaseUI
{
	public RectTransform backgroundTransform;

	public Image panel;

	private bool openEnded;

	public void onPopup()
	{
		base.onStart();
		base.gameObject.SetActive(value: true);
		backgroundTransform.localScale = new Vector3(0f, 0f, 0f);
		backgroundTransform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			openEnded = true;
		});
		panel.DOFade(0.6f, 0.5f);
	}

	public void offPopup()
	{
		if (openEnded)
		{
			backgroundTransform.DOScale(0f, 0.5f).SetEase(Ease.InBack).OnComplete(delegate
			{
				base.gameObject.SetActive(value: false);
			});
			panel.DOFade(0f, 0.5f);
		}
	}

	public override void onExit()
	{
		base.onExit();
		onDelegate();
		offPopup();
	}
}
