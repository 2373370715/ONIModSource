using System;
using System.Collections.Generic;
using Klei.AI;

// Token: 0x02000B4B RID: 2891
public class RadiationTracker : WorldTracker
{
	// Token: 0x060036D7 RID: 14039 RVA: 0x000C3935 File Offset: 0x000C1B35
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x00214C30 File Offset: 0x00212E30
	public override void UpdateData()
	{
		float num = 0f;
		List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(base.WorldID, false);
		if (worldItems.Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		foreach (MinionIdentity cmp in worldItems)
		{
			num += cmp.GetAmounts().Get(Db.Get().Amounts.RadiationBalance.Id).value;
		}
		float value = num / (float)worldItems.Count;
		base.AddPoint(value);
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x000C3A2D File Offset: 0x000C1C2D
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
