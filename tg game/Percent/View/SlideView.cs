using Percent.Http;
using Percent.Tween;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Percent.View
{
	public class SlideView : View
	{
		internal int index;

		internal static readonly int TERM = 1200;

		private static int instanceNum;

		private PercentTween showScaleTweener;

		private PercentTween showColorTweener;

		private readonly Vector2 VIEW_SIZE = new Vector2(1000f, 1500f);

		protected override void Awake()
		{
			base.Awake();
			index = ++instanceNum;
		}

		protected override void Start()
		{
			base.Start();
			initialize();
			renderTexture();
		}

		private void initialize()
		{
			initScrollView();
			initAnimation();
			initButton();
		}

		private void initScrollView()
		{
			CenterOnChild component = GameObject.Find("TypeSlide").GetComponent<CenterOnChild>();
			component.onChangeCenterContent = (CenterOnChild.OnChangeCenterContent)Delegate.Combine(component.onChangeCenterContent, new CenterOnChild.OnChangeCenterContent(onChangeCenterContent));
			thisTrans.SetParent(GameObject.Find("Content").transform);
			thisTrans.sizeDelta = VIEW_SIZE;
			thisTrans.localPosition = getPosOfIndex();
		}

		private Vector2 getPosOfIndex()
		{
			int num = 0;
			return new Vector2((PromotionData.crossPromotionData.resourceUrl.Length - 1) * TERM / 2 - (index - 1) * TERM, num);
		}

		internal void onChangeCenterContent(int indexOfCenter, int indexOfLastCenter)
		{
			int obj = -1;
			indexOfLastCenter.Equals(obj);
		}

		private void initAnimation()
		{
			showScaleTweener = GetComponent<ScaleTween>();
			showColorTweener = GetComponents<ColorTween>()[0];
		}

		private void initButton()
		{
			CrossPromotionUIEventHandler component = GameObject.Find("CrossPromotion").GetComponent<CrossPromotionUIEventHandler>();
			GetComponent<Button>().onClick.AddListener(component.onSlideTypeClick);
		}

		private void renderTexture()
		{
			TextureLifeCycle component = GameObject.Find("TextureLifeCycle").GetComponent<TextureLifeCycle>();
			int num = PromotionData.crossPromotionData.resourceUrl.Length;
			string imageUrl = PromotionData.crossPromotionData.resourceUrl[num - index];
			GetComponent<TextureLoader>().render(imageUrl, component.onEachTextureLoad);
		}

		internal override void onViewStateChange(ViewLifeCycle.Status state)
		{
			switch (state)
			{
			case ViewLifeCycle.Status.LOAD_SUCCESS:
				if (!PromotionData.crossPromotionData.type.Equals(PromotionType.SLIDE))
				{
					ViewLifeCycle viewLifeCycle = base.viewLifeCycle;
					viewLifeCycle.onStateChange = (ViewLifeCycle.OnStateChange)Delegate.Remove(viewLifeCycle.onStateChange, new ViewLifeCycle.OnStateChange(((View)this).onViewStateChange));
					base.gameObject.SetActive(value: false);
				}
				break;
			case ViewLifeCycle.Status.LOAD_FAIL:
			{
				ViewLifeCycle viewLifeCycle2 = base.viewLifeCycle;
				viewLifeCycle2.onStateChange = (ViewLifeCycle.OnStateChange)Delegate.Remove(viewLifeCycle2.onStateChange, new ViewLifeCycle.OnStateChange(((View)this).onViewStateChange));
				base.gameObject.SetActive(value: false);
				break;
			}
			case ViewLifeCycle.Status.SHOW:
			{
				int num = 1;
				GetComponentsInChildren<ScaleTween>()[num].onEnd = onShowAnimationEnd;
				if (showScaleTweener == null)
				{
					initAnimation();
				}
				showScaleTweener.play();
				showColorTweener.play();
				render.raycastTarget = true;
				break;
			}
			case ViewLifeCycle.Status.HIDE:
				showScaleTweener.startDelay = 0f;
				showScaleTweener.onEnd = onHideAnimationEnd;
				showScaleTweener.playReverse();
				showColorTweener.startDelay = 0f;
				render.raycastTarget = false;
				break;
			}
		}

		public void onShowAnimationEnd()
		{
			GetComponent<OverlayCenterXAlphaMax>().startAnimation();
		}

		public void onHideAnimationEnd()
		{
			if ((--instanceNum).Equals(0))
			{
				deactivateWholeView();
			}
		}

		private void deactivateWholeView()
		{
			GameObject.Find("TypeSlide").GetComponent<Hider>().hide();
		}
	}
}
