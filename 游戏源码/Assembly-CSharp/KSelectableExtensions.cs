using System;
using UnityEngine;

// Token: 0x02000A76 RID: 2678
public static class KSelectableExtensions
{
	// Token: 0x0600317D RID: 12669 RVA: 0x000C029A File Offset: 0x000BE49A
	public static string GetProperName(this Component cmp)
	{
		if (cmp != null && cmp.gameObject != null)
		{
			return cmp.gameObject.GetProperName();
		}
		return "";
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x001FF274 File Offset: 0x001FD474
	public static string GetProperName(this GameObject go)
	{
		if (go != null)
		{
			KSelectable component = go.GetComponent<KSelectable>();
			if (component != null)
			{
				return component.GetName();
			}
		}
		return "";
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x000C02C4 File Offset: 0x000BE4C4
	public static string GetProperName(this KSelectable cmp)
	{
		if (cmp != null)
		{
			return cmp.GetName();
		}
		return "";
	}
}
