using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor {
    public const            float WATER_PER_USE = 5f;
    public static           string SHOWER_EFFECT = "Showered";
    private static readonly string[] EffectsRemoved = { "SoakingWet", "WetFeet", "MinorIrritation", "MajorIrritation" };
    public                  int absoluteDiseaseRemoval;
    private                 SimUtil.DiseaseInfo accumulatedDisease;
    public                  float fractionalDiseaseRemoval;
    public                  SimHashes outputTargetElement;
    private                 ShowerSM.Instance smi;
    private Shower() { SetReportType(ReportManager.ReportType.PersonalTime); }

    public override List<Descriptor> GetDescriptors(GameObject go) {
        var descriptors = base.GetDescriptors(go);
        if (EffectsRemoved.Length != 0) {
            var item = default(Descriptor);
            item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE,
                                 UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE);

            descriptors.Add(item);
            for (var i = 0; i < EffectsRemoved.Length; i++) {
                var    text  = EffectsRemoved[i];
                string arg   = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
                string arg2  = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
                var    item2 = default(Descriptor);
                item2.IncreaseIndent();
                item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg),
                                      string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2));

                descriptors.Add(item2);
            }
        }

        Effect.AddModifierDescriptions(gameObject, descriptors, SHOWER_EFFECT, true);
        return descriptors;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        resetProgressOnStop = true;
        smi                 = new ShowerSM.Instance(this);
        smi.StartSM();
    }

    protected override void OnStartWork(WorkerBase worker) {
        var instance = worker.GetSMI<HygieneMonitor.Instance>();
        WorkTimeRemaining  = workTime * instance.GetDirtiness();
        accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
        smi.SetActive(true);
        base.OnStartWork(worker);
    }

    protected override void OnStopWork(WorkerBase worker) { smi.SetActive(false); }

    protected override void OnCompleteWork(WorkerBase worker) {
        base.OnCompleteWork(worker);
        var component = worker.GetComponent<Effects>();
        for (var i = 0; i < EffectsRemoved.Length; i++) {
            var effect_id = EffectsRemoved[i];
            component.Remove(effect_id);
        }

        if (!worker.HasTag(GameTags.HasSuitTank)) {
            var instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
            if (instance != null) instance.ResetExposure();
        }

        component.Add(SHOWER_EFFECT, true);
        var instance2 = worker.GetSMI<HygieneMonitor.Instance>();
        if (instance2 != null) instance2.SetDirtiness(0f);
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt) {
        var component = worker.GetComponent<PrimaryElement>();
        if (component.DiseaseCount > 0) {
            var diseaseInfo = new SimUtil.DiseaseInfo {
                idx = component.DiseaseIdx,
                count = Mathf.CeilToInt(component.DiseaseCount * (1f - Mathf.Pow(fractionalDiseaseRemoval, dt)) -
                                        absoluteDiseaseRemoval)
            };

            component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
            accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(accumulatedDisease, diseaseInfo);
            var primaryElement = GetComponent<Storage>().FindPrimaryElement(outputTargetElement);
            if (primaryElement != null) {
                primaryElement.GetComponent<PrimaryElement>()
                              .AddDisease(accumulatedDisease.idx, accumulatedDisease.count, "Shower.RemoveDisease");

                accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
            }
        }

        return false;
    }

    protected override void OnAbortWork(WorkerBase worker) {
        base.OnAbortWork(worker);
        var instance = worker.GetSMI<HygieneMonitor.Instance>();
        if (instance != null) instance.SetDirtiness(1f - GetPercentComplete());
    }

    public class ShowerSM : GameStateMachine<ShowerSM, ShowerSM.Instance, Shower> {
        public OperationalState operational;
        public State            unoperational;

        public override void InitializeStates(out BaseState default_state) {
            default_state = unoperational;
            root.Update(UpdateStatusItems);
            unoperational.EventTransition(GameHashes.OperationalChanged, operational, smi => smi.IsOperational)
                         .PlayAnim("off");

            operational.DefaultState(operational.not_ready)
                       .EventTransition(GameHashes.OperationalChanged, unoperational, smi => !smi.IsOperational);

            operational.not_ready.EventTransition(GameHashes.OnStorageChange, operational.ready, smi => smi.IsReady())
                       .PlayAnim("off");

            operational.ready.ToggleChore(CreateShowerChore, operational.not_ready);
        }

        private Chore CreateShowerChore(Instance smi) {
            var workChore = new WorkChore<Shower>(Db.Get().ChoreTypes.Shower,
                                                  smi.master,
                                                  null,
                                                  true,
                                                  null,
                                                  null,
                                                  null,
                                                  false,
                                                  Db.Get().ScheduleBlockTypes.Hygiene,
                                                  false,
                                                  true,
                                                  null,
                                                  false,
                                                  true,
                                                  false,
                                                  PriorityScreen.PriorityClass.high);

            workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, smi);
            return workChore;
        }

        private void UpdateStatusItems(Instance smi, float dt) {
            if (smi.OutputFull()) {
                smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
                return;
            }

            smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull);
        }

        public class OperationalState : State {
            public State not_ready;
            public State ready;
        }

        public new class Instance : GameInstance {
            private readonly ConduitConsumer  consumer;
            private readonly ConduitDispenser dispenser;
            private readonly Operational      operational;

            public Instance(Shower master) : base(master) {
                operational = master.GetComponent<Operational>();
                consumer    = master.GetComponent<ConduitConsumer>();
                dispenser   = master.GetComponent<ConduitDispenser>();
            }

            public bool IsOperational => operational.IsOperational && consumer.IsConnected && dispenser.IsConnected;
            public void SetActive(bool active) { operational.SetActive(active); }

            private bool HasSufficientMass() {
                var result                         = false;
                var primaryElement                 = GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
                if (primaryElement != null) result = primaryElement.Mass >= 5f;
                return result;
            }

            public bool OutputFull() {
                var primaryElement = GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
                return primaryElement != null && primaryElement.Mass >= 5f;
            }

            public bool IsReady() { return HasSufficientMass() && !OutputFull(); }
        }
    }
}