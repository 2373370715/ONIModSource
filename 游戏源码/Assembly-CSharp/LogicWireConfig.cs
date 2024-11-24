using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F9 RID: 1017
public class LogicWireConfig : BaseLogicWireConfig
{
	// Token: 0x06001110 RID: 4368 RVA: 0x00182A10 File Offset: 0x00180C10
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWire";
		string anim = "logic_wires_kanim";
		float construction_time = 3f;
		float[] tier_TINY = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier_TINY, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x000ADA18 File Offset: 0x000ABC18
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	// Token: 0x04000B9F RID: 2975
	public const string ID = "LogicWire";
}
