using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
	public Image imageClear;

	public TextMeshProUGUI textTimer;

	public Image textBackground;

	public RectTransform buttonInventory;

	public RectTransform buttonHome;

	private UICallbackDelegate callback;

	private UICallbackDelegate endedCallback;

	private int timerCount = 60;

	private Vector3 buttonInventoryPosition;

	private Vector3 buttonHomePosition;

	private SoundManager soundManager;

	private void Start()
	{
		soundManager = Singleton<SoundManager>.Instance;
		buttonInventoryPosition = buttonInventory.localPosition;
		buttonHomePosition = buttonHome.localPosition;
		buttonInventory.localPosition = new Vector3(buttonInventoryPosition.x + 288f, buttonInventoryPosition.y, buttonInventoryPosition.z);
		buttonHome.localPosition = new Vector3(buttonHomePosition.x + 288f, buttonHomePosition.y, buttonHomePosition.z);
		textTimer.transform.localScale = new Vector3(0f, 0f, 0f);
		textBackground.color = new Color(1f, 1f, 1f, 0f);
		imageClear.color = new Color(1f, 1f, 1f, 0f);
	}

	public void onGameClear(UICallbackDelegate call, UICallbackDelegate ended)
	{
		soundManager.playSound("win");
		Sequence sequence = DOTween.Sequence();
		sequence.Append(imageClear.DOFade(1f, 0.2f));
		sequence.Append(imageClear.DOFade(0f, 1f).SetDelay(0.7f));
		sequence.OnComplete(delegate
		{
			onTimer();
		});
		sequence.Play();
		callback = call;
		endedCallback = ended;
	}

	private void onTimer()
	{
		textTimer.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
		textBackground.DOFade(1f, 0.5f);
		buttonInventory.DOLocalMove(buttonInventoryPosition, 0.5f).SetEase(Ease.OutBack);
		buttonHome.DOLocalMove(buttonHomePosition, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f);
		StartCoroutine(updateTimer());
	}

	private IEnumerator updateTimer()
	{
		do
		{
			yield return new WaitForSeconds(1f);
			timerCount--;
			textTimer.text = timerCount.ToString();
			soundManager.playSound("timer");
		}
		while (timerCount > 0);
		endedTimer();
	}

	private void endedTimer()
	{
		endedCallback();
	}

	public void onInventory()
	{
		Singleton<UIControlManager>.Instance.onInventoryUI(callback);
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}

	public void onGoHome()
	{
		endedCallback();
		Singleton<SoundManager>.Instance.playSound("uiClick");
	}
}
