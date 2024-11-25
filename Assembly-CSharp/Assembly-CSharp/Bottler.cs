using System;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Bottler")]
public class Bottler : Workable, IUserControlledCapacity {
    private static readonly EventSystem.IntraObjectHandler<Bottler> OnCopySettingsDelegate
        = new EventSystem.IntraObjectHandler<Bottler>(delegate(Bottler component, object data) {
                                                          component.OnCopySettings(data);
                                                      });

    public ConduitConsumer consumer;

    [MyCmpAdd]
    private CopyBuildingSettings copyBuildingSettings;

    private Controller.Instance smi;
    public  Storage             storage;
    private int                 storageHandle;

    [Serialize]
    public float userMaxCapacity = float.PositiveInfinity;

    public  CellOffset      workCellOffset = new CellOffset(0, 0);
    private MeterController workerMeter;

    private Tag SourceTag {
        get {
            if (smi.master.consumer.conduitType != ConduitType.Gas) return GameTags.LiquidSource;

            return GameTags.GasSource;
        }
    }

    private Tag ElementTag {
        get {
            if (smi.master.consumer.conduitType != ConduitType.Gas) return GameTags.Liquid;

            return GameTags.Gas;
        }
    }

    public float UserMaxCapacity {
        get {
            if (consumer != null) return Mathf.Min(userMaxCapacity, storage.capacityKg);

            return 0f;
        }
        set {
            userMaxCapacity = value;
            SetConsumerCapacity(value);
        }
    }

    public float     AmountStored  => storage.MassStored();
    public float     MinCapacity   => 0f;
    public float     MaxCapacity   => storage.capacityKg;
    public bool      WholeValues   => false;
    public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        overrideAnims      = new[] { Assets.GetAnim("anim_interacts_bottler_kanim") };
        workAnims          = new HashedString[] { "pick_up" };
        workingPstComplete = null;
        workingPstFailed   = null;
        synchronizeAnims   = true;
        SetOffsets(new[] { workCellOffset });
        SetWorkTime(overrideAnims[0].GetData().GetAnim("pick_up").totalTime);
        resetProgressOnStop = true;
        showProgressBar     = false;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        smi = new Controller.Instance(this);
        smi.StartSM();
        Subscribe(-905833192, OnCopySettingsDelegate);
        UpdateStoredItemState();
        SetConsumerCapacity(userMaxCapacity);
    }

    protected override void OnForcedCleanUp() {
        if (worker != null) {
            var component = worker.GetComponent<ChoreDriver>();
            if (component != null)
                component.StopChore();
            else
                worker.StopWork();
        }

        if (workerMeter != null) CleanupBottleProxyObject();
        base.OnForcedCleanUp();
    }

    protected override void OnStartWork(WorkerBase worker) {
        base.OnStartWork(worker);
        CreateBottleProxyObject(worker);
    }

    private void CreateBottleProxyObject(WorkerBase worker) {
        if (workerMeter != null) {
            CleanupBottleProxyObject();
            KCrashReporter.ReportDevNotification("CreateBottleProxyObject called before cleanup",
                                                 Environment.StackTrace);
        }

        var firstPrimaryElement = smi.master.GetFirstPrimaryElement();
        if (firstPrimaryElement == null) {
            KCrashReporter.ReportDevNotification("CreateBottleProxyObject on a null element", Environment.StackTrace);
            return;
        }

        workerMeter = new MeterController(worker.GetComponent<KBatchedAnimController>(),
                                          "snapto_chest",
                                          "meter",
                                          Meter.Offset.Infront,
                                          Grid.SceneLayer.NoLayer,
                                          "snapto_chest");

        workerMeter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
        workerMeter.meterController.Play("empty", KAnim.PlayMode.Paused);
        var colour = firstPrimaryElement.Element.substance.colour;
        colour.a = byte.MaxValue;
        workerMeter.SetSymbolTint(new KAnimHashedString("meter_fill"),       colour);
        workerMeter.SetSymbolTint(new KAnimHashedString("water1"),           colour);
        workerMeter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
    }

    private void CleanupBottleProxyObject() {
        if (workerMeter != null && !workerMeter.gameObject.IsNullOrDestroyed()) {
            workerMeter.Unlink();
            workerMeter.gameObject.DeleteObject();
        } else {
            var str             = "Bottler finished work but could not clean up the proxy bottle object. workerMeter=";
            var meterController = workerMeter;
            DebugUtil.DevLogError(str + (meterController != null ? meterController.ToString() : null));
            KCrashReporter.ReportDevNotification("Bottle emptier could not clean up proxy object",
                                                 Environment.StackTrace);
        }

        workerMeter = null;
    }

    protected override void OnStopWork(WorkerBase worker) {
        base.OnStopWork(worker);
        CleanupBottleProxyObject();
    }

    protected override void OnAbortWork(WorkerBase worker) {
        base.OnAbortWork(worker);
        GetAnimController().Play("ready");
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        var component               = worker.GetComponent<Storage>();
        var pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
        if (pickupableStartWorkInfo.amount > 0f)
            storage.TransferMass(component,
                                 pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(),
                                 pickupableStartWorkInfo.amount);

        var gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
        if (gameObject != null) {
            var component2 = gameObject.GetComponent<Pickupable>();
            component2.targetWorkable = component2;
            component2.RemoveTag(SourceTag);
            pickupableStartWorkInfo.setResultCb(gameObject);
        } else
            pickupableStartWorkInfo.setResultCb(null);

        base.OnCompleteWork(worker);
    }

    private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3) {
        var forceUnfetchable = false;
        using (var enumerator = storage.items.GetEnumerator()) {
            while (enumerator.MoveNext())
                if (enumerator.Current.GetComponent<Pickupable>().ReservedAmount > 0f) {
                    forceUnfetchable = true;
                    break;
                }
        }

        foreach (var go in storage.items) {
            var instance = go.GetSMI<FetchableMonitor.Instance>();
            if (instance != null) instance.SetForceUnfetchable(forceUnfetchable);
        }
    }

    private void SetConsumerCapacity(float value) {
        if (consumer != null) {
            consumer.capacityKG = value;
            var num = storage.MassStored() - userMaxCapacity;
            if (num > 0f)
                storage.DropSome(storage.FindFirstWithMass(smi.master.ElementTag).ElementID.CreateTag(),
                                 num,
                                 false,
                                 false,
                                 new Vector3(0.8f, 0f, 0f));
        }
    }

    protected override void OnCleanUp() {
        if (smi != null) smi.StopSM("OnCleanUp");
        base.OnCleanUp();
    }

    private PrimaryElement GetFirstPrimaryElement() {
        for (var i = 0; i < storage.Count; i++) {
            var gameObject = storage[i];
            if (!(gameObject == null)) {
                var component = gameObject.GetComponent<PrimaryElement>();
                if (!(component == null)) return component;
            }
        }

        return null;
    }

    private void UpdateStoredItemState() {
        storage.allowItemRemoval = smi != null && smi.GetCurrentState() == smi.sm.ready;
        foreach (var gameObject in storage.items)
            if (gameObject != null)
                gameObject.Trigger(-778359855, storage);
    }

    private void OnCopySettings(object data) {
        var component = ((GameObject)data).GetComponent<Bottler>();
        UserMaxCapacity = component.UserMaxCapacity;
    }

    private class Controller : GameStateMachine<Controller, Controller.Instance, Bottler> {
        public State empty;
        public State filling;
        public State ready;

        public override void InitializeStates(out BaseState default_state) {
            default_state = empty;
            empty.PlayAnim("off")
                 .EventHandlerTransition(GameHashes.OnStorageChange, filling, (smi, o) => IsFull(smi))
                 .EnterTransition(ready, IsFull);

            filling.PlayAnim("working").Enter(delegate(Instance smi) { smi.UpdateMeter(); }).OnAnimQueueComplete(ready);
            ready.EventTransition(GameHashes.OnStorageChange, empty, Not(IsFull))
                 .PlayAnim("ready")
                 .Enter(delegate(Instance smi) {
                            smi.master.storage.allowItemRemoval = true;
                            smi.UpdateMeter();
                            foreach (var gameObject in smi.master.storage.items) {
                                var component = gameObject.GetComponent<Pickupable>();
                                component.targetWorkable = smi.master;
                                component.SetOffsets(new[] { smi.master.workCellOffset });
                                var pickupable = component;
                                pickupable.OnReservationsChanged
                                    = (Action<Pickupable, bool, Pickupable.Reservation>)
                                    Delegate.Combine(pickupable.OnReservationsChanged,
                                                     new Action<Pickupable, bool, Pickupable.Reservation>(smi.master
                                                         .OnReservationsChanged));

                                component.KPrefabID.AddTag(smi.master.SourceTag);
                                gameObject.Trigger(-778359855, smi.master.storage);
                            }
                        })
                 .Exit(delegate(Instance smi) {
                           smi.master.storage.allowItemRemoval = false;
                           foreach (var gameObject in smi.master.storage.items) {
                               var component = gameObject.GetComponent<Pickupable>();
                               component.targetWorkable = component;
                               component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
                               component.OnReservationsChanged
                                   = (Action<Pickupable, bool, Pickupable.Reservation>)
                                   Delegate.Remove(component.OnReservationsChanged,
                                                   new Action<Pickupable, bool, Pickupable.Reservation>(smi.master
                                                       .OnReservationsChanged));

                               component.KPrefabID.RemoveTag(smi.master.SourceTag);
                               var smi2 = component.GetSMI<FetchableMonitor.Instance>();
                               if (smi2 != null) smi2.SetForceUnfetchable(false);
                               gameObject.Trigger(-778359855, smi.master.storage);
                           }
                       });
        }

        public static bool IsFull(Instance smi) {
            return smi.master.storage.MassStored() >= smi.master.userMaxCapacity;
        }

        public new class Instance : GameInstance {
            public Instance(Bottler master) : base(master) {
                meter = new MeterController(GetComponent<KBatchedAnimController>(),
                                            "bottle",
                                            "off",
                                            Meter.Offset.UserSpecified,
                                            Grid.SceneLayer.BuildingFront,
                                            "bottle",
                                            "substance_tinter");
            }

            public MeterController meter { get; }

            public void UpdateMeter() {
                var firstPrimaryElement = smi.master.GetFirstPrimaryElement();
                if (firstPrimaryElement == null) return;

                meter.meterController.SwapAnims(firstPrimaryElement.Element.substance.anims);
                meter.meterController.Play(OreSizeVisualizerComponents.GetAnimForMass(firstPrimaryElement.Mass),
                                           KAnim.PlayMode.Paused);

                var colour = firstPrimaryElement.Element.substance.colour;
                colour.a = byte.MaxValue;
                meter.SetSymbolTint(new KAnimHashedString("meter_fill"),       colour);
                meter.SetSymbolTint(new KAnimHashedString("water1"),           colour);
                meter.SetSymbolTint(new KAnimHashedString("substance_tinter"), colour);
            }
        }
    }
}