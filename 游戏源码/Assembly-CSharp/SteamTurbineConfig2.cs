using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020005B7 RID: 1463
public class SteamTurbineConfig2 : IBuildingConfig
{
	// Token: 0x06001A32 RID: 6706 RVA: 0x001A6228 File Offset: 0x001A4428
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SteamTurbine2";
		int width = 5;
		int height = 3;
		string anim = "steamturbine2_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = array;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 1f);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.GeneratorWattageRating = SteamTurbineConfig2.MAX_WATTAGE;
		buildingDef.GeneratorBaseCapacity = SteamTurbineConfig2.MAX_WATTAGE;
		buildingDef.Entombable = true;
		buildingDef.IsFoundation = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.OverheatTemperature = 1273.15f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000B127C File Offset: 0x000AF47C
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x000B12A3 File Offset: 0x000AF4A3
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x001A6328 File Offset: 0x001A4528
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x001A637C File Offset: 0x001A457C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = false;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
		storage.capacityKg = 10f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.showDescriptor = false;
		storage2.showInUI = false;
		storage2.storageFilters = STORAGEFILTERS.GASES;
		storage2.SetDefaultStoredItemModifiers(SteamTurbineConfig2.StoredItemModifiers);
		SteamTurbine steamTurbine = go.AddOrGet<SteamTurbine>();
		steamTurbine.srcElem = SimHashes.Steam;
		steamTurbine.destElem = SimHashes.Water;
		steamTurbine.pumpKGRate = 2f;
		steamTurbine.maxSelfHeat = 64f;
		steamTurbine.wasteHeatToTurbinePercent = 0.1f;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = storage;
		conduitDispenser.alwaysDispense = true;
		go.AddOrGet<LogicOperationalController>();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
			StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
			Extents extents = game_object.GetComponent<Building>().GetExtents();
			Extents newExtents = new Extents(extents.x, extents.y - 1, extents.width, extents.height + 1);
			payload.OverrideExtents(newExtents);
			GameComps.StructureTemperatures.SetPayload(handle, ref payload);
			Storage[] components = game_object.GetComponents<Storage>();
			game_object.GetComponent<SteamTurbine>().SetStorage(components[1], components[0]);
		};
		Tinkerable.MakePowerTinkerable(go);
		SteamTurbineConfig2.AddVisualizer(go);
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x001A6498 File Offset: 0x001A4698
	private static void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go2.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -2;
		rangeVisualizer.RangeMin.y = -2;
		rangeVisualizer.RangeMax.x = 2;
		rangeVisualizer.RangeMax.y = -2;
		rangeVisualizer.TestLineOfSight = false;
		go2.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(SteamTurbineConfig2.SteamTurbineBlockingCB);
		};
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x001A6510 File Offset: 0x001A4710
	public static bool SteamTurbineBlockingCB(int cell)
	{
		Element element = ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		return element.IsLiquid || element.IsSolid;
	}

	// Token: 0x040010AD RID: 4269
	public const string ID = "SteamTurbine2";

	// Token: 0x040010AE RID: 4270
	public static float MAX_WATTAGE = 850f;

	// Token: 0x040010AF RID: 4271
	private const int HEIGHT = 3;

	// Token: 0x040010B0 RID: 4272
	private const int WIDTH = 5;

	// Token: 0x040010B1 RID: 4273
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}
