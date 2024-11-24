using System;

// Token: 0x020012DC RID: 4828
public class FakeFloorAdder : KMonoBehaviour
{
	// Token: 0x06006317 RID: 25367 RVA: 0x000E0BB4 File Offset: 0x000DEDB4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	// Token: 0x06006318 RID: 25368 RVA: 0x002B8AA0 File Offset: 0x002B6CA0
	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Building component = base.GetComponent<Building>();
		foreach (CellOffset offset in this.floorOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (active)
			{
				Grid.FakeFloor.Add(num);
			}
			else
			{
				Grid.FakeFloor.Remove(num);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
		this.isActive = active;
	}

	// Token: 0x06006319 RID: 25369 RVA: 0x000E0BCB File Offset: 0x000DEDCB
	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	// Token: 0x040046B3 RID: 18099
	public CellOffset[] floorOffsets;

	// Token: 0x040046B4 RID: 18100
	public bool initiallyActive = true;

	// Token: 0x040046B5 RID: 18101
	private bool isActive;
}
