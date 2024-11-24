using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010A7 RID: 4263
[AddComponentMenu("KMonoBehaviour/scripts/ColonyAchievementTracker")]
public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, IRenderEveryTick
{
	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06005773 RID: 22387 RVA: 0x000D911F File Offset: 0x000D731F
	// (set) Token: 0x06005774 RID: 22388 RVA: 0x000D912C File Offset: 0x000D732C
	public bool GeothermalFacilityDiscovered
	{
		get
		{
			return (this.geothermalProgress & 1) == 1;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 1;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -2;
		}
	}

	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06005775 RID: 22389 RVA: 0x000D915B File Offset: 0x000D735B
	// (set) Token: 0x06005776 RID: 22390 RVA: 0x000D9168 File Offset: 0x000D7368
	public bool GeothermalControllerRepaired
	{
		get
		{
			return (this.geothermalProgress & 2) == 2;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 2;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -3;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06005777 RID: 22391 RVA: 0x000D9197 File Offset: 0x000D7397
	// (set) Token: 0x06005778 RID: 22392 RVA: 0x000D91A4 File Offset: 0x000D73A4
	public bool GeothermalControllerHasVented
	{
		get
		{
			return (this.geothermalProgress & 4) == 4;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 4;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -5;
		}
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06005779 RID: 22393 RVA: 0x000D91D3 File Offset: 0x000D73D3
	// (set) Token: 0x0600577A RID: 22394 RVA: 0x000D91E0 File Offset: 0x000D73E0
	public bool GeothermalClearedEntombedVent
	{
		get
		{
			return (this.geothermalProgress & 8) == 8;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 8;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -9;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x0600577B RID: 22395 RVA: 0x000D920F File Offset: 0x000D740F
	// (set) Token: 0x0600577C RID: 22396 RVA: 0x000D921E File Offset: 0x000D741E
	public bool GeothermalVictoryPopupDismissed
	{
		get
		{
			return (this.geothermalProgress & 16) == 16;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 16;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -17;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x0600577D RID: 22397 RVA: 0x000D924E File Offset: 0x000D744E
	public List<string> achievementsToDisplay
	{
		get
		{
			return this.completedAchievementsToDisplay;
		}
	}

	// Token: 0x0600577E RID: 22398 RVA: 0x000D9256 File Offset: 0x000D7456
	public void ClearDisplayAchievements()
	{
		this.achievementsToDisplay.Clear();
	}

	// Token: 0x0600577F RID: 22399 RVA: 0x00286340 File Offset: 0x00284540
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (!this.achievements.ContainsKey(colonyAchievement.Id))
			{
				ColonyAchievementStatus value = new ColonyAchievementStatus(colonyAchievement.Id);
				this.achievements.Add(colonyAchievement.Id, value);
			}
		}
		this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new Action<object>(this.CheckAchievements));
		base.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
		this.UpgradeTamedCritterAchievements();
	}

	// Token: 0x06005780 RID: 22400 RVA: 0x00286404 File Offset: 0x00284604
	private void UpgradeTamedCritterAchievements()
	{
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in Db.Get().ColonyAchievements.TameAllBasicCritters.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits = colonyAchievementRequirement as CritterTypesWithTraits;
			if (critterTypesWithTraits != null)
			{
				critterTypesWithTraits.UpdateSavedState();
			}
		}
		foreach (ColonyAchievementRequirement colonyAchievementRequirement2 in Db.Get().ColonyAchievements.TameAGassyMoo.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits2 = colonyAchievementRequirement2 as CritterTypesWithTraits;
			if (critterTypesWithTraits2 != null)
			{
				critterTypesWithTraits2.UpdateSavedState();
			}
		}
	}

	// Token: 0x06005781 RID: 22401 RVA: 0x002864C4 File Offset: 0x002846C4
	public void RenderEveryTick(float dt)
	{
		if (this.updatingAchievement >= this.achievements.Count)
		{
			this.updatingAchievement = 0;
		}
		KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt(this.updatingAchievement);
		this.updatingAchievement++;
		if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
		{
			keyValuePair.Value.UpdateAchievement();
			if (keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
				this.completedAchievementsToDisplay.Add(keyValuePair.Key);
				this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				RetireColonyUtility.SaveColonySummaryData();
			}
		}
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x00286584 File Offset: 0x00284784
	private void CheckAchievements(object data = null)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				keyValuePair.Value.UpdateAchievement();
				if (keyValuePair.Value.success && !keyValuePair.Value.failed)
				{
					ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
					this.completedAchievementsToDisplay.Add(keyValuePair.Key);
					this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				}
			}
		}
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x06005783 RID: 22403 RVA: 0x0028664C File Offset: 0x0028484C
	private static void UnlockPlatformAchievement(string achievement_id)
	{
		if (DebugHandler.InstantBuildMode)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (SaveGame.Instance.sandboxEnabled)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (Game.Instance.debugWasUsed)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", new object[]
			{
				achievement_id
			});
			return;
		}
		ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
		if (colonyAchievement != null && !string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
		{
			if (SteamAchievementService.Instance)
			{
				SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
				return;
			}
			global::Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", new object[]
			{
				colonyAchievement.platformAchievementId
			});
		}
	}

	// Token: 0x06005784 RID: 22404 RVA: 0x000D9263 File Offset: 0x000D7463
	public void DebugTriggerAchievement(string id)
	{
		this.achievements[id].failed = false;
		this.achievements[id].success = true;
	}

	// Token: 0x06005785 RID: 22405 RVA: 0x00286710 File Offset: 0x00284910
	private void BeginVictorySequence(string achievementID)
	{
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
		this.ToggleVictoryUI(true);
		StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<StoryMessageScreen>();
		component.restoreInterfaceOnClose = false;
		component.title = COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
		component.body = string.Format(COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, "<b>" + Db.Get().ColonyAchievements.Get(achievementID).Name + "</b>\n" + Db.Get().ColonyAchievements.Get(achievementID).description);
		component.Show(true);
		CameraController.Instance.SetWorldInteractive(false);
		component.OnClose = (System.Action)Delegate.Combine(component.OnClose, new System.Action(delegate()
		{
			SpeedControlScreen.Instance.SetSpeed(1);
			if (!SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			CameraController.Instance.SetWorldInteractive(true);
			Db.Get().ColonyAchievements.Get(achievementID).victorySequence(this);
		}));
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x00286854 File Offset: 0x00284A54
	public bool IsAchievementUnlocked(ColonyAchievement achievement)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (keyValuePair.Key == achievement.Id)
			{
				if (keyValuePair.Value.success)
				{
					return true;
				}
				keyValuePair.Value.UpdateAchievement();
				return keyValuePair.Value.success;
			}
		}
		return false;
	}

	// Token: 0x06005787 RID: 22407 RVA: 0x000D9289 File Offset: 0x000D7489
	protected override void OnCleanUp()
	{
		this.victorySchedulerHandle.ClearScheduler();
		Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
		this.checkAchievementsHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x002868E4 File Offset: 0x00284AE4
	private void TriggerNewAchievementCompleted(string achievement, GameObject cameraTarget = null)
	{
		this.unlockedAchievementMetric[ColonyAchievementTracker.UnlockedAchievementKey] = achievement;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedAchievementMetric, "TriggerNewAchievementCompleted");
		bool flag = false;
		if (Db.Get().ColonyAchievements.Get(achievement).isVictoryCondition)
		{
			flag = true;
			this.BeginVictorySequence(achievement);
		}
		if (!flag)
		{
			AchievementEarnedMessage message = new AchievementEarnedMessage();
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x00286950 File Offset: 0x00284B50
	private void ToggleVictoryUI(bool victoryUIActive)
	{
		List<KScreen> list = new List<KScreen>();
		list.Add(NotificationScreen.Instance);
		list.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			list.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			list.Add(BuildMenu.Instance);
		}
		list.Add(ManagementMenu.Instance);
		list.Add(ToolMenu.Instance);
		list.Add(ToolMenu.Instance.PriorityScreen);
		list.Add(ResourceCategoryScreen.Instance);
		list.Add(TopLeftControlScreen.Instance);
		list.Add(global::DateTime.Instance);
		list.Add(BuildWatermark.Instance);
		list.Add(HoverTextScreen.Instance);
		list.Add(DetailsScreen.Instance);
		list.Add(DebugPaintElementScreen.Instance);
		list.Add(DebugBaseTemplateButton.Instance);
		list.Add(StarmapScreen.Instance);
		foreach (KScreen kscreen in list)
		{
			if (kscreen != null)
			{
				kscreen.Show(!victoryUIActive);
			}
		}
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x00286A80 File Offset: 0x00284C80
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.achievements.Count);
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			writer.WriteKleiString(keyValuePair.Key);
			keyValuePair.Value.Serialize(writer);
		}
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x00286AF8 File Offset: 0x00284CF8
	public void Deserialize(IReader reader)
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
		{
			return;
		}
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadKleiString();
			ColonyAchievementStatus value = ColonyAchievementStatus.Deserialize(reader, text);
			if (Db.Get().ColonyAchievements.Exists(text))
			{
				this.achievements.Add(text, value);
			}
		}
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x00286B60 File Offset: 0x00284D60
	public void LogFetchChore(GameObject fetcher, ChoreType choreType)
	{
		if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch || choreType == Db.Get().ChoreTypes.Transport)
		{
			return;
		}
		Dictionary<int, int> dictionary = null;
		if (fetcher.GetComponent<SolidTransferArm>() != null)
		{
			dictionary = this.fetchAutomatedChoreDeliveries;
		}
		else if (fetcher.GetComponent<MinionIdentity>() != null)
		{
			dictionary = this.fetchDupeChoreDeliveries;
		}
		if (dictionary != null)
		{
			int cycle = GameClock.Instance.GetCycle();
			if (!dictionary.ContainsKey(cycle))
			{
				dictionary.Add(cycle, 0);
			}
			Dictionary<int, int> dictionary2 = dictionary;
			int key = cycle;
			int num = dictionary2[key];
			dictionary2[key] = num + 1;
		}
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x000D92B7 File Offset: 0x000D74B7
	public void LogCritterTamed(Tag prefabId)
	{
		this.tamedCritterTypes.Add(prefabId);
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x00286C2C File Offset: 0x00284E2C
	public void LogSuitChore(ChoreDriver driver)
	{
		if (driver == null || driver.GetComponent<MinionIdentity>() == null)
		{
			return;
		}
		bool flag = false;
		foreach (AssignableSlotInstance assignableSlotInstance in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().IsAnyPrefabID(ColonyAchievementTracker.SuitTags))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			int cycle = GameClock.Instance.GetCycle();
			int instanceID = driver.GetComponent<KPrefabID>().InstanceID;
			if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
			{
				this.dupesCompleteChoresInSuits.Add(cycle, new List<int>
				{
					instanceID
				});
				return;
			}
			if (!this.dupesCompleteChoresInSuits[cycle].Contains(instanceID))
			{
				this.dupesCompleteChoresInSuits[cycle].Add(instanceID);
			}
		}
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x000D92C6 File Offset: 0x000D74C6
	public void LogAnalyzedSeed(Tag seed)
	{
		this.analyzedSeeds.Add(seed);
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x00286D34 File Offset: 0x00284F34
	public void OnNewDay(object data)
	{
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			if (minionStorage.GetComponent<CommandModule>() != null)
			{
				List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					int cycle = GameClock.Instance.GetCycle();
					if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
					{
						this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
					}
					for (int i = 0; i < storedMinionInfo.Count; i++)
					{
						KPrefabID kprefabID = storedMinionInfo[i].serializedMinion.Get();
						if (kprefabID != null)
						{
							this.dupesCompleteChoresInSuits[cycle].Add(kprefabID.InstanceID);
						}
					}
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = Db.Get().ColonyAchievements.SurviveInARocket.requirementChecklist[0] as SurviveARocketWithMinimumMorale;
			if (surviveARocketWithMinimumMorale != null)
			{
				float minimumMorale = surviveARocketWithMinimumMorale.minimumMorale;
				int numberOfCycles = surviveARocketWithMinimumMorale.numberOfCycles;
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					if (worldContainer.IsModuleInterior)
					{
						if (!this.cyclesRocketDupeMoraleAboveRequirement.ContainsKey(worldContainer.id))
						{
							this.cyclesRocketDupeMoraleAboveRequirement.Add(worldContainer.id, 0);
						}
						if (worldContainer.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
						{
							List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(worldContainer.id, false);
							bool flag = worldItems.Count > 0;
							foreach (MinionIdentity cmp in worldItems)
							{
								if (Db.Get().Attributes.QualityOfLife.Lookup(cmp).GetTotalValue() < minimumMorale)
								{
									flag = false;
									break;
								}
							}
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = (flag ? (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] + 1) : 0);
						}
						else if (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] < numberOfCycles)
						{
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = 0;
						}
					}
				}
			}
		}
	}

	// Token: 0x04003D0B RID: 15627
	public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();

	// Token: 0x04003D0C RID: 15628
	[Serialize]
	public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04003D0D RID: 15629
	[Serialize]
	public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04003D0E RID: 15630
	[Serialize]
	public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();

	// Token: 0x04003D0F RID: 15631
	[Serialize]
	public HashSet<Tag> tamedCritterTypes = new HashSet<Tag>();

	// Token: 0x04003D10 RID: 15632
	[Serialize]
	public bool defrostedDuplicant;

	// Token: 0x04003D11 RID: 15633
	[Serialize]
	public HashSet<Tag> analyzedSeeds = new HashSet<Tag>();

	// Token: 0x04003D12 RID: 15634
	[Serialize]
	public float totalMaterialsHarvestFromPOI;

	// Token: 0x04003D13 RID: 15635
	[Serialize]
	public float radBoltTravelDistance;

	// Token: 0x04003D14 RID: 15636
	[Serialize]
	public bool harvestAHiveWithoutGettingStung;

	// Token: 0x04003D15 RID: 15637
	[Serialize]
	public Dictionary<int, int> cyclesRocketDupeMoraleAboveRequirement = new Dictionary<int, int>();

	// Token: 0x04003D16 RID: 15638
	[Serialize]
	private int geothermalProgress;

	// Token: 0x04003D17 RID: 15639
	private const int GEO_DISCOVERED_BIT = 1;

	// Token: 0x04003D18 RID: 15640
	private const int GEO_CONTROLLER_REPAIRED_BIT = 2;

	// Token: 0x04003D19 RID: 15641
	private const int GEO_CONTROLLER_VENTED_BIT = 4;

	// Token: 0x04003D1A RID: 15642
	private const int GEO_CLEARED_ENTOMBED_BIT = 8;

	// Token: 0x04003D1B RID: 15643
	private const int GEO_VICTORY_ACK_BIT = 16;

	// Token: 0x04003D1C RID: 15644
	private SchedulerHandle checkAchievementsHandle;

	// Token: 0x04003D1D RID: 15645
	private int forceCheckAchievementHandle = -1;

	// Token: 0x04003D1E RID: 15646
	[Serialize]
	private int updatingAchievement;

	// Token: 0x04003D1F RID: 15647
	[Serialize]
	private List<string> completedAchievementsToDisplay = new List<string>();

	// Token: 0x04003D20 RID: 15648
	private SchedulerHandle victorySchedulerHandle;

	// Token: 0x04003D21 RID: 15649
	public static readonly string UnlockedAchievementKey = "UnlockedAchievement";

	// Token: 0x04003D22 RID: 15650
	private Dictionary<string, object> unlockedAchievementMetric = new Dictionary<string, object>
	{
		{
			ColonyAchievementTracker.UnlockedAchievementKey,
			null
		}
	};

	// Token: 0x04003D23 RID: 15651
	private static readonly Tag[] SuitTags = new Tag[]
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x04003D24 RID: 15652
	private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>(delegate(ColonyAchievementTracker component, object data)
	{
		component.OnNewDay(data);
	});
}
