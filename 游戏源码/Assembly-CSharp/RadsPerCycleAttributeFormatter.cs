using System;
using Klei.AI;

// Token: 0x02001BEE RID: 7150
public class RadsPerCycleAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600949D RID: 38045 RVA: 0x00100C93 File Offset: 0x000FEE93
	public RadsPerCycleAttributeFormatter() : base(GameUtil.UnitClass.Radiation, GameUtil.TimeSlice.PerCycle)
	{
	}

	// Token: 0x0600949E RID: 38046 RVA: 0x00100C9D File Offset: 0x000FEE9D
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x0600949F RID: 38047 RVA: 0x00100CAC File Offset: 0x000FEEAC
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return base.GetFormattedValue(value / 600f, timeSlice);
	}
}
