using System;
using Database;
using UnityEngine;

// Token: 0x02001D5F RID: 7519
public class KleiPermitDioramaVis_BuildingRocket : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D08 RID: 40200 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D09 RID: 40201 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009D0A RID: 40202 RVA: 0x003C65D4 File Offset: 0x003C47D4
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007B0F RID: 31503
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
