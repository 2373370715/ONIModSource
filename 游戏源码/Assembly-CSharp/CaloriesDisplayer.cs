using System;
using Klei.AI;

// Token: 0x02001BE1 RID: 7137
public class CaloriesDisplayer : StandardAmountDisplayer
{
	// Token: 0x06009477 RID: 38007 RVA: 0x00100B88 File Offset: 0x000FED88
	public CaloriesDisplayer() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new CaloriesDisplayer.CaloriesAttributeFormatter();
	}

	// Token: 0x02001BE2 RID: 7138
	public class CaloriesAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009478 RID: 38008 RVA: 0x00100B9F File Offset: 0x000FED9F
		public CaloriesAttributeFormatter() : base(GameUtil.UnitClass.Calories, GameUtil.TimeSlice.PerCycle)
		{
		}

		// Token: 0x06009479 RID: 38009 RVA: 0x00100BA9 File Offset: 0x000FEDA9
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			if (modifier.IsMultiplier)
			{
				return GameUtil.GetFormattedPercent(-modifier.Value * 100f, GameUtil.TimeSlice.None);
			}
			return base.GetFormattedModifier(modifier);
		}
	}
}
