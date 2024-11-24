using System;
using System.Collections.Generic;
using Database;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

// Token: 0x020019B3 RID: 6579
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x060088FD RID: 35069 RVA: 0x000F9AC3 File Offset: 0x000F7CC3
	// (set) Token: 0x060088FE RID: 35070 RVA: 0x000F9ACA File Offset: 0x000F7CCA
	public static StoryManager Instance { get; private set; }

	// Token: 0x060088FF RID: 35071 RVA: 0x000F9AD2 File Offset: 0x000F7CD2
	public static IReadOnlyList<StoryManager.StoryTelemetry> GetTelemetry()
	{
		return StoryManager.storyTelemetry;
	}

	// Token: 0x06008900 RID: 35072 RVA: 0x00355AD8 File Offset: 0x00353CD8
	protected override void OnPrefabInit()
	{
		StoryManager.Instance = this;
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06008901 RID: 35073 RVA: 0x00355B30 File Offset: 0x00353D30
	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06008902 RID: 35074 RVA: 0x00355B80 File Offset: 0x00353D80
	public void InitialSaveSetup()
	{
		this.highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinate();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitTemplate in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitTemplate);
				this.CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		this.LogInitialSaveSetup();
	}

	// Token: 0x06008903 RID: 35075 RVA: 0x00355C44 File Offset: 0x00353E44
	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return this.CreateStory(story, worldId);
	}

	// Token: 0x06008904 RID: 35076 RVA: 0x00355C6C File Offset: 0x00353E6C
	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		this._stories.Add(story.HashId, storyInstance);
		StoryManager.InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			this.BeginStoryEvent(story);
		}
		return storyInstance;
	}

	// Token: 0x06008905 RID: 35077 RVA: 0x000F9AD9 File Offset: 0x000F7CD9
	public StoryInstance GetStoryInstance(Story story)
	{
		return this.GetStoryInstance(story.HashId);
	}

	// Token: 0x06008906 RID: 35078 RVA: 0x00355CAC File Offset: 0x00353EAC
	public StoryInstance GetStoryInstance(int hash)
	{
		StoryInstance result;
		this._stories.TryGetValue(hash, out result);
		return result;
	}

	// Token: 0x06008907 RID: 35079 RVA: 0x000F9AE7 File Offset: 0x000F7CE7
	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return this._stories;
	}

	// Token: 0x06008908 RID: 35080 RVA: 0x000F9AEF File Offset: 0x000F7CEF
	public int GetHighestCoordinate()
	{
		return this.highestStoryCoordinateWhenGenerated;
	}

	// Token: 0x06008909 RID: 35081 RVA: 0x000F9AF7 File Offset: 0x000F7CF7
	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	// Token: 0x0600890A RID: 35082 RVA: 0x000F9B04 File Offset: 0x000F7D04
	public void ForceCreateStory(Story story, int worldId)
	{
		if (this.GetStoryInstance(story.HashId) == null)
		{
			this.CreateStory(story, worldId);
		}
	}

	// Token: 0x0600890B RID: 35083 RVA: 0x00355CCC File Offset: 0x00353ECC
	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.DISCOVERED, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
	}

	// Token: 0x0600890C RID: 35084 RVA: 0x00355CFC File Offset: 0x00353EFC
	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
	}

	// Token: 0x0600890D RID: 35085 RVA: 0x000F9B1D File Offset: 0x000F7D1D
	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (this.GetStoryInstance(story.HashId) == null || this.CheckState(StoryInstance.State.COMPLETE, story))
		{
			return;
		}
		FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
	}

	// Token: 0x0600890E RID: 35086 RVA: 0x00355D2C File Offset: 0x00353F2C
	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
		if (prefab != null)
		{
			keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = Util.KInstantiate(prefab, keepsakeSpawnPosition);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
		storyInstance.CurrentState = StoryInstance.State.COMPLETE;
		Game.Instance.unlocks.Unlock(this.GetCompleteUnlockId(story.Id), true);
	}

	// Token: 0x0600890F RID: 35087 RVA: 0x00355DCC File Offset: 0x00353FCC
	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.CurrentState >= state;
	}

	// Token: 0x06008910 RID: 35088 RVA: 0x000F9B3F File Offset: 0x000F7D3F
	public bool IsStoryComplete(Story story)
	{
		return this.CheckState(StoryInstance.State.COMPLETE, story);
	}

	// Token: 0x06008911 RID: 35089 RVA: 0x000F9B49 File Offset: 0x000F7D49
	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(this.GetCompleteUnlockId(story.Id));
	}

	// Token: 0x06008912 RID: 35090 RVA: 0x00355DF8 File Offset: 0x00353FF8
	public StoryInstance DisplayPopup(Story story, StoryManager.PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		if (info.extraButtons != null && info.extraButtons.Length != 0)
		{
			foreach (StoryManager.ExtraButtonInfo extraButtonInfo in info.extraButtons)
			{
				eventInfoData.SimpleOption(extraButtonInfo.ButtonText, extraButtonInfo.OnButtonClick).tooltip = extraButtonInfo.ButtonToolTip;
			}
		}
		Notification notification = null;
		if (!info.DisplayImmediate)
		{
			notification = EventInfoScreen.CreateNotification(eventInfoData, notificationCB);
		}
		storyInstance.SetPopupData(info, eventInfoData, notification);
		return storyInstance;
	}

	// Token: 0x06008913 RID: 35091 RVA: 0x00355EC8 File Offset: 0x003540C8
	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.HasDisplayedPopup(type);
	}

	// Token: 0x06008914 RID: 35092 RVA: 0x00355EF0 File Offset: 0x003540F0
	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryManager.StoryCreationTelemetry[] array = new StoryManager.StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> keyValuePair in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryManager.StoryCreationTelemetry
			{
				StoryId = keyValuePair.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(keyValuePair.Key, keyValuePair.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	// Token: 0x06008915 RID: 35093 RVA: 0x000F9B66 File Offset: 0x000F7D66
	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", this.highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", StoryManager.storyTelemetry);
	}

	// Token: 0x06008916 RID: 35094 RVA: 0x00355F94 File Offset: 0x00354194
	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (world == null)
		{
			return;
		}
		story.Telemetry.StoryId = story.storyId;
		story.Telemetry.WorldId = world.worldName;
		StoryManager.storyTelemetry.Add(story.Telemetry);
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x00355FF0 File Offset: 0x003541F0
	private void OnGameLoaded(object _)
	{
		StoryManager.storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in this._stories)
		{
			StoryManager.InitTelemetry(keyValuePair.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair2 in this._stories)
		{
			SettingConfig config;
			if (keyValuePair2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(keyValuePair2.Value.storyId, out config))
			{
				CustomGameSettings.Instance.SetStorySetting(config, true);
			}
		}
	}

	// Token: 0x06008918 RID: 35096 RVA: 0x000F9B8E File Offset: 0x000F7D8E
	public static void DestroyInstance()
	{
		StoryManager.storyTelemetry.Clear();
		StoryManager.Instance = null;
	}

	// Token: 0x04006721 RID: 26401
	public const int BEFORE_STORIES = -2;

	// Token: 0x04006723 RID: 26403
	private static List<StoryManager.StoryTelemetry> storyTelemetry = new List<StoryManager.StoryTelemetry>();

	// Token: 0x04006724 RID: 26404
	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	// Token: 0x04006725 RID: 26405
	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	// Token: 0x04006726 RID: 26406
	private const string STORY_TRAIT_KEY = "StoryTraits";

	// Token: 0x04006727 RID: 26407
	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	// Token: 0x04006728 RID: 26408
	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	// Token: 0x020019B4 RID: 6580
	public struct ExtraButtonInfo
	{
		// Token: 0x04006729 RID: 26409
		public string ButtonText;

		// Token: 0x0400672A RID: 26410
		public string ButtonToolTip;

		// Token: 0x0400672B RID: 26411
		public System.Action OnButtonClick;
	}

	// Token: 0x020019B5 RID: 6581
	public struct PopupInfo
	{
		// Token: 0x0400672C RID: 26412
		public string Title;

		// Token: 0x0400672D RID: 26413
		public string Description;

		// Token: 0x0400672E RID: 26414
		public string CloseButtonText;

		// Token: 0x0400672F RID: 26415
		public string CloseButtonToolTip;

		// Token: 0x04006730 RID: 26416
		public StoryManager.ExtraButtonInfo[] extraButtons;

		// Token: 0x04006731 RID: 26417
		public string TextureName;

		// Token: 0x04006732 RID: 26418
		public GameObject[] Minions;

		// Token: 0x04006733 RID: 26419
		public bool DisplayImmediate;

		// Token: 0x04006734 RID: 26420
		public EventInfoDataHelper.PopupType PopupType;
	}

	// Token: 0x020019B6 RID: 6582
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		// Token: 0x0600891B RID: 35099 RVA: 0x003560DC File Offset: 0x003542DC
		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				this.Retrofitted = ((this.Retrofitted >= 0f) ? this.Retrofitted : time);
				return;
			case StoryInstance.State.NOT_STARTED:
				break;
			case StoryInstance.State.DISCOVERED:
				this.Discovered = ((this.Discovered >= 0f) ? this.Discovered : time);
				return;
			case StoryInstance.State.IN_PROGRESS:
				this.InProgress = ((this.InProgress >= 0f) ? this.InProgress : time);
				return;
			case StoryInstance.State.COMPLETE:
				this.Completed = ((this.Completed >= 0f) ? this.Completed : time);
				break;
			default:
				return;
			}
		}

		// Token: 0x04006735 RID: 26421
		public string StoryId;

		// Token: 0x04006736 RID: 26422
		public string WorldId;

		// Token: 0x04006737 RID: 26423
		[Serialize]
		public float Retrofitted = -1f;

		// Token: 0x04006738 RID: 26424
		[Serialize]
		public float Discovered = -1f;

		// Token: 0x04006739 RID: 26425
		[Serialize]
		public float InProgress = -1f;

		// Token: 0x0400673A RID: 26426
		[Serialize]
		public float Completed = -1f;
	}

	// Token: 0x020019B7 RID: 6583
	public class StoryCreationTelemetry
	{
		// Token: 0x0400673B RID: 26427
		public string StoryId;

		// Token: 0x0400673C RID: 26428
		public bool Enabled;
	}
}
