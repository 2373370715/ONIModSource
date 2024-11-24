using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001BF4 RID: 7156
public class EasingAnimations : MonoBehaviour
{
	// Token: 0x170009AE RID: 2478
	// (get) Token: 0x060094B1 RID: 38065 RVA: 0x00100D63 File Offset: 0x000FEF63
	public bool IsPlaying
	{
		get
		{
			return this.animationCoroutine != null;
		}
	}

	// Token: 0x060094B2 RID: 38066 RVA: 0x00100D6E File Offset: 0x000FEF6E
	private void Start()
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
	}

	// Token: 0x060094B3 RID: 38067 RVA: 0x00396650 File Offset: 0x00394850
	private void Initialize()
	{
		this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
		foreach (EasingAnimations.AnimationScales animationScales in this.scales)
		{
			this.animationMap.Add(animationScales.name, animationScales);
		}
	}

	// Token: 0x060094B4 RID: 38068 RVA: 0x00396698 File Offset: 0x00394898
	public void PlayAnimation(string animationName, float delay = 0f)
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
		if (!this.animationMap.ContainsKey(animationName))
		{
			return;
		}
		if (this.animationCoroutine != null)
		{
			base.StopCoroutine(this.animationCoroutine);
		}
		this.currentAnimation = this.animationMap[animationName];
		this.currentAnimation.currentScale = this.currentAnimation.startScale;
		base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
		this.animationCoroutine = base.StartCoroutine(this.ExecuteAnimation(delay));
	}

	// Token: 0x060094B5 RID: 38069 RVA: 0x00100D8B File Offset: 0x000FEF8B
	private IEnumerator ExecuteAnimation(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + delay)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		startTime = Time.realtimeSinceStartup;
		bool keepAnimating = true;
		while (keepAnimating)
		{
			float num = Time.realtimeSinceStartup - startTime;
			this.currentAnimation.currentScale = this.GetEasing(num * this.currentAnimation.easingMultiplier);
			if (this.currentAnimation.endScale > this.currentAnimation.startScale)
			{
				keepAnimating = (this.currentAnimation.currentScale < this.currentAnimation.endScale - 0.025f);
			}
			else
			{
				keepAnimating = (this.currentAnimation.currentScale > this.currentAnimation.endScale + 0.025f);
			}
			if (!keepAnimating)
			{
				this.currentAnimation.currentScale = this.currentAnimation.endScale;
			}
			base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.animationCoroutine = null;
		if (this.OnAnimationDone != null)
		{
			this.OnAnimationDone(this.currentAnimation.name);
		}
		yield break;
	}

	// Token: 0x060094B6 RID: 38070 RVA: 0x00396740 File Offset: 0x00394940
	private float GetEasing(float t)
	{
		EasingAnimations.AnimationScales.AnimationType type = this.currentAnimation.type;
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseOutBack)
		{
			return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseInBack)
		{
			return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
	}

	// Token: 0x060094B7 RID: 38071 RVA: 0x003967BC File Offset: 0x003949BC
	public float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060094B8 RID: 38072 RVA: 0x00396838 File Offset: 0x00394A38
	public float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060094B9 RID: 38073 RVA: 0x0039686C File Offset: 0x00394A6C
	public float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x04007322 RID: 29474
	public EasingAnimations.AnimationScales[] scales;

	// Token: 0x04007323 RID: 29475
	private EasingAnimations.AnimationScales currentAnimation;

	// Token: 0x04007324 RID: 29476
	private Coroutine animationCoroutine;

	// Token: 0x04007325 RID: 29477
	private Dictionary<string, EasingAnimations.AnimationScales> animationMap;

	// Token: 0x04007326 RID: 29478
	public Action<string> OnAnimationDone;

	// Token: 0x02001BF5 RID: 7157
	[Serializable]
	public struct AnimationScales
	{
		// Token: 0x04007327 RID: 29479
		public string name;

		// Token: 0x04007328 RID: 29480
		public float startScale;

		// Token: 0x04007329 RID: 29481
		public float endScale;

		// Token: 0x0400732A RID: 29482
		public EasingAnimations.AnimationScales.AnimationType type;

		// Token: 0x0400732B RID: 29483
		public float easingMultiplier;

		// Token: 0x0400732C RID: 29484
		[HideInInspector]
		public float currentScale;

		// Token: 0x02001BF6 RID: 7158
		public enum AnimationType
		{
			// Token: 0x0400732E RID: 29486
			EaseInOutBack,
			// Token: 0x0400732F RID: 29487
			EaseOutBack,
			// Token: 0x04007330 RID: 29488
			EaseInBack
		}
	}
}
