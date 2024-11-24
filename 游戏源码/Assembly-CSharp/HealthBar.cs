using System;
using UnityEngine;

// Token: 0x02001CF7 RID: 7415
public class HealthBar : ProgressBar
{
	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x06009ACE RID: 39630 RVA: 0x00104ACE File Offset: 0x00102CCE
	private bool ShouldShow
	{
		get
		{
			return this.showTimer > 0f || base.PercentFull < this.alwaysShowThreshold;
		}
	}

	// Token: 0x06009ACF RID: 39631 RVA: 0x00104AED File Offset: 0x00102CED
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
		base.gameObject.SetActive(this.ShouldShow);
	}

	// Token: 0x06009AD0 RID: 39632 RVA: 0x00104B1B File Offset: 0x00102D1B
	public void OnChange()
	{
		base.enabled = true;
		this.showTimer = this.maxShowTime;
	}

	// Token: 0x06009AD1 RID: 39633 RVA: 0x003BC464 File Offset: 0x003BA664
	public override void Update()
	{
		base.Update();
		if (Time.timeScale > 0f)
		{
			this.showTimer = Mathf.Max(0f, this.showTimer - Time.unscaledDeltaTime);
		}
		if (!this.ShouldShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06009AD2 RID: 39634 RVA: 0x00104B30 File Offset: 0x00102D30
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06009AD3 RID: 39635 RVA: 0x00104B39 File Offset: 0x00102D39
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06009AD4 RID: 39636 RVA: 0x003BC4B4 File Offset: 0x003BA6B4
	public override void OnOverlayChanged(object data = null)
	{
		if (!this.autoHide)
		{
			return;
		}
		if ((HashedString)data == OverlayModes.None.ID)
		{
			if (!base.gameObject.activeSelf && this.ShouldShow)
			{
				base.enabled = true;
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0400790D RID: 30989
	private float showTimer;

	// Token: 0x0400790E RID: 30990
	private float maxShowTime = 10f;

	// Token: 0x0400790F RID: 30991
	private float alwaysShowThreshold = 0.8f;
}
