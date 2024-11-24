using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class SpaceTreePlant : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
	public class Def : BaseDef
	{
		public int OptimalAmountOfBranches;

		public float OptimalProductionDuration;
	}

	public class GrowingState : PlantAliveSubState
	{
		public State idle;

		public State complete;

		public State wilted;
	}

	public class ProductionStates : PlantAliveSubState
	{
		public State wilted;

		public State halted;

		public State producing;
	}

	public class HarvestStates : PlantAliveSubState
	{
		public State wilted;

		public State prevented;

		public ManualHarvestStates manualHarvest;

		public State farmerWorkCompleted;

		public State pipes;
	}

	public class ManualHarvestStates : State
	{
		public State awaitingForFarmer;

		public State farmerWorking;
	}

	public new class Instance : GameInstance
	{
		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;

		[MyCmpReq]
		private KBatchedAnimController animController;

		[MyCmpReq]
		private Growing growingComponent;

		[MyCmpReq]
		private ConduitDispenser conduitDispenser;

		[MyCmpReq]
		private Storage storage;

		[MyCmpReq]
		private SpaceTreeSyrupHarvestWorkable workable;

		[MyCmpGet]
		private PrimaryElement pe;

		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;

		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		[MyCmpGet]
		private Growing growing;

		private PlantBranchGrower.Instance tree;

		private UnstableEntombDefense.Instance entombDefenseSMI;

		private Chore harvestChore;

		public float OptimalProductionDuration
		{
			get
			{
				if (!IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		public float CurrentProductionProgress => base.sm.Fullness.Get(this);

		public bool IsWilting => base.gameObject.HasTag(GameTags.Wilting);

		public bool IsMature => growingComponent.IsGrown();

		public bool HasAtLeastOneBranch => BranchCount > 0;

		public bool IsReadyForHarvest => base.sm.ReadyForHarvest.Get(base.smi);

		public bool CanBeManuallyHarvested
		{
			get
			{
				if (UserAllowsHarvest)
				{
					return !HasPipeConnected;
				}
				return false;
			}
		}

		public bool UserAllowsHarvest
		{
			get
			{
				if (!(harvestDesignatable == null))
				{
					if (harvestDesignatable.HarvestWhenReady)
					{
						return harvestDesignatable.MarkedForHarvest;
					}
					return false;
				}
				return true;
			}
		}

		public bool HasPipeConnected => conduitDispenser.IsConnected;

		public bool IsUprooted
		{
			get
			{
				if (uprootMonitor != null)
				{
					return uprootMonitor.IsUprooted;
				}
				return false;
			}
		}

		public bool IsWildPlanted => !receptacleMonitor.Replanted;

		public bool IsEntombed
		{
			get
			{
				if (entombDefenseSMI != null)
				{
					return entombDefenseSMI.IsEntombed;
				}
				return false;
			}
		}

		public bool IsPipingEnabled => base.sm.PipingEnabled.Get(this);

		public int BranchCount
		{
			get
			{
				if (tree != null)
				{
					return tree.CurrentBranchCount;
				}
				return 0;
			}
		}

		public Workable GetWorkable()
		{
			return workable;
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			tree = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
			tree.ActionPerBranch(SubscribeToBranchCallbacks);
			tree.Subscribe(-1586842875, SubscribeToNewBranches);
			entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			base.StartSM();
			SetPipingState(IsPipingEnabled);
			RefreshFullnessVariable();
			SpaceTreeSyrupHarvestWorkable spaceTreeSyrupHarvestWorkable = workable;
			spaceTreeSyrupHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(spaceTreeSyrupHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnManualHarvestWorkableStateChanges));
		}

		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			switch (workableEvent)
			{
			case Workable.WorkableEvent.WorkStarted:
				InformBranchesTrunkIsBeingHarvestedManually();
				break;
			case Workable.WorkableEvent.WorkStopped:
				InformBranchesTrunkIsNoLongerBeingHarvestedManually();
				break;
			}
		}

		private void SubscribeToNewBranches(object obj)
		{
			if (obj != null)
			{
				PlantBranch.Instance instance = (PlantBranch.Instance)obj;
				SubscribeToBranchCallbacks(instance.gameObject);
			}
		}

		private void SubscribeToBranchCallbacks(GameObject branch)
		{
			branch.Subscribe(-724860998, OnBranchWiltStateChanged);
			branch.Subscribe(712767498, OnBranchWiltStateChanged);
			branch.Subscribe(-254803949, OnBranchGrowStatusChanged);
		}

		private void OnBranchGrowStatusChanged(object obj)
		{
			base.sm.BranchGrownStatusChanged.Trigger(this);
		}

		private void OnBranchWiltStateChanged(object obj)
		{
			base.sm.BranchWiltConditionChanged.Trigger(this);
		}

		public void SubscribeToUpdateNewBranchesReadyForHarvest()
		{
			tree.Subscribe(-1586842875, OnNewBranchSpawnedWhileTreeIsReadyForHarvest);
		}

		public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
		{
			tree.Unsubscribe(-1586842875, OnNewBranchSpawnedWhileTreeIsReadyForHarvest);
		}

		private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
		{
			if (data != null)
			{
				((PlantBranch.Instance)data).gameObject.AddTag(SpaceTreeReadyForHarvest);
			}
		}

		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this);
			SetConduitDispenserAbilityToDispense(enable);
		}

		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			conduitDispenser.SetOnState(canDispense);
		}

		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(SpaceTreeReadyForHarvest);
				if (tree != null)
				{
					tree.ActionPerBranch(delegate(GameObject branch)
					{
						branch.AddTag(SpaceTreeReadyForHarvest);
					});
				}
				return;
			}
			base.gameObject.RemoveTag(SpaceTreeReadyForHarvest);
			if (tree != null)
			{
				tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.RemoveTag(SpaceTreeReadyForHarvest);
				});
			}
		}

		public bool HasAtLeastOneHealthyFullyGrownBranch()
		{
			if (tree == null || BranchCount <= 0)
			{
				return false;
			}
			bool healthyGrownBranchFound = false;
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance sMI = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (sMI != null && !sMI.isMasterNull)
				{
					healthyGrownBranchFound = healthyGrownBranchFound || (sMI.IsBranchFullyGrown && !sMI.wiltCondition.IsWilting());
				}
			});
			return healthyGrownBranchFound;
		}

		public void CreateHarvestChore()
		{
			if (harvestChore == null)
			{
				harvestChore = new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, workable);
			}
		}

		public void CancelHarvestChore()
		{
			if (harvestChore != null)
			{
				harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
				harvestChore = null;
			}
		}

		public void ProduceUpdate(float dt)
		{
			float mass = Mathf.Min(dt / base.smi.OptimalProductionDuration * base.smi.GetProductionSpeed() * storage.capacityKg, storage.RemainingCapacity());
			float lowTemp = ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp;
			float num = 8f;
			float temperature = Mathf.Max(pe.Temperature, lowTemp + num);
			storage.AddLiquid(SimHashes.SugarWater, mass, temperature, byte.MaxValue, 0);
		}

		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			storage.DropAll(vent_gas: false, dump_liquid: false, default(Vector3), do_disease_transfer: true, list);
			foreach (GameObject item in list)
			{
				Vector3 position = item.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				item.transform.SetPosition(position);
			}
		}

		public void PlayHarvestReadyAnimation()
		{
			if (animController != null)
			{
				animController.Play("harvest_ready", KAnim.PlayMode.Loop);
			}
		}

		public void InformBranchesTrunkIsBeingHarvestedManually()
		{
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(2137182770);
			});
		}

		public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
		{
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-808006162);
			});
		}

		public void InformBranchesTrunkWantsToUnentomb()
		{
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(570354093);
			});
		}

		public void RefreshFullnessVariable()
		{
			float fullness = storage.MassStored() / storage.capacityKg;
			base.sm.Fullness.Set(fullness, this);
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-824970674, fullness);
			});
			if (fullness < 0.25f)
			{
				SetPipingState(enable: false);
			}
		}

		public float GetProductionSpeed()
		{
			if (tree == null)
			{
				return 0f;
			}
			float totalProduction = 0f;
			tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance sMI = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (sMI != null && !sMI.isMasterNull)
				{
					totalProduction += sMI.Productivity;
				}
			});
			return totalProduction / (float)base.def.OptimalAmountOfBranches;
		}

		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(growing.PercentOfCurrentHarvest() / (1f / 3f)), 0, 2);
			return "wilt" + (num + 1);
		}

		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(CurrentProductionProgress * 42f);
			if (animController.currentAnim != "grow_fill")
			{
				animController.Play("grow_fill", KAnim.PlayMode.Paused);
				animController.SetPositionPercent(CurrentProductionProgress);
				animController.enabled = false;
				animController.enabled = true;
			}
			else if (animController.currentFrame != num)
			{
				animController.SetPositionPercent(CurrentProductionProgress);
			}
		}

		public void RefreshGrowingAnimation()
		{
			animController.SetPositionPercent(growing.PercentOfCurrentHarvest());
		}
	}

	public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;

	public static Tag SpaceTreeReadyForHarvest = TagManager.Create("SpaceTreeReadyForHarvest");

	public const string GROWN_WILT_ANIM_NAME = "idle_empty";

	public const string WILT_ANIM_NAME = "wilt";

	public const string GROW_ANIM_NAME = "grow";

	public const string GROW_PST_ANIM_NAME = "grow_pst";

	public const string FILL_ANIM_NAME = "grow_fill";

	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";

	private const int FILLING_ANIMATION_FRAME_COUNT = 42;

	private const int WILT_LEVELS = 3;

	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	public const SimHashes ProductElement = SimHashes.SugarWater;

	public GrowingState growing;

	public ProductionStates production;

	public HarvestStates harvest;

	public State harvestCompleted;

	public State dead;

	public BoolParameter ReadyForHarvest;

	public BoolParameter PipingEnabled;

	public FloatParameter Fullness;

	public Signal BranchWiltConditionChanged;

	public Signal BranchGrownStatusChanged;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = growing;
		base.serializable = SerializeType.ParamsOnly;
		root.EventHandler(GameHashes.OnStorageChange, RefreshFullnessVariable);
		growing.InitializeStates(masterTarget, dead).DefaultState(growing.idle);
		growing.idle.EventTransition(GameHashes.Grow, growing.complete, IsTrunkMature).EventTransition(GameHashes.Wilt, growing.wilted, IsTrunkWilted).PlayAnim((Instance smi) => "grow", KAnim.PlayMode.Paused)
			.Enter(RefreshGrowingAnimation)
			.Update(RefreshGrowingAnimationUpdate, UpdateRate.SIM_4000ms);
		growing.complete.EnterTransition(production, TrunkHasAtLeastOneBranch).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(production);
		growing.wilted.EventTransition(GameHashes.WiltRecover, growing.idle, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.Not(IsTrunkWilted)).PlayAnim(GetGrowingStatesWiltedAnim, KAnim.PlayMode.Loop);
		production.InitializeStates(masterTarget, dead).EventTransition(GameHashes.Grow, growing, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.Not(IsTrunkMature)).ParamTransition(ReadyForHarvest, harvest, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.IsTrue)
			.ParamTransition(Fullness, harvest, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.IsGTEOne)
			.DefaultState(production.producing);
		production.producing.EventTransition(GameHashes.Wilt, production.wilted, IsTrunkWilted).OnSignal(BranchWiltConditionChanged, production.halted, CanNOTProduce).OnSignal(BranchGrownStatusChanged, production.halted, CanNOTProduce)
			.Enter(RefreshFullnessAnimation)
			.EventHandler(GameHashes.OnStorageChange, RefreshFullnessAnimation)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater)
			.Update(ProductionUpdate);
		production.halted.EventTransition(GameHashes.Wilt, production.wilted, IsTrunkWilted).EventTransition(GameHashes.TreeBranchCountChanged, production.producing, CanProduce).OnSignal(BranchWiltConditionChanged, production.producing, CanProduce)
			.OnSignal(BranchGrownStatusChanged, production.producing, CanProduce)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused)
			.Enter(RefreshFullnessAnimation);
		production.wilted.EventTransition(GameHashes.WiltRecover, production.producing, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.Not(IsTrunkWilted)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted).PlayAnim("idle_empty", KAnim.PlayMode.Once)
			.EventHandler(GameHashes.EntombDefenseReactionBegins, InformBranchesTrunkWantsToBreakFree);
		harvest.InitializeStates(masterTarget, dead).EventTransition(GameHashes.Grow, growing, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.Not(IsTrunkMature)).ParamTransition(Fullness, harvestCompleted, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.IsLTOne)
			.EventHandler(GameHashes.EntombDefenseReactionBegins, InformBranchesTrunkWantsToBreakFree)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest)
			.Enter(SetReadyToHarvest)
			.Enter(EnablePiping)
			.DefaultState(harvest.prevented);
		harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", AddHarvestReadyTag, RemoveHarvestReadyTag).Toggle("SetTag_ReadyForHarvest_OnNewBanches", SubscribeToUpdateNewBranchesReadyForHarvest, UnsubscribeToUpdateNewBranchesReadyForHarvest)
			.EventHandler(GameHashes.EntombedChanged, PlayHarvestReadyOnUntentombed)
			.EventTransition(GameHashes.HarvestDesignationChanged, harvest.manualHarvest, CanBeManuallyHarvested)
			.EventTransition(GameHashes.ConduitConnectionChanged, harvest.pipes, (Instance smi) => HasPipeConnected(smi) && smi.IsPipingEnabled)
			.ParamTransition(PipingEnabled, harvest.pipes, (Instance smi, bool pipeEnable) => pipeEnable && HasPipeConnected(smi));
		harvest.manualHarvest.DefaultState(harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", AddHarvestReadyTag, RemoveHarvestReadyTag).Toggle("SetTag_ReadyForHarvest_OnNewBanches", SubscribeToUpdateNewBranchesReadyForHarvest, UnsubscribeToUpdateNewBranchesReadyForHarvest)
			.Enter(ShowSkillRequiredStatusItemIfSkillMissing)
			.Enter(StartHarvestWorkChore)
			.EventHandler(GameHashes.EntombedChanged, PlayHarvestReadyOnUntentombed)
			.EventTransition(GameHashes.HarvestDesignationChanged, harvest.prevented, GameStateMachine<SpaceTreePlant, Instance, IStateMachineTarget, Def>.Not(CanBeManuallyHarvested))
			.EventTransition(GameHashes.ConduitConnectionChanged, harvest.pipes, (Instance smi) => HasPipeConnected(smi) && smi.IsPipingEnabled)
			.ParamTransition(PipingEnabled, harvest.pipes, (Instance smi, bool pipeEnable) => pipeEnable && HasPipeConnected(smi))
			.WorkableCompleteTransition(GetWorkable, harvest.farmerWorkCompleted)
			.Exit(CancelHarvestWorkChore)
			.Exit(HideSkillRequiredStatusItemIfSkillMissing);
		harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(GetWorkable, harvest.manualHarvest.farmerWorking);
		harvest.manualHarvest.farmerWorking.WorkableStopTransition(GetWorkable, harvest.manualHarvest.awaitingForFarmer);
		harvest.farmerWorkCompleted.Enter(DropInventory);
		harvest.pipes.Enter(RefreshFullnessAnimation).Toggle("ToggleReadyForHarvest", AddHarvestReadyTag, RemoveHarvestReadyTag).Toggle("SetTag_ReadyForHarvest_OnNewBanches", SubscribeToUpdateNewBranchesReadyForHarvest, UnsubscribeToUpdateNewBranchesReadyForHarvest)
			.PlayAnim("harvest_ready", KAnim.PlayMode.Loop)
			.EventHandler(GameHashes.OnStorageChange, RefreshFullnessAnimation)
			.EventTransition(GameHashes.ConduitConnectionChanged, harvest.prevented, (Instance smi) => !smi.IsPipingEnabled || !HasPipeConnected(smi))
			.ParamTransition(PipingEnabled, harvest.prevented, (Instance smi, bool pipeEnable) => !pipeEnable || !HasPipeConnected(smi));
		harvestCompleted.Enter(UnsetReadyToHarvest).GoTo(production);
		dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead).Enter(delegate(Instance smi)
		{
			if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = CreateDeathNotification(smi);
				notifier.Add(notification);
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(value: true);
			smi.Trigger(1623392196);
			smi.GetComponent<KBatchedAnimController>().StopAndClear();
			UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
		}).ScheduleAction("Delayed Destroy", 0.5f, SelfDestroy);
	}

	public Workable GetWorkable(Instance smi)
	{
		return smi.GetWorkable();
	}

	public static void EnablePiping(Instance smi)
	{
		smi.SetPipingState(enable: true);
	}

	public static void InformBranchesTrunkWantsToBreakFree(Instance smi)
	{
		smi.InformBranchesTrunkWantsToUnentomb();
	}

	public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(Instance smi)
	{
		smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
	}

	public static void SubscribeToUpdateNewBranchesReadyForHarvest(Instance smi)
	{
		smi.SubscribeToUpdateNewBranchesReadyForHarvest();
	}

	public static void RefreshFullnessVariable(Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	public static void ShowSkillRequiredStatusItemIfSkillMissing(Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(shouldItBeShown: true);
	}

	public static void HideSkillRequiredStatusItemIfSkillMissing(Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(shouldItBeShown: false);
	}

	public static void StartHarvestWorkChore(Instance smi)
	{
		smi.CreateHarvestChore();
	}

	public static void CancelHarvestWorkChore(Instance smi)
	{
		smi.CancelHarvestChore();
	}

	public static bool HasPipeConnected(Instance smi)
	{
		return smi.HasPipeConnected;
	}

	public static bool CanBeManuallyHarvested(Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	public static void SetReadyToHarvest(Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(value: true, smi);
	}

	public static void UnsetReadyToHarvest(Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(value: false, smi);
	}

	public static void RefreshFullnessAnimation(Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	public static void ProductionUpdate(Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	public static void DropInventory(Instance smi)
	{
		smi.DropInventory();
	}

	public static void AddHarvestReadyTag(Instance smi)
	{
		smi.SetReadyForHarvestTag(isReady: true);
	}

	public static void RemoveHarvestReadyTag(Instance smi)
	{
		smi.SetReadyForHarvestTag(isReady: false);
	}

	public static string GetGrowingStatesWiltedAnim(Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	public static void RefreshGrowingAnimation(Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	public static void RefreshGrowingAnimationUpdate(Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

	public static bool TrunkHasAtLeastOneBranch(Instance smi)
	{
		return smi.HasAtLeastOneBranch;
	}

	public static bool IsTrunkMature(Instance smi)
	{
		return smi.IsMature;
	}

	public static bool IsTrunkWilted(Instance smi)
	{
		return smi.IsWilting;
	}

	public static bool CanNOTProduce(Instance smi)
	{
		return !CanProduce(smi);
	}

	public static void PlayHarvestReadyOnUntentombed(Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayHarvestReadyAnimation();
		}
	}

	public static void SelfDestroy(Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	public static bool CanProduce(Instance smi)
	{
		if (!smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest)
		{
			return smi.HasAtLeastOneHealthyFullyGrownBranch();
		}
		return false;
	}

	public static Notification CreateDeathNotification(Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => string.Concat(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, notificationList.ReduceMessages(countNames: false)), "/t• " + smi.gameObject.GetProperName());
	}
}
