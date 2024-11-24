using Database;
using UnityEngine;

public class KleiPermitDioramaVis_BuildingHangingHook : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
	}

	public void ConfigureWith(PermitResource permit)
	{
		KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, (BuildingFacadeResource)permit);
		KleiPermitVisUtil.ConfigureBuildingPosition(buildingKAnim.rectTransform(), buildingKAnimPosition, KleiPermitVisUtil.GetBuildingDef(permit), Alignment.Top());
		KleiPermitVisUtil.AnimateIn(buildingKAnim);
	}
}
