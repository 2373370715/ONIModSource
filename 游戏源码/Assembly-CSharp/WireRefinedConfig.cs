using System;
using TUNING;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class WireRefinedConfig : BaseWireConfig
{
	// Token: 0x06001B66 RID: 7014 RVA: 0x001AB580 File Offset: 0x001A9780
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefined";
		string anim = "utilities_electric_conduct_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.NONE, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		return buildingDef;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000B1DD2 File Offset: 0x000AFFD2
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
	}

	// Token: 0x04001140 RID: 4416
	public const string ID = "WireRefined";
}
