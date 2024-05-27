using UnityEngine;
using UnityEngine.Events;

namespace Percent.Event
{
	public class XButtonEventBinder : ButtonEventBinder
	{
		protected override UnityAction onFindMethod()
		{
			return GameObject.Find("CrossPromotion").GetComponent<CrossPromotionUIEventHandler>().onHide;
		}
	}
}
