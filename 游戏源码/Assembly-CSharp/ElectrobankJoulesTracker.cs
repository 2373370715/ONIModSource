using System;

// Token: 0x02000B49 RID: 2889
public class ElectrobankJoulesTracker : WorldTracker
{
	// Token: 0x060036D1 RID: 14033 RVA: 0x000C3935 File Offset: 0x000C1B35
	public ElectrobankJoulesTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x000C3A04 File Offset: 0x000C1C04
	public override void UpdateData()
	{
		base.AddPoint(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x000C39CD File Offset: 0x000C1BCD
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
