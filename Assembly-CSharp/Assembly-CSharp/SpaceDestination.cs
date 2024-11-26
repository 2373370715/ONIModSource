using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), DebuggerDisplay("{id}: {type} at distance {distance}")]
public class SpaceDestination {
    private const int   MASS_TO_RECOVER_AMOUNT = 1000;
    private const float RARE_ITEM_CHANCE       = 0.33f;

    private static readonly List<Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<Tuple<float, int>> {
        new Tuple<float, int>(1f, 0), new Tuple<float, int>(0.33f, 1), new Tuple<float, int>(0.03f, 2)
    };

    private static readonly List<Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS
        = new List<Tuple<SimHashes, MathUtil.MinMax>> {
            new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
            new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium,   new MathUtil.MinMax(1f, 10f)),
            new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
            new Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin,  new MathUtil.MinMax(1f, 10f))
        };

    private static readonly List<Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<Tuple<string, MathUtil.MinMax>> {
        new Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
    };

    [Serialize]
    public float activePeriod = 20f;

    [Serialize]
    public int distance;

    [Serialize]
    public int id;

    [Serialize]
    public float inactivePeriod = 10f;

    [Serialize]
    public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

    [Serialize]
    public List<ResearchOpportunity> researchOpportunities = new List<ResearchOpportunity>();

    public bool startAnalyzed;

    [Serialize]
    public float startingOrbitPercentage;

    [Serialize]
    public string type;

    public SpaceDestination(int id, string type, int distance) {
        this.id       = id;
        this.type     = type;
        this.distance = distance;
        var destinationType = GetDestinationType();
        AvailableMass = destinationType.maxiumMass - destinationType.minimumMass;
        GenerateSurfaceElements();
        GenerateResearchOpportunities();
    }

    public int   OneBasedDistance => distance                         + 1;
    public float CurrentMass      => GetDestinationType().minimumMass + AvailableMass;

    [field: Serialize]
    public float AvailableMass { get; private set; }

    private static Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id) {
        foreach (var tuple in RARE_ELEMENTS)
            if (tuple.first == id)
                return tuple;

        return null;
    }

    [OnDeserialized]
    private void OnDeserialized() {
        if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9)) {
            var destinationType = GetDestinationType();
            AvailableMass = destinationType.maxiumMass - destinationType.minimumMass;
        }
    }

    public SpaceDestinationType GetDestinationType() { return Db.Get().SpaceDestinationTypes.Get(type); }

    public ResearchOpportunity TryCompleteResearchOpportunity() {
        foreach (var researchOpportunity in researchOpportunities)
            if (researchOpportunity.TryComplete(this))
                return researchOpportunity;

        return null;
    }

    public void GenerateSurfaceElements() {
        foreach (var keyValuePair in GetDestinationType().elementTable)
            recoverableElements.Add(keyValuePair.Key, Random.value);
    }

    public SpacecraftManager.DestinationAnalysisState AnalysisState() {
        return SpacecraftManager.instance.GetDestinationAnalysisState(this);
    }

    public void GenerateResearchOpportunities() {
        researchOpportunities.Add(new ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO,
                                                          ROCKETRY.DESTINATION_RESEARCH.BASIC));

        researchOpportunities.Add(new ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO,
                                                          ROCKETRY.DESTINATION_RESEARCH.BASIC));

        researchOpportunities.Add(new ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD,
                                                          ROCKETRY.DESTINATION_RESEARCH.BASIC));

        researchOpportunities.Add(new ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE,
                                                          ROCKETRY.DESTINATION_RESEARCH.BASIC));

        researchOpportunities.Add(new ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE,
                                                          ROCKETRY.DESTINATION_RESEARCH.BASIC));

        var num                                         = 0f;
        foreach (var tuple in RARE_ELEMENT_CHANCES) num += tuple.first;
        var num2                                        = Random.value * num;
        var num3                                        = 0;
        foreach (var tuple2 in RARE_ELEMENT_CHANCES) {
            num2 -= tuple2.first;
            if (num2 <= 0f) num3 = tuple2.second;
        }

        for (var i = 0; i < num3; i++)
            researchOpportunities[Random.Range(0, researchOpportunities.Count)].discoveredRareResource
                = RARE_ELEMENTS[Random.Range(0,   RARE_ELEMENTS.Count)].first;

        if (Random.value < 0.33f) {
            var index = Random.Range(0, researchOpportunities.Count);
            researchOpportunities[index].discoveredRareItem = RARE_ITEMS[Random.Range(0, RARE_ITEMS.Count)].first;
        }
    }

    public float GetResourceValue(SimHashes resource, float roll) {
        if (GetDestinationType().elementTable.ContainsKey(resource))
            return GetDestinationType().elementTable[resource].Lerp(roll);

        if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
            return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);

        return 0f;
    }

    public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace,
                                                                 float reservedMass,
                                                                 bool  solids  = true,
                                                                 bool  liquids = true,
                                                                 bool  gasses  = true) {
        var dictionary = new Dictionary<SimHashes, float>();
        var num        = 0f;
        foreach (var keyValuePair in recoverableElements)
            if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid  && solids)  ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas    && gasses))
                num += GetResourceValue(keyValuePair.Key, keyValuePair.Value);

        var num2 = Mathf.Min(CurrentMass + reservedMass - GetDestinationType().minimumMass, totalCargoSpace);
        foreach (var keyValuePair2 in recoverableElements)
            if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid  && solids)  ||
                (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) ||
                (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas    && gasses)) {
                var value = num2 * (GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
                dictionary.Add(keyValuePair2.Key, value);
            }

        return dictionary;
    }

    public Dictionary<Tag, int> GetRecoverableEntities() {
        var dictionary          = new Dictionary<Tag, int>();
        var recoverableEntities = GetDestinationType().recoverableEntities;
        if (recoverableEntities != null)
            foreach (var keyValuePair in recoverableEntities)
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);

        return dictionary;
    }

    public Dictionary<Tag, int> GetMissionEntityResult() { return GetRecoverableEntities(); }

    public float ReserveResources(CargoBay bay) {
        var num = 0f;
        if (bay != null) {
            var component = bay.GetComponent<Storage>();
            foreach (var keyValuePair in recoverableElements)
                if (HasElementType(bay.storageType)) {
                    num           += component.capacityKg;
                    AvailableMass =  Mathf.Max(0f, AvailableMass - component.capacityKg);
                    break;
                }
        }

        return num;
    }

    public bool HasElementType(CargoBay.CargoType type) {
        foreach (var keyValuePair in recoverableElements)
            if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid  && type == CargoBay.CargoType.Solids)  ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas    && type == CargoBay.CargoType.Gasses))
                return true;

        return false;
    }

    public void Replenish(float dt) {
        var destinationType                                         = GetDestinationType();
        if (CurrentMass < destinationType.maxiumMass) AvailableMass += destinationType.replishmentPerSim1000ms;
    }

    public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType) {
        var num       = 0f;
        var totalMass = GetTotalMass();
        foreach (var keyValuePair in recoverableElements)
            if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid &&
                 cargoType == CargoBay.CargoType.Liquids) ||
                (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
                num += GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;

        return num;
    }

    public float GetTotalMass() {
        var num                                               = 0f;
        foreach (var keyValuePair in recoverableElements) num += GetResourceValue(keyValuePair.Key, keyValuePair.Value);
        return num;
    }

    [SerializationConfig(MemberSerialization.OptIn)]
    public class ResearchOpportunity {
        [Serialize]
        public bool completed;

        [Serialize]
        public int dataValue;

        [Serialize]
        public string description;

        [Serialize]
        public string discoveredRareItem;

        [Serialize]
        public SimHashes discoveredRareResource = SimHashes.Void;

        public ResearchOpportunity(string description, int pointValue) {
            this.description = description;
            dataValue        = pointValue;
        }

        [OnDeserialized]
        private void OnDeserialized() {
            if (discoveredRareResource == 0) discoveredRareResource = SimHashes.Void;
            if (dataValue              > 50) dataValue              = 50;
        }

        public bool TryComplete(SpaceDestination destination) {
            if (!completed) {
                completed = true;
                if (discoveredRareResource != SimHashes.Void &&
                    !destination.recoverableElements.ContainsKey(discoveredRareResource))
                    destination.recoverableElements.Add(discoveredRareResource, Random.value);

                return true;
            }

            return false;
        }
    }
}