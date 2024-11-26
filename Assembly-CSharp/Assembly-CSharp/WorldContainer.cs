using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour {
    [Serialize]
    public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

    [Serialize]
    public string cosmicRadiationFixedTrait;

    [Serialize]
    public float currentCosmicIntensity = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

    [Serialize]
    public float currentSunlightIntensity;

    [Serialize]
    public int fixedTraitsUpdateVersion = 1;

    [Serialize]
    public int id = -1;

    [Serialize]
    private bool isDiscovered;

    [MySmiReq]
    private AlertStateManager.Instance m_alertManager;

    private readonly List<int> m_childWorlds = new List<int>();

    [Serialize]
    private List<string> m_seasonIds;

    [Serialize]
    public string[] nameTables;

    [Serialize]
    public string northernLightFixedTrait;

    [Serialize]
    public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

    [Serialize]
    public string overrideName;

    [Serialize]
    private WorldDetailSave.OverworldCell overworldCell;

    private readonly WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

    [Serialize]
    public Tag prefabTag;

    [Serialize]
    public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

    [Serialize]
    public string sunlightFixedTrait;

    [Serialize]
    public string worldDescription;

    [Serialize]
    public string worldName;

    [Serialize]
    private Vector2I worldOffset;

    [Serialize]
    private Vector2I worldSize;

    [Serialize]
    public Tag[] worldTags;

    [Serialize]
    public string worldType;

    private readonly List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();

    [Serialize]
    public WorldInventory worldInventory { get; private set; }

    public Dictionary<Tag, float> materialNeeds { get; private set; }

    [field: Serialize]
    public bool IsModuleInterior { get; private set; }

    public bool IsDiscovered => isDiscovered || DebugHandler.RevealFogOfWar;

    [field: Serialize]
    public bool IsStartWorld { get; private set; }

    [field: Serialize]
    public bool IsDupeVisited { get; private set; }

    [field: Serialize]
    public float DupeVisitedTimestamp { get; private set; } = -1f;

    [field: Serialize]
    public float DiscoveryTimestamp { get; private set; } = -1f;

    [field: Serialize]
    public bool IsRoverVisted { get; private set; }

    [field: Serialize]
    public bool IsSurfaceRevealed { get; private set; }

    public Dictionary<string, int> SunlightFixedTraits { get; } = new Dictionary<string, int> {
        { FIXEDTRAITS.SUNLIGHT.NAME.NONE, FIXEDTRAITS.SUNLIGHT.NONE },
        { FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW, FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW },
        { FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW, FIXEDTRAITS.SUNLIGHT.VERY_LOW },
        { FIXEDTRAITS.SUNLIGHT.NAME.LOW, FIXEDTRAITS.SUNLIGHT.LOW },
        { FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW, FIXEDTRAITS.SUNLIGHT.MED_LOW },
        { FIXEDTRAITS.SUNLIGHT.NAME.MED, FIXEDTRAITS.SUNLIGHT.MED },
        { FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH, FIXEDTRAITS.SUNLIGHT.MED_HIGH },
        { FIXEDTRAITS.SUNLIGHT.NAME.HIGH, FIXEDTRAITS.SUNLIGHT.HIGH },
        { FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH, FIXEDTRAITS.SUNLIGHT.VERY_HIGH },
        { FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH, FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH },
        { FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH, FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH }
    };

    public Dictionary<string, int> NorthernLightsFixedTraits { get; } = new Dictionary<string, int> {
        { FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE, FIXEDTRAITS.NORTHERNLIGHTS.NONE },
        { FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED, FIXEDTRAITS.NORTHERNLIGHTS.ENABLED }
    };

    public Dictionary<string, int> CosmicRadiationFixedTraits { get; } = new Dictionary<string, int> {
        { FIXEDTRAITS.COSMICRADIATION.NAME.NONE, FIXEDTRAITS.COSMICRADIATION.NONE },
        { FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW, FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW },
        { FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW, FIXEDTRAITS.COSMICRADIATION.VERY_LOW },
        { FIXEDTRAITS.COSMICRADIATION.NAME.LOW, FIXEDTRAITS.COSMICRADIATION.LOW },
        { FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW, FIXEDTRAITS.COSMICRADIATION.MED_LOW },
        { FIXEDTRAITS.COSMICRADIATION.NAME.MED, FIXEDTRAITS.COSMICRADIATION.MED },
        { FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH, FIXEDTRAITS.COSMICRADIATION.MED_HIGH },
        { FIXEDTRAITS.COSMICRADIATION.NAME.HIGH, FIXEDTRAITS.COSMICRADIATION.HIGH },
        { FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH, FIXEDTRAITS.COSMICRADIATION.VERY_HIGH },
        { FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH, FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH }
    };

    [field: Serialize]
    public List<string> Biomes { get; private set; }

    [field: Serialize]
    public List<string> GeneratedBiomes { get; private set; }

    [field: Serialize]
    public List<string> WorldTraitIds { get; private set; }

    [field: Serialize]
    public List<string> StoryTraitIds { get; private set; }

    public AlertStateManager.Instance AlertManager {
        get {
            if (m_alertManager == null) {
                var component = GetComponent<StateMachineController>();
                m_alertManager = component.GetSMI<AlertStateManager.Instance>();
            }

            Debug.Assert(m_alertManager != null, "AlertStateManager should never be null.");
            return m_alertManager;
        }
    }

    public int      ParentWorldId { get; private set; }
    public Vector2  minimumBounds => new Vector2(worldOffset.x,                     worldOffset.y);
    public Vector2  maximumBounds => new Vector2(worldOffset.x + (worldSize.x - 1), worldOffset.y + (worldSize.y - 1));
    public Vector2I WorldSize     => worldSize;
    public Vector2I WorldOffset   => worldOffset;

    [field: Serialize]
    public bool FullyEnclosedBorder { get; private set; }

    public int Height => worldSize.y;
    public int Width  => worldSize.x;

    public void AddTopPriorityPrioritizable(Prioritizable prioritizable) {
        if (!yellowAlertTasks.Contains(prioritizable)) yellowAlertTasks.Add(prioritizable);
        RefreshHasTopPriorityChore();
    }

    public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable) {
        for (var i = yellowAlertTasks.Count - 1; i >= 0; i--)
            if (yellowAlertTasks[i] == prioritizable || yellowAlertTasks[i].Equals(null))
                yellowAlertTasks.RemoveAt(i);

        RefreshHasTopPriorityChore();
    }

    public ICollection<int> GetChildWorldIds() { return m_childWorlds; }

    private void OnWorldRemoved(object data) {
        var num = data is int ? (int)data : 255;
        if (num != 255) m_childWorlds.Remove(num);
    }

    private void OnWorldParentChanged(object data) {
        var worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
        if (worldParentChangedEventArgs == null) return;

        if (worldParentChangedEventArgs.world.ParentWorldId == id)
            m_childWorlds.Add(worldParentChangedEventArgs.world.id);

        if (worldParentChangedEventArgs.lastParentId == ParentWorldId)
            m_childWorlds.Remove(worldParentChangedEventArgs.world.id);
    }

    public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1) {
        var     vector  = new Vector2(WorldSize.x * Grid.CellSizeInMeters, worldSize.y * Grid.CellSizeInMeters);
        Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(WorldOffset.x, WorldOffset.y));
        Vector2 vector3 = Grid.CellToPos2D(cell);
        var     array   = new Quadrant[depth];
        var     vector4 = new Vector2(vector2.x, worldOffset.y + vector.y);
        var     vector5 = new Vector2(vector2.x                + vector.x, worldOffset.y);
        for (var i = 0; i < depth; i++) {
            var num                                                                      = vector5.x - vector4.x;
            var num2                                                                     = vector4.y - vector5.y;
            var num3                                                                     = num  * 0.5f;
            var num4                                                                     = num2 * 0.5f;
            if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4) array[i] = Quadrant.NE;
            if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y  + num4) array[i] = Quadrant.SE;
            if (vector3.x < vector4.x  + num3 && vector3.y < vector5.y  + num4) array[i] = Quadrant.SW;
            if (vector3.x < vector4.x  + num3 && vector3.y >= vector5.y + num4) array[i] = Quadrant.NW;
            switch (array[i]) {
                case Quadrant.NE:
                    vector4.x += num3;
                    vector5.y += num4;
                    break;
                case Quadrant.NW:
                    vector5.x -= num3;
                    vector5.y += num4;
                    break;
                case Quadrant.SW:
                    vector4.y -= num4;
                    vector5.x -= num3;
                    break;
                case Quadrant.SE:
                    vector4.x += num3;
                    vector4.y -= num4;
                    break;
            }
        }

        return array;
    }

    [OnDeserialized]
    private void OnDeserialized() { ParentWorldId = id; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        worldInventory = GetComponent<WorldInventory>();
        materialNeeds  = new Dictionary<Tag, float>();
        ClusterManager.Instance.RegisterWorldContainer(this);
        Game.Instance.Subscribe(880851192, OnWorldParentChanged);
        ClusterManager.Instance.Subscribe(-1078710002, OnWorldRemoved);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        gameObject.AddOrGet<InfoDescription>().DescriptionLocString = worldDescription;
        RefreshHasTopPriorityChore();
        UpgradeFixedTraits();
        RefreshFixedTraits();
        if (DlcManager.IsPureVanilla()) {
            IsStartWorld  = true;
            IsDupeVisited = true;
        }
    }

    protected override void OnCleanUp() {
        SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(id);
        Game.Instance.Unsubscribe(880851192, OnWorldParentChanged);
        ClusterManager.Instance.Unsubscribe(-1078710002, OnWorldRemoved);
        base.OnCleanUp();
    }

    private void UpgradeFixedTraits() {
        if (sunlightFixedTrait == null || sunlightFixedTrait == "")
            new Dictionary<int, string> {
                { 160000, FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH },
                { 0, FIXEDTRAITS.SUNLIGHT.NAME.NONE },
                { 10000, FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW },
                { 20000, FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW },
                { 30000, FIXEDTRAITS.SUNLIGHT.NAME.LOW },
                { 35000, FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW },
                { 40000, FIXEDTRAITS.SUNLIGHT.NAME.MED },
                { 50000, FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH },
                { 60000, FIXEDTRAITS.SUNLIGHT.NAME.HIGH },
                { 80000, FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH },
                { 120000, FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH }
            }.TryGetValue(sunlight, out sunlightFixedTrait);

        if (cosmicRadiationFixedTrait == null || cosmicRadiationFixedTrait == "")
            new Dictionary<int, string> {
                { 0, FIXEDTRAITS.COSMICRADIATION.NAME.NONE },
                { 6, FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW },
                { 12, FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW },
                { 18, FIXEDTRAITS.COSMICRADIATION.NAME.LOW },
                { 21, FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW },
                { 25, FIXEDTRAITS.COSMICRADIATION.NAME.MED },
                { 31, FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH },
                { 37, FIXEDTRAITS.COSMICRADIATION.NAME.HIGH },
                { 50, FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH },
                { 75, FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH }
            }.TryGetValue(cosmicRadiation, out cosmicRadiationFixedTrait);
    }

    private void RefreshFixedTraits() {
        sunlight        = GetSunlightValueFromFixedTrait();
        cosmicRadiation = GetCosmicRadiationValueFromFixedTrait();
        northernlights  = GetNorthernlightValueFromFixedTrait();
    }

    private void RefreshHasTopPriorityChore() {
        if (AlertManager != null) AlertManager.SetHasTopPriorityChore(yellowAlertTasks.Count > 0);
    }

    public List<string> GetSeasonIds()  { return m_seasonIds; }
    public bool         IsRedAlert()    { return m_alertManager.IsRedAlert(); }
    public bool         IsYellowAlert() { return m_alertManager.IsYellowAlert(); }

    public string GetRandomName() {
        if (!overrideName.IsNullOrWhiteSpace()) return Strings.Get(overrideName);

        return GameUtil.GenerateRandomWorldName(nameTables);
    }

    public void SetID(int id) {
        this.id       = id;
        ParentWorldId = id;
    }

    public void SetParentIdx(int parentIdx) {
        parentChangeArgs.lastParentId = ParentWorldId;
        parentChangeArgs.world        = this;
        ParentWorldId                 = parentIdx;
        Game.Instance.Trigger(880851192, parentChangeArgs);
        parentChangeArgs.lastParentId = 255;
    }

    public void SetDiscovered(bool reveal_surface = false) {
        if (!isDiscovered) DiscoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
        isDiscovered = true;
        if (reveal_surface) LookAtSurface();
        Game.Instance.Trigger(-521212405, this);
    }

    public void SetDupeVisited() {
        if (!IsDupeVisited) {
            DupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
            IsDupeVisited        = true;
            Game.Instance.Trigger(-434755240, this);
        }
    }

    public void SetRoverLanded() { IsRoverVisted = true; }

    public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset) {
        SetID(world_id);
        FullyEnclosedBorder = true;
        worldOffset         = offset;
        worldSize           = size;
        isDiscovered        = true;
        IsModuleInterior    = true;
        m_seasonIds         = new List<string>();
    }

    private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin) {
        if (first == second) return 0;

        var vector  = first  - origin;
        var vector2 = second - origin;
        var num     = Mathf.Atan2(vector.x,  vector.y);
        var num2    = Mathf.Atan2(vector2.x, vector2.y);
        if (num < num2) return 1;

        if (num > num2) return -1;

        if (vector.sqrMagnitude >= vector2.sqrMagnitude) return -1;

        return 1;
    }

    public void PlaceInteriorTemplate(string template_name, System.Action callback) {
        var template = TemplateCache.GetTemplate(template_name);
        var pos      = new Vector2(worldSize.x / 2 + worldOffset.x, worldSize.y / 2 + worldOffset.y);
        TemplateLoader.Stamp(template, pos, callback);
        var val  = template.info.size.X / 2f;
        var val2 = template.info.size.Y / 2f;
        var num  = Math.Max(val, val2);
        GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
        var clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
        overworldCell = new WorldDetailSave.OverworldCell();
        var list = new List<Vector2>(template.cells.Count);
        foreach (var prefab in template.buildings)
            if (prefab.id == "RocketWallTile") {
                var vector                     = new Vector2(prefab.location_x + pos.x, prefab.location_y + pos.y);
                if (vector.x > pos.x) vector.x += 0.5f;
                if (vector.y > pos.y) vector.y += 0.5f;
                list.Add(vector);
            }

        list.Sort((v1, v2) => IsClockwise(v1, v2, pos));
        var polygon = new Polygon(list);
        overworldCell.poly     = polygon;
        overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
        overworldCell.tags     = new TagSet { WorldGenTags.RocketInterior };
        clusterDetailSave.overworldCells.Add(overworldCell);
        for (var i = 0; i < worldSize.y; i++) {
            for (var j = 0; j < worldSize.x; j++) {
                var vector2I = new Vector2I(worldOffset.x + j, worldOffset.y + i);
                var num2     = Grid.XYToCell(vector2I.x, vector2I.y);
                if (polygon.Contains(new Vector2(vector2I.x, vector2I.y))) {
                    SimMessages.ModifyCellWorldZone(num2, 14);
                    World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
                } else {
                    SimMessages.ModifyCellWorldZone(num2, 7);
                    World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
                }
            }
        }
    }

    private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory) {
        if (traitCategory == NorthernLightsFixedTraits) return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

        if (traitCategory == SunlightFixedTraits) return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

        if (traitCategory == CosmicRadiationFixedTraits) return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

        return 0;
    }

    private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory) {
        if (traitCategory == NorthernLightsFixedTraits) return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;

        if (traitCategory == SunlightFixedTraits) return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;

        if (traitCategory == CosmicRadiationFixedTraits) return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;

        return null;
    }

    private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world) {
        foreach (var text in world.Settings.world.fixedTraits)
            if (traitCategory.ContainsKey(text))
                return text;

        return GetDefaultFixedTraitFor(traitCategory);
    }

    private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait) {
        if (trait == null) trait = GetDefaultFixedTraitFor(traitCategory);
        if (traitCategory.ContainsKey(trait)) return traitCategory[trait];

        return GetDefaultValueForFixedTraitCategory(traitCategory);
    }

    private string GetNorthernlightFixedTraits(WorldGen world) {
        return GetFixedTraitsFor(NorthernLightsFixedTraits, world);
    }

    private string GetSunlightFromFixedTraits(WorldGen world) { return GetFixedTraitsFor(SunlightFixedTraits, world); }

    private string GetCosmicRadiationFromFixedTraits(WorldGen world) {
        return GetFixedTraitsFor(CosmicRadiationFixedTraits, world);
    }

    private int GetNorthernlightValueFromFixedTrait() {
        return GetFixedTraitValueForTrait(NorthernLightsFixedTraits, ref northernLightFixedTrait);
    }

    private int GetSunlightValueFromFixedTrait() {
        return GetFixedTraitValueForTrait(SunlightFixedTraits, ref sunlightFixedTrait);
    }

    private int GetCosmicRadiationValueFromFixedTrait() {
        return GetFixedTraitValueForTrait(CosmicRadiationFixedTraits, ref cosmicRadiationFixedTrait);
    }

    public void SetWorldDetails(WorldGen world) {
        if (world != null) {
            FullyEnclosedBorder = world.Settings.GetBoolSetting("DrawWorldBorder") &&
                                  world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum");

            worldOffset  = world.GetPosition();
            worldSize    = world.GetSize();
            isDiscovered = world.isStartingWorld;
            IsStartWorld = world.isStartingWorld;
            worldName    = world.Settings.world.filePath;
            nameTables   = world.Settings.world.nameTables;
            worldTags = world.Settings.world.worldTags != null
                            ? world.Settings.world.worldTags.ToArray().ToTagArray()
                            : new Tag[0];

            worldDescription          = world.Settings.world.description;
            worldType                 = world.Settings.world.name;
            IsModuleInterior          = world.Settings.world.moduleInterior;
            m_seasonIds               = new List<string>(world.Settings.world.seasons);
            GeneratedBiomes           = world.Settings.world.generatedSubworlds;
            northernLightFixedTrait   = GetNorthernlightFixedTraits(world);
            sunlightFixedTrait        = GetSunlightFromFixedTraits(world);
            cosmicRadiationFixedTrait = GetCosmicRadiationFromFixedTraits(world);
            sunlight                  = GetSunlightValueFromFixedTrait();
            northernlights            = GetNorthernlightValueFromFixedTrait();
            cosmicRadiation           = GetCosmicRadiationValueFromFixedTrait();
            currentCosmicIntensity    = cosmicRadiation;
            var hashSet = new HashSet<string>();
            foreach (var text in world.Settings.world.generatedSubworlds) {
                text = text.Substring(0,                         text.LastIndexOf('/'));
                text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
                hashSet.Add(text);
            }

            Biomes        = hashSet.ToList();
            WorldTraitIds = new List<string>();
            WorldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
            StoryTraitIds = new List<string>();
            StoryTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
            return;
        }

        FullyEnclosedBorder = false;
        worldOffset         = Vector2I.zero;
        worldSize           = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
        isDiscovered        = true;
        IsStartWorld        = true;
        IsDupeVisited       = true;
        m_seasonIds         = new List<string> { Db.Get().GameplaySeasons.MeteorShowers.Id };
    }

    public bool ContainsPoint(Vector2 point) {
        return point.x >= worldOffset.x              &&
               point.y >= worldOffset.y              &&
               point.x < worldOffset.x + worldSize.x &&
               point.y < worldOffset.y + worldSize.y;
    }

    public void LookAtSurface() {
        if (!IsDupeVisited) RevealSurface();
        var vector = SetSurfaceCameraPos();
        if (ClusterManager.Instance.activeWorldId == id && vector != null)
            CameraController.Instance.SnapTo(vector.Value);
    }

    public void RevealSurface() {
        if (IsSurfaceRevealed) return;

        IsSurfaceRevealed = true;
        for (var i = 0; i < worldSize.x; i++) {
            for (var j = worldSize.y - 1; j >= 0; j--) {
                var cell = Grid.XYToCell(i + worldOffset.x, j + worldOffset.y);
                if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell)) break;

                GridVisibility.Reveal(i + worldOffset.X, j + worldOffset.y, 7, 1f);
            }
        }
    }

    private Vector3? SetSurfaceCameraPos() {
        if (SaveGame.Instance != null) {
            var num = (int)maximumBounds.y;
            for (var i = 0; i < worldSize.X; i++) {
                for (var j = worldSize.y - 1; j >= 0; j--) {
                    var num2 = j + worldOffset.y;
                    var num3 = Grid.XYToCell(i + worldOffset.x, num2);
                    if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3))) {
                        num = Math.Min(num, num2);
                        break;
                    }
                }
            }

            var num4   = (num + worldOffset.y + worldSize.y) / 2;
            var vector = new Vector3(WorldOffset.x + Width / 2, num4, 0f);
            SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(id, vector);
            return vector;
        }

        return null;
    }

    public void EjectAllDupes(Vector3 spawn_pos) {
        foreach (var minionIdentity in Components.MinionIdentities.GetWorldItems(id))
            minionIdentity.transform.SetLocalPosition(spawn_pos);
    }

    public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement) {
        foreach (var minionIdentity in Components.MinionIdentities.GetWorldItems(id))
            if (!minionIdentity.HasTag(GameTags.Dead)) {
                var position   = new Vector3(-1f, -1f, 0f);
                var gameObject = Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
                gameObject.GetComponent<PrimaryElement>().SetElement(podElement);
                gameObject.SetActive(true);
                gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
                var smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
                smi.StartSM();
                smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
            }
    }

    public void DestroyWorldBuildings(out HashSet<int> noRefundTiles) {
        TransferBuildingMaterials(out noRefundTiles);
        foreach (var cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(id)) cmp.DeleteObject();
        ClearWorldZones();
    }

    public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles) {
        TransferPickupables(spawn_pos);
        TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
    }

    public void TransferResourcesToDebris(AxialI       sourceLocation,
                                          HashSet<int> noRefundTiles,
                                          SimHashes    debrisContainerElement) {
        var list = new List<Storage>();
        TransferPickupablesToDebris(ref list, debrisContainerElement);
        TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
        foreach (var cmp in list) {
            var smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
            smi.StartSM();
            smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
        }
    }

    private void TransferBuildingMaterials(out HashSet<int> noRefundTiles) {
        var retTemplateFoundationCells = new HashSet<int>();
        var pooledList                 = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
        GameScenePartitioner.Instance.GatherEntries((int)minimumBounds.x,
                                                    (int)minimumBounds.y,
                                                    Width,
                                                    Height,
                                                    GameScenePartitioner.Instance.completeBuildings,
                                                    pooledList);

        Action<int> <>9__0;
        foreach (var scenePartitionerEntry in pooledList) {
            var buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
            if (buildingComplete != null) {
                var component = buildingComplete.GetComponent<Deconstructable>();
                if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund)) {
                    var component2   = buildingComplete.GetComponent<PrimaryElement>();
                    var temperature  = component2.Temperature;
                    var diseaseIdx   = component2.DiseaseIdx;
                    var diseaseCount = component2.DiseaseCount;
                    var num          = 0;
                    while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num) {
                        var element = ElementLoader.GetElement(component.constructionElements[num]);
                        if (element != null)
                            element.substance.SpawnResource(buildingComplete.transform.GetPosition(),
                                                            buildingComplete.Def.Mass[num],
                                                            temperature,
                                                            diseaseIdx,
                                                            diseaseCount);
                        else {
                            var prefab = Assets.GetPrefab(component.constructionElements[num]);
                            var num2   = 0;
                            while (num2 < buildingComplete.Def.Mass[num]) {
                                GameUtil.KInstantiate(prefab,
                                                      buildingComplete.transform.GetPosition(),
                                                      Grid.SceneLayer.Ore)
                                        .SetActive(true);

                                num2++;
                            }
                        }

                        num++;
                    }
                }

                var component3 = buildingComplete.GetComponent<SimCellOccupier>();
                if (component3 != null && component3.doReplaceElement) {
                    Building    building = buildingComplete;
                    Action<int> callback;
                    if ((callback =  <>9__0) == null)
                    {
                        callback = (<>9__0 = delegate(int cell) { retTemplateFoundationCells.Add(cell); });
                    }

                    building.RunOnArea(callback);
                }

                var component4 = buildingComplete.GetComponent<Storage>();
                if (component4 != null) component4.DropAll();
                var component5 = buildingComplete.GetComponent<PlantablePlot>();
                if (component5 != null) {
                    var seedProducer = component5.Occupant != null
                                           ? component5.Occupant.GetComponent<SeedProducer>()
                                           : null;

                    if (seedProducer != null) seedProducer.DropSeed();
                }

                buildingComplete.DeleteObject();
            }
        }

        pooledList.Clear();
        noRefundTiles = retTemplateFoundationCells;
    }

    private void TransferPickupables(Vector3 pos) {
        var cell       = Grid.PosToCell(pos);
        var pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
        GameScenePartitioner.Instance.GatherEntries((int)minimumBounds.x,
                                                    (int)minimumBounds.y,
                                                    Width,
                                                    Height,
                                                    GameScenePartitioner.Instance.pickupablesLayer,
                                                    pooledList);

        foreach (var scenePartitionerEntry in pooledList)
            if (scenePartitionerEntry.obj != null) {
                var pickupable = scenePartitionerEntry.obj as Pickupable;
                if (pickupable != null)
                    pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
            }

        pooledList.Recycle();
    }

    private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles) {
        var num = (int)minimumBounds.x;
        while (num <= maximumBounds.x) {
            var num2 = (int)minimumBounds.y;
            while (num2 <= maximumBounds.y) {
                var num3 = Grid.XYToCell(num, num2);
                if (!noRefundTiles.Contains(num3)) {
                    var element = Grid.Element[num3];
                    if (element != null && !element.IsVacuum)
                        element.substance.SpawnResource(pos,
                                                        Grid.Mass[num3],
                                                        Grid.Temperature[num3],
                                                        Grid.DiseaseIdx[num3],
                                                        Grid.DiseaseCount[num3]);
                }

                num2++;
            }

            num++;
        }
    }

    private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement) {
        var pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
        GameScenePartitioner.Instance.GatherEntries((int)minimumBounds.x,
                                                    (int)minimumBounds.y,
                                                    Width,
                                                    Height,
                                                    GameScenePartitioner.Instance.pickupablesLayer,
                                                    pooledList);

        foreach (var scenePartitionerEntry in pooledList)
            if (scenePartitionerEntry.obj != null) {
                var pickupable = scenePartitionerEntry.obj as Pickupable;
                if (pickupable != null) {
                    if (pickupable.KPrefabID.HasTag(GameTags.BaseMinion))
                        Util.KDestroyGameObject(pickupable.gameObject);
                    else {
                        pickupable.PrimaryElement.Units
                            = Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));

                        if ((debrisObjects.Count                                        == 0 ||
                             debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) &&
                            pickupable.PrimaryElement.Mass > 0f)
                            debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects",
                                               debrisContainerElement));

                        var storage = debrisObjects[debrisObjects.Count - 1];
                        while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity()) {
                            var pickupable2 = pickupable.Take(storage.RemainingCapacity());
                            storage.Store(pickupable2.gameObject);
                            storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects",
                                                                             debrisContainerElement);

                            debrisObjects.Add(storage);
                        }

                        if (pickupable.PrimaryElement.Mass > 0f) storage.Store(pickupable.gameObject);
                    }
                }
            }

        pooledList.Recycle();
    }

    private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects,
                                                       HashSet<int>      noRefundTiles,
                                                       SimHashes         debrisContainerElement) {
        var num = (int)minimumBounds.x;
        while (num <= maximumBounds.x) {
            var num2 = (int)minimumBounds.y;
            while (num2 <= maximumBounds.y) {
                var num3 = Grid.XYToCell(num, num2);
                if (!noRefundTiles.Contains(num3)) {
                    var element = Grid.Element[num3];
                    if (element != null && !element.IsVacuum) {
                        var num4 = Grid.Mass[num3];
                        num4 *= 0.5f;
                        if ((debrisObjects.Count                                        == 0 ||
                             debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) &&
                            num4 > 0f)
                            debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles",
                                               debrisContainerElement));

                        var storage = debrisObjects[debrisObjects.Count - 1];
                        while (num4 > 0f) {
                            var num5 = Mathf.Min(num4, storage.RemainingCapacity());
                            num4 -= num5;
                            storage.AddOre(element.id,
                                           num5,
                                           Grid.Temperature[num3],
                                           Grid.DiseaseIdx[num3],
                                           Grid.DiseaseCount[num3]);

                            if (num4 > 0f) {
                                storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles",
                                 debrisContainerElement);

                                debrisObjects.Add(storage);
                            }
                        }
                    }
                }

                num2++;
            }

            num++;
        }
    }

    public void CancelChores() {
        for (var i = 0; i < 45; i++) {
            var num = (int)minimumBounds.x;
            while (num <= maximumBounds.x) {
                var num2 = (int)minimumBounds.y;
                while (num2 <= maximumBounds.y) {
                    var cell       = Grid.XYToCell(num, num2);
                    var gameObject = Grid.Objects[cell, i];
                    if (gameObject != null) gameObject.Trigger(2127324410, true);
                    num2++;
                }

                num++;
            }
        }

        List<Chore> list;
        GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(id, out list);
        var num3 = 0;
        while (list != null && num3 < list.Count) {
            var chore = list[num3];
            if (chore != null && chore.target != null && !chore.isNull) chore.Cancel("World destroyed");
            num3++;
        }

        List<FetchChore> list2;
        GlobalChoreProvider.Instance.fetchMap.TryGetValue(id, out list2);
        var num4 = 0;
        while (list2 != null && num4 < list2.Count) {
            var fetchChore = list2[num4];
            if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
                fetchChore.Cancel("World destroyed");

            num4++;
        }
    }

    public void ClearWorldZones() {
        if (this.overworldCell != null) {
            var clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
            var num               = -1;
            for (var i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++) {
                var overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
                if (overworldCell.zoneType  == this.overworldCell.zoneType  &&
                    overworldCell.tags      != null                         &&
                    this.overworldCell.tags != null                         &&
                    overworldCell.tags.ContainsAll(this.overworldCell.tags) &&
                    overworldCell.poly.bounds == this.overworldCell.poly.bounds) {
                    num = i;
                    break;
                }
            }

            if (num >= 0) clusterDetailSave.overworldCells.RemoveAt(num);
        }

        var num2 = (int)minimumBounds.y;
        while (num2 <= maximumBounds.y) {
            var num3 = (int)minimumBounds.x;
            while (num3 <= maximumBounds.x) {
                SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
                num3++;
            }

            num2++;
        }
    }

    public int GetSafeCell() {
        if (IsModuleInterior)
            using (var enumerator = Components.RocketControlStations.Items.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var rocketControlStation = enumerator.Current;
                    if (rocketControlStation.GetMyWorldId() == id) return Grid.PosToCell(rocketControlStation);
                }

                goto IL_A2;
            }

        foreach (var telepad in Components.Telepads.Items)
            if (telepad.GetMyWorldId() == id)
                return Grid.PosToCell(telepad);

        IL_A2:
        return Grid.XYToCell(worldOffset.x + worldSize.x / 2, worldOffset.y + worldSize.y / 2);
    }

    public string GetStatus() { return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(id); }
}