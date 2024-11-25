using STRINGS;
using UnityEngine;

public class TreeClimbStates
    : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def> {
    public State                        behaviourcomplete;
    public ClimbState                   climbing;
    public ApproachSubState<Uprootable> moving;
    public TargetParameter              target;

    public override void InitializeStates(out BaseState default_state) {
        default_state = moving;
        var state = root.Enter(SetTarget)
                        .Enter(delegate(Instance smi) {
                                   if (!ReserveClimbable(smi)) smi.GoTo(behaviourcomplete);
                               })
                        .Exit(UnreserveClimbable);

        string name              = CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME;
        string tooltip           = CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP;
        var    icon              = "";
        var    icon_type         = StatusItem.IconType.Info;
        var    notification_type = NotificationType.Neutral;
        var    allow_multiples   = false;
        var    main              = Db.Get().StatusItemCategories.Main;
        state.ToggleStatusItem(name,
                               tooltip,
                               icon,
                               icon_type,
                               notification_type,
                               allow_multiples,
                               default(HashedString),
                               129022,
                               null,
                               null,
                               main);

        moving.MoveTo(GetClimbableCell, climbing, behaviourcomplete);
        climbing.DefaultState(climbing.pre);
        climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(climbing.loop);
        climbing.loop.QueueAnim("rummage_loop", true)
                .ScheduleGoTo(3.5f, climbing.pst)
                .Update(Rummage, UpdateRate.SIM_1000ms);

        climbing.pst.QueueAnim("rummage_pst").OnAnimQueueComplete(behaviourcomplete);
        behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree);
    }

    private static void SetTarget(Instance smi) {
        smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi);
    }

    private static bool ReserveClimbable(Instance smi) {
        var gameObject = smi.sm.target.Get(smi);
        if (gameObject != null && !gameObject.HasTag(GameTags.Creatures.ReservedByCreature)) {
            gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
            return true;
        }

        return false;
    }

    private static void UnreserveClimbable(Instance smi) {
        var gameObject = smi.sm.target.Get(smi);
        if (gameObject != null) gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
    }

    private static void Rummage(Instance smi, float dt) {
        var gameObject = smi.sm.target.Get(smi);
        if (gameObject != null) {
            var component = gameObject.GetComponent<ForestTreeSeedMonitor>();
            if (component != null) {
                component.ExtractExtraSeed();
                return;
            }

            var component2 = gameObject.GetComponent<Storage>();
            if (component2 && component2.items.Count > 0) {
                var index       = Random.Range(0, component2.items.Count - 1);
                var gameObject2 = component2.items[index];
                var pickupable  = gameObject2 ? gameObject2.GetComponent<Pickupable>() : null;
                if (pickupable && pickupable.UnreservedAmount > 0.01f) smi.Toss(pickupable);
            }
        }
    }

    private static int GetClimbableCell(Instance smi) { return Grid.PosToCell(smi.sm.target.Get(smi)); }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);
        private static readonly Vector2 VEL_MAX = new Vector2(1f,  4f);
        private readonly        Storage storage;

        public Instance(Chore<Instance> chore, Def def) : base(chore, def) {
            chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition,
                                  GameTags.Creatures.WantsToClimbTree);

            storage = GetComponent<Storage>();
        }

        public void Toss(Pickupable pu) {
            var pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedAmount));
            if (pickupable != null) {
                storage.Store(pickupable.gameObject, true);
                storage.Drop(pickupable.gameObject);
                Throw(pickupable.gameObject);
            }
        }

        private void Throw(GameObject ore_go) {
            var position = transform.GetPosition();
            position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
            var     num  = Grid.PosToCell(position);
            var     num2 = Grid.CellAbove(num);
            Vector2 zero;
            if ((Grid.IsValidCell(num) && Grid.Solid[num]) || (Grid.IsValidCell(num2) && Grid.Solid[num2]))
                zero = Vector2.zero;
            else {
                position.y += 0.5f;
                zero       =  new Vector2(Random.Range(VEL_MIN.x, VEL_MAX.x), Random.Range(VEL_MIN.y, VEL_MAX.y));
            }

            ore_go.transform.SetPosition(position);
            if (GameComps.Fallers.Has(ore_go)) GameComps.Fallers.Remove(ore_go);
            GameComps.Fallers.Add(ore_go, zero);
        }
    }

    public class ClimbState : State {
        public State loop;
        public State pre;
        public State pst;
    }
}