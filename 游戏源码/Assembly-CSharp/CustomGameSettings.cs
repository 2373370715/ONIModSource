using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using Database;
using Klei.CustomSettings;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020011FE RID: 4606
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CustomGameSettings")]
public class CustomGameSettings : KMonoBehaviour
{
	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06005DDD RID: 24029 RVA: 0x000DD56B File Offset: 0x000DB76B
	public static CustomGameSettings Instance
	{
		get
		{
			return CustomGameSettings.instance;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06005DDE RID: 24030 RVA: 0x000DD572 File Offset: 0x000DB772
	public IReadOnlyDictionary<string, string> CurrentStoryLevelsBySetting
	{
		get
		{
			return this.currentStoryLevelsBySetting;
		}
	}

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06005DDF RID: 24031 RVA: 0x0029F938 File Offset: 0x0029DB38
	// (remove) Token: 0x06005DE0 RID: 24032 RVA: 0x0029F970 File Offset: 0x0029DB70
	public event Action<SettingConfig, SettingLevel> OnQualitySettingChanged;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06005DE1 RID: 24033 RVA: 0x0029F9A8 File Offset: 0x0029DBA8
	// (remove) Token: 0x06005DE2 RID: 24034 RVA: 0x0029F9E0 File Offset: 0x0029DBE0
	public event Action<SettingConfig, SettingLevel> OnStorySettingChanged;

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06005DE3 RID: 24035 RVA: 0x0029FA18 File Offset: 0x0029DC18
	// (remove) Token: 0x06005DE4 RID: 24036 RVA: 0x0029FA50 File Offset: 0x0029DC50
	public event Action<SettingConfig, SettingLevel> OnMixingSettingChanged;

	// Token: 0x06005DE5 RID: 24037 RVA: 0x0029FA88 File Offset: 0x0029DC88
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 6))
		{
			this.customGameMode = (this.is_custom_game ? CustomGameSettings.CustomGameMode.Custom : CustomGameSettings.CustomGameMode.Survival);
		}
		if (this.CurrentQualityLevelsBySetting.ContainsKey("CarePackages "))
		{
			if (!this.CurrentQualityLevelsBySetting.ContainsKey(CustomGameSettingConfigs.CarePackages.id))
			{
				this.CurrentQualityLevelsBySetting.Add(CustomGameSettingConfigs.CarePackages.id, this.CurrentQualityLevelsBySetting["CarePackages "]);
			}
			this.CurrentQualityLevelsBySetting.Remove("CarePackages ");
		}
		this.CurrentQualityLevelsBySetting.Remove("Expansion1Active");
		string clusterDefaultName;
		this.CurrentQualityLevelsBySetting.TryGetValue(CustomGameSettingConfigs.ClusterLayout.id, out clusterDefaultName);
		if (clusterDefaultName.IsNullOrWhiteSpace())
		{
			if (!DlcManager.IsExpansion1Active())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: ClusterLayout is blank, using default cluster instead"
				});
			}
			clusterDefaultName = WorldGenSettings.ClusterDefaultName;
			this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, clusterDefaultName);
		}
		if (!SettingsCache.clusterLayouts.clusterCache.ContainsKey(clusterDefaultName))
		{
			global::Debug.Log("Deserializing CustomGameSettings.ClusterLayout: '" + clusterDefaultName + "' doesn't exist in the clusterCache, trying to rewrite path to scoped path.");
			string text = SettingsCache.GetScope("EXPANSION1_ID") + clusterDefaultName;
			if (SettingsCache.clusterLayouts.clusterCache.ContainsKey(text))
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"Deserializing CustomGameSettings.ClusterLayout: Success in rewriting ClusterLayout '",
					clusterDefaultName,
					"' to '",
					text,
					"'"
				}));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, text);
			}
			else
			{
				global::Debug.LogWarning("Deserializing CustomGameSettings.ClusterLayout: Failed to find cluster '" + clusterDefaultName + "' including the scoped path, setting to default cluster name.");
				global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
				this.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, WorldGenSettings.ClusterDefaultName);
			}
		}
		this.CheckCustomGameMode();
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x0029FC5C File Offset: 0x0029DE5C
	private void AddMissingQualitySettings()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			SettingConfig value = keyValuePair.Value;
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(value.required_content) && !this.CurrentQualityLevelsBySetting.ContainsKey(value.id))
			{
				if (value.missing_content_default != "")
				{
					DebugUtil.LogArgs(new object[]
					{
						string.Concat(new string[]
						{
							"QualitySetting '",
							value.id,
							"' is missing, setting it to missing_content_default '",
							value.missing_content_default,
							"'."
						})
					});
					this.SetQualitySetting(value, value.missing_content_default);
				}
				else
				{
					DebugUtil.DevLogError("QualitySetting '" + value.id + "' is missing in this save. Either provide a missing_content_default or handle it in OnDeserialized.");
				}
			}
		}
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x0029FD5C File Offset: 0x0029DF5C
	protected override void OnPrefabInit()
	{
		DlcManager.IsExpansion1Active();
		Action<SettingConfig> action = delegate(SettingConfig setting)
		{
			this.AddQualitySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedQualitySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action2 = delegate(SettingConfig setting)
		{
			this.AddStorySettingConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedStorySettings.Add(setting.id);
			}
		};
		Action<SettingConfig> action3 = delegate(SettingConfig setting)
		{
			this.AddMixingSettingsConfig(setting);
			if (setting.coordinate_range >= 0L)
			{
				this.CoordinatedMixingSettings.Add(setting.id);
			}
		};
		CustomGameSettings.instance = this;
		action(CustomGameSettingConfigs.ClusterLayout);
		action(CustomGameSettingConfigs.WorldgenSeed);
		action(CustomGameSettingConfigs.ImmuneSystem);
		action(CustomGameSettingConfigs.CalorieBurn);
		action(CustomGameSettingConfigs.Morale);
		action(CustomGameSettingConfigs.Durability);
		action(CustomGameSettingConfigs.MeteorShowers);
		action(CustomGameSettingConfigs.Radiation);
		action(CustomGameSettingConfigs.Stress);
		action(CustomGameSettingConfigs.StressBreaks);
		action(CustomGameSettingConfigs.CarePackages);
		action(CustomGameSettingConfigs.SandboxMode);
		action(CustomGameSettingConfigs.FastWorkersMode);
		action(CustomGameSettingConfigs.SaveToCloud);
		action(CustomGameSettingConfigs.Teleporters);
		action3(CustomMixingSettingsConfigs.DLC2Mixing);
		action3(CustomMixingSettingsConfigs.IceCavesMixing);
		action3(CustomMixingSettingsConfigs.CarrotQuarryMixing);
		action3(CustomMixingSettingsConfigs.SugarWoodsMixing);
		action3(CustomMixingSettingsConfigs.CeresAsteroidMixing);
		action3(CustomMixingSettingsConfigs.DLC3Mixing);
		foreach (Story story in Db.Get().Stories.GetStoriesSortedByCoordinateOrder())
		{
			int num = (story.kleiUseOnlyCoordinateOrder == -1) ? -1 : 3;
			SettingConfig obj = new ListSettingConfig(story.Id, "", "", new List<SettingLevel>
			{
				new SettingLevel("Disabled", "", "", 0L, null),
				new SettingLevel("Guaranteed", "", "", 1L, null)
			}, "Disabled", "Disabled", (long)num, false, false, null, "", false);
			action2(obj);
		}
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && DlcManager.IsContentSubscribed(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(dlcMixingSettingConfig, "Enabled");
			}
		}
		this.VerifySettingCoordinates();
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x0029FFC0 File Offset: 0x0029E1C0
	public void DisableAllStories()
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.StorySettings)
		{
			this.SetStorySetting(keyValuePair.Value, false);
		}
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x002A001C File Offset: 0x0029E21C
	public void SetSurvivalDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Survival;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetDefaultLevelId());
		}
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x002A0088 File Offset: 0x0029E288
	public void SetNosweatDefaults()
	{
		this.customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			this.SetQualitySetting(keyValuePair.Value, keyValuePair.Value.GetNoSweatDefaultLevelId());
		}
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x000DD57A File Offset: 0x000DB77A
	public SettingLevel CycleQualitySettingLevel(ListSettingConfig config, int direction)
	{
		this.SetQualitySetting(config, config.CycleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x000DD5B2 File Offset: 0x000DB7B2
	public SettingLevel ToggleQualitySettingLevel(ToggleSettingConfig config)
	{
		this.SetQualitySetting(config, config.ToggleSettingLevelID(this.CurrentQualityLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentQualityLevelsBySetting[config.id]);
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x002A00F4 File Offset: 0x0029E2F4
	private void CheckCustomGameMode()
	{
		bool flag = true;
		bool flag2 = true;
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			if (!this.QualitySettings.ContainsKey(keyValuePair.Key))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Quality settings missing " + keyValuePair.Key
				});
			}
			else if (this.QualitySettings[keyValuePair.Key].triggers_custom_game)
			{
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetDefaultLevelId())
				{
					flag = false;
				}
				if (keyValuePair.Value != this.QualitySettings[keyValuePair.Key].GetNoSweatDefaultLevelId())
				{
					flag2 = false;
				}
				if (!flag && !flag2)
				{
					break;
				}
			}
		}
		CustomGameSettings.CustomGameMode customGameMode;
		if (flag)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Survival;
		}
		else if (flag2)
		{
			customGameMode = CustomGameSettings.CustomGameMode.Nosweat;
		}
		else
		{
			customGameMode = CustomGameSettings.CustomGameMode.Custom;
		}
		if (customGameMode != this.customGameMode)
		{
			DebugUtil.LogArgs(new object[]
			{
				"Game mode changed from",
				this.customGameMode,
				"to",
				customGameMode
			});
			this.customGameMode = customGameMode;
		}
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x000DD5E9 File Offset: 0x000DB7E9
	public void SetQualitySetting(SettingConfig config, string value)
	{
		this.SetQualitySetting(config, value, true);
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x000DD5F4 File Offset: 0x000DB7F4
	public void SetQualitySetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentQualityLevelsBySetting[config.id] = value;
		this.CheckCustomGameMode();
		if (notify && this.OnQualitySettingChanged != null)
		{
			this.OnQualitySettingChanged(config, this.GetCurrentQualitySetting(config));
		}
	}

	// Token: 0x06005DF0 RID: 24048 RVA: 0x000DD62C File Offset: 0x000DB82C
	public SettingLevel GetCurrentQualitySetting(SettingConfig setting)
	{
		return this.GetCurrentQualitySetting(setting.id);
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x002A0248 File Offset: 0x0029E448
	public SettingLevel GetCurrentQualitySetting(string setting_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(setting_id))
		{
			this.CurrentQualityLevelsBySetting[setting_id] = this.QualitySettings[setting_id].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentQualityLevelsBySetting[setting_id] : settingConfig.GetDefaultLevelId();
		return this.QualitySettings[setting_id].GetLevel(level_id);
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x000DD63A File Offset: 0x000DB83A
	public string GetCurrentQualitySettingLevelId(SettingConfig config)
	{
		return this.CurrentQualityLevelsBySetting[config.id];
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x002A02FC File Offset: 0x0029E4FC
	public string GetSettingLevelLabel(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.label;
			}
		}
		global::Debug.LogWarning("No label string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x002A0348 File Offset: 0x0029E548
	public string GetQualitySettingLevelTooltip(string setting_id, string level_id)
	{
		SettingConfig settingConfig = this.QualitySettings[setting_id];
		if (settingConfig != null)
		{
			SettingLevel level = settingConfig.GetLevel(level_id);
			if (level != null)
			{
				return level.tooltip;
			}
		}
		global::Debug.LogWarning("No tooltip string for setting: " + setting_id + " level: " + level_id);
		return "";
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x002A0394 File Offset: 0x0029E594
	public void AddQualitySettingConfig(SettingConfig config)
	{
		this.QualitySettings.Add(config.id, config);
		if (!this.CurrentQualityLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentQualityLevelsBySetting[config.id]))
		{
			this.CurrentQualityLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x002A03F8 File Offset: 0x0029E5F8
	public void AddStorySettingConfig(SettingConfig config)
	{
		this.StorySettings.Add(config.id, config);
		if (!this.currentStoryLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.currentStoryLevelsBySetting[config.id]))
		{
			this.currentStoryLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x000DD64D File Offset: 0x000DB84D
	public void SetStorySetting(SettingConfig config, string value)
	{
		this.SetStorySetting(config, value == "Guaranteed");
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x000DD661 File Offset: 0x000DB861
	public void SetStorySetting(SettingConfig config, bool value)
	{
		this.currentStoryLevelsBySetting[config.id] = (value ? "Guaranteed" : "Disabled");
		if (this.OnStorySettingChanged != null)
		{
			this.OnStorySettingChanged(config, this.GetCurrentStoryTraitSetting(config));
		}
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x002A045C File Offset: 0x0029E65C
	public void ParseAndApplyStoryTraitSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedStorySettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.StorySettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetStorySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005DFA RID: 24058 RVA: 0x002A05B8 File Offset: 0x0029E7B8
	private string GetStoryTraitSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedStorySettings)
		{
			SettingConfig settingConfig = this.StorySettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.currentStoryLevelsBySetting[key]).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x000DD69E File Offset: 0x000DB89E
	public SettingLevel GetCurrentStoryTraitSetting(SettingConfig setting)
	{
		return this.GetCurrentStoryTraitSetting(setting.id);
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x002A0654 File Offset: 0x0029E854
	public SettingLevel GetCurrentStoryTraitSetting(string settingId)
	{
		SettingConfig settingConfig = this.StorySettings[settingId];
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Survival && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetDefaultLevelId());
		}
		if (this.customGameMode == CustomGameSettings.CustomGameMode.Nosweat && settingConfig.triggers_custom_game)
		{
			return settingConfig.GetLevel(settingConfig.GetNoSweatDefaultLevelId());
		}
		if (!this.currentStoryLevelsBySetting.ContainsKey(settingId))
		{
			this.currentStoryLevelsBySetting[settingId] = this.StorySettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.currentStoryLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.StorySettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x002A0708 File Offset: 0x0029E908
	public List<string> GetCurrentStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, string> keyValuePair in this.currentStoryLevelsBySetting)
		{
			if (this.IsStoryActive(keyValuePair.Key, keyValuePair.Value))
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x002A0780 File Offset: 0x0029E980
	public bool IsStoryActive(string id, string level)
	{
		SettingConfig settingConfig;
		return this.StorySettings.TryGetValue(id, out settingConfig) && settingConfig != null && level == "Guaranteed";
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x000DD6AC File Offset: 0x000DB8AC
	public void SetMixingSetting(SettingConfig config, string value)
	{
		this.SetMixingSetting(config, value, true);
	}

	// Token: 0x06005E00 RID: 24064 RVA: 0x000DD6B7 File Offset: 0x000DB8B7
	public void SetMixingSetting(SettingConfig config, string value, bool notify)
	{
		this.CurrentMixingLevelsBySetting[config.id] = value;
		if (notify && this.OnMixingSettingChanged != null)
		{
			this.OnMixingSettingChanged(config, this.GetCurrentMixingSettingLevel(config));
		}
	}

	// Token: 0x06005E01 RID: 24065 RVA: 0x002A07B0 File Offset: 0x0029E9B0
	public void AddMixingSettingsConfig(SettingConfig config)
	{
		this.MixingSettings.Add(config.id, config);
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(config.id) || string.IsNullOrEmpty(this.CurrentMixingLevelsBySetting[config.id]))
		{
			this.CurrentMixingLevelsBySetting[config.id] = config.GetDefaultLevelId();
		}
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x000DD6E9 File Offset: 0x000DB8E9
	public SettingLevel GetCurrentMixingSettingLevel(SettingConfig setting)
	{
		return this.GetCurrentMixingSettingLevel(setting.id);
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x002A0814 File Offset: 0x0029EA14
	public SettingConfig GetWorldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = keyValuePair.Value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && worldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x002A088C File Offset: 0x0029EA8C
	public SettingConfig GetSubworldMixingSettingForWorldgenFile(string file)
	{
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = keyValuePair.Value as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && subworldMixingSettingConfig.worldgenPath == file)
			{
				return keyValuePair.Value;
			}
		}
		return null;
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x002A0904 File Offset: 0x0029EB04
	public void DisableAllMixing()
	{
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			this.SetMixingSetting(settingConfig, settingConfig.GetDefaultLevelId());
		}
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x002A0964 File Offset: 0x0029EB64
	public List<SubworldMixingSettingConfig> GetActiveSubworldMixingSettings()
	{
		List<SubworldMixingSettingConfig> list = new List<SubworldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			SubworldMixingSettingConfig subworldMixingSettingConfig = settingConfig as SubworldMixingSettingConfig;
			if (subworldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(subworldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x002A09E8 File Offset: 0x0029EBE8
	public List<WorldMixingSettingConfig> GetActiveWorldMixingSettings()
	{
		List<WorldMixingSettingConfig> list = new List<WorldMixingSettingConfig>();
		foreach (SettingConfig settingConfig in this.MixingSettings.Values)
		{
			WorldMixingSettingConfig worldMixingSettingConfig = settingConfig as WorldMixingSettingConfig;
			if (worldMixingSettingConfig != null && this.GetCurrentMixingSettingLevel(settingConfig).id != "Disabled")
			{
				list.Add(worldMixingSettingConfig);
			}
		}
		return list;
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x000DD6F7 File Offset: 0x000DB8F7
	public SettingLevel CycleMixingSettingLevel(ListSettingConfig config, int direction)
	{
		this.SetMixingSetting(config, config.CycleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id], direction));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x000DD72F File Offset: 0x000DB92F
	public SettingLevel ToggleMixingSettingLevel(ToggleSettingConfig config)
	{
		this.SetMixingSetting(config, config.ToggleSettingLevelID(this.CurrentMixingLevelsBySetting[config.id]));
		return config.GetLevel(this.CurrentMixingLevelsBySetting[config.id]);
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x002A0A6C File Offset: 0x0029EC6C
	public SettingLevel GetCurrentMixingSettingLevel(string settingId)
	{
		SettingConfig settingConfig = this.MixingSettings[settingId];
		if (!this.CurrentMixingLevelsBySetting.ContainsKey(settingId))
		{
			this.CurrentMixingLevelsBySetting[settingId] = this.MixingSettings[settingId].GetDefaultLevelId();
		}
		string level_id = DlcManager.IsAllContentSubscribed(settingConfig.required_content) ? this.CurrentMixingLevelsBySetting[settingId] : settingConfig.GetDefaultLevelId();
		return this.MixingSettings[settingId].GetLevel(level_id);
	}

	// Token: 0x06005E0B RID: 24075 RVA: 0x002A0AE8 File Offset: 0x0029ECE8
	public List<string> GetCurrentDlcMixingIds()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && dlcMixingSettingConfig.IsOnLevel(this.GetCurrentMixingSettingLevel(dlcMixingSettingConfig.id).id))
			{
				list.Add(dlcMixingSettingConfig.id);
			}
		}
		return list;
	}

	// Token: 0x06005E0C RID: 24076 RVA: 0x002A0B70 File Offset: 0x0029ED70
	public void ParseAndApplyMixingSettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedMixingSettings))
		{
			string key = (string)obj;
			SettingConfig settingConfig = this.MixingSettings[key];
			if (settingConfig.coordinate_range != -1L)
			{
				long num = (long)(dividend % settingConfig.coordinate_range);
				dividend /= settingConfig.coordinate_range;
				foreach (SettingLevel settingLevel in settingConfig.GetLevels())
				{
					if (settingLevel.coordinate_value == num)
					{
						dictionary[settingConfig] = settingLevel.id;
						break;
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetMixingSetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x002A0CCC File Offset: 0x0029EECC
	private string GetMixingSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string key in this.CoordinatedMixingSettings)
		{
			SettingConfig settingConfig = this.MixingSettings[key];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentMixingSettingLevel(settingConfig).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005E0E RID: 24078 RVA: 0x002A0D68 File Offset: 0x0029EF68
	public void RemoveInvalidMixingSettings()
	{
		ClusterLayout currentClusterLayout = this.GetCurrentClusterLayout();
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.MixingSettings)
		{
			DlcMixingSettingConfig dlcMixingSettingConfig = keyValuePair.Value as DlcMixingSettingConfig;
			if (dlcMixingSettingConfig != null && currentClusterLayout.requiredDlcIds.Contains(dlcMixingSettingConfig.id))
			{
				this.SetMixingSetting(keyValuePair.Value, "Disabled");
			}
		}
		CustomGameSettings.<>c__DisplayClass71_0 CS$<>8__locals1;
		CS$<>8__locals1.availableDlcs = this.GetCurrentDlcMixingIds();
		CS$<>8__locals1.availableDlcs.AddRange(currentClusterLayout.requiredDlcIds);
		foreach (KeyValuePair<string, SettingConfig> keyValuePair2 in this.MixingSettings)
		{
			SettingConfig value = keyValuePair2.Value;
			WorldMixingSettingConfig worldMixingSettingConfig = value as WorldMixingSettingConfig;
			if (worldMixingSettingConfig == null)
			{
				SubworldMixingSettingConfig subworldMixingSettingConfig = value as SubworldMixingSettingConfig;
				if (subworldMixingSettingConfig != null)
				{
					if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(subworldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(subworldMixingSettingConfig.forbiddenClusterTags))
					{
						this.SetMixingSetting(keyValuePair2.Value, "Disabled");
					}
				}
			}
			else if (!CustomGameSettings.<RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(worldMixingSettingConfig.required_content, ref CS$<>8__locals1) || currentClusterLayout.HasAnyTags(worldMixingSettingConfig.forbiddenClusterTags))
			{
				this.SetMixingSetting(keyValuePair2.Value, "Disabled");
			}
		}
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x002A0EDC File Offset: 0x0029F0DC
	public ClusterLayout GetCurrentClusterLayout()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			return null;
		}
		return SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
	}

	// Token: 0x06005E10 RID: 24080 RVA: 0x002A0F10 File Offset: 0x0029F110
	public int GetCurrentWorldgenSeed()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		if (currentQualitySetting == null)
		{
			return 0;
		}
		return int.Parse(currentQualitySetting.id);
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x002A0F40 File Offset: 0x0029F140
	public void LoadClusters()
	{
		Dictionary<string, ClusterLayout> clusterCache = SettingsCache.clusterLayouts.clusterCache;
		List<SettingLevel> list = new List<SettingLevel>(clusterCache.Count);
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in clusterCache)
		{
			StringEntry stringEntry;
			string label = Strings.TryGet(new StringKey(keyValuePair.Value.name), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.name;
			string tooltip = Strings.TryGet(new StringKey(keyValuePair.Value.description), out stringEntry) ? stringEntry.ToString() : keyValuePair.Value.description;
			list.Add(new SettingLevel(keyValuePair.Key, label, tooltip, 0L, null));
		}
		CustomGameSettingConfigs.ClusterLayout.StompLevels(list, WorldGenSettings.ClusterDefaultName, WorldGenSettings.ClusterDefaultName);
	}

	// Token: 0x06005E12 RID: 24082 RVA: 0x002A1030 File Offset: 0x0029F230
	public void Print()
	{
		string text = "Custom Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair.Key,
				"=",
				keyValuePair.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Story Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair2 in this.currentStoryLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair2.Key,
				"=",
				keyValuePair2.Value,
				","
			});
		}
		global::Debug.Log(text);
		text = "Mixing Settings: ";
		foreach (KeyValuePair<string, string> keyValuePair3 in this.CurrentMixingLevelsBySetting)
		{
			text = string.Concat(new string[]
			{
				text,
				keyValuePair3.Key,
				"=",
				keyValuePair3.Value,
				","
			});
		}
		global::Debug.Log(text);
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x002A11B4 File Offset: 0x0029F3B4
	private bool AllValuesMatch(Dictionary<string, string> data, CustomGameSettings.CustomGameMode mode)
	{
		bool result = true;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in this.QualitySettings)
		{
			if (!(keyValuePair.Key == CustomGameSettingConfigs.WorldgenSeed.id))
			{
				string b = null;
				if (mode != CustomGameSettings.CustomGameMode.Survival)
				{
					if (mode == CustomGameSettings.CustomGameMode.Nosweat)
					{
						b = keyValuePair.Value.GetNoSweatDefaultLevelId();
					}
				}
				else
				{
					b = keyValuePair.Value.GetDefaultLevelId();
				}
				if (data.ContainsKey(keyValuePair.Key) && data[keyValuePair.Key] != b)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x002A1268 File Offset: 0x0029F468
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		list.Add(new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameMode",
			Value = this.customGameMode.ToString()
		});
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentQualityLevelsBySetting)
		{
			list.Add(new CustomGameSettings.MetricSettingsData
			{
				Name = keyValuePair.Key,
				Value = keyValuePair.Value
			});
		}
		CustomGameSettings.MetricSettingsData item = new CustomGameSettings.MetricSettingsData
		{
			Name = "CustomGameModeActual",
			Value = CustomGameSettings.CustomGameMode.Custom.ToString()
		};
		foreach (object obj in Enum.GetValues(typeof(CustomGameSettings.CustomGameMode)))
		{
			CustomGameSettings.CustomGameMode customGameMode = (CustomGameSettings.CustomGameMode)obj;
			if (customGameMode != CustomGameSettings.CustomGameMode.Custom && this.AllValuesMatch(this.CurrentQualityLevelsBySetting, customGameMode))
			{
				item.Value = customGameMode.ToString();
				break;
			}
		}
		list.Add(item);
		return list;
	}

	// Token: 0x06005E15 RID: 24085 RVA: 0x002A13D4 File Offset: 0x0029F5D4
	public List<CustomGameSettings.MetricSettingsData> GetSettingsForMixingMetrics()
	{
		List<CustomGameSettings.MetricSettingsData> list = new List<CustomGameSettings.MetricSettingsData>();
		foreach (KeyValuePair<string, string> keyValuePair in this.CurrentMixingLevelsBySetting)
		{
			if (DlcManager.IsAllContentSubscribed(this.MixingSettings[keyValuePair.Key].required_content))
			{
				list.Add(new CustomGameSettings.MetricSettingsData
				{
					Name = keyValuePair.Key,
					Value = keyValuePair.Value
				});
			}
		}
		return list;
	}

	// Token: 0x06005E16 RID: 24086 RVA: 0x002A1470 File Offset: 0x0029F670
	public bool VerifySettingCoordinates()
	{
		bool flag = this.VerifySettingsDictionary(this.QualitySettings);
		bool flag2 = this.VerifySettingsDictionary(this.StorySettings);
		return flag || flag2;
	}

	// Token: 0x06005E17 RID: 24087 RVA: 0x002A1498 File Offset: 0x0029F698
	private bool VerifySettingsDictionary(Dictionary<string, SettingConfig> configs)
	{
		bool result = false;
		foreach (KeyValuePair<string, SettingConfig> keyValuePair in configs)
		{
			if (keyValuePair.Value.coordinate_range >= 0L)
			{
				List<SettingLevel> levels = keyValuePair.Value.GetLevels();
				if (keyValuePair.Value.coordinate_range < (long)levels.Count)
				{
					result = true;
					global::Debug.Assert(false, string.Concat(new string[]
					{
						keyValuePair.Value.id,
						": Range between coordinate min and max insufficient for all levels (",
						keyValuePair.Value.coordinate_range.ToString(),
						"<",
						levels.Count.ToString(),
						")"
					}));
				}
				foreach (SettingLevel settingLevel in levels)
				{
					Dictionary<long, string> dictionary = new Dictionary<long, string>();
					string text = keyValuePair.Value.id + " > " + settingLevel.id;
					if (keyValuePair.Value.coordinate_range <= settingLevel.coordinate_value)
					{
						result = true;
						global::Debug.Assert(false, string.Format("%s: Level coordinate value (%u) exceedes range (%u)", text, settingLevel.coordinate_value, keyValuePair.Value.coordinate_range));
					}
					if (settingLevel.coordinate_value < 0L)
					{
						result = true;
						global::Debug.Assert(false, text + ": Level coordinate value must be >= 0");
					}
					else if (settingLevel.coordinate_value == 0L)
					{
						if (settingLevel.id != keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Only the default level should have a coordinate value of 0");
						}
					}
					else
					{
						string str;
						bool flag = !dictionary.TryGetValue(settingLevel.coordinate_value, out str);
						dictionary[settingLevel.coordinate_value] = text;
						if (settingLevel.id == keyValuePair.Value.GetDefaultLevelId())
						{
							result = true;
							global::Debug.Assert(false, text + ": Default level must be a coordinate value of 0");
						}
						if (!flag)
						{
							result = true;
							global::Debug.Assert(false, text + ": Combined coordinate conflicts with another coordinate (" + str + "). Ensure this SettingConfig's min and max don't overlap with another SettingConfig's");
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005E18 RID: 24088 RVA: 0x002A1718 File Offset: 0x0029F918
	public static string[] ParseSettingCoordinate(string coord)
	{
		Match match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)").Match(coord);
		for (int i = 1; i <= 2; i++)
		{
			if (match.Groups.Count == 1)
			{
				match = new Regex("(.*)-(\\d*)-(.*)-(.*)-(.*)".Remove("(.*)-(\\d*)-(.*)-(.*)-(.*)".Length - i * 5)).Match(coord);
			}
		}
		string[] array = new string[match.Groups.Count];
		for (int j = 0; j < match.Groups.Count; j++)
		{
			array[j] = match.Groups[j].Value;
		}
		return array;
	}

	// Token: 0x06005E19 RID: 24089 RVA: 0x002A17B0 File Offset: 0x0029F9B0
	public string GetSettingsCoordinate()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (currentQualitySetting == null)
		{
			DebugUtil.DevLogError("GetSettingsCoordinate: clusterLayoutSetting is null, returning '0' coordinate");
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			return "0-0-0-0-0";
		}
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
		string otherSettingsCode = this.GetOtherSettingsCode();
		string storyTraitSettingsCode = this.GetStoryTraitSettingsCode();
		string mixingSettingsCode = this.GetMixingSettingsCode();
		return string.Format("{0}-{1}-{2}-{3}-{4}", new object[]
		{
			clusterData.GetCoordinatePrefix(),
			currentQualitySetting2.id,
			otherSettingsCode,
			storyTraitSettingsCode,
			mixingSettingsCode
		});
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x002A187C File Offset: 0x0029FA7C
	public void ParseAndApplySettingsCode(string code)
	{
		BigInteger dividend = this.Base36toBinary(code);
		Dictionary<SettingConfig, string> dictionary = new Dictionary<SettingConfig, string>();
		foreach (object obj in global::Util.Reverse(this.CoordinatedQualitySettings))
		{
			string key = (string)obj;
			if (this.QualitySettings.ContainsKey(key))
			{
				SettingConfig settingConfig = this.QualitySettings[key];
				if (settingConfig.coordinate_range != -1L)
				{
					long num = (long)(dividend % settingConfig.coordinate_range);
					dividend /= settingConfig.coordinate_range;
					foreach (SettingLevel settingLevel in settingConfig.GetLevels())
					{
						if (settingLevel.coordinate_value == num)
						{
							dictionary[settingConfig] = settingLevel.id;
							break;
						}
					}
				}
			}
		}
		foreach (KeyValuePair<SettingConfig, string> keyValuePair in dictionary)
		{
			this.SetQualitySetting(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x002A19E8 File Offset: 0x0029FBE8
	private string GetOtherSettingsCode()
	{
		BigInteger bigInteger = 0;
		foreach (string text in this.CoordinatedQualitySettings)
		{
			SettingConfig settingConfig = this.QualitySettings[text];
			bigInteger *= settingConfig.coordinate_range;
			bigInteger += settingConfig.GetLevel(this.GetCurrentQualitySetting(text).id).coordinate_value;
		}
		return this.BinarytoBase36(bigInteger);
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x002A1A84 File Offset: 0x0029FC84
	private BigInteger Base36toBinary(string input)
	{
		if (input == "0")
		{
			return 0;
		}
		BigInteger bigInteger = 0;
		for (int i = input.Length - 1; i >= 0; i--)
		{
			bigInteger *= 36;
			long value = (long)this.hexChars.IndexOf(input[i]);
			bigInteger += value;
		}
		DebugUtil.LogArgs(new object[]
		{
			"tried converting",
			input,
			", got",
			bigInteger,
			"and returns to",
			this.BinarytoBase36(bigInteger)
		});
		return bigInteger;
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x002A1B2C File Offset: 0x0029FD2C
	private string BinarytoBase36(BigInteger input)
	{
		if (input == 0L)
		{
			return "0";
		}
		BigInteger bigInteger = input;
		string text = "";
		while (bigInteger > 0L)
		{
			text += this.hexChars[(int)(bigInteger % 36)].ToString();
			bigInteger /= 36;
		}
		return text;
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x002A1C1C File Offset: 0x0029FE1C
	[CompilerGenerated]
	internal static bool <RemoveInvalidMixingSettings>g__HasRequiredContent|71_0(string[] requiredContent, ref CustomGameSettings.<>c__DisplayClass71_0 A_1)
	{
		foreach (string text in requiredContent)
		{
			if (!(text == "") && !A_1.availableDlcs.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0400428B RID: 17035
	private static CustomGameSettings instance;

	// Token: 0x0400428C RID: 17036
	public const long NO_COORDINATE_RANGE = -1L;

	// Token: 0x0400428D RID: 17037
	private const int NUM_STORY_LEVELS = 3;

	// Token: 0x0400428E RID: 17038
	public const string STORY_DISABLED_LEVEL = "Disabled";

	// Token: 0x0400428F RID: 17039
	public const string STORY_GUARANTEED_LEVEL = "Guaranteed";

	// Token: 0x04004290 RID: 17040
	[Serialize]
	public bool is_custom_game;

	// Token: 0x04004291 RID: 17041
	[Serialize]
	public CustomGameSettings.CustomGameMode customGameMode;

	// Token: 0x04004292 RID: 17042
	[Serialize]
	private Dictionary<string, string> CurrentQualityLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x04004293 RID: 17043
	[Serialize]
	private Dictionary<string, string> CurrentMixingLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x04004294 RID: 17044
	private Dictionary<string, string> currentStoryLevelsBySetting = new Dictionary<string, string>();

	// Token: 0x04004295 RID: 17045
	public List<string> CoordinatedQualitySettings = new List<string>();

	// Token: 0x04004296 RID: 17046
	public Dictionary<string, SettingConfig> QualitySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04004297 RID: 17047
	public List<string> CoordinatedStorySettings = new List<string>();

	// Token: 0x04004298 RID: 17048
	public Dictionary<string, SettingConfig> StorySettings = new Dictionary<string, SettingConfig>();

	// Token: 0x04004299 RID: 17049
	public List<string> CoordinatedMixingSettings = new List<string>();

	// Token: 0x0400429A RID: 17050
	public Dictionary<string, SettingConfig> MixingSettings = new Dictionary<string, SettingConfig>();

	// Token: 0x0400429E RID: 17054
	private const string coordinatePatern = "(.*)-(\\d*)-(.*)-(.*)-(.*)";

	// Token: 0x0400429F RID: 17055
	private string hexChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	// Token: 0x020011FF RID: 4607
	public enum CustomGameMode
	{
		// Token: 0x040042A1 RID: 17057
		Survival,
		// Token: 0x040042A2 RID: 17058
		Nosweat,
		// Token: 0x040042A3 RID: 17059
		Custom = 255
	}

	// Token: 0x02001200 RID: 4608
	public struct MetricSettingsData
	{
		// Token: 0x040042A4 RID: 17060
		public string Name;

		// Token: 0x040042A5 RID: 17061
		public string Value;
	}
}
