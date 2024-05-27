using Percent.Tween;
using System;

namespace Percent.View
{
	public class BackgroundView : View
	{
		public PromotionType type;

		private PercentTween tweener;

		protected override void Awake()
		{
			base.Awake();
			tweener = GetComponent<ColorTween>();
		}

		internal override void onViewStateChange(ViewLifeCycle.Status state)
		{
			switch (state)
			{
			case ViewLifeCycle.Status.INIT:
				render.raycastTarget = false;
				break;
			case ViewLifeCycle.Status.LOAD_SUCCESS:
				if (!PromotionData.crossPromotionData.type.Equals(type))
				{
					ViewLifeCycle viewLifeCycle2 = base.viewLifeCycle;
					viewLifeCycle2.onStateChange = (ViewLifeCycle.OnStateChange)Delegate.Remove(viewLifeCycle2.onStateChange, new ViewLifeCycle.OnStateChange(((View)this).onViewStateChange));
					base.gameObject.SetActive(value: false);
				}
				break;
			case ViewLifeCycle.Status.LOAD_FAIL:
			{
				ViewLifeCycle viewLifeCycle = base.viewLifeCycle;
				viewLifeCycle.onStateChange = (ViewLifeCycle.OnStateChange)Delegate.Remove(viewLifeCycle.onStateChange, new ViewLifeCycle.OnStateChange(((View)this).onViewStateChange));
				base.gameObject.SetActive(value: false);
				break;
			}
			case ViewLifeCycle.Status.SHOW:
				render.raycastTarget = true;
				tweener.play();
				break;
			case ViewLifeCycle.Status.HIDE:
				render.raycastTarget = false;
				tweener.playReverse();
				break;
			}
		}
	}
}
