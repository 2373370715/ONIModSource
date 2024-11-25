public class SimpleDoorController
    : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget,
        SimpleDoorController.Def> {
    public ActiveStates active;
    public State        inactive;
    public IntParameter numOpens;

    public override void InitializeStates(out BaseState default_state) {
        default_state = inactive;
        inactive.TagTransition(GameTags.RocketOnGround, active);
        active.DefaultState(active.closed)
              .TagTransition(GameTags.RocketOnGround, inactive, true)
              .Enter(delegate(StatesInstance smi) { smi.Register(); })
              .Exit(delegate(StatesInstance  smi) { smi.Unregister(); });

        active.closed.PlayAnim(smi => smi.GetDefaultAnim(), KAnim.PlayMode.Loop)
              .ParamTransition(numOpens, active.opening, (smi, p) => p > 0);

        active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(active.open);
        active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop)
              .ParamTransition(numOpens, active.closedelay, (smi, p) => p == 0);

        active.closedelay.ParamTransition(numOpens, active.open, (smi, p) => p > 0).ScheduleGoTo(0.5f, active.closing);
        active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(active.closed);
    }

    public class Def : BaseDef { }

    public class ActiveStates : State {
        public State closed;
        public State closedelay;
        public State closing;
        public State open;
        public State opening;
    }

    public class StatesInstance : GameInstance, INavDoor {
        public StatesInstance(IStateMachineTarget master, Def def) : base(master, def) { }
        public bool isSpawned => master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;
        public void Close()   { sm.numOpens.Delta(-1, smi); }
        public bool IsOpen()  { return IsInsideState(sm.active.open) || IsInsideState(sm.active.closedelay); }
        public void Open()    { sm.numOpens.Delta(1, smi); }

        public string GetDefaultAnim() {
            var component = master.GetComponent<KBatchedAnimController>();
            if (component != null) return component.initialAnim;

            return "idle_loop";
        }

        public void Register() {
            var i = Grid.PosToCell(gameObject.transform.GetPosition());
            Grid.HasDoor[i] = true;
        }

        public void Unregister() {
            var i = Grid.PosToCell(gameObject.transform.GetPosition());
            Grid.HasDoor[i] = false;
        }
    }
}