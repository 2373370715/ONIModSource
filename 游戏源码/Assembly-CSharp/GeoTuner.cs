using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000D98 RID: 3480
public class GeoTuner : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>
{
	// Token: 0x06004449 RID: 17481 RVA: 0x00247598 File Offset: 0x00245798
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType));
		this.nonOperational.DefaultState(this.nonOperational.off).OnSignal(this.geyserSwitchSignal, this.nonOperational.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.nonOperational.off.PlayAnim("off");
		this.nonOperational.switchingGeyser.QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.nonOperational.down);
		this.nonOperational.down.PlayAnim("geyser_up").QueueAnim("off", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.PlayAnim("on").Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).DefaultState(this.operational.idle).TagTransition(GameTags.Operational, this.nonOperational, true);
		this.operational.idle.ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull);
		this.operational.noGeyserSelected.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerNoGeyserSelected, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected.switchingGeyser, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorage)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.noGeyserSelected.idle);
		this.operational.noGeyserSelected.idle.PlayAnim("geyser_up").QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.geyserSelected.DefaultState(this.operational.geyserSelected.idle).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoTunerGeyserStatus, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull).OnSignal(this.geyserSwitchSignal, this.operational.geyserSelected.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		});
		this.operational.geyserSelected.idle.ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.researcherInteractionNeeded, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsFalse);
		this.operational.geyserSelected.switchingGeyser.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorageIfNotMatching)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.geyserSelected.switchingGeyser.down);
		this.operational.geyserSelected.switchingGeyser.down.QueueAnim("geyser_up", false, null).QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange)).ScheduleActionNextFrame("Switch Animation Completed", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.idle);
		});
		this.operational.geyserSelected.researcherInteractionNeeded.EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer));
		this.operational.geyserSelected.researcherInteractionNeeded.blocked.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).DoNothing();
		this.operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(new Func<GeoTuner.Instance, Chore>(this.CreateResearchChore), null).WorkableCompleteTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.completed);
		this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).WorkableStartTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress);
		this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchInProgress, null).WorkableStopTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe);
		this.operational.geyserSelected.researcherInteractionNeeded.completed.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.OnResearchCompleted));
		this.operational.geyserSelected.broadcasting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerBroadcasting, null).Toggle("Tuning", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ApplyTuning), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RemoveTuning));
		this.operational.geyserSelected.broadcasting.onHold.PlayAnim("on").UpdateTransition(this.operational.geyserSelected.broadcasting.active, (GeoTuner.Instance smi, float dt) => !GeoTuner.GeyserExitEruptionTransition(smi, dt), UpdateRate.SIM_200ms, false);
		this.operational.geyserSelected.broadcasting.active.Toggle("EnergyConsumption", delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}, delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).Toggle("BroadcastingAnimations", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.PlayBroadcastingAnimation), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.StopPlayingBroadcastingAnimation)).Update(new Action<GeoTuner.Instance, float>(GeoTuner.ExpirationTimerUpdate), UpdateRate.SIM_200ms, false).UpdateTransition(this.operational.geyserSelected.broadcasting.onHold, new Func<GeoTuner.Instance, float, bool>(GeoTuner.GeyserExitEruptionTransition), UpdateRate.SIM_200ms, false).ParamTransition<float>(this.expirationTimer, this.operational.geyserSelected.broadcasting.expired, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsLTEZero);
		this.operational.geyserSelected.broadcasting.expired.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).ScheduleActionNextFrame("Expired", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.researcherInteractionNeeded);
		});
	}

	// Token: 0x0600444A RID: 17482 RVA: 0x00247EE4 File Offset: 0x002460E4
	private static void TriggerSoundsForGeyserChange(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			EventInstance instance = default(EventInstance);
			switch (assignedGeyser.configuration.geyserType.shape)
			{
			case GeyserConfigurator.GeyserShape.Gas:
				instance = SoundEvent.BeginOneShot(GeoTuner.gasGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Liquid:
				instance = SoundEvent.BeginOneShot(GeoTuner.liquidGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Molten:
				instance = SoundEvent.BeginOneShot(GeoTuner.metalGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x00247F90 File Offset: 0x00246190
	private static void RefreshStorageRequirements(GeoTuner.Instance smi)
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
			return;
		}
		GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
		smi.storage.capacityKg = settingsForGeyser.quantity;
		smi.storage.storageFilters = new List<Tag>
		{
			settingsForGeyser.material
		};
		smi.manualDelivery.AbortDelivery("Switching to new delivery request");
		smi.manualDelivery.capacity = settingsForGeyser.quantity;
		smi.manualDelivery.refillMass = settingsForGeyser.quantity;
		smi.manualDelivery.MinimumMass = settingsForGeyser.quantity;
		smi.manualDelivery.RequestedItemTag = settingsForGeyser.material;
	}

	// Token: 0x0600444C RID: 17484 RVA: 0x0024809C File Offset: 0x0024629C
	private static void DropStorage(GeoTuner.Instance smi)
	{
		smi.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x0600444D RID: 17485 RVA: 0x002480C4 File Offset: 0x002462C4
	private static void DropStorageIfNotMatching(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
			List<GameObject> items = smi.storage.GetItems();
			if (smi.storage.GetItems() != null && items.Count > 0)
			{
				Tag tag = items[0].PrefabID();
				PrimaryElement component = items[0].GetComponent<PrimaryElement>();
				if (tag != settingsForGeyser.material)
				{
					smi.storage.DropAll(false, false, default(Vector3), true, null);
					return;
				}
				float num = component.Mass - settingsForGeyser.quantity;
				if (num > 0f)
				{
					smi.storage.DropSome(tag, num, false, false, default(Vector3), true, false);
					return;
				}
			}
		}
		else
		{
			smi.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x0600444E RID: 17486 RVA: 0x002481AC File Offset: 0x002463AC
	private static bool GeyserExitEruptionTransition(GeoTuner.Instance smi, float dt)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		return assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent != assignedGeyser.smi.sm.erupt;
	}

	// Token: 0x0600444F RID: 17487 RVA: 0x000CC324 File Offset: 0x000CA524
	public static void OnResearchCompleted(GeoTuner.Instance smi)
	{
		smi.storage.ConsumeAllIgnoringDisease();
		smi.sm.hasBeenWorkedByResearcher.Set(true, smi, false);
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x000CC345 File Offset: 0x000CA545
	public static void PlayBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x000CC367 File Offset: 0x000CA567
	public static void StopPlayingBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004452 RID: 17490 RVA: 0x000CC389 File Offset: 0x000CA589
	public static void RefreshAnimationGeyserSymbolType(GeoTuner.Instance smi)
	{
		smi.RefreshGeyserSymbol();
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x000CC391 File Offset: 0x000CA591
	public static float GetRemainingExpiraionTime(GeoTuner.Instance smi)
	{
		return smi.sm.expirationTimer.Get(smi);
	}

	// Token: 0x06004454 RID: 17492 RVA: 0x00248200 File Offset: 0x00246400
	private static void ExpirationTimerUpdate(GeoTuner.Instance smi, float dt)
	{
		float num = GeoTuner.GetRemainingExpiraionTime(smi);
		num -= dt;
		smi.sm.expirationTimer.Set(num, smi, false);
	}

	// Token: 0x06004455 RID: 17493 RVA: 0x0024822C File Offset: 0x0024642C
	private static void ResetExpirationTimer(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			smi.sm.expirationTimer.Set(smi.def.GetSettingsForGeyser(assignedGeyser).duration, smi, false);
			return;
		}
		smi.sm.expirationTimer.Set(0f, smi, false);
	}

	// Token: 0x06004456 RID: 17494 RVA: 0x000CC3A4 File Offset: 0x000CA5A4
	private static void ForgetWorkDoneByDupe(GeoTuner.Instance smi)
	{
		smi.sm.hasBeenWorkedByResearcher.Set(false, smi, false);
		smi.workable.WorkTimeRemaining = smi.workable.GetWorkTime();
	}

	// Token: 0x06004457 RID: 17495 RVA: 0x00248288 File Offset: 0x00246488
	private Chore CreateResearchChore(GeoTuner.Instance smi)
	{
		return new WorkChore<GeoTunerWorkable>(Db.Get().ChoreTypes.Research, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06004458 RID: 17496 RVA: 0x000CC3D0 File Offset: 0x000CA5D0
	private static void ApplyTuning(GeoTuner.Instance smi)
	{
		smi.GetAssignedGeyser().AddModification(smi.currentGeyserModification);
	}

	// Token: 0x06004459 RID: 17497 RVA: 0x002482C0 File Offset: 0x002464C0
	private static void RemoveTuning(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			assignedGeyser.RemoveModification(smi.currentGeyserModification);
		}
	}

	// Token: 0x0600445A RID: 17498 RVA: 0x000CC3E3 File Offset: 0x000CA5E3
	public static bool WorkRequirementsMet(GeoTuner.Instance smi)
	{
		return GeoTuner.IsInLabRoom(smi) && smi.storage.MassStored() == smi.storage.capacityKg;
	}

	// Token: 0x0600445B RID: 17499 RVA: 0x000CC407 File Offset: 0x000CA607
	public static bool IsInLabRoom(GeoTuner.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04002EDB RID: 11995
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Signal geyserSwitchSignal;

	// Token: 0x04002EDC RID: 11996
	private GeoTuner.NonOperationalState nonOperational;

	// Token: 0x04002EDD RID: 11997
	private GeoTuner.OperationalState operational;

	// Token: 0x04002EDE RID: 11998
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter FutureGeyser;

	// Token: 0x04002EDF RID: 11999
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter AssignedGeyser;

	// Token: 0x04002EE0 RID: 12000
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.BoolParameter hasBeenWorkedByResearcher;

	// Token: 0x04002EE1 RID: 12001
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.FloatParameter expirationTimer;

	// Token: 0x04002EE2 RID: 12002
	public static string liquidGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Geyser", false);

	// Token: 0x04002EE3 RID: 12003
	public static string gasGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Vent", false);

	// Token: 0x04002EE4 RID: 12004
	public static string metalGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Volcano", false);

	// Token: 0x04002EE5 RID: 12005
	public const string anim_switchGeyser_down = "geyser_down";

	// Token: 0x04002EE6 RID: 12006
	public const string anim_switchGeyser_up = "geyser_up";

	// Token: 0x04002EE7 RID: 12007
	private const string BroadcastingOnHoldAnimationName = "on";

	// Token: 0x04002EE8 RID: 12008
	private const string OnAnimName = "on";

	// Token: 0x04002EE9 RID: 12009
	private const string OffAnimName = "off";

	// Token: 0x04002EEA RID: 12010
	private const string BroadcastingAnimationName = "broadcasting";

	// Token: 0x02000D99 RID: 3481
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06004460 RID: 17504 RVA: 0x002482EC File Offset: 0x002464EC
		public GeoTunerConfig.GeotunedGeyserSettings GetSettingsForGeyser(Geyser geyser)
		{
			GeoTunerConfig.GeotunedGeyserSettings result;
			if (!this.geotunedGeyserSettings.TryGetValue(geyser.configuration.typeId, out result))
			{
				DebugUtil.DevLogError(string.Format("Geyser {0} is missing a Geotuner setting, using default", geyser.configuration.typeId));
				return this.defaultSetting;
			}
			return result;
		}

		// Token: 0x04002EEB RID: 12011
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04002EEC RID: 12012
		public Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunedGeyserSettings;

		// Token: 0x04002EED RID: 12013
		public GeoTunerConfig.GeotunedGeyserSettings defaultSetting;
	}

	// Token: 0x02000D9A RID: 3482
	public class BroadcastingState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EEE RID: 12014
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State active;

		// Token: 0x04002EEF RID: 12015
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State onHold;

		// Token: 0x04002EF0 RID: 12016
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State expired;
	}

	// Token: 0x02000D9B RID: 3483
	public class ResearchProgress : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EF1 RID: 12017
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State waitingForDupe;

		// Token: 0x04002EF2 RID: 12018
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State inProgress;
	}

	// Token: 0x02000D9C RID: 3484
	public class ResearchState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EF3 RID: 12019
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State blocked;

		// Token: 0x04002EF4 RID: 12020
		public GeoTuner.ResearchProgress available;

		// Token: 0x04002EF5 RID: 12021
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State completed;
	}

	// Token: 0x02000D9D RID: 3485
	public class SwitchingGeyser : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EF6 RID: 12022
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x02000D9E RID: 3486
	public class GeyserSelectedState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EF7 RID: 12023
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x04002EF8 RID: 12024
		public GeoTuner.SwitchingGeyser switchingGeyser;

		// Token: 0x04002EF9 RID: 12025
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State resourceNeeded;

		// Token: 0x04002EFA RID: 12026
		public GeoTuner.ResearchState researcherInteractionNeeded;

		// Token: 0x04002EFB RID: 12027
		public GeoTuner.BroadcastingState broadcasting;
	}

	// Token: 0x02000D9F RID: 3487
	public class SimpleIdleState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EFC RID: 12028
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;
	}

	// Token: 0x02000DA0 RID: 3488
	public class NonOperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002EFD RID: 12029
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State off;

		// Token: 0x04002EFE RID: 12030
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State switchingGeyser;

		// Token: 0x04002EFF RID: 12031
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x02000DA1 RID: 3489
	public class OperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002F00 RID: 12032
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x04002F01 RID: 12033
		public GeoTuner.SimpleIdleState noGeyserSelected;

		// Token: 0x04002F02 RID: 12034
		public GeoTuner.GeyserSelectedState geyserSelected;
	}

	// Token: 0x02000DA2 RID: 3490
	public enum GeyserAnimTypeSymbols
	{
		// Token: 0x04002F04 RID: 12036
		meter_gas,
		// Token: 0x04002F05 RID: 12037
		meter_metal,
		// Token: 0x04002F06 RID: 12038
		meter_liquid,
		// Token: 0x04002F07 RID: 12039
		meter_board
	}

	// Token: 0x02000DA3 RID: 3491
	public new class Instance : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.GameInstance
	{
		// Token: 0x0600446A RID: 17514 RVA: 0x0024833C File Offset: 0x0024653C
		public Instance(IStateMachineTarget master, GeoTuner.Def def) : base(master, def)
		{
			this.originID = UI.StripLinkFormatting("GeoTuner") + " [" + base.gameObject.GetInstanceID().ToString() + "]";
			this.switchGeyserMeter = new MeterController(this.animController, "geyser_target", this.GetAnimationSymbol().ToString(), Meter.Offset.Behind, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x0600446B RID: 17515 RVA: 0x002483B8 File Offset: 0x002465B8
		public override void StartSM()
		{
			base.StartSM();
			Components.GeoTuners.Add(base.smi.GetMyWorldId(), this);
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				assignedGeyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
				this.RefreshModification();
			}
			this.RefreshLogicOutput();
			this.AssignFutureGeyser(this.GetFutureGeyser());
			base.gameObject.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x000CC486 File Offset: 0x000CA686
		public Geyser GetFutureGeyser()
		{
			if (base.smi.sm.FutureGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.FutureGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x000CC4B8 File Offset: 0x000CA6B8
		public Geyser GetAssignedGeyser()
		{
			if (base.smi.sm.AssignedGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.AssignedGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x00248440 File Offset: 0x00246640
		public void AssignFutureGeyser(Geyser newFutureGeyser)
		{
			bool flag = newFutureGeyser != this.GetFutureGeyser();
			bool flag2 = this.GetAssignedGeyser() != newFutureGeyser;
			base.sm.FutureGeyser.Set(newFutureGeyser, this);
			if (flag)
			{
				if (flag2)
				{
					this.RecreateSwitchGeyserChore();
					return;
				}
				if (this.switchGeyserChore != null)
				{
					this.AbortSwitchGeyserChore("Future Geyser was set to current Geyser");
					return;
				}
			}
			else if (this.switchGeyserChore == null && flag2)
			{
				this.RecreateSwitchGeyserChore();
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x002484B0 File Offset: 0x002466B0
		private void AbortSwitchGeyserChore(string reason = "Aborting Switch Geyser Chore")
		{
			if (this.switchGeyserChore != null)
			{
				Chore chore = this.switchGeyserChore;
				chore.onComplete = (Action<Chore>)Delegate.Remove(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
				this.switchGeyserChore.Cancel(reason);
				this.switchGeyserChore = null;
			}
			this.switchGeyserChore = null;
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x00248508 File Offset: 0x00246708
		private Chore RecreateSwitchGeyserChore()
		{
			this.AbortSwitchGeyserChore("Recreating Chore");
			this.switchGeyserChore = new WorkChore<GeoTunerSwitchGeyserWorkable>(Db.Get().ChoreTypes.Toggle, this.switchGeyserWorkable, null, true, null, new Action<Chore>(this.ShowSwitchingGeyserStatusItem), new Action<Chore>(this.HideSwitchingGeyserStatusItem), true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			Chore chore = this.switchGeyserChore;
			chore.onComplete = (Action<Chore>)Delegate.Combine(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
			return this.switchGeyserChore;
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x000CC4EA File Offset: 0x000CA6EA
		private void ShowSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x000CC50D File Offset: 0x000CA70D
		private void HideSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x00248594 File Offset: 0x00246794
		private void OnSwitchGeyserChoreCompleted(Chore chore)
		{
			this.GetCurrentState();
			GeoTuner.NonOperationalState nonOperational = base.sm.nonOperational;
			Geyser futureGeyser = this.GetFutureGeyser();
			bool flag = this.GetAssignedGeyser() != futureGeyser;
			if (chore.isComplete && flag)
			{
				this.AssignGeyser(futureGeyser);
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x002485E8 File Offset: 0x002467E8
		public void AssignGeyser(Geyser geyser)
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null && assignedGeyser != geyser)
			{
				GeoTuner.RemoveTuning(base.smi);
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			Geyser geyser2 = assignedGeyser;
			base.sm.AssignedGeyser.Set(geyser, this);
			this.RefreshModification();
			if (geyser2 != geyser)
			{
				if (geyser != null)
				{
					geyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
					geyser.Trigger(1763323737, null);
				}
				if (geyser2 != null)
				{
					geyser2.Trigger(1763323737, null);
				}
				base.sm.geyserSwitchSignal.Trigger(this);
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x002486AC File Offset: 0x002468AC
		private void RefreshModification()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = base.def.GetSettingsForGeyser(assignedGeyser);
				this.currentGeyserModification = settingsForGeyser.template;
				this.currentGeyserModification.originID = this.originID;
				this.enhancementDuration = settingsForGeyser.duration;
				assignedGeyser.Trigger(1763323737, null);
			}
			GeoTuner.RefreshStorageRequirements(this);
			GeoTuner.DropStorageIfNotMatching(this);
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x00248718 File Offset: 0x00246918
		public void RefreshGeyserSymbol()
		{
			this.switchGeyserMeter.meterController.Play(this.GetAnimationSymbol().ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0024875C File Offset: 0x0024695C
		private GeoTuner.GeyserAnimTypeSymbols GetAnimationSymbol()
		{
			GeoTuner.GeyserAnimTypeSymbols result = GeoTuner.GeyserAnimTypeSymbols.meter_board;
			Geyser assignedGeyser = base.smi.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				switch (assignedGeyser.configuration.geyserType.shape)
				{
				case GeyserConfigurator.GeyserShape.Gas:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_gas;
					break;
				case GeyserConfigurator.GeyserShape.Liquid:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_liquid;
					break;
				case GeyserConfigurator.GeyserShape.Molten:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_metal;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x000CC530 File Offset: 0x000CA730
		public void OnEruptionStateChanged(object data)
		{
			bool flag = (bool)data;
			this.RefreshLogicOutput();
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x002487B0 File Offset: 0x002469B0
		public void RefreshLogicOutput()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			bool flag = this.GetCurrentState() != base.smi.sm.nonOperational;
			bool flag2 = assignedGeyser != null && this.GetCurrentState() != base.smi.sm.operational.noGeyserSelected;
			bool flag3 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && (assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.erupt || assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt);
			bool flag4 = flag && flag2 && flag3;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag4 ? 1 : 0);
			this.switchGeyserMeter.meterController.SetSymbolVisiblity("light_bloom", flag4);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x002488AC File Offset: 0x00246AAC
		public void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				GeoTuner.Instance smi = gameObject.GetSMI<GeoTuner.Instance>();
				if (smi != null && smi.GetFutureGeyser() != this.GetFutureGeyser())
				{
					Geyser futureGeyser = smi.GetFutureGeyser();
					if (futureGeyser != null && futureGeyser.GetAmountOfGeotunersPointingOrWillPointAtThisGeyser() < 5)
					{
						this.AssignFutureGeyser(smi.GetFutureGeyser());
					}
				}
			}
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0024890C File Offset: 0x00246B0C
		protected override void OnCleanUp()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			Components.GeoTuners.Remove(base.smi.GetMyWorldId(), this);
			if (assignedGeyser != null)
			{
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			GeoTuner.RemoveTuning(this);
		}

		// Token: 0x04002F08 RID: 12040
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04002F09 RID: 12041
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04002F0A RID: 12042
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04002F0B RID: 12043
		[MyCmpReq]
		public GeoTunerWorkable workable;

		// Token: 0x04002F0C RID: 12044
		[MyCmpReq]
		public GeoTunerSwitchGeyserWorkable switchGeyserWorkable;

		// Token: 0x04002F0D RID: 12045
		[MyCmpReq]
		public LogicPorts logicPorts;

		// Token: 0x04002F0E RID: 12046
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x04002F0F RID: 12047
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x04002F10 RID: 12048
		public MeterController switchGeyserMeter;

		// Token: 0x04002F11 RID: 12049
		public string originID;

		// Token: 0x04002F12 RID: 12050
		public float enhancementDuration;

		// Token: 0x04002F13 RID: 12051
		public Geyser.GeyserModification currentGeyserModification;

		// Token: 0x04002F14 RID: 12052
		private Chore switchGeyserChore;
	}
}
