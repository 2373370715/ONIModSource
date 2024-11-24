using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000F9E RID: 3998
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
	// Token: 0x060050D6 RID: 20694 RVA: 0x0026F8E8 File Offset: 0x0026DAE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreConsumer.AddProvider(GlobalChoreProvider.Instance);
		this.choreConsumer.SetReach(this.pickupRange);
		Klei.AI.Attributes attributes = this.GetAttributes();
		if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
		{
			attributes.Add(Db.Get().Attributes.CarryAmount);
		}
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, base.gameObject.GetProperName(), false, false, true);
		this.GetAttributes().Add(modifier);
		this.worker.usesMultiTool = false;
		this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
		this.simRenderLoadBalance = false;
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x0026F9AC File Offset: 0x0026DBAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".arm";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "arm";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("arm_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		for (int i = 0; i < choreGroups.Count; i++)
		{
			this.choreConsumer.SetPermittedByUser(choreGroups[i], true);
		}
		base.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
		base.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0f);
		this.DropLeftovers();
		component.enabled = false;
		component.enabled = true;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		base.smi.StartSM();
	}

	// Token: 0x060050D8 RID: 20696 RVA: 0x000D4B7E File Offset: 0x000D2D7E
	protected override void OnCleanUp()
	{
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x060050D9 RID: 20697 RVA: 0x0026FBBC File Offset: 0x0026DDBC
	public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms, float time_delta)
	{
		SolidTransferArm.BatchUpdateContext batchUpdateContext = new SolidTransferArm.BatchUpdateContext(solid_transfer_arms);
		if (batchUpdateContext.solid_transfer_arms.Count == 0)
		{
			batchUpdateContext.Finish();
			return;
		}
		SolidTransferArm.batch_update_job.Reset(batchUpdateContext);
		int num = Math.Max(1, batchUpdateContext.solid_transfer_arms.Count / CPUBudget.coreCount);
		int num2 = Math.Min(batchUpdateContext.solid_transfer_arms.Count, CPUBudget.coreCount);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? batchUpdateContext.solid_transfer_arms.Count : (num4 + num);
			SolidTransferArm.batch_update_job.Add(new SolidTransferArm.BatchUpdateTask(num4, end));
		}
		GlobalJobManager.Run(SolidTransferArm.batch_update_job);
		for (int num5 = 0; num5 != SolidTransferArm.batch_update_job.Count; num5++)
		{
			SolidTransferArm.batch_update_job.GetWorkItem(num5).Finish();
		}
		batchUpdateContext.Finish();
		SolidTransferArm.batch_update_job.Reset(null);
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x0026FCA4 File Offset: 0x0026DEA4
	private void Sim()
	{
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (this.choreConsumer.FindNextChore(ref context))
		{
			if (context.chore is FetchChore)
			{
				this.choreDriver.SetChore(context);
				FetchChore chore = context.chore as FetchChore;
				this.storage.DropUnlessMatching(chore);
				this.arm_anim_ctrl.enabled = false;
				this.arm_anim_ctrl.enabled = true;
			}
			else
			{
				bool condition = false;
				string str = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
				Chore chore2 = context.chore;
				global::Debug.Assert(condition, str + ((chore2 != null) ? chore2.ToString() : null));
			}
		}
		this.operational.SetActive(this.choreDriver.HasChore(), false);
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void Sim1000ms(float dt)
	{
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x0026FD4C File Offset: 0x0026DF4C
	private void UpdateArmAnim()
	{
		FetchAreaChore fetchAreaChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
		if (this.worker.GetWorkable() && fetchAreaChore != null && this.rotation_complete)
		{
			this.StopRotateSound();
			this.SetArmAnim(fetchAreaChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
			return;
		}
		this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
	}

	// Token: 0x060050DD RID: 20701 RVA: 0x0026FDA8 File Offset: 0x0026DFA8
	private bool AsyncUpdate(int cell, HashSet<int> workspace, GameObject game_object)
	{
		workspace.Clear();
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		for (int i = num2 - this.pickupRange; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num - this.pickupRange; j < num + this.pickupRange + 1; j++)
			{
				int num3 = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					workspace.Add(num3);
				}
			}
		}
		bool flag = !this.reachableCells.SetEquals(workspace);
		if (flag)
		{
			this.reachableCells.Clear();
			this.reachableCells.UnionWith(workspace);
		}
		this.pickupables.Clear();
		foreach (object obj in GameScenePartitioner.Instance.AsyncSafeEnumerate(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.pickupablesLayer).Concat(GameScenePartitioner.Instance.AsyncSafeEnumerate(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.storedPickupablesLayer)))
		{
			Pickupable pickupable = obj as Pickupable;
			if (Grid.GetCellRange(cell, pickupable.cachedCell) <= this.pickupRange && this.IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) && pickupable.CouldBePickedUpByTransferArm(game_object))
			{
				this.pickupables.Add(pickupable);
			}
		}
		return flag;
	}

	// Token: 0x060050DE RID: 20702 RVA: 0x000D4B92 File Offset: 0x000D2D92
	private void IncrementSerialNo()
	{
		this.serial_no += 1;
		MinionGroupProber.Get().SetValidSerialNos(this, this.serial_no, this.serial_no);
		MinionGroupProber.Get().Occupy(this, this.serial_no, this.reachableCells);
	}

	// Token: 0x060050DF RID: 20703 RVA: 0x000D4BD1 File Offset: 0x000D2DD1
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x060050E0 RID: 20704 RVA: 0x000D4BDF File Offset: 0x000D2DDF
	private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
	{
		return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
	}

	// Token: 0x060050E1 RID: 20705 RVA: 0x000D4BF7 File Offset: 0x000D2DF7
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
	}

	// Token: 0x060050E2 RID: 20706 RVA: 0x0026FF58 File Offset: 0x0026E158
	public void RenderEveryTick(float dt)
	{
		if (this.worker.GetWorkable())
		{
			Vector3 targetPoint = this.worker.GetWorkable().GetTargetPoint();
			targetPoint.z = 0f;
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(targetPoint - position);
			this.RotateArm(target_dir, false, dt);
		}
		this.UpdateArmAnim();
	}

	// Token: 0x060050E3 RID: 20707 RVA: 0x000D4C06 File Offset: 0x000D2E06
	private void OnEndChore(object data)
	{
		this.DropLeftovers();
	}

	// Token: 0x060050E4 RID: 20708 RVA: 0x0026FFC8 File Offset: 0x0026E1C8
	private void DropLeftovers()
	{
		if (!this.storage.IsEmpty() && !this.choreDriver.HasChore())
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x060050E5 RID: 20709 RVA: 0x00270008 File Offset: 0x0026E208
	private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
	{
		if (new_anim == this.arm_anim)
		{
			return;
		}
		this.arm_anim = new_anim;
		switch (this.arm_anim)
		{
		case SolidTransferArm.ArmAnim.Idle:
			this.arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Pickup:
			this.arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Drop:
			this.arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x060050E6 RID: 20710 RVA: 0x000D4C0E File Offset: 0x000D2E0E
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			if (this.choreDriver.HasChore())
			{
				this.choreDriver.StopChore();
			}
			this.UpdateArmAnim();
		}
	}

	// Token: 0x060050E7 RID: 20711 RVA: 0x000D4C36 File Offset: 0x000D2E36
	private void SetArmRotation(float rot)
	{
		this.arm_rot = rot;
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
	}

	// Token: 0x060050E8 RID: 20712 RVA: 0x002700A4 File Offset: 0x0026E2A4
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		if (num < -180f)
		{
			num += 360f;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		if (!warp)
		{
			num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num;
		this.SetArmRotation(this.arm_rot);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		if (!warp && !this.rotation_complete)
		{
			if (!this.rotateSoundPlaying)
			{
				this.StartRotateSound();
			}
			this.SetRotateSoundParameter(this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x060050E9 RID: 20713 RVA: 0x000D4C64 File Offset: 0x000D2E64
	private void StartRotateSound()
	{
		if (!this.rotateSoundPlaying)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotateSoundPlaying = true;
		}
	}

	// Token: 0x060050EA RID: 20714 RVA: 0x000D4C87 File Offset: 0x000D2E87
	private void SetRotateSoundParameter(float arm_rot)
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
		}
	}

	// Token: 0x060050EB RID: 20715 RVA: 0x000D4CA8 File Offset: 0x000D2EA8
	private void StopRotateSound()
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotateSoundPlaying = false;
		}
	}

	// Token: 0x060050EC RID: 20716 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x060050ED RID: 20717 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void BeginDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x060050EE RID: 20718 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x060050EF RID: 20719 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("ENABLE_FETCH_PROFILING")]
	private static void EndDetailedSample(string region_name, int count)
	{
	}

	// Token: 0x0400385F RID: 14431
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003860 RID: 14432
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003861 RID: 14433
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003862 RID: 14434
	[MyCmpAdd]
	private StandardWorker worker;

	// Token: 0x04003863 RID: 14435
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;

	// Token: 0x04003864 RID: 14436
	[MyCmpAdd]
	private ChoreDriver choreDriver;

	// Token: 0x04003865 RID: 14437
	public int pickupRange = 4;

	// Token: 0x04003866 RID: 14438
	private float max_carry_weight = 1000f;

	// Token: 0x04003867 RID: 14439
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x04003868 RID: 14440
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04003869 RID: 14441
	private GameObject arm_go;

	// Token: 0x0400386A RID: 14442
	private LoopingSounds looping_sounds;

	// Token: 0x0400386B RID: 14443
	private bool rotateSoundPlaying;

	// Token: 0x0400386C RID: 14444
	private string rotateSoundName = "TransferArm_rotate";

	// Token: 0x0400386D RID: 14445
	private EventReference rotateSound;

	// Token: 0x0400386E RID: 14446
	private KAnimLink link;

	// Token: 0x0400386F RID: 14447
	private float arm_rot = 45f;

	// Token: 0x04003870 RID: 14448
	private float turn_rate = 360f;

	// Token: 0x04003871 RID: 14449
	private bool rotation_complete;

	// Token: 0x04003872 RID: 14450
	private SolidTransferArm.ArmAnim arm_anim;

	// Token: 0x04003873 RID: 14451
	private HashSet<int> reachableCells = new HashSet<int>();

	// Token: 0x04003874 RID: 14452
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003875 RID: 14453
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnEndChore(data);
	});

	// Token: 0x04003876 RID: 14454
	private static WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext> batch_update_job = new WorkItemCollection<SolidTransferArm.BatchUpdateTask, SolidTransferArm.BatchUpdateContext>();

	// Token: 0x04003877 RID: 14455
	private short serial_no;

	// Token: 0x04003878 RID: 14456
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x02000F9F RID: 3999
	private enum ArmAnim
	{
		// Token: 0x0400387A RID: 14458
		Idle,
		// Token: 0x0400387B RID: 14459
		Pickup,
		// Token: 0x0400387C RID: 14460
		Drop
	}

	// Token: 0x02000FA0 RID: 4000
	public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
	{
		// Token: 0x060050F2 RID: 20722 RVA: 0x000D4CCA File Offset: 0x000D2ECA
		public SMInstance(SolidTransferArm master) : base(master)
		{
		}
	}

	// Token: 0x02000FA1 RID: 4001
	public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
	{
		// Token: 0x060050F3 RID: 20723 RVA: 0x00270214 File Offset: 0x0026E414
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidTransferArm.SMInstance smi)
			{
				smi.master.StopRotateSound();
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
		}

		// Token: 0x0400387D RID: 14461
		public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;

		// Token: 0x0400387E RID: 14462
		public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;

		// Token: 0x0400387F RID: 14463
		public SolidTransferArm.States.ReadyStates on;

		// Token: 0x02000FA2 RID: 4002
		public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
		{
			// Token: 0x04003880 RID: 14464
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;

			// Token: 0x04003881 RID: 14465
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
		}
	}

	// Token: 0x02000FA4 RID: 4004
	private class BatchUpdateContext
	{
		// Token: 0x060050FD RID: 20733 RVA: 0x00270374 File Offset: 0x0026E574
		public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms)
		{
			this.solid_transfer_arms = ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.solid_transfer_arms.Capacity = solid_transfer_arms.Count;
			this.refreshed_reachable_cells = ListPool<bool, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.refreshed_reachable_cells.Capacity = solid_transfer_arms.Count;
			this.cells = ListPool<int, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.cells.Capacity = solid_transfer_arms.Count;
			this.game_objects = ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.Allocate();
			this.game_objects.Capacity = solid_transfer_arms.Count;
			for (int num = 0; num != solid_transfer_arms.Count; num++)
			{
				UpdateBucketWithUpdater<ISim1000ms>.Entry entry = solid_transfer_arms[num];
				entry.lastUpdateTime = 0f;
				solid_transfer_arms[num] = entry;
				SolidTransferArm solidTransferArm = (SolidTransferArm)entry.data;
				if (solidTransferArm.operational.IsOperational)
				{
					this.solid_transfer_arms.Add(solidTransferArm);
					this.refreshed_reachable_cells.Add(false);
					this.cells.Add(Grid.PosToCell(solidTransferArm));
					this.game_objects.Add(solidTransferArm.gameObject);
				}
			}
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x00270478 File Offset: 0x0026E678
		public void Finish()
		{
			for (int num = 0; num != this.solid_transfer_arms.Count; num++)
			{
				if (this.refreshed_reachable_cells[num])
				{
					this.solid_transfer_arms[num].IncrementSerialNo();
				}
				this.solid_transfer_arms[num].Sim();
			}
			this.refreshed_reachable_cells.Recycle();
			this.cells.Recycle();
			this.game_objects.Recycle();
			this.solid_transfer_arms.Recycle();
		}

		// Token: 0x04003888 RID: 14472
		public ListPool<SolidTransferArm, SolidTransferArm.BatchUpdateContext>.PooledList solid_transfer_arms;

		// Token: 0x04003889 RID: 14473
		public ListPool<bool, SolidTransferArm.BatchUpdateContext>.PooledList refreshed_reachable_cells;

		// Token: 0x0400388A RID: 14474
		public ListPool<int, SolidTransferArm.BatchUpdateContext>.PooledList cells;

		// Token: 0x0400388B RID: 14475
		public ListPool<GameObject, SolidTransferArm.BatchUpdateContext>.PooledList game_objects;
	}

	// Token: 0x02000FA5 RID: 4005
	private struct BatchUpdateTask : IWorkItem<SolidTransferArm.BatchUpdateContext>
	{
		// Token: 0x060050FF RID: 20735 RVA: 0x000D4CFC File Offset: 0x000D2EFC
		public BatchUpdateTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			this.reachable_cells_workspace = HashSetPool<int, SolidTransferArm>.Allocate();
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x002704F8 File Offset: 0x0026E6F8
		public void Run(SolidTransferArm.BatchUpdateContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				context.refreshed_reachable_cells[num] = context.solid_transfer_arms[num].AsyncUpdate(context.cells[num], this.reachable_cells_workspace, context.game_objects[num]);
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x000D4D17 File Offset: 0x000D2F17
		public void Finish()
		{
			this.reachable_cells_workspace.Recycle();
		}

		// Token: 0x0400388C RID: 14476
		private int start;

		// Token: 0x0400388D RID: 14477
		private int end;

		// Token: 0x0400388E RID: 14478
		private HashSetPool<int, SolidTransferArm>.PooledHashSet reachable_cells_workspace;
	}

	// Token: 0x02000FA6 RID: 4006
	public struct CachedPickupable
	{
		// Token: 0x0400388F RID: 14479
		public Pickupable pickupable;

		// Token: 0x04003890 RID: 14480
		public int storage_cell;
	}
}
