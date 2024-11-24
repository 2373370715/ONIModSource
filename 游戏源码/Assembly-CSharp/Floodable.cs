using System;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060030E7 RID: 12519 RVA: 0x000BFC9D File Offset: 0x000BDE9D
	public bool IsFlooded
	{
		get
		{
			return this.isFlooded;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060030E8 RID: 12520 RVA: 0x000BFCA5 File Offset: 0x000BDEA5
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x001FDD30 File Offset: 0x001FBF30
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x001FDD88 File Offset: 0x001FBF88
	private void OnElementChanged(object data)
	{
		bool flag = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (Grid.IsSubstantialLiquid(this.building.PlacementCells[i], 0.35f))
			{
				flag = true;
				break;
			}
		}
		if (flag != this.isFlooded)
		{
			this.isFlooded = flag;
			this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, this);
		}
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000BFCB2 File Offset: 0x000BDEB2
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0400210B RID: 8459
	[MyCmpReq]
	private Building building;

	// Token: 0x0400210C RID: 8460
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400210D RID: 8461
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x0400210E RID: 8462
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400210F RID: 8463
	public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);

	// Token: 0x04002110 RID: 8464
	private bool isFlooded;

	// Token: 0x04002111 RID: 8465
	private HandleVector<int>.Handle partitionerEntry;
}
