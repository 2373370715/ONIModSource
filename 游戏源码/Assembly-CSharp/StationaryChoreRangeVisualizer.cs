using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B13 RID: 2835
[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
[Obsolete("Deprecated, use RangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
	// Token: 0x06003543 RID: 13635 RVA: 0x0020DF9C File Offset: 0x0020C19C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
		if (this.movable)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
			base.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
		}
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x0020DFFC File Offset: 0x0020C1FC
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
		this.ClearVisualizers();
		base.OnCleanUp();
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x0020E054 File Offset: 0x0020C254
	private void OnSelect(object data)
	{
		if ((bool)data)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), base.transform.position, 1f);
			this.UpdateVisualizers();
			return;
		}
		SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), base.transform.position, 1f);
		this.ClearVisualizers();
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x000C2BA3 File Offset: 0x000C0DA3
	private void OnRotated(object data)
	{
		this.UpdateVisualizers();
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x000C2BA3 File Offset: 0x000C0DA3
	private void OnCellChange()
	{
		this.UpdateVisualizers();
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x0020E0B8 File Offset: 0x0020C2B8
	private void UpdateVisualizers()
	{
		this.newCells.Clear();
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out num, out num2);
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num3))
				{
					int x;
					int y;
					Grid.CellToXY(num3, out x, out y);
					if (Grid.TestLineOfSight(num, num2, x, y, this.blocking_cb, this.blocking_tile_visible, false))
					{
						this.newCells.Add(num3);
					}
				}
			}
		}
		for (int k = this.visualizers.Count - 1; k >= 0; k--)
		{
			if (this.newCells.Contains(this.visualizers[k].cell))
			{
				this.newCells.Remove(this.visualizers[k].cell);
			}
			else
			{
				this.DestroyEffect(this.visualizers[k].controller);
				this.visualizers.RemoveAt(k);
			}
		}
		for (int l = 0; l < this.newCells.Count; l++)
		{
			KBatchedAnimController controller = this.CreateEffect(this.newCells[l]);
			this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData
			{
				cell = this.newCells[l],
				controller = controller
			});
		}
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0020E2A8 File Offset: 0x0020C4A8
	private void ClearVisualizers()
	{
		for (int i = 0; i < this.visualizers.Count; i++)
		{
			this.DestroyEffect(this.visualizers[i].controller);
		}
		this.visualizers.Clear();
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x0020E2F0 File Offset: 0x0020C4F0
	private KBatchedAnimController CreateEffect(int cell)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), null, false, this.sceneLayer, true);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
		return kbatchedAnimController;
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x000C2BAB File Offset: 0x000C0DAB
	private void DestroyEffect(KBatchedAnimController controller)
	{
		controller.destroyOnAnimComplete = true;
		controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04002434 RID: 9268
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002435 RID: 9269
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002436 RID: 9270
	public int x;

	// Token: 0x04002437 RID: 9271
	public int y;

	// Token: 0x04002438 RID: 9272
	public int width;

	// Token: 0x04002439 RID: 9273
	public int height;

	// Token: 0x0400243A RID: 9274
	public bool movable;

	// Token: 0x0400243B RID: 9275
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;

	// Token: 0x0400243C RID: 9276
	public CellOffset vision_offset;

	// Token: 0x0400243D RID: 9277
	public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);

	// Token: 0x0400243E RID: 9278
	public bool blocking_tile_visible = true;

	// Token: 0x0400243F RID: 9279
	private static readonly string AnimName = "transferarmgrid_kanim";

	// Token: 0x04002440 RID: 9280
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x04002441 RID: 9281
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x04002442 RID: 9282
	private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();

	// Token: 0x04002443 RID: 9283
	private List<int> newCells = new List<int>();

	// Token: 0x04002444 RID: 9284
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnSelect(data);
	});

	// Token: 0x04002445 RID: 9285
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnRotated(data);
	});

	// Token: 0x02000B14 RID: 2836
	private struct VisData
	{
		// Token: 0x04002446 RID: 9286
		public int cell;

		// Token: 0x04002447 RID: 9287
		public KBatchedAnimController controller;
	}
}
