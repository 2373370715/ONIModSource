using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
	[MyCmpReq]
	private Building building;

	[MyCmpReq]
	private PrimaryElement primaryElement;

	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	[MyCmpReq]
	private Operational operational;

	public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);

	private bool isSubmerged;

	private HandleVector<int>.Handle partitionerEntry;

	public bool IsSubmerged => isSubmerged;

	public BuildingDef Def => building.Def;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", base.gameObject, building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, OnElementChanged);
		OnElementChanged(null);
		operational.SetFlag(notSubmergedFlag, isSubmerged);
		GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !isSubmerged, this);
	}

	private void OnElementChanged(object data)
	{
		bool flag = true;
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			if (!Grid.IsLiquid(building.PlacementCells[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag != isSubmerged)
		{
			isSubmerged = flag;
			operational.SetFlag(notSubmergedFlag, isSubmerged);
			GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !isSubmerged, this);
		}
	}

	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
	}
}
