using System;
using ProcGen;
using UnityEngine;

// Token: 0x02001A61 RID: 6753
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	// Token: 0x06008D42 RID: 36162 RVA: 0x00366FE4 File Offset: 0x003651E4
	protected override void OnSpawn()
	{
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		base.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
	}

	// Token: 0x06008D43 RID: 36163 RVA: 0x000FC432 File Offset: 0x000FA632
	protected override void OnCleanUp()
	{
		if (!this.wasReplaced)
		{
			this.ClearZone();
		}
	}

	// Token: 0x06008D44 RID: 36164 RVA: 0x000FC442 File Offset: 0x000FA642
	private void OnObjectReplaced(object data)
	{
		this.ClearZone();
		this.wasReplaced = true;
	}

	// Token: 0x06008D45 RID: 36165 RVA: 0x00367028 File Offset: 0x00365228
	private void ClearZone()
	{
		foreach (int num in this.building.PlacementCells)
		{
			GameObject gameObject;
			if (!Grid.ObjectLayers[(int)this.building.Def.ObjectLayer].TryGetValue(num, out gameObject) || !(gameObject != base.gameObject) || !(gameObject != null) || !(gameObject.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = global::World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = (subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType);
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}

	// Token: 0x04006A22 RID: 27170
	[MyCmpReq]
	public Building building;

	// Token: 0x04006A23 RID: 27171
	private bool wasReplaced;

	// Token: 0x04006A24 RID: 27172
	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});
}
