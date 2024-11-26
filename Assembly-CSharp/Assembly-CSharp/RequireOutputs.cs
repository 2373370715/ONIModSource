using System;
using UnityEngine;

[SkipSaveFileSerialization, AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour {
    private static readonly Operational.Flag outputConnectedFlag
        = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

    private static readonly Operational.Flag pipesHaveRoomFlag
        = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

    private ConduitType conduitType;
    private bool        connected;
    private Guid        hasPipeGuid;
    public  bool        ignoreFullPipe;

    [MyCmpReq]
    private Operational operational;

    private HandleVector<int>.Handle partitionerEntry;
    private Guid                     pipeBlockedGuid;
    private bool                     previouslyConnected = true;
    private bool                     previouslyHadRoom   = true;

    [MyCmpReq]
    private KSelectable selectable;

    private int utilityCell;

    protected override void OnSpawn() {
        base.OnSpawn();
        ScenePartitionerLayer scenePartitionerLayer = null;
        var                   component             = GetComponent<Building>();
        utilityCell = component.GetUtilityOutputCell();
        conduitType = component.Def.OutputConduitType;
        switch (component.Def.OutputConduitType) {
            case ConduitType.Gas:
                scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
                break;
            case ConduitType.Liquid:
                scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
                break;
            case ConduitType.Solid:
                scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
                break;
        }

        UpdateConnectionState(true);
        UpdatePipeRoomState(true);
        if (scenePartitionerLayer != null)
            partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs",
                                                                 gameObject,
                                                                 utilityCell,
                                                                 scenePartitionerLayer,
                                                                 delegate { UpdateConnectionState(); });

        GetConduitFlow().AddConduitUpdater(UpdatePipeState, ConduitFlowPriority.First);
    }

    protected override void OnCleanUp() {
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        var conduitFlow = GetConduitFlow();
        if (conduitFlow != null) conduitFlow.RemoveConduitUpdater(UpdatePipeState);
        base.OnCleanUp();
    }

    private void UpdateConnectionState(bool force_update = false) {
        connected = IsConnected(utilityCell);
        if (connected != previouslyConnected || force_update) {
            operational.SetFlag(outputConnectedFlag, connected);
            previouslyConnected = connected;
            StatusItem status_item = null;
            switch (conduitType) {
                case ConduitType.Gas:
                    status_item = Db.Get().BuildingStatusItems.NeedGasOut;
                    break;
                case ConduitType.Liquid:
                    status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
                    break;
                case ConduitType.Solid:
                    status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
                    break;
            }

            hasPipeGuid = selectable.ToggleStatusItem(status_item, hasPipeGuid, !connected, this);
        }
    }

    private bool OutputPipeIsEmpty() {
        if (ignoreFullPipe) return true;

        var result            = true;
        if (connected) result = GetConduitFlow().IsConduitEmpty(utilityCell);
        return result;
    }

    private void UpdatePipeState(float dt) { UpdatePipeRoomState(); }

    private void UpdatePipeRoomState(bool force_update = false) {
        var flag = OutputPipeIsEmpty();
        if (flag != previouslyHadRoom || force_update) {
            operational.SetFlag(pipesHaveRoomFlag, flag);
            previouslyHadRoom = flag;
            var status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
            if (conduitType == ConduitType.Solid)
                status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;

            pipeBlockedGuid = selectable.ToggleStatusItem(status_item, pipeBlockedGuid, !flag);
        }
    }

    private IConduitFlow GetConduitFlow() {
        switch (conduitType) {
            case ConduitType.Gas:
                return Game.Instance.gasConduitFlow;
            case ConduitType.Liquid:
                return Game.Instance.liquidConduitFlow;
            case ConduitType.Solid:
                return Game.Instance.solidConduitFlow;
            default:
                Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + conduitType);
                return null;
        }
    }

    private bool IsConnected(int cell) { return IsConnected(cell, conduitType); }

    public static bool IsConnected(int cell, ConduitType conduitType) {
        var layer = ObjectLayer.NumLayers;
        switch (conduitType) {
            case ConduitType.Gas:
                layer = ObjectLayer.GasConduit;
                break;
            case ConduitType.Liquid:
                layer = ObjectLayer.LiquidConduit;
                break;
            case ConduitType.Solid:
                layer = ObjectLayer.SolidConduit;
                break;
        }

        var gameObject = Grid.Objects[cell, (int)layer];
        return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
    }
}