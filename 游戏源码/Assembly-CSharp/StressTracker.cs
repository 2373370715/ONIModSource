using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B47 RID: 2887
public class StressTracker : WorldTracker
{
	// Token: 0x060036CB RID: 14027 RVA: 0x000C3935 File Offset: 0x000C1B35
	public StressTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00214B38 File Offset: 0x00212D38
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			if (Components.LiveMinionIdentities[i].GetMyWorldId() == base.WorldID)
			{
				num = Mathf.Max(num, Components.LiveMinionIdentities[i].gameObject.GetAmounts().GetValue(Db.Get().Amounts.Stress.Id));
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x000C398F File Offset: 0x000C1B8F
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
