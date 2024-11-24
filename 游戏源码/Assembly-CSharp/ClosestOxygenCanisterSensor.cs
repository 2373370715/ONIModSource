using System;

// Token: 0x02000824 RID: 2084
public class ClosestOxygenCanisterSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x0600254B RID: 9547 RVA: 0x000B85AD File Offset: 0x000B67AD
	public ClosestOxygenCanisterSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.Gas, shouldStartActive)
	{
		this.requiredTags = new Tag[]
		{
			GameTags.Breathable
		};
	}
}
