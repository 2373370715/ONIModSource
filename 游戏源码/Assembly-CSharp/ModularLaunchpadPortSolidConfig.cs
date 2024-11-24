using System;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class ModularLaunchpadPortSolidConfig : IBuildingConfig
{
	// Token: 0x060014C7 RID: 5319 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x000AF1DF File Offset: 0x000AD3DF
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolid", "conduit_port_solid_loader_kanim", ConduitType.Solid, true, 2, 2);
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x000AF1F4 File Offset: 0x000AD3F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, true);
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x000AF15E File Offset: 0x000AD35E
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, true);
	}

	// Token: 0x04000DE8 RID: 3560
	public const string ID = "ModularLaunchpadPortSolid";
}
