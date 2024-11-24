using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class MorbRoverMaker : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>
{
	// Token: 0x060014EC RID: 5356 RVA: 0x00191C74 File Offset: 0x0018FE74
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.no_operational;
		this.root.Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.GermsRequiredFeedbackUpdate), UpdateRate.SIM_1000ms, false);
		this.no_operational.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Disable manual delivery while no operational. in case players disabled the machine on purpose for this reason");
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.operational.TagTransition(GameTags.Operational, this.no_operational, true).DefaultState(this.operational.covered);
		this.operational.covered.ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerDusty, null).ParamTransition<bool>(this.WasUncoverByDuplicant, this.operational.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue).Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.DisableManualDelivery(smi, "Machine can't ask for materials if it has not been investigated by a dupe");
		}).DefaultState(this.operational.covered.idle);
		this.operational.covered.idle.PlayAnim("dusty").ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.careOrderGiven, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsTrue);
		this.operational.covered.careOrderGiven.PlayAnim("dusty").Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_RevealMachine)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_RevealMachine)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_RevealMachine(), this.operational.covered.complete).ParamTransition<bool>(this.UncoverOrderRequested, this.operational.covered.idle, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsFalse);
		this.operational.covered.complete.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SetUncovered));
		this.operational.idle.Enter(delegate(MorbRoverMaker.Instance smi)
		{
			MorbRoverMaker.EnableManualDelivery(smi, "Operational and discovered");
		}).EnterTransition(this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).EnterTransition(this.operational.waitingForMorb, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.IsCraftingCompleted)).EventTransition(GameHashes.OnStorageChange, this.operational.crafting, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting)).PlayAnim("idle").ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.crafting.DefaultState(this.operational.crafting.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerCraftingBody, null);
		this.operational.crafting.conflict.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ResetRoverBodyCraftingProgress)).GoTo(this.operational.idle);
		this.operational.crafting.pre.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).PlayAnim("crafting_pre").OnAnimQueueComplete(this.operational.crafting.loop);
		this.operational.crafting.loop.EventTransition(GameHashes.OnStorageChange, this.operational.crafting.conflict, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Not(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Transition.ConditionCallback(MorbRoverMaker.ShouldBeCrafting))).Update(new Action<MorbRoverMaker.Instance, float>(MorbRoverMaker.CraftingUpdate), UpdateRate.SIM_200ms, false).PlayAnim("crafting_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.CraftProgress, this.operational.crafting.pst, GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.IsOne);
		this.operational.crafting.pst.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.ConsumeRoverBodyCraftingMaterials)).PlayAnim("crafting_pst").OnAnimQueueComplete(this.operational.waitingForMorb);
		this.operational.waitingForMorb.PlayAnim("crafting_complete").ParamTransition<long>(this.Germs, this.operational.doctor, new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.Parameter<long>.Callback(MorbRoverMaker.HasEnoughGerms)).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerGermCollectionProgress, null);
		this.operational.doctor.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.StartWorkChore_ReleaseRover)).Exit(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.CancelWorkChore_ReleaseRover)).WorkableCompleteTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.finish).DefaultState(this.operational.doctor.needed);
		this.operational.doctor.needed.PlayAnim("waiting", KAnim.PlayMode.Loop).WorkableStartTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.working).ToggleStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerReadyForDoctor, null);
		this.operational.doctor.working.WorkableStopTransition((MorbRoverMaker.Instance smi) => smi.GetWorkable_ReleaseRover(), this.operational.doctor.needed);
		this.operational.finish.Enter(new StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State.Callback(MorbRoverMaker.SpawnRover)).GoTo(this.operational.idle);
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000AF376 File Offset: 0x000AD576
	public static bool ShouldBeCrafting(MorbRoverMaker.Instance smi)
	{
		return smi.HasMaterialsForRover && smi.RoverDevelopment_Progress < 1f;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x000AF38F File Offset: 0x000AD58F
	public static bool IsCraftingCompleted(MorbRoverMaker.Instance smi)
	{
		return smi.RoverDevelopment_Progress == 1f;
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000AF39E File Offset: 0x000AD59E
	public static bool HasEnoughGerms(MorbRoverMaker.Instance smi, long germCount)
	{
		return germCount >= smi.def.GERMS_PER_ROVER;
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000AF3B1 File Offset: 0x000AD5B1
	public static void StartWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_ReleaseRover();
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x000AF3B9 File Offset: 0x000AD5B9
	public static void CancelWorkChore_ReleaseRover(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_ReleaseRover();
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x000AF3C1 File Offset: 0x000AD5C1
	public static void StartWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CreateWorkChore_RevealMachine();
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x000AF3C9 File Offset: 0x000AD5C9
	public static void CancelWorkChore_RevealMachine(MorbRoverMaker.Instance smi)
	{
		smi.CancelWorkChore_RevealMachine();
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x000AF3D1 File Offset: 0x000AD5D1
	public static void SetUncovered(MorbRoverMaker.Instance smi)
	{
		smi.Uncover();
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x000AF3D9 File Offset: 0x000AD5D9
	public static void SpawnRover(MorbRoverMaker.Instance smi)
	{
		smi.SpawnRover();
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x000AF3E1 File Offset: 0x000AD5E1
	public static void EnableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.EnableManualDelivery(reason);
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000AF3EA File Offset: 0x000AD5EA
	public static void DisableManualDelivery(MorbRoverMaker.Instance smi, string reason)
	{
		smi.DisableManualDelivery(reason);
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000AF3F3 File Offset: 0x000AD5F3
	public static void ConsumeRoverBodyCraftingMaterials(MorbRoverMaker.Instance smi)
	{
		smi.ConsumeRoverBodyCraftingMaterials();
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x000AF3FB File Offset: 0x000AD5FB
	public static void ResetRoverBodyCraftingProgress(MorbRoverMaker.Instance smi)
	{
		smi.SetRoverDevelopmentProgress(0f);
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x00192240 File Offset: 0x00190440
	public static void CraftingUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		float roverDevelopmentProgress = Mathf.Clamp((smi.RoverDevelopment_Progress * smi.def.ROVER_CRAFTING_DURATION + dt) / smi.def.ROVER_CRAFTING_DURATION, 0f, 1f);
		smi.SetRoverDevelopmentProgress(roverDevelopmentProgress);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00192284 File Offset: 0x00190484
	public static void GermsRequiredFeedbackUpdate(MorbRoverMaker.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() - smi.lastTimeGermsAdded > smi.def.FEEDBACK_NO_GERMS_DETECTED_TIMEOUT & smi.MorbDevelopment_Progress < 1f & !smi.IsInsideState(smi.sm.operational.doctor) & smi.HasBeenRevealed)
		{
			smi.ShowGermRequiredStatusItemAlert();
			return;
		}
		smi.HideGermRequiredStatusItemAlert();
	}

	// Token: 0x04000DF3 RID: 3571
	private const string ROBOT_PROGRESS_METER_TARGET_NAME = "meter_robot_target";

	// Token: 0x04000DF4 RID: 3572
	private const string ROBOT_PROGRESS_METER_ANIMATION_NAME = "meter_robot";

	// Token: 0x04000DF5 RID: 3573
	private const string COVERED_IDLE_ANIM_NAME = "dusty";

	// Token: 0x04000DF6 RID: 3574
	private const string IDLE_ANIM_NAME = "idle";

	// Token: 0x04000DF7 RID: 3575
	private const string CRAFT_PRE_ANIM_NAME = "crafting_pre";

	// Token: 0x04000DF8 RID: 3576
	private const string CRAFT_LOOP_ANIM_NAME = "crafting_loop";

	// Token: 0x04000DF9 RID: 3577
	private const string CRAFT_PST_ANIM_NAME = "crafting_pst";

	// Token: 0x04000DFA RID: 3578
	private const string CRAFT_COMPLETED_ANIM_NAME = "crafting_complete";

	// Token: 0x04000DFB RID: 3579
	private const string WAITING_FOR_DOCTOR_ANIM_NAME = "waiting";

	// Token: 0x04000DFC RID: 3580
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter UncoverOrderRequested;

	// Token: 0x04000DFD RID: 3581
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.BoolParameter WasUncoverByDuplicant;

	// Token: 0x04000DFE RID: 3582
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.LongParameter Germs;

	// Token: 0x04000DFF RID: 3583
	public StateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.FloatParameter CraftProgress;

	// Token: 0x04000E00 RID: 3584
	public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State no_operational;

	// Token: 0x04000E01 RID: 3585
	public MorbRoverMaker.OperationalStates operational;

	// Token: 0x020004A7 RID: 1191
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060014FD RID: 5373 RVA: 0x001922F0 File Offset: 0x001904F0
		public float GetConduitMaxPackageMass()
		{
			ConduitType germ_INTAKE_CONDUIT_TYPE = this.GERM_INTAKE_CONDUIT_TYPE;
			if (germ_INTAKE_CONDUIT_TYPE == ConduitType.Gas)
			{
				return 1f;
			}
			if (germ_INTAKE_CONDUIT_TYPE != ConduitType.Liquid)
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x04000E02 RID: 3586
		public float FEEDBACK_NO_GERMS_DETECTED_TIMEOUT = 2f;

		// Token: 0x04000E03 RID: 3587
		public Tag ROVER_PREFAB_ID;

		// Token: 0x04000E04 RID: 3588
		public float INITIAL_MORB_DEVELOPMENT_PERCENTAGE;

		// Token: 0x04000E05 RID: 3589
		public float ROVER_CRAFTING_DURATION;

		// Token: 0x04000E06 RID: 3590
		public float METAL_PER_ROVER;

		// Token: 0x04000E07 RID: 3591
		public long GERMS_PER_ROVER;

		// Token: 0x04000E08 RID: 3592
		public int MAX_GERMS_TAKEN_PER_PACKAGE;

		// Token: 0x04000E09 RID: 3593
		public int GERM_TYPE;

		// Token: 0x04000E0A RID: 3594
		public SimHashes ROVER_MATERIAL;

		// Token: 0x04000E0B RID: 3595
		public ConduitType GERM_INTAKE_CONDUIT_TYPE;
	}

	// Token: 0x020004A8 RID: 1192
	public class CoverStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E0C RID: 3596
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04000E0D RID: 3597
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State careOrderGiven;

		// Token: 0x04000E0E RID: 3598
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State complete;
	}

	// Token: 0x020004A9 RID: 1193
	public class OperationalStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E0F RID: 3599
		public MorbRoverMaker.CoverStates covered;

		// Token: 0x04000E10 RID: 3600
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State idle;

		// Token: 0x04000E11 RID: 3601
		public MorbRoverMaker.CraftingStates crafting;

		// Token: 0x04000E12 RID: 3602
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State waitingForMorb;

		// Token: 0x04000E13 RID: 3603
		public MorbRoverMaker.DoctorStates doctor;

		// Token: 0x04000E14 RID: 3604
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State finish;
	}

	// Token: 0x020004AA RID: 1194
	public class DoctorStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E15 RID: 3605
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State needed;

		// Token: 0x04000E16 RID: 3606
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State working;
	}

	// Token: 0x020004AB RID: 1195
	public class CraftingStates : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State
	{
		// Token: 0x04000E17 RID: 3607
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State conflict;

		// Token: 0x04000E18 RID: 3608
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pre;

		// Token: 0x04000E19 RID: 3609
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State loop;

		// Token: 0x04000E1A RID: 3610
		public GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.State pst;
	}

	// Token: 0x020004AC RID: 1196
	public new class Instance : GameStateMachine<MorbRoverMaker, MorbRoverMaker.Instance, IStateMachineTarget, MorbRoverMaker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x000AF42B File Offset: 0x000AD62B
		public long MorbDevelopment_GermsCollected
		{
			get
			{
				return base.sm.Germs.Get(base.smi);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x000AF443 File Offset: 0x000AD643
		public long MorbDevelopment_RemainingGerms
		{
			get
			{
				return base.def.GERMS_PER_ROVER - this.MorbDevelopment_GermsCollected;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x000AF457 File Offset: 0x000AD657
		public float MorbDevelopment_Progress
		{
			get
			{
				return Mathf.Clamp((float)this.MorbDevelopment_GermsCollected / (float)base.def.GERMS_PER_ROVER, 0f, 1f);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x000AF47C File Offset: 0x000AD67C
		public bool HasMaterialsForRover
		{
			get
			{
				return this.storage.GetMassAvailable(base.def.ROVER_MATERIAL) >= base.def.METAL_PER_ROVER;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x000AF4A4 File Offset: 0x000AD6A4
		public float RoverDevelopment_Progress
		{
			get
			{
				return base.sm.CraftProgress.Get(base.smi);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x000AF4BC File Offset: 0x000AD6BC
		public bool HasBeenRevealed
		{
			get
			{
				return base.sm.WasUncoverByDuplicant.Get(base.smi);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x000AF4D4 File Offset: 0x000AD6D4
		public bool CanPumpGerms
		{
			get
			{
				return this.operational && this.MorbDevelopment_Progress < 1f && this.HasBeenRevealed;
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x000AF4F8 File Offset: 0x000AD6F8
		public Workable GetWorkable_RevealMachine()
		{
			return this.workable_reveal;
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x000AF500 File Offset: 0x000AD700
		public Workable GetWorkable_ReleaseRover()
		{
			return this.workable_release;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00192320 File Offset: 0x00190520
		public void ShowGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle == default(Guid))
			{
				this.germsRequiredAlertStatusItemHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MorbRoverMakerNoGermsConsumedAlert, base.smi);
			}
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0019236C File Offset: 0x0019056C
		public void HideGermRequiredStatusItemAlert()
		{
			if (this.germsRequiredAlertStatusItemHandle != default(Guid))
			{
				this.selectable.RemoveStatusItem(this.germsRequiredAlertStatusItemHandle, false);
				this.germsRequiredAlertStatusItemHandle = default(Guid);
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x001923B0 File Offset: 0x001905B0
		public Instance(IStateMachineTarget master, MorbRoverMaker.Def def) : base(master, def)
		{
			this.RobotProgressMeter = new MeterController(this.buildingAnimCtr, "meter_robot_target", "meter_robot", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00192418 File Offset: 0x00190618
		public override void StartSM()
		{
			Building component = base.GetComponent<Building>();
			this.inputCell = component.GetUtilityInputCell();
			this.outputCell = component.GetUtilityOutputCell();
			base.StartSM();
			if (!this.HasBeenRevealed)
			{
				base.sm.Germs.Set(0L, base.smi, false);
				this.AddGerms((long)((float)base.def.GERMS_PER_ROVER * base.def.INITIAL_MORB_DEVELOPMENT_PERCENTAGE), false);
			}
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).AddConduitUpdater(new Action<float>(this.Flow), ConduitFlowPriority.Default);
			this.UpdateMeters();
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x001924B4 File Offset: 0x001906B4
		public void AddGerms(long amount, bool playAnimations = true)
		{
			long value = this.MorbDevelopment_GermsCollected + amount;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			if (amount > 0L)
			{
				if (playAnimations)
				{
					this.capsule.PlayPumpGermsAnimation();
				}
				Action<long> germsAdded = this.GermsAdded;
				if (germsAdded != null)
				{
					germsAdded(amount);
				}
				this.lastTimeGermsAdded = GameClock.Instance.GetTime();
			}
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00192520 File Offset: 0x00190720
		public long RemoveGerms(long amount)
		{
			long num = amount.Min(this.MorbDevelopment_GermsCollected);
			long value = this.MorbDevelopment_GermsCollected - num;
			base.sm.Germs.Set(value, base.smi, false);
			this.UpdateMeters();
			return num;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x000AF508 File Offset: 0x000AD708
		public void EnableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(false, reason);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000AF517 File Offset: 0x000AD717
		public void DisableManualDelivery(string reason)
		{
			this.manualDelivery.Pause(true, reason);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000AF526 File Offset: 0x000AD726
		public void SetRoverDevelopmentProgress(float value)
		{
			base.sm.CraftProgress.Set(value, base.smi, false);
			this.UpdateMeters();
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00192564 File Offset: 0x00190764
		public void UpdateMeters()
		{
			this.RobotProgressMeter.SetPositionPercent(this.RoverDevelopment_Progress);
			this.capsule.SetMorbDevelopmentProgress(this.MorbDevelopment_Progress);
			this.capsule.SetGermMeterProgress(this.HasBeenRevealed ? this.MorbDevelopment_Progress : 0f);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000AF547 File Offset: 0x000AD747
		public void Uncover()
		{
			base.sm.WasUncoverByDuplicant.Set(true, base.smi, false);
			System.Action onUncovered = this.OnUncovered;
			if (onUncovered == null)
			{
				return;
			}
			onUncovered();
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x001925B4 File Offset: 0x001907B4
		public void CreateWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover == null)
			{
				this.workChore_releaseRover = new WorkChore<MorbRoverMakerWorkable>(Db.Get().ChoreTypes.Doctor, this.workable_release, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000AF572 File Offset: 0x000AD772
		public void CancelWorkChore_ReleaseRover()
		{
			if (this.workChore_releaseRover != null)
			{
				this.workChore_releaseRover.Cancel("MorbRoverMaker.CancelWorkChore_ReleaseRover");
				this.workChore_releaseRover = null;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x001925FC File Offset: 0x001907FC
		public void CreateWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine == null)
			{
				this.workChore_revealMachine = new WorkChore<MorbRoverMakerRevealWorkable>(Db.Get().ChoreTypes.Repair, this.workable_reveal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000AF593 File Offset: 0x000AD793
		public void CancelWorkChore_RevealMachine()
		{
			if (this.workChore_revealMachine != null)
			{
				this.workChore_revealMachine.Cancel("MorbRoverMaker.CancelWorkChore_RevealMachine");
				this.workChore_revealMachine = null;
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00192644 File Offset: 0x00190844
		public void ConsumeRoverBodyCraftingMaterials()
		{
			float num = 0f;
			this.storage.ConsumeAndGetDisease(base.def.ROVER_MATERIAL.CreateTag(), base.def.METAL_PER_ROVER, out num, out this.lastastMaterialsConsumedDiseases, out this.lastastMaterialsConsumedTemp);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0019268C File Offset: 0x0019088C
		public void SpawnRover()
		{
			if (this.RoverDevelopment_Progress == 1f)
			{
				this.RemoveGerms(base.def.GERMS_PER_ROVER);
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(base.def.ROVER_PREFAB_ID), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0);
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.lastastMaterialsConsumedDiseases.idx != 255)
				{
					component.AddDisease(this.lastastMaterialsConsumedDiseases.idx, this.lastastMaterialsConsumedDiseases.count, "From the materials provided for its creation");
				}
				if (this.lastastMaterialsConsumedTemp > 0f)
				{
					component.SetMassTemperature(component.Mass, this.lastastMaterialsConsumedTemp);
				}
				gameObject.SetActive(true);
				this.SetRoverDevelopmentProgress(0f);
				Action<GameObject> onRoverSpawned = this.OnRoverSpawned;
				if (onRoverSpawned == null)
				{
					return;
				}
				onRoverSpawned(gameObject);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00192764 File Offset: 0x00190964
		private void Flow(float dt)
		{
			if (this.CanPumpGerms)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE);
				int num = 0;
				if (flowManager.HasConduit(this.inputCell) && flowManager.HasConduit(this.outputCell))
				{
					ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
					ConduitFlow.ConduitContents contents2 = flowManager.GetContents(this.outputCell);
					float num2 = Mathf.Min(contents.mass, base.def.GetConduitMaxPackageMass() * dt);
					if (flowManager.CanMergeContents(contents, contents2, num2))
					{
						float amountAllowedForMerging = flowManager.GetAmountAllowedForMerging(contents, contents2, num2);
						if (amountAllowedForMerging > 0f)
						{
							ConduitFlow conduitFlow = (base.def.GERM_INTAKE_CONDUIT_TYPE == ConduitType.Liquid) ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
							int num3 = contents.diseaseCount;
							if (contents.diseaseIdx != 255 && (int)contents.diseaseIdx == base.def.GERM_TYPE)
							{
								num = (int)this.MorbDevelopment_RemainingGerms.Min((long)base.def.MAX_GERMS_TAKEN_PER_PACKAGE).Min((long)contents.diseaseCount);
								num3 -= num;
							}
							float num4 = conduitFlow.AddElement(this.outputCell, contents.element, amountAllowedForMerging, contents.temperature, contents.diseaseIdx, num3);
							if (amountAllowedForMerging != num4)
							{
								global::Debug.Log("[Morb Rover Maker] Mass Differs By: " + (amountAllowedForMerging - num4).ToString());
							}
							flowManager.RemoveElement(this.inputCell, num4);
						}
					}
				}
				if (num > 0)
				{
					this.AddGerms((long)num, true);
				}
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000AF5B4 File Offset: 0x000AD7B4
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Conduit.GetFlowManager(base.def.GERM_INTAKE_CONDUIT_TYPE).RemoveConduitUpdater(new Action<float>(this.Flow));
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x000AF5DD File Offset: 0x000AD7DD
		public string SidescreenButtonText
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x000AF617 File Offset: 0x000AD817
		public string SidescreenButtonTooltip
		{
			get
			{
				return this.HasBeenRevealed ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.DROP_INVENTORY_TOOLTIP : (base.sm.UncoverOrderRequested.Get(base.smi) ? CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.CANCEL_REVEAL_BTN_TOOLTIP : CODEX.STORY_TRAITS.MORB_ROVER_MAKER.UI_SIDESCREENS.REVEAL_BTN_TOOLTIP);
			}
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000ABCBD File Offset: 0x000A9EBD
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x001928E8 File Offset: 0x00190AE8
		public void OnSidescreenButtonPressed()
		{
			if (this.HasBeenRevealed)
			{
				this.storage.DropAll(false, false, default(Vector3), true, null);
				return;
			}
			bool flag = base.smi.sm.UncoverOrderRequested.Get(base.smi);
			base.smi.sm.UncoverOrderRequested.Set(!flag, base.smi, false);
		}

		// Token: 0x04000E1B RID: 3611
		public Action<long> GermsAdded;

		// Token: 0x04000E1C RID: 3612
		public System.Action OnUncovered;

		// Token: 0x04000E1D RID: 3613
		public Action<GameObject> OnRoverSpawned;

		// Token: 0x04000E1E RID: 3614
		[MyCmpGet]
		private MorbRoverMakerRevealWorkable workable_reveal;

		// Token: 0x04000E1F RID: 3615
		[MyCmpGet]
		private MorbRoverMakerWorkable workable_release;

		// Token: 0x04000E20 RID: 3616
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04000E21 RID: 3617
		[MyCmpGet]
		private KBatchedAnimController buildingAnimCtr;

		// Token: 0x04000E22 RID: 3618
		[MyCmpGet]
		private ManualDeliveryKG manualDelivery;

		// Token: 0x04000E23 RID: 3619
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04000E24 RID: 3620
		[MyCmpGet]
		private MorbRoverMaker_Capsule capsule;

		// Token: 0x04000E25 RID: 3621
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04000E26 RID: 3622
		private MeterController RobotProgressMeter;

		// Token: 0x04000E27 RID: 3623
		private int inputCell = -1;

		// Token: 0x04000E28 RID: 3624
		private int outputCell = -1;

		// Token: 0x04000E29 RID: 3625
		private Chore workChore_revealMachine;

		// Token: 0x04000E2A RID: 3626
		private Chore workChore_releaseRover;

		// Token: 0x04000E2B RID: 3627
		[Serialize]
		private float lastastMaterialsConsumedTemp = -1f;

		// Token: 0x04000E2C RID: 3628
		[Serialize]
		private SimUtil.DiseaseInfo lastastMaterialsConsumedDiseases = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04000E2D RID: 3629
		public float lastTimeGermsAdded = -1f;

		// Token: 0x04000E2E RID: 3630
		private Guid germsRequiredAlertStatusItemHandle;
	}
}
