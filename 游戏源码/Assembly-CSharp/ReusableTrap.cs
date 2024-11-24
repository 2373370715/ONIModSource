using System;
using System.Collections.Generic;
using UnityEngine;

public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	public class Def : BaseDef
	{
		public string OUTPUT_LOGIC_PORT_ID;

		public Tag[] lures;

		public CellOffset releaseCellOffset = CellOffset.none;

		public bool usingSymbolChaseCapturingAnimations;

		public Func<string> getTrappedAnimationNameCallback;

		public bool usingLure
		{
			get
			{
				if (lures != null)
				{
					return lures.Length != 0;
				}
				return false;
			}
		}
	}

	public class CaptureStates : State
	{
		public State capturing;

		public State idle;

		public State release;
	}

	public class OperationalStates : State
	{
		public State unarmed;

		public State armed;

		public CaptureStates capture;
	}

	public class NonOperationalStates : State
	{
		public State idle;

		public State releasing;

		public State disarming;
	}

	public new class Instance : GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		[MyCmpGet]
		private Storage storage;

		[MyCmpGet]
		private ArmTrapWorkable workable;

		[MyCmpGet]
		private TrapTrigger trapTrigger;

		[MyCmpGet]
		public KBatchedAnimController animController;

		[MyCmpGet]
		public LogicPorts logicPorts;

		public bool WasLastCritterLarge;

		public KBatchedAnimController lastCritterCapturedAnimController;

		private Chore chore;

		public bool IsCapturingLargeCritter
		{
			get
			{
				if (HasCritter)
				{
					return CapturedCritter.HasTag(GameTags.LargeCreature);
				}
				return false;
			}
		}

		public bool HasCritter => !storage.IsEmpty();

		public GameObject CapturedCritter
		{
			get
			{
				if (!HasCritter)
				{
					return null;
				}
				return storage.items[0];
			}
		}

		public ArmTrapWorkable GetWorkable()
		{
			return workable;
		}

		public void RefreshLogicOutput()
		{
			bool flag = IsInsideState(base.sm.operational) && HasCritter;
			logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			if (HasCritter)
			{
				WasLastCritterLarge = IsCapturingLargeCritter;
			}
			ArmTrapWorkable armTrapWorkable = workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkEvent));
		}

		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && IsInsideState(base.sm.operational.unarmed))
			{
				animController.Play("unarmed");
			}
		}

		public void SetTrapTriggerActiveState(bool active)
		{
			trapTrigger.enabled = active;
		}

		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				base.gameObject.GetSMI<Lure.Instance>()?.SetActiveLures(activate ? base.def.lures : null);
			}
		}

		public void SetupCapturingAnimations()
		{
			if (HasCritter)
			{
				WasLastCritterLarge = IsCapturingLargeCritter;
				lastCritterCapturedAnimController = CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		public void UnsetCapturingAnimations()
		{
			trapTrigger.SetStoredPosition(CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && lastCritterCapturedAnimController != null)
			{
				lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop);
			}
			lastCritterCapturedAnimController = null;
		}

		public void CreateWorkableChore()
		{
			if (chore == null)
			{
				chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, workable);
			}
		}

		public void CancelWorkChore()
		{
			if (chore != null)
			{
				chore.Cancel("GroundTrap.CancelChore");
				chore = null;
			}
		}

		public void Release()
		{
			if (!HasCritter)
			{
				return;
			}
			WasLastCritterLarge = IsCapturingLargeCritter;
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
			List<GameObject> list = new List<GameObject>();
			storage.DropAll(vent_gas: false, dump_liquid: false, default(Vector3), do_disease_transfer: true, list);
			foreach (GameObject item in list)
			{
				item.transform.SetPosition(position);
				KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.SetSceneLayer(Grid.SceneLayer.Creatures);
				}
			}
		}

		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}
	}

	public const string CAPTURE_ANIMATION_NAME = "capture";

	public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";

	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";

	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";

	public const string UNARMED_ANIMATION_NAME = "unarmed";

	public const string ARMED_ANIMATION_NAME = "armed";

	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	public BoolParameter IsArmed;

	public NonOperationalStates noOperational;

	public OperationalStates operational;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = operational;
		noOperational.TagTransition(GameTags.Operational, operational).Enter(RefreshLogicOutput).DefaultState(noOperational.idle);
		noOperational.idle.EnterTransition(noOperational.releasing, StorageContainsCritter).ParamTransition(IsArmed, noOperational.disarming, GameStateMachine<ReusableTrap, Instance, IStateMachineTarget, Def>.IsTrue).PlayAnim("off");
		noOperational.releasing.Enter(MarkAsUnarmed).Enter(Release).PlayAnim(GetReleaseAnimationName)
			.OnAnimQueueComplete(noOperational.idle);
		noOperational.disarming.Enter(MarkAsUnarmed).PlayAnim("abort_armed").OnAnimQueueComplete(noOperational.idle);
		operational.Enter(RefreshLogicOutput).TagTransition(GameTags.Operational, noOperational, on_remove: true).DefaultState(operational.unarmed);
		operational.unarmed.ParamTransition(IsArmed, operational.armed, GameStateMachine<ReusableTrap, Instance, IStateMachineTarget, Def>.IsTrue).EnterTransition(operational.capture.idle, StorageContainsCritter).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming)
			.PlayAnim("unarmed")
			.Enter(DisableTrapTrigger)
			.Enter(StartArmTrapWorkChore)
			.Exit(CancelArmTrapWorkChore)
			.WorkableCompleteTransition(GetWorkable, operational.armed);
		operational.armed.Enter(MarkAsArmed).EnterTransition(operational.capture.idle, StorageContainsCritter).PlayAnim("armed", KAnim.PlayMode.Loop)
			.ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed)
			.Toggle("Enable/Disable Trap Trigger", EnableTrapTrigger, DisableTrapTrigger)
			.Toggle("Enable/Disable Lure", ActivateLure, DisableLure)
			.EventHandlerTransition(GameHashes.TrapTriggered, operational.capture.capturing, HasCritter_OnTrapTriggered);
		operational.capture.Enter(RefreshLogicOutput).Enter(DisableTrapTrigger).Enter(MarkAsUnarmed)
			.ToggleTag(GameTags.Trapped)
			.DefaultState(operational.capture.capturing)
			.EventHandlerTransition(GameHashes.OnStorageChange, operational.capture.release, OnStorageEmptied);
		operational.capture.capturing.Enter(SetupCapturingAnimations).Update(OptionalCapturingAnimationUpdate, UpdateRate.RENDER_EVERY_TICK).PlayAnim(GetCaptureAnimationName)
			.OnAnimQueueComplete(operational.capture.idle)
			.Exit(UnsetCapturingAnimations);
		operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (Instance smi) => smi.CapturedCritter).PlayAnim(GetIdleAnimationName);
		operational.capture.release.Enter(RefreshLogicOutput).QueueAnim(GetReleaseAnimationName).OnAnimQueueComplete(operational.unarmed);
	}

	public static void RefreshLogicOutput(Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	public static void Release(Instance smi)
	{
		smi.Release();
	}

	public static void StartArmTrapWorkChore(Instance smi)
	{
		smi.CreateWorkableChore();
	}

	public static void CancelArmTrapWorkChore(Instance smi)
	{
		smi.CancelWorkChore();
	}

	public static string GetIdleAnimationName(Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture_idle";
		}
		return "capture_idle_large";
	}

	public static string GetCaptureAnimationName(Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture";
		}
		return "capture_large";
	}

	public static string GetReleaseAnimationName(Instance smi)
	{
		if (!smi.WasLastCritterLarge)
		{
			return "release";
		}
		return "release_large";
	}

	public static bool OnStorageEmptied(Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	public static bool HasCritter_OnTrapTriggered(Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	public static bool StorageContainsCritter(Instance smi)
	{
		return smi.HasCritter;
	}

	public static bool StorageIsEmpty(Instance smi)
	{
		return !smi.HasCritter;
	}

	public static void EnableTrapTrigger(Instance smi)
	{
		smi.SetTrapTriggerActiveState(active: true);
	}

	public static void DisableTrapTrigger(Instance smi)
	{
		smi.SetTrapTriggerActiveState(active: false);
	}

	public static ArmTrapWorkable GetWorkable(Instance smi)
	{
		return smi.GetWorkable();
	}

	public static void ActivateLure(Instance smi)
	{
		smi.SetLureActiveState(activate: true);
	}

	public static void DisableLure(Instance smi)
	{
		smi.SetLureActiveState(activate: false);
	}

	public static void SetupCapturingAnimations(Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	public static void UnsetCapturingAnimations(Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	public static void OptionalCapturingAnimationUpdate(Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME);
			}
			bool symbolVisible;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out symbolVisible).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	public static void MarkAsArmed(Instance smi)
	{
		smi.sm.IsArmed.Set(value: true, smi);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	public static void MarkAsUnarmed(Instance smi)
	{
		smi.sm.IsArmed.Set(value: false, smi);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}
}
