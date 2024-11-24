using Database;
using UnityEngine;

public class KleiPermitDioramaVis_BuildingPresentationStand : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	private Alignment lastAlignment;

	private Vector2 anchorPos;

	public const float LEFT = -160f;

	public const float TOP = 156f;

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
		KleiPermitVisUtil.ConfigureBuildingPosition(buildingKAnim.rectTransform(), anchorPos, KleiPermitVisUtil.GetBuildingDef(permit), lastAlignment);
		KleiPermitVisUtil.AnimateIn(buildingKAnim);
	}

	public KleiPermitDioramaVis_BuildingPresentationStand WithAlignment(Alignment alignment)
	{
		lastAlignment = alignment;
		anchorPos = new Vector2(alignment.x.Remap((min: 0f, max: 1f), (min: -160f, max: 160f)), alignment.y.Remap((min: 0f, max: 1f), (min: -156f, max: 156f)));
		return this;
	}
}
