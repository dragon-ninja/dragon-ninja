using UnityEngine;

public class BaseUI : MonoBehaviour
{
	private bool openState;

	protected UICallbackDelegate callback;

	public void setDelegate(UICallbackDelegate call)
	{
		callback = call;
	}

	public void onDelegate()
	{
		if (callback != null)
		{
			callback();
		}
	}

	public virtual void onStart()
	{
		if (!openState)
		{
			Singleton<UIControlManager>.Instance.addOpenUI(this);
			Singleton<UIControlManager>.Instance.uiOpenCount();
			openState = true;
		}
	}

	public virtual void onExit()
	{
		if (openState)
		{
			Singleton<UIControlManager>.Instance.closeUI(this);
			Singleton<UIControlManager>.Instance.uiCloseCount();
			openState = false;
		}
	}
}
