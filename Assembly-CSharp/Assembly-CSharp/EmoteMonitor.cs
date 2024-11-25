using UnityEngine;

public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance> {
    public State ready;
    public State satisfied;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        serializable  = SerializeType.Both_DEPRECATED;
        satisfied.ScheduleGoTo(Random.Range(30, 90), ready);
        ready.ToggleUrge(Db.Get().Urges.Emote)
             .EventHandler(GameHashes.BeginChore, delegate(Instance smi, object o) { smi.OnStartChore(o); });
    }

    public new class Instance : GameInstance {
        public Instance(IStateMachineTarget master) : base(master) { }

        public void OnStartChore(object o) {
            if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Emote)) GoTo(sm.satisfied);
        }
    }
}