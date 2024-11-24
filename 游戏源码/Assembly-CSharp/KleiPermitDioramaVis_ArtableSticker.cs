using Database;
using UnityEngine;

public class KleiPermitDioramaVis_ArtableSticker : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
		SymbolOverrideControllerUtil.AddToPrefab(buildingKAnim.gameObject);
	}

	public void ConfigureWith(PermitResource permit)
	{
		DbStickerBomb artablePermit = (DbStickerBomb)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, artablePermit);
	}
}
