using System;

// Token: 0x02001994 RID: 6548
public readonly struct SpaceScannerTarget
{
	// Token: 0x06008889 RID: 34953 RVA: 0x000F9527 File Offset: 0x000F7727
	private SpaceScannerTarget(string id)
	{
		this.id = id;
	}

	// Token: 0x0600888A RID: 34954 RVA: 0x000F9530 File Offset: 0x000F7730
	public static SpaceScannerTarget MeteorShower()
	{
		return new SpaceScannerTarget("meteor_shower");
	}

	// Token: 0x0600888B RID: 34955 RVA: 0x000F953C File Offset: 0x000F773C
	public static SpaceScannerTarget BallisticObject()
	{
		return new SpaceScannerTarget("ballistic_object");
	}

	// Token: 0x0600888C RID: 34956 RVA: 0x000F9548 File Offset: 0x000F7748
	public static SpaceScannerTarget RocketBaseGame(LaunchConditionManager rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_base_game::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x0600888D RID: 34957 RVA: 0x000F9569 File Offset: 0x000F7769
	public static SpaceScannerTarget RocketDlc1(Clustercraft rocket)
	{
		return new SpaceScannerTarget(string.Format("rocket_dlc1::{0}", rocket.GetComponent<KPrefabID>().InstanceID));
	}

	// Token: 0x040066B5 RID: 26293
	public readonly string id;
}
