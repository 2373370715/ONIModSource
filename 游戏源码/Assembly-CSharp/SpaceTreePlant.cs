using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class SpaceTreePlant : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
	// Token: 0x06000AF2 RID: 2802 RVA: 0x0016DC58 File Offset: 0x0016BE58
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessVariable));
		this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
		this.growing.idle.EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature)).EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).PlayAnim((SpaceTreePlant.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshGrowingAnimation)).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms, false);
		this.growing.complete.EnterTransition(this.production, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.TrunkHasAtLeastOneBranch)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.production);
		this.growing.wilted.EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).PlayAnim(new Func<SpaceTreePlant.Instance, string>(SpaceTreePlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
		this.production.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<bool>(this.ReadyForHarvest, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsTrue).ParamTransition<float>(this.Fullness, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsGTEOne).DefaultState(this.production.producing);
		this.production.producing.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).OnSignal(this.BranchWiltConditionChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater, null).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.ProductionUpdate), UpdateRate.SIM_200ms, false);
		this.production.halted.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).EventTransition(GameHashes.TreeBranchCountChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchWiltConditionChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation));
		this.production.wilted.EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted, null).PlayAnim("idle_empty", KAnim.PlayMode.Once).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree));
		this.harvest.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<float>(this.Fullness, this.harvestCompleted, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsLTOne).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree)).ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SetReadyToHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.EnablePiping)).DefaultState(this.harvest.prevented);
		this.harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.manualHarvest, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi));
		this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.ShowSkillRequiredStatusItemIfSkillMissing)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.StartHarvestWorkChore)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested))).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi)).WorkableCompleteTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.CancelHarvestWorkChore)).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.HideSkillRequiredStatusItemIfSkillMissing));
		this.harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
		this.harvest.manualHarvest.farmerWorking.WorkableStopTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
		this.harvest.farmerWorkCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.DropInventory));
		this.harvest.pipes.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).PlayAnim("harvest_ready", KAnim.PlayMode.Loop).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (SpaceTreePlant.Instance smi) => !smi.IsPipingEnabled || !SpaceTreePlant.HasPipeConnected(smi)).ParamTransition<bool>(this.PipingEnabled, this.harvest.prevented, (SpaceTreePlant.Instance smi, bool pipeEnable) => !pipeEnable || !SpaceTreePlant.HasPipeConnected(smi));
		this.harvestCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsetReadyToHarvest)).GoTo(this.production);
		this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(SpaceTreePlant.Instance smi)
		{
			if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = SpaceTreePlant.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.GetComponent<KBatchedAnimController>().StopAndClear();
			UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
		}).ScheduleAction("Delayed Destroy", 0.5f, new Action<SpaceTreePlant.Instance>(SpaceTreePlant.SelfDestroy));
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x000AB1EE File Offset: 0x000A93EE
	public Workable GetWorkable(SpaceTreePlant.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x000AB1F6 File Offset: 0x000A93F6
	public static void EnablePiping(SpaceTreePlant.Instance smi)
	{
		smi.SetPipingState(true);
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x000AB1FF File Offset: 0x000A93FF
	public static void InformBranchesTrunkWantsToBreakFree(SpaceTreePlant.Instance smi)
	{
		smi.InformBranchesTrunkWantsToUnentomb();
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x000AB207 File Offset: 0x000A9407
	public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x000AB20F File Offset: 0x000A940F
	public static void SubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.SubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x000AB217 File Offset: 0x000A9417
	public static void RefreshFullnessVariable(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000AB21F File Offset: 0x000A941F
	public static void ShowSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000AB22D File Offset: 0x000A942D
	public static void HideSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x000AB23B File Offset: 0x000A943B
	public static void StartHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CreateHarvestChore();
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x000AB243 File Offset: 0x000A9443
	public static void CancelHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CancelHarvestChore();
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x000AB24B File Offset: 0x000A944B
	public static bool HasPipeConnected(SpaceTreePlant.Instance smi)
	{
		return smi.HasPipeConnected;
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x000AB253 File Offset: 0x000A9453
	public static bool CanBeManuallyHarvested(SpaceTreePlant.Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x000AB25B File Offset: 0x000A945B
	public static void SetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(true, smi, false);
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x000AB271 File Offset: 0x000A9471
	public static void UnsetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(false, smi, false);
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x000AB287 File Offset: 0x000A9487
	public static void RefreshFullnessAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x000AB28F File Offset: 0x000A948F
	public static void ProductionUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x000AB298 File Offset: 0x000A9498
	public static void DropInventory(SpaceTreePlant.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x000AB2A0 File Offset: 0x000A94A0
	public static void AddHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(true);
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x000AB2A9 File Offset: 0x000A94A9
	public static void RemoveHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(false);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x000AB2B2 File Offset: 0x000A94B2
	public static string GetGrowingStatesWiltedAnim(SpaceTreePlant.Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x000AB2BA File Offset: 0x000A94BA
	public static void RefreshGrowingAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x000AB2BA File Offset: 0x000A94BA
	public static void RefreshGrowingAnimationUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x000AB2C2 File Offset: 0x000A94C2
	public static bool TrunkHasAtLeastOneBranch(SpaceTreePlant.Instance smi)
	{
		return smi.HasAtLeastOneBranch;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x000AB2CA File Offset: 0x000A94CA
	public static bool IsTrunkMature(SpaceTreePlant.Instance smi)
	{
		return smi.IsMature;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x000AB2D2 File Offset: 0x000A94D2
	public static bool IsTrunkWilted(SpaceTreePlant.Instance smi)
	{
		return smi.IsWilting;
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x000AB2DA File Offset: 0x000A94DA
	public static bool CanNOTProduce(SpaceTreePlant.Instance smi)
	{
		return !SpaceTreePlant.CanProduce(smi);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x000AB2E5 File Offset: 0x000A94E5
	public static void PlayHarvestReadyOnUntentombed(SpaceTreePlant.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayHarvestReadyAnimation();
		}
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x000AAC7F File Offset: 0x000A8E7F
	public static void SelfDestroy(SpaceTreePlant.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x000AB2F5 File Offset: 0x000A94F5
	public static bool CanProduce(SpaceTreePlant.Instance smi)
	{
		return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest && smi.HasAtLeastOneHealthyFullyGrownBranch();
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0016E528 File Offset: 0x0016C728
	public static Notification CreateDeathNotification(SpaceTreePlant.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x0400087E RID: 2174
	public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;

	// Token: 0x0400087F RID: 2175
	public static Tag SpaceTreeReadyForHarvest = TagManager.Create("SpaceTreeReadyForHarvest");

	// Token: 0x04000880 RID: 2176
	public const string GROWN_WILT_ANIM_NAME = "idle_empty";

	// Token: 0x04000881 RID: 2177
	public const string WILT_ANIM_NAME = "wilt";

	// Token: 0x04000882 RID: 2178
	public const string GROW_ANIM_NAME = "grow";

	// Token: 0x04000883 RID: 2179
	public const string GROW_PST_ANIM_NAME = "grow_pst";

	// Token: 0x04000884 RID: 2180
	public const string FILL_ANIM_NAME = "grow_fill";

	// Token: 0x04000885 RID: 2181
	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";

	// Token: 0x04000886 RID: 2182
	private const int FILLING_ANIMATION_FRAME_COUNT = 42;

	// Token: 0x04000887 RID: 2183
	private const int WILT_LEVELS = 3;

	// Token: 0x04000888 RID: 2184
	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	// Token: 0x04000889 RID: 2185
	public const SimHashes ProductElement = SimHashes.SugarWater;

	// Token: 0x0400088A RID: 2186
	public SpaceTreePlant.GrowingState growing;

	// Token: 0x0400088B RID: 2187
	public SpaceTreePlant.ProductionStates production;

	// Token: 0x0400088C RID: 2188
	public SpaceTreePlant.HarvestStates harvest;

	// Token: 0x0400088D RID: 2189
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State harvestCompleted;

	// Token: 0x0400088E RID: 2190
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State dead;

	// Token: 0x0400088F RID: 2191
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter ReadyForHarvest;

	// Token: 0x04000890 RID: 2192
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter PipingEnabled;

	// Token: 0x04000891 RID: 2193
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.FloatParameter Fullness;

	// Token: 0x04000892 RID: 2194
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchWiltConditionChanged;

	// Token: 0x04000893 RID: 2195
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchGrownStatusChanged;

	// Token: 0x020002CD RID: 717
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000894 RID: 2196
		public int OptimalAmountOfBranches;

		// Token: 0x04000895 RID: 2197
		public float OptimalProductionDuration;
	}

	// Token: 0x020002CE RID: 718
	public class GrowingState : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x04000896 RID: 2198
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State idle;

		// Token: 0x04000897 RID: 2199
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State complete;

		// Token: 0x04000898 RID: 2200
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
	}

	// Token: 0x020002CF RID: 719
	public class ProductionStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x04000899 RID: 2201
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x0400089A RID: 2202
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State halted;

		// Token: 0x0400089B RID: 2203
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State producing;
	}

	// Token: 0x020002D0 RID: 720
	public class HarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x0400089C RID: 2204
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x0400089D RID: 2205
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State prevented;

		// Token: 0x0400089E RID: 2206
		public SpaceTreePlant.ManualHarvestStates manualHarvest;

		// Token: 0x0400089F RID: 2207
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorkCompleted;

		// Token: 0x040008A0 RID: 2208
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State pipes;
	}

	// Token: 0x020002D1 RID: 721
	public class ManualHarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State
	{
		// Token: 0x040008A1 RID: 2209
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State awaitingForFarmer;

		// Token: 0x040008A2 RID: 2210
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorking;
	}

	// Token: 0x020002D2 RID: 722
	public new class Instance : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.GameInstance
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000AB348 File Offset: 0x000A9548
		public float OptimalProductionDuration
		{
			get
			{
				if (!this.IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x000AB36F File Offset: 0x000A956F
		public float CurrentProductionProgress
		{
			get
			{
				return base.sm.Fullness.Get(this);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x000AB382 File Offset: 0x000A9582
		public bool IsWilting
		{
			get
			{
				return base.gameObject.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x000AB394 File Offset: 0x000A9594
		public bool IsMature
		{
			get
			{
				return this.growingComponent.IsGrown();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x000AB3A1 File Offset: 0x000A95A1
		public bool HasAtLeastOneBranch
		{
			get
			{
				return this.BranchCount > 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x000AB3AC File Offset: 0x000A95AC
		public bool IsReadyForHarvest
		{
			get
			{
				return base.sm.ReadyForHarvest.Get(base.smi);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x000AB3C4 File Offset: 0x000A95C4
		public bool CanBeManuallyHarvested
		{
			get
			{
				return this.UserAllowsHarvest && !this.HasPipeConnected;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x000AB3D9 File Offset: 0x000A95D9
		public bool UserAllowsHarvest
		{
			get
			{
				return this.harvestDesignatable == null || (this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x000AB405 File Offset: 0x000A9605
		public bool HasPipeConnected
		{
			get
			{
				return this.conduitDispenser.IsConnected;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x000AB412 File Offset: 0x000A9612
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000AB42F File Offset: 0x000A962F
		public bool IsWildPlanted
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x000AB43F File Offset: 0x000A963F
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000AB456 File Offset: 0x000A9656
		public bool IsPipingEnabled
		{
			get
			{
				return base.sm.PipingEnabled.Get(this);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x000AB469 File Offset: 0x000A9669
		public int BranchCount
		{
			get
			{
				if (this.tree != null)
				{
					return this.tree.CurrentBranchCount;
				}
				return 0;
			}
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000AB480 File Offset: 0x000A9680
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x000AB488 File Offset: 0x000A9688
		public Instance(IStateMachineTarget master, SpaceTreePlant.Def def) : base(master, def)
		{
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0016E588 File Offset: 0x0016C788
		public override void StartSM()
		{
			this.tree = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
			this.tree.ActionPerBranch(new Action<GameObject>(this.SubscribeToBranchCallbacks));
			this.tree.Subscribe(-1586842875, new Action<object>(this.SubscribeToNewBranches));
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			base.StartSM();
			this.SetPipingState(this.IsPipingEnabled);
			this.RefreshFullnessVariable();
			SpaceTreeSyrupHarvestWorkable spaceTreeSyrupHarvestWorkable = this.workable;
			spaceTreeSyrupHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(spaceTreeSyrupHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges));
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000AB492 File Offset: 0x000A9692
		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			if (workableEvent == Workable.WorkableEvent.WorkStarted)
			{
				this.InformBranchesTrunkIsBeingHarvestedManually();
				return;
			}
			if (workableEvent == Workable.WorkableEvent.WorkStopped)
			{
				this.InformBranchesTrunkIsNoLongerBeingHarvestedManually();
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0016E62C File Offset: 0x0016C82C
		private void SubscribeToNewBranches(object obj)
		{
			if (obj == null)
			{
				return;
			}
			PlantBranch.Instance instance = (PlantBranch.Instance)obj;
			this.SubscribeToBranchCallbacks(instance.gameObject);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0016E650 File Offset: 0x0016C850
		private void SubscribeToBranchCallbacks(GameObject branch)
		{
			branch.Subscribe(-724860998, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(712767498, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(-254803949, new Action<object>(this.OnBranchGrowStatusChanged));
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x000AB4A8 File Offset: 0x000A96A8
		private void OnBranchGrowStatusChanged(object obj)
		{
			base.sm.BranchGrownStatusChanged.Trigger(this);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x000AB4BB File Offset: 0x000A96BB
		private void OnBranchWiltStateChanged(object obj)
		{
			base.sm.BranchWiltConditionChanged.Trigger(this);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000AB4CE File Offset: 0x000A96CE
		public void SubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Subscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000AB4EC File Offset: 0x000A96EC
		public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Unsubscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000AB50A File Offset: 0x000A970A
		private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
		{
			if (data == null)
			{
				return;
			}
			((PlantBranch.Instance)data).gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000AB525 File Offset: 0x000A9725
		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this, false);
			this.SetConduitDispenserAbilityToDispense(enable);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000AB542 File Offset: 0x000A9742
		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			this.conduitDispenser.SetOnState(canDispense);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0016E6A8 File Offset: 0x0016C8A8
		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
			else
			{
				base.gameObject.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0016E740 File Offset: 0x0016C940
		public bool HasAtLeastOneHealthyFullyGrownBranch()
		{
			if (this.tree == null || this.BranchCount <= 0)
			{
				return false;
			}
			bool healthyGrownBranchFound = false;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					healthyGrownBranchFound = (healthyGrownBranchFound || (smi.IsBranchFullyGrown && !smi.wiltCondition.IsWilting()));
				}
			});
			return healthyGrownBranchFound;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0016E78C File Offset: 0x0016C98C
		public void CreateHarvestChore()
		{
			if (this.harvestChore == null)
			{
				this.harvestChore = new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000AB550 File Offset: 0x000A9750
		public void CancelHarvestChore()
		{
			if (this.harvestChore != null)
			{
				this.harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
				this.harvestChore = null;
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0016E7D4 File Offset: 0x0016C9D4
		public void ProduceUpdate(float dt)
		{
			float mass = Mathf.Min(dt / base.smi.OptimalProductionDuration * base.smi.GetProductionSpeed() * this.storage.capacityKg, this.storage.RemainingCapacity());
			float lowTemp = ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp;
			float num = 8f;
			float temperature = Mathf.Max(this.pe.Temperature, lowTemp + num);
			this.storage.AddLiquid(SimHashes.SugarWater, mass, temperature, byte.MaxValue, 0, false, true);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0016E864 File Offset: 0x0016CA64
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject in list)
			{
				Vector3 position = gameObject.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
			}
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000AB571 File Offset: 0x000A9771
		public void PlayHarvestReadyAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play("harvest_ready", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000AB5A1 File Offset: 0x000A97A1
		public void InformBranchesTrunkIsBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(2137182770, null);
			});
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000AB5CD File Offset: 0x000A97CD
		public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-808006162, null);
			});
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000AB5F9 File Offset: 0x000A97F9
		public void InformBranchesTrunkWantsToUnentomb()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(570354093, null);
			});
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0016E8F4 File Offset: 0x0016CAF4
		public void RefreshFullnessVariable()
		{
			float fullness = this.storage.MassStored() / this.storage.capacityKg;
			base.sm.Fullness.Set(fullness, this, false);
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-824970674, fullness);
			});
			if (fullness < 0.25f)
			{
				this.SetPipingState(false);
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0016E968 File Offset: 0x0016CB68
		public float GetProductionSpeed()
		{
			if (this.tree == null)
			{
				return 0f;
			}
			float totalProduction = 0f;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					totalProduction += smi.Productivity;
				}
			});
			return totalProduction / (float)base.def.OptimalAmountOfBranches;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0016E9C0 File Offset: 0x0016CBC0
		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.33333334f), 0, 2);
			return "wilt" + (num + 1).ToString();
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0016EA00 File Offset: 0x0016CC00
		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
			if (this.animController.currentAnim != "grow_fill")
			{
				this.animController.Play("grow_fill", KAnim.PlayMode.Paused, 1f, 0f);
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
				this.animController.enabled = false;
				this.animController.enabled = true;
				return;
			}
			if (this.animController.currentFrame != num)
			{
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x000AB625 File Offset: 0x000A9825
		public void RefreshGrowingAnimation()
		{
			this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
		}

		// Token: 0x040008A3 RID: 2211
		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;

		// Token: 0x040008A4 RID: 2212
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x040008A5 RID: 2213
		[MyCmpReq]
		private Growing growingComponent;

		// Token: 0x040008A6 RID: 2214
		[MyCmpReq]
		private ConduitDispenser conduitDispenser;

		// Token: 0x040008A7 RID: 2215
		[MyCmpReq]
		private Storage storage;

		// Token: 0x040008A8 RID: 2216
		[MyCmpReq]
		private SpaceTreeSyrupHarvestWorkable workable;

		// Token: 0x040008A9 RID: 2217
		[MyCmpGet]
		private PrimaryElement pe;

		// Token: 0x040008AA RID: 2218
		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;

		// Token: 0x040008AB RID: 2219
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x040008AC RID: 2220
		[MyCmpGet]
		private Growing growing;

		// Token: 0x040008AD RID: 2221
		private PlantBranchGrower.Instance tree;

		// Token: 0x040008AE RID: 2222
		private UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x040008AF RID: 2223
		private Chore harvestChore;
	}
}
