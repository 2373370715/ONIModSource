using System;

// Token: 0x02000823 RID: 2083
public class ClosestElectrobankSensor : ClosestPickupableSensor<Electrobank>
{
	// Token: 0x0600254A RID: 9546 RVA: 0x000B859E File Offset: 0x000B679E
	public ClosestElectrobankSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.ChargedPortableBattery, shouldStartActive)
	{
	}
}
