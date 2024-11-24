using System;

// Token: 0x02000B48 RID: 2888
public class KCalTracker : WorldTracker
{
	// Token: 0x060036CE RID: 14030 RVA: 0x000C3935 File Offset: 0x000C1B35
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x000C39DB File Offset: 0x000C1BDB
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x000C2256 File Offset: 0x000C0456
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
