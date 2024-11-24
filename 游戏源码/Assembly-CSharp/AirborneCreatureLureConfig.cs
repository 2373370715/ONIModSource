using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class AirborneCreatureLureConfig : IBuildingConfig
{
	// Token: 0x06000076 RID: 118 RVA: 0x0013FA90 File Offset: 0x0013DC90
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AirborneCreatureLure", 1, 4, "airbornecreaturetrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Deprecated = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0013FB00 File Offset: 0x0013DD00
	public override void ConfigureBuildingTemplate(GameObject prefab, Tag prefab_tag)
	{
		CreatureLure creatureLure = prefab.AddOrGet<CreatureLure>();
		creatureLure.baitStorage = prefab.AddOrGet<Storage>();
		creatureLure.baitTypes = new List<Tag>
		{
			GameTags.SlimeMold,
			GameTags.Phosphorite
		};
		creatureLure.baitStorage.storageFilters = creatureLure.baitTypes;
		creatureLure.baitStorage.allowItemRemoval = false;
		creatureLure.baitStorage.SetDefaultStoredItemModifiers(Storage.StandardFabricatorStorage);
		prefab.AddOrGet<Operational>();
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0013FB78 File Offset: 0x0013DD78
	public override void DoPostConfigureComplete(GameObject prefab)
	{
		BuildingTemplates.DoPostConfigure(prefab);
		SymbolOverrideControllerUtil.AddToPrefab(prefab);
		prefab.AddOrGet<LogicOperationalController>();
		Lure.Def def = prefab.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(-1, 4),
			new CellOffset(0, 4),
			new CellOffset(1, 4),
			new CellOffset(-2, 3),
			new CellOffset(-1, 3),
			new CellOffset(0, 3),
			new CellOffset(1, 3),
			new CellOffset(2, 3),
			new CellOffset(-1, 2),
			new CellOffset(0, 2),
			new CellOffset(1, 2),
			new CellOffset(0, 1)
		};
		def.radius = 32;
		Prioritizable.AddRef(prefab);
	}

	// Token: 0x04000057 RID: 87
	public const string ID = "AirborneCreatureLure";
}
