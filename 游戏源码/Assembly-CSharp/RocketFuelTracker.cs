using System;

// Token: 0x02000B4C RID: 2892
public class RocketFuelTracker : WorldTracker
{
	// Token: 0x060036DA RID: 14042 RVA: 0x000C3935 File Offset: 0x000C1B35
	public RocketFuelTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x00214CE0 File Offset: 0x00212EE0
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.FuelRemaining : 0f);
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x000C229F File Offset: 0x000C049F
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
