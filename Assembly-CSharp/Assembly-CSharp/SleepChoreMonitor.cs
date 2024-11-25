using System.Collections.Generic;
using UnityEngine;

public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance> {
    public TargetParameter bed;
    public State           bedassigned;
    public State           checkforbed;
    public State           passingout;
    public State           passingout_bedassigned;
    public State           satisfied;
    public State           sleeponfloor;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        serializable  = SerializeType.Never;
        root.EventHandler(GameHashes.AssignablesChanged, delegate(Instance smi) { smi.UpdateBed(); });
        satisfied.EventTransition(GameHashes.AddUrge, checkforbed, smi => smi.HasSleepUrge());
        checkforbed.Enter("SetBed",
                          delegate(Instance smi) {
                              smi.UpdateBed();
                              if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep()) {
                                  if (bed.Get(smi) != null && smi.IsBedReachable()) {
                                      smi.GoTo(passingout_bedassigned);
                                      return;
                                  }

                                  smi.GoTo(passingout);
                                  return;
                              }

                              if (bed.Get(smi) == null || !smi.IsBedReachable()) {
                                  smi.GoTo(sleeponfloor);
                                  return;
                              }

                              smi.GoTo(bedassigned);
                          });

        passingout.EventTransition(GameHashes.AssignablesChanged, checkforbed)
                  .EventHandlerTransition(GameHashes.AssignableReachabilityChanged,
                                          checkforbed,
                                          (smi, data) => smi.IsBedReachable())
                  .ToggleChore(CreatePassingOutChore, satisfied, satisfied);

        passingout_bedassigned.ParamTransition(bed, checkforbed, (smi, p) => p == null)
                              .EventTransition(GameHashes.AssignablesChanged, checkforbed)
                              .EventTransition(GameHashes.AssignableReachabilityChanged,
                                               checkforbed,
                                               smi => !smi.IsBedReachable())
                              .ToggleChore(CreateExhaustedSleepChore, satisfied, satisfied);

        sleeponfloor.EventTransition(GameHashes.AssignablesChanged, checkforbed)
                    .EventHandlerTransition(GameHashes.AssignableReachabilityChanged,
                                            checkforbed,
                                            (smi, data) => smi.IsBedReachable())
                    .ToggleChore(CreateSleepOnFloorChore, satisfied, satisfied);

        bedassigned.ParamTransition(bed, checkforbed, (smi, p) => p == null)
                   .EventTransition(GameHashes.AssignablesChanged,            checkforbed)
                   .EventTransition(GameHashes.AssignableReachabilityChanged, checkforbed, smi => !smi.IsBedReachable())
                   .ToggleChore(CreateSleepChore, satisfied, satisfied);
    }

    private Chore CreatePassingOutChore(Instance smi) {
        var gameObject = smi.CreatePassedOutLocator();
        return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, false);
    }

    private Chore CreateSleepOnFloorChore(Instance smi) {
        var gameObject = smi.CreateFloorLocator();
        return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, true);
    }

    private Chore CreateSleepChore(Instance smi) {
        return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, bed.Get(smi), false, true);
    }

    private Chore CreateExhaustedSleepChore(Instance smi) {
        return new SleepChore(Db.Get().ChoreTypes.Sleep,
                              smi.master,
                              bed.Get(smi),
                              false,
                              true,
                              new[] { Db.Get().DuplicantStatusItems.SleepingExhausted });
    }

    public new class Instance : GameInstance {
        public  GameObject locator;
        private int        locatorCell;
        public Instance(IStateMachineTarget master) : base(master) { }

        public void UpdateBed() {
            var        soleOwner  = sm.masterTarget.Get(smi).GetComponent<MinionIdentity>().GetSoleOwner();
            var        assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
            Assignable assignable2;
            if (assignable != null &&
                assignable.CanAutoAssignTo(sm.masterTarget.Get(smi)
                                             .GetComponent<MinionIdentity>()
                                             .assignableProxy.Get()))
                assignable2 = assignable;
            else {
                assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
                if (assignable2 == null) {
                    assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
                    if (assignable2 != null) {
                        var sensor = GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
                        if (sensor.IsEnabled) sensor.Update();
                    }
                }
            }

            smi.sm.bed.Set(assignable2, smi);
        }

        public bool HasSleepUrge() { return GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep); }

        public bool IsBedReachable() {
            var sensor = GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
            return sensor.IsReachable(Db.Get().AssignableSlots.Bed) ||
                   sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
        }

        public GameObject CreatePassedOutLocator() {
            var safeFloorLocator = SleepChore.GetSafeFloorLocator(master.gameObject);
            safeFloorLocator.effectName    = "PassedOutSleep";
            safeFloorLocator.wakeEffects   = new List<string> { "SoreBack" };
            safeFloorLocator.stretchOnWake = false;
            return safeFloorLocator.gameObject;
        }

        public GameObject CreateFloorLocator() {
            var safeFloorLocator = SleepChore.GetSafeFloorLocator(master.gameObject);
            safeFloorLocator.effectName    = "FloorSleep";
            safeFloorLocator.wakeEffects   = new List<string> { "SoreBack" };
            safeFloorLocator.stretchOnWake = false;
            return safeFloorLocator.gameObject;
        }
    }
}