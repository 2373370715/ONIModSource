﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
			public int AutoSaveCycleInterval
	{
		get
		{
			return this.autoSaveCycleInterval;
		}
		set
		{
			this.autoSaveCycleInterval = value;
		}
	}

			public Vector2I TimelapseResolution
	{
		get
		{
			return this.timelapseResolution;
		}
		set
		{
			this.timelapseResolution = value;
		}
	}

		public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

		public ColonyAchievementTracker ColonyAchievementTracker
	{
		get
		{
			if (this.colonyAchievementTracker == null)
			{
				this.colonyAchievementTracker = base.GetComponent<ColonyAchievementTracker>();
			}
			return this.colonyAchievementTracker;
		}
	}

	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	public int GetSpeed()
	{
		return this.speed;
	}

	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 626616U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	public static global::Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
	{
		try
		{
			SaveGame.Header a;
			SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out a);
			if (gameInfo.saveMajorVersion >= 7)
			{
				return new global::Tuple<SaveGame.Header, SaveGame.GameInfo>(a, gameInfo);
			}
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Exception while loading " + filename);
			global::Debug.LogWarning(obj);
		}
		return null;
	}

	public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header, string debugFileName)
	{
		header = default(SaveGame.Header);
		header.buildVersion = br.ReadUInt32();
		header.headerSize = br.ReadInt32();
		header.headerVersion = br.ReadUInt32();
		if (1U <= header.headerVersion)
		{
			header.compression = br.ReadInt32();
		}
		byte[] data = br.ReadBytes(header.headerSize);
		if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
		{
			SaveGame.debug_SaveFileHeaderBlank_sent = true;
			global::Debug.LogWarning("SaveFileHeaderBlank - " + debugFileName);
		}
		SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
		if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
		{
			string[] worldTraits = gameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				worldTraits[i] = worldTraits[i].Replace('\\', '/');
			}
		}
		if (gameInfo.IsVersionOlderThan(7, 20))
		{
			gameInfo.dlcId = "";
		}
		if (gameInfo.IsVersionOlderThan(7, 34))
		{
			gameInfo.dlcIds = new List<string>
			{
				gameInfo.dlcId
			};
		}
		return gameInfo;
	}

	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	public List<global::Tuple<string, TextStyleSetting>> GetColonyToolTip()
	{
		List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterLayout;
		SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
		list.Add(new global::Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		if (DlcManager.IsExpansion1Active())
		{
			StringEntry entry = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry, ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		if (GameClock.Instance != null)
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.CYCLES_OLD, GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.TIME_PLAYED, (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
		WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
		list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		else
		{
			StringEntry entry2 = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry2, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
		{
			string[] worldTraits = SaveLoader.Instance.GameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraits[i], false);
				if (cachedWorldTrait != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
		}
		else if (world.WorldTraitIds != null)
		{
			foreach (string name in world.WorldTraitIds)
			{
				WorldTrait cachedWorldTrait2 = SettingsCache.GetCachedWorldTrait(name, false);
				if (cachedWorldTrait2 != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait2.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
			if (world.WorldTraitIds.Count == 0)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
		}
		return list;
	}

	[Serialize]
	private int speed;

	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	[Serialize]
	public int minGermCountForDisinfect = 10000;

	[Serialize]
	public bool enableAutoDisinfect = true;

	[Serialize]
	public bool sandboxEnabled;

	[Serialize]
	public float relativeTemperatureOverlaySliderValue = 294.15f;

	[Serialize]
	private int autoSaveCycleInterval = 1;

	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	private string baseName;

	public static SaveGame Instance;

	private ColonyAchievementTracker colonyAchievementTracker;

	public EntombedItemManager entombedItemManager;

	public WorldGenSpawner worldGenSpawner;

	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	private static bool debug_SaveFileHeaderBlank_sent;

	public struct Header
	{
				public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		public uint buildVersion;

		public int headerSize;

		public uint headerVersion;

		public int compression;
	}

	public struct GameInfo
	{
		public GameInfo(int numberOfCycles, int numberOfDuplicants, string baseName, bool isAutoSave, string originalSaveName, string clusterId, string[] worldTraits, Guid colonyGuid, List<string> dlcIds, bool sandboxEnabled = false)
		{
			this.numberOfCycles = numberOfCycles;
			this.numberOfDuplicants = numberOfDuplicants;
			this.baseName = baseName;
			this.isAutoSave = isAutoSave;
			this.originalSaveName = originalSaveName;
			this.clusterId = clusterId;
			this.worldTraits = worldTraits;
			this.colonyGuid = colonyGuid;
			this.sandboxEnabled = sandboxEnabled;
			this.dlcIds = dlcIds;
			this.dlcId = null;
			this.saveMajorVersion = 7;
			this.saveMinorVersion = 34;
		}

		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		public bool IsCompatableWithCurrentDlcConfiguration(out HashSet<string> dlcIdsToEnable, out HashSet<string> dlcIdToDisable)
		{
			dlcIdsToEnable = new HashSet<string>();
			foreach (string item in this.dlcIds)
			{
				if (!DlcManager.IsContentSubscribed(item))
				{
					dlcIdsToEnable.Add(item);
				}
			}
			dlcIdToDisable = new HashSet<string>();
			if (!this.dlcIds.Contains("EXPANSION1_ID") && DlcManager.IsExpansion1Active())
			{
				dlcIdToDisable.Add("EXPANSION1_ID");
			}
			return dlcIdsToEnable.Count == 0 && dlcIdToDisable.Count == 0;
		}

		public int numberOfCycles;

		public int numberOfDuplicants;

		public string baseName;

		public bool isAutoSave;

		public string originalSaveName;

		public int saveMajorVersion;

		public int saveMinorVersion;

		public string clusterId;

		public string[] worldTraits;

		public bool sandboxEnabled;

		public Guid colonyGuid;

		[Obsolete("Please use dlcIds instead.")]
		public string dlcId;

		public List<string> dlcIds;
	}
}
