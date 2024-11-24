using System;
using System.Collections.Generic;

// Token: 0x02000B4E RID: 2894
public class WorkingToiletTracker : WorldTracker
{
	// Token: 0x060036E0 RID: 14048 RVA: 0x000C3935 File Offset: 0x000C1B35
	public WorkingToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x00214D68 File Offset: 0x00212F68
	public override void UpdateData()
	{
		int num = 0;
		using (IEnumerator<IUsable> enumerator = Components.Toilets.WorldItemsEnumerate(base.WorldID, true).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsUsable())
				{
					num++;
				}
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x000C20D4 File Offset: 0x000C02D4
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
