using System.Collections;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/Workable/Diggable")]
public class Diggable : Workable {
    private static readonly List<Tuple<string, Tag>> lasersForHardness = new List<Tuple<string, Tag>> {
        new Tuple<string, Tag>("dig", "fx_dig_splash"), new Tuple<string, Tag>("specialistdig", "fx_dig_splash")
    };

    private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate
        = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data) {
                                                           component.OnReachableChanged(data);
                                                       });

    private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate
        = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data) {
                                                           component.OnRefreshUserMenu(data);
                                                       });

    private int          cached_cell = -1;
    private MeshRenderer childRenderer;
    public  Chore        chore;

    [SerializeField]
    public HashedString choreTypeIdHash;

    private int  handle;
    private bool isDigComplete;

    [SerializeField]
    public MeshRenderer materialDisplay;

    [SerializeField]
    public Material[] materials;

    private Element                  originalDigElement;
    private HandleVector<int>.Handle partitionerEntry;

    [MyCmpAdd]
    private Prioritizable prioritizable;

    private HandleVector<int>.Handle unstableEntry;
    private Diggable() { SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners); }
    public bool Reachable { get; private set; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        workerStatusItem            = Db.Get().DuplicantStatusItems.Digging;
        readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
        faceTargetWhenWorking       = true;
        Subscribe(-1432940121, OnReachableChangedDelegate);
        attributeConverter            = Db.Get().AttributeConverters.DiggingSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Mining.Id;
        skillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        multitoolContext              = "dig";
        multitoolHitEffectTag         = "fx_dig_splash";
        workingPstComplete            = null;
        workingPstFailed              = null;
        Prioritizable.AddRef(gameObject);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        cached_cell        = Grid.PosToCell(this);
        originalDigElement = Grid.Element[cached_cell];
        if (originalDigElement.hardness == 255) OnCancel();
        GetComponent<KSelectable>()
            .SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig);

        UpdateColor(Reachable);
        Grid.Objects[cached_cell, 7] = gameObject;
        var chore_type                          = Db.Get().ChoreTypes.Dig;
        if (choreTypeIdHash.IsValid) chore_type = Db.Get().ChoreTypes.GetByHash(choreTypeIdHash);
        chore = new WorkChore<Diggable>(chore_type,
                                        this,
                                        null,
                                        true,
                                        null,
                                        null,
                                        null,
                                        true,
                                        null,
                                        false,
                                        true,
                                        null,
                                        true);

        SetWorkTime(float.PositiveInfinity);
        partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn",
                                                             gameObject,
                                                             Grid.PosToCell(this),
                                                             GameScenePartitioner.Instance.solidChangedLayer,
                                                             OnSolidChanged);

        OnSolidChanged(null);
        new ReachabilityMonitor.Instance(this).StartSM();
        Subscribe(493375141, OnRefreshUserMenuDelegate);
        handle = Game.Instance.Subscribe(-1523247426, UpdateStatusItem);
        Components.Diggables.Add(this);
    }

    public override int GetCell() { return cached_cell; }

    public override AnimInfo GetAnim(Worker worker) {
        var result                                                                   = default(AnimInfo);
        if (overrideAnims != null && overrideAnims.Length != 0) result.overrideAnims = overrideAnims;
        if (multitoolContext.IsValid && multitoolHitEffectTag.IsValid)
            result.smi = new MultitoolController.Instance(this,
                                                          worker,
                                                          multitoolContext,
                                                          Assets.GetPrefab(multitoolHitEffectTag));

        return result;
    }

    private static bool IsCellBuildable(int cell) {
        var result                                                                         = false;
        var gameObject                                                                     = Grid.Objects[cell, 1];
        if (gameObject != null && gameObject.GetComponent<Constructable>() != null) result = true;
        return result;
    }

    private IEnumerator PeriodicUnstableFallingRecheck() {
        yield return SequenceUtil.WaitForSeconds(2f);

        OnSolidChanged(null);
    }

    private void OnSolidChanged(object data) {
        if (this == null || gameObject == null) return;

        GameScenePartitioner.Instance.Free(ref unstableEntry);
        var num = -1;
        UpdateColor(Reachable);
        if (Grid.Element[cached_cell].hardness == 255) {
            UpdateColor(false);
            requiredSkillPerk = null;
            chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigUnobtanium);
        } else if (Grid.Element[cached_cell].hardness >= 251) {
            var flag = false;
            using (var enumerator = chore.GetPreconditions().GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.id == ChorePreconditions.instance.HasSkillPerk.id) {
                        flag = true;
                        break;
                    }
            }

            if (!flag)
                chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk,
                                      Db.Get().SkillPerks.CanDigRadioactiveMaterials);

            requiredSkillPerk              = Db.Get().SkillPerks.CanDigRadioactiveMaterials.Id;
            materialDisplay.sharedMaterial = materials[3];
        } else if (Grid.Element[cached_cell].hardness >= 200) {
            var flag2 = false;
            using (var enumerator = chore.GetPreconditions().GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.id == ChorePreconditions.instance.HasSkillPerk.id) {
                        flag2 = true;
                        break;
                    }
            }

            if (!flag2)
                chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk,
                                      Db.Get().SkillPerks.CanDigSuperDuperHard);

            requiredSkillPerk              = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
            materialDisplay.sharedMaterial = materials[3];
        } else if (Grid.Element[cached_cell].hardness >= 150) {
            var flag3 = false;
            using (var enumerator = chore.GetPreconditions().GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.id == ChorePreconditions.instance.HasSkillPerk.id) {
                        flag3 = true;
                        break;
                    }
            }

            if (!flag3)
                chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk,
                                      Db.Get().SkillPerks.CanDigNearlyImpenetrable);

            requiredSkillPerk              = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
            materialDisplay.sharedMaterial = materials[2];
        } else if (Grid.Element[cached_cell].hardness >= 50) {
            var flag4 = false;
            using (var enumerator = chore.GetPreconditions().GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.id == ChorePreconditions.instance.HasSkillPerk.id) {
                        flag4 = true;
                        break;
                    }
            }

            if (!flag4)
                chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigVeryFirm);

            requiredSkillPerk              = Db.Get().SkillPerks.CanDigVeryFirm.Id;
            materialDisplay.sharedMaterial = materials[1];
        } else {
            requiredSkillPerk = null;
            chore.GetPreconditions()
                 .Remove(chore.GetPreconditions().Find(o => o.id == ChorePreconditions.instance.HasSkillPerk.id));
        }

        UpdateStatusItem();
        var flag5 = false;
        if (!Grid.Solid[cached_cell]) {
            num = GetUnstableCellAbove(cached_cell);
            if (num == -1)
                flag5 = true;
            else
                StartCoroutine("PeriodicUnstableFallingRecheck");
        } else if (Grid.Foundation[cached_cell]) flag5 = true;

        if (!flag5) {
            if (num != -1) {
                var extents = default(Extents);
                Grid.CellToXY(cached_cell, out extents.x, out extents.y);
                extents.width  = 1;
                extents.height = (num - cached_cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
                unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged",
                                                                  gameObject,
                                                                  extents,
                                                                  GameScenePartitioner.Instance.solidChangedLayer,
                                                                  OnSolidChanged);
            }

            return;
        }

        isDigComplete = true;
        if (chore == null || !chore.InProgress()) {
            Util.KDestroyGameObject(gameObject);
            return;
        }

        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public          Element GetTargetElement()     { return Grid.Element[cached_cell]; }
    public override string  GetConversationTopic() { return originalDigElement.tag.Name; }

    protected override bool OnWorkTick(Worker worker, float dt) {
        DoDigTick(cached_cell, dt);
        return isDigComplete;
    }

    protected override void OnStopWork(Worker worker) {
        if (isDigComplete) Util.KDestroyGameObject(gameObject);
    }

    public override bool InstantlyFinish(Worker worker) {
        if (Grid.Element[cached_cell].hardness == 255) return false;

        var approximateDigTime = GetApproximateDigTime(cached_cell);
        worker.Work(approximateDigTime);
        return true;
    }

    public static void DoDigTick(int cell, float dt) { DoDigTick(cell, dt, WorldDamage.DamageType.Absolute); }

    public static void DoDigTick(int cell, float dt, WorldDamage.DamageType damageType) {
        var approximateDigTime = GetApproximateDigTime(cell);
        var amount             = dt / approximateDigTime;
        WorldDamage.Instance.ApplyDamage(cell, amount, -1, damageType);
    }

    public static float GetApproximateDigTime(int cell) {
        UnityEngine.Debug.Log("到GetApproximateDigTime了");
        float num = Grid.Element[cell].hardness;
        UnityEngine.Debug.Log("num:" + num);
        if (num == 255f) return float.MaxValue;

        var element = ElementLoader.FindElementByHash(SimHashes.Ice);
        UnityEngine.Debug.Log("element.hardness:" + element.hardness);
        var num2 = num / element.hardness;
        var num3 = Mathf.Min(Grid.Mass[cell], 400f) / 400f;
        var num4 = 4f                               * num3;
        UnityEngine.Debug.Log("num2 = " + num2 + ", num3 = " + num3 + ", num4 = " + num4);
        return num4 + num2 * num4;
    }

    public static Diggable GetDiggable(int cell) {
        var gameObject = Grid.Objects[cell, 7];
        if (gameObject != null) return gameObject.GetComponent<Diggable>();

        return null;
    }

    public static bool IsDiggable(int cell) {
        if (Grid.Solid[cell]) return !Grid.Foundation[cell];

        return GetUnstableCellAbove(cell) != Grid.InvalidCell;
    }

    private static int GetUnstableCellAbove(int cell) {
        var cellXY = Grid.CellToXY(cell);
        var cellsContainingFallingAbove
            = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(cellXY);

        if (cellsContainingFallingAbove.Contains(cell)) return cell;

        var b   = Grid.WorldIdx[cell];
        var num = Grid.CellAbove(cell);
        while (Grid.IsValidCell(num) && Grid.WorldIdx[num] == b) {
            if (Grid.Foundation[num]) return Grid.InvalidCell;

            if (Grid.Solid[num]) {
                if (Grid.Element[num].IsUnstable) return num;

                return Grid.InvalidCell;
            }

            if (cellsContainingFallingAbove.Contains(num)) return num;

            num = Grid.CellAbove(num);
        }

        return Grid.InvalidCell;
    }

    public static bool RequiresTool(Element e) { return false; }
    public static bool Undiggable(Element   e) { return e.id == SimHashes.Unobtanium; }

    private void OnReachableChanged(object data) {
        if (childRenderer == null) childRenderer = GetComponentInChildren<MeshRenderer>();
        var material                             = childRenderer.material;
        Reachable = (bool)data;
        if (material.color == Game.Instance.uiColours.Dig.invalidLocation) return;

        UpdateColor(Reachable);
        var component = GetComponent<KSelectable>();
        if (Reachable) {
            component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable);
            return;
        }

        component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, this);
        GameScheduler.Instance.Schedule("Locomotion Tutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion);
                                        });
    }

    private void UpdateColor(bool reachable) {
        if (childRenderer != null) {
            var material = childRenderer.material;
            if (RequiresTool(Grid.Element[Grid.PosToCell(gameObject)]) ||
                Undiggable(Grid.Element[Grid.PosToCell(gameObject)])) {
                material.color = Game.Instance.uiColours.Dig.invalidLocation;
                return;
            }

            if (Grid.Element[Grid.PosToCell(gameObject)].hardness >= 50) {
                if (reachable)
                    material.color = Game.Instance.uiColours.Dig.validLocation;
                else
                    material.color = Game.Instance.uiColours.Dig.unreachable;

                multitoolContext      = lasersForHardness[1].first;
                multitoolHitEffectTag = lasersForHardness[1].second;
                return;
            }

            if (reachable)
                material.color = Game.Instance.uiColours.Dig.validLocation;
            else
                material.color = Game.Instance.uiColours.Dig.unreachable;

            multitoolContext      = lasersForHardness[0].first;
            multitoolHitEffectTag = lasersForHardness[0].second;
        }
    }

    public override float GetPercentComplete() { return Grid.Damage[Grid.PosToCell(this)]; }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        GameScenePartitioner.Instance.Free(ref unstableEntry);
        Game.Instance.Unsubscribe(handle);
        var cell = Grid.PosToCell(this);
        GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.digDestroyedLayer, null);
        Components.Diggables.Remove(this);
    }

    private void OnCancel() {
        if (DetailsScreen.Instance != null) DetailsScreen.Instance.Show(false);
        gameObject.Trigger(2127324410);
    }

    private void OnRefreshUserMenu(object data) {
        Game.Instance.userMenu.AddButton(gameObject,
                                         new KIconButtonMenu.ButtonInfo("icon_cancel",
                                                                        UI.USERMENUACTIONS.CANCELDIG.NAME,
                                                                        OnCancel,
                                                                        Action.NumActions,
                                                                        null,
                                                                        null,
                                                                        null,
                                                                        UI.USERMENUACTIONS.CANCELDIG.TOOLTIP));
    }
}