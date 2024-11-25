using System;
using Klei.AI;
using TUNING;

public class PartyChore : Chore<PartyChore.StatesInstance>, IWorkerPrioritizable {
    public const string specificEffect = "Socialized";
    public const string trackingEffect = "RecentlySocialized";
    public       int    basePriority   = RELAXATION.PRIORITY.SPECIAL_EVENT;

    public PartyChore(IStateMachineTarget master,
                      Workable            chat_workable,
                      Action<Chore>       on_complete = null,
                      Action<Chore>       on_begin    = null,
                      Action<Chore>       on_end      = null) : base(Db.Get().ChoreTypes.Party,
                                                                     master,
                                                                     master.GetComponent<ChoreProvider>(),
                                                                     true,
                                                                     on_complete,
                                                                     on_begin,
                                                                     on_end,
                                                                     PriorityScreen.PriorityClass.high,
                                                                     5,
                                                                     false,
                                                                     true,
                                                                     0,
                                                                     false,
                                                                     ReportManager.ReportType.PersonalTime) {
        smi = new StatesInstance(this);
        smi.sm.chitchatlocator.Set(chat_workable, smi);
        AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
        AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
    }

    public bool GetWorkerPriority(WorkerBase worker, out int priority) {
        priority = basePriority;
        return true;
    }

    public override void Begin(Precondition.Context context) {
        smi.sm.partyer.Set(context.consumerState.gameObject, smi);
        base.Begin(context);
        smi.sm.partyer.Get(smi).gameObject.AddTag(GameTags.Partying);
    }

    protected override void End(string reason) {
        if (smi.sm.partyer.Get(smi) != null) smi.sm.partyer.Get(smi).gameObject.RemoveTag(GameTags.Partying);
        base.End(reason);
    }

    public class States : GameStateMachine<States, StatesInstance, PartyChore> {
        public State                           chat;
        public ApproachSubState<IApproachable> chat_move;
        public TargetParameter                 chitchatlocator;
        public TargetParameter                 partyer;
        public ApproachSubState<IApproachable> stand;
        public State                           success;

        public override void InitializeStates(out BaseState default_state) {
            default_state = stand;
            Target(partyer);
            stand.InitializeStates(partyer, masterTarget, chat);
            chat_move.InitializeStates(partyer, chitchatlocator, chat);
            chat.ToggleWork<Workable>(chitchatlocator, success, null, null);
            success.Enter(delegate(StatesInstance smi) {
                              partyer.Get(smi).gameObject.GetComponent<Effects>().Add("RecentlyPartied", true);
                          })
                   .ReturnSuccess();
        }
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, PartyChore, object>.GameInstance {
        public StatesInstance(PartyChore master) : base(master) { }
    }
}