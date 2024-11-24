using Database;
using UnityEngine;

public class KleiPermitDioramaVis_BuildingOnBackground : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private KBatchedAnimController buildingKAnimPrefab;

	private KBatchedAnimController[] buildingKAnimArray;

	public void ConfigureSetup()
	{
		buildingKAnimPrefab.gameObject.SetActive(value: false);
		buildingKAnimArray = new KBatchedAnimController[9];
		for (int i = 0; i < buildingKAnimArray.Length; i++)
		{
			buildingKAnimArray[i] = (KBatchedAnimController)Object.Instantiate((Object)buildingKAnimPrefab, buildingKAnimPrefab.transform.parent, instantiateInWorldSpace: false);
		}
		Vector2 anchoredPosition = buildingKAnimPrefab.rectTransform().anchoredPosition;
		Vector2 vector = 175f * Vector2.one;
		Vector2 vector2 = anchoredPosition + vector * new Vector2(-1f, 0f);
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			int num2 = 0;
			while (num2 < 3)
			{
				buildingKAnimArray[num].rectTransform().anchoredPosition = vector2 + vector * new Vector2(j, num2);
				buildingKAnimArray[num].gameObject.SetActive(value: true);
				num2++;
				num++;
			}
		}
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureWith(PermitResource permit)
	{
		BuildingFacadeResource buildingPermit = (BuildingFacadeResource)permit;
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		DebugUtil.DevAssert(buildingDef.WidthInCells == 1, "assert failed");
		DebugUtil.DevAssert(buildingDef.HeightInCells == 1, "assert failed");
		KBatchedAnimController[] array = buildingKAnimArray;
		for (int i = 0; i < array.Length; i++)
		{
			KleiPermitVisUtil.ConfigureToRenderBuilding(array[i], buildingPermit);
		}
	}
}
