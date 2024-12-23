﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoRocketUtility {
    public static void StartAutoRocket(LaunchPad selectedPad) {
        selectedPad.StartCoroutine(AutoRocketRoutine(selectedPad));
    }

    private static IEnumerator AutoRocketRoutine(LaunchPad selectedPad) {
        var baseModule   = AddEngine(selectedPad);
        var oxidizerTank = AddOxidizerTank(baseModule);
        yield return SequenceUtil.WaitForEndOfFrame;

        AddOxidizer(oxidizerTank);
        var gameObject = AddPassengerModule(oxidizerTank);
        AddDrillCone(AddSolidStorageModule(gameObject));
        var passengerModule = gameObject.GetComponent<PassengerRocketModule>();
        var exteriorDoor    = passengerModule.GetComponent<ClustercraftExteriorDoor>();
        var max             = 100;
        while (exteriorDoor.GetInteriorDoor() == null && max > 0) {
            var num = max;
            max = num - 1;
            yield return SequenceUtil.WaitForEndOfFrame;
        }

        var interiorWorld = passengerModule.GetComponent<RocketModuleCluster>().CraftInterface.GetInteriorWorld();
        var station       = Components.RocketControlStations.GetWorldItems(interiorWorld.id)[0];
        var minion        = AddPilot(station);
        AddOxygen(station);
        yield return SequenceUtil.WaitForEndOfFrame;

        AssignCrew(minion, passengerModule);
    }

    private static GameObject AddEngine(LaunchPad selectedPad) {
        var buildingDef = Assets.GetBuildingDef("KeroseneEngineClusterSmall");
        var elements    = new List<Tag> { SimHashes.Steel.CreateTag() };
        var gameObject  = selectedPad.AddBaseModule(buildingDef, elements);
        var element     = ElementLoader.GetElement(gameObject.GetComponent<RocketEngineCluster>().fuelTag);
        var component   = gameObject.GetComponent<Storage>();
        if (element.IsGas) {
            component.AddGasChunk(element.id,
                                  component.Capacity(),
                                  element.defaultValues.temperature,
                                  byte.MaxValue,
                                  0,
                                  false);

            return gameObject;
        }

        if (element.IsLiquid) {
            component.AddLiquid(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0);
            return gameObject;
        }

        if (element.IsSolid)
            component.AddOre(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0);

        return gameObject;
    }

    private static GameObject AddPassengerModule(GameObject baseModule) {
        var component      = baseModule.GetComponent<ReorderableBuilding>();
        var buildingDef    = Assets.GetBuildingDef("HabitatModuleMedium");
        var buildMaterials = new List<Tag> { SimHashes.Cuprite.CreateTag() };
        return component.AddModule(buildingDef, buildMaterials);
    }

    private static GameObject AddSolidStorageModule(GameObject baseModule) {
        var component      = baseModule.GetComponent<ReorderableBuilding>();
        var buildingDef    = Assets.GetBuildingDef("SolidCargoBaySmall");
        var buildMaterials = new List<Tag> { SimHashes.Steel.CreateTag() };
        return component.AddModule(buildingDef, buildMaterials);
    }

    private static GameObject AddDrillCone(GameObject baseModule) {
        var component      = baseModule.GetComponent<ReorderableBuilding>();
        var buildingDef    = Assets.GetBuildingDef("NoseconeHarvest");
        var buildMaterials = new List<Tag> { SimHashes.Steel.CreateTag(), SimHashes.Polypropylene.CreateTag() };
        var gameObject     = component.AddModule(buildingDef, buildMaterials);
        gameObject.GetComponent<Storage>().AddOre(SimHashes.Diamond, 1000f, 273f, byte.MaxValue, 0);
        return gameObject;
    }

    private static GameObject AddOxidizerTank(GameObject baseModule) {
        var component      = baseModule.GetComponent<ReorderableBuilding>();
        var buildingDef    = Assets.GetBuildingDef("SmallOxidizerTank");
        var buildMaterials = new List<Tag> { SimHashes.Cuprite.CreateTag() };
        return component.AddModule(buildingDef, buildMaterials);
    }

    private static void AddOxidizer(GameObject oxidizerTank) {
        var simHashes = SimHashes.OxyRock;
        var element   = ElementLoader.FindElementByHash(simHashes);
        DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
        oxidizerTank.GetComponent<OxidizerTank>().DEBUG_FillTank(simHashes);
    }

    private static GameObject AddPilot(RocketControlStation station) {
        var minionStartingStats = new MinionStartingStats(false, null, null, true);
        var position = station.transform.position;
        var prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
        var gameObject = Util.KInstantiate(prefab);
        gameObject.name = prefab.name;
        Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
        var position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Move);
        gameObject.transform.SetLocalPosition(position2);
        gameObject.SetActive(true);
        minionStartingStats.Apply(gameObject);
        var component = gameObject.GetComponent<MinionResume>();
        if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1) component.ForceAddSkillPoint();
        var id                     = Db.Get().Skills.RocketPiloting1.Id;
        var skillMasteryConditions = component.GetSkillMasteryConditions(id);
        var flag                   = component.CanMasterSkill(skillMasteryConditions);
        if (component != null && !component.HasMasteredSkill(id) && flag) component.MasterSkill(id);
        return gameObject;
    }

    private static void AddOxygen(RocketControlStation station) {
        SimMessages.ReplaceElement(Grid.PosToCell(station.transform.position + Vector3.up * 2f),
                                   SimHashes.OxyRock,
                                   CellEventLogger.Instance.DebugTool,
                                   1000f,
                                   273f);
    }

    private static void AssignCrew(GameObject minion, PassengerRocketModule passengerModule) {
        for (var i = 0; i < Components.MinionAssignablesProxy.Count; i++)
            if (Components.MinionAssignablesProxy[i].GetTargetGameObject() == minion) {
                passengerModule.GetComponent<AssignmentGroupController>()
                               .SetMember(Components.MinionAssignablesProxy[i], true);

                break;
            }

        passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
    }

    private static void
        SetDestination(CraftModuleInterface craftModuleInterface, PassengerRocketModule passengerModule) {
        craftModuleInterface.GetComponent<ClusterDestinationSelector>()
                            .SetDestination(passengerModule.GetMyWorldLocation() + AxialI.NORTHEAST);
    }
}