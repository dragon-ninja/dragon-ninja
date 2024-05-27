using UnityEngine;
using UnityEngine.Events;

namespace Percent.Event
{
	public class CheckButtonEventBinder : ButtonEventBinder
	{
		public PromotionType type;

		protected override UnityAction onFindMethod()
		{
			CrossPromotionUIEventHandler component = GameObject.Find("CrossPromotion").GetComponent<CrossPromotionUIEventHandler>();
			UnityAction result = null;
			switch (type)
			{
			case PromotionType.IMAGE:
				result = component.onImageTypeClick;
				break;
			case PromotionType.SLIDE:
				result = component.onSlideTypeClick;
				break;
			}
			return result;
		}
	}
}
