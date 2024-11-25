using System;
using Database;
using UnityEngine;

public class KleiPermitDioramaVis_BuildingRocket : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
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
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, buildingPermit);
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

		[SerializeField]
	private KBatchedAnimController buildingKAnim;
}
