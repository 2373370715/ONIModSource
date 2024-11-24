using System;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class ModularLaunchpadPortSolidUnloaderConfig : IBuildingConfig
{
	// Token: 0x060014CC RID: 5324 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x000AF204 File Offset: 0x000AD404
	public override BuildingDef CreateBuildingDef()
	{
		return BaseModularLaunchpadPortConfig.CreateBaseLaunchpadPort("ModularLaunchpadPortSolidUnloader", "conduit_port_solid_unloader_kanim", ConduitType.Solid, false, 2, 3);
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000AF219 File Offset: 0x000AD419
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BaseModularLaunchpadPortConfig.ConfigureBuildingTemplate(go, prefab_tag, ConduitType.Solid, 20f, false);
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x000AF18C File Offset: 0x000AD38C
	public override void DoPostConfigureComplete(GameObject go)
	{
		BaseModularLaunchpadPortConfig.DoPostConfigureComplete(go, false);
	}

	// Token: 0x04000DE9 RID: 3561
	public const string ID = "ModularLaunchpadPortSolidUnloader";
}
