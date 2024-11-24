using System;

// Token: 0x02000B44 RID: 2884
public class ResourceTracker : WorldTracker
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060036C0 RID: 14016 RVA: 0x000C39A2 File Offset: 0x000C1BA2
	// (set) Token: 0x060036C1 RID: 14017 RVA: 0x000C39AA File Offset: 0x000C1BAA
	public Tag tag { get; private set; }

	// Token: 0x060036C2 RID: 14018 RVA: 0x000C39B3 File Offset: 0x000C1BB3
	public ResourceTracker(int worldID, Tag materialCategoryTag) : base(worldID)
	{
		this.tag = materialCategoryTag;
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00214918 File Offset: 0x00212B18
	public override void UpdateData()
	{
		if (ClusterManager.Instance.GetWorld(base.WorldID).worldInventory == null)
		{
			return;
		}
		base.AddPoint(ClusterManager.Instance.GetWorld(base.WorldID).worldInventory.GetAmount(this.tag, false));
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x000C229F File Offset: 0x000C049F
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
	}
}
