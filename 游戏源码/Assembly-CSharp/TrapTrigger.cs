using System;
using UnityEngine;

// Token: 0x020019F4 RID: 6644
public class TrapTrigger : KMonoBehaviour
{
	// Token: 0x06008A70 RID: 35440 RVA: 0x0035B4FC File Offset: 0x003596FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject gameObject = base.gameObject;
		this.SetTriggerCell(Grid.PosToCell(gameObject));
		foreach (GameObject gameObject2 in this.storage.items)
		{
			this.SetStoredPosition(gameObject2);
			KBoxCollider2D component = gameObject2.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	// Token: 0x06008A71 RID: 35441 RVA: 0x0035B584 File Offset: 0x00359784
	public void SetTriggerCell(int cell)
	{
		HandleVector<int>.Handle handle = this.partitionerEntry;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, new Action<object>(this.OnCreatureOnTrap));
	}

	// Token: 0x06008A72 RID: 35442 RVA: 0x0035B5DC File Offset: 0x003597DC
	public void SetStoredPosition(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
		Vector3 vector = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
		if (this.addTrappedAnimationOffset)
		{
			vector.x += this.trappedOffset.x - component.Offset.x;
			vector.y += this.trappedOffset.y - component.Offset.y;
		}
		else
		{
			vector.x += this.trappedOffset.x;
			vector.y += this.trappedOffset.y;
		}
		go.transform.SetPosition(vector);
		go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(vector));
		component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
	}

	// Token: 0x06008A73 RID: 35443 RVA: 0x0035B6B4 File Offset: 0x003598B4
	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Trapped))
		{
			return;
		}
		if (trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		foreach (Tag tag in this.trappableCreatures)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		this.storage.Store(trappable.gameObject, true, false, true, false);
		this.SetStoredPosition(trappable.gameObject);
		base.Trigger(-358342870, trappable.gameObject);
	}

	// Token: 0x06008A74 RID: 35444 RVA: 0x000FA9EA File Offset: 0x000F8BEA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0400683C RID: 26684
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400683D RID: 26685
	public Tag[] trappableCreatures;

	// Token: 0x0400683E RID: 26686
	public Vector2 trappedOffset = Vector2.zero;

	// Token: 0x0400683F RID: 26687
	public bool addTrappedAnimationOffset = true;

	// Token: 0x04006840 RID: 26688
	[MyCmpReq]
	private Storage storage;
}
