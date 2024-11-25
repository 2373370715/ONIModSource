using UnityEngine;

public class BeInBatterySaveModeChore : Chore<BeInBatterySaveModeChore.StatesInstance> {
    public const string EFFECT_NAME = "BionicBatterySaveMode";

    public BeInBatterySaveModeChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeBatterySaveMode,
                                                                       master,
                                                                       master.GetComponent<ChoreProvider>(),
                                                                       true,
                                                                       null,
                                                                       null,
                                                                       null,
                                                                       PriorityScreen.PriorityClass.personalNeeds) {
        smi = new StatesInstance(this, master.gameObject);
        AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
    }

    public static bool IsBatteryMonitorWaitingForUsToExit(StatesInstance smi, float dt) {
        return smi.batteryMonitor.IsInsideState(smi.batteryMonitor.sm.online.batterySaveMode.idle.exit);
    }

    public static string GetEnterAnim(StatesInstance smi) {
        var currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
        if (currentNavType != NavType.Ladder) { }

        return "low_power_pre";
    }

    public static string GetIdleAnim(StatesInstance smi) {
        var currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
        if (currentNavType != NavType.Ladder) { }

        return "low_power_loop";
    }

    public static string GetExitAnim(StatesInstance smi) {
        var currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
        if (currentNavType != NavType.Ladder) { }

        return "low_power_pst";
    }

    public class States : GameStateMachine<States, StatesInstance, BeInBatterySaveModeChore> {
        public State end;
        public State enter;
        public State exit;
        public State idle;

        public override void InitializeStates(out BaseState default_state) {
            default_state = enter;
            root.ToggleTag(GameTags.BatterySaveMode)
                .TriggerOnEnter(GameHashes.BionicBatterySaveModeChanged, smi => true)
                .TriggerOnExit(GameHashes.BionicBatterySaveModeChanged, smi => false)
                .ToggleEffect("BionicBatterySaveMode");

            enter.ToggleAnims("anim_bionic_kanim").PlayAnim(GetEnterAnim).OnAnimQueueComplete(idle);
            idle.ToggleAnims("anim_bionic_kanim")
                .PlayAnim(GetIdleAnim, KAnim.PlayMode.Loop)
                .UpdateTransition(exit, IsBatteryMonitorWaitingForUsToExit, UpdateRate.SIM_1000ms);

            exit.ToggleAnims("anim_bionic_kanim").PlayAnim(GetExitAnim).OnAnimQueueComplete(end);
            end.ReturnSuccess();
        }
    }

    public class StatesInstance
        : GameStateMachine<States, StatesInstance, BeInBatterySaveModeChore, object>.GameInstance {
        public BionicBatteryMonitor.Instance batteryMonitor;

        public StatesInstance(BeInBatterySaveModeChore master, GameObject duplicant) : base(master) {
            batteryMonitor = duplicant.GetSMI<BionicBatteryMonitor.Instance>();
        }
    }
}