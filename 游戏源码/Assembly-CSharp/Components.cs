using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010CB RID: 4299
public class Components
{
	// Token: 0x04003DBC RID: 15804
	public static Components.Cmps<RobotAi.Instance> LiveRobotsIdentities = new Components.Cmps<RobotAi.Instance>();

	// Token: 0x04003DBD RID: 15805
	public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04003DBE RID: 15806
	public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04003DBF RID: 15807
	public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();

	// Token: 0x04003DC0 RID: 15808
	public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();

	// Token: 0x04003DC1 RID: 15809
	public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();

	// Token: 0x04003DC2 RID: 15810
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> MinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x04003DC3 RID: 15811
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> LiveMinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x04003DC4 RID: 15812
	public static Components.CmpsByWorld<Sleepable> NormalBeds = new Components.CmpsByWorld<Sleepable>();

	// Token: 0x04003DC5 RID: 15813
	public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();

	// Token: 0x04003DC6 RID: 15814
	public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();

	// Token: 0x04003DC7 RID: 15815
	public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();

	// Token: 0x04003DC8 RID: 15816
	public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003DC9 RID: 15817
	public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();

	// Token: 0x04003DCA RID: 15818
	public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();

	// Token: 0x04003DCB RID: 15819
	public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();

	// Token: 0x04003DCC RID: 15820
	public static Components.CmpsByWorld<PlantablePlot> PlantablePlots = new Components.CmpsByWorld<PlantablePlot>();

	// Token: 0x04003DCD RID: 15821
	public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();

	// Token: 0x04003DCE RID: 15822
	public static Components.Cmps<NavTeleporter> NavTeleporters = new Components.Cmps<NavTeleporter>();

	// Token: 0x04003DCF RID: 15823
	public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();

	// Token: 0x04003DD0 RID: 15824
	public static Components.CmpsByWorld<CreatureFeeder> CreatureFeeders = new Components.CmpsByWorld<CreatureFeeder>();

	// Token: 0x04003DD1 RID: 15825
	public static Components.CmpsByWorld<MilkFeeder.Instance> MilkFeeders = new Components.CmpsByWorld<MilkFeeder.Instance>();

	// Token: 0x04003DD2 RID: 15826
	public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();

	// Token: 0x04003DD3 RID: 15827
	public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();

	// Token: 0x04003DD4 RID: 15828
	public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();

	// Token: 0x04003DD5 RID: 15829
	public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();

	// Token: 0x04003DD6 RID: 15830
	public static Components.Cmps<IResearchCenter> ResearchCenters = new Components.Cmps<IResearchCenter>();

	// Token: 0x04003DD7 RID: 15831
	public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();

	// Token: 0x04003DD8 RID: 15832
	public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();

	// Token: 0x04003DD9 RID: 15833
	public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();

	// Token: 0x04003DDA RID: 15834
	public static Components.Cmps<Health> Health = new Components.Cmps<Health>();

	// Token: 0x04003DDB RID: 15835
	public static Components.Cmps<Equipment> Equipment = new Components.Cmps<Equipment>();

	// Token: 0x04003DDC RID: 15836
	public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();

	// Token: 0x04003DDD RID: 15837
	public static Components.Cmps<FactionAlignment> PlayerTargeted = new Components.Cmps<FactionAlignment>();

	// Token: 0x04003DDE RID: 15838
	public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();

	// Token: 0x04003DDF RID: 15839
	public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();

	// Token: 0x04003DE0 RID: 15840
	public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();

	// Token: 0x04003DE1 RID: 15841
	public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();

	// Token: 0x04003DE2 RID: 15842
	public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();

	// Token: 0x04003DE3 RID: 15843
	public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();

	// Token: 0x04003DE4 RID: 15844
	public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();

	// Token: 0x04003DE5 RID: 15845
	public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();

	// Token: 0x04003DE6 RID: 15846
	public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();

	// Token: 0x04003DE7 RID: 15847
	public static Components.Cmps<EntityCellVisualizer> EntityCellVisualizers = new Components.Cmps<EntityCellVisualizer>();

	// Token: 0x04003DE8 RID: 15848
	public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();

	// Token: 0x04003DE9 RID: 15849
	public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();

	// Token: 0x04003DEA RID: 15850
	public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();

	// Token: 0x04003DEB RID: 15851
	public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();

	// Token: 0x04003DEC RID: 15852
	public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();

	// Token: 0x04003DED RID: 15853
	public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();

	// Token: 0x04003DEE RID: 15854
	public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();

	// Token: 0x04003DEF RID: 15855
	public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();

	// Token: 0x04003DF0 RID: 15856
	public static Components.Cmps<MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<MinionAssignablesProxy>();

	// Token: 0x04003DF1 RID: 15857
	public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();

	// Token: 0x04003DF2 RID: 15858
	public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

	// Token: 0x04003DF3 RID: 15859
	public static Components.Cmps<PlantableSeed> PlantableSeeds = new Components.Cmps<PlantableSeed>();

	// Token: 0x04003DF4 RID: 15860
	public static Components.Cmps<IBasicBuilding> BasicBuildings = new Components.Cmps<IBasicBuilding>();

	// Token: 0x04003DF5 RID: 15861
	public static Components.Cmps<Painting> Paintings = new Components.Cmps<Painting>();

	// Token: 0x04003DF6 RID: 15862
	public static Components.Cmps<BuildingComplete> TemplateBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003DF7 RID: 15863
	public static Components.Cmps<Teleporter> Teleporters = new Components.Cmps<Teleporter>();

	// Token: 0x04003DF8 RID: 15864
	public static Components.Cmps<MutantPlant> MutantPlants = new Components.Cmps<MutantPlant>();

	// Token: 0x04003DF9 RID: 15865
	public static Components.Cmps<LandingBeacon.Instance> LandingBeacons = new Components.Cmps<LandingBeacon.Instance>();

	// Token: 0x04003DFA RID: 15866
	public static Components.Cmps<HighEnergyParticle> HighEnergyParticles = new Components.Cmps<HighEnergyParticle>();

	// Token: 0x04003DFB RID: 15867
	public static Components.Cmps<HighEnergyParticlePort> HighEnergyParticlePorts = new Components.Cmps<HighEnergyParticlePort>();

	// Token: 0x04003DFC RID: 15868
	public static Components.Cmps<Clustercraft> Clustercrafts = new Components.Cmps<Clustercraft>();

	// Token: 0x04003DFD RID: 15869
	public static Components.Cmps<ClustercraftInteriorDoor> ClusterCraftInteriorDoors = new Components.Cmps<ClustercraftInteriorDoor>();

	// Token: 0x04003DFE RID: 15870
	public static Components.Cmps<PassengerRocketModule> PassengerRocketModules = new Components.Cmps<PassengerRocketModule>();

	// Token: 0x04003DFF RID: 15871
	public static Components.Cmps<ClusterTraveler> ClusterTravelers = new Components.Cmps<ClusterTraveler>();

	// Token: 0x04003E00 RID: 15872
	public static Components.Cmps<LaunchPad> LaunchPads = new Components.Cmps<LaunchPad>();

	// Token: 0x04003E01 RID: 15873
	public static Components.Cmps<WarpReceiver> WarpReceivers = new Components.Cmps<WarpReceiver>();

	// Token: 0x04003E02 RID: 15874
	public static Components.Cmps<RocketControlStation> RocketControlStations = new Components.Cmps<RocketControlStation>();

	// Token: 0x04003E03 RID: 15875
	public static Components.Cmps<Reactor> NuclearReactors = new Components.Cmps<Reactor>();

	// Token: 0x04003E04 RID: 15876
	public static Components.Cmps<BuildingComplete> EntombedBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003E05 RID: 15877
	public static Components.Cmps<SpaceArtifact> SpaceArtifacts = new Components.Cmps<SpaceArtifact>();

	// Token: 0x04003E06 RID: 15878
	public static Components.Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations = new Components.Cmps<ArtifactAnalysisStationWorkable>();

	// Token: 0x04003E07 RID: 15879
	public static Components.Cmps<RocketConduitReceiver> RocketConduitReceivers = new Components.Cmps<RocketConduitReceiver>();

	// Token: 0x04003E08 RID: 15880
	public static Components.Cmps<RocketConduitSender> RocketConduitSenders = new Components.Cmps<RocketConduitSender>();

	// Token: 0x04003E09 RID: 15881
	public static Components.Cmps<LogicBroadcaster> LogicBroadcasters = new Components.Cmps<LogicBroadcaster>();

	// Token: 0x04003E0A RID: 15882
	public static Components.Cmps<Telephone> Telephones = new Components.Cmps<Telephone>();

	// Token: 0x04003E0B RID: 15883
	public static Components.Cmps<MissionControlWorkable> MissionControlWorkables = new Components.Cmps<MissionControlWorkable>();

	// Token: 0x04003E0C RID: 15884
	public static Components.Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables = new Components.Cmps<MissionControlClusterWorkable>();

	// Token: 0x04003E0D RID: 15885
	public static Components.Cmps<MinorFossilDigSite.Instance> MinorFossilDigSites = new Components.Cmps<MinorFossilDigSite.Instance>();

	// Token: 0x04003E0E RID: 15886
	public static Components.Cmps<MajorFossilDigSite.Instance> MajorFossilDigSites = new Components.Cmps<MajorFossilDigSite.Instance>();

	// Token: 0x04003E0F RID: 15887
	public static Components.Cmps<GameObject> FoodRehydrators = new Components.Cmps<GameObject>();

	// Token: 0x04003E10 RID: 15888
	public static Components.CmpsByWorld<SocialGatheringPoint> SocialGatheringPoints = new Components.CmpsByWorld<SocialGatheringPoint>();

	// Token: 0x04003E11 RID: 15889
	public static Components.CmpsByWorld<Geyser> Geysers = new Components.CmpsByWorld<Geyser>();

	// Token: 0x04003E12 RID: 15890
	public static Components.CmpsByWorld<GeoTuner.Instance> GeoTuners = new Components.CmpsByWorld<GeoTuner.Instance>();

	// Token: 0x04003E13 RID: 15891
	public static Components.CmpsByWorld<CritterCondo.Instance> CritterCondos = new Components.CmpsByWorld<CritterCondo.Instance>();

	// Token: 0x04003E14 RID: 15892
	public static Components.CmpsByWorld<GeothermalController> GeothermalControllers = new Components.CmpsByWorld<GeothermalController>();

	// Token: 0x04003E15 RID: 15893
	public static Components.CmpsByWorld<GeothermalVent> GeothermalVents = new Components.CmpsByWorld<GeothermalVent>();

	// Token: 0x04003E16 RID: 15894
	public static Components.CmpsByWorld<RemoteWorkerDock> RemoteWorkerDocks = new Components.CmpsByWorld<RemoteWorkerDock>();

	// Token: 0x04003E17 RID: 15895
	public static Components.CmpsByWorld<IRemoteDockWorkTarget> RemoteDockWorkTargets = new Components.CmpsByWorld<IRemoteDockWorkTarget>();

	// Token: 0x04003E18 RID: 15896
	public static Components.Cmps<Assignable> AssignableItems = new Components.Cmps<Assignable>();

	// Token: 0x04003E19 RID: 15897
	public static Components.CmpsByWorld<Comet> Meteors = new Components.CmpsByWorld<Comet>();

	// Token: 0x04003E1A RID: 15898
	public static Components.CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new Components.CmpsByWorld<DetectorNetwork.Instance>();

	// Token: 0x04003E1B RID: 15899
	public static Components.CmpsByWorld<ScannerNetworkVisualizer> ScannerVisualizers = new Components.CmpsByWorld<ScannerNetworkVisualizer>();

	// Token: 0x04003E1C RID: 15900
	public static Components.Cmps<IncubationMonitor.Instance> IncubationMonitors = new Components.Cmps<IncubationMonitor.Instance>();

	// Token: 0x04003E1D RID: 15901
	public static Components.Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors = new Components.Cmps<FixedCapturableMonitor.Instance>();

	// Token: 0x04003E1E RID: 15902
	public static Components.Cmps<BeeHive.StatesInstance> BeeHives = new Components.Cmps<BeeHive.StatesInstance>();

	// Token: 0x04003E1F RID: 15903
	public static Components.Cmps<StateMachine.Instance> EffectImmunityProviderStations = new Components.Cmps<StateMachine.Instance>();

	// Token: 0x04003E20 RID: 15904
	public static Components.Cmps<PeeChoreMonitor.Instance> CriticalBladders = new Components.Cmps<PeeChoreMonitor.Instance>();

	// Token: 0x020010CC RID: 4300
	public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x000D97F5 File Offset: 0x000D79F5
		public List<T> Items
		{
			get
			{
				return this.items.GetDataList();
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x000D9802 File Offset: 0x000D7A02
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x000D980F File Offset: 0x000D7A0F
		public Cmps()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.items = new KCompactedVector<T>(0);
			this.table = new Dictionary<T, HandleVector<int>.Handle>();
		}

		// Token: 0x17000537 RID: 1335
		public T this[int idx]
		{
			get
			{
				return this.Items[idx];
			}
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x000D985C File Offset: 0x000D7A5C
		private void Clear()
		{
			this.items.Clear();
			this.table.Clear();
			this.OnAdd = null;
			this.OnRemove = null;
		}

		// Token: 0x06005831 RID: 22577 RVA: 0x0028B474 File Offset: 0x00289674
		public void Add(T cmp)
		{
			HandleVector<int>.Handle value = this.items.Allocate(cmp);
			this.table[cmp] = value;
			if (this.OnAdd != null)
			{
				this.OnAdd(cmp);
			}
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x0028B4B0 File Offset: 0x002896B0
		public void Remove(T cmp)
		{
			HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
			if (this.table.TryGetValue(cmp, out invalidHandle))
			{
				this.table.Remove(cmp);
				this.items.Free(invalidHandle);
				if (this.OnRemove != null)
				{
					this.OnRemove(cmp);
				}
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x0028B504 File Offset: 0x00289704
		public void Register(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd += on_add;
			this.OnRemove += on_remove;
			foreach (T obj in this.Items)
			{
				this.OnAdd(obj);
			}
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x000D9882 File Offset: 0x000D7A82
		public void Unregister(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd -= on_add;
			this.OnRemove -= on_remove;
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x0028B56C File Offset: 0x0028976C
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.GetWorldItems(worldId, otherWorldIds, null);
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x0028B598 File Offset: 0x00289798
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds, Func<T, bool> filter)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.GetWorldItems(worldId, otherWorldIds, filter);
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x0028B5C4 File Offset: 0x002897C4
		public List<T> GetWorldItems(int worldId, ICollection<int> otherWorldIds, Func<T, bool> filter)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.Items.Count; i++)
			{
				T t = this.Items[i];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				bool flag = worldId == myWorldId;
				if (!flag && otherWorldIds != null && otherWorldIds.Contains(myWorldId))
				{
					flag = true;
				}
				if (flag && filter != null)
				{
					flag = filter(t);
				}
				if (flag)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x06005838 RID: 22584 RVA: 0x0028B640 File Offset: 0x00289840
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x000D9892 File Offset: 0x000D7A92
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			int num;
			for (int index = 0; index < this.Items.Count; index = num + 1)
			{
				T t = this.Items[index];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				if (myWorldId == worldId || (otherWorldIds != null && otherWorldIds.Contains(myWorldId)))
				{
					yield return t;
				}
				num = index;
			}
			yield break;
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x0600583A RID: 22586 RVA: 0x0028B66C File Offset: 0x0028986C
		// (remove) Token: 0x0600583B RID: 22587 RVA: 0x0028B6A4 File Offset: 0x002898A4
		public event Action<T> OnAdd;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600583C RID: 22588 RVA: 0x0028B6DC File Offset: 0x002898DC
		// (remove) Token: 0x0600583D RID: 22589 RVA: 0x0028B714 File Offset: 0x00289914
		public event Action<T> OnRemove;

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x0600583F RID: 22591 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06005840 RID: 22592 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005841 RID: 22593 RVA: 0x000D98B0 File Offset: 0x000D7AB0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06005842 RID: 22594 RVA: 0x000D98B0 File Offset: 0x000D7AB0
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06005843 RID: 22595 RVA: 0x000D98B0 File Offset: 0x000D7AB0
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x04003E21 RID: 15905
		private Dictionary<T, HandleVector<int>.Handle> table;

		// Token: 0x04003E22 RID: 15906
		private KCompactedVector<T> items;
	}

	// Token: 0x020010CE RID: 4302
	public class CmpsByWorld<T>
	{
		// Token: 0x0600584C RID: 22604 RVA: 0x000D98F9 File Offset: 0x000D7AF9
		public CmpsByWorld()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.m_CmpsByWorld = new Dictionary<int, Components.Cmps<T>>();
		}

		// Token: 0x0600584D RID: 22605 RVA: 0x000D992C File Offset: 0x000D7B2C
		public void Clear()
		{
			this.m_CmpsByWorld.Clear();
		}

		// Token: 0x0600584E RID: 22606 RVA: 0x0028B85C File Offset: 0x00289A5C
		public Components.Cmps<T> CreateOrGetCmps(int worldId)
		{
			Components.Cmps<T> cmps;
			if (!this.m_CmpsByWorld.TryGetValue(worldId, out cmps))
			{
				cmps = new Components.Cmps<T>();
				this.m_CmpsByWorld[worldId] = cmps;
			}
			return cmps;
		}

		// Token: 0x0600584F RID: 22607 RVA: 0x000D9939 File Offset: 0x000D7B39
		public void Add(int worldId, T cmp)
		{
			DebugUtil.DevAssertArgs(worldId != -1, new object[]
			{
				"CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
				cmp
			});
			this.CreateOrGetCmps(worldId).Add(cmp);
		}

		// Token: 0x06005850 RID: 22608 RVA: 0x000D996B File Offset: 0x000D7B6B
		public void Remove(int worldId, T cmp)
		{
			this.CreateOrGetCmps(worldId).Remove(cmp);
		}

		// Token: 0x06005851 RID: 22609 RVA: 0x000D997A File Offset: 0x000D7B7A
		public void Register(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Register(on_add, on_remove);
		}

		// Token: 0x06005852 RID: 22610 RVA: 0x000D998A File Offset: 0x000D7B8A
		public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
		}

		// Token: 0x06005853 RID: 22611 RVA: 0x000D999A File Offset: 0x000D7B9A
		public List<T> GetItems(int worldId)
		{
			return this.CreateOrGetCmps(worldId).Items;
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x000D99A8 File Offset: 0x000D7BA8
		public Dictionary<int, Components.Cmps<T>>.KeyCollection GetWorldsIds()
		{
			return this.m_CmpsByWorld.Keys;
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06005855 RID: 22613 RVA: 0x0028B890 File Offset: 0x00289A90
		public int GlobalCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<int, Components.Cmps<T>> keyValuePair in this.m_CmpsByWorld)
				{
					num += keyValuePair.Value.Count;
				}
				return num;
			}
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x0028B8F0 File Offset: 0x00289AF0
		public int CountWorldItems(int worldId, bool includeChildren = false)
		{
			int num = this.GetItems(worldId).Count;
			if (includeChildren)
			{
				foreach (int worldId2 in ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds())
				{
					num += this.GetItems(worldId2).Count;
				}
			}
			return num;
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x0028B960 File Offset: 0x00289B60
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x000D99B5 File Offset: 0x000D7BB5
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			List<T> items = this.GetItems(worldId);
			int num;
			for (int index = 0; index < items.Count; index = num + 1)
			{
				yield return items[index];
				num = index;
			}
			if (otherWorldIds != null)
			{
				foreach (int worldId2 in otherWorldIds)
				{
					items = this.GetItems(worldId2);
					for (int index = 0; index < items.Count; index = num + 1)
					{
						yield return items[index];
						num = index;
					}
				}
				IEnumerator<int> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003E2E RID: 15918
		private Dictionary<int, Components.Cmps<T>> m_CmpsByWorld;
	}
}
