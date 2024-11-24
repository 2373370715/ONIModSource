using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class FishTrapConfig : IBuildingConfig
{
	// Token: 0x06000987 RID: 2439 RVA: 0x001663D0 File Offset: 0x001645D0
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00166430 File Offset: 0x00164630
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00166498 File Offset: 0x00164698
	public override void DoPostConfigureComplete(GameObject go)
	{
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		def.initialLures = new Tag[]
		{
			GameTags.Creatures.FishTrapLure
		};
	}

	// Token: 0x0400072E RID: 1838
	public const string ID = "FishTrap";

	// Token: 0x0400072F RID: 1839
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
