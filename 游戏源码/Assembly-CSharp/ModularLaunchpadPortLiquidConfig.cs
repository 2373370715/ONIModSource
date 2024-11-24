using System;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class ModularLaunchpadPortLiquidConfig : IBuildingConfig
{
	// Token: 0x060014BD RID: 5309 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000AF195 File Offset: 0x000AD395
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquid", "conduit_port_liquid_loader_kanim", ConduitType.Liquid, true, 2, 2);
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x000AF1AA File Offset: 0x000AD3AA
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, true);
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000AF15E File Offset: 0x000AD35E
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000DE6 RID: 3558
	public const string ID = "ModularLaunchpadPortLiquid";
}
