using System;
using UnityEngine;

// Token: 0x02000ED9 RID: 3801
public class ModularConduitPortTiler : KMonoBehaviour
{
	// Token: 0x06004C9C RID: 19612 RVA: 0x00262D48 File Offset: 0x00260F48
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				GameTags.ModularConduitPort
			};
		}
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x00262D98 File Offset: 0x00260F98
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.leftCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
		if (this.manageLeftCap)
		{
			this.leftCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
			this.leftCapConduit = new KAnimSynchronizedController(component2, component2.GetLayer() + Grid.SceneLayer.Backwall, ModularConduitPortTiler.leftCapConduitStr);
		}
		this.rightCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
		if (this.manageRightCap)
		{
			this.rightCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
			this.rightCapConduit = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
		this.CorrectAdjacentLaunchPads();
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x000D1A93 File Offset: 0x000CFC93
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x00262EF0 File Offset: 0x002610F0
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellLeft = this.GetCellLeft();
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellLeft))
		{
			if (this.HasTileableNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (Grid.IsValidCell(cellRight))
		{
			if (this.HasTileableNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (this.manageLeftCap)
		{
			this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
		if (this.manageRightCap)
		{
			this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
	}

	// Token: 0x06004CA0 RID: 19616 RVA: 0x00263008 File Offset: 0x00261208
	private int GetCellLeft()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num - 1, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06004CA1 RID: 19617 RVA: 0x00263044 File Offset: 0x00261244
	private int GetCellRight()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num + this.extents.width, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06004CA2 RID: 19618 RVA: 0x00263088 File Offset: 0x00261288
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

	// Token: 0x06004CA3 RID: 19619 RVA: 0x002630D4 File Offset: 0x002612D4
	private bool HasLaunchpadNeighbour(int neighbour_cell)
	{
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06004CA4 RID: 19620 RVA: 0x000D1AAB File Offset: 0x000CFCAB
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

	// Token: 0x06004CA5 RID: 19621 RVA: 0x00263110 File Offset: 0x00261310
	private void CorrectAdjacentLaunchPads()
	{
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
		{
			Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
		int cellLeft = this.GetCellLeft();
		if (Grid.IsValidCell(cellLeft) && this.HasLaunchpadNeighbour(cellLeft))
		{
			Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
	}

	// Token: 0x04003538 RID: 13624
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003539 RID: 13625
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x0400353A RID: 13626
	public Tag[] tags;

	// Token: 0x0400353B RID: 13627
	public bool manageLeftCap = true;

	// Token: 0x0400353C RID: 13628
	public bool manageRightCap = true;

	// Token: 0x0400353D RID: 13629
	public int leftCapDefaultSceneLayerAdjust;

	// Token: 0x0400353E RID: 13630
	public int rightCapDefaultSceneLayerAdjust;

	// Token: 0x0400353F RID: 13631
	private Extents extents;

	// Token: 0x04003540 RID: 13632
	private ModularConduitPortTiler.AnimCapType leftCapSetting;

	// Token: 0x04003541 RID: 13633
	private ModularConduitPortTiler.AnimCapType rightCapSetting;

	// Token: 0x04003542 RID: 13634
	private static readonly string leftCapDefaultStr = "#cap_left_default";

	// Token: 0x04003543 RID: 13635
	private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";

	// Token: 0x04003544 RID: 13636
	private static readonly string leftCapConduitStr = "#cap_left_conduit";

	// Token: 0x04003545 RID: 13637
	private static readonly string rightCapDefaultStr = "#cap_right_default";

	// Token: 0x04003546 RID: 13638
	private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";

	// Token: 0x04003547 RID: 13639
	private static readonly string rightCapConduitStr = "#cap_right_conduit";

	// Token: 0x04003548 RID: 13640
	private KAnimSynchronizedController leftCapDefault;

	// Token: 0x04003549 RID: 13641
	private KAnimSynchronizedController leftCapLaunchpad;

	// Token: 0x0400354A RID: 13642
	private KAnimSynchronizedController leftCapConduit;

	// Token: 0x0400354B RID: 13643
	private KAnimSynchronizedController rightCapDefault;

	// Token: 0x0400354C RID: 13644
	private KAnimSynchronizedController rightCapLaunchpad;

	// Token: 0x0400354D RID: 13645
	private KAnimSynchronizedController rightCapConduit;

	// Token: 0x02000EDA RID: 3802
	private enum AnimCapType
	{
		// Token: 0x0400354F RID: 13647
		Default,
		// Token: 0x04003550 RID: 13648
		Conduit,
		// Token: 0x04003551 RID: 13649
		Launchpad
	}
}
