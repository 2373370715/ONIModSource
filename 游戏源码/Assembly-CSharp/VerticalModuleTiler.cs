using System;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
public class VerticalModuleTiler : KMonoBehaviour
{
	// Token: 0x06001AFE RID: 6910 RVA: 0x001A9AB8 File Offset: 0x001A7CB8
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (this.manageTopCap)
		{
			this.topCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.topCapStr);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
		}
		this.PostReorderMove();
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000B19F6 File Offset: 0x000AFBF6
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000B1A0E File Offset: 0x000AFC0E
	public void PostReorderMove()
	{
		this.dirty = true;
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000B1A17 File Offset: 0x000AFC17
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

	// Token: 0x06001B02 RID: 6914 RVA: 0x001A9B2C File Offset: 0x001A7D2C
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellTop = this.GetCellTop();
		int cellBottom = this.GetCellBottom();
		if (Grid.IsValidCell(cellTop))
		{
			if (this.HasWideNeighbor(cellTop))
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (Grid.IsValidCell(cellBottom))
		{
			if (this.HasWideNeighbor(cellBottom))
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (this.manageTopCap)
		{
			this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x001A9BD0 File Offset: 0x001A7DD0
	private int GetCellTop()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x001A9C14 File Offset: 0x001A7E14
	private int GetCellBottom()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 - 1);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x001A9C50 File Offset: 0x001A7E50
	private bool HasWideNeighbor(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.GetComponent<ReorderableBuilding>() != null && component.GetComponent<Building>().Def.WidthInCells >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x001A9CB0 File Offset: 0x001A7EB0
	private void LateUpdate()
	{
		if (this.animController.Offset != this.m_previousAnimControllerOffset)
		{
			this.m_previousAnimControllerOffset = this.animController.Offset;
			this.bottomCapWide.Dirty();
			this.topCapWide.Dirty();
		}
		if (this.dirty)
		{
			if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
			{
				GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			}
			OccupyArea component = base.GetComponent<OccupyArea>();
			if (component != null)
			{
				this.extents = component.GetExtents();
			}
			Extents extents = new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
			this.UpdateEndCaps();
			this.dirty = false;
		}
	}

	// Token: 0x04001107 RID: 4359
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001108 RID: 4360
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001109 RID: 4361
	private Extents extents;

	// Token: 0x0400110A RID: 4362
	private VerticalModuleTiler.AnimCapType topCapSetting;

	// Token: 0x0400110B RID: 4363
	private VerticalModuleTiler.AnimCapType bottomCapSetting;

	// Token: 0x0400110C RID: 4364
	private bool manageTopCap = true;

	// Token: 0x0400110D RID: 4365
	private bool manageBottomCap = true;

	// Token: 0x0400110E RID: 4366
	private KAnimSynchronizedController topCapWide;

	// Token: 0x0400110F RID: 4367
	private KAnimSynchronizedController bottomCapWide;

	// Token: 0x04001110 RID: 4368
	private static readonly string topCapStr = "#cap_top_5";

	// Token: 0x04001111 RID: 4369
	private static readonly string bottomCapStr = "#cap_bottom_5";

	// Token: 0x04001112 RID: 4370
	private bool dirty;

	// Token: 0x04001113 RID: 4371
	[MyCmpGet]
	private KAnimControllerBase animController;

	// Token: 0x04001114 RID: 4372
	private Vector3 m_previousAnimControllerOffset;

	// Token: 0x020005DA RID: 1498
	private enum AnimCapType
	{
		// Token: 0x04001116 RID: 4374
		ThreeWide,
		// Token: 0x04001117 RID: 4375
		FiveWide
	}
}
