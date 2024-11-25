public class BeeHiveMonitor
    : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def> {
    public State idle;
    public State night;

    public override void InitializeStates(out BaseState default_state) {
        default_state = idle;
        idle.EventTransition(GameHashes.Nighttime,
                             smi => GameClock.Instance,
                             night,
                             smi => GameClock.Instance.IsNighttime());

        night.EventTransition(GameHashes.NewDay,
                              smi => GameClock.Instance,
                              idle,
                              smi => !GameClock.Instance.IsNighttime())
             .ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, ShouldMakeHome);
    }

    public bool ShouldMakeHome(Instance smi) { return !CanGoHome(smi); }
    public bool CanGoHome(Instance      smi) { return smi.gameObject.GetComponent<Bee>().FindHiveInRoom() != null; }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        public Instance(IStateMachineTarget master, Def def) : base(master, def) { }
    }
}