using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001BE5 RID: 7141
public class DecorDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600947E RID: 38014 RVA: 0x00100C08 File Offset: 0x000FEE08
	public DecorDisplayer() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new DecorDisplayer.DecorAttributeFormatter();
	}

	// Token: 0x0600947F RID: 38015 RVA: 0x00395C68 File Offset: 0x00393E68
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(LocText.ParseText(master.description), this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		int cell = Grid.PosToCell(instance.gameObject);
		if (Grid.IsValidCell(cell))
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_CURRENT, GameUtil.GetDecorAtCell(cell));
		}
		text += "\n";
		DecorMonitor.Instance smi = instance.gameObject.GetSMI<DecorMonitor.Instance>();
		if (smi != null)
		{
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_TODAY, this.formatter.GetFormattedValue(smi.GetTodaysAverageDecor(), GameUtil.TimeSlice.None));
			text += string.Format(DUPLICANTS.STATS.DECOR.TOOLTIP_AVERAGE_YESTERDAY, this.formatter.GetFormattedValue(smi.GetYesterdaysAverageDecor(), GameUtil.TimeSlice.None));
		}
		return text;
	}

	// Token: 0x02001BE6 RID: 7142
	public class DecorAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009480 RID: 38016 RVA: 0x00100BFE File Offset: 0x000FEDFE
		public DecorAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.PerCycle)
		{
		}
	}
}
