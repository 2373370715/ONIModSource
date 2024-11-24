using System;
using UnityEngine;

// Token: 0x02002026 RID: 8230
public abstract class TargetScreen : KScreen
{
	// Token: 0x0600AF2C RID: 44844
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600AF2D RID: 44845 RVA: 0x0041EC70 File Offset: 0x0041CE70
	public virtual void SetTarget(GameObject target)
	{
		Console.WriteLine(target);
		if (this.selectedTarget != target)
		{
			if (this.selectedTarget != null)
			{
				this.OnDeselectTarget(this.selectedTarget);
			}
			this.selectedTarget = target;
			if (this.selectedTarget != null)
			{
				this.OnSelectTarget(this.selectedTarget);
			}
		}
	}

	// Token: 0x0600AF2E RID: 44846 RVA: 0x00111E10 File Offset: 0x00110010
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.SetTarget(null);
	}

	// Token: 0x0600AF2F RID: 44847 RVA: 0x00111E1F File Offset: 0x0011001F
	public virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600AF30 RID: 44848 RVA: 0x00111E39 File Offset: 0x00110039
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600AF31 RID: 44849 RVA: 0x00111E52 File Offset: 0x00110052
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x040089EF RID: 35311
	protected GameObject selectedTarget;
}
