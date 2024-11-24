using System;

// Token: 0x02000B3E RID: 2878
public class AllWorkTimeTracker : WorldTracker
{
	// Token: 0x060036AF RID: 13999 RVA: 0x000C3935 File Offset: 0x000C1B35
	public AllWorkTimeTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036B0 RID: 14000 RVA: 0x0021467C File Offset: 0x0021287C
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			num += TrackerTool.Instance.GetWorkTimeTracker(base.WorldID, Db.Get().ChoreGroups[i]).GetCurrentValue();
		}
		base.AddPoint(num);
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x000C394E File Offset: 0x000C1B4E
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedPercent(value, GameUtil.TimeSlice.None).ToString();
	}
}
