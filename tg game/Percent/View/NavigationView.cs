using Percent.Tween;
using System;
using UnityEngine;

namespace Percent.View
{
	public class NavigationView : View
	{
		internal int index;

		private PercentTween showScaleTweener;

		private PercentTween showColorTweener;

		private PercentTween slideTweener;

		internal readonly int TERM = 82;

		private static int instanceNum;

		protected override void Awake()
		{
			base.Awake();
			index = ++instanceNum;
		}

		protected override void Start()
		{
			base.Start();
			initialize();
		}

		private void initialize()
		{
			initScrollView();
			initAnimation();
		}

		private void initScrollView()
		{
			CenterOnChild component = GameObject.Find("TypeSlide").GetComponent<CenterOnChild>();
			component.onChangeCenterContent = (CenterOnChild.OnChangeCenterContent)Delegate.Combine(component.onChangeCenterContent, new CenterOnChild.OnChangeCenterContent(onChangeCenterContent));
			thisTrans.SetParent(GameObject.Find("Navigation").transform);
			thisTrans.localPosition = getPosOfIndex();
		}

		private Vector2 getPosOfIndex()
		{
			int num = -809;
			return new Vector2((PromotionData.crossPromotionData.resourceUrl.Length - 1) * TERM / 2 - (index - 1) * TERM, num);
		}

		private void initAnimation()
		{
			showScaleTweener = GetComponent<ScaleTween>();
			PercentTween[] components = GetComponents<ColorTween>();
			PercentTween[] array = components;
			showColorTweener = array[0];
			slideTweener = array[1];
			setShowTweenSequece();
		}

		private void setShowTweenSequece()
		{
			float num = 0.1f;
			int num2 = PromotionData.crossPromotionData.resourceUrl.Length;
			float startDelay = 0.7f + (float)(num2 - index) * num;
			showScaleTweener.startDelay = startDelay;
			showColorTweener.startDelay = startDelay;
		}

		internal void onChangeCenterContent(int indexOfCenter, int indexOfLastCenter)
		{
			int obj = -1;
			if (!indexOfLastCenter.Equals(obj))
			{
				if (indexOfCenter.Equals(index))
				{
					slideTweener.play();
				}
				else if (indexOfLastCenter.Equals(index))
				{
					slideTweener.playReverse();
				}
			}
		}

		internal override void onViewStateChange(ViewLifeCycle.Status state)
		{
			switch (state)
			{
			case ViewLifeCycle.Status.LOAD_SUCCESS:
				if (!PromotionData.crossPromotionData.type.Equals(PromotionType.SLIDE))
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
				if (showScaleTweener == null)
				{
					initAnimation();
				}
				showScaleTweener.play();
				if (isFirstNavigation())
				{
					showColorTweener.onEnd = onShowAnimationEnd;
				}
				showColorTweener.play();
				break;
			case ViewLifeCycle.Status.HIDE:
				showScaleTweener.startDelay = 0f;
				showScaleTweener.playReverse();
				showColorTweener.startDelay = 0f;
				break;
			}
		}

		private bool isFirstNavigation()
		{
			int obj = PromotionData.crossPromotionData.resourceUrl.Length;
			if (index.Equals(obj))
			{
				return true;
			}
			return false;
		}

		public void onShowAnimationEnd()
		{
			render.color = Color.white;
		}
	}
}
