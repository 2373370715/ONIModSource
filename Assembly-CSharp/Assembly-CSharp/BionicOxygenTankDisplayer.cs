using Klei.AI;
using STRINGS;

public class BionicOxygenTankDisplayer : AsPercentAmountDisplayer {
    public BionicOxygenTankDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice) { }

    public override string GetTooltip(Amount master, AmountInstance instance) {
        var smi  = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
        var text = string.Format(master.description, formatter.GetFormattedValue(instance.value));
        text += "\n\n";
        var num = instance.deltaAttribute.GetTotalDisplayValue();
        if (smi != null) {
            var totalValue = smi.airConsumptionRate.GetTotalValue();
            num += totalValue;
        }

        if (formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
            text += string.Format(UI.CHANGEPERCYCLE,
                                  formatter.GetFormattedValue(ToPercent(num, instance), GameUtil.TimeSlice.PerCycle));
        else
            text += string.Format(UI.CHANGEPERSECOND,
                                  formatter.GetFormattedValue(ToPercent(num, instance), GameUtil.TimeSlice.PerSecond));

        Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0,
                     "BionicOxygenTankDisplayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, the rate of breathing is defined by  Db.Get().Attributes.AirConsumptionRate");

        return text;
    }
}