using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class ModularLaunchpadPortGasConfig : IBuildingConfig
{
	// Token: 0x060014B3 RID: 5299 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x000AF139 File Offset: 0x000AD339
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortGas", "conduit_port_gas_loader_kanim", ConduitType.Gas, true, 2, 2);
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000AF14E File Offset: 0x000AD34E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Gas, 1f, true);
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x000AF15E File Offset: 0x000AD35E
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000DE4 RID: 3556
	public const string ID = "ModularLaunchpadPortGas";
}
