using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class ModularLaunchpadPortLiquidUnloaderConfig : IBuildingConfig
{
	// Token: 0x060014C2 RID: 5314 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x000AF1BA File Offset: 0x000AD3BA
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortLiquidUnloader", "conduit_port_liquid_unloader_kanim", ConduitType.Liquid, false, 2, 3);
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x000AF1CF File Offset: 0x000AD3CF
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Liquid, 10f, false);
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x000AF18C File Offset: 0x000AD38C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000DE7 RID: 3559
	public const string ID = "ModularLaunchpadPortLiquidUnloader";
}
