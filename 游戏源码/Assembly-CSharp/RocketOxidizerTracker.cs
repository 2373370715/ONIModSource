using System;

// Token: 0x02000B4D RID: 2893
public class RocketOxidizerTracker : WorldTracker
{
	// Token: 0x060036DD RID: 14045 RVA: 0x000C3935 File Offset: 0x000C1B35
	public RocketOxidizerTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x00214D24 File Offset: 0x00212F24
	public override void UpdateData()
	{
		Clustercraft component = ClusterManager.Instance.GetWorld(base.WorldID).GetComponent<Clustercraft>();
		base.AddPoint((component != null) ? component.ModuleInterface.OxidizerPowerRemaining : 0f);
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x000C229F File Offset: 0x000C049F
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
