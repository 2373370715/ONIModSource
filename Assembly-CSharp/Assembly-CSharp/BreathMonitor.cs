using Klei.AI;
using TUNING;

public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance> {
    public LowBreathState lowbreath;
    public IntParameter   recoverBreathCell;
    public SatisfiedState satisfied;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        satisfied.DefaultState(satisfied.full).Transition(lowbreath, IsLowBreath);
        satisfied.full.Transition(satisfied.notfull, IsNotFullBreath).Enter(HideBreathBar);
        satisfied.notfull.Transition(satisfied.full, IsFullBreath).Enter(ShowBreathBar);
        lowbreath.DefaultState(lowbreath.nowheretorecover)
                 .Transition(satisfied, IsFullBreath)
                 .ToggleExpression(Db.Get().Expressions.RecoverBreath, IsNotInBreathableArea)
                 .ToggleUrge(Db.Get().Urges.RecoverBreath)
                 .ToggleThought(Db.Get().Thoughts.Suffocating)
                 .ToggleTag(GameTags.HoldingBreath)
                 .Enter(ShowBreathBar)
                 .Enter(UpdateRecoverBreathCell)
                 .Update(UpdateRecoverBreathCell, UpdateRate.RENDER_1000ms, true);

        lowbreath.nowheretorecover.ParamTransition(recoverBreathCell, lowbreath.recoveryavailable, IsValidRecoverCell);
        lowbreath.recoveryavailable
                 .ParamTransition(recoverBreathCell, lowbreath.nowheretorecover, IsNotValidRecoverCell)
                 .Enter(UpdateRecoverBreathCell)
                 .ToggleChore(CreateRecoverBreathChore, lowbreath.nowheretorecover);
    }

    private static bool IsLowBreath(Instance smi) {
        var myWorld = smi.master.gameObject.GetMyWorld();
        if (!(myWorld == null) && myWorld.AlertManager.IsRedAlert())
            return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;

        return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.RETREAT_AMOUNT;
    }

    private static Chore CreateRecoverBreathChore(Instance smi) { return new RecoverBreathChore(smi.master); }
    private static bool  IsNotFullBreath(Instance          smi) { return !IsFullBreath(smi); }
    private static bool  IsFullBreath(Instance             smi) { return smi.breath.value >= smi.breath.GetMax(); }

    private static bool IsNotInBreathableArea(Instance smi) {
        return smi.breather.IsSuffocating || !smi.breather.IsBreathableElementAtCell(Grid.PosToCell(smi));
    }

    private static void ShowBreathBar(Instance smi) {
        if (NameDisplayScreen.Instance != null)
            NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, smi.GetBreath, true);
    }

    private static void HideBreathBar(Instance smi) {
        if (NameDisplayScreen.Instance != null)
            NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, null, false);
    }

    private static bool IsValidRecoverCell(Instance      smi, int   cell) { return cell != Grid.InvalidCell; }
    private static bool IsNotValidRecoverCell(Instance   smi, int   cell) { return !IsValidRecoverCell(smi, cell); }
    private static void UpdateRecoverBreathCell(Instance smi, float dt)   { UpdateRecoverBreathCell(smi); }

    private static void UpdateRecoverBreathCell(Instance smi) {
        if (smi.canRecoverBreath) {
            smi.query.Reset();
            smi.navigator.RunQuery(smi.query);
            var num                                               = smi.query.GetResultCell();
            if (!smi.breather.IsBreathableElementAtCell(num)) num = PathFinder.InvalidCell;
            smi.sm.recoverBreathCell.Set(num, smi);
        }
    }

    public class LowBreathState : State {
        public State nowheretorecover;
        public State recoveryavailable;
    }

    public class SatisfiedState : State {
        public State full;
        public State notfull;
    }

    public new class Instance : GameInstance {
        public AmountInstance breath;
        public OxygenBreather breather;
        public bool           canRecoverBreath = true;
        public Navigator      navigator;
        public SafetyQuery    query;

        public Instance(IStateMachineTarget master) : base(master) {
            breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
            query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker,
                                    GetComponent<KMonoBehaviour>(),
                                    int.MaxValue);

            navigator = GetComponent<Navigator>();
            breather  = GetComponent<OxygenBreather>();
        }

        public int   GetRecoverCell() { return sm.recoverBreathCell.Get(smi); }
        public float GetBreath()      { return breath.value / breath.GetMax(); }
    }
}