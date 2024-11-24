using System;
using Database;
using UnityEngine;

// Token: 0x02001D59 RID: 7513
public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009CEF RID: 40175 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009CF0 RID: 40176 RVA: 0x001061A8 File Offset: 0x001043A8
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06009CF1 RID: 40177 RVA: 0x003C62C8 File Offset: 0x003C44C8
	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
	}

	// Token: 0x04007B03 RID: 31491
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
