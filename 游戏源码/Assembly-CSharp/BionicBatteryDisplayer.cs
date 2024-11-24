using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001BDD RID: 7133
public class BionicBatteryDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600946F RID: 37999 RVA: 0x003955D8 File Offset: 0x003937D8
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		BionicBatteryMonitor.Instance smi = instance.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
		string text = "";
		if (master.description.IndexOf("{1}") > -1)
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None), GameUtil.GetIdentityDescriptor(instance.gameObject, this.tense));
		}
		else
		{
			text += string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		}
		text += "\n\n";
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float wattage = smi.Wattage;
			num += wattage;
		}
		float seconds = (num == 0f) ? 0f : (smi.CurrentCharge / num);
		text += string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.CURRENT_WATTAGE_TOOLTIP_LABEL, this.formatter.GetFormattedValue(num, this.formatter.DeltaTimeSlice));
		if (smi != null)
		{
			string text2 = "\n    • ";
			if (smi.IsBatterySaveModeActive)
			{
				string str = "<b>+</b>";
				text2 += string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.SAVING_MODE_TEMPLATE, DUPLICANTS.MODIFIERS.BIONIC_WATTS.BASE_NAME, DUPLICANTS.MODIFIERS.BIONIC_WATTS.SAVING_MODE_NAME, str + GameUtil.GetFormattedWattage(smi.GetBaseWattage(), GameUtil.WattageFormatterUnit.Automatic, true));
			}
			else
			{
				string str2 = "<b>+</b>";
				text2 += string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.MODIFIERS.BIONIC_WATTS.BASE_NAME, str2 + GameUtil.GetFormattedWattage(smi.GetBaseWattage(), GameUtil.WattageFormatterUnit.Automatic, true));
			}
			text += text2;
			float num2 = 0f;
			string text3 = "";
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in smi.Modifiers)
			{
				if (wattageModifier.value != 0f)
				{
					text = text + "\n    • " + wattageModifier.name;
				}
				else
				{
					text3 = text3 + "\n    • " + wattageModifier.name;
					num2 += wattageModifier.potentialValue;
				}
			}
			text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.POTENTIAL_EXTRA_WATTAGE_TOOLTIP_LABEL, this.formatter.GetFormattedValue(num2, this.formatter.DeltaTimeSlice)) + text3;
		}
		Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, "Bionic Battery Displayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, instead, use BionicBatteryMonitor.Instance.Modifiers");
		text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.ESTIMATED_LIFE_TIME_REMAINING, GameUtil.GetFormattedCycles(seconds, "F1", false));
		return text;
	}

	// Token: 0x06009470 RID: 38000 RVA: 0x00100B52 File Offset: 0x000FED52
	public override string GetValueString(Amount master, AmountInstance instance)
	{
		return base.GetValueString(master, instance);
	}

	// Token: 0x06009471 RID: 38001 RVA: 0x00100B5C File Offset: 0x000FED5C
	public BionicBatteryDisplayer() : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond, null, GameUtil.IdentityDescriptorTense.Normal)
	{
		this.formatter = new BionicBatteryDisplayer.BionicBatteryAttributeFormatter();
	}

	// Token: 0x0400731C RID: 29468
	private const float criticalIconFlashFrequency = 0.45f;

	// Token: 0x02001BDE RID: 7134
	public class BionicBatteryAttributeFormatter : StandardAttributeFormatter
	{
		// Token: 0x06009472 RID: 38002 RVA: 0x00100B74 File Offset: 0x000FED74
		public BionicBatteryAttributeFormatter() : base(GameUtil.UnitClass.Energy, GameUtil.TimeSlice.PerSecond)
		{
		}
	}
}
