using System;
using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class SocialGatheringPoint : StateMachineComponent<SocialGatheringPoint.StatesInstance> {
    public  int                            basePriority;
    public  int                            choreCount   = 2;
    public  CellOffset[]                   choreOffsets = { new CellOffset(0, 0), new CellOffset(1, 0) };
    public  System.Action                  OnSocializeBeginCB;
    public  System.Action                  OnSocializeEndCB;
    public  string                         socialEffect;
    private SocialChoreTracker             tracker;
    private SocialGatheringPointWorkable[] workables;
    public  float                          workTime = 15f;

    protected override void OnSpawn() {
        base.OnSpawn();
        workables = new SocialGatheringPointWorkable[choreOffsets.Length];
        for (var i = 0; i < workables.Length; i++) {
            var pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), choreOffsets[i]), Grid.SceneLayer.Move);
            var socialGatheringPointWorkable = ChoreHelpers.CreateLocator("SocialGatheringPointWorkable", pos)
                                                           .AddOrGet<SocialGatheringPointWorkable>();

            socialGatheringPointWorkable.basePriority      = basePriority;
            socialGatheringPointWorkable.specificEffect    = socialEffect;
            socialGatheringPointWorkable.OnWorkableEventCB = OnWorkableEvent;
            socialGatheringPointWorkable.SetWorkTime(workTime);
            workables[i] = socialGatheringPointWorkable;
        }

        tracker               = new SocialChoreTracker(gameObject, choreOffsets);
        tracker.choreCount    = choreCount;
        tracker.CreateChoreCB = CreateChore;
        smi.StartSM();
        Components.SocialGatheringPoints.Add(Grid.WorldIdx[Grid.PosToCell(this)], this);
    }

    protected override void OnCleanUp() {
        if (tracker != null) {
            tracker.Clear();
            tracker = null;
        }

        if (workables != null)
            for (var i = 0; i < workables.Length; i++)
                if (workables[i]) {
                    Util.KDestroyGameObject(workables[i]);
                    workables[i] = null;
                }

        Components.SocialGatheringPoints.Remove(Grid.WorldIdx[Grid.PosToCell(this)], this);
        base.OnCleanUp();
    }

    private Chore CreateChore(int i) {
        Workable            workable           = workables[i];
        var                 relax              = Db.Get().ChoreTypes.Relax;
        IStateMachineTarget target             = workable;
        ChoreProvider       chore_provider     = null;
        var                 run_until_complete = true;
        Action<Chore>       on_complete        = null;
        Action<Chore>       on_begin           = null;
        var                 recreation         = Db.Get().ScheduleBlockTypes.Recreation;
        var workChore = new WorkChore<SocialGatheringPointWorkable>(relax,
                                                                    target,
                                                                    chore_provider,
                                                                    run_until_complete,
                                                                    on_complete,
                                                                    on_begin,
                                                                    OnSocialChoreEnd,
                                                                    false,
                                                                    recreation,
                                                                    false,
                                                                    true,
                                                                    null,
                                                                    false,
                                                                    true,
                                                                    false,
                                                                    PriorityScreen.PriorityClass.high,
                                                                    5,
                                                                    false,
                                                                    false);

        workChore.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
        workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
        workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
        return workChore;
    }

    private void OnSocialChoreEnd(Chore chore) {
        if (smi.IsInsideState(smi.sm.on)) tracker.Update();
    }

    private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event) {
        if (workable_event == Workable.WorkableEvent.WorkStarted) {
            if (OnSocializeBeginCB != null) OnSocializeBeginCB();
        } else if (workable_event == Workable.WorkableEvent.WorkStopped && OnSocializeEndCB != null) OnSocializeEndCB();
    }

    public class States : GameStateMachine<States, StatesInstance, SocialGatheringPoint> {
        public State off;
        public State on;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            root.DoNothing();
            off.TagTransition(GameTags.Operational, on);
            on.TagTransition(GameTags.Operational, off, true)
              .Enter("CreateChore", delegate(StatesInstance smi) { smi.master.tracker.Update(); })
              .Exit("CancelChore", delegate(StatesInstance  smi) { smi.master.tracker.Update(false); });
        }
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, SocialGatheringPoint, object>.GameInstance {
        public StatesInstance(SocialGatheringPoint smi) : base(smi) { }
    }
}