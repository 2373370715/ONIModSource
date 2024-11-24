using Database;
using UnityEngine;

public class KleiPermitDioramaVis_BuildingOnFloorBig : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
	}

	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(buildingKAnim);
	}
}
