using UnityEngine;

public class DeliverToSweepLockerStates
    : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget,
        DeliverToSweepLockerStates.Def> {
    public State behaviourcomplete;
    public State idle;
    public State lockerFull;
    public State movingToStorage;
    public State unloading;

    public override void InitializeStates(out BaseState default_state) {
        default_state = movingToStorage;
        idle.ScheduleGoTo(1f, movingToStorage);
        movingToStorage.MoveTo(delegate(Instance smi) {
                                   if (!(GetSweepLocker(smi) == null)) return Grid.PosToCell(GetSweepLocker(smi));

                                   return Grid.InvalidCell;
                               },
                               unloading,
                               idle);

        unloading.Enter(delegate(Instance smi) {
                            var sweepLocker = GetSweepLocker(smi);
                            if (sweepLocker == null) {
                                smi.GoTo(behaviourcomplete);
                                return;
                            }

                            var storage = smi.master.gameObject.GetComponents<Storage>()[1];
                            var num = Mathf.Max(0f,
                                                Mathf.Min(storage.ExactMassStored(), sweepLocker.RemainingCapacity()));

                            for (var i = storage.items.Count - 1; i >= 0; i--) {
                                var gameObject = storage.items[i];
                                if (!(gameObject == null)) {
                                    var num2 = Mathf.Min(gameObject.GetComponent<PrimaryElement>().Mass, num);
                                    if (num2 != 0f)
                                        storage.Transfer(sweepLocker,
                                                         gameObject.GetComponent<KPrefabID>().PrefabTag,
                                                         num2);

                                    num -= num2;
                                    if (num <= 0f) break;
                                }
                            }

                            smi.master.GetComponent<KBatchedAnimController>().Play("dropoff");
                            smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
                            sweepLocker.GetComponent<KBatchedAnimController>().Play("dropoff");
                            if (storage.MassStored() > 0f) {
                                smi.ScheduleGoTo(2f, lockerFull);
                                return;
                            }

                            smi.ScheduleGoTo(2f, behaviourcomplete);
                        });

        lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(movingToStorage);
        behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour);
    }

    public Storage GetSweepLocker(Instance smi) {
        var smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
        if (smi2 == null) return null;

        return smi2.sm.sweepLocker.Get(smi2);
    }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        public Instance(Chore<Instance> chore, Def def) : base(chore, def) {
            chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition,
                                  GameTags.Robots.Behaviours.UnloadBehaviour);
        }

        public override void StartSM() {
            base.StartSM();
            GetComponent<KSelectable>()
                .SetStatusItem(Db.Get().StatusItemCategories.Main,
                               Db.Get().RobotStatusItems.UnloadingStorage,
                               gameObject);
        }

        protected override void OnCleanUp() {
            base.OnCleanUp();
            GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage);
        }
    }
}