using System;

// Token: 0x02000B41 RID: 2881
public class CropTracker : WorldTracker
{
	// Token: 0x060036B7 RID: 14007 RVA: 0x000C3935 File Offset: 0x000C1B35
	public CropTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x002147B0 File Offset: 0x002129B0
	public override void UpdateData()
	{
		float num = 0f;
		foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(base.WorldID))
		{
			if (!(plantablePlot.plant == null) && plantablePlot.HasDepositTag(GameTags.CropSeed) && !plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				num += 1f;
			}
		}
		base.AddPoint(num);
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x000C398F File Offset: 0x000C1B8F
	public override string FormatValueString(float value)
	{
		return value.ToString() + "%";
	}
}
