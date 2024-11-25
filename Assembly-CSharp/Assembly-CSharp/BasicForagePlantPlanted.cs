public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance> {
    [MyCmpReq]
    private KBatchedAnimController animController;

    [MyCmpReq]
    private Harvestable harvestable;

    [MyCmpReq]
    private SeedProducer seedProducer;

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
    }

    protected void DestroySelf(object callbackParam) {
        CreatureHelpers.DeselectCreature(gameObject);
        Util.KDestroyGameObject(gameObject);
    }

    public class
        StatesInstance : GameStateMachine<States, StatesInstance, BasicForagePlantPlanted, object>.GameInstance {
        public StatesInstance(BasicForagePlantPlanted smi) : base(smi) { }
    }

    public class States : GameStateMachine<States, StatesInstance, BasicForagePlantPlanted> {
        public AliveStates alive;
        public State       dead;
        public State       seed_grow;

        public override void InitializeStates(out BaseState default_state) {
            default_state = seed_grow;
            serializable  = SerializeType.Both_DEPRECATED;
            seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, alive.idle);
            alive.InitializeStates(masterTarget, dead);
            alive.idle.PlayAnim("idle")
                 .EventTransition(GameHashes.Harvest, alive.harvest)
                 .Enter(delegate(StatesInstance smi) { smi.master.harvestable.SetCanBeHarvested(true); });

            alive.harvest.Enter(delegate(StatesInstance smi) { smi.master.seedProducer.DropSeed(); }).GoTo(dead);
            dead.Enter(delegate(StatesInstance smi) {
                           GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId),
                                                 smi.master.transform.GetPosition(),
                                                 Grid.SceneLayer.FXFront)
                                   .SetActive(true);

                           smi.master.Trigger(1623392196);
                           smi.master.animController.StopAndClear();
                           Destroy(smi.master.animController);
                           smi.master.DestroySelf(null);
                       });
        }

        public class AliveStates : PlantAliveSubState {
            public State harvest;
            public State idle;
        }
    }
}