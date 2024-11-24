using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class ModularLaunchpadPortGasUnloaderConfig : IBuildingConfig
{
	// Token: 0x060014B8 RID: 5304 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x000AF167 File Offset: 0x000AD367
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGasUnloader", "conduit_port_gas_unloader_kanim", ConduitType.Gas, false, 2, 3);
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x000AF17C File Offset: 0x000AD37C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, false);
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x000AF18C File Offset: 0x000AD38C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000DE5 RID: 3557
	public const string ID = "ModularLaunchpadPortGasUnloader";
}
