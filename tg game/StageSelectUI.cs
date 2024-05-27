using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : BaseUI
{
	private float numberDistance = 130f;

	public Transform background;

	public Image panel;

	public GameObject leftButton;

	public GameObject rightButton;

	public List<TextMeshProUGUI> listNumbers = new List<TextMeshProUGUI>();

	private int nowIndex;

	private bool setting;

	private void settingNumberIndex(int index, bool action)
	{
		int num = index - 1;
		int count = listNumbers.Count;
		for (int i = 0; i < count; i++)
		{
			int num2 = i - num;
			int num3 = Mathf.Abs(num2);
			Vector3 vector = new Vector3((float)num2 * numberDistance, 76f, 0f);
			float num4 = 1f - (float)num3 * 0.2f;
			float num5 = 1f - (float)num3 * 0.35f;
			if (num4 < 0f)
			{
				num4 = 0f;
			}
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			TextMeshProUGUI textMeshProUGUI = listNumbers[i];
			if (action)
			{
				textMeshProUGUI.transform.DOKill();
				textMeshProUGUI.DOKill();
				textMeshProUGUI.transform.DOLocalMove(vector, 0.2f);
				textMeshProUGUI.transform.DOScale(new Vector3(num4, num4, num4), 0.2f);
				textMeshProUGUI.DOFade(num5, 0.2f);
			}
			else
			{
				textMeshProUGUI.transform.localPosition = vector;
				textMeshProUGUI.transform.localScale = new Vector3(num4, num4, num4);
				textMeshProUGUI.color = new Color(1f, 1f, 1f, num5);
			}
		}
	}

	public override void onStart()
	{
		base.onStart();
		base.gameObject.SetActive(value: true);
		nowIndex = Singleton<DataManager>.Instance.selectLevel;
		settingNumberIndex(nowIndex, action: false);
		checkButton();
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

	public void onUp()
	{
		int count = listNumbers.Count;
		nowIndex++;
		if (nowIndex > count)
		{
			nowIndex = count;
		}
		settingNumberIndex(nowIndex, action: true);
		checkButton();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onDown()
	{
		int count = listNumbers.Count;
		nowIndex--;
		if (nowIndex < 1)
		{
			nowIndex = 1;
		}
		settingNumberIndex(nowIndex, action: true);
		checkButton();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	private void checkButton()
	{
		leftButton.SetActive(nowIndex != 1);
		rightButton.SetActive(nowIndex != listNumbers.Count);
	}

	public void onSelect()
	{
		Singleton<DataManager>.Instance.selectLevel = nowIndex;
		onDelegate();
		onExit();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}
}
