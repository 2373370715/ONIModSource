using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConduitOverflow")]
public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput {
    private int inputCell;
    private int outputCell;

    [SerializeField]
    public ConduitPortInfo portInfo;

    private FlowUtilityNetwork.NetworkItem secondaryOutput;
    public  bool HasSecondaryConduitType(ConduitType type) { return portInfo.conduitType == type; }
    public  CellOffset GetSecondaryConduitOffset(ConduitType type) { return portInfo.offset; }

    protected override void OnSpawn() {
        base.OnSpawn();
        var component = GetComponent<Building>();
        inputCell  = component.GetUtilityInputCell();
        outputCell = component.GetUtilityOutputCell();
        var cell          = Grid.PosToCell(transform.GetPosition());
        var rotatedOffset = component.GetRotatedOffset(portInfo.offset);
        var cell2         = Grid.OffsetCell(cell, rotatedOffset);
        Conduit.GetFlowManager(portInfo.conduitType).AddConduitUpdater(ConduitUpdate);
        var networkManager = Conduit.GetNetworkManager(portInfo.conduitType);
        secondaryOutput = new FlowUtilityNetwork.NetworkItem(portInfo.conduitType, Endpoint.Sink, cell2, gameObject);
        networkManager.AddToNetworks(secondaryOutput.Cell, secondaryOutput, true);
    }

    protected override void OnCleanUp() {
        Conduit.GetNetworkManager(portInfo.conduitType).RemoveFromNetworks(secondaryOutput.Cell, secondaryOutput, true);
        Conduit.GetFlowManager(portInfo.conduitType).RemoveConduitUpdater(ConduitUpdate);
        base.OnCleanUp();
    }

    private void ConduitUpdate(float dt) {
        var flowManager = Conduit.GetFlowManager(portInfo.conduitType);
        if (!flowManager.HasConduit(inputCell)) return;

        var contents = flowManager.GetContents(inputCell);
        if (contents.mass <= 0f) return;

        var cell      = outputCell;
        var contents2 = flowManager.GetContents(cell);
        if (contents2.mass > 0f) {
            cell      = secondaryOutput.Cell;
            contents2 = flowManager.GetContents(cell);
        }

        if (contents2.mass <= 0f) {
            var num = flowManager.AddElement(cell,
                                             contents.element,
                                             contents.mass,
                                             contents.temperature,
                                             contents.diseaseIdx,
                                             contents.diseaseCount);

            if (num > 0f) flowManager.RemoveElement(inputCell, num);
        }
    }
}