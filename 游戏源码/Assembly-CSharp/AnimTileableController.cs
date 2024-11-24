using System;
using UnityEngine;

// Token: 0x02000C16 RID: 3094
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour
{
	// Token: 0x06003AFB RID: 15099 RVA: 0x000C60F9 File Offset: 0x000C42F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06003AFC RID: 15100 RVA: 0x002293D4 File Offset: 0x002275D4
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		KBatchedAnimController component3 = base.GetComponent<KBatchedAnimController>();
		this.left = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.leftName);
		this.right = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.rightName);
		this.top = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.topName);
		this.bottom = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.bottomName);
		this.UpdateEndCaps();
	}

	// Token: 0x06003AFD RID: 15101 RVA: 0x000C6130 File Offset: 0x000C4330
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003AFE RID: 15102 RVA: 0x002294F4 File Offset: 0x002276F4
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool enable = true;
		bool enable2 = true;
		bool enable3 = true;
		bool enable4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			enable = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			enable2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			enable3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			enable4 = !this.HasTileableNeighbour(num6);
		}
		this.left.Enable(enable);
		this.right.Enable(enable2);
		this.top.Enable(enable3);
		this.bottom.Enable(enable4);
	}

	// Token: 0x06003AFF RID: 15103 RVA: 0x00229678 File Offset: 0x00227878
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06003B00 RID: 15104 RVA: 0x000C6148 File Offset: 0x000C4348
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x04002846 RID: 10310
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002847 RID: 10311
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04002848 RID: 10312
	public Tag[] tags;

	// Token: 0x04002849 RID: 10313
	private Extents extents;

	// Token: 0x0400284A RID: 10314
	public string leftName = "#cap_left";

	// Token: 0x0400284B RID: 10315
	public string rightName = "#cap_right";

	// Token: 0x0400284C RID: 10316
	public string topName = "#cap_top";

	// Token: 0x0400284D RID: 10317
	public string bottomName = "#cap_bottom";

	// Token: 0x0400284E RID: 10318
	private KAnimSynchronizedController left;

	// Token: 0x0400284F RID: 10319
	private KAnimSynchronizedController right;

	// Token: 0x04002850 RID: 10320
	private KAnimSynchronizedController top;

	// Token: 0x04002851 RID: 10321
	private KAnimSynchronizedController bottom;
}
