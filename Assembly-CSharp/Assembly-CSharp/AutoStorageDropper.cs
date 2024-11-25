using UnityEngine;

public class AutoStorageDropper
    : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def> {
    private State         blocked;
    public  Signal        checkCanDrop;
    private State         dropping;
    private State         idle;
    private BoolParameter isBlocked;
    private State         pre_drop;

    public override void InitializeStates(out BaseState default_state) {
        default_state = idle;
        root.Update(delegate(Instance smi, float dt) { smi.UpdateBlockedStatus(); }, UpdateRate.SIM_200ms, true);
        idle.EventTransition(GameHashes.OnStorageChange, pre_drop)
            .OnSignal(checkCanDrop, pre_drop, smi => !smi.GetComponent<Storage>().IsEmpty())
            .ParamTransition(isBlocked, blocked, IsTrue);

        pre_drop.ScheduleGoTo(smi => smi.def.delay, dropping);
        dropping.Enter(delegate(Instance smi) { smi.Drop(); }).GoTo(idle);
        blocked.ParamTransition(isBlocked, pre_drop, IsFalse)
               .ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
    }

    public class DropperFxConfig {
        public string          animFile;
        public string          animName;
        public bool            flipX;
        public bool            flipY;
        public Grid.SceneLayer layer          = Grid.SceneLayer.FXFront;
        public bool            useElementTint = true;
    }

    public class Def : BaseDef {
        public bool            asOre;
        public bool            blockedBySubstantialLiquid;
        public float           cooldown = 2f;
        public float           delay;
        public DropperFxConfig downFx;
        public CellOffset      dropOffset;
        public SimHashes[]     elementFilter;
        public Vector3         fxOffset = Vector3.zero;
        public bool            invertElementFilterInitialValue;
        public DropperFxConfig leftFx;
        public DropperFxConfig neutralFx;
        public DropperFxConfig rightFx;
        public DropperFxConfig upFx;
    }

    public new class Instance : GameInstance {
        [MyCmpGet]
        private Rotatable m_rotatable;

        [MyCmpGet]
        private Storage m_storage;

        private float m_timeSinceLastDrop;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) {
            isInvertElementFilter = def.invertElementFilterInitialValue;
        }

        public bool isInvertElementFilter { get; private set; }

        public void SetInvertElementFilter(bool value) {
            smi.isInvertElementFilter = value;
            smi.sm.checkCanDrop.Trigger(smi);
        }

        public void UpdateBlockedStatus() {
            var cell  = Grid.PosToCell(smi.GetDropPosition());
            var value = Grid.IsSolidCell(cell) || (def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell));
            sm.isBlocked.Set(value, smi);
        }

        private bool IsFilteredElement(SimHashes element) {
            for (var num = 0; num != def.elementFilter.Length; num++)
                if (def.elementFilter[num] == element)
                    return true;

            return false;
        }

        private bool AllowedToDrop(SimHashes element) {
            return def.elementFilter        == null                       ||
                   def.elementFilter.Length == 0                          ||
                   (!isInvertElementFilter && IsFilteredElement(element)) ||
                   (isInvertElementFilter  && !IsFilteredElement(element));
        }

        public void Drop() {
            var     flag    = false;
            Element element = null;
            for (var i = m_storage.Count - 1; i >= 0; i--) {
                var gameObject = m_storage.items[i];
                var component  = gameObject.GetComponent<PrimaryElement>();
                if (AllowedToDrop(component.ElementID)) {
                    if (def.asOre) {
                        m_storage.Drop(gameObject);
                        gameObject.transform.SetPosition(GetDropPosition());
                        element = component.Element;
                        flag    = true;
                    } else {
                        var component2 = gameObject.GetComponent<Dumpable>();
                        if (!component2.IsNullOrDestroyed()) {
                            component2.Dump(GetDropPosition());
                            element = component.Element;
                            flag    = true;
                        }
                    }
                }
            }

            var dropperAnim = GetDropperAnim();
            if (flag && dropperAnim != null && GameClock.Instance.GetTime() > m_timeSinceLastDrop + def.cooldown) {
                m_timeSinceLastDrop = GameClock.Instance.GetTime();
                var vector = Grid.CellToPosCCC(Grid.PosToCell(GetDropPosition()), dropperAnim.layer);
                vector += m_rotatable != null ? m_rotatable.GetRotatedOffset(def.fxOffset) : def.fxOffset;
                var kbatchedAnimController
                    = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer);

                kbatchedAnimController.destroyOnAnimComplete = false;
                kbatchedAnimController.FlipX                 = dropperAnim.flipX;
                kbatchedAnimController.FlipY                 = dropperAnim.flipY;
                if (dropperAnim.useElementTint) kbatchedAnimController.TintColour = element.substance.colour;
                kbatchedAnimController.Play(dropperAnim.animName);
            }
        }

        public DropperFxConfig GetDropperAnim() {
            var cellOffset = m_rotatable != null ? m_rotatable.GetRotatedCellOffset(def.dropOffset) : def.dropOffset;
            if (cellOffset.x < 0) return def.leftFx;

            if (cellOffset.x > 0) return def.rightFx;

            if (cellOffset.y < 0) return def.downFx;

            if (cellOffset.y > 0) return def.upFx;

            return def.neutralFx;
        }

        public Vector3 GetDropPosition() {
            if (!(m_rotatable != null)) return transform.GetPosition() + def.dropOffset.ToVector3();

            return transform.GetPosition() + m_rotatable.GetRotatedCellOffset(def.dropOffset).ToVector3();
        }
    }
}