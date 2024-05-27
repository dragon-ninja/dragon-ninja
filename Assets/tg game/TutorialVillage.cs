using DG.Tweening;
using UnityEngine;

public class TutorialVillage : MonoBehaviour
{
	public GameObject startObject;

	public GameObject arrowObject;

	public GameObject startFinger;

	public GameObject arrowImage;

	private bool arrowOff;

	public void onStartTutorial()
	{
		startObject.SetActive(value: true);
		TutorialManager.fingerAction(startFinger);
	}

	public void onStartTutorialEnd()
	{
		startObject.SetActive(value: false);
		if (!arrowOff)
		{
			arrowObject.SetActive(value: true);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(arrowImage.transform.DOScale(1.2f, 0.2f).SetDelay(0.5f));
			sequence.Append(arrowImage.transform.DOScale(1f, 0.2f));
			sequence.SetLoops(-1);
			sequence.Play();
			arrowOff = true;
		}
	}

	public void onDungeonStart()
	{
		arrowObject.SetActive(value: false);
	}
}
