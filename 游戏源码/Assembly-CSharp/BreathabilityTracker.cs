using System;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
public class BreathabilityTracker : WorldTracker
{
	// Token: 0x060036BA RID: 14010 RVA: 0x000C3935 File Offset: 0x000C1B35
	public BreathabilityTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00214848 File Offset: 0x00212A48
	public override void UpdateData()
	{
		float num = 0f;
		if (Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false).Count == 0)
		{
			base.AddPoint(0f);
			return;
		}
		int num2 = 0;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.WorldID, false))
		{
			OxygenBreather component = minionIdentity.GetComponent<OxygenBreather>();
			if (!(component == null))
			{
				OxygenBreather.IGasProvider gasProvider = component.GetGasProvider();
				num2++;
				if (!component.IsSuffocating)
				{
					num += 100f;
					if (gasProvider.IsLowOxygen())
					{
						num -= 50f;
					}
				}
			}
		}
		num /= (float)num2;
		base.AddPoint((float)Mathf.RoundToInt(num));
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x000C398F File Offset: 0x000C1B8F
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
