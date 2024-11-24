using System;
using UnityEngine;

// Token: 0x02002025 RID: 8229
public abstract class TargetPanel : KMonoBehaviour
{
	// Token: 0x0600AF26 RID: 44838
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600AF27 RID: 44839 RVA: 0x0041EC18 File Offset: 0x0041CE18
	public virtual void SetTarget(GameObject target)
	{
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

	// Token: 0x0600AF28 RID: 44840 RVA: 0x00111DC9 File Offset: 0x0010FFC9
	protected virtual void OnSelectTarget(GameObject target)
	{
		target.Subscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00111DE3 File Offset: 0x0010FFE3
	public virtual void OnDeselectTarget(GameObject target)
	{
		target.Unsubscribe(1502190696, new Action<object>(this.OnTargetDestroyed));
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x00111DFC File Offset: 0x0010FFFC
	private void OnTargetDestroyed(object data)
	{
		DetailsScreen.Instance.Show(false);
		this.SetTarget(null);
	}

	// Token: 0x040089EE RID: 35310
	protected GameObject selectedTarget;
}
