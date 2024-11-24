using ProcGen;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	[MyCmpReq]
	public Building building;

	private bool wasReplaced;

	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});

	protected override void OnSpawn()
	{
		int[] placementCells = building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		Subscribe(1606648047, OnObjectReplacedDelegate);
	}

	protected override void OnCleanUp()
	{
		if (!wasReplaced)
		{
			ClearZone();
		}
	}

	private void OnObjectReplaced(object data)
	{
		ClearZone();
		wasReplaced = true;
	}

	private void ClearZone()
	{
		int[] placementCells = building.PlacementCells;
		foreach (int num in placementCells)
		{
			if (!Grid.ObjectLayers[(int)building.Def.ObjectLayer].TryGetValue(num, out var value) || !(value != base.gameObject) || !(value != null) || !(value.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = ((subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType));
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}
}
