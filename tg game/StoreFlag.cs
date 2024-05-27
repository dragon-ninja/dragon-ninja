using DG.Tweening;
using UnityEngine;

public class StoreFlag : MonoBehaviour
{
	private UIControlManager uiControlManager;

	private bool openState;

	private Sequence sequence;

	private void Start()
	{
		uiControlManager = Singleton<UIControlManager>.Instance;
	}

	public void onOpen(Transform target)
	{
		base.transform.position = new Vector3(target.position.x - 1f, 4f, 10f);
		if (sequence != null)
		{
			sequence.Kill();
			sequence = null;
		}
		base.transform.DOKill();
		base.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			sequence = DOTween.Sequence();
			sequence.Append(base.transform.DOMoveY(-0.3f, 0.5f).SetRelative(isRelative: true));
			sequence.Append(base.transform.DOMoveY(0.3f, 1f).SetRelative(isRelative: true));
			sequence.SetLoops(-1);
			sequence.SetEase(Ease.OutBounce);
			sequence.Play();
		});
		openState = true;
	}

	public void onClose()
	{
		if (sequence != null)
		{
			sequence.Kill();
			sequence = null;
		}
		base.transform.DOKill();
		base.transform.DOScale(new Vector3(0f, 0f, 1f), 0.2f).SetEase(Ease.InBack);
		openState = false;
	}

	public void onClick(UICallbackDelegate call)
	{
		uiControlManager.onInventoryUI(call);
	}

	public bool flagOpenState()
	{
		return openState;
	}
}
