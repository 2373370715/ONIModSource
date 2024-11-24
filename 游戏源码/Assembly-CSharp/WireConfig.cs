using System;
using TUNING;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public class WireConfig : BaseWireConfig
{
	// Token: 0x06001B56 RID: 6998 RVA: 0x001AB448 File Offset: 0x001A9648
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Wire";
		string anim = "utilities_electric_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000B1D47 File Offset: 0x000AFF47
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	// Token: 0x0400113C RID: 4412
	public const string ID = "Wire";
}
