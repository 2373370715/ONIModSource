using System;
using TUNING;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class WireRefinedHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001B69 RID: 7017 RVA: 0x001AB5C4 File Offset: 0x001A97C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefinedHighWattage";
		string anim = "utilities_electric_conduct_hiwatt_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x000B1DDC File Offset: 0x000AFFDC
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x000B1DE6 File Offset: 0x000AFFE6
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x04001141 RID: 4417
	public const string ID = "WireRefinedHighWattage";
}
