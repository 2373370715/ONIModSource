using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001C81 RID: 7297
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	// Token: 0x0600981D RID: 38941 RVA: 0x00102DE8 File Offset: 0x00100FE8
	protected override void OnSpawn()
	{
		this.storages = base.GetComponents<Storage>();
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		base.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
	}

	// Token: 0x0600981E RID: 38942 RVA: 0x00102E18 File Offset: 0x00101018
	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	// Token: 0x0600981F RID: 38943 RVA: 0x00102E2B File Offset: 0x0010102B
	private void OnAteFromStorage(object data)
	{
		if (string.IsNullOrEmpty(this.effectId))
		{
			return;
		}
		(data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
	}

	// Token: 0x06009820 RID: 38944 RVA: 0x003AEFF4 File Offset: 0x003AD1F4
	public bool StoragesAreEmpty()
	{
		foreach (Storage storage in this.storages)
		{
			if (!(storage == null) && storage.Count > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06009821 RID: 38945 RVA: 0x00102E53 File Offset: 0x00101053
	public Vector2I GetTargetFeederCell()
	{
		return Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(this), this.feederOffset));
	}

	// Token: 0x0400766F RID: 30319
	public Storage[] storages;

	// Token: 0x04007670 RID: 30320
	public string effectId;

	// Token: 0x04007671 RID: 30321
	public CellOffset feederOffset = CellOffset.none;

	// Token: 0x04007672 RID: 30322
	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});
}
