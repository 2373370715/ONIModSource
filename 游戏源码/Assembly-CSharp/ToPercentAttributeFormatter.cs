using System;
using Klei.AI;

// Token: 0x02001BF2 RID: 7154
public class ToPercentAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060094A9 RID: 38057 RVA: 0x00100CFA File Offset: 0x000FEEFA
	public ToPercentAttributeFormatter(float max, GameUtil.TimeSlice deltaTimeSlice = GameUtil.TimeSlice.None) : base(GameUtil.UnitClass.Percent, deltaTimeSlice)
	{
		this.max = max;
	}

	// Token: 0x060094AA RID: 38058 RVA: 0x00100D16 File Offset: 0x000FEF16
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), base.DeltaTimeSlice);
	}

	// Token: 0x060094AB RID: 38059 RVA: 0x00100D2A File Offset: 0x000FEF2A
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, base.DeltaTimeSlice);
	}

	// Token: 0x060094AC RID: 38060 RVA: 0x00100D3E File Offset: 0x000FEF3E
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return GameUtil.GetFormattedPercent(value / this.max * 100f, timeSlice);
	}

	// Token: 0x04007321 RID: 29473
	public float max = 1f;
}
