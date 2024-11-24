using System;
using Klei.AI;

// Token: 0x02001BF3 RID: 7155
public class PercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060094AD RID: 38061 RVA: 0x00100C33 File Offset: 0x000FEE33
	public PercentAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060094AE RID: 38062 RVA: 0x00100D16 File Offset: 0x000FEF16
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x060094AF RID: 38063 RVA: 0x00100D2A File Offset: 0x000FEF2A
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x060094B0 RID: 38064 RVA: 0x00100D54 File Offset: 0x000FEF54
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value * 100f, timeSlice);
	}
}
