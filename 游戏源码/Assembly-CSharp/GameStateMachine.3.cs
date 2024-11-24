using System;

// Token: 0x020008B8 RID: 2232
public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType> : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, IStateMachineTarget, object>.GameInstance
{
}
