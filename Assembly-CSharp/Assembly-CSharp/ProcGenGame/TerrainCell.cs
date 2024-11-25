using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGen.Map;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame {
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TerrainCell {
        public delegate void SetValuesFunction(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc);

        private const float MASS_VARIATION                = 0.2f;
        public const  int   DONT_SET_TEMPERATURE_DEFAULTS = -1;

        private static readonly Tag[] noPOISpawnTags = {
            WorldGenTags.StartLocation,
            WorldGenTags.AtStart,
            WorldGenTags.NearStartLocation,
            WorldGenTags.POI,
            WorldGenTags.Feature
        };

        private static readonly TagSet noPOISpawnTagSet = new TagSet(noPOISpawnTags);

        private static readonly Tag[] relaxedNoPOISpawnTags = {
            WorldGenTags.StartLocation, WorldGenTags.AtStart, WorldGenTags.NearStartLocation, WorldGenTags.POI
        };

        private static readonly TagSet relaxedNoPOISpawnTagSet             = new TagSet(relaxedNoPOISpawnTags);
        private static readonly Tag[]  noPOISpawnTagsNearStart             = { WorldGenTags.POI, WorldGenTags.Feature };
        private static readonly TagSet noPOISpawnNearStartTagSet           = new TagSet(noPOISpawnTagsNearStart);
        private static readonly Tag[]  relaxedNoPOISpawnTagsAllowNearStart = { WorldGenTags.POI };

        private static readonly TagSet relaxedNoPOISpawnAllowNearStartTagSet
            = new TagSet(relaxedNoPOISpawnTagsAllowNearStart);

        private List<int>                    allCells;
        private HashSet<int>                 availableSpawnPoints;
        private HashSet<int>                 availableTerrainPoints;
        private bool                         debugMode;
        private HashSet<int>                 featureSpawnPoints;
        private float                        finalSize;
        public  List<int>                    neighbourTerrainCells = new List<int>();
        public  List<KeyValuePair<int, Tag>> poi;
        public  List<KeyValuePair<int, Tag>> terrainPositions;
        protected TerrainCell() { }

        protected TerrainCell(Cell node, Diagram.Site site, Dictionary<Tag, int> distancesToTags) {
            this.node            = node;
            this.site            = site;
            this.distancesToTags = distancesToTags;
        }

        public Polygon                      poly            => site.poly;
        public Cell                         node            { get; }
        public Diagram.Site                 site            { get; }
        public Dictionary<Tag, int>         distancesToTags { get; }
        public bool                         HasMobs         => mobs != null && mobs.Count > 0;
        public List<KeyValuePair<int, Tag>> mobs            { get; private set; }

        public virtual void LogInfo(string evt, string param, float value) {
            Debug.Log(string.Concat(evt, ":", param, "=", value.ToString()));
        }

        public void InitializeCells(HashSet<int> claimedCells) {
            if (allCells != null) return;

            allCells               = new List<int>();
            availableTerrainPoints = new HashSet<int>();
            availableSpawnPoints   = new HashSet<int>();
            var num = (int)poly.bounds.y;
            while (num < poly.bounds.y + poly.bounds.height) {
                var num2 = (int)poly.bounds.x;
                while (num2 < poly.bounds.x + poly.bounds.width) {
                    if (poly.Contains(new Vector2(num2, num))) {
                        var item = Grid.XYToCell(num2, num);
                        availableTerrainPoints.Add(item);
                        availableSpawnPoints.Add(item);
                        if (claimedCells.Add(item)) allCells.Add(item);
                    }

                    num2++;
                }

                num++;
            }

            LogInfo("Initialise cells", "", allCells.Count);
        }

        public List<int> GetAllCells() { return new List<int>(allCells); }

        public List<int> GetAvailableSpawnCellsAll() {
            var list = new List<int>();
            foreach (var item in availableSpawnPoints) list.Add(item);
            return list;
        }

        public List<int> GetAvailableSpawnCellsFeature() {
            var list    = new List<int>();
            var hashSet = new HashSet<int>(availableSpawnPoints);
            hashSet.ExceptWith(availableTerrainPoints);
            foreach (var item in hashSet) list.Add(item);
            return list;
        }

        public List<int> GetAvailableSpawnCellsBiome() {
            var list    = new List<int>();
            var hashSet = new HashSet<int>(availableSpawnPoints);
            hashSet.ExceptWith(featureSpawnPoints);
            foreach (var item in hashSet) list.Add(item);
            return list;
        }

        private bool RemoveFromAvailableSpawnCells(int cell) { return availableSpawnPoints.Remove(cell); }

        public void AddMobs(IEnumerable<KeyValuePair<int, Tag>> newMobs) {
            foreach (var mob in newMobs) AddMob(mob);
        }

        private void AddMob(int cellIdx, string tag) { AddMob(new KeyValuePair<int, Tag>(cellIdx, new Tag(tag))); }

        public void AddMob(KeyValuePair<int, Tag> mob) {
            if (mobs == null) mobs = new List<KeyValuePair<int, Tag>>();
            mobs.Add(mob);
            var flag = RemoveFromAvailableSpawnCells(mob.Key);
            LogInfo("\t\t\tRemoveFromAvailableCells", mob.Value.Name + ": " + (flag ? "success" : "failed"), mob.Key);
            if (!flag) {
                if (!allCells.Contains(mob.Key)) {
                    Debug.Assert(false,
                                 string.Concat("Couldnt find cell [",
                                               mob.Key.ToString(),
                                               "] we dont own, to remove for mob [",
                                               mob.Value.Name,
                                               "]"));

                    return;
                }

                Debug.Assert(false,
                             string.Concat("Couldnt find cell [",
                                           mob.Key.ToString(),
                                           "] to remove for mob [",
                                           mob.Value.Name,
                                           "]"));
            }
        }

        protected string GetSubWorldType(WorldGen worldGen) {
            var pos = new Vector2I((int)site.poly.Centroid().x, (int)site.poly.Centroid().y);
            return worldGen.GetSubWorldType(pos);
        }

        protected Temperature.Range GetTemperatureRange(WorldGen worldGen) {
            var subWorldType = GetSubWorldType(worldGen);
            if (subWorldType == null) return Temperature.Range.Mild;

            if (!worldGen.Settings.HasSubworld(subWorldType)) return Temperature.Range.Mild;

            return worldGen.Settings.GetSubWorld(subWorldType).temperatureRange;
        }

        protected void GetTemperatureRange(WorldGen worldGen, ref float min, ref float range) {
            var temperatureRange = GetTemperatureRange(worldGen);
            min   = SettingsCache.temperatures[temperatureRange].min;
            range = SettingsCache.temperatures[temperatureRange].max - min;
        }

        protected float GetDensityMassForCell(Chunk world, int cellIdx, float mass) {
            if (!Grid.IsValidCell(cellIdx)) return 0f;

            Debug.Assert(world.density[cellIdx] >= 0f && world.density[cellIdx] <= 1f,
                         "Density [" + world.density[cellIdx] + "] out of range [0-1]");

            var num                 = 0.2f * (world.density[cellIdx] - 0.5f);
            var num2                = mass + mass * num;
            if (num2 > 10000f) num2 = 10000f;
            return num2;
        }

        private void HandleSprinkleOfElement(WorldGenSettings  settings,
                                             Tag               targetTag,
                                             Chunk             world,
                                             SetValuesFunction SetValues,
                                             float             temperatureMin,
                                             float             temperatureRange,
                                             SeededRandom      rnd) {
            var element = ElementLoader.FindElementByName(settings.GetFeature(targetTag.Name)
                                                                  .GetOneWeightedSimHash("SprinkleOfElementChoices",
                                                                   rnd)
                                                                  .element);

            ProcGen.Room room = null;
            SettingsCache.rooms.TryGetValue(targetTag.Name, out room);
            SampleDescriber sampleDescriber = room;
            var             defaultValues   = element.defaultValues;
            var             invalid         = Sim.DiseaseCell.Invalid;
            for (var i = 0; i < terrainPositions.Count; i++)
                if (!(terrainPositions[i].Value != targetTag)) {
                    var radius       = rnd.RandomRange(sampleDescriber.blobSize.min, sampleDescriber.blobSize.max);
                    var filledCircle = ProcGen.Util.GetFilledCircle(Grid.CellToPos2D(terrainPositions[i].Key), radius);
                    for (var j = 0; j < filledCircle.Count; j++) {
                        var num = Grid.XYToCell(filledCircle[j].x, filledCircle[j].y);
                        if (Grid.IsValidCell(num)) {
                            defaultValues.mass        = GetDensityMassForCell(world, num, element.defaultValues.mass);
                            defaultValues.temperature = temperatureMin + world.heatOffset[num] * temperatureRange;
                            SetValues(num, element, defaultValues, invalid);
                        }
                    }
                }
        }

        private HashSet<Vector2I> DigFeature(ProcGen.Room.Shape       shape,
                                             float                    size,
                                             List<int>                bordersWidths,
                                             SeededRandom             rnd,
                                             out List<Vector2I>       featureCenterPoints,
                                             out List<List<Vector2I>> featureBorders) {
            var hashSet = new HashSet<Vector2I>();
            featureCenterPoints = new List<Vector2I>();
            featureBorders      = new List<List<Vector2I>>();
            if (size < 1f) return hashSet;

            var center = site.poly.Centroid();
            finalSize = size;
            switch (shape) {
                case ProcGen.Room.Shape.Circle:
                    featureCenterPoints = ProcGen.Util.GetFilledCircle(center, finalSize);
                    break;
                case ProcGen.Room.Shape.Blob:
                    featureCenterPoints = ProcGen.Util.GetBlob(center, finalSize, rnd.RandomSource());
                    break;
                case ProcGen.Room.Shape.Square:
                    featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, finalSize, finalSize, rnd);
                    break;
                case ProcGen.Room.Shape.TallThin:
                    featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, finalSize / 4f, finalSize, rnd);
                    break;
                case ProcGen.Room.Shape.ShortWide:
                    featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, finalSize, finalSize / 4f, rnd);
                    break;
                case ProcGen.Room.Shape.Splat:
                    featureCenterPoints = ProcGen.Util.GetSplat(center, finalSize, rnd.RandomSource());
                    break;
            }

            hashSet.UnionWith(featureCenterPoints);
            if (featureCenterPoints.Count != 0    &&
                bordersWidths             != null &&
                bordersWidths.Count       > 0     &&
                bordersWidths[0]          > 0) {
                var num = 0;
                while (num < bordersWidths.Count && bordersWidths[num] > 0) {
                    featureBorders.Add(ProcGen.Util.GetBorder(hashSet, bordersWidths[num]));
                    hashSet.UnionWith(featureBorders[num]);
                    num++;
                }
            }

            return hashSet;
        }

        public static ElementOverride GetElementOverride(string element, SampleDescriber.Override overrides) {
            Debug.Assert(element != null && element.Length > 0);
            var elementOverride = new ElementOverride { element = ElementLoader.FindElementByName(element) };
            Debug.Assert(elementOverride.element != null, "Couldn't find an element called " + element);
            elementOverride.pdelement   = elementOverride.element.defaultValues;
            elementOverride.dc          = Sim.DiseaseCell.Invalid;
            elementOverride.mass        = elementOverride.element.defaultValues.mass;
            elementOverride.temperature = elementOverride.element.defaultValues.temperature;
            if (overrides == null) return elementOverride;

            elementOverride.overrideMass          = false;
            elementOverride.overrideTemperature   = false;
            elementOverride.overrideDiseaseIdx    = false;
            elementOverride.overrideDiseaseAmount = false;
            if (overrides.massOverride != null) {
                elementOverride.mass         = overrides.massOverride.Value;
                elementOverride.overrideMass = true;
            }

            if (overrides.massMultiplier != null) {
                elementOverride.mass         *= overrides.massMultiplier.Value;
                elementOverride.overrideMass =  true;
            }

            if (overrides.temperatureOverride != null) {
                elementOverride.temperature         = overrides.temperatureOverride.Value;
                elementOverride.overrideTemperature = true;
            }

            if (overrides.temperatureMultiplier != null) {
                elementOverride.temperature         *= overrides.temperatureMultiplier.Value;
                elementOverride.overrideTemperature =  true;
            }

            if (overrides.diseaseOverride != null) {
                elementOverride.diseaseIdx         = WorldGen.diseaseStats.GetIndex(overrides.diseaseOverride);
                elementOverride.overrideDiseaseIdx = true;
            }

            if (overrides.diseaseAmountOverride != null) {
                elementOverride.diseaseAmount         = overrides.diseaseAmountOverride.Value;
                elementOverride.overrideDiseaseAmount = true;
            }

            if (elementOverride.overrideTemperature)
                elementOverride.pdelement.temperature = elementOverride.temperature;

            if (elementOverride.overrideMass) elementOverride.pdelement.mass           = elementOverride.mass;
            if (elementOverride.overrideDiseaseIdx) elementOverride.dc.diseaseIdx      = elementOverride.diseaseIdx;
            if (elementOverride.overrideDiseaseAmount) elementOverride.dc.elementCount = elementOverride.diseaseAmount;
            return elementOverride;
        }

        private bool IsFeaturePointContainedInBorder(Vector2 point, WorldGen worldGen) {
            if (!node.tags.Contains(WorldGenTags.AllowExceedNodeBorders)) return true;

            if (!poly.Contains(point)) {
                var terrainCell = worldGen.TerrainCells.Find(x => x.poly.Contains(point));
                if (terrainCell != null) {
                    var subWorld  = worldGen.Settings.GetSubWorld(node.GetSubworld());
                    var subWorld2 = worldGen.Settings.GetSubWorld(terrainCell.node.GetSubworld());
                    if (subWorld.zoneType != subWorld2.zoneType) return false;
                }
            }

            return true;
        }

        private void ApplyPlaceElementForRoom(FeatureSettings   feature,
                                              string            group,
                                              List<Vector2I>    cells,
                                              WorldGen          worldGen,
                                              Chunk             world,
                                              SetValuesFunction SetValues,
                                              float             temperatureMin,
                                              float             temperatureRange,
                                              SeededRandom      rnd,
                                              HashSet<int>      highPriorityClaims) {
            if (cells == null || cells.Count == 0) return;

            if (!feature.HasGroup(group)) return;

            switch (feature.ElementChoiceGroups[group].selectionMethod) {
                case ProcGen.Room.Selection.Weighted:
                case ProcGen.Room.Selection.WeightedResample:
                    for (var i = 0; i < cells.Count; i++) {
                        var num = Grid.XYToCell(cells[i].x, cells[i].y);
                        if (Grid.IsValidCell(num)             &&
                            !highPriorityClaims.Contains(num) &&
                            IsFeaturePointContainedInBorder(cells[i], worldGen)) {
                            var oneWeightedSimHash = feature.GetOneWeightedSimHash(group, rnd);
                            var elementOverride
                                = GetElementOverride(oneWeightedSimHash.element, oneWeightedSimHash.overrides);

                            if (!elementOverride.overrideTemperature)
                                elementOverride.pdelement.temperature
                                    = temperatureMin + world.heatOffset[num] * temperatureRange;

                            if (!elementOverride.overrideMass)
                                elementOverride.pdelement.mass
                                    = GetDensityMassForCell(world, num, elementOverride.mass);

                            SetValues(num, elementOverride.element, elementOverride.pdelement, elementOverride.dc);
                        }
                    }

                    return;
                case ProcGen.Room.Selection.HorizontalSlice: {
                    var num2 = int.MaxValue;
                    var num3 = int.MinValue;
                    for (var j = 0; j < cells.Count; j++) {
                        num2 = Mathf.Min(cells[j].y, num2);
                        num3 = Mathf.Max(cells[j].y, num3);
                    }

                    var num4 = num3 - num2;
                    for (var k = 0; k < cells.Count; k++) {
                        var num5 = Grid.XYToCell(cells[k].x, cells[k].y);
                        if (Grid.IsValidCell(num5)             &&
                            !highPriorityClaims.Contains(num5) &&
                            IsFeaturePointContainedInBorder(cells[k], worldGen)) {
                            var percentage              = 1f - (cells[k].y - num2) / (float)num4;
                            var weightedSimHashAtChoice = feature.GetWeightedSimHashAtChoice(group, percentage);
                            var elementOverride2
                                = GetElementOverride(weightedSimHashAtChoice.element,
                                                     weightedSimHashAtChoice.overrides);

                            if (!elementOverride2.overrideTemperature)
                                elementOverride2.pdelement.temperature
                                    = temperatureMin + world.heatOffset[num5] * temperatureRange;

                            if (!elementOverride2.overrideMass)
                                elementOverride2.pdelement.mass
                                    = GetDensityMassForCell(world, num5, elementOverride2.mass);

                            SetValues(num5, elementOverride2.element, elementOverride2.pdelement, elementOverride2.dc);
                        }
                    }

                    return;
                }
            }

            var oneWeightedSimHash2 = feature.GetOneWeightedSimHash(group, rnd);
            for (var l = 0; l < cells.Count; l++) {
                var num6 = Grid.XYToCell(cells[l].x, cells[l].y);
                if (Grid.IsValidCell(num6)             &&
                    !highPriorityClaims.Contains(num6) &&
                    IsFeaturePointContainedInBorder(cells[l], worldGen)) {
                    var elementOverride3
                        = GetElementOverride(oneWeightedSimHash2.element, oneWeightedSimHash2.overrides);

                    if (!elementOverride3.overrideTemperature)
                        elementOverride3.pdelement.temperature
                            = temperatureMin + world.heatOffset[num6] * temperatureRange;

                    if (!elementOverride3.overrideMass)
                        elementOverride3.pdelement.mass = GetDensityMassForCell(world, num6, elementOverride3.mass);

                    SetValues(num6, elementOverride3.element, elementOverride3.pdelement, elementOverride3.dc);
                }
            }
        }

        private int GetIndexForLocation(List<Vector2I> points, Mob.Location location, SeededRandom rnd) {
            var num = -1;
            if (points == null || points.Count == 0) return num;

            if (location == Mob.Location.Air || location == Mob.Location.Solid) return rnd.RandomRange(0, points.Count);

            for (var i = 0; i < points.Count; i++)
                if (Grid.IsValidCell(Grid.XYToCell(points[i].x, points[i].y))) {
                    if (num == -1)
                        num = i;
                    else if (location != Mob.Location.Floor) {
                        if (location == Mob.Location.Ceiling && points[i].y > points[num].y) num = i;
                    } else if (points[i].y < points[num].y) num = i;
                }

            return num;
        }

        private void PlaceMobsInRoom(WorldGenSettings   settings,
                                     List<MobReference> mobTags,
                                     List<Vector2I>     points,
                                     SeededRandom       rnd) {
            if (points == null) return;

            if (mobs == null) mobs = new List<KeyValuePair<int, Tag>>();
            for (var i = 0; i < mobTags.Count; i++)
                if (!settings.HasMob(mobTags[i].type))
                    Debug.LogError("Missing sample description for tag [" + mobTags[i].type + "]");
                else {
                    var mob = settings.GetMob(mobTags[i].type);
                    var num = Mathf.RoundToInt(mobTags[i].count.GetRandomValueWithinRange(rnd));
                    for (var j = 0; j < num; j++) {
                        var indexForLocation = GetIndexForLocation(points, mob.location, rnd);
                        if (indexForLocation == -1) break;

                        if (points.Count <= indexForLocation) return;

                        var cellIdx = Grid.XYToCell(points[indexForLocation].x, points[indexForLocation].y);
                        points.RemoveAt(indexForLocation);
                        AddMob(cellIdx, mobTags[i].type);
                    }
                }
        }

        private int[] ConvertNoiseToPoints(float[] basenoise, float minThreshold = 0.9f, float maxThreshold = 1f) {
            if (basenoise == null) return null;

            var list   = new List<int>();
            var width  = site.poly.bounds.width;
            var height = site.poly.bounds.height;
            for (var num = site.position.y - height / 2f; num < site.position.y + height / 2f; num += 1f) {
                for (var num2 = site.position.x - width / 2f; num2 < site.position.x + width / 2f; num2 += 1f) {
                    var num3 = Grid.PosToCell(new Vector2(num2, num));
                    if (site.poly.Contains(new Vector2(num2, num))) {
                        float num4 = (int)basenoise[num3];
                        if (num4 >= minThreshold && num4 <= maxThreshold && !list.Contains(num3))
                            list.Add(Grid.PosToCell(new Vector2(num2, num)));
                    }
                }
            }

            return list.ToArray();
        }

        private void ApplyForeground(WorldGen          worldGen,
                                     Chunk             world,
                                     SetValuesFunction SetValues,
                                     float             temperatureMin,
                                     float             temperatureRange,
                                     SeededRandom      rnd) {
            LogInfo("Apply foregreound", (node.tags != null).ToString(), node.tags != null ? node.tags.Count : 0);
            if (node.tags != null) {
                var featureSettings = worldGen.Settings.TryGetFeature(node.type);
                LogInfo("\tFeature?", (featureSettings != null).ToString(), 0f);
                if (featureSettings == null && node.tags != null) {
                    var list = new List<Tag>();
                    foreach (var item in node.tags)
                        if (worldGen.Settings.HasFeature(item.Name))
                            list.Add(item);

                    LogInfo("\tNo feature, checking possible feature tags, found", "", list.Count);
                    if (list.Count > 0) {
                        var tag = list[rnd.RandomSource().Next(list.Count)];
                        featureSettings = worldGen.Settings.GetFeature(tag.Name);
                        LogInfo("\tPicked feature", tag.Name, 0f);
                    }
                }

                if (featureSettings != null) {
                    LogInfo("APPLY FOREGROUND", node.type, 0f);
                    var num  = featureSettings.blobSize.GetRandomValueWithinRange(rnd);
                    var num2 = poly.DistanceToClosestEdge();
                    if (!node.tags.Contains(WorldGenTags.AllowExceedNodeBorders) && num2 < num) {
                        if (debugMode)
                            Debug.LogWarning(string.Concat(node.type,
                                                           " ",
                                                           featureSettings.shape.ToString(),
                                                           "  blob size too large to fit in node. Size reduced. ",
                                                           num.ToString(),
                                                           "->",
                                                           (num2 - 6f).ToString()));

                        num = num2 - 6f;
                    }

                    if (num <= 0f) return;

                    List<Vector2I>       cells;
                    List<List<Vector2I>> list2;
                    var hashSet = DigFeature(featureSettings.shape,
                                             num,
                                             featureSettings.borders,
                                             rnd,
                                             out cells,
                                             out list2);

                    featureSpawnPoints = new HashSet<int>();
                    foreach (var vector2I in hashSet) featureSpawnPoints.Add(Grid.XYToCell(vector2I.x, vector2I.y));
                    LogInfo("\t\t", "claimed points", featureSpawnPoints.Count);
                    availableTerrainPoints.ExceptWith(featureSpawnPoints);
                    ApplyPlaceElementForRoom(featureSettings,
                                             "RoomCenterElements",
                                             cells,
                                             worldGen,
                                             world,
                                             SetValues,
                                             temperatureMin,
                                             temperatureRange,
                                             rnd,
                                             worldGen.HighPriorityClaimedCells);

                    if (list2 != null)
                        for (var i = 0; i < list2.Count; i++)
                            ApplyPlaceElementForRoom(featureSettings,
                                                     "RoomBorderChoices" + i,
                                                     list2[i],
                                                     worldGen,
                                                     world,
                                                     SetValues,
                                                     temperatureMin,
                                                     temperatureRange,
                                                     rnd,
                                                     worldGen.HighPriorityClaimedCells);

                    if (featureSettings.tags.Contains(WorldGenTags.HighPriorityFeature.Name))
                        worldGen.AddHighPriorityCells(featureSpawnPoints);
                }
            }
        }

        private void ApplyBackground(WorldGen          worldGen,
                                     Chunk             world,
                                     SetValuesFunction SetValues,
                                     float             temperatureMin,
                                     float             temperatureRange,
                                     SeededRandom      rnd) {
            LogInfo("Apply Background", node.type, 0f);
            var floatSetting       = worldGen.Settings.GetFloatSetting("CaveOverrideMaxValue");
            var floatSetting2      = worldGen.Settings.GetFloatSetting("CaveOverrideSliverValue");
            var leafForTerrainCell = worldGen.GetLeafForTerrainCell(this);
            var flag               = leafForTerrainCell.tags.Contains(WorldGenTags.IgnoreCaveOverride);
            var flag2              = leafForTerrainCell.tags.Contains(WorldGenTags.CaveVoidSliver);
            var flag3              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroid);
            var flag4              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroidInv);
            var flag5              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdge);
            var flag6              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdgeInv);
            var flag7              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorder);
            var flag8              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderWeak);
            var flag9              = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderInv);
            var flag10             = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTop);
            var flag11             = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTopOrSide);
            var flag12             = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointCentroid);
            var flag13             = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointEdge);
            LogInfo("Getting Element Bands", node.type, 0f);
            var elementBandConfiguration = worldGen.Settings.GetElementBandForBiome(node.type);
            if (elementBandConfiguration == null && node.biomeSpecificTags != null) {
                LogInfo("\tType is not a biome, checking tags", "", node.tags.Count);
                var list = new List<ElementBandConfiguration>();
                foreach (var tag in node.biomeSpecificTags) {
                    var elementBandForBiome = worldGen.Settings.GetElementBandForBiome(tag.Name);
                    if (elementBandForBiome != null) {
                        list.Add(elementBandForBiome);
                        LogInfo("\tFound biome", tag.Name, 0f);
                    }
                }

                if (list.Count > 0) {
                    var num = rnd.RandomSource().Next(list.Count);
                    elementBandConfiguration = list[num];
                    LogInfo("\tPicked biome", "", num);
                }
            }

            DebugUtil.Assert(elementBandConfiguration != null, "A node didn't get assigned a biome! ", node.type);
            foreach (var num2 in availableTerrainPoints) {
                var vector2I = Grid.CellToXY(num2);
                if (!worldGen.HighPriorityClaimedCells.Contains(num2)) {
                    var num3 = world.overrides[num2];
                    if (!flag && num3 >= 100f) {
                        if (num3 >= 300f)
                            SetValues(num2,
                                      WorldGen.voidElement,
                                      WorldGen.voidElement.defaultValues,
                                      Sim.DiseaseCell.Invalid);
                        else if (num3 >= 200f)
                            SetValues(num2,
                                      WorldGen.unobtaniumElement,
                                      WorldGen.unobtaniumElement.defaultValues,
                                      Sim.DiseaseCell.Invalid);
                        else
                            SetValues(num2,
                                      WorldGen.katairiteElement,
                                      WorldGen.katairiteElement.defaultValues,
                                      Sim.DiseaseCell.Invalid);
                    } else {
                        var num4   = 1f;
                        var vector = new Vector2(vector2I.x, vector2I.y);
                        if (flag3 || flag4) {
                            var num5 = 15f;
                            if (flag13) {
                                var d           = 0f;
                                var closestEdge = poly.GetClosestEdge(vector, ref d);
                                num5 = Vector2.Distance(closestEdge.First +
                                                        (closestEdge.Second - closestEdge.First) * d,
                                                        vector);
                            }

                            num4 = Vector2.Distance(poly.Centroid(), vector) / num5;
                            num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
                            if (flag4) num4 = 1f - num4;
                        }

                        if (flag6 || flag5) {
                            var d2           = 0f;
                            var closestEdge2 = poly.GetClosestEdge(vector, ref d2);
                            var a            = closestEdge2.First + (closestEdge2.Second - closestEdge2.First) * d2;
                            var num6         = 15f;
                            if (flag12) num6 = Vector2.Distance(poly.Centroid(), vector);
                            num4 = Vector2.Distance(a,                           vector) / num6;
                            num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
                            if (flag6) num4 = 1f - num4;
                        }

                        if (flag9 || flag7) {
                            var edgesWithTag
                                = worldGen.WorldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);

                            var num7 = float.MaxValue;
                            foreach (var edge in edgesWithTag) {
                                var segment = new MathUtil.Pair<Vector2, Vector2>(edge.corner0.position,
                                 edge.corner1.position);

                                var num8 = 0f;
                                num7 = Mathf.Min(Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment,
                                                            vector,
                                                            ref num8)),
                                                 num7);
                            }

                            var num9         = flag8 ? 7f : 20f;
                            if (flag12) num9 = Vector2.Distance(poly.Centroid(), vector);
                            num4 = num7 / num9;
                            num4 = Mathf.Max(0f, Mathf.Min(1f, num4));
                            if (flag9) num4 = 1f - num4;
                        }

                        if (flag10) {
                            float y     = worldGen.WorldSize.y;
                            var   num10 = 38f;
                            var   num11 = 58f;
                            var   num12 = y - vector.y;
                            if (num12 < num10)
                                num4 = 0f;
                            else if (num12 < num11)
                                num4 = Mathf.Clamp01((num12 - num10) / (num11 - num10));
                            else
                                num4 = 1f;
                        }

                        if (flag11) {
                            float y2    = worldGen.WorldSize.y;
                            var   x     = worldGen.WorldSize.x;
                            var   num13 = 2f;
                            var   num14 = 10f;
                            var   num15 = y2 - vector.y;
                            var   x2    = vector.x;
                            var   num16 = x - vector.x;
                            var   num17 = Mathf.Min(num15, x2, num16);
                            if (num17 < num13)
                                num4 = 0f;
                            else if (num17 < num14)
                                num4 = Mathf.Clamp01((num17 - num13) / (num14 - num13));
                            else
                                num4 = 1f;
                        }

                        Element         element;
                        Sim.PhysicsData defaultValues;
                        Sim.DiseaseCell dc;
                        worldGen.GetElementForBiomePoint(world,
                                                         elementBandConfiguration,
                                                         vector2I,
                                                         out element,
                                                         out defaultValues,
                                                         out dc,
                                                         num4);

                        defaultValues.mass += defaultValues.mass *
                                              0.2f               *
                                              (world.density[vector2I.x + world.size.x * vector2I.y] - 0.5f);

                        if (!element.IsVacuum                 &&
                            element.id != SimHashes.Katairite &&
                            element.id != SimHashes.Unobtanium) {
                            var num18 = temperatureMin;
                            if (element.lowTempTransition != null && temperatureMin < element.lowTemp)
                                num18 = element.lowTemp;

                            defaultValues.temperature = num18 + world.heatOffset[num2] * temperatureRange;
                        }

                        if (element.IsSolid && !flag && num3 > floatSetting && num3 < 100f) {
                            if (flag2 && num3 > floatSetting2)
                                element = WorldGen.voidElement;
                            else
                                element = WorldGen.vacuumElement;

                            defaultValues = element.defaultValues;
                        }

                        SetValues(num2, element, defaultValues, dc);
                    }
                }
            }

            if (node.tags.Contains(WorldGenTags.SprinkleOfOxyRock))
                HandleSprinkleOfElement(worldGen.Settings,
                                        WorldGenTags.SprinkleOfOxyRock,
                                        world,
                                        SetValues,
                                        temperatureMin,
                                        temperatureRange,
                                        rnd);

            if (node.tags.Contains(WorldGenTags.SprinkleOfMetal))
                HandleSprinkleOfElement(worldGen.Settings,
                                        WorldGenTags.SprinkleOfMetal,
                                        world,
                                        SetValues,
                                        temperatureMin,
                                        temperatureRange,
                                        rnd);
        }

        private void GenerateActionCells(WorldGenSettings settings,
                                         Tag              tag,
                                         HashSet<int>     possiblePoints,
                                         SeededRandom     rnd) {
            ProcGen.Room room = null;
            SettingsCache.rooms.TryGetValue(tag.Name, out room);
            SampleDescriber sampleDescriber                                           = room;
            if (sampleDescriber == null && settings.HasMob(tag.Name)) sampleDescriber = settings.GetMob(tag.Name);
            if (sampleDescriber == null) return;

            var           hashSet                = new HashSet<int>();
            var           randomValueWithinRange = sampleDescriber.density.GetRandomValueWithinRange(rnd);
            var           selectMethod           = sampleDescriber.selectMethod;
            List<Vector2> list;
            if (selectMethod != SampleDescriber.PointSelectionMethod.RandomPoints) {
                if (selectMethod != SampleDescriber.PointSelectionMethod.Centroid) { }

                list = new List<Vector2>();
                list.Add(node.position);
            } else
                list = PointGenerator.GetRandomPoints(poly,
                                                      randomValueWithinRange,
                                                      0f,
                                                      null,
                                                      sampleDescriber.sampleBehaviour,
                                                      true,
                                                      rnd);

            foreach (var vector in list) {
                var item = Grid.XYToCell((int)vector.x, (int)vector.y);
                if (possiblePoints.Contains(item)) hashSet.Add(item);
            }

            if (room != null && room.mobselection == ProcGen.Room.Selection.None) {
                if (terrainPositions == null) terrainPositions = new List<KeyValuePair<int, Tag>>();
                foreach (var num in hashSet)
                    if (Grid.IsValidCell(num))
                        terrainPositions.Add(new KeyValuePair<int, Tag>(num, tag));
            }
        }

        private void DoProcess(WorldGen worldGen, Chunk world, SetValuesFunction SetValues, SeededRandom rnd) {
            var temperatureMin   = 265f;
            var temperatureRange = 30f;
            InitializeCells(worldGen.ClaimedCells);
            GetTemperatureRange(worldGen, ref temperatureMin, ref temperatureRange);
            ApplyForeground(worldGen, world, SetValues, temperatureMin, temperatureRange, rnd);
            for (var i = 0; i < node.tags.Count; i++)
                GenerateActionCells(worldGen.Settings, node.tags[i], availableTerrainPoints, rnd);

            ApplyBackground(worldGen, world, SetValues, temperatureMin, temperatureRange, rnd);
        }

        public void Process(WorldGen          worldGen,
                            Sim.Cell[]        cells,
                            float[]           bgTemp,
                            Sim.DiseaseCell[] dcs,
                            Chunk             world,
                            SeededRandom      rnd) {
            SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc) {
                                              if (Grid.IsValidCell(index)) {
                                                  if (pd.temperature == 0f ||
                                                      (elem as Element).HasTag(GameTags.Special))
                                                      bgTemp[index] = -1f;

                                                  cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
                                                  dcs[index] = dc;
                                                  return;
                                              }

                                              Debug.LogError(string.Concat("Process::SetValuesFunction Index [",
                                                                           index.ToString(),
                                                                           "] is not valid. cells.Length [",
                                                                           cells.Length.ToString(),
                                                                           "]"));
                                          };

            DoProcess(worldGen, world, setValues, rnd);
        }

        public void Process(WorldGen worldGen, Chunk world, SeededRandom rnd) {
            SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc) {
                                              SimMessages.ModifyCell(index,
                                                                     (elem as Element).idx,
                                                                     pd.temperature,
                                                                     pd.mass,
                                                                     dc.diseaseIdx,
                                                                     dc.elementCount,
                                                                     SimMessages.ReplaceType.Replace);
                                          };

            DoProcess(worldGen, world, setValues, rnd);
        }

        public int DistanceToTag(Tag tag) {
            int result;
            if (!distancesToTags.TryGetValue(tag, out result))
                DebugUtil.DevLogError(string.Format("DistanceToTag could not find tag '{0}', did forget to include a start template?",
                                                    tag));

            return result;
        }

        public bool IsSafeToSpawnPOI(List<TerrainCell> allCells, bool log = true) {
            return IsSafeToSpawnPOI(allCells, noPOISpawnTags, noPOISpawnTagSet, log);
        }

        public bool IsSafeToSpawnPOIRelaxed(List<TerrainCell> allCells, bool log = true) {
            return IsSafeToSpawnPOI(allCells, relaxedNoPOISpawnTags, relaxedNoPOISpawnTagSet, log);
        }

        public bool IsSafeToSpawnPOINearStart(List<TerrainCell> allCells, bool log = true) {
            return IsSafeToSpawnPOI(allCells, noPOISpawnTagsNearStart, noPOISpawnNearStartTagSet, log);
        }

        public bool IsSafeToSpawnPOINearStartRelaxed(List<TerrainCell> allCells, bool log = true) {
            return IsSafeToSpawnPOI(allCells,
                                    relaxedNoPOISpawnTagsAllowNearStart,
                                    relaxedNoPOISpawnAllowNearStartTagSet,
                                    log);
        }

        private bool IsSafeToSpawnPOI(List<TerrainCell> allCells, Tag[] noSpawnTags, TagSet noSpawnTagSet, bool log) {
            return !node.tags.ContainsOne(noSpawnTagSet);
        }

        public struct ElementOverride {
            public Element         element;
            public Sim.PhysicsData pdelement;
            public Sim.DiseaseCell dc;
            public float           mass;
            public float           temperature;
            public byte            diseaseIdx;
            public int             diseaseAmount;
            public bool            overrideMass;
            public bool            overrideTemperature;
            public bool            overrideDiseaseIdx;
            public bool            overrideDiseaseAmount;
        }
    }
}