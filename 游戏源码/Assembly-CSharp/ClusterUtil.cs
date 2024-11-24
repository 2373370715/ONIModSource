using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200109A RID: 4250
public static class ClusterUtil
{
	// Token: 0x06005724 RID: 22308 RVA: 0x000D8EAE File Offset: 0x000D70AE
	public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorld();
	}

	// Token: 0x06005725 RID: 22309 RVA: 0x000D8EBB File Offset: 0x000D70BB
	public static WorldContainer GetMyWorld(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorld();
	}

	// Token: 0x06005726 RID: 22310 RVA: 0x00284EE4 File Offset: 0x002830E4
	public static WorldContainer GetMyWorld(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
		}
		return null;
	}

	// Token: 0x06005727 RID: 22311 RVA: 0x000D8EC8 File Offset: 0x000D70C8
	public static int GetMyWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldId();
	}

	// Token: 0x06005728 RID: 22312 RVA: 0x000D8ED5 File Offset: 0x000D70D5
	public static int GetMyWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldId();
	}

	// Token: 0x06005729 RID: 22313 RVA: 0x00284F24 File Offset: 0x00283124
	public static int GetMyWorldId(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return (int)Grid.WorldIdx[num];
		}
		return -1;
	}

	// Token: 0x0600572A RID: 22314 RVA: 0x000D8EE2 File Offset: 0x000D70E2
	public static int GetMyParentWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
	}

	// Token: 0x0600572B RID: 22315 RVA: 0x000D8EEF File Offset: 0x000D70EF
	public static int GetMyParentWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyParentWorldId();
	}

	// Token: 0x0600572C RID: 22316 RVA: 0x00284F58 File Offset: 0x00283158
	public static int GetMyParentWorldId(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld == null)
		{
			return gameObject.GetMyWorldId();
		}
		return myWorld.ParentWorldId;
	}

	// Token: 0x0600572D RID: 22317 RVA: 0x000D8EFC File Offset: 0x000D70FC
	public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
	}

	// Token: 0x0600572E RID: 22318 RVA: 0x000D8F09 File Offset: 0x000D7109
	public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldLocation();
	}

	// Token: 0x0600572F RID: 22319 RVA: 0x00284F84 File Offset: 0x00283184
	public static AxialI GetMyWorldLocation(this GameObject gameObject)
	{
		ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
		if (component != null)
		{
			return component.Location;
		}
		WorldContainer myWorld = gameObject.GetMyWorld();
		DebugUtil.DevAssertArgs(myWorld != null, new object[]
		{
			"GetMyWorldLocation called on object with no world",
			gameObject
		});
		return myWorld.GetComponent<ClusterGridEntity>().Location;
	}

	// Token: 0x06005730 RID: 22320 RVA: 0x00284FD8 File Offset: 0x002831D8
	public static bool IsMyWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyWorld(otherCell);
	}

	// Token: 0x06005731 RID: 22321 RVA: 0x00284FF4 File Offset: 0x002831F4
	public static bool IsMyWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		return Grid.IsValidCell(num) && Grid.IsValidCell(otherCell) && Grid.WorldIdx[num] == Grid.WorldIdx[otherCell];
	}

	// Token: 0x06005732 RID: 22322 RVA: 0x0028502C File Offset: 0x0028322C
	public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyParentWorld(otherCell);
	}

	// Token: 0x06005733 RID: 22323 RVA: 0x00285048 File Offset: 0x00283248
	public static bool IsMyParentWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		if (Grid.IsValidCell(num) && Grid.IsValidCell(otherCell))
		{
			if (Grid.WorldIdx[num] == Grid.WorldIdx[otherCell])
			{
				return true;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			WorldContainer world2 = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[otherCell]);
			if (world == null)
			{
				DebugUtil.DevLogError(string.Format("{0} at {1} has a valid cell but no world", go, num));
			}
			if (world2 == null)
			{
				DebugUtil.DevLogError(string.Format("{0} is a valid cell but no world", otherCell));
			}
			if (world != null && world2 != null && world.ParentWorldId == world2.ParentWorldId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005734 RID: 22324 RVA: 0x00285108 File Offset: 0x00283308
	public static int GetAsteroidWorldIdAtLocation(AxialI location)
	{
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid)
			{
				WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
				if (component != null)
				{
					return component.id;
				}
			}
		}
		return -1;
	}

	// Token: 0x06005735 RID: 22325 RVA: 0x000D8F16 File Offset: 0x000D7116
	public static bool ActiveWorldIsRocketInterior()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior;
	}

	// Token: 0x06005736 RID: 22326 RVA: 0x000D8F27 File Offset: 0x000D7127
	public static bool ActiveWorldHasPrinter()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count > 0;
	}

	// Token: 0x06005737 RID: 22327 RVA: 0x00285184 File Offset: 0x00283384
	public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
	{
		WorldContainer worldContainer = worldInventory.WorldContainer;
		float num = 0f;
		int parentWorldId = worldContainer.ParentWorldId;
		foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer2.ParentWorldId == parentWorldId)
			{
				num += worldContainer2.worldInventory.GetAmount(element, false);
			}
		}
		return num;
	}

	// Token: 0x06005738 RID: 22328 RVA: 0x002851FC File Offset: 0x002833FC
	public static List<Pickupable> GetPickupablesFromRelatedWorlds(WorldInventory worldInventory, Tag tag)
	{
		List<Pickupable> list = new List<Pickupable>();
		int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.ParentWorldId == parentWorldId)
			{
				ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag, false);
				if (pickupables != null)
				{
					list.AddRange(pickupables);
				}
			}
		}
		return list;
	}

	// Token: 0x06005739 RID: 22329 RVA: 0x00285280 File Offset: 0x00283480
	public static string DebugGetMyWorldName(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld != null)
		{
			return myWorld.worldName;
		}
		return string.Format("InvalidWorld(pos={0})", gameObject.transform.GetPosition());
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x002852C0 File Offset: 0x002834C0
	public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
	{
		foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
		{
			if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
			{
				ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
				if (asteroidAtCell != null)
				{
					return asteroidAtCell;
				}
			}
		}
		return null;
	}
}
