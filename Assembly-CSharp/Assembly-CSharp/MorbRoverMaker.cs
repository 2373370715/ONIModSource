using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

public class MorbRoverMaker
    : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def> {
    private const string            ROBOT_PROGRESS_METER_TARGET_NAME    = "meter_robot_target";
    private const string            ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";
    private const string            COVERED_IDLE_ANIM_NAME              = "dusty";
    private const string            IDLE_ANIM_NAME                      = "idle";
    private const string            CRAFT_PRE_ANIM_NAME                 = "crafting_pre";
    private const string            CRAFT_LOOP_ANIM_NAME                = "crafting_loop";
    private const string            CRAFT_PST_ANIM_NAME                 = "crafting_pst";
    private const string            CRAFT_COMPLETED_ANIM_NAME           = "crafting_complete";
    private const string            WAITING_FOR_DOCTOR_ANIM_NAME        = "waiting";
    public        FloatParameter    CraftProgress;
    public        LongParameter     Germs;
    public        State             no_operational;
    public        OperationalStates operational;
    public        BoolParameter     UncoverOrderRequested;
    public        BoolParameter     WasUncoverByDuplicant;

    public override void InitializeStates(out BaseState default_state) {
        serializable  = SerializeType.ParamsOnly;
        default_state = no_operational;
        root.Update(GermsRequiredFeedbackUpdate, UpdateRate.SIM_1000ms);
        no_operational.Enter(delegate(Instance smi) {
                                 DisableManualDelivery(smi,
                                                       "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
                             })
                      .TagTransition(GameTags.Operational, operational);

        operational.TagTransition(GameTags.Operational, no_operational, true).DefaultState(operational.covered);
        operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty, null)
                   .ParamTransition(WasUncoverByDuplicant, operational.idle, IsTrue)
                   .Enter(delegate(Instance smi) {
                              DisableManualDelivery(smi,
                                                    "Machine can't ask for materials if it has not been investigated by a dupe");
                          })
                   .DefaultState(operational.covered.idle);

        operational.covered.idle.PlayAnim("dusty")
                   .ParamTransition(UncoverOrderRequested, operational.covered.careOrderGiven, IsTrue);

        operational.covered.careOrderGiven.PlayAnim("dusty")
                   .Enter(StartWorkChore_RevealMachine)
                   .Exit(CancelWorkChore_RevealMachine)
                   .WorkableCompleteTransition(smi => smi.GetWorkable_RevealMachine(), operational.covered.complete)
                   .ParamTransition(UncoverOrderRequested, operational.covered.idle, IsFalse);

        operational.covered.complete.Enter(SetUncovered);
        operational.idle.Enter(delegate(Instance smi) { EnableManualDelivery(smi, "Operational and discovered"); })
                   .EnterTransition(operational.crafting,       ShouldBeCrafting)
                   .EnterTransition(operational.waitingForMorb, IsCraftingCompleted)
                   .EventTransition(GameHashes.OnStorageChange, operational.crafting, ShouldBeCrafting)
                   .PlayAnim("idle")
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);

        operational.crafting.DefaultState(operational.crafting.pre)
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null)
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody,           null);

        operational.crafting.conflict.Enter(ResetRoverBodyCraftingProgress).GoTo(operational.idle);
        operational.crafting.pre
                   .EventTransition(GameHashes.OnStorageChange, operational.crafting.conflict, Not(ShouldBeCrafting))
                   .PlayAnim("crafting_pre")
                   .OnAnimQueueComplete(operational.crafting.loop);

        operational.crafting.loop
                   .EventTransition(GameHashes.OnStorageChange, operational.crafting.conflict, Not(ShouldBeCrafting))
                   .Update(CraftingUpdate)
                   .PlayAnim("crafting_loop", KAnim.PlayMode.Loop)
                   .ParamTransition(CraftProgress, operational.crafting.pst, IsOne);

        operational.crafting.pst.Enter(ConsumeRoverBodyCraftingMaterials)
                   .PlayAnim("crafting_pst")
                   .OnAnimQueueComplete(operational.waitingForMorb);

        operational.waitingForMorb.PlayAnim("crafting_complete")
                   .ParamTransition(Germs, operational.doctor, HasEnoughGerms)
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);

        operational.doctor.Enter(StartWorkChore_ReleaseRover)
                   .Exit(CancelWorkChore_ReleaseRover)
                   .WorkableCompleteTransition(smi => smi.GetWorkable_ReleaseRover(), operational.finish)
                   .DefaultState(operational.doctor.needed);

        operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop)
                   .WorkableStartTransition(smi => smi.GetWorkable_ReleaseRover(), operational.doctor.working)
                   .ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor, null);

        operational.doctor.working.WorkableStopTransition(smi => smi.GetWorkable_ReleaseRover(),
                                                          operational.doctor.needed);

        operational.finish.Enter(SpawnRover).GoTo(operational.idle);
    }

    public static bool ShouldBeCrafting(Instance smi) {
        return smi.HasMaterialsForRover && smi.RoverDevelopment_Progress < 1f;
    }

    public static bool IsCraftingCompleted(Instance smi) { return smi.RoverDevelopment_Progress == 1f; }
    public static bool HasEnoughGerms(Instance smi, long germCount) { return germCount >= smi.def.GERMS_PER_ROVER; }
    public static void StartWorkChore_ReleaseRover(Instance smi) { smi.CreateWorkChore_ReleaseRover(); }
    public static void CancelWorkChore_ReleaseRover(Instance smi) { smi.CancelWorkChore_ReleaseRover(); }
    public static void StartWorkChore_RevealMachine(Instance smi) { smi.CreateWorkChore_RevealMachine(); }
    public static void CancelWorkChore_RevealMachine(Instance smi) { smi.CancelWorkChore_RevealMachine(); }
    public static void SetUncovered(Instance smi) { smi.Uncover(); }
    public static void SpawnRover(Instance smi) { smi.SpawnRover(); }
    public static void EnableManualDelivery(Instance smi, string reason) { smi.EnableManualDelivery(reason); }
    public static void DisableManualDelivery(Instance smi, string reason) { smi.DisableManualDelivery(reason); }
    public static void ConsumeRoverBodyCraftingMaterials(Instance smi) { smi.ConsumeRoverBodyCraftingMaterials(); }
    public static void ResetRoverBodyCraftingProgress(Instance smi) { smi.SetRoverDevelopmentProgress(0f); }

    public static void CraftingUpdate(Instance smi, float dt) {
        var roverDevelopmentProgress
            = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) /
                          smi.def.ROVER_CRAFTING_DURATION,
                          0f,
                          1f);

        smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
    }

    public static void GermsRequiredFeedbackUpdate(Instance smi, float dt) {
        if ((GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT) &
            (smi.MorbDevelopment_Progress                          < 1f)                                         &
            !smi.IsInsideState(smi.sm.operational.doctor)                                                        &
            smi.HasBeenRevealed) {
            smi.ShowGermRequiredStatusItemAlert();
            return;
        }

        smi.HideGermRequiredStatusItemAlert();
    }

    public class Def : BaseDef {
        public float       FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;
        public ConduitType GERM_INTAKE_CONDUIT_TYPE;
        public int         GERM_TYPE;
        public long        GERMS_PER_ROVER;
        public float       INITIAL_MORB_DEVELOPMENT_PERCENTAGE;
        public int         MAX_GERMS_TAKEN_PER_PACKAGE;
        public float       METAL_PER_ROVER;
        public float       ROVER_CRAFTING_DURATION;
        public SimHashes   ROVER_MATERIAL;
        public Tag         ROVER_PREFAB_ID;

        public float GetConduitMaxPackageMass() {
            var germ_INTAKE_CONDUIT_TYPE = GERM_INTAKE_CONDUIT_TYPE;
            if (germ_INTAKE_CONDUIT_TYPE == ConduitType.Gas) return 1f;

            if (germ_INTAKE_CONDUIT_TYPE != ConduitType.Liquid) return 1f;

            return 10f;
        }
    }

    public class CoverStates : State {
        public State careOrderGiven;
        public State complete;
        public State idle;
    }

    public class OperationalStates : State {
        public CoverStates    covered;
        public CraftingStates crafting;
        public DoctorStates   doctor;
        public State          finish;
        public State          idle;
        public State          waitingForMorb;
    }

    public class DoctorStates : State {
        public State needed;
        public State working;
    }

    public class CraftingStates : State {
        public State conflict;
        public State loop;
        public State pre;
        public State pst;
    }

    public new class Instance : GameInstance, ISidescreenButtonControl {
        [MyCmpGet]
        private KBatchedAnimController buildingAnimCtr;

        [MyCmpGet]
        private MorbRoverMaker_Capsule capsule;

        public  Action<long> GermsAdded;
        private Guid         germsRequiredAlertStatusItemHandle;
        private int          inputCell = -1;

        [Serialize]
        private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

        [Serialize]
        private float lastastMaterialsConsumedTemp = -1f;

        public float lastTimeGermsAdded = -1f;

        [MyCmpGet]
        private ManualDeliveryKG manualDelivery;

        public Action<GameObject> OnRoverSpawned;
        public System.Action      OnUncovered;

        [MyCmpGet]
        private Operational operational;

        private          int             outputCell = -1;
        private readonly MeterController RobotProgressMeter;

        [MyCmpGet]
        private KSelectable selectable;

        [MyCmpGet]
        private Storage storage;

        [MyCmpGet]
        private MorbRoverMakerWorkable workable_release;

        [MyCmpGet]
        private MorbRoverMakerRevealWorkable workable_reveal;

        private Chore workChore_releaseRover;
        private Chore workChore_revealMachine;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            RobotProgressMeter = new MeterController(buildingAnimCtr,
                                                     "meter_robot_target",
                                                     "meter_robot",
                                                     Meter.Offset.UserSpecified,
                                                     Grid.SceneLayer.BuildingFront,
                                                     Array.Empty<string>());
        }

        public long MorbDevelopment_GermsCollected => sm.Germs.Get(smi);
        public long MorbDevelopment_RemainingGerms => def.GERMS_PER_ROVER - MorbDevelopment_GermsCollected;

        public float MorbDevelopment_Progress =>
            Mathf.Clamp(MorbDevelopment_GermsCollected / (float)def.GERMS_PER_ROVER, 0f, 1f);

        public bool  HasMaterialsForRover      => storage.GetMassAvailable(def.ROVER_MATERIAL) >= def.METAL_PER_ROVER;
        public float RoverDevelopment_Progress => sm.CraftProgress.Get(smi);
        public bool  HasBeenRevealed           => sm.WasUncoverByDuplicant.Get(smi);
        public bool  CanPumpGerms              => operational && MorbDevelopment_Progress < 1f && HasBeenRevealed;

        public string SidescreenButtonText =>
            HasBeenRevealed                   ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY :
            sm.UncoverOrderRequested.Get(smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN :
                                                CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN;

        public string SidescreenButtonTooltip =>
            HasBeenRevealed
                ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP
                :
                sm.UncoverOrderRequested.Get(smi)
                    ?
                    CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP
                    : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP;

        public bool SidescreenEnabled()                                        { return true; }
        public bool SidescreenButtonInteractable()                             { return true; }
        public int  HorizontalGroupID()                                        { return 0; }
        public int  ButtonSideScreenSortOrder()                                { return 20; }
        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { throw new NotImplementedException(); }

        public void OnSidescreenButtonPressed() {
            if (HasBeenRevealed) {
                storage.DropAll();
                return;
            }

            var flag = smi.sm.UncoverOrderRequested.Get(smi);
            smi.sm.UncoverOrderRequested.Set(!flag, smi);
        }

        public Workable GetWorkable_RevealMachine() { return workable_reveal; }
        public Workable GetWorkable_ReleaseRover()  { return workable_release; }

        public void ShowGermRequiredStatusItemAlert() {
            if (germsRequiredAlertStatusItemHandle == default(Guid))
                germsRequiredAlertStatusItemHandle
                    = selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, smi);
        }

        public void HideGermRequiredStatusItemAlert() {
            if (germsRequiredAlertStatusItemHandle != default(Guid)) {
                selectable.RemoveStatusItem(germsRequiredAlertStatusItemHandle);
                germsRequiredAlertStatusItemHandle = default(Guid);
            }
        }

        public override void StartSM() {
            var component = GetComponent<Building>();
            inputCell  = component.GetUtilityInputCell();
            outputCell = component.GetUtilityOutputCell();
            base.StartSM();
            if (!HasBeenRevealed) {
                sm.Germs.Set(0L, smi);
                AddGerms((long)(def.GERMS_PER_ROVER * def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
            }

            Conduit.GetFlowManager(def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(Flow);
            UpdateMeters();
        }

        public void AddGerms(long amount, bool playAnimations = true) {
            var value = MorbDevelopment_GermsCollected + amount;
            sm.Germs.Set(value, smi);
            UpdateMeters();
            if (amount > 0L) {
                if (playAnimations) capsule.PlayPumpGermsAnimation();
                var germsAdded = GermsAdded;
                if (germsAdded != null) germsAdded(amount);
                lastTimeGermsAdded = GameClock.Instance.GetTime();
            }
        }

        public long RemoveGerms(long amount) {
            var num   = amount.Min(MorbDevelopment_GermsCollected);
            var value = MorbDevelopment_GermsCollected - num;
            sm.Germs.Set(value, smi);
            UpdateMeters();
            return num;
        }

        public void EnableManualDelivery(string  reason) { manualDelivery.Pause(false, reason); }
        public void DisableManualDelivery(string reason) { manualDelivery.Pause(true,  reason); }

        public void SetRoverDevelopmentProgress(float value) {
            sm.CraftProgress.Set(value, smi);
            UpdateMeters();
        }

        public void UpdateMeters() {
            RobotProgressMeter.SetPositionPercent(RoverDevelopment_Progress);
            capsule.SetMorbDevelopmentProgress(MorbDevelopment_Progress);
            capsule.SetGermMeterProgress(HasBeenRevealed ? MorbDevelopment_Progress : 0f);
        }

        public void Uncover() {
            sm.WasUncoverByDuplicant.Set(true, smi);
            var onUncovered = OnUncovered;
            if (onUncovered == null) return;

            onUncovered();
        }

        public void CreateWorkChore_ReleaseRover() {
            if (workChore_releaseRover == null)
                workChore_releaseRover
                    = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, workable_release);
        }

        public void CancelWorkChore_ReleaseRover() {
            if (workChore_releaseRover != null) {
                workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
                workChore_releaseRover = null;
            }
        }

        public void CreateWorkChore_RevealMachine() {
            if (workChore_revealMachine == null)
                workChore_revealMachine
                    = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, workable_reveal);
        }

        public void CancelWorkChore_RevealMachine() {
            if (workChore_revealMachine != null) {
                workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
                workChore_revealMachine = null;
            }
        }

        public void ConsumeRoverBodyCraftingMaterials() {
            var num = 0f;
            storage.ConsumeAndGetDisease(def.ROVER_MATERIAL.CreateTag(),
                                         def.METAL_PER_ROVER,
                                         out num,
                                         out lastastMaterialsConsumedDiseases,
                                         out lastastMaterialsConsumedTemp);
        }

        public void SpawnRover() {
            if (RoverDevelopment_Progress == 1f) {
                RemoveGerms(def.GERMS_PER_ROVER);
                var gameObject = GameUtil.KInstantiate(Assets.GetPrefab(def.ROVER_PREFAB_ID),
                                                       this.gameObject.transform.GetPosition(),
                                                       Grid.SceneLayer.Creatures);

                var component = gameObject.GetComponent<PrimaryElement>();
                if (lastastMaterialsConsumedDiseases.idx != 255)
                    component.AddDisease(lastastMaterialsConsumedDiseases.idx,
                                         lastastMaterialsConsumedDiseases.count,
                                         "From the materials provided for its creation");

                if (lastastMaterialsConsumedTemp > 0f)
                    component.SetMassTemperature(component.Mass, lastastMaterialsConsumedTemp);

                gameObject.SetActive(true);
                SetRoverDevelopmentProgress(0f);
                var onRoverSpawned = OnRoverSpawned;
                if (onRoverSpawned == null) return;

                onRoverSpawned(gameObject);
            }
        }

        private void Flow(float dt) {
            if (CanPumpGerms) {
                var flowManager = Conduit.GetFlowManager(def.GERM_INTAKE_CONDUIT_TYPE);
                var num         = 0;
                if (flowManager.HasConduit(inputCell) && flowManager.HasConduit(outputCell)) {
                    var contents  = flowManager.GetContents(inputCell);
                    var contents2 = flowManager.GetContents(outputCell);
                    var num2      = Mathf.Min(contents.mass, def.GetConduitMaxPackageMass() * dt);
                    if (flowManager.CanMergeContents(contents, contents2, num2)) {
                        var amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
                        if (amountAllowedForMerging > 0f) {
                            var conduitFlow = def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid
                                                  ? Game.Instance.liquidConduitFlow
                                                  : Game.Instance.gasConduitFlow;

                            var num3 = contents.diseaseCount;
                            if (contents.diseaseIdx != 255 && contents.diseaseIdx == def.GERM_TYPE) {
                                num = (int)MorbDevelopment_RemainingGerms.Min(def.MAX_GERMS_TAKEN_PER_PACKAGE)
                                                                         .Min(contents.diseaseCount);

                                num3 -= num;
                            }

                            var num4 = conduitFlow.AddElement(outputCell,
                                                              contents.element,
                                                              amountAllowedForMerging,
                                                              contents.temperature,
                                                              contents.diseaseIdx,
                                                              num3);

                            if (amountAllowedForMerging != num4)
                                Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4));

                            flowManager.RemoveElement(inputCell, num4);
                        }
                    }
                }

                if (num > 0) AddGerms(num);
            }
        }

        protected override void OnCleanUp() {
            base.OnCleanUp();
            Conduit.GetFlowManager(def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(Flow);
        }
    }
}