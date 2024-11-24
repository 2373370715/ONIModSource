using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02001675 RID: 5749
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour
{
	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x060076B9 RID: 30393 RVA: 0x000EE03C File Offset: 0x000EC23C
	public CellOffset[] OccupiedCellsOffsets
	{
		get
		{
			this.UpdateRotatedCells();
			return this._RotatedOccupiedCellsOffsets;
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x060076BA RID: 30394 RVA: 0x000EE04A File Offset: 0x000EC24A
	// (set) Token: 0x060076BB RID: 30395 RVA: 0x000EE052 File Offset: 0x000EC252
	public bool ApplyToCells
	{
		get
		{
			return this.applyToCells;
		}
		set
		{
			if (value != this.applyToCells)
			{
				if (value)
				{
					this.UpdateOccupiedArea();
				}
				else
				{
					this.ClearOccupiedArea();
				}
				this.applyToCells = value;
			}
		}
	}

	// Token: 0x060076BC RID: 30396 RVA: 0x000EE075 File Offset: 0x000EC275
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.applyToCells)
		{
			this.UpdateOccupiedArea();
		}
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x000EE08B File Offset: 0x000EC28B
	private void ValidatePosition()
	{
		if (!Grid.IsValidCell(Grid.PosToCell(this)))
		{
			global::Debug.LogWarning(base.name + " is outside the grid! DELETING!");
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x060076BE RID: 30398 RVA: 0x000EE0BA File Offset: 0x000EC2BA
	[OnSerializing]
	private void OnSerializing()
	{
		this.ValidatePosition();
	}

	// Token: 0x060076BF RID: 30399 RVA: 0x000EE0BA File Offset: 0x000EC2BA
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ValidatePosition();
	}

	// Token: 0x060076C0 RID: 30400 RVA: 0x0030A190 File Offset: 0x00308390
	public int GetOffsetCellWithRotation(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x060076C1 RID: 30401 RVA: 0x000EE0C2 File Offset: 0x000EC2C2
	public void SetCellOffsets(CellOffset[] cells)
	{
		this._UnrotatedOccupiedCellsOffsets = cells;
		this._RotatedOccupiedCellsOffsets = cells;
		this.UpdateRotatedCells();
	}

	// Token: 0x060076C2 RID: 30402 RVA: 0x0030A1CC File Offset: 0x003083CC
	private void UpdateRotatedCells()
	{
		if (this.rotatable != null && this.appliedOrientation != this.rotatable.Orientation)
		{
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int i = 0; i < this._UnrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset offset = this._UnrotatedOccupiedCellsOffsets[i];
				this._RotatedOccupiedCellsOffsets[i] = this.rotatable.GetRotatedCellOffset(offset);
			}
			this.appliedOrientation = this.rotatable.Orientation;
		}
	}

	// Token: 0x060076C3 RID: 30403 RVA: 0x0030A258 File Offset: 0x00308458
	public bool CheckIsOccupying(int checkCell)
	{
		int num = Grid.PosToCell(base.gameObject);
		if (checkCell == num)
		{
			return true;
		}
		foreach (CellOffset offset in this.OccupiedCellsOffsets)
		{
			if (Grid.OffsetCell(num, offset) == checkCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060076C4 RID: 30404 RVA: 0x000EE0D8 File Offset: 0x000EC2D8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearOccupiedArea();
	}

	// Token: 0x060076C5 RID: 30405 RVA: 0x0030A2A4 File Offset: 0x003084A4
	private void ClearOccupiedArea()
	{
		if (this.occupiedGridCells == null)
		{
			return;
		}
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				foreach (int cell in this.occupiedGridCells)
				{
					if (Grid.Objects[cell, (int)objectLayer] == base.gameObject)
					{
						Grid.Objects[cell, (int)objectLayer] = null;
					}
				}
			}
		}
	}

	// Token: 0x060076C6 RID: 30406 RVA: 0x0030A320 File Offset: 0x00308520
	public void UpdateOccupiedArea()
	{
		if (this.objectLayers.Length == 0)
		{
			return;
		}
		if (this.occupiedGridCells == null)
		{
			this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
		}
		this.ClearOccupiedArea();
		int cell = Grid.PosToCell(base.gameObject);
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				for (int j = 0; j < this.OccupiedCellsOffsets.Length; j++)
				{
					CellOffset offset = this.OccupiedCellsOffsets[j];
					int num = Grid.OffsetCell(cell, offset);
					Grid.Objects[num, (int)objectLayer] = base.gameObject;
					this.occupiedGridCells[j] = num;
				}
			}
		}
	}

	// Token: 0x060076C7 RID: 30407 RVA: 0x0030A3D0 File Offset: 0x003085D0
	public int GetWidthInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.x);
			num2 = Math.Max(num2, cellOffset.x);
		}
		return num2 - num + 1;
	}

	// Token: 0x060076C8 RID: 30408 RVA: 0x0030A428 File Offset: 0x00308628
	public int GetHeightInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.y);
			num2 = Math.Max(num2, cellOffset.y);
		}
		return num2 - num + 1;
	}

	// Token: 0x060076C9 RID: 30409 RVA: 0x000EE0E6 File Offset: 0x000EC2E6
	public Extents GetExtents()
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets);
	}

	// Token: 0x060076CA RID: 30410 RVA: 0x000EE0FE File Offset: 0x000EC2FE
	public Extents GetExtents(Orientation orientation)
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets, orientation);
	}

	// Token: 0x060076CB RID: 30411 RVA: 0x0030A480 File Offset: 0x00308680
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (this.OccupiedCellsOffsets != null)
		{
			foreach (CellOffset offset in this.OccupiedCellsOffsets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
			}
		}
		if (this.AboveOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset2 in this.AboveOccupiedCellOffsets)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
		if (this.BelowOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset3 in this.BelowOccupiedCellOffsets)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
	}

	// Token: 0x060076CC RID: 30412 RVA: 0x0030A5F8 File Offset: 0x003087F8
	public bool CanOccupyArea(int rootCell, ObjectLayer layer)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int cell = Grid.OffsetCell(rootCell, offset);
			if (Grid.Objects[cell, (int)layer] != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060076CD RID: 30413 RVA: 0x0030A644 File Offset: 0x00308844
	public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int arg = Grid.OffsetCell(rootCell, offset);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060076CE RID: 30414 RVA: 0x0030A688 File Offset: 0x00308888
	public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.AboveOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y + 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.AboveOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.AboveOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060076CF RID: 30415 RVA: 0x0030A738 File Offset: 0x00308938
	public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.BelowOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y - 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.BelowOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.BelowOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.BelowOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040058D5 RID: 22741
	private CellOffset[] AboveOccupiedCellOffsets;

	// Token: 0x040058D6 RID: 22742
	private CellOffset[] BelowOccupiedCellOffsets;

	// Token: 0x040058D7 RID: 22743
	private int[] occupiedGridCells;

	// Token: 0x040058D8 RID: 22744
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040058D9 RID: 22745
	private Orientation appliedOrientation;

	// Token: 0x040058DA RID: 22746
	public CellOffset[] _UnrotatedOccupiedCellsOffsets;

	// Token: 0x040058DB RID: 22747
	public CellOffset[] _RotatedOccupiedCellsOffsets;

	// Token: 0x040058DC RID: 22748
	public ObjectLayer[] objectLayers = new ObjectLayer[0];

	// Token: 0x040058DD RID: 22749
	[SerializeField]
	private bool applyToCells = true;
}
