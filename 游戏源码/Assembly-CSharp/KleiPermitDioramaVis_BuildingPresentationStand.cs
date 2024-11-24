using System;
using Database;
using UnityEngine;

// Token: 0x02001D5E RID: 7518
public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D03 RID: 40195 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D04 RID: 40196 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009D05 RID: 40197 RVA: 0x003C6508 File Offset: 0x003C4708
	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.ConfigureBuildingPosition(this.buildingKAnim.rectTransform(), this.anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), this.lastAlignment);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	// Token: 0x06009D06 RID: 40198 RVA: 0x003C6560 File Offset: 0x003C4760
	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		this.lastAlignment = alignment;
		this.anchorPos = new Vector2(alignment.x.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-160f, 160f)), alignment.y.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(-156f, 156f)));
		return this;
	}

	// Token: 0x04007B0A RID: 31498
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	// Token: 0x04007B0B RID: 31499
	private Alignment lastAlignment;

	// Token: 0x04007B0C RID: 31500
	private Vector2 anchorPos;

	// Token: 0x04007B0D RID: 31501
	public const float LEFT = -160f;

	// Token: 0x04007B0E RID: 31502
	public const float TOP = 156f;
}
