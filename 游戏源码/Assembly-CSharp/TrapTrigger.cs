using UnityEngine;

public class TrapTrigger : KMonoBehaviour
{
	private HandleVector<int>.Handle partitionerEntry;

	public Tag[] trappableCreatures;

	public Vector2 trappedOffset = Vector2.zero;

	public bool addTrappedAnimationOffset = true;

	[MyCmpReq]
	private Storage storage;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameObject go = base.gameObject;
		SetTriggerCell(Grid.PosToCell(go));
		foreach (GameObject item in storage.items)
		{
			SetStoredPosition(item);
			KBoxCollider2D component = item.GetComponent<KBoxCollider2D>();
			if (component != null)
			{
				component.enabled = true;
			}
		}
	}

	public void SetTriggerCell(int cell)
	{
		_ = partitionerEntry;
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
		partitionerEntry = GameScenePartitioner.Instance.Add("Trap", base.gameObject, cell, GameScenePartitioner.Instance.trapsLayer, OnCreatureOnTrap);
	}

	public void SetStoredPosition(GameObject go)
	{
		if (!(go == null))
		{
			KBatchedAnimController component = go.GetComponent<KBatchedAnimController>();
			Vector3 vector = Grid.CellToPosCBC(Grid.PosToCell(base.transform.GetPosition()), Grid.SceneLayer.BuildingBack);
			if (addTrappedAnimationOffset)
			{
				vector.x += trappedOffset.x - component.Offset.x;
				vector.y += trappedOffset.y - component.Offset.y;
			}
			else
			{
				vector.x += trappedOffset.x;
				vector.y += trappedOffset.y;
			}
			go.transform.SetPosition(vector);
			go.GetComponent<Pickupable>().UpdateCachedCell(Grid.PosToCell(vector));
			component.SetSceneLayer(Grid.SceneLayer.BuildingFront);
		}
	}

	public void OnCreatureOnTrap(object data)
	{
		if (!base.enabled || !storage.IsEmpty())
		{
			return;
		}
		Trappable trappable = (Trappable)data;
		if (trappable.HasTag(GameTags.Stored) || trappable.HasTag(GameTags.Trapped) || trappable.HasTag(GameTags.Creatures.Bagged))
		{
			return;
		}
		bool flag = false;
		Tag[] array = trappableCreatures;
		foreach (Tag tag in array)
		{
			if (trappable.HasTag(tag))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			storage.Store(trappable.gameObject, hide_popups: true);
			SetStoredPosition(trappable.gameObject);
			Trigger(-358342870, trappable.gameObject);
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
	}
}
