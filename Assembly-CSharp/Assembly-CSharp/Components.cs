﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Components {
    public static Cmps<RobotAi.Instance>           LiveRobotsIdentities      = new Cmps<RobotAi.Instance>();
    public static Cmps<MinionIdentity>             LiveMinionIdentities      = new Cmps<MinionIdentity>();
    public static Cmps<MinionIdentity>             MinionIdentities          = new Cmps<MinionIdentity>();
    public static Cmps<StoredMinionIdentity>       StoredMinionIdentities    = new Cmps<StoredMinionIdentity>();
    public static Cmps<MinionStorage>              MinionStorages            = new Cmps<MinionStorage>();
    public static Cmps<MinionResume>               MinionResumes             = new Cmps<MinionResume>();
    public static Cmps<Sleepable>                  Sleepables                = new Cmps<Sleepable>();
    public static Cmps<IUsable>                    Toilets                   = new Cmps<IUsable>();
    public static Cmps<Pickupable>                 Pickupables               = new Cmps<Pickupable>();
    public static Cmps<Brain>                      Brains                    = new Cmps<Brain>();
    public static Cmps<BuildingComplete>           BuildingCompletes         = new Cmps<BuildingComplete>();
    public static Cmps<Notifier>                   Notifiers                 = new Cmps<Notifier>();
    public static Cmps<Fabricator>                 Fabricators               = new Cmps<Fabricator>();
    public static Cmps<Refinery>                   Refineries                = new Cmps<Refinery>();
    public static CmpsByWorld<PlantablePlot>       PlantablePlots            = new CmpsByWorld<PlantablePlot>();
    public static Cmps<Ladder>                     Ladders                   = new Cmps<Ladder>();
    public static Cmps<NavTeleporter>              NavTeleporters            = new Cmps<NavTeleporter>();
    public static Cmps<ITravelTubePiece>           ITravelTubePieces         = new Cmps<ITravelTubePiece>();
    public static CmpsByWorld<CreatureFeeder>      CreatureFeeders           = new CmpsByWorld<CreatureFeeder>();
    public static CmpsByWorld<MilkFeeder.Instance> MilkFeeders               = new CmpsByWorld<MilkFeeder.Instance>();
    public static Cmps<Light2D>                    Light2Ds                  = new Cmps<Light2D>();
    public static Cmps<Radiator>                   Radiators                 = new Cmps<Radiator>();
    public static Cmps<Edible>                     Edibles                   = new Cmps<Edible>();
    public static Cmps<Diggable>                   Diggables                 = new Cmps<Diggable>();
    public static Cmps<IResearchCenter>            ResearchCenters           = new Cmps<IResearchCenter>();
    public static Cmps<Harvestable>                Harvestables              = new Cmps<Harvestable>();
    public static Cmps<HarvestDesignatable>        HarvestDesignatables      = new Cmps<HarvestDesignatable>();
    public static Cmps<Uprootable>                 Uprootables               = new Cmps<Uprootable>();
    public static Cmps<Health>                     Health                    = new Cmps<Health>();
    public static Cmps<Equipment>                  Equipment                 = new Cmps<Equipment>();
    public static Cmps<FactionAlignment>           FactionAlignments         = new Cmps<FactionAlignment>();
    public static Cmps<FactionAlignment>           PlayerTargeted            = new Cmps<FactionAlignment>();
    public static Cmps<Telepad>                    Telepads                  = new Cmps<Telepad>();
    public static Cmps<Generator>                  Generators                = new Cmps<Generator>();
    public static Cmps<EnergyConsumer>             EnergyConsumers           = new Cmps<EnergyConsumer>();
    public static Cmps<Battery>                    Batteries                 = new Cmps<Battery>();
    public static Cmps<Breakable>                  Breakables                = new Cmps<Breakable>();
    public static Cmps<Crop>                       Crops                     = new Cmps<Crop>();
    public static Cmps<Prioritizable>              Prioritizables            = new Cmps<Prioritizable>();
    public static Cmps<Clinic>                     Clinics                   = new Cmps<Clinic>();
    public static Cmps<HandSanitizer>              HandSanitizers            = new Cmps<HandSanitizer>();
    public static Cmps<EntityCellVisualizer>       EntityCellVisualizers     = new Cmps<EntityCellVisualizer>();
    public static Cmps<RoleStation>                RoleStations              = new Cmps<RoleStation>();
    public static Cmps<Telescope>                  Telescopes                = new Cmps<Telescope>();
    public static Cmps<Capturable>                 Capturables               = new Cmps<Capturable>();
    public static Cmps<NotCapturable>              NotCapturables            = new Cmps<NotCapturable>();
    public static Cmps<DiseaseSourceVisualizer>    DiseaseSourceVisualizers  = new Cmps<DiseaseSourceVisualizer>();
    public static Cmps<Grave>                      Graves                    = new Cmps<Grave>();
    public static Cmps<AttachableBuilding>         AttachableBuildings       = new Cmps<AttachableBuilding>();
    public static Cmps<BuildingAttachPoint>        BuildingAttachPoints      = new Cmps<BuildingAttachPoint>();
    public static Cmps<MinionAssignablesProxy>     MinionAssignablesProxy    = new Cmps<MinionAssignablesProxy>();
    public static Cmps<ComplexFabricator>          ComplexFabricators        = new Cmps<ComplexFabricator>();
    public static Cmps<MonumentPart>               MonumentParts             = new Cmps<MonumentPart>();
    public static Cmps<PlantableSeed>              PlantableSeeds            = new Cmps<PlantableSeed>();
    public static Cmps<IBasicBuilding>             BasicBuildings            = new Cmps<IBasicBuilding>();
    public static Cmps<Painting>                   Paintings                 = new Cmps<Painting>();
    public static Cmps<BuildingComplete>           TemplateBuildings         = new Cmps<BuildingComplete>();
    public static Cmps<Teleporter>                 Teleporters               = new Cmps<Teleporter>();
    public static Cmps<MutantPlant>                MutantPlants              = new Cmps<MutantPlant>();
    public static Cmps<LandingBeacon.Instance>     LandingBeacons            = new Cmps<LandingBeacon.Instance>();
    public static Cmps<HighEnergyParticle>         HighEnergyParticles       = new Cmps<HighEnergyParticle>();
    public static Cmps<HighEnergyParticlePort>     HighEnergyParticlePorts   = new Cmps<HighEnergyParticlePort>();
    public static Cmps<Clustercraft>               Clustercrafts             = new Cmps<Clustercraft>();
    public static Cmps<ClustercraftInteriorDoor>   ClusterCraftInteriorDoors = new Cmps<ClustercraftInteriorDoor>();
    public static Cmps<PassengerRocketModule>      PassengerRocketModules    = new Cmps<PassengerRocketModule>();
    public static Cmps<ClusterTraveler>            ClusterTravelers          = new Cmps<ClusterTraveler>();
    public static Cmps<LaunchPad>                  LaunchPads                = new Cmps<LaunchPad>();
    public static Cmps<WarpReceiver>               WarpReceivers             = new Cmps<WarpReceiver>();
    public static Cmps<RocketControlStation>       RocketControlStations     = new Cmps<RocketControlStation>();
    public static Cmps<Reactor>                    NuclearReactors           = new Cmps<Reactor>();
    public static Cmps<BuildingComplete>           EntombedBuildings         = new Cmps<BuildingComplete>();
    public static Cmps<SpaceArtifact>              SpaceArtifacts            = new Cmps<SpaceArtifact>();

    public static Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations
        = new Cmps<ArtifactAnalysisStationWorkable>();

    public static Cmps<RocketConduitReceiver>  RocketConduitReceivers  = new Cmps<RocketConduitReceiver>();
    public static Cmps<RocketConduitSender>    RocketConduitSenders    = new Cmps<RocketConduitSender>();
    public static Cmps<LogicBroadcaster>       LogicBroadcasters       = new Cmps<LogicBroadcaster>();
    public static Cmps<Telephone>              Telephones              = new Cmps<Telephone>();
    public static Cmps<MissionControlWorkable> MissionControlWorkables = new Cmps<MissionControlWorkable>();

    public static Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables
        = new Cmps<MissionControlClusterWorkable>();

    public static Cmps<MinorFossilDigSite.Instance>     MinorFossilDigSites = new Cmps<MinorFossilDigSite.Instance>();
    public static Cmps<MajorFossilDigSite.Instance>     MajorFossilDigSites = new Cmps<MajorFossilDigSite.Instance>();
    public static Cmps<GameObject>                      FoodRehydrators = new Cmps<GameObject>();
    public static CmpsByWorld<Geyser>                   Geysers = new CmpsByWorld<Geyser>();
    public static CmpsByWorld<GeoTuner.Instance>        GeoTuners = new CmpsByWorld<GeoTuner.Instance>();
    public static CmpsByWorld<CritterCondo.Instance>    CritterCondos = new CmpsByWorld<CritterCondo.Instance>();
    public static CmpsByWorld<GeothermalController>     GeothermalControllers = new CmpsByWorld<GeothermalController>();
    public static CmpsByWorld<GeothermalVent>           GeothermalVents = new CmpsByWorld<GeothermalVent>();
    public static CmpsByWorld<Comet>                    Meteors = new CmpsByWorld<Comet>();
    public static CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new CmpsByWorld<DetectorNetwork.Instance>();

    public static CmpsByWorld<ScannerNetworkVisualizer>
        ScannerVisualizers = new CmpsByWorld<ScannerNetworkVisualizer>();

    public static Cmps<IncubationMonitor.Instance> IncubationMonitors = new Cmps<IncubationMonitor.Instance>();

    public static Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors
        = new Cmps<FixedCapturableMonitor.Instance>();

    public static Cmps<BeeHive.StatesInstance> BeeHives                       = new Cmps<BeeHive.StatesInstance>();
    public static Cmps<StateMachine.Instance>  EffectImmunityProviderStations = new Cmps<StateMachine.Instance>();

    public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T> {
        private readonly KCompactedVector<T>                     items;
        private readonly Dictionary<T, HandleVector<int>.Handle> table;

        public Cmps() {
            App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(Clear));
            items              = new KCompactedVector<T>();
            table              = new Dictionary<T, HandleVector<int>.Handle>();
        }

        public List<T> Items => items.GetDataList();
        public T this[int idx] => Items[idx];
        public int              Count                          => items.Count;
        public bool             IsSynchronized                 => throw new NotImplementedException();
        public object           SyncRoot                       => throw new NotImplementedException();
        public void             CopyTo(Array array, int index) { throw new NotImplementedException(); }
        IEnumerator IEnumerable.GetEnumerator() { return items.GetEnumerator(); }

        private void Clear() {
            items.Clear();
            table.Clear();
            OnAdd    = null;
            OnRemove = null;
        }

        public void Add(T cmp) {
            var value = items.Allocate(cmp);
            table[cmp] = value;
            if (OnAdd != null) OnAdd(cmp);
        }

        public void Remove(T cmp) {
            var invalidHandle = HandleVector<int>.InvalidHandle;
            if (table.TryGetValue(cmp, out invalidHandle)) {
                table.Remove(cmp);
                items.Free(invalidHandle);
                if (OnRemove != null) OnRemove(cmp);
            }
        }

        public void Register(Action<T> on_add, Action<T> on_remove) {
            OnAdd    += on_add;
            OnRemove += on_remove;
            foreach (var obj in Items) OnAdd(obj);
        }

        public void Unregister(Action<T> on_add, Action<T> on_remove) {
            OnAdd    -= on_add;
            OnRemove -= on_remove;
        }

        public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false) {
            var list = new List<T>();
            foreach (var t in Items) {
                var component = t as KMonoBehaviour;
                var flag      = component.GetMyWorldId() == worldId;
                if (!flag && checkChildWorlds) {
                    var myWorld                                                   = component.GetMyWorld();
                    if (myWorld != null && myWorld.ParentWorldId == worldId) flag = true;
                }

                if (flag) list.Add(t);
            }

            return list;
        }

        public event Action<T> OnAdd;
        public event Action<T> OnRemove;
        private IEnumerator<T> IEnumerable;
        public  IEnumerator<T>    GetEnumerator() { return items.GetEnumerator(); }
    }

    public class CmpsByWorld<T> {
        private readonly Dictionary<int, Cmps<T>> m_CmpsByWorld;

        public CmpsByWorld() {
            App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(Clear));
            m_CmpsByWorld      = new Dictionary<int, Cmps<T>>();
        }

        public int GlobalCount {
            get {
                var num                                         = 0;
                foreach (var keyValuePair in m_CmpsByWorld) num += m_CmpsByWorld.Count;
                return num;
            }
        }

        public void Clear() { m_CmpsByWorld.Clear(); }

        public Cmps<T> CreateOrGetCmps(int worldId) {
            Cmps<T> cmps;
            if (!m_CmpsByWorld.TryGetValue(worldId, out cmps)) {
                cmps                   = new Cmps<T>();
                m_CmpsByWorld[worldId] = cmps;
            }

            return cmps;
        }

        public void Add(int worldId, T cmp) {
            DebugUtil.DevAssertArgs(worldId != -1,
                                    "CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
                                    cmp);

            CreateOrGetCmps(worldId).Add(cmp);
        }

        public void Remove(int worldId, T cmp) { CreateOrGetCmps(worldId).Remove(cmp); }

        public void Register(int worldId, Action<T> on_add, Action<T> on_remove) {
            CreateOrGetCmps(worldId).Register(on_add, on_remove);
        }

        public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove) {
            CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
        }

        public List<T>                                GetItems(int worldId) { return CreateOrGetCmps(worldId).Items; }
        public Dictionary<int, Cmps<T>>.KeyCollection GetWorldsIds()        { return m_CmpsByWorld.Keys; }
    }
}