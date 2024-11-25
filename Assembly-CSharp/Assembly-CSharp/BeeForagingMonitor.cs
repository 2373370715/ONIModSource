using UnityEngine;

public class BeeForagingMonitor
    : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def> {
    public override void InitializeStates(out BaseState default_state) {
        default_state = root;
        root.ToggleBehaviour(GameTags.Creatures.WantsToForage,
                             ShouldForage,
                             delegate(Instance smi) { smi.RefreshSearchTime(); });
    }

    public static bool ShouldForage(Instance smi) {
        var flag      = GameClock.Instance.GetTimeInCycles() >= smi.nextSearchTime;
        var kprefabID = smi.master.GetComponent<Bee>().FindHiveInRoom();
        if (kprefabID != null) {
            var smi2                                   = kprefabID.GetSMI<BeehiveCalorieMonitor.Instance>();
            if (smi2 == null || !smi2.IsHungry()) flag = false;
        }

        return flag && kprefabID != null;
    }

    public class Def : BaseDef {
        public float searchMaxInterval = 0.3f;
        public float searchMinInterval = 0.25f;
    }

    public new class Instance : GameInstance {
        public float nextSearchTime;
        public Instance(IStateMachineTarget master, Def def) : base(master, def) { RefreshSearchTime(); }

        public void RefreshSearchTime() {
            nextSearchTime = GameClock.Instance.GetTimeInCycles() +
                             Mathf.Lerp(def.searchMinInterval, def.searchMaxInterval, Random.value);
        }
    }
}