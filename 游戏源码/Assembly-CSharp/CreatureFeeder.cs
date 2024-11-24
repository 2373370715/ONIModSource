using Klei.AI;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	public Storage[] storages;

	public string effectId;

	public CellOffset feederOffset = CellOffset.none;

	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});

	protected override void OnSpawn()
	{
		storages = GetComponents<Storage>();
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		Subscribe(-1452790913, OnAteFromStorageDelegate);
	}

	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	private void OnAteFromStorage(object data)
	{
		if (!string.IsNullOrEmpty(effectId))
		{
			(data as GameObject).GetComponent<Effects>().Add(effectId, should_save: true);
		}
	}

	public bool StoragesAreEmpty()
	{
		Storage[] array = storages;
		foreach (Storage storage in array)
		{
			if (!(storage == null) && storage.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	public Vector2I GetTargetFeederCell()
	{
		return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), feederOffset));
	}
}
