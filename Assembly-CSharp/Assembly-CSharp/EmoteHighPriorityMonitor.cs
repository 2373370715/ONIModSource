public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance> {
    public State ready;
    public State resetting;

    public override void InitializeStates(out BaseState default_state) {
        default_state = ready;
        serializable  = SerializeType.Both_DEPRECATED;
        ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority)
             .EventHandler(GameHashes.BeginChore, delegate(Instance smi, object o) { smi.OnStartChore(o); });

        resetting.GoTo(ready);
    }

    public new class Instance : GameInstance {
        public Instance(IStateMachineTarget master) : base(master) { }

        public void OnStartChore(object o) {
            if (((Chore)o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority)) GoTo(sm.resetting);
        }
    }
}