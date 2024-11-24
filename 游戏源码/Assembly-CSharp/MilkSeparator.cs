using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EB4 RID: 3764
public class MilkSeparator : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>
{
	// Token: 0x06004BDD RID: 19421 RVA: 0x0025FDE4 File Offset: 0x0025DFE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.noOperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.RefreshMeters));
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
		this.operational.TagTransition(GameTags.Operational, this.noOperational, true).PlayAnim("on").DefaultState(this.operational.idle);
		this.operational.idle.EventTransition(GameHashes.OnStorageChange, this.operational.working.pre, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanBeginSeparate)).EnterTransition(this.operational.full, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.RequiresEmptying));
		this.operational.working.pre.QueueAnim("separating_pre", false, null).OnAnimQueueComplete(this.operational.working.work);
		this.operational.working.work.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.BeginSeparation)).PlayAnim("separating_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStorageChange, this.operational.working.post, new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.Transition.ConditionCallback(MilkSeparator.CanNOTKeepSeparating)).Exit(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.EndSeparation));
		this.operational.working.post.QueueAnim("separating_pst", false, null).OnAnimQueueComplete(this.operational.idle);
		this.operational.full.PlayAnim("ready").ToggleRecurringChore(new Func<MilkSeparator.Instance, Chore>(MilkSeparator.CreateEmptyChore), null).WorkableCompleteTransition((MilkSeparator.Instance smi) => smi.workable, this.operational.emptyComplete).ToggleStatusItem(Db.Get().BuildingStatusItems.MilkSeparatorNeedsEmptying, null);
		this.operational.emptyComplete.Enter(new StateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State.Callback(MilkSeparator.DropMilkFat)).ScheduleActionNextFrame("AfterMilkFatDrop", delegate(MilkSeparator.Instance smi)
		{
			smi.GoTo(this.operational.idle);
		});
	}

	// Token: 0x06004BDE RID: 19422 RVA: 0x000D10CF File Offset: 0x000CF2CF
	public static void BeginSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(true, false);
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x000D10DE File Offset: 0x000CF2DE
	public static void EndSeparation(MilkSeparator.Instance smi)
	{
		smi.operational.SetActive(false, false);
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x000D10ED File Offset: 0x000CF2ED
	public static bool CanBeginSeparate(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x000D1105 File Offset: 0x000CF305
	public static bool CanKeepSeparating(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached && smi.elementConverter.CanConvertAtAll();
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x000D111C File Offset: 0x000CF31C
	public static bool CanNOTKeepSeparating(MilkSeparator.Instance smi)
	{
		return !MilkSeparator.CanKeepSeparating(smi);
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x000D1127 File Offset: 0x000CF327
	public static bool RequiresEmptying(MilkSeparator.Instance smi)
	{
		return smi.MilkFatLimitReached;
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x000D112F File Offset: 0x000CF32F
	public static bool ThereIsCapacityForMilkFat(MilkSeparator.Instance smi)
	{
		return !smi.MilkFatLimitReached;
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x000D113A File Offset: 0x000CF33A
	public static void DropMilkFat(MilkSeparator.Instance smi)
	{
		smi.DropMilkFat();
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x000D1142 File Offset: 0x000CF342
	public static void RefreshMeters(MilkSeparator.Instance smi)
	{
		smi.RefreshMeters();
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x00260024 File Offset: 0x0025E224
	private static Chore CreateEmptyChore(MilkSeparator.Instance smi)
	{
		return new WorkChore<EmptyMilkSeparatorWorkable>(Db.Get().ChoreTypes.EmptyStorage, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04003491 RID: 13457
	public const string WORK_PRE_ANIM_NAME = "separating_pre";

	// Token: 0x04003492 RID: 13458
	public const string WORK_ANIM_NAME = "separating_loop";

	// Token: 0x04003493 RID: 13459
	public const string WORK_POST_ANIM_NAME = "separating_pst";

	// Token: 0x04003494 RID: 13460
	public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State noOperational;

	// Token: 0x04003495 RID: 13461
	public MilkSeparator.OperationalStates operational;

	// Token: 0x02000EB5 RID: 3765
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06004BEA RID: 19434 RVA: 0x000D1165 File Offset: 0x000CF365
		public Def()
		{
			this.MILK_FAT_TAG = ElementLoader.FindElementByHash(SimHashes.MilkFat).tag;
			this.MILK_TAG = ElementLoader.FindElementByHash(SimHashes.Milk).tag;
		}

		// Token: 0x04003496 RID: 13462
		public float MILK_FAT_CAPACITY = 100f;

		// Token: 0x04003497 RID: 13463
		public Tag MILK_TAG;

		// Token: 0x04003498 RID: 13464
		public Tag MILK_FAT_TAG;
	}

	// Token: 0x02000EB6 RID: 3766
	public class WorkingStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x04003499 RID: 13465
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State pre;

		// Token: 0x0400349A RID: 13466
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State work;

		// Token: 0x0400349B RID: 13467
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State post;
	}

	// Token: 0x02000EB7 RID: 3767
	public class OperationalStates : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State
	{
		// Token: 0x0400349C RID: 13468
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State idle;

		// Token: 0x0400349D RID: 13469
		public MilkSeparator.WorkingStates working;

		// Token: 0x0400349E RID: 13470
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State full;

		// Token: 0x0400349F RID: 13471
		public GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.State emptyComplete;
	}

	// Token: 0x02000EB8 RID: 3768
	public new class Instance : GameStateMachine<MilkSeparator, MilkSeparator.Instance, IStateMachineTarget, MilkSeparator.Def>.GameInstance
	{
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06004BED RID: 19437 RVA: 0x000D11AA File Offset: 0x000CF3AA
		public float MilkFatStored
		{
			get
			{
				return this.storage.GetAmountAvailable(base.def.MILK_FAT_TAG);
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x000D11C2 File Offset: 0x000CF3C2
		public float MilkFatStoragePercentage
		{
			get
			{
				return Mathf.Clamp(this.MilkFatStored / base.def.MILK_FAT_CAPACITY, 0f, 1f);
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06004BEF RID: 19439 RVA: 0x000D11E5 File Offset: 0x000CF3E5
		public bool MilkFatLimitReached
		{
			get
			{
				return this.MilkFatStored >= base.def.MILK_FAT_CAPACITY;
			}
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x0026005C File Offset: 0x0025E25C
		public Instance(IStateMachineTarget master, MilkSeparator.Def def) : base(master, def)
		{
			KAnimControllerBase component = base.GetComponent<KBatchedAnimController>();
			this.fatMeter = new MeterController(component, "meter_target_1", "meter_fat", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target_1"
			});
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x000D11FD File Offset: 0x000CF3FD
		public override void StartSM()
		{
			base.StartSM();
			this.workable.OnWork_PST_Begins = new System.Action(this.Play_Empty_MeterAnimation);
			this.RefreshMeters();
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x000D1222 File Offset: 0x000CF422
		private void Play_Empty_MeterAnimation()
		{
			this.fatMeter.SetPositionPercent(0f);
			this.fatMeter.meterController.Play("meter_fat_empty", KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x002600A0 File Offset: 0x0025E2A0
		public void DropMilkFat()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Drop(base.def.MILK_FAT_TAG, list);
			Vector3 dropSpawnLocation = this.GetDropSpawnLocation();
			foreach (GameObject gameObject in list)
			{
				gameObject.transform.position = dropSpawnLocation;
			}
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00260118 File Offset: 0x0025E318
		private Vector3 GetDropSpawnLocation()
		{
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(new HashedString("milkfat"), out flag).GetColumn(3);
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				return vector;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x00260184 File Offset: 0x0025E384
		public void RefreshMeters()
		{
			if (this.fatMeter.meterController.currentAnim != "meter_fat")
			{
				this.fatMeter.meterController.Play("meter_fat", KAnim.PlayMode.Paused, 1f, 0f);
			}
			this.fatMeter.SetPositionPercent(this.MilkFatStoragePercentage);
		}

		// Token: 0x040034A0 RID: 13472
		[MyCmpGet]
		public EmptyMilkSeparatorWorkable workable;

		// Token: 0x040034A1 RID: 13473
		[MyCmpGet]
		public Operational operational;

		// Token: 0x040034A2 RID: 13474
		[MyCmpGet]
		public ElementConverter elementConverter;

		// Token: 0x040034A3 RID: 13475
		[MyCmpGet]
		private Storage storage;

		// Token: 0x040034A4 RID: 13476
		private MeterController fatMeter;
	}
}
