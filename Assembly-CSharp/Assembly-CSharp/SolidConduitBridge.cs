using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitBridge")]
public class SolidConduitBridge : ConduitBridgeBase {
    private int inputCell;

    [MyCmpGet]
    private Operational operational;

    private int  outputCell;
    public  bool IsDispensing { get; private set; }

    protected override void OnSpawn() {
        base.OnSpawn();
        var component = GetComponent<Building>();
        inputCell  = component.GetUtilityInputCell();
        outputCell = component.GetUtilityOutputCell();
        SolidConduit.GetFlowManager().AddConduitUpdater(ConduitUpdate);
    }

    protected override void OnCleanUp() {
        SolidConduit.GetFlowManager().RemoveConduitUpdater(ConduitUpdate);
        base.OnCleanUp();
    }

    private void ConduitUpdate(float dt) {
        IsDispensing = false;
        var num = 0f;
        if (operational && !operational.IsOperational) {
            SendEmptyOnMassTransfer();
            return;
        }

        var flowManager = SolidConduit.GetFlowManager();
        if (!flowManager.HasConduit(inputCell) || !flowManager.HasConduit(outputCell)) {
            SendEmptyOnMassTransfer();
            return;
        }

        if (flowManager.IsConduitFull(inputCell) && flowManager.IsConduitEmpty(outputCell)) {
            var pickupable = flowManager.GetPickupable(flowManager.GetContents(inputCell).pickupableHandle);
            if (pickupable == null) {
                flowManager.RemovePickupable(inputCell);
                SendEmptyOnMassTransfer();
                return;
            }

            var num2 = pickupable.PrimaryElement.Mass;
            if (desiredMassTransfer != null)
                num2 = desiredMassTransfer(dt,
                                           pickupable.PrimaryElement.Element.id,
                                           pickupable.PrimaryElement.Mass,
                                           pickupable.PrimaryElement.Temperature,
                                           pickupable.PrimaryElement.DiseaseIdx,
                                           pickupable.PrimaryElement.DiseaseCount,
                                           pickupable);

            if (num2 == 0f) {
                SendEmptyOnMassTransfer();
                return;
            }

            if (num2 < pickupable.PrimaryElement.Mass) {
                var pickupable2 = pickupable.Take(num2);
                flowManager.AddPickupable(outputCell, pickupable2);
                IsDispensing = true;
                num          = pickupable2.PrimaryElement.Mass;
                if (OnMassTransfer != null)
                    OnMassTransfer(pickupable2.PrimaryElement.ElementID,
                                   num,
                                   pickupable2.PrimaryElement.Temperature,
                                   pickupable2.PrimaryElement.DiseaseIdx,
                                   pickupable2.PrimaryElement.DiseaseCount,
                                   pickupable2);
            } else {
                var pickupable3 = flowManager.RemovePickupable(inputCell);
                if (pickupable3) {
                    flowManager.AddPickupable(outputCell, pickupable3);
                    IsDispensing = true;
                    num          = pickupable3.PrimaryElement.Mass;
                    if (OnMassTransfer != null)
                        OnMassTransfer(pickupable3.PrimaryElement.ElementID,
                                       num,
                                       pickupable3.PrimaryElement.Temperature,
                                       pickupable3.PrimaryElement.DiseaseIdx,
                                       pickupable3.PrimaryElement.DiseaseCount,
                                       pickupable3);
                }
            }
        }

        if (num == 0f) SendEmptyOnMassTransfer();
    }
}