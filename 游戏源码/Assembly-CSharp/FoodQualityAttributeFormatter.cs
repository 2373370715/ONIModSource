using System;
using Klei.AI;

// Token: 0x02001BEF RID: 7151
public class FoodQualityAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060094A0 RID: 38048 RVA: 0x00100CBC File Offset: 0x000FEEBC
	public FoodQualityAttributeFormatter() : base(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060094A1 RID: 38049 RVA: 0x00100C70 File Offset: 0x000FEE70
	public override string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x060094A2 RID: 38050 RVA: 0x00100CC6 File Offset: 0x000FEEC6
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetFormattedInt(modifier.Value, GameUtil.TimeSlice.None);
	}

	// Token: 0x060094A3 RID: 38051 RVA: 0x00100CD4 File Offset: 0x000FEED4
	public override string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return Util.StripTextFormatting(GameUtil.GetFormattedFoodQuality((int)value));
	}
}
