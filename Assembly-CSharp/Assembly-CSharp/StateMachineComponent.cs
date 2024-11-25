using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class StateMachineComponent : KMonoBehaviour, ISaveLoadable, IStateMachineTarget {
    [MyCmpAdd]
    protected StateMachineController stateMachineController;

    public abstract StateMachine.Instance GetSMI();
}