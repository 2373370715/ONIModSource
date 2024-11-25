using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms {
    private const float OperationalUpdateInterval = 1f;

    public static readonly Operational.Flag PumpableFlag
        = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

    private Guid conduitBlockedStatusGuid;

    [MyCmpGet]
    private ElementConsumer consumer;

    [MyCmpGet]
    private ConduitDispenser dispenser;

    private float elapsedTime;
    private Guid  noElementStatusGuid;

    [MyCmpReq]
    private Operational operational;

    private bool pumpable;

    [MyCmpGet]
    private KSelectable selectable;

    [MyCmpGet]
    private Storage storage;

    public ConduitType conduitType => dispenser.conduitType;

    public void Sim1000ms(float dt) {
        elapsedTime += dt;
        if (elapsedTime >= 1f) {
            pumpable    = UpdateOperational();
            elapsedTime = 0f;
        }

        if (operational.IsOperational && pumpable) {
            operational.SetActive(true);
            return;
        }

        operational.SetActive(false);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        consumer.EnableConsumption(false);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        elapsedTime = 0f;
        pumpable    = UpdateOperational();
        dispenser.GetConduitManager().AddConduitUpdater(OnConduitUpdate, ConduitFlowPriority.LastPostUpdate);
    }

    protected override void OnCleanUp() {
        dispenser.GetConduitManager().RemoveConduitUpdater(OnConduitUpdate);
        base.OnCleanUp();
    }

    private bool UpdateOperational() {
        var state       = Element.State.Vacuum;
        var conduitType = dispenser.conduitType;
        if (conduitType != ConduitType.Gas) {
            if (conduitType == ConduitType.Liquid) state = Element.State.Liquid;
        } else
            state = Element.State.Gas;

        var flag = IsPumpable(state, consumer.consumptionRadius);
        var status_item = state == Element.State.Gas
                              ? Db.Get().BuildingStatusItems.NoGasElementToPump
                              : Db.Get().BuildingStatusItems.NoLiquidElementToPump;

        noElementStatusGuid = selectable.ToggleStatusItem(status_item, noElementStatusGuid, !flag);
        operational.SetFlag(PumpableFlag, !storage.IsFull() && flag);
        return flag;
    }

    private bool IsPumpable(Element.State expected_state, int radius) {
        var num = Grid.PosToCell(transform.GetPosition());
        for (var i = 0; i < consumer.consumptionRadius; i++) {
            for (var j = 0; j < consumer.consumptionRadius; j++) {
                var num2 = num + j + Grid.WidthInCells * i;
                if (Grid.Element[num2].IsState(expected_state)) return true;
            }
        }

        return false;
    }

    private void OnConduitUpdate(float dt) {
        conduitBlockedStatusGuid = selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked,
                                                               conduitBlockedStatusGuid,
                                                               dispenser.blocked);
    }
}