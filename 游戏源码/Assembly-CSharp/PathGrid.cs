using System;
using System.Collections.Generic;

// Token: 0x020007F2 RID: 2034
public class PathGrid
{
	// Token: 0x0600245A RID: 9306 RVA: 0x000B7A19 File Offset: 0x000B5C19
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.groupProber = group_prober;
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x001C9564 File Offset: 0x001C7764
	public PathGrid(int width_in_cells, int height_in_cells, bool apply_offset, NavType[] valid_nav_types)
	{
		this.applyOffset = apply_offset;
		this.widthInCells = width_in_cells;
		this.heightInCells = height_in_cells;
		this.ValidNavTypes = valid_nav_types;
		int num = 0;
		this.NavTypeTable = new int[11];
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
		DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!", null);
		this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
		this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
		this.serialNo = 0;
		this.previousSerialNo = -1;
		this.isUpdating = false;
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x000B7A22 File Offset: 0x000B5C22
	public void OnCleanUp()
	{
		if (this.groupProber != null)
		{
			this.groupProber.ReleaseProber(this);
		}
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x000B7A39 File Offset: 0x000B5C39
	public void ResetUpdate()
	{
		this.previousSerialNo = -1;
	}

	// Token: 0x0600245E RID: 9310 RVA: 0x001C9640 File Offset: 0x001C7840
	public void BeginUpdate(int root_cell, bool isContinuation)
	{
		this.isUpdating = true;
		this.freshlyOccupiedCells.Clear();
		if (isContinuation)
		{
			return;
		}
		if (this.applyOffset)
		{
			Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
			this.rootX -= this.widthInCells / 2;
			this.rootY -= this.heightInCells / 2;
		}
		this.serialNo += 1;
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.previousSerialNo, this.serialNo);
		}
	}

	// Token: 0x0600245F RID: 9311 RVA: 0x001C96D8 File Offset: 0x001C78D8
	public void EndUpdate(bool isComplete)
	{
		this.isUpdating = false;
		if (this.groupProber != null)
		{
			this.groupProber.Occupy(this, this.serialNo, this.freshlyOccupiedCells);
		}
		if (!isComplete)
		{
			return;
		}
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.serialNo, this.serialNo);
		}
		this.previousSerialNo = this.serialNo;
	}

	// Token: 0x06002460 RID: 9312 RVA: 0x000B7A42 File Offset: 0x000B5C42
	private bool IsValidSerialNo(short serialNo)
	{
		return serialNo == this.serialNo || (!this.isUpdating && this.previousSerialNo != -1 && serialNo == this.previousSerialNo);
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x000B7A6B File Offset: 0x000B5C6B
	public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
	{
		return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
	}

	// Token: 0x06002462 RID: 9314 RVA: 0x001C973C File Offset: 0x001C793C
	public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
	{
		int num = this.OffsetCell(cell);
		is_cell_in_range = (-1 != num);
		if (!is_cell_in_range)
		{
			return PathGrid.InvalidCell;
		}
		PathFinder.Cell cell2 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type]];
		if (!this.IsValidSerialNo(cell2.queryId))
		{
			return PathGrid.InvalidCell;
		}
		return cell2;
	}

	// Token: 0x06002463 RID: 9315 RVA: 0x001C9798 File Offset: 0x001C7998
	public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
	{
		int num = this.OffsetCell(potential_path.cell);
		if (-1 == num)
		{
			return;
		}
		cell_data.queryId = this.serialNo;
		int num2 = this.NavTypeTable[(int)potential_path.navType];
		int num3 = num * this.ValidNavTypes.Length + num2;
		this.Cells[num3] = cell_data;
		if (potential_path.navType != NavType.Tube)
		{
			PathGrid.ProberCell proberCell = this.ProberCells[num];
			if (cell_data.queryId != proberCell.queryId || cell_data.cost < proberCell.cost)
			{
				proberCell.queryId = cell_data.queryId;
				proberCell.cost = cell_data.cost;
				this.ProberCells[num] = proberCell;
				this.freshlyOccupiedCells.Add(potential_path.cell);
			}
		}
	}

	// Token: 0x06002464 RID: 9316 RVA: 0x001C985C File Offset: 0x001C7A5C
	public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		int num = -1;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(num2))
			{
				PathGrid.ProberCell proberCell = this.ProberCells[num2];
				if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
				{
					num = proberCell.cost;
				}
			}
		}
		return num;
	}

	// Token: 0x06002465 RID: 9317 RVA: 0x001C98CC File Offset: 0x001C7ACC
	public int GetCost(int cell)
	{
		int num = this.OffsetCell(cell);
		if (-1 == num)
		{
			return -1;
		}
		PathGrid.ProberCell proberCell = this.ProberCells[num];
		if (!this.IsValidSerialNo(proberCell.queryId))
		{
			return -1;
		}
		return proberCell.cost;
	}

	// Token: 0x06002466 RID: 9318 RVA: 0x001C990C File Offset: 0x001C7B0C
	private int OffsetCell(int cell)
	{
		if (!this.applyOffset)
		{
			return cell;
		}
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		if (num < this.rootX || num >= this.rootX + this.widthInCells || num2 < this.rootY || num2 >= this.rootY + this.heightInCells)
		{
			return -1;
		}
		int num3 = num - this.rootX;
		return (num2 - this.rootY) * this.widthInCells + num3;
	}

	// Token: 0x0400186C RID: 6252
	private PathFinder.Cell[] Cells;

	// Token: 0x0400186D RID: 6253
	private PathGrid.ProberCell[] ProberCells;

	// Token: 0x0400186E RID: 6254
	private List<int> freshlyOccupiedCells = new List<int>();

	// Token: 0x0400186F RID: 6255
	private NavType[] ValidNavTypes;

	// Token: 0x04001870 RID: 6256
	private int[] NavTypeTable;

	// Token: 0x04001871 RID: 6257
	private int widthInCells;

	// Token: 0x04001872 RID: 6258
	private int heightInCells;

	// Token: 0x04001873 RID: 6259
	private bool applyOffset;

	// Token: 0x04001874 RID: 6260
	private int rootX;

	// Token: 0x04001875 RID: 6261
	private int rootY;

	// Token: 0x04001876 RID: 6262
	private short serialNo;

	// Token: 0x04001877 RID: 6263
	private short previousSerialNo;

	// Token: 0x04001878 RID: 6264
	private bool isUpdating;

	// Token: 0x04001879 RID: 6265
	private IGroupProber groupProber;

	// Token: 0x0400187A RID: 6266
	public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell
	{
		cost = -1
	};

	// Token: 0x020007F3 RID: 2035
	private struct ProberCell
	{
		// Token: 0x0400187B RID: 6267
		public int cost;

		// Token: 0x0400187C RID: 6268
		public short queryId;
	}
}
