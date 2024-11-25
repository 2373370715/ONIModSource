using UnityEngine;

public class PowerUseTracker : WorldTracker {
    public PowerUseTracker(int worldID) : base(worldID) { }

    public override void UpdateData() {
        var num = 0f;
        foreach (var utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks()) {
            var electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
            if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0) {
                var num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
                if (Grid.WorldIdx[num2] == WorldID)
                    num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager
                                                                                  .GetCircuitID(num2));
            }
        }

        AddPoint(Mathf.Round(num));
    }

    public override string FormatValueString(float value) { return GameUtil.GetFormattedWattage(value); }
}