using System;

// Token: 0x02000B3C RID: 2876
public class AllChoresCountTracker : WorldTracker
{
	// Token: 0x060036A9 RID: 13993 RVA: 0x000C3935 File Offset: 0x000C1B35
	public AllChoresCountTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x002144D4 File Offset: 0x002126D4
	public override void UpdateData()
	{
		float num = 0f;
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			Tracker choreGroupTracker = TrackerTool.Instance.GetChoreGroupTracker(base.WorldID, Db.Get().ChoreGroups[i]);
			num += ((choreGroupTracker == null) ? 0f : choreGroupTracker.GetCurrentValue());
		}
		base.AddPoint(num);
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x000C20D4 File Offset: 0x000C02D4
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
