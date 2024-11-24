using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace ProcGenGame;

public class TemplateSpawning
{
	public class TemplateSpawner
	{
		public Vector2I position;

		public TemplateContainer container;

		public TerrainCell terrainCell;

		public RectInt bounds;

		public TemplateSpawner(Vector2I position, RectInt bounds, TemplateContainer container, TerrainCell terrainCell)
		{
			this.position = position;
			this.container = container;
			this.terrainCell = terrainCell;
			this.bounds = bounds;
		}
	}

	private static float s_minProgressPercent;

	private static float s_maxProgressPercent;

	private static int s_poiPadding;

	private const int TEMPERATURE_PADDING = 3;

	private const float EXTREME_POI_OVERLAP_TEMPERATURE_RANGE = 100f;

	public static List<TemplateSpawner> DetermineTemplatesForWorld(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, ref List<WorldTrait> placedStoryTraits, WorldGen.OfflineCallbackFunction successCallbackFn)
	{
		successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 0f, WorldGenProgressStages.Stages.PlaceTemplates);
		List<TemplateSpawner> templateSpawnTargets = new List<TemplateSpawner>();
		s_poiPadding = settings.GetIntSetting("POIPadding");
		s_minProgressPercent = 0f;
		s_maxProgressPercent = 0.33f;
		SpawnStartingTemplate(settings, terrainCells, ref templateSpawnTargets, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
		s_minProgressPercent = s_maxProgressPercent;
		s_maxProgressPercent = 0.66f;
		SpawnTemplatesFromTemplateRules(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, isRunningDebugGen, successCallbackFn);
		s_minProgressPercent = s_maxProgressPercent;
		s_maxProgressPercent = 1f;
		SpawnStoryTraitTemplates(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, ref placedStoryTraits, isRunningDebugGen, successCallbackFn);
		successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, 1f, WorldGenProgressStages.Stages.PlaceTemplates);
		return templateSpawnTargets;
	}

	private static float ProgressPercent(float stagePercent)
	{
		return MathUtil.ReRange(stagePercent, 0f, 1f, s_minProgressPercent, s_maxProgressPercent);
	}

	private static void SpawnStartingTemplate(WorldGenSettings settings, List<TerrainCell> terrainCells, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
	{
		TerrainCell terrainCell = terrainCells.Find((TerrainCell tc) => tc.node.tags.Contains(WorldGenTags.StartLocation));
		if (settings.world.startingBaseTemplate.IsNullOrWhiteSpace())
		{
			return;
		}
		TemplateContainer template = TemplateCache.GetTemplate(settings.world.startingBaseTemplate);
		Vector2I position = new Vector2I((int)terrainCell.poly.Centroid().x, (int)terrainCell.poly.Centroid().y);
		RectInt templateBounds = template.GetTemplateBounds(position, s_poiPadding);
		TemplateSpawner item = new TemplateSpawner(position, templateBounds, template, terrainCell);
		if (IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
		{
			string text = "TemplateSpawning: Starting template overlaps world boundaries in world '" + settings.world.filePath + "'";
			DebugUtil.DevLogError(text);
			if (!isRunningDebugGen)
			{
				throw new Exception(text);
			}
		}
		successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, ProgressPercent(1f), WorldGenProgressStages.Stages.PlaceTemplates);
		templateSpawnTargets.Add(item);
		placedPOIBounds.Add(templateBounds);
	}

	private static void SpawnTemplatesFromTemplateRules(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
	{
		List<ProcGen.World.TemplateSpawnRules> list = new List<ProcGen.World.TemplateSpawnRules>();
		if (settings.world.worldTemplateRules != null)
		{
			list.AddRange(settings.world.worldTemplateRules);
		}
		foreach (WeightedSubworldName subworldFile in settings.world.subworldFiles)
		{
			SubWorld subWorld = settings.GetSubWorld(subworldFile.name);
			if (subWorld.subworldTemplateRules != null)
			{
				list.AddRange(subWorld.subworldTemplateRules);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		int num = 0;
		float num2 = list.Count;
		list.Sort((ProcGen.World.TemplateSpawnRules a, ProcGen.World.TemplateSpawnRules b) => b.priority.CompareTo(a.priority));
		List<TemplateSpawner> newTemplateSpawnTargets = new List<TemplateSpawner>();
		HashSet<string> usedTemplates = new HashSet<string>();
		foreach (ProcGen.World.TemplateSpawnRules item in list)
		{
			successCallbackFn(UI.WORLDGEN.PLACINGTEMPLATES.key, ProgressPercent((float)num++ / num2), WorldGenProgressStages.Stages.PlaceTemplates);
			if (!ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, item, ref usedTemplates, out var errorMessage, ref newTemplateSpawnTargets))
			{
				DebugUtil.LogErrorArgs(errorMessage);
				if (!isRunningDebugGen)
				{
					throw new WorldgenException(errorMessage, UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE);
				}
			}
		}
	}

	private static void SpawnStoryTraitTemplates(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, ref List<WorldTrait> placedStoryTraits, bool isRunningDebugGen, WorldGen.OfflineCallbackFunction successCallbackFn)
	{
		Queue<WorldTrait> queue = new Queue<WorldTrait>(settings.GetStoryTraitCandiates());
		int count = queue.Count;
		List<WorldTrait> list = new List<WorldTrait>();
		HashSet<string> usedTemplates = new HashSet<string>();
		while (queue.Count > 0 && list.Count < count)
		{
			WorldTrait worldTrait = queue.Dequeue();
			bool flag = false;
			List<TemplateSpawner> newTemplateSpawnTargets = new List<TemplateSpawner>();
			string errorMessage = "";
			List<ProcGen.World.TemplateSpawnRules> list2 = new List<ProcGen.World.TemplateSpawnRules>();
			list2.AddRange(worldTrait.additionalWorldTemplateRules);
			list2.Sort((ProcGen.World.TemplateSpawnRules a, ProcGen.World.TemplateSpawnRules b) => b.priority.CompareTo(a.priority));
			foreach (ProcGen.World.TemplateSpawnRules item in list2)
			{
				flag = ApplyTemplateRule(settings, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, item, ref usedTemplates, out errorMessage, ref newTemplateSpawnTargets);
				if (!flag)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				placedStoryTraits.Add(worldTrait);
				list.Add(worldTrait);
				settings.ApplyStoryTrait(worldTrait);
				DebugUtil.LogArgs("Applied story trait '" + worldTrait.filePath + "'");
				continue;
			}
			foreach (TemplateSpawner item2 in newTemplateSpawnTargets)
			{
				RemoveTemplate(item2, settings, terrainCells, ref templateSpawnTargets, ref placedPOIBounds);
				usedTemplates.Remove(item2.container.name);
			}
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				DebugUtil.LogArgs("Cannot place story trait on '" + worldTrait.filePath + "' and will try another world. error='" + errorMessage + "'.");
			}
			else
			{
				DebugUtil.LogArgs("Cannot place story trait '" + worldTrait.filePath + "' error='" + errorMessage + "'");
			}
		}
	}

	private static void RemoveTemplate(TemplateSpawner toRemove, WorldGenSettings settings, List<TerrainCell> terrainCells, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds)
	{
		UpdateNodeTags(toRemove.terrainCell.node, toRemove.container.name, remove: true);
		templateSpawnTargets.Remove(toRemove);
		placedPOIBounds.RemoveAll((RectInt bound) => bound.center == toRemove.position);
	}

	private static bool ApplyTemplateRule(WorldGenSettings settings, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, ProcGen.World.TemplateSpawnRules rule, ref HashSet<string> usedTemplates, out string errorMessage, ref List<TemplateSpawner> newTemplateSpawnTargets)
	{
		for (int i = 0; i < rule.times; i++)
		{
			ListPool<string, TemplateSpawning>.PooledList pooledList = ListPool<string, TemplateSpawning>.Allocate();
			if (!rule.allowDuplicates)
			{
				foreach (string name in rule.names)
				{
					if (!usedTemplates.Contains(name))
					{
						if (!TemplateCache.TemplateExists(name))
						{
							DebugUtil.DevLogError("TemplateSpawning: Missing template '" + name + "' in world '" + settings.world.filePath + "'");
						}
						else
						{
							pooledList.Add(name);
						}
					}
				}
			}
			else
			{
				pooledList.AddRange(rule.names);
			}
			pooledList.ShuffleSeeded(myRandom.RandomSource());
			if (pooledList.Count == 0)
			{
				pooledList.Recycle();
				continue;
			}
			int num = 0;
			if (rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange || rule.listRule == ProcGen.World.TemplateSpawnRules.ListRule.TryRange)
			{
				num = myRandom.RandomRange(rule.range.x, rule.range.y);
			}
			int num2 = 0;
			int num3 = 0;
			switch (rule.listRule)
			{
			case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeAll:
				num2 = pooledList.Count;
				num3 = pooledList.Count;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSome:
				num2 = rule.someCount;
				num3 = rule.someCount;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeSomeTryMore:
				num2 = rule.someCount;
				num3 = rule.someCount + rule.moreCount;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeOne:
				num2 = 1;
				num3 = 1;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.TryAll:
				num3 = pooledList.Count;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.TrySome:
				num3 = rule.someCount;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.TryOne:
				num3 = 1;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeRange:
				num2 = num;
				num3 = num;
				break;
			case ProcGen.World.TemplateSpawnRules.ListRule.TryRange:
				num3 = num;
				break;
			}
			string text = "";
			foreach (string item2 in pooledList)
			{
				if (num3 <= 0)
				{
					break;
				}
				TemplateContainer template = TemplateCache.GetTemplate(item2);
				if (template == null)
				{
					continue;
				}
				bool guarantee = num2 > 0;
				Vector2I position = Vector2I.zero;
				TerrainCell terrainCell;
				if (rule.overridePlacement != Vector2I.minusone)
				{
					position = rule.overridePlacement;
					terrainCell = terrainCells.Find((TerrainCell x) => x.poly.Contains(rule.overridePlacement));
					if (num2 > 0 && terrainCell.node.templateTag != Tag.Invalid)
					{
						errorMessage = $"Tried to place '{item2}' at ({position.x},{position.y}) using overridePlacement but '{terrainCell.node.templateTag}' is already there.";
						return false;
					}
				}
				else
				{
					terrainCell = FindTargetForTemplate(template, rule, terrainCells, myRandom, ref templateSpawnTargets, ref placedPOIBounds, guarantee, settings);
					if (terrainCell != null)
					{
						position = new Vector2I((int)terrainCell.poly.Centroid().x + rule.overrideOffset.x, (int)terrainCell.poly.Centroid().y + rule.overrideOffset.y);
					}
				}
				if (terrainCell != null)
				{
					RectInt templateBounds = template.GetTemplateBounds(position, s_poiPadding);
					TemplateSpawner item = new TemplateSpawner(position, templateBounds, template, terrainCell);
					templateSpawnTargets.Add(item);
					newTemplateSpawnTargets.Add(item);
					placedPOIBounds.Add(templateBounds);
					UpdateNodeTags(terrainCell.node, item2);
					usedTemplates.Add(item2);
					num3--;
					num2--;
				}
				else
				{
					text = text + "\n    - " + item2;
				}
			}
			pooledList.Recycle();
			if (num2 > 0)
			{
				string text2 = string.Join(", ", settings.GetWorldTraitIDs());
				string text3 = string.Join(", ", settings.GetStoryTraitIDs());
				errorMessage = "TemplateSpawning: Guaranteed placement failure on " + settings.world.filePath + "\n" + $"    listRule={rule.listRule} someCount={rule.someCount} moreCount={rule.moreCount} count={pooledList.Count}\n" + "    Could not place templates:" + text + "\n    world traits=" + text2 + "\n    story traits=" + text3;
				return false;
			}
		}
		errorMessage = "";
		return true;
	}

	private static void UpdateNodeTags(Node node, string template, bool remove = false)
	{
		Tag tag = template.ToTag();
		if (remove)
		{
			node.templateTag = Tag.Invalid;
			node.tags.Remove(tag);
			node.tags.Remove(WorldGenTags.POI);
		}
		else
		{
			node.templateTag = tag;
			node.tags.Add(template.ToTag());
			node.tags.Add(WorldGenTags.POI);
		}
	}

	private static TerrainCell FindTargetForTemplate(TemplateContainer template, ProcGen.World.TemplateSpawnRules rule, List<TerrainCell> terrainCells, SeededRandom myRandom, ref List<TemplateSpawner> templateSpawnTargets, ref List<RectInt> placedPOIBounds, bool guarantee, WorldGenSettings settings)
	{
		List<TerrainCell> filteredTerrainCells = (rule.allowNearStart ? terrainCells.FindAll(delegate(TerrainCell tc)
		{
			tc.LogInfo("Filtering Near Start", template.name, 0f);
			return tc.IsSafeToSpawnPOINearStart(terrainCells) && DoesCellMatchFilters(tc, rule.allowedCellsFilter);
		}) : (rule.useRelaxedFiltering ? terrainCells.FindAll(delegate(TerrainCell tc)
		{
			tc.LogInfo("Filtering Relaxed (replace features)", template.name, 0f);
			return tc.IsSafeToSpawnPOIRelaxed(terrainCells) && DoesCellMatchFilters(tc, rule.allowedCellsFilter);
		}) : terrainCells.FindAll(delegate(TerrainCell tc)
		{
			tc.LogInfo("Filtering", template.name, 0f);
			return tc.IsSafeToSpawnPOI(terrainCells) && DoesCellMatchFilters(tc, rule.allowedCellsFilter);
		})));
		RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
		if (filteredTerrainCells.Count == 0 && guarantee)
		{
			if (rule.allowNearStart && rule.useRelaxedFiltering)
			{
				DebugUtil.LogWarningArgs("Could not place " + template.name + " using normal rules, trying relaxed near start");
				filteredTerrainCells = terrainCells.FindAll(delegate(TerrainCell tc)
				{
					tc.LogInfo("Filtering Near Start Relaxed", template.name, 0f);
					return tc.IsSafeToSpawnPOINearStartRelaxed(terrainCells) && DoesCellMatchFilters(tc, rule.allowedCellsFilter);
				});
				RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
			}
			else if (!rule.useRelaxedFiltering)
			{
				DebugUtil.LogWarningArgs("Could not place " + template.name + " using normal rules, trying relaxed");
				filteredTerrainCells = terrainCells.FindAll(delegate(TerrainCell tc)
				{
					tc.LogInfo("Filtering Relaxed", template.name, 0f);
					return tc.IsSafeToSpawnPOIRelaxed(terrainCells) && DoesCellMatchFilters(tc, rule.allowedCellsFilter);
				});
				RemoveOverlappingPOIs(ref filteredTerrainCells, ref terrainCells, ref placedPOIBounds, template, settings, rule.allowExtremeTemperatureOverlap, rule.overrideOffset);
			}
		}
		if (filteredTerrainCells.Count == 0)
		{
			return null;
		}
		filteredTerrainCells.ShuffleSeeded(myRandom.RandomSource());
		return filteredTerrainCells[filteredTerrainCells.Count - 1];
	}

	private static bool IsPOIOverlappingBounds(List<RectInt> placedPOIBounds, RectInt templateBounds)
	{
		foreach (RectInt placedPOIBound in placedPOIBounds)
		{
			if (templateBounds.Overlaps(placedPOIBound))
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsPOIOverlappingHighTemperatureDelta(RectInt paddedTemplateBounds, SubWorld subworld, ref List<TerrainCell> allCells, WorldGenSettings settings)
	{
		Vector2 vector = 2f * Vector2.one * s_poiPadding;
		Vector2 vector2 = 2f * Vector2.one * 3f;
		Rect rect = new Rect(paddedTemplateBounds.position, paddedTemplateBounds.size - vector + vector2);
		Temperature temperature = SettingsCache.temperatures[subworld.temperatureRange];
		foreach (TerrainCell allCell in allCells)
		{
			SubWorld subWorld = settings.GetSubWorld(allCell.node.GetSubworld());
			Temperature temperature2 = SettingsCache.temperatures[subWorld.temperatureRange];
			if (subWorld.temperatureRange != subworld.temperatureRange)
			{
				float num = Mathf.Min(temperature.min, temperature2.min);
				float num2 = Mathf.Max(temperature.max, temperature2.max) - num;
				bool num3 = rect.Overlaps(allCell.poly.bounds);
				bool flag = num2 > 100f;
				if (num3 && flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void RemoveOverlappingPOIs(ref List<TerrainCell> filteredTerrainCells, ref List<TerrainCell> allCells, ref List<RectInt> placedPOIBounds, TemplateContainer container, WorldGenSettings settings, bool allowExtremeTemperatureOverlap, Vector2 poiOffset)
	{
		for (int num = filteredTerrainCells.Count - 1; num >= 0; num--)
		{
			TerrainCell terrainCell = filteredTerrainCells[num];
			int index = num;
			SubWorld subWorld = settings.GetSubWorld(terrainCell.node.GetSubworld());
			RectInt templateBounds = container.GetTemplateBounds(terrainCell.poly.Centroid() + poiOffset, s_poiPadding);
			bool flag = false;
			if (IsPOIOverlappingBounds(placedPOIBounds, templateBounds))
			{
				terrainCell.LogInfo("-> Removed due to overlapping POIs", "", 0f);
				flag = true;
			}
			else if (!allowExtremeTemperatureOverlap && IsPOIOverlappingHighTemperatureDelta(templateBounds, subWorld, ref allCells, settings))
			{
				terrainCell.LogInfo("-> Removed due to overlapping extreme temperature delta", "", 0f);
				flag = true;
			}
			if (flag)
			{
				filteredTerrainCells.RemoveAt(index);
			}
		}
	}

	private static bool DoesCellMatchFilters(TerrainCell cell, List<ProcGen.World.AllowedCellsFilter> filters)
	{
		bool flag = false;
		foreach (ProcGen.World.AllowedCellsFilter filter in filters)
		{
			bool applied;
			bool flag2 = DoesCellMatchFilter(cell, filter, out applied);
			if (!applied)
			{
				continue;
			}
			switch (filter.command)
			{
			case ProcGen.World.AllowedCellsFilter.Command.All:
				flag = true;
				break;
			case ProcGen.World.AllowedCellsFilter.Command.Clear:
				flag = false;
				break;
			case ProcGen.World.AllowedCellsFilter.Command.Replace:
				flag = flag2;
				break;
			case ProcGen.World.AllowedCellsFilter.Command.ExceptWith:
			case ProcGen.World.AllowedCellsFilter.Command.SymmetricExceptWith:
				if (flag2)
				{
					flag = false;
				}
				break;
			case ProcGen.World.AllowedCellsFilter.Command.UnionWith:
				flag = flag2 || flag;
				break;
			case ProcGen.World.AllowedCellsFilter.Command.IntersectWith:
				flag = flag2 && flag;
				break;
			}
			cell.LogInfo("-> DoesCellMatchFilter " + filter.command, flag2 ? "1" : "0", flag ? 1 : 0);
		}
		cell.LogInfo("> Final match", flag ? "true" : "false", 0f);
		return flag;
	}

	private static bool DoesCellMatchFilter(TerrainCell cell, ProcGen.World.AllowedCellsFilter filter, out bool applied)
	{
		applied = true;
		if (!ValidateFilter(filter))
		{
			return false;
		}
		if (filter.tagcommand != 0)
		{
			switch (filter.tagcommand)
			{
			case ProcGen.World.AllowedCellsFilter.TagCommand.Default:
				return true;
			case ProcGen.World.AllowedCellsFilter.TagCommand.AtTag:
				return cell.node.tags.Contains(filter.tag);
			case ProcGen.World.AllowedCellsFilter.TagCommand.NotAtTag:
				return !cell.node.tags.Contains(filter.tag);
			case ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag:
			{
				Tag tag = filter.tag.ToTag();
				bool flag = cell.distancesToTags.ContainsKey(tag);
				if (!flag && tag == WorldGenTags.AtStart && !filter.ignoreIfMissingTag)
				{
					DebugUtil.DevLogError("DistanceFromTag was used on a world without an AtStart tag, use ignoreIfMissingTag to skip it.");
				}
				else
				{
					Debug.Assert(flag || filter.ignoreIfMissingTag, "DistanceFromTag is missing tag " + filter.tag + ", use ignoreIfMissingTag to skip it.");
				}
				if (flag)
				{
					int num = cell.DistanceToTag(tag);
					if (num >= filter.minDistance)
					{
						return num <= filter.maxDistance;
					}
					return false;
				}
				applied = false;
				return true;
			}
			}
		}
		else
		{
			if (filter.subworldNames != null && filter.subworldNames.Count > 0)
			{
				foreach (string subworldName in filter.subworldNames)
				{
					if (cell.node.tags.Contains(subworldName))
					{
						return true;
					}
				}
				return false;
			}
			if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
			{
				foreach (SubWorld.ZoneType zoneType in filter.zoneTypes)
				{
					if (cell.node.tags.Contains(zoneType.ToString()))
					{
						return true;
					}
				}
				return false;
			}
			if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
			{
				foreach (Temperature.Range temperatureRange in filter.temperatureRanges)
				{
					if (cell.node.tags.Contains(temperatureRange.ToString()))
					{
						return true;
					}
				}
				return false;
			}
		}
		return true;
	}

	private static bool ValidateFilter(ProcGen.World.AllowedCellsFilter filter)
	{
		if (filter.command == ProcGen.World.AllowedCellsFilter.Command.All)
		{
			return true;
		}
		int num = 0;
		if (filter.tagcommand != 0)
		{
			num++;
		}
		if (filter.subworldNames != null && filter.subworldNames.Count > 0)
		{
			num++;
		}
		if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
		{
			num++;
		}
		if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
		{
			num++;
		}
		if (num != 1)
		{
			string text = "BAD ALLOWED CELLS FILTER in FEATURE RULES!";
			text += "\nA filter can only specify one of `tagcommand`, `subworldNames`, `zoneTypes`, or `temperatureRanges`.";
			text += "\nFound a filter with the following:";
			if (filter.tagcommand != 0)
			{
				text += "\ntagcommand:\n\t";
				text += filter.tagcommand;
				text += "\ntag:\n\t";
				text += filter.tag;
			}
			if (filter.subworldNames != null && filter.subworldNames.Count > 0)
			{
				text += "\nsubworldNames:\n\t";
				text += string.Join(", ", filter.subworldNames);
			}
			if (filter.zoneTypes != null && filter.zoneTypes.Count > 0)
			{
				text += "\nzoneTypes:\n";
				text += string.Join(", ", filter.zoneTypes);
			}
			if (filter.temperatureRanges != null && filter.temperatureRanges.Count > 0)
			{
				text += "\ntemperatureRanges:\n";
				text += string.Join(", ", filter.temperatureRanges);
			}
			Debug.LogError(text);
			return false;
		}
		return true;
	}
}
