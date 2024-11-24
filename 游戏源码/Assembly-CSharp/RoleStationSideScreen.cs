using System;
using UnityEngine;

// Token: 0x02001FBF RID: 8127
public class RoleStationSideScreen : SideScreenContent
{
	// Token: 0x0600ABE9 RID: 44009 RVA: 0x0010D160 File Offset: 0x0010B360
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600ABEA RID: 44010 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x04008718 RID: 34584
	public GameObject content;

	// Token: 0x04008719 RID: 34585
	private GameObject target;

	// Token: 0x0400871A RID: 34586
	public LocText DescriptionText;
}
