using System;
using UnityEngine;

// Token: 0x0200117B RID: 4475
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06005B47 RID: 23367 RVA: 0x000DB84B File Offset: 0x000D9A4B
	// (set) Token: 0x06005B48 RID: 23368 RVA: 0x000DB853 File Offset: 0x000D9A53
	public bool Valid { get; private set; }

	// Token: 0x06005B49 RID: 23369 RVA: 0x00297154 File Offset: 0x00295354
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
		if (this.objectLayer != ObjectLayer.NumLayers)
		{
			this.objectPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnAreaChanged));
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
		this.OnAreaChanged(null);
	}

	// Token: 0x06005B4A RID: 23370 RVA: 0x0029721C File Offset: 0x0029541C
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x06005B4B RID: 23371 RVA: 0x000DB85C File Offset: 0x000D9A5C
	private void OnCellChange()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
		this.OnAreaChanged(null);
	}

	// Token: 0x06005B4C RID: 23372 RVA: 0x000DB89B File Offset: 0x000D9A9B
	public void SetSolid()
	{
		this.occupyArea.ApplyToCells = true;
	}

	// Token: 0x06005B4D RID: 23373 RVA: 0x000DB8A9 File Offset: 0x000D9AA9
	private void OnAreaChanged(object obj)
	{
		this.UpdateValidity();
	}

	// Token: 0x06005B4E RID: 23374 RVA: 0x0029726C File Offset: 0x0029546C
	public void UpdateValidity()
	{
		bool valid = this.Valid;
		this.Valid = this.occupyArea.TestArea(Grid.PosToCell(this), this, EntityPreview.ValidTestDelegate);
		if (this.Valid)
		{
			this.animController.TintColour = Color.white;
		}
		else
		{
			this.animController.TintColour = Color.red;
		}
		if (valid != this.Valid)
		{
			base.Trigger(-1820564715, this.Valid);
		}
	}

	// Token: 0x06005B4F RID: 23375 RVA: 0x002972F0 File Offset: 0x002954F0
	private static bool ValidTest(int cell, object data)
	{
		EntityPreview entityPreview = (EntityPreview)data;
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && (entityPreview.objectLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)entityPreview.objectLayer] == entityPreview.gameObject || Grid.Objects[cell, (int)entityPreview.objectLayer] == null);
	}

	// Token: 0x04004076 RID: 16502
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04004077 RID: 16503
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04004078 RID: 16504
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04004079 RID: 16505
	public ObjectLayer objectLayer = ObjectLayer.NumLayers;

	// Token: 0x0400407B RID: 16507
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x0400407C RID: 16508
	private HandleVector<int>.Handle objectPartitionerEntry;

	// Token: 0x0400407D RID: 16509
	private static readonly Func<int, object, bool> ValidTestDelegate = (int cell, object data) => EntityPreview.ValidTest(cell, data);
}
