using UnityEngine;

public class SuperProductiveFX : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance> {
    public Signal          destroyFX;
    public TargetParameter fx;
    public State           idle;
    public State           pre;
    public State           productive;
    public State           pst;
    public Signal          wasProductive;

    public override void InitializeStates(out BaseState default_state) {
        default_state = pre;
        Target(fx);
        root.OnSignal(wasProductive, productive, smi => smi.GetCurrentState() != smi.sm.pst).OnSignal(destroyFX, pst);
        pre.PlayAnim("productive_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(idle);
        idle.PlayAnim("productive_loop", KAnim.PlayMode.Loop);
        productive.QueueAnim("productive_achievement").OnAnimQueueComplete(idle);
        pst.PlayAnim("productive_pst")
           .EventHandler(GameHashes.AnimQueueComplete, delegate(Instance smi) { smi.DestroyFX(); });
    }

    public new class Instance : GameInstance {
        public Instance(IStateMachineTarget master, Vector3 offset) : base(master) {
            var kbatchedAnimController = FXHelpers.CreateEffect("productive_fx_kanim",
                                                                master.gameObject.transform.GetPosition() + offset,
                                                                master.gameObject.transform,
                                                                true,
                                                                Grid.SceneLayer.FXFront);

            sm.fx.Set(kbatchedAnimController.gameObject, smi);
        }

        public void DestroyFX() {
            Util.KDestroyGameObject(sm.fx.Get(smi));
            smi.StopSM("destroyed");
        }
    }
}