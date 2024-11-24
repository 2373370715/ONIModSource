using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	public struct ExtraButtonInfo
	{
		public string ButtonText;

		public string ButtonToolTip;

		public System.Action OnButtonClick;
	}

	public struct PopupInfo
	{
		public string Title;

		public string Description;

		public string CloseButtonText;

		public string CloseButtonToolTip;

		public ExtraButtonInfo[] extraButtons;

		public string TextureName;

		public GameObject[] Minions;

		public bool DisplayImmediate;

		public EventInfoDataHelper.PopupType PopupType;
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		public string StoryId;

		public string WorldId;

		[Serialize]
		public float Retrofitted = -1f;

		[Serialize]
		public float Discovered = -1f;

		[Serialize]
		public float InProgress = -1f;

		[Serialize]
		public float Completed = -1f;

		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				Retrofitted = ((Retrofitted >= 0f) ? Retrofitted : time);
				break;
			case StoryInstance.State.DISCOVERED:
				Discovered = ((Discovered >= 0f) ? Discovered : time);
				break;
			case StoryInstance.State.IN_PROGRESS:
				InProgress = ((InProgress >= 0f) ? InProgress : time);
				break;
			case StoryInstance.State.COMPLETE:
				Completed = ((Completed >= 0f) ? Completed : time);
				break;
			case StoryInstance.State.NOT_STARTED:
				break;
			}
		}
	}

	public class StoryCreationTelemetry
	{
		public string StoryId;

		public bool Enabled;
	}

	public const int BEFORE_STORIES = -2;

	private static List<StoryTelemetry> storyTelemetry = new List<StoryTelemetry>();

	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	private const string STORY_TRAIT_KEY = "StoryTraits";

	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	public static StoryManager Instance { get; private set; }

	public static IReadOnlyList<StoryTelemetry> GetTelemetry()
	{
		return storyTelemetry;
	}

	protected override void OnPrefabInit()
	{
		Instance = this;
		GameClock.Instance.Subscribe(631075836, OnNewDayStarted);
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(OnGameLoaded));
	}

	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, OnNewDayStarted);
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(OnGameLoaded));
	}

	public void InitialSaveSetup()
	{
		highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinate();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitId in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitId);
				CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		LogInitialSaveSetup();
	}

	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return CreateStory(story, worldId);
	}

	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		_stories.Add(story.HashId, storyInstance);
		InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			BeginStoryEvent(story);
		}
		return storyInstance;
	}

	public StoryInstance GetStoryInstance(Story story)
	{
		return GetStoryInstance(story.HashId);
	}

	public StoryInstance GetStoryInstance(int hash)
	{
		_stories.TryGetValue(hash, out var value);
		return value;
	}

	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return _stories;
	}

	public int GetHighestCoordinate()
	{
		return highestStoryCoordinateWhenGenerated;
	}

	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	public void ForceCreateStory(Story story, int worldId)
	{
		if (GetStoryInstance(story.HashId) == null)
		{
			CreateStory(story, worldId);
		}
	}

	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = GetStoryInstance(story.HashId);
		if (storyInstance != null && !CheckState(StoryInstance.State.DISCOVERED, story))
		{
			storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
		}
	}

	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = GetStoryInstance(story.HashId);
		if (storyInstance != null && !CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
		}
	}

	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (GetStoryInstance(story.HashId) != null && !CheckState(StoryInstance.State.COMPLETE, story))
		{
			FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
		}
	}

	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = GetStoryInstance(story.HashId);
		if (storyInstance != null)
		{
			GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
			if (prefab != null)
			{
				keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				GameObject obj = Util.KInstantiate(prefab, keepsakeSpawnPosition);
				obj.SetActive(value: true);
				new UpgradeFX.Instance(obj.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			}
			storyInstance.CurrentState = StoryInstance.State.COMPLETE;
			Game.Instance.unlocks.Unlock(GetCompleteUnlockId(story.Id));
		}
	}

	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return false;
		}
		return storyInstance.CurrentState >= state;
	}

	public bool IsStoryComplete(Story story)
	{
		return CheckState(StoryInstance.State.COMPLETE, story);
	}

	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(GetCompleteUnlockId(story.Id));
	}

	public StoryInstance DisplayPopup(Story story, PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		if (info.extraButtons != null && info.extraButtons.Length != 0)
		{
			ExtraButtonInfo[] extraButtons = info.extraButtons;
			for (int i = 0; i < extraButtons.Length; i++)
			{
				ExtraButtonInfo extraButtonInfo = extraButtons[i];
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

	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		return GetStoryInstance(story.HashId)?.HasDisplayedPopup(type) ?? false;
	}

	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryCreationTelemetry[] array = new StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> item in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryCreationTelemetry
			{
				StoryId = item.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(item.Key, item.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", storyTelemetry);
	}

	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (!(world == null))
		{
			story.Telemetry.StoryId = story.storyId;
			story.Telemetry.WorldId = world.worldName;
			storyTelemetry.Add(story.Telemetry);
		}
	}

	private void OnGameLoaded(object _)
	{
		storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> story in _stories)
		{
			InitTelemetry(story.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> story2 in _stories)
		{
			if (story2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(story2.Value.storyId, out var value))
			{
				CustomGameSettings.Instance.SetStorySetting(value, value: true);
			}
		}
	}

	public static void DestroyInstance()
	{
		storyTelemetry.Clear();
		Instance = null;
	}
}
