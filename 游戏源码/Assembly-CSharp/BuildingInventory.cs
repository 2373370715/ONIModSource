using System;
using System.Collections.Generic;

// Token: 0x02000C84 RID: 3204
public class BuildingInventory : KMonoBehaviour
{
	// Token: 0x06003DA2 RID: 15778 RVA: 0x000C7F6D File Offset: 0x000C616D
	public static void DestroyInstance()
	{
		BuildingInventory.Instance = null;
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x000C7F75 File Offset: 0x000C6175
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildingInventory.Instance = this;
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x000C7F83 File Offset: 0x000C6183
	public HashSet<BuildingComplete> GetBuildings(Tag tag)
	{
		return this.Buildings[tag];
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x000C7F91 File Offset: 0x000C6191
	public int BuildingCount(Tag tag)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		return this.Buildings[tag].Count;
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00231E2C File Offset: 0x0023002C
	public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		int num = 0;
		using (HashSet<BuildingComplete>.Enumerator enumerator = this.Buildings[tag].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetMyWorldId() == worldId)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00231E9C File Offset: 0x0023009C
	public void RegisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			hashSet = new HashSet<BuildingComplete>();
			this.Buildings[prefabTag] = hashSet;
		}
		hashSet.Add(building);
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00231EE0 File Offset: 0x002300E0
	public void UnregisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			DebugUtil.DevLogError(string.Format("Unregistering building {0} before it was registered.", prefabTag));
			return;
		}
		DebugUtil.DevAssert(hashSet.Remove(building), string.Format("Building {0} was not found to be removed", prefabTag), null);
	}

	// Token: 0x040029FB RID: 10747
	public static BuildingInventory Instance;

	// Token: 0x040029FC RID: 10748
	private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();
}
