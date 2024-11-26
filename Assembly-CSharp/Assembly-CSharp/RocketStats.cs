using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class RocketStats {
    public static Dictionary<Tag, float> oxidizerEfficiencies = new Dictionary<Tag, float> {
        { SimHashes.OxyRock.CreateTag(), ROCKETRY.OXIDIZER_EFFICIENCY.LOW },
        { SimHashes.LiquidOxygen.CreateTag(), ROCKETRY.OXIDIZER_EFFICIENCY.HIGH }
    };

    private readonly CommandModule commandModule;
    public RocketStats(CommandModule commandModule) { this.commandModule = commandModule; }

    public float GetRocketMaxDistance() {
        var totalMass   = GetTotalMass();
        var totalThrust = GetTotalThrust();
        var num         = ROCKETRY.CalculateMassWithPenalty(totalMass);
        return Mathf.Max(0f, totalThrust - num);
    }

    // 获取质量
    public float GetTotalMass() { return GetDryMass() + GetWetMass(); }

    public float GetDryMass() {
        var num = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component              = gameObject.GetComponent<RocketModule>();
            if (component != null) num += component.GetComponent<PrimaryElement>().Mass;
        }

        return num;
    }

    public float GetWetMass() {
        var num = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component = gameObject.GetComponent<RocketModule>();
            if (component != null) {
                var component2              = component.GetComponent<FuelTank>();
                var component3              = component.GetComponent<OxidizerTank>();
                var component4              = component.GetComponent<SolidBooster>();
                if (component2 != null) num += component2.storage.MassStored();
                if (component3 != null) num += component3.storage.MassStored();
                if (component4 != null) num += component4.fuelStorage.MassStored();
            }
        }

        return num;
    }

    public Tag GetEngineFuelTag() {
        var mainEngine = GetMainEngine();
        if (mainEngine != null) return mainEngine.fuelTag;

        return null;
    }

    public float GetTotalFuel(bool includeBoosters = false) {
        var num = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component              = gameObject.GetComponent<FuelTank>();
            var engineFuelTag          = GetEngineFuelTag();
            if (component != null) num += component.storage.GetAmountAvailable(engineFuelTag);
            if (includeBoosters) {
                var component2              = gameObject.GetComponent<SolidBooster>();
                if (component2 != null) num += component2.fuelStorage.GetAmountAvailable(component2.fuelTag);
            }
        }

        return num;
    }

    public float GetTotalOxidizer(bool includeBoosters = false) {
        var num = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component              = gameObject.GetComponent<OxidizerTank>();
            if (component != null) num += component.GetTotalOxidizerAvailable();
            if (includeBoosters) {
                var component2              = gameObject.GetComponent<SolidBooster>();
                if (component2 != null) num += component2.fuelStorage.GetAmountAvailable(GameTags.OxyRock);
            }
        }

        return num;
    }

    public float GetAverageOxidizerEfficiency() {
        var dictionary = new Dictionary<Tag, float>();
        dictionary[SimHashes.LiquidOxygen.CreateTag()] = 0f;
        dictionary[SimHashes.OxyRock.CreateTag()]      = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component = gameObject.GetComponent<OxidizerTank>();
            if (component != null)
                foreach (var keyValuePair in component.GetOxidizersAvailable())
                    if (dictionary.ContainsKey(keyValuePair.Key)) {
                        var dictionary2 = dictionary;
                        var key         = keyValuePair.Key;
                        dictionary2[key] += keyValuePair.Value;
                    }
        }

        var num  = 0f;
        var num2 = 0f;
        foreach (var keyValuePair2 in dictionary) {
            num  += keyValuePair2.Value * oxidizerEfficiencies[keyValuePair2.Key];
            num2 += keyValuePair2.Value;
        }

        if (num2 == 0f) return 0f;

        return num / num2 * 100f;
    }

    public float GetTotalThrust() {
        var totalFuel                 = GetTotalFuel();
        var totalOxidizer             = GetTotalOxidizer();
        var averageOxidizerEfficiency = GetAverageOxidizerEfficiency();
        var mainEngine                = GetMainEngine();
        if (mainEngine == null) return 0f;

        return (mainEngine.requireOxidizer
                    ? Mathf.Min(totalFuel, totalOxidizer) * (mainEngine.efficiency * (averageOxidizerEfficiency / 100f))
                    : totalFuel                           * mainEngine.efficiency) +
               GetBoosterThrust();
    }

    public float GetBoosterThrust() {
        var num = 0f;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            var component = gameObject.GetComponent<SolidBooster>();
            if (component != null) {
                var amountAvailable
                    = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);

                var amountAvailable2
                    = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);

                num += component.efficiency * Mathf.Min(amountAvailable, amountAvailable2);
            }
        }

        return num;
    }

    public float GetEngineEfficiency() {
        var mainEngine = GetMainEngine();
        if (mainEngine != null) return mainEngine.efficiency;

        return 0f;
    }

    public RocketEngine GetMainEngine() {
        RocketEngine rocketEngine = null;
        foreach (var gameObject in
                 AttachableBuilding.GetAttachedNetwork(commandModule.GetComponent<AttachableBuilding>())) {
            rocketEngine = gameObject.GetComponent<RocketEngine>();
            if (rocketEngine != null && rocketEngine.mainEngine) break;
        }

        return rocketEngine;
    }

    public float GetTotalOxidizableFuel() {
        var totalFuel     = GetTotalFuel();
        var totalOxidizer = GetTotalOxidizer();
        return Mathf.Min(totalFuel, totalOxidizer);
    }
}