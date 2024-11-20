using System;

namespace Store {
    /// <summary>
    /// 状态机
    /// </summary>
    public sealed class StoreInstance
        : GameStateMachine<StoreStateMachine, StoreInstance, StoreTem, object>.GameInstance {
        public StoreInstance(StoreTem master) : base(master) { }
    }
}