using Klei.AI;
using STRINGS;

public class BeeHappinessMonitor
    : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget,
        BeeHappinessMonitor.Def> {
    private State  happy;
    private Effect happyEffect;
    private Effect neutralEffect;
    private State  satisfied;
    private State  unhappy;
    private Effect unhappyEffect;

    public override void InitializeStates(out BaseState default_state) {
        default_state = satisfied;
        satisfied.TriggerOnEnter(GameHashes.Satisfied)
                 .Transition(happy,   IsHappy,   UpdateRate.SIM_1000ms)
                 .Transition(unhappy, IsUnhappy, UpdateRate.SIM_1000ms)
                 .ToggleEffect(smi => neutralEffect);

        happy.TriggerOnEnter(GameHashes.Happy)
             .ToggleEffect(smi => happyEffect)
             .Transition(satisfied, Not(IsHappy), UpdateRate.SIM_1000ms);

        unhappy.TriggerOnEnter(GameHashes.Unhappy)
               .Transition(satisfied, Not(IsUnhappy), UpdateRate.SIM_1000ms)
               .ToggleEffect(smi => unhappyEffect);

        happyEffect = new Effect("Happy",
                                 CREATURES.MODIFIERS.HAPPY_WILD.NAME,
                                 CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP,
                                 0f,
                                 true,
                                 false,
                                 false);

        neutralEffect = new Effect("Neutral",
                                   CREATURES.MODIFIERS.NEUTRAL.NAME,
                                   CREATURES.MODIFIERS.NEUTRAL.TOOLTIP,
                                   0f,
                                   true,
                                   false,
                                   false);

        unhappyEffect = new Effect("Unhappy",
                                   CREATURES.MODIFIERS.GLUM.NAME,
                                   CREATURES.MODIFIERS.GLUM.TOOLTIP,
                                   0f,
                                   true,
                                   false,
                                   true);
    }

    private static bool IsHappy(Instance   smi) { return smi.happiness.GetTotalValue() >= smi.def.happyThreshold; }
    private static bool IsUnhappy(Instance smi) { return smi.happiness.GetTotalValue() <= smi.def.unhappyThreshold; }

    public class Def : BaseDef {
        public float happyThreshold   = 4f;
        public float unhappyThreshold = -1f;
    }

    public new class Instance : GameInstance {
        public AttributeInstance happiness;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            happiness = gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
        }
    }
}