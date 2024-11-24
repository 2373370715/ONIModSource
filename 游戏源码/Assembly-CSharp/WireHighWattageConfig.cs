using System;
using TUNING;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class WireHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001B59 RID: 7001 RVA: 0x001AB480 File Offset: 0x001A9680
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighWattageWire";
		string anim = "utilities_electric_insulated_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER5, none);
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000B1D59 File Offset: 0x000AFF59
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000B1D63 File Offset: 0x000AFF63
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x0400113D RID: 4413
	public const string ID = "HighWattageWire";
}
