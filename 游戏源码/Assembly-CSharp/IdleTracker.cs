using System;
using System.Collections.Generic;

// Token: 0x02000B4A RID: 2890
public class IdleTracker : WorldTracker
{
	// Token: 0x060036D4 RID: 14036 RVA: 0x000C3935 File Offset: 0x000C1B35
	public IdleTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x00214BBC File Offset: 0x00212DBC
	public override void UpdateData()
	{
		this.objectsOfInterest.Clear();
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
				this.objectsOfInterest.Add(worldItems[i].gameObject);
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x000C20D4 File Offset: 0x000C02D4
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
