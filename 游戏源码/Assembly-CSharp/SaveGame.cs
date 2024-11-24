using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001809 RID: 6153
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06007EE8 RID: 32488 RVA: 0x000F3A3B File Offset: 0x000F1C3B
	// (set) Token: 0x06007EE9 RID: 32489 RVA: 0x000F3A43 File Offset: 0x000F1C43
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

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06007EEA RID: 32490 RVA: 0x000F3A4C File Offset: 0x000F1C4C
	// (set) Token: 0x06007EEB RID: 32491 RVA: 0x000F3A54 File Offset: 0x000F1C54
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

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06007EEC RID: 32492 RVA: 0x000F3A5D File Offset: 0x000F1C5D
	public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	// Token: 0x06007EED RID: 32493 RVA: 0x000F3A65 File Offset: 0x000F1C65
	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06007EEE RID: 32494 RVA: 0x000F3A6D File Offset: 0x000F1C6D
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

	// Token: 0x06007EEF RID: 32495 RVA: 0x0032CEFC File Offset: 0x0032B0FC
	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	// Token: 0x06007EF0 RID: 32496 RVA: 0x000F3A8F File Offset: 0x000F1C8F
	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	// Token: 0x06007EF1 RID: 32497 RVA: 0x000F3AA1 File Offset: 0x000F1CA1
	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	// Token: 0x06007EF2 RID: 32498 RVA: 0x000F3AB8 File Offset: 0x000F1CB8
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06007EF3 RID: 32499 RVA: 0x0032CF54 File Offset: 0x0032B154
	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 642695U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	// Token: 0x06007EF4 RID: 32500 RVA: 0x000F3AC0 File Offset: 0x000F1CC0
	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	// Token: 0x06007EF5 RID: 32501 RVA: 0x0032D018 File Offset: 0x0032B218
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

	// Token: 0x06007EF6 RID: 32502 RVA: 0x0032D070 File Offset: 0x0032B270
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

	// Token: 0x06007EF7 RID: 32503 RVA: 0x000F3AFD File Offset: 0x000F1CFD
	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	// Token: 0x06007EF8 RID: 32504 RVA: 0x000F3B0F File Offset: 0x000F1D0F
	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	// Token: 0x06007EF9 RID: 32505 RVA: 0x000F3B2B File Offset: 0x000F1D2B
	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	// Token: 0x06007EFA RID: 32506 RVA: 0x0032D16C File Offset: 0x0032B36C
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

	// Token: 0x0400602E RID: 24622
	[Serialize]
	private int speed;

	// Token: 0x0400602F RID: 24623
	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	// Token: 0x04006030 RID: 24624
	[Serialize]
	public int minGermCountForDisinfect = 10000;

	// Token: 0x04006031 RID: 24625
	[Serialize]
	public bool enableAutoDisinfect = true;

	// Token: 0x04006032 RID: 24626
	[Serialize]
	public bool sandboxEnabled;

	// Token: 0x04006033 RID: 24627
	[Serialize]
	public float relativeTemperatureOverlaySliderValue = 294.15f;

	// Token: 0x04006034 RID: 24628
	[Serialize]
	private int autoSaveCycleInterval = 1;

	// Token: 0x04006035 RID: 24629
	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	// Token: 0x04006036 RID: 24630
	private string baseName;

	// Token: 0x04006037 RID: 24631
	public static SaveGame Instance;

	// Token: 0x04006038 RID: 24632
	private ColonyAchievementTracker colonyAchievementTracker;

	// Token: 0x04006039 RID: 24633
	public EntombedItemManager entombedItemManager;

	// Token: 0x0400603A RID: 24634
	public WorldGenSpawner worldGenSpawner;

	// Token: 0x0400603B RID: 24635
	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	// Token: 0x0400603C RID: 24636
	private static bool debug_SaveFileHeaderBlank_sent;

	// Token: 0x0200180A RID: 6154
	public struct Header
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06007EFC RID: 32508 RVA: 0x000F3B48 File Offset: 0x000F1D48
		public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		// Token: 0x0400603D RID: 24637
		public uint buildVersion;

		// Token: 0x0400603E RID: 24638
		public int headerSize;

		// Token: 0x0400603F RID: 24639
		public uint headerVersion;

		// Token: 0x04006040 RID: 24640
		public int compression;
	}

	// Token: 0x0200180B RID: 6155
	public struct GameInfo
	{
		// Token: 0x06007EFD RID: 32509 RVA: 0x0032D4D0 File Offset: 0x0032B6D0
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
			this.saveMinorVersion = 35;
		}

		// Token: 0x06007EFE RID: 32510 RVA: 0x000F3B53 File Offset: 0x000F1D53
		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x000F3B74 File Offset: 0x000F1D74
		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x0032D540 File Offset: 0x0032B740
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

		// Token: 0x04006041 RID: 24641
		public int numberOfCycles;

		// Token: 0x04006042 RID: 24642
		public int numberOfDuplicants;

		// Token: 0x04006043 RID: 24643
		public string baseName;

		// Token: 0x04006044 RID: 24644
		public bool isAutoSave;

		// Token: 0x04006045 RID: 24645
		public string originalSaveName;

		// Token: 0x04006046 RID: 24646
		public int saveMajorVersion;

		// Token: 0x04006047 RID: 24647
		public int saveMinorVersion;

		// Token: 0x04006048 RID: 24648
		public string clusterId;

		// Token: 0x04006049 RID: 24649
		public string[] worldTraits;

		// Token: 0x0400604A RID: 24650
		public bool sandboxEnabled;

		// Token: 0x0400604B RID: 24651
		public Guid colonyGuid;

		// Token: 0x0400604C RID: 24652
		[Obsolete("Please use dlcIds instead.")]
		public string dlcId;

		// Token: 0x0400604D RID: 24653
		public List<string> dlcIds;
	}
}
