using System;

namespace Store {
    public sealed class StoreStateMachine : GameStateMachine<StoreStateMachine, StoreInstance, StoreTem> {
        public override void InitializeStates(out BaseState defaultState) {
            // 设置默认状态为Off
            defaultState = Off;

            // Off状态转换
            Off.EventTransition(GameHashes.OperationalChanged,
                                On,
                                smi => smi.GetComponent<Operational>().IsOperational);

            // On状态转换
            On.EventTransition(GameHashes.OperationalChanged,
                               Off,
                               smi => !smi.GetComponent<Operational>().IsOperational);
        }

        public State Off;
        public State On;
    }
}