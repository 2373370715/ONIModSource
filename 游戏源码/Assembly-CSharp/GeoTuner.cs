using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

public class GeoTuner : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>
{
	public class Def : BaseDef
	{
		public string OUTPUT_LOGIC_PORT_ID;

		public Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunedGeyserSettings;

		public GeoTunerConfig.GeotunedGeyserSettings defaultSetting;

		public GeoTunerConfig.GeotunedGeyserSettings GetSettingsForGeyser(Geyser geyser)
		{
			if (!geotunedGeyserSettings.TryGetValue(geyser.configuration.typeId, out var value))
			{
				DebugUtil.DevLogError($"Geyser {geyser.configuration.typeId} is missing a Geotuner setting, using default");
				return defaultSetting;
			}
			return value;
		}
	}

	public class BroadcastingState : State
	{
		public State active;

		public State onHold;

		public State expired;
	}

	public class ResearchProgress : State
	{
		public State waitingForDupe;

		public State inProgress;
	}

	public class ResearchState : State
	{
		public State blocked;

		public ResearchProgress available;

		public State completed;
	}

	public class SwitchingGeyser : State
	{
		public State down;
	}

	public class GeyserSelectedState : State
	{
		public State idle;

		public SwitchingGeyser switchingGeyser;

		public State resourceNeeded;

		public ResearchState researcherInteractionNeeded;

		public BroadcastingState broadcasting;
	}

	public class SimpleIdleState : State
	{
		public State idle;
	}

	public class NonOperationalState : State
	{
		public State off;

		public State switchingGeyser;

		public State down;
	}

	public class OperationalState : State
	{
		public State idle;

		public SimpleIdleState noGeyserSelected;

		public GeyserSelectedState geyserSelected;
	}

	public enum GeyserAnimTypeSymbols
	{
		meter_gas,
		meter_metal,
		meter_liquid,
		meter_board
	}

	public new class Instance : GameInstance
	{
		[MyCmpReq]
		public Operational operational;

		[MyCmpReq]
		public Storage storage;

		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		[MyCmpReq]
		public GeoTunerWorkable workable;

		[MyCmpReq]
		public GeoTunerSwitchGeyserWorkable switchGeyserWorkable;

		[MyCmpReq]
		public LogicPorts logicPorts;

		[MyCmpReq]
		public RoomTracker roomTracker;

		[MyCmpReq]
		public KBatchedAnimController animController;

		public MeterController switchGeyserMeter;

		public string originID;

		public float enhancementDuration;

		public Geyser.GeyserModification currentGeyserModification;

		private Chore switchGeyserChore;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			originID = UI.StripLinkFormatting("GeoTuner") + " [" + base.gameObject.GetInstanceID() + "]";
			switchGeyserMeter = new MeterController(animController, "geyser_target", GetAnimationSymbol().ToString(), Meter.Offset.Behind, Grid.SceneLayer.NoLayer);
		}

		public override void StartSM()
		{
			base.StartSM();
			Components.GeoTuners.Add(base.smi.GetMyWorldId(), this);
			Geyser assignedGeyser = GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				assignedGeyser.Subscribe(-593169791, OnEruptionStateChanged);
				RefreshModification();
			}
			RefreshLogicOutput();
			AssignFutureGeyser(GetFutureGeyser());
			base.gameObject.Subscribe(-905833192, OnCopySettings);
		}

		public Geyser GetFutureGeyser()
		{
			if (base.smi.sm.FutureGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.FutureGeyser.Get(this).GetComponent<Geyser>();
		}

		public Geyser GetAssignedGeyser()
		{
			if (base.smi.sm.AssignedGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.AssignedGeyser.Get(this).GetComponent<Geyser>();
		}

		public void AssignFutureGeyser(Geyser newFutureGeyser)
		{
			bool num = newFutureGeyser != GetFutureGeyser();
			bool flag = GetAssignedGeyser() != newFutureGeyser;
			base.sm.FutureGeyser.Set(newFutureGeyser, this);
			if (num)
			{
				if (flag)
				{
					RecreateSwitchGeyserChore();
				}
				else if (switchGeyserChore != null)
				{
					AbortSwitchGeyserChore("Future Geyser was set to current Geyser");
				}
			}
			else if (switchGeyserChore == null && flag)
			{
				RecreateSwitchGeyserChore();
			}
		}

		private void AbortSwitchGeyserChore(string reason = "Aborting Switch Geyser Chore")
		{
			if (switchGeyserChore != null)
			{
				Chore chore = switchGeyserChore;
				chore.onComplete = (Action<Chore>)Delegate.Remove(chore.onComplete, new Action<Chore>(OnSwitchGeyserChoreCompleted));
				switchGeyserChore.Cancel(reason);
				switchGeyserChore = null;
			}
			switchGeyserChore = null;
		}

		private Chore RecreateSwitchGeyserChore()
		{
			AbortSwitchGeyserChore("Recreating Chore");
			switchGeyserChore = new WorkChore<GeoTunerSwitchGeyserWorkable>(Db.Get().ChoreTypes.Toggle, switchGeyserWorkable, null, run_until_complete: true, null, ShowSwitchingGeyserStatusItem, HideSwitchingGeyserStatusItem, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
			Chore chore = switchGeyserChore;
			chore.onComplete = (Action<Chore>)Delegate.Combine(chore.onComplete, new Action<Chore>(OnSwitchGeyserChoreCompleted));
			return switchGeyserChore;
		}

		private void ShowSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle);
		}

		private void HideSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle);
		}

		private void OnSwitchGeyserChoreCompleted(Chore chore)
		{
			GetCurrentState();
			_ = base.sm.nonOperational;
			Geyser futureGeyser = GetFutureGeyser();
			bool flag = GetAssignedGeyser() != futureGeyser;
			if (chore.isComplete && flag)
			{
				AssignGeyser(futureGeyser);
			}
			Trigger(1980521255);
		}

		public void AssignGeyser(Geyser geyser)
		{
			Geyser assignedGeyser = GetAssignedGeyser();
			if (assignedGeyser != null && assignedGeyser != geyser)
			{
				RemoveTuning(base.smi);
				assignedGeyser.Unsubscribe(-593169791, base.smi.OnEruptionStateChanged);
			}
			Geyser geyser2 = assignedGeyser;
			base.sm.AssignedGeyser.Set(geyser, this);
			RefreshModification();
			if (geyser2 != geyser)
			{
				if (geyser != null)
				{
					geyser.Subscribe(-593169791, OnEruptionStateChanged);
					geyser.Trigger(1763323737);
				}
				if (geyser2 != null)
				{
					geyser2.Trigger(1763323737);
				}
				base.sm.geyserSwitchSignal.Trigger(this);
			}
		}

		private void RefreshModification()
		{
			Geyser assignedGeyser = GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = base.def.GetSettingsForGeyser(assignedGeyser);
				currentGeyserModification = settingsForGeyser.template;
				currentGeyserModification.originID = originID;
				enhancementDuration = settingsForGeyser.duration;
				assignedGeyser.Trigger(1763323737);
			}
			RefreshStorageRequirements(this);
			DropStorageIfNotMatching(this);
		}

		public void RefreshGeyserSymbol()
		{
			switchGeyserMeter.meterController.Play(GetAnimationSymbol().ToString());
		}

		private GeyserAnimTypeSymbols GetAnimationSymbol()
		{
			GeyserAnimTypeSymbols result = GeyserAnimTypeSymbols.meter_board;
			Geyser assignedGeyser = base.smi.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				switch (assignedGeyser.configuration.geyserType.shape)
				{
				case GeyserConfigurator.GeyserShape.Liquid:
					result = GeyserAnimTypeSymbols.meter_liquid;
					break;
				case GeyserConfigurator.GeyserShape.Gas:
					result = GeyserAnimTypeSymbols.meter_gas;
					break;
				case GeyserConfigurator.GeyserShape.Molten:
					result = GeyserAnimTypeSymbols.meter_metal;
					break;
				}
			}
			return result;
		}

		public void OnEruptionStateChanged(object data)
		{
			_ = (bool)data;
			RefreshLogicOutput();
		}

		public void RefreshLogicOutput()
		{
			Geyser assignedGeyser = GetAssignedGeyser();
			bool num = GetCurrentState() != base.smi.sm.nonOperational;
			bool flag = assignedGeyser != null && GetCurrentState() != base.smi.sm.operational.noGeyserSelected;
			bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && (assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.erupt || assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt);
			bool flag3 = num && flag && flag2;
			logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag3 ? 1 : 0);
			switchGeyserMeter.meterController.SetSymbolVisiblity("light_bloom", flag3);
		}

		public void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (!(gameObject != null))
			{
				return;
			}
			Instance sMI = gameObject.GetSMI<Instance>();
			if (sMI != null && sMI.GetFutureGeyser() != GetFutureGeyser())
			{
				Geyser futureGeyser = sMI.GetFutureGeyser();
				if (futureGeyser != null && futureGeyser.GetAmountOfGeotunersPointingOrWillPointAtThisGeyser() < 5)
				{
					AssignFutureGeyser(sMI.GetFutureGeyser());
				}
			}
		}

		protected override void OnCleanUp()
		{
			Geyser assignedGeyser = GetAssignedGeyser();
			Components.GeoTuners.Remove(base.smi.GetMyWorldId(), this);
			if (assignedGeyser != null)
			{
				assignedGeyser.Unsubscribe(-593169791, base.smi.OnEruptionStateChanged);
			}
			RemoveTuning(this);
		}
	}

	private Signal geyserSwitchSignal;

	private NonOperationalState nonOperational;

	private OperationalState operational;

	private TargetParameter FutureGeyser;

	private TargetParameter AssignedGeyser;

	public BoolParameter hasBeenWorkedByResearcher;

	public FloatParameter expirationTimer;

	public static string liquidGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Geyser");

	public static string gasGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Vent");

	public static string metalGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Volcano");

	public const string anim_switchGeyser_down = "geyser_down";

	public const string anim_switchGeyser_up = "geyser_up";

	private const string BroadcastingOnHoldAnimationName = "on";

	private const string OnAnimName = "on";

	private const string OffAnimName = "off";

	private const string BroadcastingAnimationName = "broadcasting";

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = operational;
		base.serializable = SerializeType.ParamsOnly;
		root.Enter(RefreshAnimationGeyserSymbolType);
		nonOperational.DefaultState(nonOperational.off).OnSignal(geyserSwitchSignal, nonOperational.switchingGeyser).Enter(delegate(Instance smi)
		{
			smi.RefreshLogicOutput();
		})
			.TagTransition(GameTags.Operational, operational);
		nonOperational.off.PlayAnim("off");
		nonOperational.switchingGeyser.QueueAnim("geyser_down").OnAnimQueueComplete(nonOperational.down);
		nonOperational.down.PlayAnim("geyser_up").QueueAnim("off").Enter(RefreshAnimationGeyserSymbolType)
			.Enter(TriggerSoundsForGeyserChange);
		operational.PlayAnim("on").Enter(delegate(Instance smi)
		{
			smi.RefreshLogicOutput();
		}).DefaultState(operational.idle)
			.TagTransition(GameTags.Operational, nonOperational, on_remove: true);
		operational.idle.ParamTransition(AssignedGeyser, operational.geyserSelected, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsNotNull).ParamTransition(AssignedGeyser, operational.noGeyserSelected, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsNull);
		operational.noGeyserSelected.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerNoGeyserSelected).ParamTransition(AssignedGeyser, operational.geyserSelected.switchingGeyser, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsNotNull).Enter(delegate(Instance smi)
		{
			smi.RefreshLogicOutput();
		})
			.Enter(DropStorage)
			.Enter(RefreshStorageRequirements)
			.Exit(ForgetWorkDoneByDupe)
			.Exit(ResetExpirationTimer)
			.QueueAnim("geyser_down")
			.OnAnimQueueComplete(operational.noGeyserSelected.idle);
		operational.noGeyserSelected.idle.PlayAnim("geyser_up").QueueAnim("on").Enter(RefreshAnimationGeyserSymbolType)
			.Enter(TriggerSoundsForGeyserChange);
		operational.geyserSelected.DefaultState(operational.geyserSelected.idle).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoTunerGeyserStatus).ParamTransition(AssignedGeyser, operational.noGeyserSelected, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsNull)
			.OnSignal(geyserSwitchSignal, operational.geyserSelected.switchingGeyser)
			.Enter(delegate(Instance smi)
			{
				smi.RefreshLogicOutput();
			});
		operational.geyserSelected.idle.ParamTransition(hasBeenWorkedByResearcher, operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsTrue).ParamTransition(hasBeenWorkedByResearcher, operational.geyserSelected.researcherInteractionNeeded, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsFalse);
		operational.geyserSelected.switchingGeyser.Enter(DropStorageIfNotMatching).Enter(ForgetWorkDoneByDupe).Enter(ResetExpirationTimer)
			.Enter(RefreshStorageRequirements)
			.Enter(delegate(Instance smi)
			{
				smi.RefreshLogicOutput();
			})
			.QueueAnim("geyser_down")
			.OnAnimQueueComplete(operational.geyserSelected.switchingGeyser.down);
		operational.geyserSelected.switchingGeyser.down.QueueAnim("geyser_up").QueueAnim("on").Enter(RefreshAnimationGeyserSymbolType)
			.Enter(TriggerSoundsForGeyserChange)
			.ScheduleActionNextFrame("Switch Animation Completed", delegate(Instance smi)
			{
				smi.GoTo(operational.geyserSelected.idle);
			});
		operational.geyserSelected.researcherInteractionNeeded.EventTransition(GameHashes.UpdateRoom, operational.geyserSelected.researcherInteractionNeeded.blocked, (Instance smi) => !WorkRequirementsMet(smi)).EventTransition(GameHashes.UpdateRoom, operational.geyserSelected.researcherInteractionNeeded.available, WorkRequirementsMet).EventTransition(GameHashes.OnStorageChange, operational.geyserSelected.researcherInteractionNeeded.blocked, (Instance smi) => !WorkRequirementsMet(smi))
			.EventTransition(GameHashes.OnStorageChange, operational.geyserSelected.researcherInteractionNeeded.available, WorkRequirementsMet)
			.ParamTransition(hasBeenWorkedByResearcher, operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsTrue)
			.Exit(ResetExpirationTimer);
		operational.geyserSelected.researcherInteractionNeeded.blocked.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded).DoNothing();
		operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(CreateResearchChore).WorkableCompleteTransition((Instance smi) => smi.workable, operational.geyserSelected.researcherInteractionNeeded.completed);
		operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded).WorkableStartTransition((Instance smi) => smi.workable, operational.geyserSelected.researcherInteractionNeeded.available.inProgress);
		operational.geyserSelected.researcherInteractionNeeded.available.inProgress.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchInProgress).WorkableStopTransition((Instance smi) => smi.workable, operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe);
		operational.geyserSelected.researcherInteractionNeeded.completed.Enter(OnResearchCompleted);
		operational.geyserSelected.broadcasting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerBroadcasting).Toggle("Tuning", ApplyTuning, RemoveTuning);
		operational.geyserSelected.broadcasting.onHold.PlayAnim("on").UpdateTransition(operational.geyserSelected.broadcasting.active, (Instance smi, float dt) => !GeyserExitEruptionTransition(smi, dt));
		operational.geyserSelected.broadcasting.active.Toggle("EnergyConsumption", delegate(Instance smi)
		{
			smi.operational.SetActive(value: true);
		}, delegate(Instance smi)
		{
			smi.operational.SetActive(value: false);
		}).Toggle("BroadcastingAnimations", PlayBroadcastingAnimation, StopPlayingBroadcastingAnimation).Update(ExpirationTimerUpdate)
			.UpdateTransition(operational.geyserSelected.broadcasting.onHold, GeyserExitEruptionTransition)
			.ParamTransition(expirationTimer, operational.geyserSelected.broadcasting.expired, GameStateMachine<GeoTuner, Instance, IStateMachineTarget, Def>.IsLTEZero);
		operational.geyserSelected.broadcasting.expired.Enter(ForgetWorkDoneByDupe).Enter(ResetExpirationTimer).ScheduleActionNextFrame("Expired", delegate(Instance smi)
		{
			smi.GoTo(operational.geyserSelected.researcherInteractionNeeded);
		});
	}

	private static void TriggerSoundsForGeyserChange(Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			EventInstance instance = default(EventInstance);
			switch (assignedGeyser.configuration.geyserType.shape)
			{
			case GeyserConfigurator.GeyserShape.Liquid:
				instance = SoundEvent.BeginOneShot(liquidGeyserTuningSoundPath, smi.transform.GetPosition());
				break;
			case GeyserConfigurator.GeyserShape.Gas:
				instance = SoundEvent.BeginOneShot(gasGeyserTuningSoundPath, smi.transform.GetPosition());
				break;
			case GeyserConfigurator.GeyserShape.Molten:
				instance = SoundEvent.BeginOneShot(metalGeyserTuningSoundPath, smi.transform.GetPosition());
				break;
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	private static void RefreshStorageRequirements(Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser == null)
		{
			smi.storage.capacityKg = 0f;
			smi.storage.storageFilters = null;
			smi.manualDelivery.capacity = 0f;
			smi.manualDelivery.refillMass = 0f;
			smi.manualDelivery.RequestedItemTag = null;
			smi.manualDelivery.AbortDelivery("No geyser is selected for tuning");
		}
		else
		{
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
			smi.storage.capacityKg = settingsForGeyser.quantity;
			smi.storage.storageFilters = new List<Tag> { settingsForGeyser.material };
			smi.manualDelivery.AbortDelivery("Switching to new delivery request");
			smi.manualDelivery.capacity = settingsForGeyser.quantity;
			smi.manualDelivery.refillMass = settingsForGeyser.quantity;
			smi.manualDelivery.MinimumMass = settingsForGeyser.quantity;
			smi.manualDelivery.RequestedItemTag = settingsForGeyser.material;
		}
	}

	private static void DropStorage(Instance smi)
	{
		smi.storage.DropAll();
	}

	private static void DropStorageIfNotMatching(Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
			List<GameObject> items = smi.storage.GetItems();
			if (smi.storage.GetItems() == null || items.Count <= 0)
			{
				return;
			}
			Tag tag = items[0].PrefabID();
			PrimaryElement component = items[0].GetComponent<PrimaryElement>();
			if (tag != settingsForGeyser.material)
			{
				smi.storage.DropAll();
				return;
			}
			float num = component.Mass - settingsForGeyser.quantity;
			if (num > 0f)
			{
				smi.storage.DropSome(tag, num);
			}
		}
		else
		{
			smi.storage.DropAll();
		}
	}

	private static bool GeyserExitEruptionTransition(Instance smi, float dt)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null)
		{
			return assignedGeyser.smi.GetCurrentState().parent != assignedGeyser.smi.sm.erupt;
		}
		return false;
	}

	public static void OnResearchCompleted(Instance smi)
	{
		smi.storage.ConsumeAllIgnoringDisease();
		smi.sm.hasBeenWorkedByResearcher.Set(value: true, smi);
	}

	public static void PlayBroadcastingAnimation(Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Loop);
	}

	public static void StopPlayingBroadcastingAnimation(Instance smi)
	{
		smi.animController.Play("broadcasting");
	}

	public static void RefreshAnimationGeyserSymbolType(Instance smi)
	{
		smi.RefreshGeyserSymbol();
	}

	public static float GetRemainingExpiraionTime(Instance smi)
	{
		return smi.sm.expirationTimer.Get(smi);
	}

	private static void ExpirationTimerUpdate(Instance smi, float dt)
	{
		float remainingExpiraionTime = GetRemainingExpiraionTime(smi);
		remainingExpiraionTime -= dt;
		smi.sm.expirationTimer.Set(remainingExpiraionTime, smi);
	}

	private static void ResetExpirationTimer(Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			smi.sm.expirationTimer.Set(smi.def.GetSettingsForGeyser(assignedGeyser).duration, smi);
		}
		else
		{
			smi.sm.expirationTimer.Set(0f, smi);
		}
	}

	private static void ForgetWorkDoneByDupe(Instance smi)
	{
		smi.sm.hasBeenWorkedByResearcher.Set(value: false, smi);
		smi.workable.WorkTimeRemaining = smi.workable.GetWorkTime();
	}

	private Chore CreateResearchChore(Instance smi)
	{
		return new WorkChore<GeoTunerWorkable>(Db.Get().ChoreTypes.Research, smi.workable);
	}

	private static void ApplyTuning(Instance smi)
	{
		smi.GetAssignedGeyser().AddModification(smi.currentGeyserModification);
	}

	private static void RemoveTuning(Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			assignedGeyser.RemoveModification(smi.currentGeyserModification);
		}
	}

	public static bool WorkRequirementsMet(Instance smi)
	{
		if (IsInLabRoom(smi))
		{
			return smi.storage.MassStored() == smi.storage.capacityKg;
		}
		return false;
	}

	public static bool IsInLabRoom(Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}
}
