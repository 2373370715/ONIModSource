using System;
using TUNING;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public class LogicRibbonConfig : BaseLogicWireConfig
{
	// Token: 0x060010E8 RID: 4328 RVA: 0x001820CC File Offset: 0x001802CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbon";
		string anim = "logic_ribbon_kanim";
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		EffectorValues none = NOISE_POLLUTION.NONE;
		return base.CreateBuildingDef(id, anim, construction_time, tier, BUILDINGS.DECOR.PENALTY.TIER0, none);
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x000AD823 File Offset: 0x000ABA23
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	// Token: 0x04000B94 RID: 2964
	public const string ID = "LogicRibbon";
}
