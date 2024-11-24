using System;
using UnityEngine;

// Token: 0x02001FC0 RID: 8128
public class SealedDoorSideScreen : SideScreenContent
{
	// Token: 0x0600ABEC RID: 44012 RVA: 0x0010FB65 File Offset: 0x0010DD65
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += delegate()
		{
			this.target.OrderUnseal();
		};
		this.Refresh();
	}

	// Token: 0x0600ABED RID: 44013 RVA: 0x0010E2E4 File Offset: 0x0010C4E4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x0600ABEE RID: 44014 RVA: 0x0040C30C File Offset: 0x0040A50C
	public override void SetTarget(GameObject target)
	{
		Door component = target.GetComponent<Door>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a Door associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x0600ABEF RID: 44015 RVA: 0x0010FB8A File Offset: 0x0010DD8A
	private void Refresh()
	{
		if (!this.target.isSealed)
		{
			this.ContentContainer.SetActive(false);
			return;
		}
		this.ContentContainer.SetActive(true);
	}

	// Token: 0x0400871B RID: 34587
	[SerializeField]
	private LocText label;

	// Token: 0x0400871C RID: 34588
	[SerializeField]
	private KButton button;

	// Token: 0x0400871D RID: 34589
	[SerializeField]
	private Door target;
}
