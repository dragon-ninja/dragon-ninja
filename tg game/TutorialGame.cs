using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGame : MonoBehaviour
{
	public GameObject canvas1;

	public Image finger1;

	public GameObject text1;

	public GameObject canvas2;

	public Image finger2;

	public GameObject text2;

	public GameObject panel;

	private void fingerAction(Image finger, float moveX)
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(finger.DOFade(1f, 0.5f));
		sequence.Append(finger.transform.DOLocalMoveX(moveX, 1f).SetDelay(0.5f).SetRelative(isRelative: true)
			.SetEase(Ease.OutExpo));
		sequence.Append(finger.DOFade(0f, 0.5f));
		sequence.Append(finger.transform.DOLocalMoveX(0f - moveX, 0f).SetRelative(isRelative: true));
		sequence.SetLoops(-1);
		sequence.Play();
	}

	public void onAttach1()
	{
		panel.SetActive(value: true);
		panel.transform.DOMoveX(0f, 0.6f).SetRelative(isRelative: true).OnComplete(delegate
		{
			panel.SetActive(value: false);
			canvas1.SetActive(value: true);
			fingerAction(finger1, -176f);
			TutorialManager.fingerAction(text1);
		});
	}

	public void offAttach1()
	{
		canvas1.SetActive(value: false);
	}

	public void onAttach2()
	{
		panel.SetActive(value: true);
		panel.transform.DOMoveX(0f, 1f).SetRelative(isRelative: true).OnComplete(delegate
		{
			panel.SetActive(value: false);
			canvas2.SetActive(value: true);
			fingerAction(finger2, -352f);
			TutorialManager.fingerAction(text2);
		});
	}

	public void offAttach2()
	{
		canvas2.SetActive(value: false);
	}
}
