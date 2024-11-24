using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000C4B RID: 3147
public class BionicUpgrade_ExplorerBoosterMonitor : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>
{
	// Token: 0x06003C4D RID: 15437 RVA: 0x0022D82C File Offset: 0x0022BA2C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.attachToBooster;
		this.attachToBooster.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.FindAndAttachToInstalledBooster)).GoTo(this.Inactive);
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode))).EventTransition(GameHashes.ScheduleChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode))).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline))).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive))).DefaultState(this.Active.gatheringData);
		this.Active.gatheringData.OnSignal(this.ReadyToDiscoverSignal, this.Active.discover, new Func<BionicUpgrade_ExplorerBoosterMonitor.Instance, bool>(BionicUpgrade_ExplorerBoosterMonitor.IsReadyToDiscoverAndThereIsSomethingToDiscover)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicExplorerBooster, null).Update(new Action<BionicUpgrade_ExplorerBoosterMonitor.Instance, float>(BionicUpgrade_ExplorerBoosterMonitor.DataGatheringUpdate), UpdateRate.SIM_200ms, false);
		this.Active.discover.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.ConsumeAllData)).Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.RevealUndiscoveredGeyser)).EnterTransition(this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover))).GoTo(this.Active.gatheringData);
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x000C6F84 File Offset: 0x000C5184
	public static bool ShouldBeActive(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline(smi) && BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBatterySaveMode(smi) && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06003C4F RID: 15439 RVA: 0x000C6F9E File Offset: 0x000C519E
	public static bool IsReadyToDiscoverAndThereIsSomethingToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return smi.IsReadyToDiscover && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x000C6FB0 File Offset: 0x000C51B0
	public static void ConsumeAllData(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.ConsumeAllData();
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x000C6FB8 File Offset: 0x000C51B8
	public static void FindAndAttachToInstalledBooster(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x000C6FC0 File Offset: 0x000C51C0
	public static void DataGatheringUpdate(BionicUpgrade_ExplorerBoosterMonitor.Instance smi, float dt)
	{
		smi.GatheringDataUpdate(dt);
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x0022DA78 File Offset: 0x0022BC78
	public static bool IsThereGeysersToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			return list.Count > 0;
		}
		return false;
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0022DB14 File Offset: 0x0022BD14
	public static void RevealUndiscoveredGeyser(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			if (list.Count > 0)
			{
				WorldGenSpawner.Spawnable random = list.GetRandom<WorldGenSpawner.Spawnable>();
				int baseX;
				int baseY;
				Grid.CellToXY(random.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, 4, 4f);
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification geyserDiscoveredNotification = smi.GetGeyserDiscoveredNotification();
				int cell = random.cell;
				geyserDiscoveredNotification.customClickCallback = delegate(object obj)
				{
					GameUtil.FocusCamera(cell);
				};
				notifier.Add(geyserDiscoveredNotification, "");
			}
		}
	}

	// Token: 0x04002936 RID: 10550
	public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State attachToBooster;

	// Token: 0x04002937 RID: 10551
	public new BionicUpgrade_ExplorerBoosterMonitor.ActiveStates Active;

	// Token: 0x04002938 RID: 10552
	public StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Signal ReadyToDiscoverSignal;

	// Token: 0x02000C4C RID: 3148
	public new class Def : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def
	{
		// Token: 0x06003C56 RID: 15446 RVA: 0x000C6FD1 File Offset: 0x000C51D1
		public Def(string upgradeID) : base(upgradeID)
		{
		}
	}

	// Token: 0x02000C4D RID: 3149
	public class ActiveStates : GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State
	{
		// Token: 0x04002939 RID: 10553
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State gatheringData;

		// Token: 0x0400293A RID: 10554
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State discover;
	}

	// Token: 0x02000C4E RID: 3150
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06003C58 RID: 15448 RVA: 0x000C6FE2 File Offset: 0x000C51E2
		public bool IsReadyToDiscover
		{
			get
			{
				return this.explorerBooster != null && this.explorerBooster.IsReady;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x000C6FF9 File Offset: 0x000C51F9
		public float CurrentProgress
		{
			get
			{
				if (this.explorerBooster != null)
				{
					return this.explorerBooster.Progress;
				}
				return 0f;
			}
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x000C7014 File Offset: 0x000C5214
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBoosterMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x0022DC18 File Offset: 0x0022BE18
		public void Initialize()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgrade_ExplorerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_ExplorerBooster.Instance>();
					if (smi != null && !smi.IsBeingMonitored)
					{
						this.explorerBooster = smi;
						smi.SetMonitor(this);
						return;
					}
				}
			}
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x000C701E File Offset: 0x000C521E
		protected override void OnCleanUp()
		{
			if (this.explorerBooster != null)
			{
				this.explorerBooster.SetMonitor(null);
			}
			base.OnCleanUp();
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x0022DC78 File Offset: 0x0022BE78
		public void GatheringDataUpdate(float dt)
		{
			bool isReadyToDiscover = this.IsReadyToDiscover;
			float dataProgressDelta = (dt == 0f) ? 0f : (dt / 600f);
			this.explorerBooster.AddData(dataProgressDelta);
			if (this.IsReadyToDiscover && !isReadyToDiscover)
			{
				base.sm.ReadyToDiscoverSignal.Trigger(this);
			}
		}

		// Token: 0x06003C5E RID: 15454 RVA: 0x000C703A File Offset: 0x000C523A
		public void ConsumeAllData()
		{
			this.explorerBooster.SetDataProgress(0f);
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x0022DCCC File Offset: 0x0022BECC
		public Notification GetGeyserDiscoveredNotification()
		{
			return new Notification(DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_NAME, NotificationType.MessageImportant, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x06003C60 RID: 15456 RVA: 0x000C704C File Offset: 0x000C524C
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0022DD18 File Offset: 0x0022BF18
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x0400293B RID: 10555
		private BionicUpgrade_ExplorerBooster.Instance explorerBooster;
	}
}
