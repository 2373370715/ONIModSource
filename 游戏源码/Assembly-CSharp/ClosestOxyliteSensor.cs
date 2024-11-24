using System;

// Token: 0x02000825 RID: 2085
public class ClosestOxyliteSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x0600254C RID: 9548 RVA: 0x000B85D4 File Offset: 0x000B67D4
	public ClosestOxyliteSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.OxyRock, shouldStartActive)
	{
	}
}
