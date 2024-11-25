using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick {
    private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate
        = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data) {
                                                                   component.OnOperationalChanged(data);
                                                               });

    private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate
        = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data) {
                                                                   component.OnEndChore(data);
                                                               });

    private static readonly WorkItemCollection<BatchUpdateTask, BatchUpdateContext> batch_update_job
        = new WorkItemCollection<BatchUpdateTask, BatchUpdateContext>();

    private static readonly HashedString           HASH_ROTATION = "rotation";
    private                 ArmAnim                arm_anim;
    private                 KBatchedAnimController arm_anim_ctrl;
    private                 GameObject             arm_go;
    private                 float                  arm_rot = 45f;

    [MyCmpAdd]
    private ChoreConsumer choreConsumer;

    [MyCmpAdd]
    private ChoreDriver choreDriver;

    private          KAnimLink     link;
    private          LoopingSounds looping_sounds;
    private readonly float         max_carry_weight = 1000f;

    [MyCmpReq]
    private Operational operational;

    private readonly List<Pickupable> pickupables    = new List<Pickupable>();
    public           int              pickupRange    = 4;
    private readonly HashSet<int>     reachableCells = new HashSet<int>();

    [MyCmpGet]
    private Rotatable rotatable;

    private          EventReference rotateSound;
    private readonly string         rotateSoundName = "TransferArm_rotate";
    private          bool           rotateSoundPlaying;
    private          bool           rotation_complete;
    private          short          serial_no;

    [MyCmpAdd]
    private Storage storage;

    private readonly float turn_rate = 360f;

    [MyCmpAdd]
    private StandardWorker worker;

    public void RenderEveryTick(float dt) {
        if (worker.GetWorkable()) {
            var targetPoint = worker.GetWorkable().GetTargetPoint();
            targetPoint.z = 0f;
            var position = transform.GetPosition();
            position.z = 0f;
            var target_dir = Vector3.Normalize(targetPoint - position);
            RotateArm(target_dir, false, dt);
        }

        UpdateArmAnim();
    }

    public void Sim1000ms(float dt) { }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        choreConsumer.AddProvider(GlobalChoreProvider.Instance);
        choreConsumer.SetReach(pickupRange);
        var attributes = this.GetAttributes();
        if (attributes.Get(Db.Get().Attributes.CarryAmount) == null) attributes.Add(Db.Get().Attributes.CarryAmount);
        var modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id,
                                             max_carry_weight,
                                             gameObject.GetProperName());

        this.GetAttributes().Add(modifier);
        worker.usesMultiTool = false;
        storage.fxPrefix     = Storage.FXPrefix.PickedUp;
        simRenderLoadBalance = false;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        var component = GetComponent<KBatchedAnimController>();
        var name      = component.name + ".arm";
        arm_go = new GameObject(name);
        arm_go.SetActive(false);
        arm_go.transform.parent = component.transform;
        looping_sounds          = arm_go.AddComponent<LoopingSounds>();
        var sound = GlobalAssets.GetSound(rotateSoundName);
        rotateSound                                = RuntimeManager.PathToEventReference(sound);
        arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
        arm_anim_ctrl                              = arm_go.AddComponent<KBatchedAnimController>();
        arm_anim_ctrl.AnimFiles                    = new[] { component.AnimFiles[0] };
        arm_anim_ctrl.initialAnim                  = "arm";
        arm_anim_ctrl.isMovable                    = true;
        arm_anim_ctrl.sceneLayer                   = Grid.SceneLayer.TransferArm;
        component.SetSymbolVisiblity("arm_target", false);
        bool    flag;
        Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
        position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
        arm_go.transform.SetPosition(position);
        arm_go.SetActive(true);
        link = new KAnimLink(component, arm_anim_ctrl);
        var choreGroups = Db.Get().ChoreGroups;
        for (var i = 0; i < choreGroups.Count; i++) choreConsumer.SetPermittedByUser(choreGroups[i], true);
        Subscribe(-592767678, OnOperationalChangedDelegate);
        Subscribe(1745615042, OnEndChoreDelegate);
        RotateArm(rotatable.GetRotatedOffset(Vector3.up), true, 0f);
        DropLeftovers();
        component.enabled = false;
        component.enabled = true;
        MinionGroupProber.Get().SetValidSerialNos(this, serial_no, serial_no);
        smi.StartSM();
    }

    protected override void OnCleanUp() {
        MinionGroupProber.Get().ReleaseProber(this);
        base.OnCleanUp();
    }

    public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms,
                                   float                                           time_delta) {
        var batchUpdateContext = new BatchUpdateContext(solid_transfer_arms);
        if (batchUpdateContext.solid_transfer_arms.Count == 0) {
            batchUpdateContext.Finish();
            return;
        }

        batch_update_job.Reset(batchUpdateContext);
        var num  = Math.Max(1, batchUpdateContext.solid_transfer_arms.Count / CPUBudget.coreCount);
        var num2 = Math.Min(batchUpdateContext.solid_transfer_arms.Count, CPUBudget.coreCount);
        for (var num3 = 0; num3 != num2; num3++) {
            var num4 = num3 * num;
            var end  = num3 == num2 - 1 ? batchUpdateContext.solid_transfer_arms.Count : num4 + num;
            batch_update_job.Add(new BatchUpdateTask(num4, end));
        }

        GlobalJobManager.Run(batch_update_job);
        for (var num5 = 0; num5 != batch_update_job.Count; num5++) batch_update_job.GetWorkItem(num5).Finish();
        batchUpdateContext.Finish();
        batch_update_job.Reset(null);
    }

    private void Sim() {
        var context = default(Chore.Precondition.Context);
        if (choreConsumer.FindNextChore(ref context)) {
            if (context.chore is FetchChore) {
                choreDriver.SetChore(context);
                var chore = context.chore as FetchChore;
                storage.DropUnlessMatching(chore);
                arm_anim_ctrl.enabled = false;
                arm_anim_ctrl.enabled = true;
            } else {
                var condition = false;
                var str       = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
                var chore2    = context.chore;
                Debug.Assert(condition, str + (chore2 != null ? chore2.ToString() : null));
            }
        }

        operational.SetActive(choreDriver.HasChore());
    }

    private void UpdateArmAnim() {
        var fetchAreaChore = choreDriver.GetCurrentChore() as FetchAreaChore;
        if (worker.GetWorkable() && fetchAreaChore != null && rotation_complete) {
            StopRotateSound();
            SetArmAnim(fetchAreaChore.IsDelivering ? ArmAnim.Drop : ArmAnim.Pickup);
            return;
        }

        SetArmAnim(ArmAnim.Idle);
    }

    private bool AsyncUpdate(int cell, HashSet<int> workspace, GameObject game_object) {
        workspace.Clear();
        int num;
        int num2;
        Grid.CellToXY(cell, out num, out num2);
        for (var i = num2 - pickupRange; i < num2 + pickupRange + 1; i++) {
            for (var j = num - pickupRange; j < num + pickupRange + 1; j++) {
                var num3 = Grid.XYToCell(j, i);
                if (Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true)) workspace.Add(num3);
            }
        }

        var flag = !reachableCells.SetEquals(workspace);
        if (flag) {
            reachableCells.Clear();
            reachableCells.UnionWith(workspace);
        }

        pickupables.Clear();
        foreach (var obj in GameScenePartitioner.Instance
                                                .AsyncSafeEnumerate(num             - pickupRange,
                                                                    num2            - pickupRange,
                                                                    2 * pickupRange + 1,
                                                                    2 * pickupRange + 1,
                                                                    GameScenePartitioner.Instance.pickupablesLayer)
                                                .Concat(GameScenePartitioner.Instance.AsyncSafeEnumerate(num -
                                                             pickupRange,
                                                         num2            - pickupRange,
                                                         2 * pickupRange + 1,
                                                         2 * pickupRange + 1,
                                                         GameScenePartitioner.Instance.storedPickupablesLayer))) {
            var pickupable = obj as Pickupable;
            if (Grid.GetCellRange(cell, pickupable.cachedCell) <= pickupRange                  &&
                IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) &&
                pickupable.CouldBePickedUpByTransferArm(game_object))
                pickupables.Add(pickupable);
        }

        return flag;
    }

    private void IncrementSerialNo() {
        serial_no += 1;
        MinionGroupProber.Get().SetValidSerialNos(this, serial_no, serial_no);
        MinionGroupProber.Get().Occupy(this, serial_no, reachableCells);
    }

    public bool IsCellReachable(int cell) { return reachableCells.Contains(cell); }

    private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell) {
        return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && IsCellReachable(storage_cell);
    }

    public Pickupable FindFetchTarget(Storage destination, FetchChore chore) {
        return FetchManager.FindFetchTarget(pickupables, destination, chore);
    }

    private void OnEndChore(object data) { DropLeftovers(); }

    private void DropLeftovers() {
        if (!storage.IsEmpty() && !choreDriver.HasChore()) storage.DropAll();
    }

    private void SetArmAnim(ArmAnim new_anim) {
        if (new_anim == arm_anim) return;

        arm_anim = new_anim;
        switch (arm_anim) {
            case ArmAnim.Idle:
                arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop);
                return;
            case ArmAnim.Pickup:
                arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop);
                return;
            case ArmAnim.Drop:
                arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop);
                return;
            default:
                return;
        }
    }

    private void OnOperationalChanged(object data) {
        if (!(bool)data) {
            if (choreDriver.HasChore()) choreDriver.StopChore();
            UpdateArmAnim();
        }
    }

    private void SetArmRotation(float rot) {
        arm_rot                   = rot;
        arm_go.transform.rotation = Quaternion.Euler(0f, 0f, arm_rot);
    }

    private void RotateArm(Vector3 target_dir, bool warp, float dt) {
        var num              = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - arm_rot;
        if (num < -180f) num += 360f;
        if (num > 180f) num  -= 360f;
        if (!warp) num       =  Mathf.Clamp(num, -turn_rate * dt, turn_rate * dt);
        arm_rot += num;
        SetArmRotation(arm_rot);
        rotation_complete = Mathf.Approximately(num, 0f);
        if (!warp && !rotation_complete) {
            if (!rotateSoundPlaying) StartRotateSound();
            SetRotateSoundParameter(arm_rot);
            return;
        }

        StopRotateSound();
    }

    private void StartRotateSound() {
        if (!rotateSoundPlaying) {
            looping_sounds.StartSound(rotateSound);
            rotateSoundPlaying = true;
        }
    }

    private void SetRotateSoundParameter(float arm_rot) {
        if (rotateSoundPlaying) looping_sounds.SetParameter(rotateSound, HASH_ROTATION, arm_rot);
    }

    private void StopRotateSound() {
        if (rotateSoundPlaying) {
            looping_sounds.StopSound(rotateSound);
            rotateSoundPlaying = false;
        }
    }

    [Conditional("ENABLE_FETCH_PROFILING")]
    private static void BeginDetailedSample(string region_name) { }

    [Conditional("ENABLE_FETCH_PROFILING")]
    private static void BeginDetailedSample(string region_name, int count) { }

    [Conditional("ENABLE_FETCH_PROFILING")]
    private static void EndDetailedSample(string region_name) { }

    [Conditional("ENABLE_FETCH_PROFILING")]
    private static void EndDetailedSample(string region_name, int count) { }

    private enum ArmAnim {
        Idle,
        Pickup,
        Drop
    }

    public class SMInstance : GameStateMachine<States, SMInstance, SolidTransferArm, object>.GameInstance {
        public SMInstance(SolidTransferArm master) : base(master) { }
    }

    public class States : GameStateMachine<States, SMInstance, SolidTransferArm> {
        public State         off;
        public ReadyStates   on;
        public BoolParameter transferring;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            root.DoNothing();
            off.PlayAnim("off")
               .EventTransition(GameHashes.OperationalChanged, on, smi => smi.GetComponent<Operational>().IsOperational)
               .Enter(delegate(SMInstance smi) { smi.master.StopRotateSound(); });

            on.DefaultState(on.idle)
              .EventTransition(GameHashes.OperationalChanged,
                               off,
                               smi => !smi.GetComponent<Operational>().IsOperational);

            on.idle.PlayAnim("on")
              .EventTransition(GameHashes.ActiveChanged, on.working, smi => smi.GetComponent<Operational>().IsActive);

            on.working.PlayAnim("working")
              .EventTransition(GameHashes.ActiveChanged, on.idle, smi => !smi.GetComponent<Operational>().IsActive);
        }

        public class ReadyStates : State {
            public State idle;
            public State working;
        }
    }

    private class BatchUpdateContext {
        public readonly ListPool<int, BatchUpdateContext>.PooledList              cells;
        public readonly ListPool<GameObject, BatchUpdateContext>.PooledList       game_objects;
        public readonly ListPool<bool, BatchUpdateContext>.PooledList             refreshed_reachable_cells;
        public readonly ListPool<SolidTransferArm, BatchUpdateContext>.PooledList solid_transfer_arms;

        public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms) {
            this.solid_transfer_arms           = ListPool<SolidTransferArm, BatchUpdateContext>.Allocate();
            this.solid_transfer_arms.Capacity  = solid_transfer_arms.Count;
            refreshed_reachable_cells          = ListPool<bool, BatchUpdateContext>.Allocate();
            refreshed_reachable_cells.Capacity = solid_transfer_arms.Count;
            cells                              = ListPool<int, BatchUpdateContext>.Allocate();
            cells.Capacity                     = solid_transfer_arms.Count;
            game_objects                       = ListPool<GameObject, BatchUpdateContext>.Allocate();
            game_objects.Capacity              = solid_transfer_arms.Count;
            for (var num = 0; num != solid_transfer_arms.Count; num++) {
                var entry = solid_transfer_arms[num];
                entry.lastUpdateTime     = 0f;
                solid_transfer_arms[num] = entry;
                var solidTransferArm = (SolidTransferArm)entry.data;
                if (solidTransferArm.operational.IsOperational) {
                    this.solid_transfer_arms.Add(solidTransferArm);
                    refreshed_reachable_cells.Add(false);
                    cells.Add(Grid.PosToCell(solidTransferArm));
                    game_objects.Add(solidTransferArm.gameObject);
                }
            }
        }

        public void Finish() {
            for (var num = 0; num != solid_transfer_arms.Count; num++) {
                if (refreshed_reachable_cells[num]) solid_transfer_arms[num].IncrementSerialNo();
                solid_transfer_arms[num].Sim();
            }

            refreshed_reachable_cells.Recycle();
            cells.Recycle();
            game_objects.Recycle();
            solid_transfer_arms.Recycle();
        }
    }

    private struct BatchUpdateTask : IWorkItem<BatchUpdateContext> {
        public BatchUpdateTask(int start, int end) {
            this.start                = start;
            this.end                  = end;
            reachable_cells_workspace = HashSetPool<int, SolidTransferArm>.Allocate();
        }

        public void Run(BatchUpdateContext context) {
            for (var num = start; num != end; num++)
                context.refreshed_reachable_cells[num] = context.solid_transfer_arms[num]
                                                                .AsyncUpdate(context.cells[num],
                                                                             reachable_cells_workspace,
                                                                             context.game_objects[num]);
        }

        public           void Finish() { reachable_cells_workspace.Recycle(); }
        private readonly int start;
        private readonly int end;
        private readonly HashSetPool<int, SolidTransferArm>.PooledHashSet reachable_cells_workspace;
    }

    public struct CachedPickupable {
        public Pickupable pickupable;
        public int        storage_cell;
    }
}