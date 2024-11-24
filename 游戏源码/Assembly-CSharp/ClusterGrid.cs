using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

// Token: 0x0200194F RID: 6479
public class ClusterGrid
{
	// Token: 0x0600872A RID: 34602 RVA: 0x000F8723 File Offset: 0x000F6923
	public static void DestroyInstance()
	{
		ClusterGrid.Instance = null;
	}

	// Token: 0x0600872B RID: 34603 RVA: 0x000F872B File Offset: 0x000F692B
	private ClusterFogOfWarManager.Instance GetFOWManager()
	{
		if (this.m_fowManager == null)
		{
			this.m_fowManager = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
		}
		return this.m_fowManager;
	}

	// Token: 0x0600872C RID: 34604 RVA: 0x000F874B File Offset: 0x000F694B
	public bool IsValidCell(AxialI cell)
	{
		return this.cellContents.ContainsKey(cell);
	}

	// Token: 0x0600872D RID: 34605 RVA: 0x000F8759 File Offset: 0x000F6959
	public ClusterGrid(int numRings)
	{
		ClusterGrid.Instance = this;
		this.GenerateGrid(numRings);
		this.m_onClusterLocationChangedDelegate = new Action<object>(this.OnClusterLocationChanged);
	}

	// Token: 0x0600872E RID: 34606 RVA: 0x000F878B File Offset: 0x000F698B
	public ClusterRevealLevel GetCellRevealLevel(AxialI cell)
	{
		return this.GetFOWManager().GetCellRevealLevel(cell);
	}

	// Token: 0x0600872F RID: 34607 RVA: 0x000F8799 File Offset: 0x000F6999
	public bool IsCellVisible(AxialI cell)
	{
		return this.GetFOWManager().IsLocationRevealed(cell);
	}

	// Token: 0x06008730 RID: 34608 RVA: 0x000F87A7 File Offset: 0x000F69A7
	public float GetRevealCompleteFraction(AxialI cell)
	{
		return this.GetFOWManager().GetRevealCompleteFraction(cell);
	}

	// Token: 0x06008731 RID: 34609 RVA: 0x000F87B5 File Offset: 0x000F69B5
	public bool IsVisible(ClusterGridEntity entity)
	{
		return entity.IsVisible && this.IsCellVisible(entity.Location);
	}

	// Token: 0x06008732 RID: 34610 RVA: 0x0034FDD8 File Offset: 0x0034DFD8
	public List<ClusterGridEntity> GetVisibleEntitiesAtCell(AxialI cell)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x06008733 RID: 34611 RVA: 0x0034FE38 File Offset: 0x0034E038
	public ClusterGridEntity GetVisibleEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		if (this.IsValidCell(cell) && this.GetFOWManager().IsLocationRevealed(cell))
		{
			foreach (ClusterGridEntity clusterGridEntity in this.cellContents[cell])
			{
				if (clusterGridEntity.IsVisible && clusterGridEntity.Layer == entityLayer)
				{
					return clusterGridEntity;
				}
			}
		}
		return null;
	}

	// Token: 0x06008734 RID: 34612 RVA: 0x0034FEBC File Offset: 0x0034E0BC
	public ClusterGridEntity GetVisibleEntityOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetVisibleEntitiesAtCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x06008735 RID: 34613 RVA: 0x0034FF00 File Offset: 0x0034E100
	public List<ClusterGridEntity> GetHiddenEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008736 RID: 34614 RVA: 0x0034FF48 File Offset: 0x0034E148
	public List<ClusterGridEntity> GetEntitiesOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x06008737 RID: 34615 RVA: 0x0034FF90 File Offset: 0x0034E190
	public ClusterGridEntity GetEntityOfLayerAtCell(AxialI cell, EntityLayer entityLayer)
	{
		return AxialUtil.GetRing(cell, 0).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetEntitiesOnCell)).FirstOrDefault((ClusterGridEntity entity) => entity.Layer == entityLayer);
	}

	// Token: 0x06008738 RID: 34616 RVA: 0x0034FFD4 File Offset: 0x0034E1D4
	public List<ClusterGridEntity> GetHiddenEntitiesAtCell(AxialI cell)
	{
		if (this.cellContents.ContainsKey(cell) && !this.GetFOWManager().IsLocationRevealed(cell))
		{
			return (from entity in this.cellContents[cell]
			where entity.IsVisible
			select entity).ToList<ClusterGridEntity>();
		}
		return new List<ClusterGridEntity>();
	}

	// Token: 0x06008739 RID: 34617 RVA: 0x000F87CD File Offset: 0x000F69CD
	public List<ClusterGridEntity> GetNotVisibleEntitiesAtAdjacentCell(AxialI cell)
	{
		return AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell)).ToList<ClusterGridEntity>();
	}

	// Token: 0x0600873A RID: 34618 RVA: 0x00350038 File Offset: 0x0034E238
	public List<ClusterGridEntity> GetNotVisibleEntitiesOfLayerAtAdjacentCell(AxialI cell, EntityLayer entityLayer)
	{
		return (from entity in AxialUtil.GetRing(cell, 1).SelectMany(new Func<AxialI, IEnumerable<ClusterGridEntity>>(this.GetHiddenEntitiesAtCell))
		where entity.Layer == entityLayer
		select entity).ToList<ClusterGridEntity>();
	}

	// Token: 0x0600873B RID: 34619 RVA: 0x00350080 File Offset: 0x0034E280
	public bool GetVisibleUnidentifiedMeteorShowerWithinRadius(AxialI center, int radius, out ClusterMapMeteorShower.Instance result)
	{
		for (int i = 0; i <= radius; i++)
		{
			foreach (AxialI axialI in AxialUtil.GetRing(center, i))
			{
				if (this.IsValidCell(axialI) && this.GetFOWManager().IsLocationRevealed(axialI))
				{
					foreach (ClusterGridEntity cmp in this.GetEntitiesOfLayerAtCell(axialI, EntityLayer.Craft))
					{
						ClusterMapMeteorShower.Instance smi = cmp.GetSMI<ClusterMapMeteorShower.Instance>();
						if (smi != null && !smi.HasBeenIdentified)
						{
							result = smi;
							return true;
						}
					}
				}
			}
		}
		result = null;
		return false;
	}

	// Token: 0x0600873C RID: 34620 RVA: 0x00350158 File Offset: 0x0034E358
	public ClusterGridEntity GetAsteroidAtCell(AxialI cell)
	{
		if (!this.cellContents.ContainsKey(cell))
		{
			return null;
		}
		return (from e in this.cellContents[cell]
		where e.Layer == EntityLayer.Asteroid
		select e).FirstOrDefault<ClusterGridEntity>();
	}

	// Token: 0x0600873D RID: 34621 RVA: 0x000F87EC File Offset: 0x000F69EC
	public bool HasVisibleAsteroidAtCell(AxialI cell)
	{
		return this.GetVisibleEntityOfLayerAtCell(cell, EntityLayer.Asteroid) != null;
	}

	// Token: 0x0600873E RID: 34622 RVA: 0x000F87FC File Offset: 0x000F69FC
	public void RegisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Add(entity);
		entity.Subscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x0600873F RID: 34623 RVA: 0x000F8827 File Offset: 0x000F6A27
	public void UnregisterEntity(ClusterGridEntity entity)
	{
		this.cellContents[entity.Location].Remove(entity);
		entity.Unsubscribe(-1298331547, this.m_onClusterLocationChangedDelegate);
	}

	// Token: 0x06008740 RID: 34624 RVA: 0x003501AC File Offset: 0x0034E3AC
	public void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.oldLocation), string.Format("ChangeEntityCell move order FROM invalid location: {0} {1}", clusterLocationChangedEvent.oldLocation, clusterLocationChangedEvent.entity));
		global::Debug.Assert(this.IsValidCell(clusterLocationChangedEvent.newLocation), string.Format("ChangeEntityCell move order TO invalid location: {0} {1}", clusterLocationChangedEvent.newLocation, clusterLocationChangedEvent.entity));
		this.cellContents[clusterLocationChangedEvent.oldLocation].Remove(clusterLocationChangedEvent.entity);
		this.cellContents[clusterLocationChangedEvent.newLocation].Add(clusterLocationChangedEvent.entity);
	}

	// Token: 0x06008741 RID: 34625 RVA: 0x000F8852 File Offset: 0x000F6A52
	private AxialI GetNeighbor(AxialI cell, AxialI direction)
	{
		return cell + direction;
	}

	// Token: 0x06008742 RID: 34626 RVA: 0x00350254 File Offset: 0x0034E454
	public int GetCellRing(AxialI cell)
	{
		Vector3I vector3I = cell.ToCube();
		Vector3I vector3I2 = new Vector3I(vector3I.x, vector3I.y, vector3I.z);
		Vector3I vector3I3 = new Vector3I(0, 0, 0);
		return (int)((float)((Mathf.Abs(vector3I2.x - vector3I3.x) + Mathf.Abs(vector3I2.y - vector3I3.y) + Mathf.Abs(vector3I2.z - vector3I3.z)) / 2));
	}

	// Token: 0x06008743 RID: 34627 RVA: 0x000F885B File Offset: 0x000F6A5B
	private void CleanUpGrid()
	{
		this.cellContents.Clear();
	}

	// Token: 0x06008744 RID: 34628 RVA: 0x003502C8 File Offset: 0x0034E4C8
	private int GetHexDistance(AxialI a, AxialI b)
	{
		Vector3I vector3I = a.ToCube();
		Vector3I vector3I2 = b.ToCube();
		return Mathf.Max(new int[]
		{
			Mathf.Abs(vector3I.x - vector3I2.x),
			Mathf.Abs(vector3I.y - vector3I2.y),
			Mathf.Abs(vector3I.z - vector3I2.z)
		});
	}

	// Token: 0x06008745 RID: 34629 RVA: 0x00350330 File Offset: 0x0034E530
	public List<ClusterGridEntity> GetEntitiesInRange(AxialI center, int range = 1)
	{
		List<ClusterGridEntity> list = new List<ClusterGridEntity>();
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in this.cellContents)
		{
			if (this.GetHexDistance(keyValuePair.Key, center) <= range)
			{
				list.AddRange(keyValuePair.Value);
			}
		}
		return list;
	}

	// Token: 0x06008746 RID: 34630 RVA: 0x000F8868 File Offset: 0x000F6A68
	public List<ClusterGridEntity> GetEntitiesOnCell(AxialI cell)
	{
		return this.cellContents[cell];
	}

	// Token: 0x06008747 RID: 34631 RVA: 0x000F8876 File Offset: 0x000F6A76
	public bool IsInRange(AxialI a, AxialI b, int range = 1)
	{
		return this.GetHexDistance(a, b) <= range;
	}

	// Token: 0x06008748 RID: 34632 RVA: 0x003503A4 File Offset: 0x0034E5A4
	private void GenerateGrid(int rings)
	{
		this.CleanUpGrid();
		this.numRings = rings;
		for (int i = -rings + 1; i < rings; i++)
		{
			for (int j = -rings + 1; j < rings; j++)
			{
				for (int k = -rings + 1; k < rings; k++)
				{
					if (i + j + k == 0)
					{
						AxialI key = new AxialI(i, j);
						this.cellContents.Add(key, new List<ClusterGridEntity>());
					}
				}
			}
		}
	}

	// Token: 0x06008749 RID: 34633 RVA: 0x0035040C File Offset: 0x0034E60C
	public AxialI GetRandomCellAtEdgeOfUniverse()
	{
		int num = this.numRings - 1;
		List<AxialI> rings = AxialUtil.GetRings(AxialI.ZERO, num, num);
		return rings.ElementAt(UnityEngine.Random.Range(0, rings.Count));
	}

	// Token: 0x0600874A RID: 34634 RVA: 0x00350444 File Offset: 0x0034E644
	public Vector3 GetPosition(ClusterGridEntity entity)
	{
		float r = (float)entity.Location.R;
		float q = (float)entity.Location.Q;
		List<ClusterGridEntity> list = this.cellContents[entity.Location];
		if (list.Count <= 1 || !entity.SpaceOutInSameHex())
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		int num = 0;
		int num2 = 0;
		foreach (ClusterGridEntity clusterGridEntity in list)
		{
			if (entity == clusterGridEntity)
			{
				num = num2;
			}
			if (clusterGridEntity.SpaceOutInSameHex())
			{
				num2++;
			}
		}
		if (list.Count > num2)
		{
			num2 += 5;
			num += 5;
		}
		else if (num2 > 0)
		{
			num2++;
			num++;
		}
		if (num2 == 0 || num2 == 1)
		{
			return AxialUtil.AxialToWorld(r, q);
		}
		float num3 = Mathf.Min(Mathf.Pow((float)num2, 0.5f), 1f) * 0.5f;
		float num4 = Mathf.Pow((float)num / (float)num2, 0.5f);
		float num5 = 0.81f;
		float num6 = Mathf.Pow((float)num2, 0.5f) * num5;
		float f = 6.2831855f * num6 * num4;
		float x = Mathf.Cos(f) * num3 * num4;
		float y = Mathf.Sin(f) * num3 * num4;
		return AxialUtil.AxialToWorld(r, q) + new Vector3(x, y, 0f);
	}

	// Token: 0x0600874B RID: 34635 RVA: 0x003505C8 File Offset: 0x0034E7C8
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector)
	{
		string text;
		return this.GetPath(start, end, destination_selector, out text, false);
	}

	// Token: 0x0600874C RID: 34636 RVA: 0x003505E4 File Offset: 0x0034E7E4
	public List<AxialI> GetPath(AxialI start, AxialI end, ClusterDestinationSelector destination_selector, out string fail_reason, bool dodgeHiddenAsteroids = false)
	{
		ClusterGrid.<>c__DisplayClass41_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.destination_selector = destination_selector;
		CS$<>8__locals1.start = start;
		CS$<>8__locals1.end = end;
		CS$<>8__locals1.dodgeHiddenAsteroids = dodgeHiddenAsteroids;
		fail_reason = null;
		if (!CS$<>8__locals1.destination_selector.canNavigateFogOfWar && !this.IsCellVisible(CS$<>8__locals1.end))
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR;
			return null;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = this.GetVisibleEntityOfLayerAtCell(CS$<>8__locals1.end, EntityLayer.Asteroid);
		if (visibleEntityOfLayerAtCell != null && CS$<>8__locals1.destination_selector.requireLaunchPadOnAsteroidDestination)
		{
			bool flag = false;
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == visibleEntityOfLayerAtCell.Location)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD;
				return null;
			}
		}
		if (visibleEntityOfLayerAtCell == null && CS$<>8__locals1.destination_selector.requireAsteroidDestination)
		{
			fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID;
			return null;
		}
		CS$<>8__locals1.frontier = new HashSet<AxialI>();
		CS$<>8__locals1.visited = new HashSet<AxialI>();
		CS$<>8__locals1.buffer = new HashSet<AxialI>();
		CS$<>8__locals1.cameFrom = new Dictionary<AxialI, AxialI>();
		CS$<>8__locals1.frontier.Add(CS$<>8__locals1.start);
		while (!CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end) && CS$<>8__locals1.frontier.Count > 0)
		{
			this.<GetPath>g__ExpandFrontier|41_0(ref CS$<>8__locals1);
		}
		if (CS$<>8__locals1.frontier.Contains(CS$<>8__locals1.end))
		{
			List<AxialI> list = new List<AxialI>();
			AxialI axialI = CS$<>8__locals1.end;
			while (axialI != CS$<>8__locals1.start)
			{
				list.Add(axialI);
				axialI = CS$<>8__locals1.cameFrom[axialI];
			}
			list.Reverse();
			return list;
		}
		fail_reason = UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_NO_PATH;
		return null;
	}

	// Token: 0x0600874D RID: 34637 RVA: 0x003507D4 File Offset: 0x0034E9D4
	public void GetLocationDescription(AxialI location, out Sprite sprite, out string label, out string sublabel)
	{
		ClusterGridEntity clusterGridEntity = this.GetVisibleEntitiesAtCell(location).Find((ClusterGridEntity x) => x.Layer == EntityLayer.Asteroid);
		ClusterGridEntity visibleEntityOfLayerAtAdjacentCell = this.GetVisibleEntityOfLayerAtAdjacentCell(location, EntityLayer.Asteroid);
		if (clusterGridEntity != null)
		{
			sprite = clusterGridEntity.GetUISprite();
			label = clusterGridEntity.Name;
			WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component.worldType);
			return;
		}
		if (visibleEntityOfLayerAtAdjacentCell != null)
		{
			sprite = visibleEntityOfLayerAtAdjacentCell.GetUISprite();
			label = UI.SPACEDESTINATIONS.ORBIT.NAME_FMT.Replace("{Name}", visibleEntityOfLayerAtAdjacentCell.Name);
			WorldContainer component2 = visibleEntityOfLayerAtAdjacentCell.GetComponent<WorldContainer>();
			sublabel = Strings.Get(component2.worldType);
			return;
		}
		if (this.IsCellVisible(location))
		{
			sprite = Assets.GetSprite("hex_unknown");
			label = UI.SPACEDESTINATIONS.EMPTY_SPACE.NAME;
			sublabel = "";
			return;
		}
		sprite = Assets.GetSprite("unknown_far");
		label = UI.SPACEDESTINATIONS.FOG_OF_WAR_SPACE.NAME;
		sublabel = "";
	}

	// Token: 0x0600874E RID: 34638 RVA: 0x003508E4 File Offset: 0x0034EAE4
	[CompilerGenerated]
	private void <GetPath>g__ExpandFrontier|41_0(ref ClusterGrid.<>c__DisplayClass41_0 A_1)
	{
		A_1.buffer.Clear();
		foreach (AxialI axialI in A_1.frontier)
		{
			foreach (AxialI direction in AxialI.DIRECTIONS)
			{
				AxialI neighbor = this.GetNeighbor(axialI, direction);
				if (!A_1.visited.Contains(neighbor) && this.IsValidCell(neighbor) && (this.IsCellVisible(neighbor) || A_1.destination_selector.canNavigateFogOfWar) && (!this.HasVisibleAsteroidAtCell(neighbor) || !(neighbor != A_1.start) || !(neighbor != A_1.end)) && (!A_1.dodgeHiddenAsteroids || !(ClusterGrid.Instance.GetAsteroidAtCell(neighbor) != null) || ClusterGrid.Instance.GetAsteroidAtCell(neighbor).IsVisibleInFOW == ClusterRevealLevel.Visible || !(neighbor != A_1.start) || !(neighbor != A_1.end)))
				{
					A_1.buffer.Add(neighbor);
					if (!A_1.cameFrom.ContainsKey(neighbor))
					{
						A_1.cameFrom.Add(neighbor, axialI);
					}
				}
			}
			A_1.visited.Add(axialI);
		}
		HashSet<AxialI> frontier = A_1.frontier;
		A_1.frontier = A_1.buffer;
		A_1.buffer = frontier;
	}

	// Token: 0x040065F0 RID: 26096
	public static ClusterGrid Instance;

	// Token: 0x040065F1 RID: 26097
	public const float NodeDistanceScale = 600f;

	// Token: 0x040065F2 RID: 26098
	private const float MAX_OFFSET_RADIUS = 0.5f;

	// Token: 0x040065F3 RID: 26099
	public int numRings;

	// Token: 0x040065F4 RID: 26100
	private ClusterFogOfWarManager.Instance m_fowManager;

	// Token: 0x040065F5 RID: 26101
	private Action<object> m_onClusterLocationChangedDelegate;

	// Token: 0x040065F6 RID: 26102
	public Dictionary<AxialI, List<ClusterGridEntity>> cellContents = new Dictionary<AxialI, List<ClusterGridEntity>>();
}
