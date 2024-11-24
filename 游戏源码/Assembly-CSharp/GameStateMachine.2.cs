using System;

// Token: 0x020008B7 RID: 2231
public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance where MasterType : IStateMachineTarget
{
}
