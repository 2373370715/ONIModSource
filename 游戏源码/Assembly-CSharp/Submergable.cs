using System;
using UnityEngine;

// Token: 0x02000B28 RID: 2856
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06003651 RID: 13905 RVA: 0x000C358B File Offset: 0x000C178B
	public bool IsSubmerged
	{
		get
		{
			return this.isSubmerged;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06003652 RID: 13906 RVA: 0x000C3593 File Offset: 0x000C1793
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00213388 File Offset: 0x00211588
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
		this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x0021341C File Offset: 0x0021161C
	private void OnElementChanged(object data)
	{
		bool flag = true;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (!Grid.IsLiquid(this.building.PlacementCells[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag != this.isSubmerged)
		{
			this.isSubmerged = flag;
			this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
		}
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x000C35A0 File Offset: 0x000C17A0
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040024F8 RID: 9464
	[MyCmpReq]
	private Building building;

	// Token: 0x040024F9 RID: 9465
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040024FA RID: 9466
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x040024FB RID: 9467
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040024FC RID: 9468
	public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);

	// Token: 0x040024FD RID: 9469
	private bool isSubmerged;

	// Token: 0x040024FE RID: 9470
	private HandleVector<int>.Handle partitionerEntry;
}
