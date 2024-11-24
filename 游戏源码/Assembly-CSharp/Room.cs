using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017F0 RID: 6128
public class Room : IAssignableIdentity
{
	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06007E37 RID: 32311 RVA: 0x000F332C File Offset: 0x000F152C
	public List<KPrefabID> buildings
	{
		get
		{
			return this.cavity.buildings;
		}
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x06007E38 RID: 32312 RVA: 0x000F3339 File Offset: 0x000F1539
	public List<KPrefabID> plants
	{
		get
		{
			return this.cavity.plants;
		}
	}

	// Token: 0x06007E39 RID: 32313 RVA: 0x000F3346 File Offset: 0x000F1546
	public string GetProperName()
	{
		return this.roomType.Name;
	}

	// Token: 0x06007E3A RID: 32314 RVA: 0x003297FC File Offset: 0x003279FC
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (KPrefabID kprefabID in this.GetPrimaryEntities())
		{
			if (kprefabID != null)
			{
				Ownable component = kprefabID.GetComponent<Ownable>();
				if (component != null && component.assignee != null && component.assignee != this)
				{
					foreach (Ownables item in component.assignee.GetOwners())
					{
						if (!this.current_owners.Contains(item))
						{
							this.current_owners.Add(item);
						}
					}
				}
			}
		}
		return this.current_owners;
	}

	// Token: 0x06007E3B RID: 32315 RVA: 0x003298E8 File Offset: 0x00327AE8
	public List<GameObject> GetBuildingsOnFloor()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.buildings.Count; i++)
		{
			if (!Grid.Solid[Grid.PosToCell(this.buildings[i])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell(this.buildings[i]))])
			{
				list.Add(this.buildings[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06007E3C RID: 32316 RVA: 0x00329968 File Offset: 0x00327B68
	public Ownables GetSoleOwner()
	{
		List<Ownables> owners = this.GetOwners();
		if (owners.Count <= 0)
		{
			return null;
		}
		return owners[0];
	}

	// Token: 0x06007E3D RID: 32317 RVA: 0x00329990 File Offset: 0x00327B90
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Find((Ownables x) => x == owner) != null;
	}

	// Token: 0x06007E3E RID: 32318 RVA: 0x000F3353 File Offset: 0x000F1553
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06007E3F RID: 32319 RVA: 0x003299C8 File Offset: 0x00327BC8
	public List<KPrefabID> GetPrimaryEntities()
	{
		this.primary_buildings.Clear();
		RoomType roomType = this.roomType;
		if (roomType.primary_constraint != null)
		{
			foreach (KPrefabID kprefabID in this.buildings)
			{
				if (kprefabID != null && roomType.primary_constraint.building_criteria(kprefabID))
				{
					this.primary_buildings.Add(kprefabID);
				}
			}
			foreach (KPrefabID kprefabID2 in this.plants)
			{
				if (kprefabID2 != null && roomType.primary_constraint.building_criteria(kprefabID2))
				{
					this.primary_buildings.Add(kprefabID2);
				}
			}
		}
		return this.primary_buildings;
	}

	// Token: 0x06007E40 RID: 32320 RVA: 0x00329AC4 File Offset: 0x00327CC4
	public void RetriggerBuildings()
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, this);
			}
		}
		foreach (KPrefabID kprefabID2 in this.plants)
		{
			if (!(kprefabID2 == null))
			{
				kprefabID2.Trigger(144050788, this);
			}
		}
	}

	// Token: 0x06007E41 RID: 32321 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06007E42 RID: 32322 RVA: 0x000F3360 File Offset: 0x000F1560
	public void CleanUp()
	{
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
	}

	// Token: 0x04005F80 RID: 24448
	public CavityInfo cavity;

	// Token: 0x04005F81 RID: 24449
	public RoomType roomType;

	// Token: 0x04005F82 RID: 24450
	private List<KPrefabID> primary_buildings = new List<KPrefabID>();

	// Token: 0x04005F83 RID: 24451
	private List<Ownables> current_owners = new List<Ownables>();
}
