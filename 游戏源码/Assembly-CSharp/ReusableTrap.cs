using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017BC RID: 6076
public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	// Token: 0x06007D2D RID: 32045 RVA: 0x00325C40 File Offset: 0x00323E40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.operational;
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
		this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>(this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
		this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
		this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.unarmed);
		this.operational.unarmed.ParamTransition<bool>(this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming, null).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
		this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed, null).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
		this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
		this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK, false).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetCaptureAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
		this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted, null).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (ReusableTrap.Instance smi) => smi.CapturedCritter).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetIdleAnimationName), KAnim.PlayMode.Once);
		this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), false, null).OnAnimQueueComplete(this.operational.unarmed);
	}

	// Token: 0x06007D2E RID: 32046 RVA: 0x000F2687 File Offset: 0x000F0887
	public static void RefreshLogicOutput(ReusableTrap.Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	// Token: 0x06007D2F RID: 32047 RVA: 0x000F268F File Offset: 0x000F088F
	public static void Release(ReusableTrap.Instance smi)
	{
		smi.Release();
	}

	// Token: 0x06007D30 RID: 32048 RVA: 0x000F2697 File Offset: 0x000F0897
	public static void StartArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06007D31 RID: 32049 RVA: 0x000F269F File Offset: 0x000F089F
	public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06007D32 RID: 32050 RVA: 0x000F26A7 File Offset: 0x000F08A7
	public static string GetIdleAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture_idle";
		}
		return "capture_idle_large";
	}

	// Token: 0x06007D33 RID: 32051 RVA: 0x000F26BC File Offset: 0x000F08BC
	public static string GetCaptureAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture";
		}
		return "capture_large";
	}

	// Token: 0x06007D34 RID: 32052 RVA: 0x000F26D1 File Offset: 0x000F08D1
	public static string GetReleaseAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.WasLastCritterLarge)
		{
			return "release";
		}
		return "release_large";
	}

	// Token: 0x06007D35 RID: 32053 RVA: 0x000F26E6 File Offset: 0x000F08E6
	public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06007D36 RID: 32054 RVA: 0x000F26F1 File Offset: 0x000F08F1
	public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	// Token: 0x06007D37 RID: 32055 RVA: 0x000F26F1 File Offset: 0x000F08F1
	public static bool StorageContainsCritter(ReusableTrap.Instance smi)
	{
		return smi.HasCritter;
	}

	// Token: 0x06007D38 RID: 32056 RVA: 0x000F26E6 File Offset: 0x000F08E6
	public static bool StorageIsEmpty(ReusableTrap.Instance smi)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06007D39 RID: 32057 RVA: 0x000F26F9 File Offset: 0x000F08F9
	public static void EnableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(true);
	}

	// Token: 0x06007D3A RID: 32058 RVA: 0x000F2702 File Offset: 0x000F0902
	public static void DisableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(false);
	}

	// Token: 0x06007D3B RID: 32059 RVA: 0x000F270B File Offset: 0x000F090B
	public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x06007D3C RID: 32060 RVA: 0x000F2713 File Offset: 0x000F0913
	public static void ActivateLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(true);
	}

	// Token: 0x06007D3D RID: 32061 RVA: 0x000F271C File Offset: 0x000F091C
	public static void DisableLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(false);
	}

	// Token: 0x06007D3E RID: 32062 RVA: 0x000F2725 File Offset: 0x000F0925
	public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	// Token: 0x06007D3F RID: 32063 RVA: 0x000F272D File Offset: 0x000F092D
	public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	// Token: 0x06007D40 RID: 32064 RVA: 0x003260B8 File Offset: 0x003242B8
	public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME, KAnim.PlayMode.Once, 1f, 0f);
			}
			bool flag;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out flag).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	// Token: 0x06007D41 RID: 32065 RVA: 0x000F2735 File Offset: 0x000F0935
	public static void MarkAsArmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(true, smi, false);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	// Token: 0x06007D42 RID: 32066 RVA: 0x000F275B File Offset: 0x000F095B
	public static void MarkAsUnarmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(false, smi, false);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}

	// Token: 0x04005EBC RID: 24252
	public const string CAPTURE_ANIMATION_NAME = "capture";

	// Token: 0x04005EBD RID: 24253
	public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";

	// Token: 0x04005EBE RID: 24254
	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	// Token: 0x04005EBF RID: 24255
	public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";

	// Token: 0x04005EC0 RID: 24256
	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	// Token: 0x04005EC1 RID: 24257
	public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";

	// Token: 0x04005EC2 RID: 24258
	public const string UNARMED_ANIMATION_NAME = "unarmed";

	// Token: 0x04005EC3 RID: 24259
	public const string ARMED_ANIMATION_NAME = "armed";

	// Token: 0x04005EC4 RID: 24260
	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	// Token: 0x04005EC5 RID: 24261
	public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;

	// Token: 0x04005EC6 RID: 24262
	public ReusableTrap.NonOperationalStates noOperational;

	// Token: 0x04005EC7 RID: 24263
	public ReusableTrap.OperationalStates operational;

	// Token: 0x020017BD RID: 6077
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06007D44 RID: 32068 RVA: 0x000F2789 File Offset: 0x000F0989
		public bool usingLure
		{
			get
			{
				return this.lures != null && this.lures.Length != 0;
			}
		}

		// Token: 0x04005EC8 RID: 24264
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04005EC9 RID: 24265
		public Tag[] lures;

		// Token: 0x04005ECA RID: 24266
		public CellOffset releaseCellOffset = CellOffset.none;

		// Token: 0x04005ECB RID: 24267
		public bool usingSymbolChaseCapturingAnimations;

		// Token: 0x04005ECC RID: 24268
		public Func<string> getTrappedAnimationNameCallback;
	}

	// Token: 0x020017BE RID: 6078
	public class CaptureStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04005ECD RID: 24269
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;

		// Token: 0x04005ECE RID: 24270
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04005ECF RID: 24271
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
	}

	// Token: 0x020017BF RID: 6079
	public class OperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04005ED0 RID: 24272
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;

		// Token: 0x04005ED1 RID: 24273
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;

		// Token: 0x04005ED2 RID: 24274
		public ReusableTrap.CaptureStates capture;
	}

	// Token: 0x020017C0 RID: 6080
	public class NonOperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04005ED3 RID: 24275
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04005ED4 RID: 24276
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;

		// Token: 0x04005ED5 RID: 24277
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
	}

	// Token: 0x020017C1 RID: 6081
	public new class Instance : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06007D49 RID: 32073 RVA: 0x000F27BA File Offset: 0x000F09BA
		public bool IsCapturingLargeCritter
		{
			get
			{
				return this.HasCritter && this.CapturedCritter.HasTag(GameTags.LargeCreature);
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06007D4A RID: 32074 RVA: 0x000F27D6 File Offset: 0x000F09D6
		public bool HasCritter
		{
			get
			{
				return !this.storage.IsEmpty();
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06007D4B RID: 32075 RVA: 0x000F27E6 File Offset: 0x000F09E6
		public GameObject CapturedCritter
		{
			get
			{
				if (!this.HasCritter)
				{
					return null;
				}
				return this.storage.items[0];
			}
		}

		// Token: 0x06007D4C RID: 32076 RVA: 0x000F2803 File Offset: 0x000F0A03
		public ArmTrapWorkable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06007D4D RID: 32077 RVA: 0x0032615C File Offset: 0x0032435C
		public void RefreshLogicOutput()
		{
			bool flag = base.IsInsideState(base.sm.operational) && this.HasCritter;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		// Token: 0x06007D4E RID: 32078 RVA: 0x000F280B File Offset: 0x000F0A0B
		public Instance(IStateMachineTarget master, ReusableTrap.Def def) : base(master, def)
		{
		}

		// Token: 0x06007D4F RID: 32079 RVA: 0x003261A8 File Offset: 0x003243A8
		public override void StartSM()
		{
			base.StartSM();
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
			}
			ArmTrapWorkable armTrapWorkable = this.workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x003261F8 File Offset: 0x003243F8
		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && base.IsInsideState(base.sm.operational.unarmed))
			{
				this.animController.Play("unarmed", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06007D51 RID: 32081 RVA: 0x000F282B File Offset: 0x000F0A2B
		public void SetTrapTriggerActiveState(bool active)
		{
			this.trapTrigger.enabled = active;
		}

		// Token: 0x06007D52 RID: 32082 RVA: 0x0032625C File Offset: 0x0032445C
		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				Lure.Instance smi = base.gameObject.GetSMI<Lure.Instance>();
				if (smi != null)
				{
					smi.SetActiveLures(activate ? base.def.lures : null);
				}
			}
		}

		// Token: 0x06007D53 RID: 32083 RVA: 0x000F2839 File Offset: 0x000F0A39
		public void SetupCapturingAnimations()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		// Token: 0x06007D54 RID: 32084 RVA: 0x0032629C File Offset: 0x0032449C
		public void UnsetCapturingAnimations()
		{
			this.trapTrigger.SetStoredPosition(this.CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && this.lastCritterCapturedAnimController != null)
			{
				this.lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.lastCritterCapturedAnimController = null;
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x003262FC File Offset: 0x003244FC
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x000F2860 File Offset: 0x000F0A60
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("GroundTrap.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06007D57 RID: 32087 RVA: 0x00326344 File Offset: 0x00324544
		public void Release()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
				List<GameObject> list = new List<GameObject>();
				Storage storage = this.storage;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list)
				{
					gameObject.transform.SetPosition(position);
					KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.SetSceneLayer(Grid.SceneLayer.Creatures);
					}
				}
			}
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x000F2881 File Offset: 0x000F0A81
		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}

		// Token: 0x04005ED6 RID: 24278
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		// Token: 0x04005ED7 RID: 24279
		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		// Token: 0x04005ED8 RID: 24280
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04005ED9 RID: 24281
		[MyCmpGet]
		private ArmTrapWorkable workable;

		// Token: 0x04005EDA RID: 24282
		[MyCmpGet]
		private TrapTrigger trapTrigger;

		// Token: 0x04005EDB RID: 24283
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04005EDC RID: 24284
		[MyCmpGet]
		public LogicPorts logicPorts;

		// Token: 0x04005EDD RID: 24285
		public bool WasLastCritterLarge;

		// Token: 0x04005EDE RID: 24286
		public KBatchedAnimController lastCritterCapturedAnimController;

		// Token: 0x04005EDF RID: 24287
		private Chore chore;
	}
}
