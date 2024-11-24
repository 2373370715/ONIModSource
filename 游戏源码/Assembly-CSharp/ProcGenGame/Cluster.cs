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

namespace ProcGenGame
{
	// Token: 0x020020BA RID: 8378
	[Serializable]
	public class Cluster
	{
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600B232 RID: 45618 RVA: 0x00113C30 File Offset: 0x00111E30
		public ClusterLayout clusterLayout
		{
			get
			{
				return this.mutatedClusterLayout.layout;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600B233 RID: 45619 RVA: 0x00113C3D File Offset: 0x00111E3D
		// (set) Token: 0x0600B234 RID: 45620 RVA: 0x00113C45 File Offset: 0x00111E45
		public bool IsGenerationComplete { get; private set; }

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600B235 RID: 45621 RVA: 0x00113C4E File Offset: 0x00111E4E
		public bool IsGenerating
		{
			get
			{
				return this.thread != null && this.thread.IsAlive;
			}
		}

		// Token: 0x0600B236 RID: 45622 RVA: 0x00113C65 File Offset: 0x00111E65
		private Cluster()
		{
		}

		// Token: 0x0600B237 RID: 45623 RVA: 0x004336B4 File Offset: 0x004318B4
		public Cluster(string clusterName, int seed, List<string> chosenStoryTraitIds, bool assertMissingTraits, bool skipWorldTraits, bool isRunningWorldgenDebug = false)
		{
			DebugUtil.Assert(!string.IsNullOrEmpty(clusterName), "Cluster file is missing");
			this.seed = seed;
			this.Id = clusterName;
			this.assertMissingTraits = assertMissingTraits;
			this.worldTraitsEnabled = (seed > 0 && !skipWorldTraits);
			WorldGen.LoadSettings(false);
			this.InitializeWorlds(false, isRunningWorldgenDebug);
			this.unplacedStoryTraits = new List<WorldTrait>();
			if (!this.clusterLayout.disableStoryTraits)
			{
				this.chosenStoryTraitIds = chosenStoryTraitIds;
				using (List<string>.Enumerator enumerator = chosenStoryTraitIds.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string name = enumerator.Current;
						WorldTrait cachedStoryTrait = SettingsCache.GetCachedStoryTrait(name, assertMissingTraits);
						if (cachedStoryTrait != null)
						{
							this.unplacedStoryTraits.Add(cachedStoryTrait);
						}
					}
					goto IL_E9;
				}
			}
			this.chosenStoryTraitIds = new List<string>();
			IL_E9:
			if (CustomGameSettings.Instance != null)
			{
				foreach (string name2 in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
				{
					DlcMixingSettings cachedDlcMixingSettings = SettingsCache.GetCachedDlcMixingSettings(name2);
					if (cachedDlcMixingSettings != null && this.clusterLayout.poiPlacements != null)
					{
						this.clusterLayout.poiPlacements.AddRange(cachedDlcMixingSettings.spacePois);
					}
				}
			}
			if (this.clusterLayout.numRings > 0)
			{
				this.numRings = this.clusterLayout.numRings;
			}
		}

		// Token: 0x0600B238 RID: 45624 RVA: 0x00433850 File Offset: 0x00431A50
		public void InitializeWorlds(bool reuseWorldgen = false, bool isRunningWorldgenDebug = false)
		{
			this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(SettingsCache.clusterLayouts.clusterCache[this.Id], this.seed, isRunningWorldgenDebug, false);
			for (int i = 0; i < this.clusterLayout.worldPlacements.Count; i++)
			{
				ProcGen.World worldData = SettingsCache.worlds.GetWorldData(this.clusterLayout.worldPlacements[i], this.seed);
				if (worldData != null)
				{
					this.clusterLayout.worldPlacements[i].SetSize(worldData.worldsize);
					if (i == this.clusterLayout.startWorldIndex)
					{
						this.clusterLayout.worldPlacements[i].startWorld = true;
					}
				}
			}
			this.size = BestFit.BestFitWorlds(this.clusterLayout.worldPlacements, false);
			int num = this.seed;
			for (int j = 0; j < this.clusterLayout.worldPlacements.Count; j++)
			{
				WorldPlacement worldPlacement = this.clusterLayout.worldPlacements[j];
				List<string> chosenWorldTraits = new List<string>();
				ProcGen.World worldData2 = SettingsCache.worlds.GetWorldData(worldPlacement, num);
				if (this.worldTraitsEnabled)
				{
					chosenWorldTraits = SettingsCache.GetRandomTraits(num, worldData2);
					num++;
				}
				WorldGen worldGen;
				if (reuseWorldgen)
				{
					if (worldData2.name == this.worlds[j].Settings.world.name)
					{
						worldGen = this.worlds[j];
					}
					else
					{
						worldGen = new WorldGen(worldPlacement, num, chosenWorldTraits, null, this.assertMissingTraits);
						this.worlds[j] = worldGen;
					}
				}
				else
				{
					worldGen = new WorldGen(worldPlacement, num, chosenWorldTraits, null, this.assertMissingTraits);
					this.worlds.Add(worldGen);
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
					this.currentWorld = worldGen;
					worldGen.isStartingWorld = true;
				}
			}
			if (this.currentWorld == null)
			{
				DebugUtil.DevLogErrorFormat("Start world not set. Defaulting to first world {0}", new object[]
				{
					this.worlds[0].Settings.world.name
				});
				this.currentWorld = this.worlds[0];
			}
		}

		// Token: 0x0600B239 RID: 45625 RVA: 0x00113C9C File Offset: 0x00111E9C
		public void Reset()
		{
			this.worlds.Clear();
		}

		// Token: 0x0600B23A RID: 45626 RVA: 0x00433B38 File Offset: 0x00431D38
		private void LogBeginGeneration()
		{
			string str = (CustomGameSettings.Instance != null) ? CustomGameSettings.Instance.GetSettingsCoordinate() : this.seed.ToString();
			Console.WriteLine("\n\n");
			DebugUtil.LogArgs(new object[]
			{
				"WORLDGEN START"
			});
			DebugUtil.LogArgs(new object[]
			{
				" - seed:     " + str
			});
			DebugUtil.LogArgs(new object[]
			{
				" - cluster:  " + this.clusterLayout.filePath
			});
			if (this.chosenStoryTraitIds.Count == 0)
			{
				DebugUtil.LogArgs(new object[]
				{
					" - storytraits: none"
				});
				return;
			}
			DebugUtil.LogArgs(new object[]
			{
				" - storytraits:"
			});
			foreach (string str2 in this.chosenStoryTraitIds)
			{
				DebugUtil.LogArgs(new object[]
				{
					"    - " + str2
				});
			}
		}

		// Token: 0x0600B23B RID: 45627 RVA: 0x00433C50 File Offset: 0x00431E50
		public void Generate(WorldGen.OfflineCallbackFunction callbackFn, Action<OfflineWorldGen.ErrorInfo> error_cb, int worldSeed = -1, int layoutSeed = -1, int terrainSeed = -1, int noiseSeed = -1, bool doSimSettle = true, bool debug = false, bool skipPlacingTemplates = false)
		{
			this.doSimSettle = doSimSettle;
			for (int num = 0; num != this.worlds.Count; num++)
			{
				if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(num, this.worlds[num]))
				{
					this.worlds[num].Initialise(callbackFn, error_cb, worldSeed + num, layoutSeed + num, terrainSeed + num, noiseSeed + num, debug, skipPlacingTemplates);
				}
			}
			this.IsGenerationComplete = false;
			this.ApplicationIsPlaying = Application.isPlaying;
			this.thread = new Thread(new ThreadStart(this.ThreadMain));
			global::Util.ApplyInvariantCultureToThread(this.thread);
			this.thread.Start();
		}

		// Token: 0x0600B23C RID: 45628 RVA: 0x00113CA9 File Offset: 0x00111EA9
		private void StopThread()
		{
			this.thread = null;
		}

		// Token: 0x0600B23D RID: 45629 RVA: 0x00113CB2 File Offset: 0x00111EB2
		private bool IsRunningDebugGen()
		{
			return !this.ApplicationIsPlaying;
		}

		// Token: 0x0600B23E RID: 45630 RVA: 0x00433D04 File Offset: 0x00431F04
		private void BeginGeneration()
		{
			this.LogBeginGeneration();
			try
			{
				WorldgenMixing.DoSubworldMixing(this, this.seed, this.ShouldSkipWorldCallback, this.IsRunningDebugGen());
			}
			catch (WorldgenException ex)
			{
				if (!this.IsRunningDebugGen())
				{
					this.currentWorld.ReportWorldGenError(ex, ex.userMessage);
				}
				this.StopThread();
				return;
			}
			Sim.Cell[] arg = null;
			Sim.DiseaseCell[] arg2 = null;
			int num = 0;
			AxialI startLoc = this.worlds[0].GetClusterLocation();
			foreach (WorldGen worldGen in this.worlds)
			{
				if (worldGen.isStartingWorld)
				{
					startLoc = worldGen.GetClusterLocation();
				}
			}
			List<WorldGen> list = new List<WorldGen>(this.worlds);
			list.Sort(delegate(WorldGen a, WorldGen b)
			{
				int distance = AxialUtil.GetDistance(startLoc, a.GetClusterLocation());
				int distance2 = AxialUtil.GetDistance(startLoc, b.GetClusterLocation());
				if (distance == distance2)
				{
					return 0;
				}
				if (distance >= distance2)
				{
					return 1;
				}
				return -1;
			});
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream);
			for (int i = 0; i < list.Count; i++)
			{
				WorldGen worldGen2 = list[i];
				if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(i, worldGen2))
				{
					DebugUtil.Separator();
					DebugUtil.LogArgs(new object[]
					{
						"Generating world: " + worldGen2.Settings.world.filePath
					});
					if (worldGen2.Settings.GetWorldTraitIDs().Length != 0)
					{
						DebugUtil.LogArgs(new object[]
						{
							" - worldtraits: " + string.Join(", ", worldGen2.Settings.GetWorldTraitIDs().ToArray<string>())
						});
					}
					if (this.PerWorldGenBeginCallback != null)
					{
						this.PerWorldGenBeginCallback(i, worldGen2);
					}
					List<WorldTrait> list2 = new List<WorldTrait>();
					list2.AddRange(this.unplacedStoryTraits);
					worldGen2.Settings.SetStoryTraitCandidates(list2);
					GridSettings.Reset(worldGen2.GetSize().x, worldGen2.GetSize().y);
					if (!worldGen2.GenerateOffline())
					{
						this.StopThread();
						return;
					}
					worldGen2.FinalizeStartLocation();
					arg = null;
					arg2 = null;
					List<WorldTrait> list3 = new List<WorldTrait>();
					if (!worldGen2.RenderOffline(this.doSimSettle, writer, ref arg, ref arg2, num, ref list3, worldGen2.isStartingWorld))
					{
						this.StopThread();
						return;
					}
					if (this.PerWorldGenCompleteCallback != null)
					{
						this.PerWorldGenCompleteCallback(i, worldGen2, arg, arg2);
					}
					foreach (WorldTrait item in list3)
					{
						this.unplacedStoryTraits.Remove(item);
					}
					num++;
				}
			}
			if (this.unplacedStoryTraits.Count > 0)
			{
				List<string> list4 = new List<string>();
				foreach (WorldTrait worldTrait in this.unplacedStoryTraits)
				{
					list4.Add(worldTrait.filePath);
				}
				string message = "Story trait failure, unable to place on any world: " + string.Join(", ", list4.ToArray());
				if (!this.worlds[0].isRunningDebugGen)
				{
					this.worlds[0].ReportWorldGenError(new Exception(message), UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FAILURE_STORY);
				}
				this.StopThread();
				return;
			}
			DebugUtil.Separator();
			DebugUtil.LogArgs(new object[]
			{
				"Placing worlds on cluster map"
			});
			if (!this.AssignClusterLocations())
			{
				this.StopThread();
				return;
			}
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create));
			this.Save(binaryWriter);
			binaryWriter.Write(memoryStream.ToArray());
			this.StopThread();
			DebugUtil.Separator();
			DebugUtil.LogArgs(new object[]
			{
				"WORLDGEN COMPLETE\n\n\n"
			});
			this.IsGenerationComplete = true;
		}

		// Token: 0x0600B23F RID: 45631 RVA: 0x00113CBD File Offset: 0x00111EBD
		private bool IsValidHex(AxialI location)
		{
			return location.IsWithinRadius(AxialI.ZERO, this.numRings - 1);
		}

		// Token: 0x0600B240 RID: 45632 RVA: 0x004340F8 File Offset: 0x004322F8
		public bool AssignClusterLocations()
		{
			this.myRandom = new SeededRandom(this.seed);
			List<WorldPlacement> list = new List<WorldPlacement>(SettingsCache.clusterLayouts.clusterCache[this.Id].worldPlacements);
			List<SpaceMapPOIPlacement> list2 = (this.clusterLayout.poiPlacements == null) ? new List<SpaceMapPOIPlacement>() : new List<SpaceMapPOIPlacement>(this.clusterLayout.poiPlacements);
			this.currentWorld.SetClusterLocation(AxialI.ZERO);
			HashSet<AxialI> assignedLocations = new HashSet<AxialI>();
			HashSet<AxialI> worldForbiddenLocations = new HashSet<AxialI>();
			new HashSet<AxialI>();
			HashSet<AxialI> poiWorldAvoidance = new HashSet<AxialI>();
			int maxRadius = 2;
			for (int i = 0; i < this.worlds.Count; i++)
			{
				WorldGen worldGen = this.worlds[i];
				WorldPlacement worldPlacement = list[i];
				DebugUtil.Assert(worldPlacement != null, "Somehow we're trying to generate a cluster with a world that isn't the cluster .yaml's world list!", worldGen.Settings.world.filePath);
				HashSet<AxialI> antiBuffer = new HashSet<AxialI>();
				foreach (AxialI center in assignedLocations)
				{
					antiBuffer.UnionWith(AxialUtil.GetRings(center, 1, worldPlacement.buffer));
				}
				List<AxialI> list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1))
				where !assignedLocations.Contains(location) && !worldForbiddenLocations.Contains(location) && !antiBuffer.Contains(location)
				select location).ToList<AxialI>();
				if (list3.Count > 0)
				{
					AxialI axialI = list3[this.myRandom.RandomRange(0, list3.Count)];
					worldGen.SetClusterLocation(axialI);
					assignedLocations.Add(axialI);
					worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI, 1, worldPlacement.buffer));
					poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI, 1, maxRadius));
				}
				else
				{
					DebugUtil.DevLogError(string.Concat(new string[]
					{
						"Could not find a spot in the cluster for ",
						worldGen.Settings.world.filePath,
						". Check the placement settings in ",
						this.Id,
						".yaml to ensure there are no conflicts."
					}));
					HashSet<AxialI> minBuffers = new HashSet<AxialI>();
					foreach (AxialI center2 in assignedLocations)
					{
						minBuffers.UnionWith(AxialUtil.GetRings(center2, 1, 2));
					}
					list3 = (from location in AxialUtil.GetRings(AxialI.ZERO, worldPlacement.allowedRings.min, Mathf.Min(worldPlacement.allowedRings.max, this.numRings - 1))
					where !assignedLocations.Contains(location) && !minBuffers.Contains(location)
					select location).ToList<AxialI>();
					if (list3.Count <= 0)
					{
						string text = string.Concat(new string[]
						{
							"Could not find a spot in the cluster for ",
							worldGen.Settings.world.filePath,
							" EVEN AFTER REDUCING BUFFERS. Check the placement settings in ",
							this.Id,
							".yaml to ensure there are no conflicts."
						});
						DebugUtil.LogErrorArgs(new object[]
						{
							text
						});
						if (!worldGen.isRunningDebugGen)
						{
							this.currentWorld.ReportWorldGenError(new Exception(text), null);
						}
						return false;
					}
					AxialI axialI2 = list3[this.myRandom.RandomRange(0, list3.Count)];
					worldGen.SetClusterLocation(axialI2);
					assignedLocations.Add(axialI2);
					worldForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI2, 1, worldPlacement.buffer));
					poiWorldAvoidance.UnionWith(AxialUtil.GetRings(axialI2, 1, maxRadius));
				}
			}
			if (DlcManager.FeatureClusterSpaceEnabled() && list2 != null)
			{
				HashSet<AxialI> poiClumpLocations = new HashSet<AxialI>();
				HashSet<AxialI> poiForbiddenLocations = new HashSet<AxialI>();
				float num = 0.5f;
				int num2 = 3;
				int num3 = 0;
				Func<AxialI, bool> <>9__4;
				Func<AxialI, bool> <>9__2;
				Func<AxialI, bool> <>9__3;
				foreach (SpaceMapPOIPlacement spaceMapPOIPlacement in list2)
				{
					List<string> list4 = new List<string>(spaceMapPOIPlacement.pois);
					for (int j = 0; j < spaceMapPOIPlacement.numToSpawn; j++)
					{
						bool flag = this.myRandom.RandomRange(0f, 1f) <= num;
						List<AxialI> list5 = null;
						if (flag && num3 < num2 && !spaceMapPOIPlacement.avoidClumping)
						{
							num3++;
							IEnumerable<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, spaceMapPOIPlacement.allowedRings.min, Mathf.Min(spaceMapPOIPlacement.allowedRings.max, this.numRings - 1));
							Func<AxialI, bool> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((AxialI location) => !assignedLocations.Contains(location) && poiClumpLocations.Contains(location) && !poiWorldAvoidance.Contains(location)));
							}
							list5 = rings.Where(predicate).ToList<AxialI>();
						}
						if (list5 == null || list5.Count <= 0)
						{
							num3 = 0;
							poiClumpLocations.Clear();
							IEnumerable<AxialI> rings2 = AxialUtil.GetRings(AxialI.ZERO, spaceMapPOIPlacement.allowedRings.min, Mathf.Min(spaceMapPOIPlacement.allowedRings.max, this.numRings - 1));
							Func<AxialI, bool> predicate2;
							if ((predicate2 = <>9__3) == null)
							{
								predicate2 = (<>9__3 = ((AxialI location) => !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location) && !poiForbiddenLocations.Contains(location)));
							}
							list5 = rings2.Where(predicate2).ToList<AxialI>();
						}
						if (spaceMapPOIPlacement.guarantee && (list5 == null || list5.Count <= 0))
						{
							num3 = 0;
							poiClumpLocations.Clear();
							IEnumerable<AxialI> rings3 = AxialUtil.GetRings(AxialI.ZERO, spaceMapPOIPlacement.allowedRings.min, Mathf.Min(spaceMapPOIPlacement.allowedRings.max, this.numRings - 1));
							Func<AxialI, bool> predicate3;
							if ((predicate3 = <>9__4) == null)
							{
								predicate3 = (<>9__4 = ((AxialI location) => !assignedLocations.Contains(location) && !poiWorldAvoidance.Contains(location)));
							}
							list5 = rings3.Where(predicate3).ToList<AxialI>();
						}
						if (list5 != null && list5.Count > 0)
						{
							AxialI axialI3 = list5[this.myRandom.RandomRange(0, list5.Count)];
							string text2 = list4[this.myRandom.RandomRange(0, list4.Count)];
							if (!spaceMapPOIPlacement.canSpawnDuplicates)
							{
								list4.Remove(text2);
							}
							this.poiPlacements[axialI3] = text2;
							poiForbiddenLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 3));
							poiClumpLocations.UnionWith(AxialUtil.GetRings(axialI3, 1, 1));
							assignedLocations.Add(axialI3);
						}
						else
						{
							global::Debug.LogWarning(string.Format("There is no room for a Space POI in ring range [{0}, {1}] with pois: {2}", spaceMapPOIPlacement.allowedRings.min, spaceMapPOIPlacement.allowedRings.max, string.Join("\n - ", spaceMapPOIPlacement.pois.ToArray())));
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600B241 RID: 45633 RVA: 0x00113CD2 File Offset: 0x00111ED2
		public void AbortGeneration()
		{
			if (this.thread != null && this.thread.IsAlive)
			{
				this.thread.Abort();
				this.thread = null;
			}
		}

		// Token: 0x0600B242 RID: 45634 RVA: 0x00113CFB File Offset: 0x00111EFB
		private void ThreadMain()
		{
			this.BeginGeneration();
		}

		// Token: 0x0600B243 RID: 45635 RVA: 0x004348E4 File Offset: 0x00432AE4
		private void Save(BinaryWriter fileWriter)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
					{
						try
						{
							Manager.Clear();
							ClusterLayoutSave clusterLayoutSave = new ClusterLayoutSave();
							clusterLayoutSave.version = new Vector2I(1, 1);
							clusterLayoutSave.size = this.size;
							clusterLayoutSave.ID = this.Id;
							clusterLayoutSave.numRings = this.numRings;
							clusterLayoutSave.poiLocations = this.poiLocations;
							clusterLayoutSave.poiPlacements = this.poiPlacements;
							for (int num = 0; num != this.worlds.Count; num++)
							{
								WorldGen worldGen = this.worlds[num];
								if (this.ShouldSkipWorldCallback == null || !this.ShouldSkipWorldCallback(num, worldGen))
								{
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
										traits = worldGen.Settings.GetWorldTraitIDs().ToList<string>(),
										storyTraits = worldGen.Settings.GetStoryTraitIDs().ToList<string>(),
										seasons = worldGen.Settings.world.seasons,
										generatedSubworlds = hashSet.ToList<string>()
									});
									if (worldGen == this.currentWorld)
									{
										clusterLayoutSave.currentWorldIdx = num;
									}
								}
							}
							Serializer.Serialize(clusterLayoutSave, binaryWriter);
						}
						catch (Exception ex)
						{
							DebugUtil.LogErrorArgs(new object[]
							{
								"Couldn't serialize",
								ex.Message,
								ex.StackTrace
							});
						}
					}
					Manager.SerializeDirectory(fileWriter);
					fileWriter.Write(memoryStream.ToArray());
				}
			}
			catch (Exception ex2)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"Couldn't write",
					ex2.Message,
					ex2.StackTrace
				});
			}
		}

		// Token: 0x0600B244 RID: 45636 RVA: 0x00434B90 File Offset: 0x00432D90
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
					WorldGen worldGen = WorldGen.Load(reader, true);
					cluster.worlds.Add(worldGen);
					cluster.size = worldGen.GetSize();
					cluster.currentWorld = (cluster.worlds[0] ?? null);
				}
				else
				{
					for (int num = 0; num != clusterLayoutSave.worlds.Count; num++)
					{
						ClusterLayoutSave.World world = clusterLayoutSave.worlds[num];
						WorldGen worldGen2 = new WorldGen(world.name, world.data, world.traits, world.storyTraits, false);
						worldGen2.Settings.world.ReplaceSeasons(world.seasons);
						worldGen2.Settings.world.generatedSubworlds = world.generatedSubworlds;
						cluster.worlds.Add(worldGen2);
						if (num == clusterLayoutSave.currentWorldIdx)
						{
							cluster.currentWorld = worldGen2;
							cluster.worlds[num].isStartingWorld = true;
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
				DebugUtil.LogErrorArgs(new object[]
				{
					"SolarSystem.Load Error!\n",
					ex.Message,
					ex.StackTrace
				});
				cluster = null;
			}
			return cluster;
		}

		// Token: 0x0600B245 RID: 45637 RVA: 0x00434D7C File Offset: 0x00432F7C
		public void LoadClusterSim(List<SimSaveFileStructure> loadedWorlds, FastReader reader)
		{
			try
			{
				for (int num = 0; num != this.worlds.Count; num++)
				{
					SimSaveFileStructure simSaveFileStructure = new SimSaveFileStructure();
					Manager.DeserializeDirectory(reader);
					Deserializer.Deserialize(simSaveFileStructure, reader);
					if (simSaveFileStructure.worldDetail == null)
					{
						if (!GenericGameSettings.instance.devAutoWorldGenActive)
						{
							global::Debug.LogError("Detail is null for world " + num.ToString());
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
					DebugUtil.LogErrorArgs(new object[]
					{
						"LoadSim Error!\n",
						ex.Message,
						ex.StackTrace
					});
				}
			}
		}

		// Token: 0x0600B246 RID: 45638 RVA: 0x00434E2C File Offset: 0x0043302C
		public void SetIsRunningDebug(bool isDebug)
		{
			foreach (WorldGen worldGen in this.worlds)
			{
				worldGen.isRunningDebugGen = isDebug;
			}
		}

		// Token: 0x0600B247 RID: 45639 RVA: 0x00113D03 File Offset: 0x00111F03
		public void DEBUG_UpdateSeed(int seed)
		{
			this.seed = seed;
			this.InitializeWorlds(true, true);
		}

		// Token: 0x0600B248 RID: 45640 RVA: 0x00434E80 File Offset: 0x00433080
		public int MaxSupportedSubworldMixings()
		{
			int num = 0;
			foreach (WorldGen worldGen in this.worlds)
			{
				num += worldGen.Settings.world.subworldMixingRules.Count;
			}
			return num;
		}

		// Token: 0x0600B249 RID: 45641 RVA: 0x00434EE8 File Offset: 0x004330E8
		public int MaxSupportedWorldMixings()
		{
			int num = 0;
			foreach (WorldPlacement worldPlacement in this.clusterLayout.worldPlacements)
			{
				if (worldPlacement.worldMixing != null && (worldPlacement.worldMixing.requiredTags.Count != 0 || worldPlacement.worldMixing.forbiddenTags.Count != 0))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04008CAE RID: 36014
		public List<WorldGen> worlds = new List<WorldGen>();

		// Token: 0x04008CAF RID: 36015
		public WorldGen currentWorld;

		// Token: 0x04008CB0 RID: 36016
		public Vector2I size;

		// Token: 0x04008CB1 RID: 36017
		public string Id;

		// Token: 0x04008CB2 RID: 36018
		public int numRings = 5;

		// Token: 0x04008CB3 RID: 36019
		public bool worldTraitsEnabled;

		// Token: 0x04008CB4 RID: 36020
		public bool assertMissingTraits;

		// Token: 0x04008CB5 RID: 36021
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x04008CB6 RID: 36022
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x04008CB7 RID: 36023
		private int seed;

		// Token: 0x04008CB8 RID: 36024
		private SeededRandom myRandom;

		// Token: 0x04008CB9 RID: 36025
		private bool doSimSettle = true;

		// Token: 0x04008CBA RID: 36026
		[NonSerialized]
		public Action<int, WorldGen> PerWorldGenBeginCallback;

		// Token: 0x04008CBB RID: 36027
		[NonSerialized]
		public Action<int, WorldGen, Sim.Cell[], Sim.DiseaseCell[]> PerWorldGenCompleteCallback;

		// Token: 0x04008CBC RID: 36028
		[NonSerialized]
		public Func<int, WorldGen, bool> ShouldSkipWorldCallback;

		// Token: 0x04008CBD RID: 36029
		[NonSerialized]
		public List<WorldTrait> unplacedStoryTraits;

		// Token: 0x04008CBE RID: 36030
		[NonSerialized]
		public List<string> chosenStoryTraitIds;

		// Token: 0x04008CBF RID: 36031
		private MutatedClusterLayout mutatedClusterLayout;

		// Token: 0x04008CC0 RID: 36032
		private Thread thread;

		// Token: 0x04008CC2 RID: 36034
		private bool ApplicationIsPlaying;
	}
}
