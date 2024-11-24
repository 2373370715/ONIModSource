using Database;
using UnityEngine;

public class KleiPermitDioramaVis_ArtableSculpture : KMonoBehaviour, IKleiPermitDioramaVisTarget
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
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, artablePermit);
		KleiPermitVisUtil.AnimateIn(buildingKAnim);
	}
}
