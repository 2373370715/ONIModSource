using System;
using Database;
using UnityEngine;

// Token: 0x02001D58 RID: 7512
public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009CEB RID: 40171 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009CEC RID: 40172 RVA: 0x00106195 File Offset: 0x00104395
	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(this.buildingKAnim.gameObject);
	}

	// Token: 0x06009CED RID: 40173 RVA: 0x003C6294 File Offset: 0x003C4494
	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, artablePermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x04007B02 RID: 31490
	[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
