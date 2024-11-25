﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitFlow : IConduitFlow
{
		public SolidConduitFlow.SOAInfo GetSOAInfo()
	{
		return this.soaInfo;
	}

				public event System.Action onConduitsRebuilt;

		public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new SolidConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

		public void RemoveConduitUpdater(Action<float> callback)
	{
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			if (this.conduitUpdaters[i].callback == callback)
			{
				this.conduitUpdaters.RemoveAt(i);
				this.dirtyConduitUpdaters = true;
				return;
			}
		}
	}

		public static int FlowBit(SolidConduitFlow.FlowDirection direction)
	{
		return 1 << direction - SolidConduitFlow.FlowDirection.Left;
	}

		public SolidConduitFlow(int num_cells, IUtilityNetworkMgr network_mgr, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.networkMgr = network_mgr;
		this.maskedOverlayLayer = LayerMask.NameToLayer("MaskedOverlay");
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

		public void Initialize(int num_cells)
	{
		this.grid = new SolidConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.pickupableHandle = HandleVector<int>.InvalidHandle;
		}
	}

		private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			FlowUtilityNetwork network = (FlowUtilityNetwork)utilityNetwork;
			this.ScanNetworkSources(network);
		}
		this.RefreshPaths();
	}

		private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		this.soaInfo.Clear(this);
		this.pathList.Clear();
		ObjectLayer layer = ObjectLayer.SolidConduit;
		foreach (int num in root_nodes)
		{
			if (this.replacements.Contains(num))
			{
				this.replacements.Remove(num);
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
				this.grid[num].conduitIdx = conduitIdx;
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		foreach (int num2 in root_nodes)
		{
			UtilityConnections connections = this.networkMgr.GetConnections(num2, true);
			if (connections != (UtilityConnections)0 && this.grid[num2].conduitIdx != -1)
			{
				int conduitIdx2 = this.grid[num2].conduitIdx;
				SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduitIdx2);
				int num3 = num2 - 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					conduitConnections.left = this.grid[num3].conduitIdx;
				}
				num3 = num2 + 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					conduitConnections.right = this.grid[num3].conduitIdx;
				}
				num3 = num2 - Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					conduitConnections.down = this.grid[num3].conduitIdx;
				}
				num3 = num2 + Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					conduitConnections.up = this.grid[num3].conduitIdx;
				}
				this.soaInfo.SetConduitConnections(conduitIdx2, conduitConnections);
			}
		}
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

		public void ScanNetworkSources(FlowUtilityNetwork network)
	{
		if (network == null)
		{
			return;
		}
		for (int i = 0; i < network.sources.Count; i++)
		{
			FlowUtilityNetwork.IItem item = network.sources[i];
			this.path.Clear();
			this.visited.Clear();
			this.FindSinks(i, item.Cell);
		}
	}

		public void RefreshPaths()
	{
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				SolidConduitFlow.Conduit conduit = list[i];
				SolidConduitFlow.Conduit target_conduit = list[i + 1];
				if (conduit.GetTargetFlowDirection(this) == SolidConduitFlow.FlowDirection.None)
				{
					SolidConduitFlow.FlowDirection direction = this.GetDirection(conduit, target_conduit);
					conduit.SetTargetFlowDirection(direction, this);
				}
			}
		}
	}

		private void FindSinks(int source_idx, int cell)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.FindSinksInternal(source_idx, gridNode.conduitIdx);
		}
	}

		private void FindSinksInternal(int source_idx, int conduit_idx)
	{
		if (this.visited.Contains(conduit_idx))
		{
			return;
		}
		this.visited.Add(conduit_idx);
		SolidConduitFlow.Conduit conduit = this.soaInfo.GetConduit(conduit_idx);
		if (conduit.GetPermittedFlowDirections(this) == -1)
		{
			return;
		}
		this.path.Add(conduit);
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)this.networkMgr.GetEndpoint(this.soaInfo.GetCell(conduit_idx));
		if (item != null && item.EndpointType == Endpoint.Sink)
		{
			this.FoundSink(source_idx);
		}
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit_idx);
		if (conduitConnections.down != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.down);
		}
		if (conduitConnections.left != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.left);
		}
		if (conduitConnections.right != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.right);
		}
		if (conduitConnections.up != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.up);
		}
		if (this.path.Count > 0)
		{
			this.path.RemoveAt(this.path.Count - 1);
		}
	}

		private SolidConduitFlow.FlowDirection GetDirection(SolidConduitFlow.Conduit conduit, SolidConduitFlow.Conduit target_conduit)
	{
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Right;
		}
		return SolidConduitFlow.FlowDirection.None;
	}

		private void FoundSink(int source_idx)
	{
		for (int i = 0; i < this.path.Count - 1; i++)
		{
			SolidConduitFlow.FlowDirection direction = this.GetDirection(this.path[i], this.path[i + 1]);
			SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(direction);
			int cellFromDirection = SolidConduitFlow.GetCellFromDirection(this.soaInfo.GetCell(this.path[i].idx), direction2);
			SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(this.path[i].idx, direction2);
			if (i == 0 || (this.path[i].GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0 || (cellFromDirection != this.soaInfo.GetCell(this.path[i - 1].idx) && (this.soaInfo.GetSrcFlowIdx(this.path[i].idx) == source_idx || (conduitFromDirection.GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0)))
			{
				int permittedFlowDirections = this.path[i].GetPermittedFlowDirections(this);
				this.soaInfo.SetSrcFlowIdx(this.path[i].idx, source_idx);
				this.path[i].SetPermittedFlowDirections(permittedFlowDirections | SolidConduitFlow.FlowBit(direction), this);
				this.path[i].SetTargetFlowDirection(direction, this);
			}
		}
		for (int j = 1; j < this.path.Count; j++)
		{
			SolidConduitFlow.FlowDirection direction3 = this.GetDirection(this.path[j], this.path[j - 1]);
			this.soaInfo.SetSrcFlowDirection(this.path[j].idx, direction3);
		}
		List<SolidConduitFlow.Conduit> list = new List<SolidConduitFlow.Conduit>(this.path);
		list.Reverse();
		this.TryAdd(list);
	}

		private void TryAdd(List<SolidConduitFlow.Conduit> new_path)
	{
		Predicate<SolidConduitFlow.Conduit> <>9__0;
		Predicate<SolidConduitFlow.Conduit> <>9__1;
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			if (list.Count >= new_path.Count)
			{
				bool flag = false;
				List<SolidConduitFlow.Conduit> list2 = list;
				Predicate<SolidConduitFlow.Conduit> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[0].idx));
				}
				int num = list2.FindIndex(match);
				List<SolidConduitFlow.Conduit> list3 = list;
				Predicate<SolidConduitFlow.Conduit> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[new_path.Count - 1].idx));
				}
				int num2 = list3.FindIndex(match2);
				if (num != -1 && num2 != -1)
				{
					flag = true;
					int i = num;
					int num3 = 0;
					while (i < num2)
					{
						if (list[i].idx != new_path[num3].idx)
						{
							flag = false;
							break;
						}
						i++;
						num3++;
					}
				}
				if (flag)
				{
					return;
				}
			}
		}
		for (int j = this.pathList.Count - 1; j >= 0; j--)
		{
			if (this.pathList[j].Count <= 0)
			{
				this.pathList.RemoveAt(j);
			}
		}
		for (int k = this.pathList.Count - 1; k >= 0; k--)
		{
			List<SolidConduitFlow.Conduit> old_path = this.pathList[k];
			if (new_path.Count >= old_path.Count)
			{
				bool flag2 = false;
				int num4 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[0].idx);
				int num5 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[old_path.Count - 1].idx);
				if (num4 != -1 && num5 != -1)
				{
					flag2 = true;
					int l = num4;
					int num6 = 0;
					while (l < num5)
					{
						if (new_path[l].idx != old_path[num6].idx)
						{
							flag2 = false;
							break;
						}
						l++;
						num6++;
					}
				}
				if (flag2)
				{
					this.pathList.RemoveAt(k);
				}
			}
		}
		foreach (List<SolidConduitFlow.Conduit> list4 in this.pathList)
		{
			for (int m = new_path.Count - 1; m >= 0; m--)
			{
				SolidConduitFlow.Conduit new_conduit = new_path[m];
				if (list4.FindIndex((SolidConduitFlow.Conduit t) => t.idx == new_conduit.idx) != -1 && Mathf.IsPowerOfTwo(this.soaInfo.GetPermittedFlowDirections(new_conduit.idx)))
				{
					new_path.RemoveAt(m);
				}
			}
		}
		this.pathList.Add(new_path);
	}

		public SolidConduitFlow.ConduitContents GetContents(int cell)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		return contents;
	}

		private void SetContents(int cell, SolidConduitFlow.ConduitContents contents)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

		public void SetContents(int cell, Pickupable pickupable)
	{
		SolidConduitFlow.ConduitContents contents = new SolidConduitFlow.ConduitContents
		{
			pickupableHandle = HandleVector<int>.InvalidHandle
		};
		if (pickupable != null)
		{
			KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
			SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
			{
				kbac = component,
				pickupable = pickupable
			};
			contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
			KBatchedAnimController component2 = pickupable.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			pickupable.Trigger(856640610, true);
		}
		this.SetContents(cell, contents);
	}

		public static int GetCellFromDirection(int cell, SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return Grid.CellLeft(cell);
		case SolidConduitFlow.FlowDirection.Right:
			return Grid.CellRight(cell);
		case SolidConduitFlow.FlowDirection.Up:
			return Grid.CellAbove(cell);
		case SolidConduitFlow.FlowDirection.Down:
			return Grid.CellBelow(cell);
		default:
			return -1;
		}
	}

		public static SolidConduitFlow.FlowDirection InverseFlow(SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return SolidConduitFlow.FlowDirection.Right;
		case SolidConduitFlow.FlowDirection.Right:
			return SolidConduitFlow.FlowDirection.Left;
		case SolidConduitFlow.FlowDirection.Up:
			return SolidConduitFlow.FlowDirection.Down;
		case SolidConduitFlow.FlowDirection.Down:
			return SolidConduitFlow.FlowDirection.Up;
		default:
			return SolidConduitFlow.FlowDirection.None;
		}
	}

		public void Sim200ms(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		this.elapsedTime += dt;
		if (this.elapsedTime < 1f)
		{
			return;
		}
		float obj = 1f;
		this.elapsedTime -= 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			foreach (SolidConduitFlow.Conduit conduit in list)
			{
				this.UpdateConduit(conduit);
			}
		}
		this.soaInfo.UpdateFlowDirection(this);
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((SolidConduitFlow.ConduitUpdater a, SolidConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

		public void RenderEveryTick(float dt)
	{
		for (int i = 0; i < this.GetSOAInfo().NumEntries; i++)
		{
			SolidConduitFlow.Conduit conduit = this.GetSOAInfo().GetConduit(i);
			SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this);
			if (lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None)
			{
				int cell = conduit.GetCell(this);
				int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
				SolidConduitFlow.ConduitContents contents = this.GetContents(cellFromDirection);
				if (contents.pickupableHandle.IsValid())
				{
					Vector3 a = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					Vector3 b = Grid.CellToPosCCC(cellFromDirection, Grid.SceneLayer.SolidConduitContents);
					Vector3 position = Vector3.Lerp(a, b, this.ContinuousLerpPercent);
					Pickupable pickupable = this.GetPickupable(contents.pickupableHandle);
					if (pickupable != null)
					{
						pickupable.transform.SetPosition(position);
					}
				}
			}
		}
	}

		private void UpdateConduit(SolidConduitFlow.Conduit conduit)
	{
		if (this.soaInfo.GetUpdated(conduit.idx))
		{
			return;
		}
		if (this.soaInfo.GetSrcFlowDirection(conduit.idx) == SolidConduitFlow.FlowDirection.None)
		{
			this.soaInfo.SetSrcFlowDirection(conduit.idx, conduit.GetNextFlowSource(this));
		}
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		if (!contents.pickupableHandle.IsValid())
		{
			return;
		}
		SolidConduitFlow.FlowDirection targetFlowDirection = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, targetFlowDirection);
		if (conduitFromDirection.idx == -1)
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		int cell2 = this.soaInfo.GetCell(conduitFromDirection.idx);
		SolidConduitFlow.ConduitContents contents2 = this.grid[cell2].contents;
		if (contents2.pickupableHandle.IsValid())
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		if ((this.soaInfo.GetPermittedFlowDirections(conduit.idx) & SolidConduitFlow.FlowBit(targetFlowDirection)) != 0)
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx));
				if (conduitFromDirection2.idx == conduit.idx)
				{
					flag = true;
					break;
				}
				if (conduitFromDirection2.idx != -1)
				{
					int cell3 = this.soaInfo.GetCell(conduitFromDirection2.idx);
					SolidConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
					if (contents3.pickupableHandle.IsValid())
					{
						break;
					}
				}
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
			if (flag && !contents2.pickupableHandle.IsValid())
			{
				SolidConduitFlow.ConduitContents contents4 = this.RemoveFromGrid(conduit);
				this.AddToGrid(cell2, contents4);
				this.soaInfo.SetLastFlowInfo(conduit.idx, this.soaInfo.GetTargetFlowDirection(conduit.idx));
				this.soaInfo.SetUpdated(conduitFromDirection.idx, true);
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
		}
		this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
	}

			public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

			public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

		private void AddToGrid(int cell_idx, SolidConduitFlow.ConduitContents contents)
	{
		this.grid[cell_idx].contents = contents;
	}

		private SolidConduitFlow.ConduitContents RemoveFromGrid(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.ConduitContents contents2 = SolidConduitFlow.ConduitContents.EmptyContents();
		this.grid[cell].contents = contents2;
		return contents;
	}

		public void AddPickupable(int cell_idx, Pickupable pickupable)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			global::Debug.LogWarning("No conduit in cell: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		SolidConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.pickupableHandle.IsValid())
		{
			global::Debug.LogWarning("Conduit already full: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
		SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
		{
			kbac = component,
			pickupable = pickupable
		};
		contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
		if (this.viewingConduits)
		{
			this.ApplyOverlayVisualization(component);
		}
		if (pickupable.storage)
		{
			pickupable.storage.Remove(pickupable.gameObject, true);
		}
		pickupable.Trigger(856640610, true);
		this.SetContents(cell_idx, contents);
	}

		public Pickupable RemovePickupable(int cell_idx)
	{
		Pickupable pickupable = null;
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell_idx);
		if (conduit.idx != -1)
		{
			SolidConduitFlow.ConduitContents conduitContents = this.RemoveFromGrid(conduit);
			if (conduitContents.pickupableHandle.IsValid())
			{
				SolidConduitFlow.StoredInfo data = this.conveyorPickupables.GetData(conduitContents.pickupableHandle);
				this.ClearOverlayVisualization(data.kbac);
				pickupable = data.pickupable;
				if (pickupable)
				{
					pickupable.Trigger(856640610, false);
				}
				this.freedHandles.Add(conduitContents.pickupableHandle);
			}
		}
		return pickupable;
	}

		public int GetPermittedFlow(int cell)
	{
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return 0;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

		public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

		public SolidConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return SolidConduitFlow.Conduit.Invalid();
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

		private void DumpPipeContents(int cell)
	{
		Pickupable pickupable = this.RemovePickupable(cell);
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

		private void DumpPickupable(Pickupable pickupable)
	{
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

		public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell);
	}

		public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

		public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		SolidConduitFlow.ConduitContents contents = SolidConduitFlow.ConduitContents.EmptyContents();
		this.SetContents(cell, contents);
	}

		public UtilityNetwork GetNetwork(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

		public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

		public bool IsConduitFull(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.pickupableHandle.IsValid();
	}

		public bool IsConduitEmpty(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return !contents.pickupableHandle.IsValid();
	}

		public void Initialize()
	{
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			OverlayScreen instance2 = OverlayScreen.Instance;
			instance2.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance2.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		}
	}

		private void OnOverlayChanged(HashedString mode)
	{
		bool flag = mode == OverlayModes.SolidConveyor.ID;
		if (flag == this.viewingConduits)
		{
			return;
		}
		this.viewingConduits = flag;
		int layer = this.viewingConduits ? this.maskedOverlayLayer : Game.PickupableLayer;
		Color32 tintColour = this.viewingConduits ? SolidConduitFlow.OverlayColour : SolidConduitFlow.NormalColour;
		List<SolidConduitFlow.StoredInfo> dataList = this.conveyorPickupables.GetDataList();
		for (int i = 0; i < dataList.Count; i++)
		{
			SolidConduitFlow.StoredInfo storedInfo = dataList[i];
			if (storedInfo.kbac != null)
			{
				storedInfo.kbac.SetLayer(layer);
				storedInfo.kbac.TintColour = tintColour;
			}
		}
	}

		private void ApplyOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(this.maskedOverlayLayer);
		kbac.TintColour = SolidConduitFlow.OverlayColour;
	}

		private void ClearOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(Game.PickupableLayer);
		kbac.TintColour = SolidConduitFlow.NormalColour;
	}

		public Pickupable GetPickupable(HandleVector<int>.Handle h)
	{
		Pickupable result = null;
		if (h.IsValid())
		{
			result = this.conveyorPickupables.GetData(h).pickupable;
		}
		return result;
	}

		public const float MAX_SOLID_MASS = 20f;

		public const float TickRate = 1f;

		public const float WaitTime = 1f;

		private float elapsedTime;

		private float lastUpdateTime = float.NegativeInfinity;

		private KCompactedVector<SolidConduitFlow.StoredInfo> conveyorPickupables = new KCompactedVector<SolidConduitFlow.StoredInfo>(0);

		private List<HandleVector<int>.Handle> freedHandles = new List<HandleVector<int>.Handle>();

		private SolidConduitFlow.SOAInfo soaInfo = new SolidConduitFlow.SOAInfo();

		private bool dirtyConduitUpdaters;

		private List<SolidConduitFlow.ConduitUpdater> conduitUpdaters = new List<SolidConduitFlow.ConduitUpdater>();

		private SolidConduitFlow.GridNode[] grid;

		public IUtilityNetworkMgr networkMgr;

		private HashSet<int> visited = new HashSet<int>();

		private HashSet<int> replacements = new HashSet<int>();

		private List<SolidConduitFlow.Conduit> path = new List<SolidConduitFlow.Conduit>();

		private List<List<SolidConduitFlow.Conduit>> pathList = new List<List<SolidConduitFlow.Conduit>>();

		public static readonly SolidConduitFlow.ConduitContents emptyContents = new SolidConduitFlow.ConduitContents
	{
		pickupableHandle = HandleVector<int>.InvalidHandle
	};

		private int maskedOverlayLayer;

		private bool viewingConduits;

		private static readonly Color32 NormalColour = Color.white;

		private static readonly Color32 OverlayColour = new Color(0.25f, 0.25f, 0.25f, 0f);

		private struct StoredInfo
	{
				public KBatchedAnimController kbac;

				public Pickupable pickupable;
	}

		public class SOAInfo
	{
						public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

						public List<int> Cells
		{
			get
			{
				return this.cells;
			}
		}

				public int AddConduit(SolidConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			SolidConduitFlow.Conduit item = new SolidConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new SolidConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(new SolidConduitFlow.ConduitFlowInfo
			{
				direction = SolidConduitFlow.FlowDirection.None
			});
			this.cells.Add(cell);
			this.updated.Add(false);
			this.diseaseContentsVisible.Add(false);
			this.conduitGOs.Add(conduit_go);
			this.srcFlowIdx.Add(-1);
			this.permittedFlowDirections.Add(0);
			this.srcFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			this.targetFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			return count;
		}

				public void Clear(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.ForcePermanentDiseaseContainer(i, false);
				int num = this.cells[i];
				SolidConduitFlow.ConduitContents contents = manager.grid[num].contents;
				manager.grid[num].contents = contents;
				manager.grid[num].conduitIdx = -1;
			}
			this.cells.Clear();
			this.updated.Clear();
			this.diseaseContentsVisible.Clear();
			this.srcFlowIdx.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

				public SolidConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

				public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

				public SolidConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

				public void SetConduitConnections(int idx, SolidConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

				public void ForcePermanentDiseaseContainer(int idx, bool force_on)
		{
			if (this.diseaseContentsVisible[idx] != force_on)
			{
				this.diseaseContentsVisible[idx] = force_on;
				GameObject gameObject = this.conduitGOs[idx];
				if (gameObject == null)
				{
					return;
				}
				gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
			}
		}

				public SolidConduitFlow.Conduit GetConduitFromDirection(int idx, SolidConduitFlow.FlowDirection direction)
		{
			SolidConduitFlow.Conduit result = SolidConduitFlow.Conduit.Invalid();
			SolidConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case SolidConduitFlow.FlowDirection.Left:
				result = ((conduitConnections.left != -1) ? this.conduits[conduitConnections.left] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Right:
				result = ((conduitConnections.right != -1) ? this.conduits[conduitConnections.right] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Up:
				result = ((conduitConnections.up != -1) ? this.conduits[conduitConnections.up] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Down:
				result = ((conduitConnections.down != -1) ? this.conduits[conduitConnections.down] : SolidConduitFlow.Conduit.Invalid());
				break;
			}
			return result;
		}

				public void BeginFrame(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.updated[i] = false;
				SolidConduitFlow.ConduitContents contents = this.conduits[i].GetContents(manager);
				this.initialContents[i] = contents;
				this.lastFlowInfo[i] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				int num = this.cells[i];
				manager.grid[num].contents = contents;
			}
			for (int j = 0; j < manager.freedHandles.Count; j++)
			{
				HandleVector<int>.Handle handle = manager.freedHandles[j];
				manager.conveyorPickupables.Free(handle);
			}
			manager.freedHandles.Clear();
		}

				public void EndFrame(SolidConduitFlow manager)
		{
		}

				public void UpdateFlowDirection(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				SolidConduitFlow.Conduit conduit = this.conduits[i];
				if (!this.updated[i])
				{
					int cell = conduit.GetCell(manager);
					SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
					if (!contents.pickupableHandle.IsValid())
					{
						this.srcFlowDirections[conduit.idx] = conduit.GetNextFlowSource(manager);
					}
				}
			}
		}

				public void MarkConduitEmpty(int idx, SolidConduitFlow manager)
		{
			if (this.lastFlowInfo[idx].direction != SolidConduitFlow.FlowDirection.None)
			{
				this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				SolidConduitFlow.Conduit conduit = this.conduits[idx];
				this.targetFlowDirections[idx] = conduit.GetNextFlowTarget(manager);
				int num = this.cells[idx];
				manager.grid[num].contents = SolidConduitFlow.ConduitContents.EmptyContents();
			}
		}

				public void SetLastFlowInfo(int idx, SolidConduitFlow.FlowDirection direction)
		{
			this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
			{
				direction = direction
			};
		}

				public SolidConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

				public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

				public int GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

				public void SetPermittedFlowDirections(int idx, int permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

				public SolidConduitFlow.FlowDirection GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

				public void SetTargetFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

				public int GetSrcFlowIdx(int idx)
		{
			return this.srcFlowIdx[idx];
		}

				public void SetSrcFlowIdx(int idx, int new_src_idx)
		{
			this.srcFlowIdx[idx] = new_src_idx;
		}

				public SolidConduitFlow.FlowDirection GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

				public void SetSrcFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

				public int GetCell(int idx)
		{
			return this.cells[idx];
		}

				public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

				public bool GetUpdated(int idx)
		{
			return this.updated[idx];
		}

				public void SetUpdated(int idx, bool is_updated)
		{
			this.updated[idx] = is_updated;
		}

				private List<SolidConduitFlow.Conduit> conduits = new List<SolidConduitFlow.Conduit>();

				private List<SolidConduitFlow.ConduitConnections> conduitConnections = new List<SolidConduitFlow.ConduitConnections>();

				private List<SolidConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<SolidConduitFlow.ConduitFlowInfo>();

				private List<SolidConduitFlow.ConduitContents> initialContents = new List<SolidConduitFlow.ConduitContents>();

				private List<GameObject> conduitGOs = new List<GameObject>();

				private List<bool> diseaseContentsVisible = new List<bool>();

				private List<bool> updated = new List<bool>();

				private List<int> cells = new List<int>();

				private List<int> permittedFlowDirections = new List<int>();

				private List<int> srcFlowIdx = new List<int>();

				private List<SolidConduitFlow.FlowDirection> srcFlowDirections = new List<SolidConduitFlow.FlowDirection>();

				private List<SolidConduitFlow.FlowDirection> targetFlowDirections = new List<SolidConduitFlow.FlowDirection>();
	}

		[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
				public ConduitFlowPriority priority;

				public Action<float> callback;
	}

		public struct GridNode
	{
				public int conduitIdx;

				public SolidConduitFlow.ConduitContents contents;
	}

		public enum FlowDirection
	{
				Blocked = -1,
				None,
				Left,
				Right,
				Up,
				Down,
				Num
	}

		public struct ConduitConnections
	{
				public int left;

				public int right;

				public int up;

				public int down;
	}

		public struct ConduitFlowInfo
	{
				public SolidConduitFlow.FlowDirection direction;
	}

		[Serializable]
	public struct Conduit : IEquatable<SolidConduitFlow.Conduit>
	{
				public static SolidConduitFlow.Conduit Invalid()
		{
			return new SolidConduitFlow.Conduit(-1);
		}

				public Conduit(int idx)
		{
			this.idx = idx;
		}

				public int GetPermittedFlowDirections(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

				public void SetPermittedFlowDirections(int permitted, SolidConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

				public SolidConduitFlow.FlowDirection GetTargetFlowDirection(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

				public void SetTargetFlowDirection(SolidConduitFlow.FlowDirection directions, SolidConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

				public SolidConduitFlow.ConduitContents GetContents(SolidConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			return manager.grid[cell].contents;
		}

				public void SetContents(SolidConduitFlow manager, SolidConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = manager.GetPickupable(contents.pickupableHandle);
				if (pickupable != null)
				{
					pickupable.transform.parent = null;
					Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					pickupable.transform.SetPosition(position);
					KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
					component.GetBatchInstanceData().ClearOverrideTransformMatrix();
					component.SetSceneLayer(Grid.SceneLayer.SolidConduitContents);
				}
			}
		}

				public SolidConduitFlow.FlowDirection GetNextFlowSource(SolidConduitFlow manager)
		{
			if (manager.soaInfo.GetPermittedFlowDirections(this.idx) == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			SolidConduitFlow.FlowDirection flowDirection = manager.soaInfo.GetSrcFlowDirection(this.idx);
			if (flowDirection == SolidConduitFlow.FlowDirection.None)
			{
				flowDirection = SolidConduitFlow.FlowDirection.Down;
			}
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.FlowDirection flowDirection2 = (flowDirection + i - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.Conduit conduitFromDirection = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection2);
				if (conduitFromDirection.idx != -1)
				{
					SolidConduitFlow.ConduitContents contents = manager.grid[conduitFromDirection.GetCell(manager)].contents;
					if (contents.pickupableHandle.IsValid())
					{
						int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection.idx);
						if (permittedFlowDirections != -1)
						{
							SolidConduitFlow.FlowDirection direction = SolidConduitFlow.InverseFlow(flowDirection2);
							if (manager.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit(direction)) != 0)
							{
								return flowDirection2;
							}
						}
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				SolidConduitFlow.FlowDirection flowDirection3 = (manager.soaInfo.GetTargetFlowDirection(this.idx) + j - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(flowDirection3);
				SolidConduitFlow.Conduit conduitFromDirection2 = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection3);
				if (conduitFromDirection2.idx != -1)
				{
					int permittedFlowDirections2 = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection2.idx);
					if (permittedFlowDirections2 != -1 && (permittedFlowDirections2 & SolidConduitFlow.FlowBit(direction2)) != 0)
					{
						return flowDirection3;
					}
				}
			}
			return SolidConduitFlow.FlowDirection.None;
		}

				public SolidConduitFlow.FlowDirection GetNextFlowTarget(SolidConduitFlow manager)
		{
			int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(this.idx);
			if (permittedFlowDirections == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			for (int i = 0; i < 5; i++)
			{
				int num = (manager.soaInfo.GetTargetFlowDirection(this.idx) + i - SolidConduitFlow.FlowDirection.Left + 1) % 5 + 1;
				if (manager.soaInfo.GetConduitFromDirection(this.idx, (SolidConduitFlow.FlowDirection)num).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit((SolidConduitFlow.FlowDirection)num)) != 0)
				{
					return (SolidConduitFlow.FlowDirection)num;
				}
			}
			return SolidConduitFlow.FlowDirection.Blocked;
		}

				public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

				public SolidConduitFlow.ConduitContents GetInitialContents(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

				public int GetCell(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

				public bool Equals(SolidConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

				public int idx;
	}

		[DebuggerDisplay("{pickupable}")]
	public struct ConduitContents
	{
				public ConduitContents(HandleVector<int>.Handle pickupable_handle)
		{
			this.pickupableHandle = pickupable_handle;
		}

				public static SolidConduitFlow.ConduitContents EmptyContents()
		{
			return new SolidConduitFlow.ConduitContents
			{
				pickupableHandle = HandleVector<int>.InvalidHandle
			};
		}

				public HandleVector<int>.Handle pickupableHandle;
	}
}
