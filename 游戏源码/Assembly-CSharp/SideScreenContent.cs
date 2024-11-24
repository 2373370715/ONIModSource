using System;
using UnityEngine;

// Token: 0x02001FCC RID: 8140
public abstract class SideScreenContent : KScreen
{
	// Token: 0x0600AC4D RID: 44109 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void SetTarget(GameObject target)
	{
	}

	// Token: 0x0600AC4E RID: 44110 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ClearTarget()
	{
	}

	// Token: 0x0600AC4F RID: 44111
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600AC50 RID: 44112 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x0600AC51 RID: 44113 RVA: 0x00110043 File Offset: 0x0010E243
	public virtual string GetTitle()
	{
		return Strings.Get(this.titleKey);
	}

	// Token: 0x04008767 RID: 34663
	[SerializeField]
	protected string titleKey;

	// Token: 0x04008768 RID: 34664
	public GameObject ContentContainer;
}
