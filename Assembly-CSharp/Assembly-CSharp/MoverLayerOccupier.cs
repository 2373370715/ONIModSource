using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
	private void RefreshCellOccupy()
	{
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in this.cellOffsets)
		{
			int current_cell = Grid.OffsetCell(cell, offset);
			if (this.previousCell != Grid.InvalidCell)
			{
				int previous_cell = Grid.OffsetCell(this.previousCell, offset);
				this.UpdateCell(previous_cell, current_cell);
			}
			else
			{
				this.UpdateCell(this.previousCell, current_cell);
			}
		}
		this.previousCell = cell;
	}

	public void Sim200ms(float dt)
	{
		this.RefreshCellOccupy();
	}

	private void UpdateCell(int previous_cell, int current_cell)
	{
		foreach (ObjectLayer layer in this.objectLayers)
		{
			if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && Grid.Objects[previous_cell, (int)layer] == base.gameObject)
			{
				Grid.Objects[previous_cell, (int)layer] = null;
			}
			GameObject gameObject = Grid.Objects[current_cell, (int)layer];
			if (gameObject == null)
			{
				Grid.Objects[current_cell, (int)layer] = base.gameObject;
			}
			else
			{
				KPrefabID component = base.GetComponent<KPrefabID>();
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component.InstanceID > component2.InstanceID)
				{
					Grid.Objects[current_cell, (int)layer] = base.gameObject;
				}
			}
		}
	}

	private void CleanUpOccupiedCells()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		foreach (CellOffset offset in this.cellOffsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			foreach (ObjectLayer layer in this.objectLayers)
			{
				if (Grid.Objects[cell2, (int)layer] == base.gameObject)
				{
					Grid.Objects[cell2, (int)layer] = null;
				}
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RefreshCellOccupy();
	}

	protected override void OnCleanUp()
	{
		this.CleanUpOccupiedCells();
		base.OnCleanUp();
	}

	private int previousCell = Grid.InvalidCell;

	public ObjectLayer[] objectLayers;

	public CellOffset[] cellOffsets;
}
