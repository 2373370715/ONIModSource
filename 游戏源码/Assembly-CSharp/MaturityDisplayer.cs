using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001BE7 RID: 7143
public class MaturityDisplayer : AsPercentAmountDisplayer
{
	// Token: 0x06009481 RID: 38017 RVA: 0x00100C1F File Offset: 0x000FEE1F
	public MaturityDisplayer() : base(GameUtil.TimeSlice.PerCycle)
	{
		this.formatter = new MaturityDisplayer.MaturityAttributeFormatter();
	}

	// Token: 0x06009482 RID: 38018 RVA: 0x00395D3C File Offset: 0x00393F3C
	public override string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		string text = base.GetTooltipDescription(master, instance);
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component.IsGrowing())
		{
			float seconds = (instance.GetMax() - instance.value) / instance.GetDelta();
			if (component != null && component.IsGrowing())
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING_CROP, GameUtil.GetFormattedCycles(seconds, "F1", false), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
			}
			else
			{
				text += string.Format(CREATURES.STATS.MATURITY.TOOLTIP_GROWING, GameUtil.GetFormattedCycles(seconds, "F1", false));
			}
		}
		else if (component.ReachedNextHarvest())
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_GROWN;
		}
		else
		{
			text += CREATURES.STATS.MATURITY.TOOLTIP_STALLED;
		}
		return text;
	}

	// Token: 0x06009483 RID: 38019 RVA: 0x00395E14 File Offset: 0x00394014
	public override string GetDescription(Amount master, AmountInstance instance)
	{
		Growing component = instance.gameObject.GetComponent<Growing>();
		if (component != null && component.IsGrowing())
		{
			return string.Format(CREATURES.STATS.MATURITY.AMOUNT_DESC_FMT, master.Name, this.formatter.GetFormattedValue(base.ToPercent(instance.value, instance), GameUtil.TimeSlice.None), GameUtil.GetFormattedCycles(component.TimeUntilNextHarvest(), "F1", false));
		}
		return base.GetDescription(master, instance);
	}

	// Token: 0x02001BE8 RID: 7144
	public class MaturityAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009484 RID: 38020 RVA: 0x00100C33 File Offset: 0x000FEE33
		public MaturityAttributeFormatter() : base(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.None)
		{
		}

		// Token: 0x06009485 RID: 38021 RVA: 0x00395E88 File Offset: 0x00394088
		public override string GetFormattedModifier(AttributeModifier modifier)
		{
			float num = modifier.Value;
			GameUtil.TimeSlice timeSlice = base.DeltaTimeSlice;
			if (modifier.IsMultiplier)
			{
				num *= 100f;
				timeSlice = GameUtil.TimeSlice.None;
			}
			return this.GetFormattedValue(num, timeSlice);
		}
	}
}
