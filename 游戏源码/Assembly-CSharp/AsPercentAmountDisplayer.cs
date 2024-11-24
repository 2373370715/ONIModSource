using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001BDC RID: 7132
public class AsPercentAmountDisplayer : IAmountDisplayer
{
	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x06009463 RID: 37987 RVA: 0x00100A5F File Offset: 0x000FEC5F
	public IAttributeFormatter Formatter
	{
		get
		{
			return this.formatter;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x06009464 RID: 37988 RVA: 0x00100A67 File Offset: 0x000FEC67
	// (set) Token: 0x06009465 RID: 37989 RVA: 0x00100A74 File Offset: 0x000FEC74
	public GameUtil.TimeSlice DeltaTimeSlice
	{
		get
		{
			return this.formatter.DeltaTimeSlice;
		}
		set
		{
			this.formatter.DeltaTimeSlice = value;
		}
	}

	// Token: 0x06009466 RID: 37990 RVA: 0x00100A82 File Offset: 0x000FEC82
	public AsPercentAmountDisplayer(GameUtil.TimeSlice deltaTimeSlice)
	{
		this.formatter = new StandardAttributeFormatter(GameUtil.UnitClass.Percent, deltaTimeSlice);
	}

	// Token: 0x06009467 RID: 37991 RVA: 0x00100A97 File Offset: 0x000FEC97
	public string GetValueString(Amount master, AmountInstance instance)
	{
		return this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None);
	}

	// Token: 0x06009468 RID: 37992 RVA: 0x00100AB2 File Offset: 0x000FECB2
	public virtual string GetDescription(Amount master, AmountInstance instance)
	{
		return string.Format("{0}: {1}", master.Name, this.formatter.GetFormattedValue(this.ToPercent(instance.value, instance), GameUtil.TimeSlice.None));
	}

	// Token: 0x06009469 RID: 37993 RVA: 0x00100ADD File Offset: 0x000FECDD
	public virtual string GetTooltipDescription(Amount master, AmountInstance instance)
	{
		return string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600946A RID: 37994 RVA: 0x003954AC File Offset: 0x003936AC
	public virtual string GetTooltip(Amount master, AmountInstance instance)
	{
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += string.Format(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(this.ToPercent(instance.deltaAttribute.GetTotalDisplayValue(), instance), GameUtil.TimeSlice.PerSecond));
		}
		for (int num = 0; num != instance.deltaAttribute.Modifiers.Count; num++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[num];
			float modifierContribution = instance.deltaAttribute.GetModifierContribution(attributeModifier);
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(this.ToPercent(modifierContribution, instance), this.formatter.DeltaTimeSlice));
		}
		return text;
	}

	// Token: 0x0600946B RID: 37995 RVA: 0x00100AFC File Offset: 0x000FECFC
	public string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.formatter.GetFormattedAttribute(instance);
	}

	// Token: 0x0600946C RID: 37996 RVA: 0x00100B0A File Offset: 0x000FED0A
	public string GetFormattedModifier(AttributeModifier modifier)
	{
		if (modifier.IsMultiplier)
		{
			return GameUtil.GetFormattedPercent(modifier.Value * 100f, GameUtil.TimeSlice.None);
		}
		return this.formatter.GetFormattedModifier(modifier);
	}

	// Token: 0x0600946D RID: 37997 RVA: 0x00100B33 File Offset: 0x000FED33
	public string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice)
	{
		return this.formatter.GetFormattedValue(value, timeSlice);
	}

	// Token: 0x0600946E RID: 37998 RVA: 0x00100B42 File Offset: 0x000FED42
	protected float ToPercent(float value, AmountInstance instance)
	{
		return 100f * value / instance.GetMax();
	}

	// Token: 0x0400731B RID: 29467
	protected StandardAttributeFormatter formatter;
}
