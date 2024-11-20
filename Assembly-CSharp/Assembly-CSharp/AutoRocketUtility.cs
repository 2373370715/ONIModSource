using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoRocketUtility
{
	public static void StartAutoRocket(LaunchPad selectedPad)
	{
		selectedPad.StartCoroutine(AutoRocketUtility.AutoRocketRoutine(selectedPad));
	}

	private static IEnumerator AutoRocketRoutine(LaunchPad selectedPad)
	{
		GameObject baseModule = AutoRocketUtility.AddEngine(selectedPad);
		GameObject oxidizerTank = AutoRocketUtility.AddOxidizerTank(baseModule);
		yield return SequenceUtil.WaitForEndOfFrame;
		AutoRocketUtility.AddOxidizer(oxidizerTank);
		GameObject gameObject = AutoRocketUtility.AddPassengerModule(oxidizerTank);
		AutoRocketUtility.AddDrillCone(AutoRocketUtility.AddSolidStorageModule(gameObject));
		PassengerRocketModule passengerModule = gameObject.GetComponent<PassengerRocketModule>();
		ClustercraftExteriorDoor exteriorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>();
		int max = 100;
		while (exteriorDoor.GetInteriorDoor() == null && max > 0)
		{
			int num = max;
			max = num - 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		WorldContainer interiorWorld = passengerModule.GetComponent<RocketModuleCluster>().CraftInterface.GetInteriorWorld();
		RocketControlStation station = Components.RocketControlStations.GetWorldItems(interiorWorld.id, false)[0];
		GameObject minion = AutoRocketUtility.AddPilot(station);
		AutoRocketUtility.AddOxygen(station);
		yield return SequenceUtil.WaitForEndOfFrame;
		AutoRocketUtility.AssignCrew(minion, passengerModule);
		yield break;
	}

	private static GameObject AddEngine(LaunchPad selectedPad)
	{
		BuildingDef buildingDef = Assets.GetBuildingDef("KeroseneEngineClusterSmall");
		List<Tag> elements = new List<Tag>
		{
			SimHashes.Steel.CreateTag()
		};
		GameObject gameObject = selectedPad.AddBaseModule(buildingDef, elements);
		Element element = ElementLoader.GetElement(gameObject.GetComponent<RocketEngineCluster>().fuelTag);
		Storage component = gameObject.GetComponent<Storage>();
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
			return gameObject;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
			return gameObject;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, component.Capacity(), element.defaultValues.temperature, byte.MaxValue, 0, false, true);
		}
		return gameObject;
	}

	private static GameObject AddPassengerModule(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("HabitatModuleMedium");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	private static GameObject AddSolidStorageModule(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("SolidCargoBaySmall");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Steel.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	private static GameObject AddDrillCone(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("NoseconeHarvest");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Steel.CreateTag(),
			SimHashes.Polypropylene.CreateTag()
		};
		GameObject gameObject = component.AddModule(buildingDef, buildMaterials);
		gameObject.GetComponent<Storage>().AddOre(SimHashes.Diamond, 1000f, 273f, byte.MaxValue, 0, false, true);
		return gameObject;
	}

	private static GameObject AddOxidizerTank(GameObject baseModule)
	{
		ReorderableBuilding component = baseModule.GetComponent<ReorderableBuilding>();
		BuildingDef buildingDef = Assets.GetBuildingDef("SmallOxidizerTank");
		List<Tag> buildMaterials = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return component.AddModule(buildingDef, buildMaterials);
	}

	private static void AddOxidizer(GameObject oxidizerTank)
	{
		SimHashes simHashes = SimHashes.OxyRock;
		Element element = ElementLoader.FindElementByHash(simHashes);
		DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
		oxidizerTank.GetComponent<OxidizerTank>().DEBUG_FillTank(simHashes);
	}

	private static GameObject AddPilot(RocketControlStation station)
	{
		Vector3 position = station.transform.position;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID), null, null);
		gameObject.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position2);
		gameObject.SetActive(true);
		new MinionStartingStats(false, null, null, true).Apply(gameObject);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		if (DebugHandler.InstantBuildMode && component.AvailableSkillpoints < 1)
		{
			component.ForceAddSkillPoint();
		}
		string id = Db.Get().Skills.RocketPiloting1.Id;
		MinionResume.SkillMasteryConditions[] skillMasteryConditions = component.GetSkillMasteryConditions(id);
		bool flag = component.CanMasterSkill(skillMasteryConditions);
		if (component != null && !component.HasMasteredSkill(id) && flag)
		{
			component.MasterSkill(id);
		}
		return gameObject;
	}

	private static void AddOxygen(RocketControlStation station)
	{
		SimMessages.ReplaceElement(Grid.PosToCell(station.transform.position + Vector3.up * 2f), SimHashes.OxyRock, CellEventLogger.Instance.DebugTool, 1000f, 273f, byte.MaxValue, 0, -1);
	}

	private static void AssignCrew(GameObject minion, PassengerRocketModule passengerModule)
	{
		for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
		{
			if (Components.MinionAssignablesProxy[i].GetTargetGameObject() == minion)
			{
				passengerModule.GetComponent<AssignmentGroupController>().SetMember(Components.MinionAssignablesProxy[i], true);
				break;
			}
		}
		passengerModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
	}

	private static void SetDestination(CraftModuleInterface craftModuleInterface, PassengerRocketModule passengerModule)
	{
		craftModuleInterface.GetComponent<ClusterDestinationSelector>().SetDestination(passengerModule.GetMyWorldLocation() + AxialI.NORTHEAST);
	}
}
