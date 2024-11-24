using System;
using Database;
using UnityEngine;

// Token: 0x02001D5D RID: 7517
public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009CFF RID: 40191 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D00 RID: 40192 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009D01 RID: 40193 RVA: 0x003C64D4 File Offset: 0x003C46D4
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007B09 RID: 31497
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
