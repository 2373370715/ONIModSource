using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace ProcGenGame;

[Serializable]
public class Cluster
{
	public List<WorldGen> worlds = new List<WorldGen>();

	public WorldGen currentWorld;

	public Vector2I size;

	public string Id;

	public int numRings = 5;

	public bool worldTraitsEnabled;

	public bool assertMissingTraits;

	public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

	public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

	private int seed;

	private SeededRandom myRandom;

	private bool doSimSettle = true;

	[NonSerialized]
	public Action<int, WorldGen> PerWorldGenBeginCallback;

	[NonSerialized]
	public Action<int, WorldGen, Sim.Cell[], Sim.DiseaseCell[]> PerWorldGenCompleteCallback;

	[NonSerialized]
	public Func<int, WorldGen, bool> ShouldSkipWorldCallback;

	[NonSerialized]
	public List<WorldTrait> unplacedStoryTraits;

	[NonSerialized]
	public List<string> chosenStoryTraitIds;

	private MutatedClusterLayout mutatedClusterLayout;

	private Thread thread;

	private bool ApplicationIsPlaying;

	public ClusterLayout clusterLayout => mutatedClusterLayout.layout;

	public bool IsGenerationComplete { get; private set; }

	public bool IsGenerating
	{
		get
		{
			if (thread != null)
			{
				return thread.IsAlive;
			}
			return false;
		}
	}

	private Cluster()
	{
	}

	public Cluster(string clusterName, int seed, List<string> chosenStoryTraitIds, bool assertMissingTraits, bool skipWorldTraits, bool isRunningWorldgenDebug = false)
	{
		DebugUtil.Assert(!string.IsNullOrEmpty(clusterName), "Cluster file is missing");
		this.seed = seed;
		Id = clusterName;
		this.assertMissingTraits = assertMissingTraits;
		worldTraitsEnabled = seed > 0 && !skipWorldTraits;
		WorldGen.LoadSettings();
		InitializeWorlds(reuseWorldgen: false, isRunningWorldgenDebug);
		unplacedStoryTraits = new List<WorldTrait>();
		if (!clusterLayout.disableStoryTraits)
		{
			this.chosenStoryTraitIds = chosenStoryTraitIds;
			foreach (string chosenStoryTraitId in chosenStoryTraitIds)
			{
				WorldTrait cachedStoryTrait = SettingsCache.GetCachedStoryTrait(chosenStoryTraitId, assertMissingTraits);
				if (cachedStoryTrait != null)
				{
					unplacedStoryTraits.Add(cachedStoryTrait);
				}
			}
		}
		else
		{
			this.chosenStoryTraitIds = new List<string>();
		}
		if (CustomGameSettings.Instance != null)
		{
			foreach (string currentDlcMixingId in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
			{
				DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(currentDlcMixingId);
				if (clusterLayout.poiPlacements != null)
				{
					clusterLayout.poiPlacements.AddRange(cachedDlcMixingSettings.spacePois);
				}
			}
		}
		if (clusterLayout.numRings > 0)
		{
			numRings = clusterLayout.numRings;
		}
	}

	public void InitializeWorlds(bool reuseWorldgen = false, bool isRunningWorldgenDebug = false)
	{
		mutatedClusterLayout = WorldgenMixing.DoWorldMixing(SettingsCache.clusterLayouts.clusterCache[Id], seed, isRunningWorldgenDebug, muteErrors: false);
		for (int i = 0; i < clusterLayout.worldPlacements.Count; i++)
		{
			ProcGen.World worldData = SettingsCache.worlds.GetWorldData(clusterLayout.worldPlacements[i], seed);
			if (worldData != null)
			{
				clusterLayout.worldPlacements[i].SetSize(worldData.worldsize);
				if (i == clusterLayout.startWorldIndex)
				{
					clusterLayout.worldPlacements[i].startWorld = true;
				}
			}
		}
		size = BestFit.BestFitWorlds(clusterLayout.worldPlacements);
		int num = seed;
		for (int j = 0; j < clusterLayout.worldPlacements.Count; j++)
		{
			WorldPlacement worldPlacement = clusterLayout.worldPlacements[j];
			List<string> chosenWorldTraits = new List<string>();
			ProcGen.World worldData2 = SettingsCache.worlds.GetWorldData(worldPlacement, num);
			if (worldTraitsEnabled)
			{
				chosenWorldTraits = SettingsCache.GetRandomTraits(num, worldData2);
				num++;
			}
			WorldGen worldGen;
			if (reuseWorldgen)
			{
				if (worldData2.name == worlds[j].Settings.world.name)
				{
					worldGen = worlds[j];
				}
				else
				{
					worldGen = new WorldGen(worldPlacement, num, chosenWorldTraits, null, assertMissingTraits);
					worlds[j] = worldGen;
				}
			}
			else
			{
				worldGen = new WorldGen(worldPlacement, num, chosenWorldTraits, null, assertMissingTraits);
				worlds.Add(worldGen);
			}
			Vector2I worldsize = worldGen.Settings.world.worldsize;
			worldGen.SetWorldSize(worldsize.x, worldsize.y);
			worldGen.SetPosition(new Vector2I(worldPlacement.x, worldPlacement.y));
			if (!reuseWorldgen && worldPlacement.worldMixing.mixingWasApplied)
			{
				worldGen.Settings.world.worldTemplateRules.AddRange(worldPlacement.worldMixing.additionalWorldTemplateRules);
				worldGen.Settings.world.subworldFiles.AddRange(worldPlacement.worldMixing.additionalSubworldFiles);
				worldGen.Settings.world.AddUnknownCellsAllowedSubworlds(worldPlacement.worldMixing.additionalUnknownCellFilters);
				worldGen.Settings.world.AddSeasons(worldPlacement.worldMixing.additionalSeasons);
			}
			if (worldPlacement.startWorld)
			{
				currentWorld = worldGen;
				worldGen.isStartingWorld = true;
			}
		}
		if (currentWorld == null)
		{
			DebugUtil.DevLogErrorFormat("Start world not set. Defaulting to first world {0}", worlds[0].Settings.world.name);
			currentWorld = worlds[0];
		}
	}

	public void Reset()
	{
		worlds.Clear();
	}

	private void LogBeginGeneration()
	{
		string text = ((CustomGameSettings.Instance != null) ? CustomGameSettings.Instance.GetSettingsCoordinate() : seed.ToString());
		Console.WriteLine("\n\n");
		DebugUtil.LogArgs("WORLDGEN START");
		DebugUtil.LogArgs(" - seed:     " + text);
		DebugUtil.LogArgs(" - cluster:  " + clusterLayout.filePath);
		if (chosenStoryTraitIds.Count == 0)
		{
			DebugUtil.LogArgs(" - storytraits: none");
			return;
		}
		DebugUtil.LogArgs(" - storytraits:");
		foreach (string chosenStoryTraitId in chosenStoryTraitIds)
		{
			DebugUtil.LogArgs("    - " + chosenStoryTraitId);
		}
	}

	public void Generate(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed = -1, int layoutSeed = -1, int terrainSeed = -1, int noiseSeed = -1, bool doSimSettle = true, bool debug = false, bool skipPlacingTemplates = false)
	{
		this.doSimSettle = doSimSettle;
		for (int i = 0; i != worlds.Count; i++)
		{
			if (ShouldSkipWorldCallback == null || !ShouldSkipWorldCallback(i, worlds[i]))
			{
				worlds[i].Initialise(callbackFn, error_cb, worldSeed + i, layoutSeed + i, terrainSeed + i, noiseSeed + i, debug, skipPlacingTemplates);
			}
		}
		IsGenerationComplete = false;
		ApplicationIsPlaying = Application.isPlaying;
		thread = new Thread(ThreadMain);
		Util.ApplyInvariantCultureToThread(thread);
		thread.Start();
	}

	private void StopThread()
	{
		thread = null;
	}

	private bool IsRunningDebugGen()
	{
		return !ApplicationIsPlaying;
	}

	private void BeginGeneration()
	{
		LogBeginGeneration();
		try
		{
			WorldgenMixing.DoSubworldMixing(this, seed, ShouldSkipWorldCallback, IsRunningDebugGen());
		}
		catch (WorldgenException ex)
		{
			if (!IsRunningDebugGen())
			{
				currentWorld.ReportWorldGenError(ex, ex.userMessage);
			}
			StopThread();
			return;
		}
		Sim.Cell[] array = null;
		Sim.DiseaseCell[] array2 = null;
		int num = 0;
		AxialI startLoc = worlds[0].GetClusterLocation();
		foreach (WorldGen world in worlds)
		{
			if (world.isStartingWorld)
			{
				startLoc = world.GetClusterLocation();
			}
		}
		List<WorldGen> list = new List<WorldGen>(worlds);
		list.Sort(delegate(WorldGen a, WorldGen b)
		{
			int distance = AxialUtil.GetDistance(startLoc, a.GetClusterLocation());
			int distance2 = AxialUtil.GetDistance(startLoc, b.GetClusterLocation());
			if (distance == distance2)
			{
				return 0;
			}
			return (distance >= distance2) ? 1 : (-1);
		});
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(memoryStream);
		for (int i = 0; i < list.Count; i++)
		{
			WorldGen worldGen = list[i];
			if (ShouldSkipWorldCallback != null && ShouldSkipWorldCallback(i, worldGen))
			{
				continue;
			}
			DebugUtil.Separator();
			DebugUtil.LogArgs("Generating world: " + worldGen.Settings.world.filePath);
			if (worldGen.Settings.GetWorldTraitIDs().Length != 0)
			{
				DebugUtil.LogArgs(" - worldtraits: " + string.Join(", ", worldGen.Settings.GetWorldTraitIDs().ToArray()));
			}
			if (PerWorldGenBeginCallback != null)
			{
				PerWorldGenBeginCallback(i, worldGen);
			}
			List<WorldTrait> list2 = new List<WorldTrait>();
			list2.AddRange(unplacedStoryTraits);
			worldGen.Settings.SetStoryTraitCandidates(list2);
			GridSettings.Reset(worldGen.GetSize().x, worldGen.GetSize().y);
			if (!worldGen.GenerateOffline())
			{
				StopThread();
				return;
			}
			worldGen.FinalizeStartLocation();
			array = null;
			array2 = null;
			List<WorldTrait> placedStoryTraits = new List<WorldTrait>();
			if (!worldGen.RenderOffline(doSimSettle, writer, ref array, ref array2, num, ref placedStoryTraits, worldGen.isStartingWorld))
			{
				StopThread();
				return;
			}
			if (PerWorldGenCompleteCallback != null)
			{
				PerWorldGenCompleteCallback(i, worldGen, array, array2);
			}
			foreach (WorldTrait item in placedStoryTraits)
			{
				unplacedStoryTraits.Remove(item);
			}
			num++;
		}
		if (unplacedStoryTraits.Count > 0)
		{
			List<string> list3 = new List<string>();
			foreach (WorldTrait unplacedStoryTrait in unplacedStoryTraits)
			{
				list3.Add(unplacedStoryTrait.filePath);
			}
			string message = "Story trait failure, unable to place on any world: " + string.Join(", ", list3.ToArray());
			if (!worlds[0].isRunningDebugGen)
			{
				worlds[0].ReportWorldGenError(new Exception(message), UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_STORY);
			}
			StopThread();
			return;
		}
		DebugUtil.Separator();
		DebugUtil.LogArgs("Placing worlds on cluster map");
		if (!AssignClusterLocations())
		{
			StopThread();
			return;
		}
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create));
		Save(binaryWriter);
		binaryWriter.Write(memoryStream.ToArray());
		StopThread();
		DebugUtil.Separator();
		DebugUtil.LogArgs("WORLDGEN COMPLETE\n\n\n");
		IsGenerationComplete = true;
	}

	private bool IsValidHex(AxialI location)
	{
		return location.IsWithinRadius(AxialI.ZERO, numRings - 1);
	}

	public bool AssignClusterLocations()
	{
		myRandom = new SeededRandom(seed);
		List<WorldPlacement> list = new List<WorldPlacement>(SettingsCache.clusterLayouts.clusterCache[Id].worldPlacements);
		List<SpaceMapPOIPlacement> list2 = ((clusterLayout.poiPlacements == null) ? new List<SpaceMapPOIPlacement>() : new List<SpaceMapPOIPlacement>(clusterLayout.poiPlacements));
		currentWorld.SetClusterLocation(AxialI.ZERO);
		HashSet<AxialI> assignedLocations = new HashSet<AxialI>();
		HashSet<AxialI> worldForbiddenLocations = new HashSet<AxialI>();
		new HashSet<AxialI>();
		HashSet<AxialI> poiWorldAvoidance = new HashSet<AxialI>();
		int maxRadius = 2;
		for (int i = 0; i < worlds.Count; i++)
		{
			WorldGen worldGen = worlds[i];
			WorldPlacement worldPlacement = list[i];
			DebugUtil.Assert(worldPlacement != null, "Somehow we're trying to generate a cluster with a world that isn't the cluster .yaml's world list!", worldGen.Settings.world.filePath);
			HashSet<AxialI> antiBuffer = new HashSet<AxialI>();
			foreach (AxialI item in assignedLocations)
			{
				antiBuffer.UnionWith(AxialUtil.GetRings(item, 1, worldPlacement.buffer));
			}
			List<AxialI> list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, numRings - 1))
				where !assignedLocations.Contains(location) && !worldForbiddenLocations.Contains(location) && !antiBuffer.Contains(location)
				select location).ToList();
			if (list3.Count > 0)
			{
				AxialI axialI = list3[myRandom.RandomRange(0, list3.Count)];
				worldGen.SetClusterLocation(axialI);
				assignedLocations.Add(axialI);
				worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI, 1, worldPlacement.buffer));
				poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI, 1, maxRadius));
				continue;
			}
			DebugUtil.DevLogError("Could not find a spot in the cluster for " + worldGen.Settings.world.filePath + ". Check the placement settings in " + Id + ".yaml to ensure there are no conflicts.");
			HashSet<AxialI> minBuffers = new HashSet<AxialI>();
			foreach (AxialI item2 in assignedLocations)
			{
				minBuffers.UnionWith(AxialUtil.GetRings(item2, 1, 2));
			}
			list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, numRings - 1))
				where !assignedLocations.Contains(location) && !minBuffers.Contains(location)
				select location).ToList();
			if (list3.Count > 0)
			{
				AxialI axialI2 = list3[myRandom.RandomRange(0, list3.Count)];
				worldGen.SetClusterLocation(axialI2);
				assignedLocations.Add(axialI2);
				worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI2, 1, worldPlacement.buffer));
				poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI2, 1, maxRadius));
				continue;
			}
			string text = "Could not find a spot in the cluster for " + worldGen.Settings.world.filePath + " EVEN AFTER REDUCING BUFFERS. Check the placement settings in " + Id + ".yaml to ensure there are no conflicts.";
			DebugUtil.LogErrorArgs(text);
			if (!worldGen.isRunningDebugGen)
			{
				currentWorld.ReportWorldGenError(new Exception(text));
			}
			return false;
		}
		if (DlcManager.FeatureClusterSpaceEnabled() && list2 != null)
		{
			HashSet<AxialI> poiClumpLocations = new HashSet<AxialI>();
			HashSet<AxialI> poiForbiddenLocations = new HashSet<AxialI>();
			float num = 0.5f;
			int num2 = 3;
			int num3 = 0;
			foreach (SpaceMapPOIPlacement item3 in list2)
			{
				List<string> list4 = new List<string>(item3.pois);
				for (int j = 0; j < item3.numToSpawn; j++)
				{
					bool num4 = myRandom.RandomRange(0f, 1f) <= num;
					List<AxialI> list5 = null;
					if (num4 && num3 < num2 && !item3.avoidClumping)
					{
						num3++;
						list5 = (from location in AxialUtil.GetRings(AxialI.ZERO, item3.allowedRings.min, Mathf.Min(item3.allowedRings.max, numRings - 1))
							where !assignedLocations.Contains(location) && poiClumpLocations.Contains(location) && !poiWorldAvoidance.Contains(location)
							select location).ToList();
					}
					if (list5 == null || list5.Count <= 0)
					{
						num3 = 0;
						poiClumpLocations.Clear();
						list5 = (from location in AxialUtil.GetRings(AxialI.ZERO, item3.allowedRings.min, Mathf.Min(item3.allowedRings.max, numRings - 1))
							where !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location) && !poiForbiddenLocations.Contains(location)
							select location).ToList();
					}
					if (item3.guarantee && (list5 == null || list5.Count <= 0))
					{
						num3 = 0;
						poiClumpLocations.Clear();
						list5 = (from location in AxialUtil.GetRings(AxialI.ZERO, item3.allowedRings.min, Mathf.Min(item3.allowedRings.max, numRings - 1))
							where !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location)
							select location).ToList();
					}
					if (list5 != null && list5.Count > 0)
					{
						AxialI axialI3 = list5[myRandom.RandomRange(0, list5.Count)];
						string text2 = list4[myRandom.RandomRange(0, list4.Count)];
						if (!item3.canSpawnDuplicates)
						{
							list4.Remove(text2);
						}
						poiPlacements[axialI3] = text2;
						poiForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 3));
						poiClumpLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 1));
						assignedLocations.Add(axialI3);
					}
					else
					{
						Debug.LogWarning(string.Format("There is no room for a Space POI in ring range [{0}, {1}] with pois: {2}", item3.allowedRings.min, item3.allowedRings.max, string.Join("\n - ", item3.pois.ToArray())));
					}
				}
			}
		}
		return true;
	}

	public void AbortGeneration()
	{
		if (thread != null && thread.IsAlive)
		{
			thread.Abort();
			thread = null;
		}
	}

	private void ThreadMain()
	{
		BeginGeneration();
	}

	private void Save(BinaryWriter fileWriter)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter writer = new BinaryWriter(memoryStream))
			{
				try
				{
					Manager.Clear();
					ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
					clusterLayoutSave.version = new Vector2I(1, 1);
					clusterLayoutSave.size = size;
					clusterLayoutSave.ID = Id;
					clusterLayoutSave.numRings = numRings;
					clusterLayoutSave.poiLocations = poiLocations;
					clusterLayoutSave.poiPlacements = poiPlacements;
					for (int i = 0; i != worlds.Count; i++)
					{
						WorldGen worldGen = worlds[i];
						if (ShouldSkipWorldCallback != null && ShouldSkipWorldCallback(i, worldGen))
						{
							continue;
						}
						HashSet<string> hashSet = new HashSet<string>();
						foreach (TerrainCell terrainCell in worldGen.TerrainCells)
						{
							hashSet.Add(terrainCell.node.GetSubworld());
						}
						clusterLayoutSave.worlds.Add(new ClusterLayoutSave.World
						{
							data = worldGen.data,
							name = worldGen.Settings.world.filePath,
							isDiscovered = worldGen.isStartingWorld,
							traits = worldGen.Settings.GetWorldTraitIDs().ToList(),
							storyTraits = worldGen.Settings.GetStoryTraitIDs().ToList(),
							seasons = worldGen.Settings.world.seasons,
							generatedSubworlds = hashSet.ToList()
						});
						if (worldGen == currentWorld)
						{
							clusterLayoutSave.currentWorldIdx = i;
						}
					}
					Serializer.Serialize(clusterLayoutSave, writer);
				}
				catch (Exception ex)
				{
					DebugUtil.LogErrorArgs("Couldn't serialize", ex.Message, ex.StackTrace);
				}
			}
			Manager.SerializeDirectory(fileWriter);
			fileWriter.Write(memoryStream.ToArray());
		}
		catch (Exception ex2)
		{
			DebugUtil.LogErrorArgs("Couldn't write", ex2.Message, ex2.StackTrace);
		}
	}

	public static Cluster Load(FastReader reader)
	{
		Cluster cluster = new Cluster();
		try
		{
			Manager.DeserializeDirectory(reader);
			int position = reader.Position;
			ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
			if (!Deserializer.Deserialize(clusterLayoutSave, reader))
			{
				reader.Position = position;
				WorldGen worldGen = WorldGen.Load(reader, defaultDiscovered: true);
				cluster.worlds.Add(worldGen);
				cluster.size = worldGen.GetSize();
				cluster.currentWorld = cluster.worlds[0] ?? null;
			}
			else
			{
				for (int i = 0; i != clusterLayoutSave.worlds.Count; i++)
				{
					ClusterLayoutSave.World world = clusterLayoutSave.worlds[i];
					WorldGen worldGen2 = new WorldGen(world.name, world.data, world.traits, world.storyTraits, assertMissingTraits: false);
					worldGen2.Settings.world.ReplaceSeasons(world.seasons);
					worldGen2.Settings.world.generatedSubworlds = world.generatedSubworlds;
					cluster.worlds.Add(worldGen2);
					if (i == clusterLayoutSave.currentWorldIdx)
					{
						cluster.currentWorld = worldGen2;
						cluster.worlds[i].isStartingWorld = true;
					}
				}
				cluster.size = clusterLayoutSave.size;
				cluster.Id = clusterLayoutSave.ID;
				cluster.numRings = clusterLayoutSave.numRings;
				cluster.poiLocations = clusterLayoutSave.poiLocations;
				cluster.poiPlacements = clusterLayoutSave.poiPlacements;
			}
			DebugUtil.Assert(cluster.currentWorld != null);
			if (cluster.currentWorld == null)
			{
				DebugUtil.Assert(0 < cluster.worlds.Count);
				cluster.currentWorld = cluster.worlds[0];
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs("SolarSystem.Load Error!\n", ex.Message, ex.StackTrace);
			cluster = null;
		}
		return cluster;
	}

	public void LoadClusterSim(List<SimSaveFileStructure> loadedWorlds, FastReader reader)
	{
		try
		{
			for (int i = 0; i != worlds.Count; i++)
			{
				SimSaveFileStructure simSaveFileStructure = new SimSaveFileStructure();
				Manager.DeserializeDirectory(reader);
				Deserializer.Deserialize(simSaveFileStructure, reader);
				if (simSaveFileStructure.worldDetail == null)
				{
					if (!GenericGameSettings.instance.devAutoWorldGenActive)
					{
						Debug.LogError("Detail is null for world " + i);
					}
				}
				else
				{
					loadedWorlds.Add(simSaveFileStructure);
				}
			}
		}
		catch (Exception ex)
		{
			if (!GenericGameSettings.instance.devAutoWorldGenActive)
			{
				DebugUtil.LogErrorArgs("LoadSim Error!\n", ex.Message, ex.StackTrace);
			}
		}
	}

	public void SetIsRunningDebug(bool isDebug)
	{
		foreach (WorldGen world in worlds)
		{
			world.isRunningDebugGen = isDebug;
		}
	}

	public void DEBUG_UpdateSeed(int seed)
	{
		this.seed = seed;
		InitializeWorlds(reuseWorldgen: true, isRunningWorldgenDebug: true);
	}

	public int MaxSupportedSubworldMixings()
	{
		int num = 0;
		foreach (WorldGen world in worlds)
		{
			num += world.Settings.world.subworldMixingRules.Count;
		}
		return num;
	}

	public int MaxSupportedWorldMixings()
	{
		int num = 0;
		foreach (WorldPlacement worldPlacement in clusterLayout.worldPlacements)
		{
			if (worldPlacement.worldMixing != null && (worldPlacement.worldMixing.requiredTags.Count != 0 || worldPlacement.worldMixing.forbiddenTags.Count != 0))
			{
				num++;
			}
		}
		return num;
	}
}
