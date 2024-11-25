using STRINGS;
using UnityEngine;

public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def> {
    private State loop;
    private State move;

    public override void InitializeStates(out BaseState default_state) {
        default_state = loop;
        var    state = root.Exit("StopNavigator", delegate(Instance smi) { smi.GetComponent<Navigator>().Stop(); });
        string name = CREATURES.STATUSITEMS.IDLE.NAME;
        string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
        var    icon = "";
        var    icon_type = StatusItem.IconType.Info;
        var    notification_type = NotificationType.Neutral;
        var    allow_multiples = false;
        var    main = Db.Get().StatusItemCategories.Main;
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
                               main)
             .ToggleTag(GameTags.Idle);

        loop.Enter(PlayIdle)
            .ToggleScheduleCallback("IdleMove", smi => Random.Range(3, 10), delegate(Instance smi) { smi.GoTo(move); });

        move.Enter(MoveToNewCell)
            .EventTransition(GameHashes.DestinationReached, loop)
            .EventTransition(GameHashes.NavigationFailed,   loop);
    }

    public void MoveToNewCell(Instance smi) {
        if (smi.HasTag(GameTags.StationaryIdling)) {
            smi.GoTo(smi.sm.loop);
            return;
        }

        var component     = smi.GetComponent<Navigator>();
        var moveCellQuery = new MoveCellQuery(component.CurrentNavType);
        moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
        moveCellQuery.submerged   = smi.gameObject.HasTag(GameTags.Creatures.Submerged);
        var num = Grid.PosToCell(component);
        if (component.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(num)) {
            var num2 = 0;
            var cell = num;
            for (var i = 0; i < 10; i++) {
                cell = Grid.CellBelow(cell);
                if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || !CellSelectionObject.IsExposedToSpace(cell))
                    break;

                num2++;
            }

            moveCellQuery.lowerCellBias = num2 == 10;
        }

        component.RunQuery(moveCellQuery);
        component.GoTo(moveCellQuery.GetResultCell());
    }

    public void PlayIdle(Instance smi) {
        var component                                        = smi.GetComponent<KAnimControllerBase>();
        var component2                                       = smi.GetComponent<Navigator>();
        var nav_type                                         = component2.CurrentNavType;
        if (smi.GetComponent<Facing>().GetFacing()) nav_type = NavGrid.MirrorNavType(nav_type);
        if (smi.def.customIdleAnim != null) {
            var invalid      = HashedString.Invalid;
            var hashedString = smi.def.customIdleAnim(smi, ref invalid);
            if (hashedString != HashedString.Invalid) {
                if (invalid != HashedString.Invalid) component.Play(invalid);
                component.Queue(hashedString, KAnim.PlayMode.Loop);
                return;
            }
        }

        var idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
        component.Play(idleAnim, KAnim.PlayMode.Loop);
    }

    public class Def : BaseDef {
        public delegate HashedString IdleAnimCallback(Instance smi, ref HashedString pre_anim);

        public IdleAnimCallback customIdleAnim;
    }

    public new class Instance : GameInstance {
        public Instance(Chore<Instance> chore, Def def) : base(chore, def) { }
    }

    public class MoveCellQuery : PathFinderQuery {
        private          int     maxIterations;
        private readonly NavType navType;
        private          int     targetCell = Grid.InvalidCell;

        public MoveCellQuery(NavType navType) {
            this.navType  = navType;
            maxIterations = Random.Range(5, 25);
        }

        public bool allowLiquid   { get; set; }
        public bool submerged     { get; set; }
        public bool lowerCellBias { get; set; }

        public override bool IsMatch(int cell, int parent_cell, int cost) {
            if (!Grid.IsValidCell(cell)) return false;

            GameObject gameObject;
            Grid.ObjectLayers[1].TryGetValue(cell, out gameObject);
            if (gameObject != null) {
                var component = gameObject.GetComponent<BuildingUnderConstruction>();
                if (component != null && (component.Def.IsFoundation || component.HasTag(GameTags.NoCreatureIdling)))
                    return false;
            }

            var flag  = submerged || Grid.IsNavigatableLiquid(cell);
            var flag2 = navType != NavType.Swim;
            var flag3 = navType == NavType.Swim || allowLiquid;
            if (flag && !flag3) return false;

            if (!flag && !flag2) return false;

            if (targetCell == Grid.InvalidCell || !lowerCellBias)
                targetCell = cell;
            else {
                var num                                  = Grid.CellRow(targetCell);
                if (Grid.CellRow(cell) < num) targetCell = cell;
            }

            var num2 = maxIterations - 1;
            maxIterations = num2;
            return num2 <= 0;
        }

        public override int GetResultCell() { return targetCell; }
    }
}