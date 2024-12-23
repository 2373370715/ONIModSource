﻿[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance> {
    protected override void OnSpawn() { smi.StartSM(); }
    protected bool IsUncomfortable() { return smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore; }

    public class StatesInstance : GameStateMachine<States, StatesInstance, Workaholic, object>.GameInstance {
        public StatesInstance(Workaholic master) : base(master) { }
    }

    public class States : GameStateMachine<States, StatesInstance, Workaholic> {
        public State satisfied;
        public State suffering;

        public override void InitializeStates(out BaseState default_state) {
            default_state = satisfied;
            root.Update("WorkaholicCheck",
                        delegate(StatesInstance smi, float dt) {
                            if (smi.master.IsUncomfortable()) {
                                smi.GoTo(suffering);
                                return;
                            }

                            smi.GoTo(satisfied);
                        },
                        UpdateRate.SIM_1000ms);

            suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable);
            satisfied.DoNothing();
        }
    }
}