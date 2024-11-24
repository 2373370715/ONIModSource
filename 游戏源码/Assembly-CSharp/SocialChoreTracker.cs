using System;
using UnityEngine;

// Token: 0x0200167D RID: 5757
public class SocialChoreTracker
{
	// Token: 0x060076F9 RID: 30457 RVA: 0x0030C48C File Offset: 0x0030A68C
	public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
	{
		this.owner = owner;
		this.choreOffsets = chore_offsets;
		this.chores = new Chore[this.choreOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
	}

	// Token: 0x060076FA RID: 30458 RVA: 0x0030C500 File Offset: 0x0030A700
	public void Update(bool update = true)
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		int num = 0;
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			CellOffset offset = this.choreOffsets[i];
			Chore chore = this.chores[i];
			if (update && num < this.choreCount && this.IsOffsetValid(offset))
			{
				num++;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = ((this.CreateChoreCB != null) ? this.CreateChoreCB(i) : null);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
		this.updating = false;
	}

	// Token: 0x060076FB RID: 30459 RVA: 0x000EE25A File Offset: 0x000EC45A
	private void OnCellChanged(object data)
	{
		if (this.owner.HasTag(GameTags.Operational))
		{
			this.Update(true);
		}
	}

	// Token: 0x060076FC RID: 30460 RVA: 0x000EE275 File Offset: 0x000EC475
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.Update(false);
	}

	// Token: 0x060076FD RID: 30461 RVA: 0x0030C5B4 File Offset: 0x0030A7B4
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
	}

	// Token: 0x040058FA RID: 22778
	public Func<int, Chore> CreateChoreCB;

	// Token: 0x040058FB RID: 22779
	public int choreCount;

	// Token: 0x040058FC RID: 22780
	private GameObject owner;

	// Token: 0x040058FD RID: 22781
	private CellOffset[] choreOffsets;

	// Token: 0x040058FE RID: 22782
	private Chore[] chores;

	// Token: 0x040058FF RID: 22783
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04005900 RID: 22784
	private bool updating;
}
