using System.Collections.Generic;

public class PartyCake : GameStateMachine<PartyCake, PartyCake.StatesInstance, IStateMachineTarget, PartyCake.Def> {
    public State          complete;
    public CreatingStates creating;
    public State          party;
    public State          ready_to_party;

    public override void InitializeStates(out BaseState default_state) {
        default_state = creating.ready;
        creating.ready.PlayAnim("base").GoTo(creating.tier1);
        creating.tier1.InitializeStates(masterTarget, "tier_1", creating.tier2);
        creating.tier2.InitializeStates(masterTarget, "tier_2", creating.tier3);
        creating.tier3.InitializeStates(masterTarget, "tier_3", ready_to_party);
        ready_to_party.PlayAnim("unlit");
        party.PlayAnim("lit");
        complete.PlayAnim("finished");
    }

    private static Chore CreateFetchChore(StatesInstance smi) {
        return new FetchChore(Db.Get().ChoreTypes.FarmFetch,
                              smi.GetComponent<Storage>(),
                              10f,
                              new HashSet<Tag> { "MushBar".ToTag() },
                              FetchChore.MatchCriteria.MatchID,
                              Tag.Invalid,
                              null,
                              null,
                              true,
                              null,
                              null,
                              null,
                              Operational.State.Functional);
    }

    private static Chore CreateWorkChore(StatesInstance smi) {
        Workable component = smi.master.GetComponent<PartyCakeWorkable>();
        return new WorkChore<PartyCakeWorkable>(Db.Get().ChoreTypes.Cook,
                                                component,
                                                null,
                                                true,
                                                null,
                                                null,
                                                null,
                                                false,
                                                Db.Get().ScheduleBlockTypes.Work,
                                                false,
                                                true,
                                                null,
                                                false,
                                                true,
                                                false,
                                                PriorityScreen.PriorityClass.high);
    }

    public class Def : BaseDef { }

    public class CreatingStates : State {
        public State finish;
        public State ready;
        public Tier  tier1;
        public Tier  tier2;
        public Tier  tier3;

        public class Tier : State {
            public State fetch;
            public State work;

            public State InitializeStates(TargetParameter targ, string animPrefix, State success) {
                root.Target(targ).DefaultState(fetch);
                fetch.PlayAnim(animPrefix + "_ready").ToggleChore(CreateFetchChore, work);
                work.Enter(delegate(StatesInstance smi) {
                               var component = smi.GetComponent<KBatchedAnimController>();
                               component.Play(animPrefix + "_working");
                               component.SetPositionPercent(0f);
                           })
                    .ToggleChore(CreateWorkChore, success, work);

                return this;
            }
        }
    }

    public class StatesInstance : GameInstance {
        public StatesInstance(IStateMachineTarget smi, Def def) : base(smi, def) { }
    }
}