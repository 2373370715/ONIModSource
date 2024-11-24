using Klei.AI;
using STRINGS;

public class CritterTemperatureDeltaAsEnergyAmountDisplayer : StandardAmountDisplayer
{
	public CritterTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice timeSlice)
		: base(unitClass, timeSlice)
	{
	}

	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		CritterTemperatureMonitor.Def def = instance.gameObject.GetDef<CritterTemperatureMonitor.Def>();
		PrimaryElement component = instance.gameObject.GetComponent<PrimaryElement>();
		string text = string.Format(master.description, formatter.GetFormattedValue(def.temperatureColdUncomfortable), formatter.GetFormattedValue(def.temperatureHotUncomfortable));
		float num = ElementLoader.FindElementByHash(SimHashes.Creature).specificHeatCapacity * component.Mass * 1000f;
		if (formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			text += "\n\n";
			text += string.Format(UI.CHANGEPERCYCLE, formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerCycle));
		}
		else if (instance.deltaAttribute.Modifiers.Count > 0)
		{
			text += "\n\n";
			text += string.Format(UI.CHANGEPERSECOND, formatter.GetFormattedValue(instance.deltaAttribute.GetTotalDisplayValue(), GameUtil.TimeSlice.PerSecond));
			text = text + "\n" + string.Format(UI.CHANGEPERSECOND, GameUtil.GetFormattedJoules(instance.deltaAttribute.GetTotalDisplayValue() * num));
		}
		for (int i = 0; i != instance.deltaAttribute.Modifiers.Count; i++)
		{
			AttributeModifier attributeModifier = instance.deltaAttribute.Modifiers[i];
			text = text + "\n" + string.Format(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), GameUtil.GetFormattedHeatEnergyRate(attributeModifier.Value * num * 1f));
		}
		return text;
	}
}
