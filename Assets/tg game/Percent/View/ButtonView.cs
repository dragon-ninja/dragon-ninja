using Percent.Tween;
using System;
using UnityEngine.UI;

namespace Percent.View
{
	public class ButtonView : ResponsiveView
	{
		public PromotionType type;

		private PercentTween showScaleTweener;

		private PercentTween showColorTweener;

		protected override void Awake()
		{
			base.Awake();
			render = base.transform.parent.GetComponent<RawImage>();
			showScaleTweener = GetComponent<ScaleTween>();
			showColorTweener = GetComponent<ColorTween>();
		}

		internal override void onViewStateChange(ViewLifeCycle.Status state)
		{
			switch (state)
			{
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
				showScaleTweener.play();
				showColorTweener.play();
				render.raycastTarget = true;
				break;
			case ViewLifeCycle.Status.HIDE:
				showScaleTweener.startDelay = 0f;
				showScaleTweener.onEnd = onAnimationEnd;
				showScaleTweener.playReverse();
				showColorTweener.startDelay = 0f;
				showColorTweener.playReverse();
				render.raycastTarget = false;
				break;
			}
		}

		public void onAnimationEnd()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
