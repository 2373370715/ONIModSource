using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017FC RID: 6140
public class CavityInfo
{
	// Token: 0x06007EB0 RID: 32432 RVA: 0x0032C37C File Offset: 0x0032A57C
	public CavityInfo()
	{
		this.handle = HandleVector<int>.InvalidHandle;
		this.dirty = true;
	}

	// Token: 0x06007EB1 RID: 32433 RVA: 0x000F381D File Offset: 0x000F1A1D
	public void AddBuilding(KPrefabID bc)
	{
		this.buildings.Add(bc);
		this.dirty = true;
	}

	// Token: 0x06007EB2 RID: 32434 RVA: 0x000F3832 File Offset: 0x000F1A32
	public void AddPlants(KPrefabID plant)
	{
		this.plants.Add(plant);
		this.dirty = true;
	}

	// Token: 0x06007EB3 RID: 32435 RVA: 0x0032C3D0 File Offset: 0x0032A5D0
	public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
	{
		int num = -1;
		for (int i = 0; i < listToRemove.Count; i++)
		{
			if (id.InstanceID == listToRemove[i].InstanceID)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			listToRemove.RemoveAt(num);
		}
	}

	// Token: 0x06007EB4 RID: 32436 RVA: 0x0032C414 File Offset: 0x0032A614
	public void OnEnter(object data)
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(-832141045, data);
			}
		}
	}

	// Token: 0x06007EB5 RID: 32437 RVA: 0x000F3847 File Offset: 0x000F1A47
	public Vector3 GetCenter()
	{
		return new Vector3((float)(this.minX + (this.maxX - this.minX) / 2), (float)(this.minY + (this.maxY - this.minY) / 2));
	}

	// Token: 0x04005FFC RID: 24572
	public HandleVector<int>.Handle handle;

	// Token: 0x04005FFD RID: 24573
	public bool dirty;

	// Token: 0x04005FFE RID: 24574
	public int numCells;

	// Token: 0x04005FFF RID: 24575
	public int maxX;

	// Token: 0x04006000 RID: 24576
	public int maxY;

	// Token: 0x04006001 RID: 24577
	public int minX;

	// Token: 0x04006002 RID: 24578
	public int minY;

	// Token: 0x04006003 RID: 24579
	public Room room;

	// Token: 0x04006004 RID: 24580
	public List<KPrefabID> buildings = new List<KPrefabID>();

	// Token: 0x04006005 RID: 24581
	public List<KPrefabID> plants = new List<KPrefabID>();

	// Token: 0x04006006 RID: 24582
	public List<KPrefabID> creatures = new List<KPrefabID>();

	// Token: 0x04006007 RID: 24583
	public List<KPrefabID> eggs = new List<KPrefabID>();
}
