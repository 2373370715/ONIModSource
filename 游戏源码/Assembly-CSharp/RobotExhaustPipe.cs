using System;
using TUNING;
using UnityEngine;

// Token: 0x020017D2 RID: 6098
[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
	// Token: 0x06007D94 RID: 32148 RVA: 0x003270B8 File Offset: 0x003252B8
	public void Sim4000ms(float dt)
	{
		Facing component = base.GetComponent<Facing>();
		bool flip = false;
		if (component)
		{
			flip = component.GetFacing();
		}
		CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(base.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
	}

	// Token: 0x04005F1F RID: 24351
	private float CO2_RATE = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_TO_CO2_CONVERSION / 2f;
}
