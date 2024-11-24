using System;
using Klei.AI;

// Token: 0x02001BF1 RID: 7153
public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x060094A7 RID: 38055 RVA: 0x00100CE2 File Offset: 0x000FEEE2
	public GermResistanceAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x060094A8 RID: 38056 RVA: 0x00100CEC File Offset: 0x000FEEEC
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
	}
}
