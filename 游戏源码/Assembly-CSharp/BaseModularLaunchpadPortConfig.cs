using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class BaseModularLaunchpadPortConfig
{
	// Token: 0x060000B1 RID: 177 RVA: 0x00140DC4 File Offset: 0x0013EFC4
	public static BuildingDef CreateBaseLaunchpadPort(string id, string anim, ConduitType conduitType, bool isLoader, int width = 2, int height = 3)
	{
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		switch (conduitType)
		{
		case ConduitType.Gas:
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			break;
		case ConduitType.Liquid:
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			break;
		case ConduitType.Solid:
			buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
			break;
		}
		if (isLoader)
		{
			buildingDef.InputConduitType = conduitType;
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		}
		else
		{
			buildingDef.OutputConduitType = conduitType;
			buildingDef.UtilityOutputOffset = new CellOffset(1, 2);
		}
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = true;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.CanMove = false;
		return buildingDef;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00140EDC File Offset: 0x0013F0DC
	public static void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag, ConduitType conduitType, float storageSize, bool isLoader)
	{
		go.AddOrGet<LoopingSounds>();
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		component.AddTag(BaseModularLaunchpadPortConfig.LinkTag, false);
		component.AddTag(GameTags.ModularConduitPort, false);
		component.AddTag(GameTags.NotRocketInteriorBuilding, false);
		go.AddOrGetDef<ModularConduitPortController.Def>().mode = (isLoader ? ModularConduitPortController.Mode.Load : ModularConduitPortController.Mode.Unload);
		if (!isLoader)
		{
			Storage storage = go.AddComponent<Storage>();
			storage.capacityKg = storageSize;
			storage.allowSettingOnlyFetchMarkedItems = false;
			storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
			{
				Storage.StoredItemModifier.Hide,
				Storage.StoredItemModifier.Seal,
				Storage.StoredItemModifier.Insulate
			});
			if (conduitType == ConduitType.Gas)
			{
				storage.storageFilters = STORAGEFILTERS.GASES;
			}
			else if (conduitType == ConduitType.Liquid)
			{
				storage.storageFilters = STORAGEFILTERS.LIQUIDS;
			}
			else
			{
				storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
			}
			TreeFilterable treeFilterable = go.AddOrGet<TreeFilterable>();
			treeFilterable.dropIncorrectOnFilterChange = false;
			treeFilterable.autoSelectStoredOnLoad = false;
			if (conduitType == ConduitType.Solid)
			{
				SolidConduitDispenser solidConduitDispenser = go.AddOrGet<SolidConduitDispenser>();
				solidConduitDispenser.storage = storage;
				solidConduitDispenser.elementFilter = null;
			}
			else
			{
				ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
				conduitDispenser.storage = storage;
				conduitDispenser.conduitType = conduitType;
				conduitDispenser.elementFilter = null;
				conduitDispenser.alwaysDispense = true;
			}
		}
		else
		{
			Storage storage2 = go.AddComponent<Storage>();
			storage2.capacityKg = storageSize;
			storage2.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
			{
				Storage.StoredItemModifier.Hide,
				Storage.StoredItemModifier.Seal,
				Storage.StoredItemModifier.Insulate
			});
			if (conduitType == ConduitType.Solid)
			{
				SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
				solidConduitConsumer.storage = storage2;
				solidConduitConsumer.capacityTag = GameTags.Any;
				solidConduitConsumer.capacityKG = storageSize;
			}
			else
			{
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.storage = storage2;
				conduitConsumer.conduitType = conduitType;
				conduitConsumer.capacityTag = GameTags.Any;
				conduitConsumer.capacityKG = storageSize;
			}
		}
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000A5E40 File Offset: 0x000A4040
	public static void DoPostConfigureComplete(GameObject go, bool isLoader)
	{
	}

	// Token: 0x04000080 RID: 128
	public static Tag LinkTag = new Tag("ModularLaunchpadPort");
}
