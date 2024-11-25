using System.Linq;
using UnityEngine;

public class ClimbableTreeMonitor
    : GameStateMachine<ClimbableTreeMonitor, ClimbableTreeMonitor.Instance, IStateMachineTarget,
        ClimbableTreeMonitor.Def> {
    private const int MAX_NAV_COST = 2147483647;

    public override void InitializeStates(out BaseState default_state) {
        default_state = root;
        root.ToggleBehaviour(GameTags.Creatures.WantsToClimbTree,
                             smi => smi.UpdateHasClimbable(),
                             delegate(Instance smi) { smi.OnClimbComplete(); });
    }

    public class Def : BaseDef {
        public float searchMaxInterval = 120f;
        public float searchMinInterval = 60f;
    }

    public new class Instance : GameInstance {
        public GameObject climbTarget;
        public float      nextSearchTime;
        public Instance(IStateMachineTarget master, Def def) : base(master, def) { RefreshSearchTime(); }

        private void RefreshSearchTime() {
            nextSearchTime = Time.time + Mathf.Lerp(def.searchMinInterval, def.searchMaxInterval, Random.value);
        }

        public bool UpdateHasClimbable() {
            if (climbTarget == null) {
                if (Time.time < nextSearchTime) return false;

                FindClimbableTree();
                RefreshSearchTime();
            }

            return climbTarget != null;
        }

        private void FindClimbableTree() {
            climbTarget = null;
            var pooledList = ListPool<KMonoBehaviour, ClimbableTreeMonitor>.Allocate();
            var position   = master.transform.GetPosition();
            var extents    = new Extents(Grid.PosToCell(position), 10);
            var component  = GetComponent<Navigator>();
            var first = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x,
                                                                         extents.y,
                                                                         extents.width,
                                                                         extents.height,
                                                                         GameScenePartitioner.Instance.plants);

            var second = GameScenePartitioner.Instance.AsyncSafeEnumerate(extents.x,
                                                                          extents.y,
                                                                          extents.width,
                                                                          extents.height,
                                                                          GameScenePartitioner.Instance
                                                                              .completeBuildings);

            foreach (var obj in first.Concat(second)) {
                var kmonoBehaviour = obj as KMonoBehaviour;
                if (!kmonoBehaviour.HasTag(GameTags.Creatures.ReservedByCreature)) {
                    var cell = Grid.PosToCell(kmonoBehaviour);
                    if (component.CanReach(cell)) {
                        var component2 = kmonoBehaviour.GetComponent<ForestTreeSeedMonitor>();
                        var component3 = kmonoBehaviour.GetComponent<StorageLocker>();
                        if (component2 != null) {
                            if (!component2.ExtraSeedAvailable) continue;
                        } else {
                            if (!(component3 != null)) continue;

                            var component4 = component3.GetComponent<Storage>();
                            if (!component4.allowItemRemoval || component4.IsEmpty()) continue;
                        }

                        pooledList.Add(kmonoBehaviour);
                    }
                }
            }

            if (pooledList.Count > 0) {
                var index           = Random.Range(0, pooledList.Count);
                var kmonoBehaviour2 = pooledList[index];
                climbTarget = kmonoBehaviour2.gameObject;
            }

            pooledList.Recycle();
        }

        public void OnClimbComplete() { climbTarget = null; }
    }
}