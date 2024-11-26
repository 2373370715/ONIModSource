using STRINGS;
using UnityEngine;

public class
    AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def> {
    public ApproachSubState<AttackableBase> approach;
    public AttackingStates                  attack;
    public State                            behaviourcomplete;
    public CellOffset[]                     cellOffsets;
    public TargetParameter                  target;
    public State                            waitBeforeAttack;

    public override void InitializeStates(out BaseState default_state) {
        default_state = waitBeforeAttack;
        root.Enter("SetTarget",
                   delegate(Instance smi) {
                       target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi);
                       cellOffsets = smi.def.cellOffsets;
                   });

        waitBeforeAttack.ScheduleGoTo(smi => Random.Range(0f, 4f), approach);
        var    state             = approach.InitializeStates(masterTarget, target, attack, null, cellOffsets);
        string name              = CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME;
        string tooltip           = CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP;
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

        var    state2             = attack.DefaultState(attack.pre);
        string name2              = CREATURES.STATUSITEMS.ATTACK.NAME;
        string tooltip2           = CREATURES.STATUSITEMS.ATTACK.TOOLTIP;
        var    icon2              = "";
        var    icon_type2         = StatusItem.IconType.Info;
        var    notification_type2 = NotificationType.Neutral;
        var    allow_multiples2   = false;
        main = Db.Get().StatusItemCategories.Main;
        state2.ToggleStatusItem(name2,
                                tooltip2,
                                icon2,
                                icon_type2,
                                notification_type2,
                                allow_multiples2,
                                default(HashedString),
                                129022,
                                null,
                                null,
                                main);

        attack.pre.PlayAnim(smi => smi.def.preAnim)
              .Exit(delegate(Instance smi) { smi.GetComponent<Weapon>().AttackTarget(target.Get(smi)); })
              .OnAnimQueueComplete(attack.pst);

        attack.pst.PlayAnim(smi => smi.def.pstAnim).OnAnimQueueComplete(behaviourcomplete);
        behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack);
    }

    public class Def : BaseDef {
        public CellOffset[] cellOffsets = {
            new CellOffset(0,  0),
            new CellOffset(1,  0),
            new CellOffset(-1, 0),
            new CellOffset(1,  1),
            new CellOffset(-1, 1)
        };

        public string preAnim;
        public string pstAnim;

        public Def(string pre_anim = "eat_pre", string pst_anim = "eat_pst", CellOffset[] cell_offsets = null) {
            preAnim = pre_anim;
            pstAnim = pst_anim;
            if (cell_offsets != null) cellOffsets = cell_offsets;
        }
    }

    public class AttackingStates : State {
        public State pre;
        public State pst;
    }

    public new class Instance : GameInstance {
        public Instance(Chore<Instance> chore, Def def) : base(chore, def) {
            chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Attack);
        }
    }
}