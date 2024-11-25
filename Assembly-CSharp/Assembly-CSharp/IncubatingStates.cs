using STRINGS;
using UnityEngine;

public class IncubatingStates
    : GameStateMachine<IncubatingStates, IncubatingStates.Instance, IStateMachineTarget, IncubatingStates.Def> {
    public IncubatorStates incubator;

    public override void InitializeStates(out BaseState default_state) {
        default_state = incubator;
        var    root              = this.root;
        string name              = CREATURES.STATUSITEMS.IN_INCUBATOR.NAME;
        string tooltip           = CREATURES.STATUSITEMS.IN_INCUBATOR.TOOLTIP;
        var    icon              = "";
        var    icon_type         = StatusItem.IconType.Info;
        var    notification_type = NotificationType.Neutral;
        var    allow_multiples   = false;
        var    main              = Db.Get().StatusItemCategories.Main;
        root.ToggleStatusItem(name,
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

        incubator.DefaultState(incubator.idle)
                 .ToggleTag(GameTags.Creatures.Deliverable)
                 .TagTransition(GameTags.Creatures.InIncubator, null, true);

        incubator.idle.Enter("VariantUpdate", VariantUpdate)
                 .PlayAnim("incubator_idle_loop")
                 .OnAnimQueueComplete(incubator.choose);

        incubator.choose.Transition(incubator.variant, DoVariant).Transition(incubator.idle, Not(DoVariant));
        incubator.variant.PlayAnim("incubator_variant").OnAnimQueueComplete(incubator.idle);
    }

    public static bool DoVariant(Instance smi) { return smi.variant_time == 0; }

    public static void VariantUpdate(Instance smi) {
        if (smi.variant_time <= 0) {
            smi.variant_time = Random.Range(3, 7);
            return;
        }

        smi.variant_time--;
    }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        public static readonly Chore.Precondition IsInIncubator = new Chore.Precondition {
            id = "IsInIncubator",
            fn = delegate(ref Chore.Precondition.Context context, object data) {
                     return context.consumerState.prefabid.HasTag(GameTags.Creatures.InIncubator);
                 }
        };

        public int variant_time = 3;
        public Instance(Chore<Instance> chore, Def def) : base(chore, def) { chore.AddPrecondition(IsInIncubator); }
    }

    public class IncubatorStates : State {
        public State choose;
        public State idle;
        public State variant;
    }
}