using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001BF8 RID: 7160
public class ExpandRevealUIContent : MonoBehaviour
{
	// Token: 0x060094C1 RID: 38081 RVA: 0x00396A48 File Offset: 0x00394C48
	private void OnDisable()
	{
		if (this.BGChildFitter)
		{
			this.BGChildFitter.WidthScale = (this.BGChildFitter.HeightScale = 0f);
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = 0f;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = 0f;
			}
		}
		if (this.BGRectStretcher)
		{
			this.BGRectStretcher.XStretchFactor = (this.BGRectStretcher.YStretchFactor = 0f);
			this.BGRectStretcher.UpdateStretching();
		}
		if (this.MaskRectStretcher)
		{
			this.MaskRectStretcher.XStretchFactor = (this.MaskRectStretcher.YStretchFactor = 0f);
			this.MaskRectStretcher.UpdateStretching();
		}
	}

	// Token: 0x060094C2 RID: 38082 RVA: 0x00396B34 File Offset: 0x00394D34
	public void Expand(Action<object> completeCallback)
	{
		if (this.MaskChildFitter && this.MaskRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a MaskChildFitter and a MaskRectStretcher. It should have only one or the other. ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.BGChildFitter && this.BGRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a BGChildFitter and a BGRectStretcher . It should have only one or the other.  ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.activeRoutine != null)
		{
			base.StopCoroutine(this.activeRoutine);
		}
		this.CollapsedImmediate();
		this.activeRoutineCompleteCallback = completeCallback;
		this.activeRoutine = base.StartCoroutine(this.ExpandRoutine(null));
	}

	// Token: 0x060094C3 RID: 38083 RVA: 0x00396BC0 File Offset: 0x00394DC0
	public void Collapse(Action<object> completeCallback)
	{
		if (this.activeRoutine != null)
		{
			if (this.activeRoutineCompleteCallback != null)
			{
				this.activeRoutineCompleteCallback(null);
			}
			base.StopCoroutine(this.activeRoutine);
		}
		this.activeRoutineCompleteCallback = completeCallback;
		if (base.gameObject.activeInHierarchy)
		{
			this.activeRoutine = base.StartCoroutine(this.CollapseRoutine(completeCallback));
			return;
		}
		this.activeRoutine = null;
		if (completeCallback != null)
		{
			completeCallback(null);
		}
	}

	// Token: 0x060094C4 RID: 38084 RVA: 0x00100DB8 File Offset: 0x000FEFB8
	private IEnumerator ExpandRoutine(Action<object> completeCallback)
	{
		this.Collapsing = false;
		this.Expanding = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.expandAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num / this.speedScale;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime * this.speedScale)
		{
			this.SetStretch(this.expandAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.expandAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Expanding = false;
		yield break;
	}

	// Token: 0x060094C5 RID: 38085 RVA: 0x00396C30 File Offset: 0x00394E30
	private void SetStretch(float value)
	{
		if (this.BGRectStretcher)
		{
			if (this.BGRectStretcher.StretchX)
			{
				this.BGRectStretcher.XStretchFactor = value;
			}
			if (this.BGRectStretcher.StretchY)
			{
				this.BGRectStretcher.YStretchFactor = value;
			}
		}
		if (this.MaskRectStretcher)
		{
			if (this.MaskRectStretcher.StretchX)
			{
				this.MaskRectStretcher.XStretchFactor = value;
			}
			if (this.MaskRectStretcher.StretchY)
			{
				this.MaskRectStretcher.YStretchFactor = value;
			}
		}
		if (this.BGChildFitter)
		{
			if (this.BGChildFitter.fitWidth)
			{
				this.BGChildFitter.WidthScale = value;
			}
			if (this.BGChildFitter.fitHeight)
			{
				this.BGChildFitter.HeightScale = value;
			}
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = value;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = value;
			}
		}
	}

	// Token: 0x060094C6 RID: 38086 RVA: 0x00100DCE File Offset: 0x000FEFCE
	private IEnumerator CollapseRoutine(Action<object> completeCallback)
	{
		this.Expanding = false;
		this.Collapsing = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.collapseAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime)
		{
			this.SetStretch(this.collapseAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.collapseAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Collapsing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060094C7 RID: 38087 RVA: 0x00396D3C File Offset: 0x00394F3C
	public void CollapsedImmediate()
	{
		float time = (float)this.collapseAnimation.length;
		this.SetStretch(this.collapseAnimation.Evaluate(time));
	}

	// Token: 0x04007337 RID: 29495
	private Coroutine activeRoutine;

	// Token: 0x04007338 RID: 29496
	private Action<object> activeRoutineCompleteCallback;

	// Token: 0x04007339 RID: 29497
	public AnimationCurve expandAnimation;

	// Token: 0x0400733A RID: 29498
	public AnimationCurve collapseAnimation;

	// Token: 0x0400733B RID: 29499
	public KRectStretcher MaskRectStretcher;

	// Token: 0x0400733C RID: 29500
	public KRectStretcher BGRectStretcher;

	// Token: 0x0400733D RID: 29501
	public KChildFitter MaskChildFitter;

	// Token: 0x0400733E RID: 29502
	public KChildFitter BGChildFitter;

	// Token: 0x0400733F RID: 29503
	public float speedScale = 1f;

	// Token: 0x04007340 RID: 29504
	public bool Collapsing;

	// Token: 0x04007341 RID: 29505
	public bool Expanding;
}
