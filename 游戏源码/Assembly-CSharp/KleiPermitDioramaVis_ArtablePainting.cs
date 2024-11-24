using Database;
using UnityEngine;

public class KleiPermitDioramaVis_ArtablePainting : KMonoBehaviour, IKleiPermitDioramaVisTarget
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
		SymbolOverrideControllerUtil.AddToPrefab(buildingKAnim.gameObject);
	}

	public void ConfigureWith(PermitResource permit)
	{
		ArtableStage artablePermit = (ArtableStage)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(buildingKAnim, artablePermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		buildingKAnimPosition.SetOn(buildingKAnim);
		buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f * (float)buildingDef.HeightInCells / 2f + 176f);
		buildingKAnim.rectTransform().localScale = Vector3.one * 0.9f;
		KleiPermitVisUtil.AnimateIn(buildingKAnim);
	}
}
