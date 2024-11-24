using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200108D RID: 4237
public class ClusterFogOfWarManager : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>
{
	// Token: 0x060056C5 RID: 22213 RVA: 0x00283144 File Offset: 0x00281344
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.Initialize();
		}).EventHandler(GameHashes.DiscoveredWorldsChanged, (ClusterFogOfWarManager.Instance smi) => Game.Instance, delegate(ClusterFogOfWarManager.Instance smi)
		{
			smi.UpdateRevealedCellsFromDiscoveredWorlds();
		});
	}

	// Token: 0x04003CBF RID: 15551
	public const int AUTOMATIC_PEEK_RADIUS = 2;

	// Token: 0x0200108E RID: 4238
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200108F RID: 4239
	public new class Instance : GameStateMachine<ClusterFogOfWarManager, ClusterFogOfWarManager.Instance, IStateMachineTarget, ClusterFogOfWarManager.Def>.GameInstance
	{
		// Token: 0x060056C8 RID: 22216 RVA: 0x000D8AE1 File Offset: 0x000D6CE1
		public Instance(IStateMachineTarget master, ClusterFogOfWarManager.Def def) : base(master, def)
		{
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x000D8AF6 File Offset: 0x000D6CF6
		public void Initialize()
		{
			this.UpdateRevealedCellsFromDiscoveredWorlds();
			this.EnsureRevealedTilesHavePeek();
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x000D8B04 File Offset: 0x000D6D04
		public ClusterRevealLevel GetCellRevealLevel(AxialI location)
		{
			if (this.GetRevealCompleteFraction(location) >= 1f)
			{
				return ClusterRevealLevel.Visible;
			}
			if (this.GetRevealCompleteFraction(location) > 0f)
			{
				return ClusterRevealLevel.Peeked;
			}
			return ClusterRevealLevel.Hidden;
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x000D8B27 File Offset: 0x000D6D27
		public void DEBUG_REVEAL_ENTIRE_MAP()
		{
			this.RevealLocation(AxialI.ZERO, 100);
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x000D8B36 File Offset: 0x000D6D36
		public bool IsLocationRevealed(AxialI location)
		{
			return this.GetRevealCompleteFraction(location) >= 1f;
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x002831CC File Offset: 0x002813CC
		private void EnsureRevealedTilesHavePeek()
		{
			foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
			{
				if (this.IsLocationRevealed(keyValuePair.Key))
				{
					this.PeekLocation(keyValuePair.Key, 2);
				}
			}
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x0028323C File Offset: 0x0028143C
		public void PeekLocation(AxialI location, int radius)
		{
			foreach (AxialI key in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				if (this.m_revealPointsByCell.ContainsKey(key))
				{
					this.m_revealPointsByCell[key] = Mathf.Max(this.m_revealPointsByCell[key], 0.01f);
				}
				else
				{
					this.m_revealPointsByCell[key] = 0.01f;
				}
			}
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x002832CC File Offset: 0x002814CC
		public void RevealLocation(AxialI location, int radius = 0)
		{
			if (ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(location, EntityLayer.Asteroid).Count > 0 || ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null)
			{
				radius = Mathf.Max(radius, 1);
			}
			bool flag = false;
			foreach (AxialI cell in AxialUtil.GetAllPointsWithinRadius(location, radius))
			{
				flag |= this.RevealCellIfValid(cell);
			}
			if (flag)
			{
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x00283370 File Offset: 0x00281570
		public void EarnRevealPointsForLocation(AxialI location, float points)
		{
			global::Debug.Assert(ClusterGrid.Instance.IsValidCell(location), string.Format("EarnRevealPointsForLocation called with invalid location: {0}", location));
			if (this.IsLocationRevealed(location))
			{
				return;
			}
			if (this.m_revealPointsByCell.ContainsKey(location))
			{
				Dictionary<AxialI, float> revealPointsByCell = this.m_revealPointsByCell;
				revealPointsByCell[location] += points;
			}
			else
			{
				this.m_revealPointsByCell[location] = points;
				Game.Instance.Trigger(-1554423969, location);
			}
			if (this.IsLocationRevealed(location))
			{
				this.RevealLocation(location, 0);
				this.PeekLocation(location, 2);
				Game.Instance.Trigger(-1991583975, location);
			}
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x00283420 File Offset: 0x00281620
		public float GetRevealCompleteFraction(AxialI location)
		{
			if (!ClusterGrid.Instance.IsValidCell(location))
			{
				global::Debug.LogError(string.Format("GetRevealCompleteFraction called with invalid location: {0}, {1}", location.r, location.q));
			}
			if (DebugHandler.RevealFogOfWar)
			{
				return 1f;
			}
			float num;
			if (this.m_revealPointsByCell.TryGetValue(location, out num))
			{
				return Mathf.Min(num / ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL, 1f);
			}
			return 0f;
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x000D8B49 File Offset: 0x000D6D49
		private bool RevealCellIfValid(AxialI cell)
		{
			if (!ClusterGrid.Instance.IsValidCell(cell))
			{
				return false;
			}
			if (this.IsLocationRevealed(cell))
			{
				return false;
			}
			this.m_revealPointsByCell[cell] = ROCKETRY.CLUSTER_FOW.POINTS_TO_REVEAL;
			this.PeekLocation(cell, 2);
			return true;
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x00283494 File Offset: 0x00281694
		public bool GetUnrevealedLocationWithinRadius(AxialI center, int radius, out AxialI result)
		{
			for (int i = 0; i <= radius; i++)
			{
				foreach (AxialI axialI in AxialUtil.GetRing(center, i))
				{
					if (ClusterGrid.Instance.IsValidCell(axialI) && !this.IsLocationRevealed(axialI))
					{
						result = axialI;
						return true;
					}
				}
			}
			result = AxialI.ZERO;
			return false;
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x0028351C File Offset: 0x0028171C
		public void UpdateRevealedCellsFromDiscoveredWorlds()
		{
			int radius = DlcManager.IsExpansion1Active() ? 0 : 2;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (worldContainer.IsDiscovered && !DebugHandler.RevealFogOfWar)
				{
					this.RevealLocation(worldContainer.GetComponent<ClusterGridEntity>().Location, radius);
				}
			}
		}

		// Token: 0x04003CC0 RID: 15552
		[Serialize]
		private Dictionary<AxialI, float> m_revealPointsByCell = new Dictionary<AxialI, float>();
	}
}
