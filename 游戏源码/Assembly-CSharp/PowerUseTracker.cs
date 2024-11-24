using System;
using UnityEngine;

// Token: 0x02000B45 RID: 2885
public class PowerUseTracker : WorldTracker
{
	// Token: 0x060036C5 RID: 14021 RVA: 0x000C3935 File Offset: 0x000C1B35
	public PowerUseTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x0021496C File Offset: 0x00212B6C
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
					num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(num2));
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x000C39C3 File Offset: 0x000C1BC3
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
	}
}
