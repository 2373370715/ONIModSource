using KSerialization;
using UnityEngine;

public class OrbitalDeployCargoModule
    : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget,
        OrbitalDeployCargoModule.Def> {
    public Signal            emptyCargo;
    public GroundedStates    grounded;
    public BoolParameter     hasCargo;
    public NotGroundedStates not_grounded;
    public IntParameter      numVisualCapsules;

    public override void InitializeStates(out BaseState default_state) {
        default_state = grounded;
        root.Enter(delegate(StatesInstance                                    smi) { smi.CheckIfLoaded(); })
            .EventHandler(GameHashes.OnStorageChange, delegate(StatesInstance smi) { smi.CheckIfLoaded(); })
            .EventHandler(GameHashes.ClusterDestinationReached,
                          delegate(StatesInstance smi) {
                              if (smi.AutoDeploy && smi.IsValidDropLocation()) smi.DeployCargoPods();
                          });

        grounded.DefaultState(grounded.loaded).TagTransition(GameTags.RocketNotOnGround, not_grounded);
        grounded.loading.PlayAnim(smi => smi.GetLoadingAnimName())
                .ParamTransition(hasCargo, grounded.empty, IsFalse)
                .OnAnimQueueComplete(grounded.loaded);

        grounded.loaded.ParamTransition(hasCargo, grounded.empty, IsFalse)
                .EventTransition(GameHashes.OnStorageChange, grounded.loading, smi => smi.NeedsVisualUpdate());

        grounded.empty.Enter(delegate(StatesInstance smi) { numVisualCapsules.Set(0, smi); })
                .PlayAnim("deployed")
                .ParamTransition(hasCargo, grounded.loaded, IsTrue);

        not_grounded.DefaultState(not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, grounded, true);
        not_grounded.loaded.PlayAnim("loaded")
                    .ParamTransition(hasCargo, not_grounded.empty, IsFalse)
                    .OnSignal(emptyCargo, not_grounded.emptying);

        not_grounded.emptying.PlayAnim("deploying").GoTo(not_grounded.empty);
        not_grounded.empty.PlayAnim("deployed").ParamTransition(hasCargo, not_grounded.loaded, IsTrue);
    }

    public class Def : BaseDef {
        public float numCapsules;
    }

    public class GroundedStates : State {
        public State empty;
        public State loaded;
        public State loading;
    }

    public class NotGroundedStates : State {
        public State empty;
        public State emptying;
        public State loaded;
    }

    public class StatesInstance : GameInstance, IEmptyableCargo {
        private readonly Storage storage;

        public StatesInstance(IStateMachineTarget master, Def def) : base(master, def) {
            storage = GetComponent<Storage>();
            GetComponent<RocketModule>()
                .AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage,
                                    new LoadingCompleteCondition(storage));

            gameObject.Subscribe(-1683615038, SetupMeter);
        }

        [field: Serialize]
        public bool AutoDeploy { get; set; }

        public bool CanAutoDeploy   => true;
        public void EmptyCargo()    { DeployCargoPods(); }
        public bool CanEmptyCargo() { return sm.hasCargo.Get(smi) && IsValidDropLocation(); }
        public bool ChooseDuplicant => false;

        public MinionIdentity ChosenDuplicant {
            get => null;
            set { }
        }

        public bool ModuleDeployed => false;

        private void SetupMeter(object obj) {
            var componentInChildren = gameObject.GetComponentInChildren<KBatchedAnimTracker>();
            componentInChildren.forceAlwaysAlive  = true;
            componentInChildren.matchParentOffset = true;
        }

        protected override void OnCleanUp() {
            gameObject.Unsubscribe(-1683615038, SetupMeter);
            base.OnCleanUp();
        }

        public bool NeedsVisualUpdate() {
            var num  = sm.numVisualCapsules.Get(this);
            var num2 = Mathf.FloorToInt(storage.MassStored() / 200f);
            if (num < num2) {
                sm.numVisualCapsules.Delta(1, this);
                return true;
            }

            return false;
        }

        public string GetLoadingAnimName() {
            var num  = sm.numVisualCapsules.Get(this);
            var num2 = Mathf.RoundToInt(storage.capacityKg / 200f);
            if (num == num2) return "loading6_full";

            if (num == num2 - 1) return "loading5";

            if (num == num2 - 2) return "loading4";

            if (num == num2 - 3 || num > 2) return "loading3_repeat";

            if (num == 2) return "loading2";

            if (num == 1) return "loading1";

            return "deployed";
        }

        public void DeployCargoPods() {
            var component     = master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
            var orbitAsteroid = component.GetOrbitAsteroid();
            if (orbitAsteroid != null) {
                var component2 = orbitAsteroid.GetComponent<WorldContainer>();
                var id         = component2.id;
                var position = new Vector3(component2.minimumBounds.x + 1f,
                                           component2.maximumBounds.y,
                                           Grid.GetLayerZ(Grid.SceneLayer.Front));

                while (storage.MassStored() > 0f) {
                    var gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), position);
                    gameObject.GetComponent<Pickupable>().deleteOffGrid = false;
                    var num = 0f;
                    while (num < 200f && storage.MassStored() > 0f)
                        num += storage.Transfer(gameObject.GetComponent<Storage>(),
                                                GameTags.Stored,
                                                200f - num,
                                                false,
                                                true);

                    gameObject.SetActive(true);
                    gameObject.GetSMI<RailGunPayload.StatesInstance>()
                              .Travel(component.Location, component2.GetMyWorldLocation());
                }
            }

            CheckIfLoaded();
        }

        public bool CheckIfLoaded() {
            var flag = storage.MassStored() > 0f;
            if (flag != sm.hasCargo.Get(this)) sm.hasCargo.Set(flag, this);
            return flag;
        }

        public bool IsValidDropLocation() {
            return GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetOrbitAsteroid() !=
                   null;
        }
    }
}