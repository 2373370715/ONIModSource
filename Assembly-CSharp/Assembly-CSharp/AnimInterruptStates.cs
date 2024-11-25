public class AnimInterruptStates
    : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget,
        AnimInterruptStates.Def> {
    public State behaviourcomplete;
    public State play_anim;

    public override void InitializeStates(out BaseState default_state) {
        default_state = play_anim;
        play_anim.Enter(PlayAnim).OnAnimQueueComplete(behaviourcomplete);
        behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.PlayInterruptAnim);
    }

    private void PlayAnim(Instance smi) {
        var kbatchedAnimController = smi.Get<KBatchedAnimController>();
        var anims                  = smi.GetSMI<AnimInterruptMonitor.Instance>().anims;
        kbatchedAnimController.Play(anims[0]);
        for (var i = 1; i < anims.Length; i++) kbatchedAnimController.Queue(anims[i]);
    }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        public Instance(Chore<Instance> chore, Def def) : base(chore, def) {
            chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition,
                                  GameTags.Creatures.Behaviours.PlayInterruptAnim);
        }
    }
}