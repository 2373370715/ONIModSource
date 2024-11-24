using System;
using System.Collections.Generic;
using System.Linq;
using Klei;
using Klei.CustomSettings;
using ObjectCloner;
using ProcGen;
using STRINGS;

namespace ProcGenGame;

public class WorldgenMixing
{
	public class MixingOption<T> : IComparable<MixingOption<T>>
	{
		public string worldgenPath;

		public T mixingSettings;

		public int minCount;

		public int maxCount;

		public bool IsExhausted => maxCount <= 0;

		public bool IsSatisfied => minCount <= 0;

		public void Consume()
		{
			minCount--;
			maxCount--;
		}

		public int CompareTo(MixingOption<T> other)
		{
			int num = other.minCount.CompareTo(minCount);
			if (num != 0)
			{
				return num;
			}
			return other.maxCount.CompareTo(maxCount);
		}
	}

	public class WorldMixingOption : MixingOption<WorldMixingSettings>
	{
		public ProcGen.World cachedWorld;
	}

	private const int NUM_WORLD_TO_TRY_SUBWORLDMIXING = 3;

	public static void RefreshWorldMixing(MutatedClusterLayout mutatedLayout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
	{
		foreach (WorldPlacement worldPlacement in mutatedLayout.layout.worldPlacements)
		{
			worldPlacement.UndoWorldMixing();
		}
		DoWorldMixingInternal(mutatedLayout, seed, isRunningWorldgenDebug, muteErrors);
	}

	public static MutatedClusterLayout DoWorldMixing(ClusterLayout layout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
	{
		return DoWorldMixingInternal(new MutatedClusterLayout(layout), seed, isRunningWorldgenDebug, muteErrors);
	}

	private static MutatedClusterLayout DoWorldMixingInternal(MutatedClusterLayout mutatedClusterLayout, int seed, bool isRunningWorldgenDebug, bool muteErrors)
	{
		List<WorldMixingOption> list = new List<WorldMixingOption>();
		if (CustomGameSettings.Instance != null && !GenericGameSettings.instance.devAutoWorldGen)
		{
			foreach (WorldMixingSettingConfig activeWorldMixingSetting in CustomGameSettings.Instance.GetActiveWorldMixingSettings())
			{
				WorldMixingSettings worldMixingSettings = SettingsCache.TryGetCachedWorldMixingSetting(activeWorldMixingSetting.worldgenPath);
				if (!mutatedClusterLayout.layout.HasAnyTags(worldMixingSettings.forbiddenClusterTags))
				{
					int minCount = ((CustomGameSettings.Instance.GetCurrentMixingSettingLevel(activeWorldMixingSetting.id).id == "GuranteeMixing") ? 1 : 0);
					ProcGen.World worldData = SettingsCache.worlds.GetWorldData(worldMixingSettings.world);
					list.Add(new WorldMixingOption
					{
						worldgenPath = worldMixingSettings.world,
						mixingSettings = worldMixingSettings,
						minCount = minCount,
						maxCount = 1,
						cachedWorld = worldData
					});
				}
			}
		}
		else
		{
			string[] devWorldMixing = GenericGameSettings.instance.devWorldMixing;
			for (int i = 0; i < devWorldMixing.Length; i++)
			{
				WorldMixingSettings worldMixingSettings2 = SettingsCache.TryGetCachedWorldMixingSetting(devWorldMixing[i]);
				ProcGen.World worldData2 = SettingsCache.worlds.GetWorldData(worldMixingSettings2.world);
				list.Add(new WorldMixingOption
				{
					worldgenPath = worldMixingSettings2.world,
					mixingSettings = worldMixingSettings2,
					minCount = 1,
					maxCount = 1,
					cachedWorld = worldData2
				});
			}
		}
		KRandom rng = new KRandom(seed);
		foreach (WorldPlacement worldPlacement in mutatedClusterLayout.layout.worldPlacements)
		{
			worldPlacement.UndoWorldMixing();
		}
		List<WorldPlacement> list2 = new List<WorldPlacement>(mutatedClusterLayout.layout.worldPlacements);
		list2.ShuffleSeeded(rng);
		foreach (WorldPlacement item in list2)
		{
			if (!item.IsMixingPlacement())
			{
				continue;
			}
			list.ShuffleSeeded(rng);
			WorldMixingOption worldMixingOption = FindWorldMixingOption(item, list);
			if (worldMixingOption != null)
			{
				Debug.Log("Mixing: Applied world substitution " + item.world + " -> " + worldMixingOption.worldgenPath);
				item.worldMixing.previousWorld = item.world;
				item.worldMixing.mixingWasApplied = true;
				item.world = worldMixingOption.worldgenPath;
				worldMixingOption.Consume();
				if (worldMixingOption.IsExhausted)
				{
					list.Remove(worldMixingOption);
				}
			}
		}
		if (!ValidateWorldMixingOptions(list, isRunningWorldgenDebug, muteErrors))
		{
			return null;
		}
		return mutatedClusterLayout;
	}

	private static WorldMixingOption FindWorldMixingOption(WorldPlacement worldPlacement, List<WorldMixingOption> options)
	{
		options = options.StableSort().ToList();
		foreach (WorldMixingOption option in options)
		{
			if (option.IsExhausted)
			{
				continue;
			}
			bool flag = true;
			foreach (string requiredTag in worldPlacement.worldMixing.requiredTags)
			{
				if (!option.cachedWorld.worldTags.Contains(requiredTag))
				{
					flag = false;
					break;
				}
			}
			foreach (string forbiddenTag in worldPlacement.worldMixing.forbiddenTags)
			{
				if (option.cachedWorld.worldTags.Contains(forbiddenTag))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return option;
			}
		}
		return null;
	}

	private static bool ValidateWorldMixingOptions(List<WorldMixingOption> options, bool isRunningWorldgenDebug, bool muteErrors)
	{
		List<string> list = new List<string>();
		foreach (WorldMixingOption option in options)
		{
			if (!option.IsSatisfied)
			{
				list.Add($"{option.worldgenPath} ({option.minCount})");
			}
		}
		if (list.Count > 0)
		{
			if (muteErrors)
			{
				return false;
			}
			string text = "WorldgenMixing: Could not guarantee these world mixings: " + string.Join("\n - ", list);
			if (!isRunningWorldgenDebug)
			{
				DebugUtil.LogWarningArgs(text);
				throw new WorldgenException(text, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
			}
			DebugUtil.LogErrorArgs(text);
			return false;
		}
		return true;
	}

	public static void DoSubworldMixing(Cluster cluster, int seed, Func<int, WorldGen, bool> ShouldSkipWorldCallback, bool isRunningWorldgenDebug)
	{
		List<MixingOption<SubworldMixingSettings>> list = new List<MixingOption<SubworldMixingSettings>>();
		if (CustomGameSettings.Instance != null && !GenericGameSettings.instance.devAutoWorldGen)
		{
			foreach (SubworldMixingSettingConfig activeSubworldMixingSetting in CustomGameSettings.Instance.GetActiveSubworldMixingSettings())
			{
				SubworldMixingSettings mixingSettings = SettingsCache.TryGetCachedSubworldMixingSetting(activeSubworldMixingSetting.worldgenPath);
				if (!cluster.clusterLayout.HasAnyTags(activeSubworldMixingSetting.forbiddenClusterTags))
				{
					int minCount = ((CustomGameSettings.Instance.GetCurrentMixingSettingLevel(activeSubworldMixingSetting.id).id == "GuranteeMixing") ? 1 : 0);
					list.Add(new MixingOption<SubworldMixingSettings>
					{
						worldgenPath = activeSubworldMixingSetting.worldgenPath,
						mixingSettings = mixingSettings,
						minCount = minCount,
						maxCount = 3
					});
				}
			}
		}
		else
		{
			string[] devSubworldMixing = GenericGameSettings.instance.devSubworldMixing;
			foreach (string text in devSubworldMixing)
			{
				SubworldMixingSettings mixingSettings2 = SettingsCache.TryGetCachedSubworldMixingSetting(text);
				list.Add(new MixingOption<SubworldMixingSettings>
				{
					worldgenPath = text,
					mixingSettings = mixingSettings2,
					minCount = 1,
					maxCount = 3
				});
			}
		}
		KRandom rng = new KRandom(seed);
		List<WorldGen> list2 = new List<WorldGen>(cluster.worlds);
		list2.ShuffleSeeded(rng);
		list2.Sort((WorldGen a, WorldGen b) => WorldPlacement.GetSortOrder(a.Settings.worldType).CompareTo(WorldPlacement.GetSortOrder(b.Settings.worldType)));
		for (int j = 0; j < cluster.worlds.Count; j++)
		{
			WorldGen worldGen = list2[j];
			list.ShuffleSeeded(rng);
			ApplySubworldMixingToWorld(worldGen.Settings.world, list);
		}
		ValidateSubworldMixingOptions(list, isRunningWorldgenDebug);
	}

	private static void ValidateSubworldMixingOptions(List<MixingOption<SubworldMixingSettings>> options, bool isRunningWorldgenDebug)
	{
		List<string> list = new List<string>();
		foreach (MixingOption<SubworldMixingSettings> option in options)
		{
			if (!option.IsSatisfied)
			{
				list.Add($"{option.worldgenPath} ({option.minCount})");
			}
		}
		if (list.Count > 0)
		{
			string text = "WorldgenMixing: Could not guarantee these subworld mixings: " + string.Join("\n - ", list);
			if (!isRunningWorldgenDebug)
			{
				DebugUtil.LogWarningArgs(text);
				throw new WorldgenException(text, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_MIXING);
			}
			DebugUtil.LogErrorArgs(text);
		}
	}

	private static void ApplySubworldMixingToWorld(ProcGen.World world, List<MixingOption<SubworldMixingSettings>> availableSubworldsForMixing)
	{
		List<ProcGen.World.SubworldMixingRule> list = new List<ProcGen.World.SubworldMixingRule>();
		foreach (ProcGen.World.SubworldMixingRule subworldMixingRule in world.subworldMixingRules)
		{
			if (availableSubworldsForMixing.Count == 0)
			{
				CleanupUnusedMixing(world);
				return;
			}
			MixingOption<SubworldMixingSettings> mixingOption = FindSubworldMixing(subworldMixingRule, availableSubworldsForMixing);
			if (mixingOption == null)
			{
				Debug.Log("WorldgenMixing: No valid mixing for '" + subworldMixingRule.name + "' on World '" + world.name + "' from options: " + string.Join(", ", from x in availableSubworldsForMixing
					where !x.IsExhausted
					select x.mixingSettings.name));
				continue;
			}
			WeightedSubworldName weightedSubworldName = SerializingCloner.Copy(mixingOption.mixingSettings.subworld);
			weightedSubworldName.minCount = Math.Max(subworldMixingRule.minCount, weightedSubworldName.minCount);
			weightedSubworldName.maxCount = Math.Min(subworldMixingRule.maxCount, weightedSubworldName.maxCount);
			world.subworldFiles.Add(weightedSubworldName);
			foreach (ProcGen.World.AllowedCellsFilter unknownCellsAllowedSubworld in world.unknownCellsAllowedSubworlds)
			{
				for (int i = 0; i < unknownCellsAllowedSubworld.subworldNames.Count; i++)
				{
					if (unknownCellsAllowedSubworld.subworldNames[i] == subworldMixingRule.name)
					{
						unknownCellsAllowedSubworld.subworldNames[i] = weightedSubworldName.name;
					}
				}
			}
			if (!list.Contains(subworldMixingRule))
			{
				world.worldTemplateRules.AddRange(mixingOption.mixingSettings.additionalWorldTemplateRules);
				list.Add(subworldMixingRule);
			}
			mixingOption.Consume();
			if (mixingOption.IsExhausted)
			{
				availableSubworldsForMixing.Remove(mixingOption);
			}
		}
		CleanupUnusedMixing(world);
	}

	private static MixingOption<SubworldMixingSettings> FindSubworldMixing(ProcGen.World.SubworldMixingRule rule, List<MixingOption<SubworldMixingSettings>> options)
	{
		options = options.StableSort().ToList();
		foreach (MixingOption<SubworldMixingSettings> option in options)
		{
			if (option.IsExhausted)
			{
				continue;
			}
			bool flag = true;
			foreach (string forbiddenTag in rule.forbiddenTags)
			{
				if (option.mixingSettings.mixingTags.Contains(forbiddenTag))
				{
					flag = false;
				}
			}
			foreach (string requiredTag in rule.requiredTags)
			{
				if (!option.mixingSettings.mixingTags.Contains(requiredTag))
				{
					flag = false;
				}
			}
			int num = Math.Max(rule.minCount, option.mixingSettings.subworld.minCount);
			int num2 = Math.Min(rule.maxCount, option.mixingSettings.subworld.maxCount);
			if (num > num2)
			{
				flag = false;
			}
			if (flag)
			{
				return option;
			}
		}
		return null;
	}

	private static void CleanupUnusedMixing(ProcGen.World world)
	{
		foreach (ProcGen.World.AllowedCellsFilter unknownCellsAllowedSubworld in world.unknownCellsAllowedSubworlds)
		{
			unknownCellsAllowedSubworld.subworldNames.RemoveAll(IsMixingProxyName);
		}
	}

	private static bool IsMixingProxyName(string name)
	{
		return name.StartsWith("(");
	}
}
