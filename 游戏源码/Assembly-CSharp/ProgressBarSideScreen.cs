using System;
using UnityEngine;

// Token: 0x02001FAA RID: 8106
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x0600AB4E RID: 43854 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600AB4F RID: 43855 RVA: 0x0010F403 File Offset: 0x0010D603
	public override int GetSideScreenSortOrder()
	{
		return -10;
	}

	// Token: 0x0600AB50 RID: 43856 RVA: 0x0010F407 File Offset: 0x0010D607
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IProgressBarSideScreen>() != null;
	}

	// Token: 0x0600AB51 RID: 43857 RVA: 0x0010F412 File Offset: 0x0010D612
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetObject = target.GetComponent<IProgressBarSideScreen>();
		this.RefreshBar();
	}

	// Token: 0x0600AB52 RID: 43858 RVA: 0x00409D24 File Offset: 0x00407F24
	private void RefreshBar()
	{
		this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
		this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
		this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
		this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
		this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
	}

	// Token: 0x0600AB53 RID: 43859 RVA: 0x0010F42D File Offset: 0x0010D62D
	public void Render1000ms(float dt)
	{
		this.RefreshBar();
	}

	// Token: 0x040086A6 RID: 34470
	public LocText label;

	// Token: 0x040086A7 RID: 34471
	public GenericUIProgressBar progressBar;

	// Token: 0x040086A8 RID: 34472
	public IProgressBarSideScreen targetObject;
}
