using System;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class BatteryTracker : WorldTracker
{
	// Token: 0x060036C8 RID: 14024 RVA: 0x000C3935 File Offset: 0x000C1B35
	public BatteryTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x00214A2C File Offset: 0x00212C2C
	public override void UpdateData()
	{
		float num = 0f;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num2] == base.WorldID)
				{
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num2);
					foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
					{
						num += battery.JoulesAvailable;
					}
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x000C39CD File Offset: 0x000C1BCD
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
