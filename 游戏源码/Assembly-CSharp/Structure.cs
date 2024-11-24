using System;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
	// Token: 0x06003631 RID: 13873 RVA: 0x000C340A File Offset: 0x000C160A
	public bool IsEntombed()
	{
		return this.isEntombed;
	}

	// Token: 0x06003632 RID: 13874 RVA: 0x00212F14 File Offset: 0x00211114
	public static bool IsBuildingEntombed(Building building)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(building)))
		{
			return false;
		}
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			int num = building.PlacementCells[i];
			if (Grid.Element[num].IsSolid && !Grid.Foundation[num])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x00212F6C File Offset: 0x0021116C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Extents extents = this.building.GetExtents();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		base.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x000C3412 File Offset: 0x000C1612
	public void UpdatePosition()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x000C342F File Offset: 0x000C162F
	private void RocketChanged(object data)
	{
		this.OnSolidChanged(data);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x00212FD8 File Offset: 0x002111D8
	private void OnSolidChanged(object data)
	{
		bool flag = Structure.IsBuildingEntombed(this.building);
		if (flag != this.isEntombed)
		{
			this.isEntombed = flag;
			if (this.isEntombed)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			}
			this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, this);
			base.Trigger(-1089732772, null);
		}
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x000C3438 File Offset: 0x000C1638
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040024E7 RID: 9447
	[MyCmpReq]
	private Building building;

	// Token: 0x040024E8 RID: 9448
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040024E9 RID: 9449
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040024EA RID: 9450
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x040024EB RID: 9451
	private bool isEntombed;

	// Token: 0x040024EC RID: 9452
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040024ED RID: 9453
	private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>(delegate(Structure cmp, object data)
	{
		cmp.RocketChanged(data);
	});
}
