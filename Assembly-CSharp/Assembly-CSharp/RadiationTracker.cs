using System;
using System.Collections.Generic;
using Klei.AI;

public class RadiationTracker : WorldTracker
{
	public RadiationTracker(int worldID) : base(worldID)
	{
	}

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

	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}
}
