using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020010DF RID: 4319
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow
{
	// Token: 0x14000016 RID: 22
	// (add) Token: 0x060058AB RID: 22699 RVA: 0x0028CA4C File Offset: 0x0028AC4C
	// (remove) Token: 0x060058AC RID: 22700 RVA: 0x0028CA84 File Offset: 0x0028AC84
	public event System.Action onConduitsRebuilt;

	// Token: 0x060058AD RID: 22701 RVA: 0x0028CABC File Offset: 0x0028ACBC
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new ConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x060058AE RID: 22702 RVA: 0x0028CAF4 File Offset: 0x0028ACF4
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

	// Token: 0x060058AF RID: 22703 RVA: 0x000D9CEB File Offset: 0x000D7EEB
	private static ConduitFlow.FlowDirections ComputeFlowDirection(int index)
	{
		return (ConduitFlow.FlowDirections)(1 << index);
	}

	// Token: 0x060058B0 RID: 22704 RVA: 0x0028CB44 File Offset: 0x0028AD44
	private static ConduitFlow.FlowDirections ComputeNextFlowDirection(ConduitFlow.FlowDirections current)
	{
		switch (current)
		{
		case ConduitFlow.FlowDirections.None:
		case ConduitFlow.FlowDirections.Up:
			return ConduitFlow.FlowDirections.Down;
		case ConduitFlow.FlowDirections.Down:
			return ConduitFlow.FlowDirections.Left;
		case ConduitFlow.FlowDirections.Left:
			return ConduitFlow.FlowDirections.Right;
		case ConduitFlow.FlowDirections.Right:
			return ConduitFlow.FlowDirections.Up;
		}
		global::Debug.Assert(false, "multiple bits are set in 'FlowDirections'...can't compute next direction");
		return ConduitFlow.FlowDirections.Down;
	}

	// Token: 0x060058B1 RID: 22705 RVA: 0x000D9CF4 File Offset: 0x000D7EF4
	public static ConduitFlow.FlowDirections Invert(ConduitFlow.FlowDirections directions)
	{
		return ConduitFlow.FlowDirections.All & ~directions;
	}

	// Token: 0x060058B2 RID: 22706 RVA: 0x0028CB94 File Offset: 0x0028AD94
	public static ConduitFlow.FlowDirections Opposite(ConduitFlow.FlowDirections directions)
	{
		ConduitFlow.FlowDirections result = ConduitFlow.FlowDirections.None;
		if ((directions & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Right;
		}
		else if ((directions & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Left;
		}
		else if ((directions & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Down;
		}
		else if ((directions & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Up;
		}
		return result;
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x0028CBC8 File Offset: 0x0028ADC8
	public ConduitFlow(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, float max_conduit_mass, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.conduitType = conduit_type;
		this.networkMgr = network_mgr;
		this.MaxMass = max_conduit_mass;
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x0028CC78 File Offset: 0x0028AE78
	public void Initialize(int num_cells)
	{
		this.grid = new ConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.element = SimHashes.Vacuum;
			this.grid[i].contents.diseaseIdx = byte.MaxValue;
		}
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x0028CCE8 File Offset: 0x0028AEE8
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		int count = this.networks.Count - networks.Count;
		if (0 < this.networks.Count - networks.Count)
		{
			this.networks.RemoveRange(networks.Count, count);
		}
		global::Debug.Assert(this.networks.Count <= networks.Count);
		for (int num = 0; num != networks.Count; num++)
		{
			if (num < this.networks.Count)
			{
				this.networks[num] = new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = this.networks[num].cells
				};
				this.networks[num].cells.Clear();
			}
			else
			{
				this.networks.Add(new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = new List<int>()
				});
			}
		}
		this.build_network_job.Reset(this);
		foreach (ConduitFlow.Network network in this.networks)
		{
			this.build_network_job.Add(new ConduitFlow.BuildNetworkTask(network, this.soaInfo.NumEntries));
		}
		GlobalJobManager.Run(this.build_network_job);
		for (int num2 = 0; num2 != this.build_network_job.Count; num2++)
		{
			this.build_network_job.GetWorkItem(num2).Finish();
		}
	}

	// Token: 0x060058B6 RID: 22710 RVA: 0x0028CEA8 File Offset: 0x0028B0A8
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		ConduitFlow.ConnectContext connectContext = new ConduitFlow.ConnectContext(this);
		this.soaInfo.Clear(this);
		this.replacements.ExceptWith(root_nodes);
		ObjectLayer layer = (this.conduitType == ConduitType.Gas) ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
		foreach (int num in root_nodes)
		{
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				global::Conduit component = gameObject.GetComponent<global::Conduit>();
				if (!(component != null) || !component.IsDisconnected())
				{
					int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
					this.grid[num].conduitIdx = conduitIdx;
					connectContext.cells.Add(num);
				}
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		this.connect_job.Reset(connectContext);
		int num2 = 256;
		for (int i = 0; i < connectContext.cells.Count; i += num2)
		{
			this.connect_job.Add(new ConduitFlow.ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));
		}
		GlobalJobManager.Run(this.connect_job);
		connectContext.Finish();
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x0028D010 File Offset: 0x0028B210
	private ConduitFlow.FlowDirections GetDirection(ConduitFlow.Conduit conduit, ConduitFlow.Conduit target_conduit)
	{
		global::Debug.Assert(conduit.idx != -1);
		global::Debug.Assert(target_conduit.idx != -1);
		ConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Right;
		}
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x0028D094 File Offset: 0x0028B294
	public int ComputeUpdateOrder(int cell)
	{
		foreach (ConduitFlow.Network network in this.networks)
		{
			int num = network.cells.IndexOf(cell);
			if (num != -1)
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x0028D0F8 File Offset: 0x0028B2F8
	public ConduitFlow.ConduitContents GetContents(int cell)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell].contents;
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));
		}
		return contents;
	}

	// Token: 0x060058BA RID: 22714 RVA: 0x0028D180 File Offset: 0x0028B380
	public void SetContents(int cell, ConduitFlow.ConduitContents contents)
	{
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x060058BB RID: 22715 RVA: 0x000D9CFC File Offset: 0x000D7EFC
	public static int GetCellFromDirection(int cell, ConduitFlow.FlowDirections direction)
	{
		switch (direction)
		{
		case ConduitFlow.FlowDirections.Down:
			return Grid.CellBelow(cell);
		case ConduitFlow.FlowDirections.Left:
			return Grid.CellLeft(cell);
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return Grid.CellRight(cell);
		default:
			if (direction == ConduitFlow.FlowDirections.Up)
			{
				return Grid.CellAbove(cell);
			}
			break;
		}
		return -1;
	}

	// Token: 0x060058BC RID: 22716 RVA: 0x0028D1D4 File Offset: 0x0028B3D4
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
		this.elapsedTime -= 1f;
		float obj = 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.PooledList pooledList = ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.Allocate();
		pooledList.Capacity = Mathf.Max(pooledList.Capacity, this.networks.Count);
		foreach (ConduitFlow.Network network in this.networks)
		{
			pooledList.Add(new ConduitFlow.UpdateNetworkTask(network));
		}
		int num = 0;
		while (num != 4 && pooledList.Count != 0)
		{
			this.update_networks_job.Reset(this);
			foreach (ConduitFlow.UpdateNetworkTask work_item in pooledList)
			{
				this.update_networks_job.Add(work_item);
			}
			GlobalJobManager.Run(this.update_networks_job);
			pooledList.Clear();
			for (int num2 = 0; num2 != this.update_networks_job.Count; num2++)
			{
				ConduitFlow.UpdateNetworkTask workItem = this.update_networks_job.GetWorkItem(num2);
				if (workItem.continue_updating && num != 3)
				{
					pooledList.Add(workItem);
				}
				else
				{
					workItem.Finish(this);
				}
			}
			num++;
		}
		pooledList.Recycle();
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((ConduitFlow.ConduitUpdater a, ConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x0028D3E4 File Offset: 0x0028B5E4
	private float ComputeMovableMass(ConduitFlow.GridNode grid_node)
	{
		ConduitFlow.ConduitContents contents = grid_node.contents;
		if (contents.element == SimHashes.Vacuum)
		{
			return 0f;
		}
		return contents.movable_mass;
	}

	// Token: 0x060058BE RID: 22718 RVA: 0x0028D414 File Offset: 0x0028B614
	private bool UpdateConduit(ConduitFlow.Conduit conduit)
	{
		bool result = false;
		int cell = this.soaInfo.GetCell(conduit.idx);
		ConduitFlow.GridNode gridNode = this.grid[cell];
		float num = this.ComputeMovableMass(gridNode);
		ConduitFlow.FlowDirections permittedFlowDirections = this.soaInfo.GetPermittedFlowDirections(conduit.idx);
		ConduitFlow.FlowDirections flowDirections = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		if (num <= 0f)
		{
			for (int num2 = 0; num2 != 4; num2++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection.idx != -1);
					if ((this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None)
					{
						this.soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
					}
				}
			}
		}
		else
		{
			for (int num3 = 0; num3 != 4; num3++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection2.idx != -1);
					ConduitFlow.FlowDirections srcFlowDirection = this.soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
					bool flag = (srcFlowDirection & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None;
					if (srcFlowDirection != ConduitFlow.FlowDirections.None && !flag)
					{
						result = true;
					}
					else
					{
						int cell2 = this.soaInfo.GetCell(conduitFromDirection2.idx);
						global::Debug.Assert(cell2 != -1);
						ConduitFlow.ConduitContents contents = this.grid[cell2].contents;
						bool flag2 = contents.element == SimHashes.Vacuum || contents.element == gridNode.contents.element;
						float effectiveCapacity = contents.GetEffectiveCapacity(this.MaxMass);
						bool flag3 = flag2 && effectiveCapacity > 0f;
						float num4 = Mathf.Min(num, effectiveCapacity);
						if (flag && flag3)
						{
							this.soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
						}
						if (num4 > 0f && flag3)
						{
							this.soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
							global::Debug.Assert(gridNode.contents.temperature > 0f);
							contents.temperature = GameUtil.GetFinalTemperature(gridNode.contents.temperature, num4, contents.temperature, contents.mass);
							contents.AddMass(num4);
							contents.element = gridNode.contents.element;
							int num5 = (int)(num4 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
							if (num5 != 0)
							{
								SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx, num5, contents.diseaseIdx, contents.diseaseCount);
								contents.diseaseIdx = diseaseInfo.idx;
								contents.diseaseCount = diseaseInfo.count;
							}
							this.grid[cell2].contents = contents;
							global::Debug.Assert(num4 <= gridNode.contents.mass);
							float num6 = gridNode.contents.mass - num4;
							num -= num4;
							if (num6 <= 0f)
							{
								global::Debug.Assert(num <= 0f);
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
								gridNode.contents = ConduitFlow.ConduitContents.Empty;
							}
							else
							{
								int num7 = (int)(num6 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
								global::Debug.Assert(num7 >= 0);
								ConduitFlow.ConduitContents contents2 = gridNode.contents;
								contents2.RemoveMass(num6);
								contents2.diseaseCount -= num7;
								gridNode.contents.RemoveMass(num4);
								gridNode.contents.diseaseCount = num7;
								if (num7 == 0)
								{
									gridNode.contents.diseaseIdx = byte.MaxValue;
								}
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
							}
							this.grid[cell].contents = gridNode.contents;
							result = (0f < this.ComputeMovableMass(gridNode));
							break;
						}
					}
				}
			}
		}
		ConduitFlow.FlowDirections srcFlowDirection2 = this.soaInfo.GetSrcFlowDirection(conduit.idx);
		ConduitFlow.FlowDirections pullDirection = this.soaInfo.GetPullDirection(conduit.idx);
		if (srcFlowDirection2 == ConduitFlow.FlowDirections.None || (ConduitFlow.Opposite(srcFlowDirection2) & pullDirection) != ConduitFlow.FlowDirections.None)
		{
			this.soaInfo.SetPullDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			this.soaInfo.SetSrcFlowDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			for (int num8 = 0; num8 != 2; num8++)
			{
				ConduitFlow.FlowDirections flowDirections2 = srcFlowDirection2;
				for (int num9 = 0; num9 != 4; num9++)
				{
					flowDirections2 = ConduitFlow.ComputeNextFlowDirection(flowDirections2);
					ConduitFlow.Conduit conduitFromDirection3 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
					if (conduitFromDirection3.idx != -1 && (this.soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & ConduitFlow.Opposite(flowDirections2)) != ConduitFlow.FlowDirections.None)
					{
						int cell3 = this.soaInfo.GetCell(conduitFromDirection3.idx);
						ConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
						float num10 = (num8 == 0) ? contents3.movable_mass : contents3.mass;
						if (0f < num10)
						{
							this.soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
							break;
						}
					}
				}
				if (this.soaInfo.GetSrcFlowDirection(conduit.idx) != ConduitFlow.FlowDirections.None)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x060058BF RID: 22719 RVA: 0x000D9D39 File Offset: 0x000D7F39
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x060058C0 RID: 22720 RVA: 0x000D9D52 File Offset: 0x000D7F52
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x060058C1 RID: 22721 RVA: 0x000D9D65 File Offset: 0x000D7F65
	public float GetAmountAllowedForMerging(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massDesiredtoBeMoved)
	{
		return Mathf.Min(massDesiredtoBeMoved, this.MaxMass - to.mass);
	}

	// Token: 0x060058C2 RID: 22722 RVA: 0x000D9D7B File Offset: 0x000D7F7B
	public bool CanMergeContents(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massToMove)
	{
		return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) && this.GetAmountAllowedForMerging(from, to, massToMove) > 0f;
	}

	// Token: 0x060058C3 RID: 22723 RVA: 0x0028D974 File Offset: 0x0028BB74
	public float AddElement(int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			return 0f;
		}
		ConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, this.MaxMass - contents.mass);
		float num2 = num / mass;
		if (num <= 0f)
		{
			return 0f;
		}
		contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
		contents.AddMass(num);
		contents.element = element;
		contents.ConsolidateMass();
		int num3 = (int)(num2 * (float)disease_count);
		if (num3 > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);
			contents.diseaseIdx = diseaseInfo.idx;
			contents.diseaseCount = diseaseInfo.count;
		}
		this.SetContents(cell_idx, contents);
		return num;
	}

	// Token: 0x060058C4 RID: 22724 RVA: 0x0028DA74 File Offset: 0x0028BC74
	public ConduitFlow.ConduitContents RemoveElement(int cell, float delta)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.ConduitContents.Empty;
		}
		return this.RemoveElement(conduit, delta);
	}

	// Token: 0x060058C5 RID: 22725 RVA: 0x0028DAA0 File Offset: 0x0028BCA0
	public ConduitFlow.ConduitContents RemoveElement(ConduitFlow.Conduit conduit, float delta)
	{
		ConduitFlow.ConduitContents contents = conduit.GetContents(this);
		float num = Mathf.Min(contents.mass, delta);
		float num2 = contents.mass - num;
		if (num2 <= 0f)
		{
			conduit.SetContents(this, ConduitFlow.ConduitContents.Empty);
			return contents;
		}
		ConduitFlow.ConduitContents result = contents;
		result.RemoveMass(num2);
		int num3 = (int)(num2 / contents.mass * (float)contents.diseaseCount);
		result.diseaseCount = contents.diseaseCount - num3;
		ConduitFlow.ConduitContents contents2 = contents;
		contents2.RemoveMass(num);
		contents2.diseaseCount = num3;
		if (num3 <= 0)
		{
			contents2.diseaseIdx = byte.MaxValue;
			contents2.diseaseCount = 0;
		}
		conduit.SetContents(this, contents2);
		return result;
	}

	// Token: 0x060058C6 RID: 22726 RVA: 0x0028DB50 File Offset: 0x0028BD50
	public ConduitFlow.FlowDirections GetPermittedFlow(int cell)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.FlowDirections.None;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x060058C7 RID: 22727 RVA: 0x000D9DB5 File Offset: 0x000D7FB5
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x060058C8 RID: 22728 RVA: 0x0028DB84 File Offset: 0x0028BD84
	public ConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return ConduitFlow.Conduit.Invalid;
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x060058C9 RID: 22729 RVA: 0x0028DBBC File Offset: 0x0028BDBC
	private void DumpPipeContents(int cell, ConduitFlow.ConduitContents contents)
	{
		if (contents.element != SimHashes.Vacuum && contents.mass > 0f)
		{
			SimMessages.AddRemoveSubstance(cell, contents.element, CellEventLogger.Instance.ConduitFlowEmptyConduit, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, true, -1);
			this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
		}
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x000D9DCE File Offset: 0x000D7FCE
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell, this.grid[cell].contents);
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x000D9DF7 File Offset: 0x000D7FF7
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x000D9E06 File Offset: 0x000D8006
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
	}

	// Token: 0x060058CD RID: 22733 RVA: 0x000D9E26 File Offset: 0x000D8026
	[Conditional("CHECK_NAN")]
	private void Validate(ConduitFlow.ConduitContents contents)
	{
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError("zero degree pipe contents");
		}
	}

	// Token: 0x060058CE RID: 22734 RVA: 0x0028DC24 File Offset: 0x0028BE24
	[OnSerializing]
	private void OnSerializing()
	{
		int numEntries = this.soaInfo.NumEntries;
		if (numEntries > 0)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[numEntries];
			this.serializedIdx = new int[numEntries];
			for (int i = 0; i < numEntries; i++)
			{
				ConduitFlow.Conduit conduit = this.soaInfo.GetConduit(i);
				ConduitFlow.ConduitContents contents = conduit.GetContents(this);
				this.serializedIdx[i] = this.soaInfo.GetCell(conduit.idx);
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(contents);
			}
			return;
		}
		this.serializedContents = null;
		this.versionedSerializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x000D9E4D File Offset: 0x000D804D
	[OnSerialized]
	private void OnSerialized()
	{
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060058D0 RID: 22736 RVA: 0x0028DCBC File Offset: 0x0028BEBC
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedContents != null)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[this.serializedContents.Length];
			for (int i = 0; i < this.serializedContents.Length; i++)
			{
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(this.serializedContents[i]);
			}
			this.serializedContents = null;
		}
		if (this.versionedSerializedContents == null)
		{
			return;
		}
		for (int j = 0; j < this.versionedSerializedContents.Length; j++)
		{
			int num = this.serializedIdx[j];
			ConduitFlow.SerializedContents serializedContents = this.versionedSerializedContents[j];
			ConduitFlow.ConduitContents conduitContents = (serializedContents.mass <= 0f) ? ConduitFlow.ConduitContents.Empty : new ConduitFlow.ConduitContents(serializedContents.element, Math.Min(this.MaxMass, serializedContents.mass), serializedContents.temperature, byte.MaxValue, 0);
			if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0)
			{
				conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
				conduitContents.diseaseCount = ((conduitContents.diseaseIdx == byte.MaxValue) ? 0 : serializedContents.diseaseCount);
			}
			if (float.IsNaN(conduitContents.temperature) || (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) || 10000f < conduitContents.temperature)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				DeserializeWarnings.Instance.PipeContentsTemperatureIsNan.Warn(string.Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})", new object[]
				{
					conduitContents.temperature,
					vector2I.x,
					vector2I.y,
					num
				}), null);
				conduitContents.temperature = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
			}
			this.SetContents(num, conduitContents);
		}
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060058D1 RID: 22737 RVA: 0x0028DEB0 File Offset: 0x0028C0B0
	public UtilityNetwork GetNetwork(ConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x060058D2 RID: 22738 RVA: 0x000D9E64 File Offset: 0x000D8064
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x060058D3 RID: 22739 RVA: 0x0028DEDC File Offset: 0x0028C0DC
	public bool IsConduitFull(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return this.MaxMass - contents.mass <= 0f;
	}

	// Token: 0x060058D4 RID: 22740 RVA: 0x0028DF14 File Offset: 0x0028C114
	public bool IsConduitEmpty(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.mass <= 0f;
	}

	// Token: 0x060058D5 RID: 22741 RVA: 0x0028DF44 File Offset: 0x0028C144
	public void FreezeConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-700727624, null);
		}
	}

	// Token: 0x060058D6 RID: 22742 RVA: 0x0028DFA0 File Offset: 0x0028C1A0
	public void MeltConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-1152799878, null);
		}
	}

	// Token: 0x04003E8A RID: 16010
	public const float MAX_LIQUID_MASS = 10f;

	// Token: 0x04003E8B RID: 16011
	public const float MAX_GAS_MASS = 1f;

	// Token: 0x04003E8C RID: 16012
	public ConduitType conduitType;

	// Token: 0x04003E8D RID: 16013
	private float MaxMass = 10f;

	// Token: 0x04003E8E RID: 16014
	private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;

	// Token: 0x04003E8F RID: 16015
	public const float TickRate = 1f;

	// Token: 0x04003E90 RID: 16016
	public const float WaitTime = 1f;

	// Token: 0x04003E91 RID: 16017
	private float elapsedTime;

	// Token: 0x04003E92 RID: 16018
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04003E93 RID: 16019
	public ConduitFlow.SOAInfo soaInfo = new ConduitFlow.SOAInfo();

	// Token: 0x04003E95 RID: 16021
	private bool dirtyConduitUpdaters;

	// Token: 0x04003E96 RID: 16022
	private List<ConduitFlow.ConduitUpdater> conduitUpdaters = new List<ConduitFlow.ConduitUpdater>();

	// Token: 0x04003E97 RID: 16023
	private ConduitFlow.GridNode[] grid;

	// Token: 0x04003E98 RID: 16024
	[Serialize]
	public int[] serializedIdx;

	// Token: 0x04003E99 RID: 16025
	[Serialize]
	public ConduitFlow.ConduitContents[] serializedContents;

	// Token: 0x04003E9A RID: 16026
	[Serialize]
	public ConduitFlow.SerializedContents[] versionedSerializedContents;

	// Token: 0x04003E9B RID: 16027
	private IUtilityNetworkMgr networkMgr;

	// Token: 0x04003E9C RID: 16028
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04003E9D RID: 16029
	private const int FLOW_DIRECTION_COUNT = 4;

	// Token: 0x04003E9E RID: 16030
	private List<ConduitFlow.Network> networks = new List<ConduitFlow.Network>();

	// Token: 0x04003E9F RID: 16031
	private WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow> build_network_job = new WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow>();

	// Token: 0x04003EA0 RID: 16032
	private WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext> connect_job = new WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext>();

	// Token: 0x04003EA1 RID: 16033
	private WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow> update_networks_job = new WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow>();

	// Token: 0x020010E0 RID: 4320
	[DebuggerDisplay("{NumEntries}")]
	public class SOAInfo
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060058D7 RID: 22743 RVA: 0x000D9E71 File Offset: 0x000D8071
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x060058D8 RID: 22744 RVA: 0x0028DFFC File Offset: 0x0028C1FC
		public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			ConduitFlow.Conduit item = new ConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new ConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(ConduitFlow.ConduitFlowInfo.DEFAULT);
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
			HandleVector<int>.Handle handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType, count, handle, ref contents);
			HandleVector<int>.Handle item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
			this.cells.Add(cell);
			this.diseaseContentsVisible.Add(false);
			this.structureTemperatureHandles.Add(handle);
			this.temperatureHandles.Add(handle2);
			this.diseaseHandles.Add(item2);
			this.conduitGOs.Add(conduit_go);
			this.permittedFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.srcFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.pullDirections.Add(ConduitFlow.FlowDirections.None);
			this.targetFlowDirections.Add(ConduitFlow.FlowDirections.None);
			return count;
		}

		// Token: 0x060058D9 RID: 22745 RVA: 0x0028E144 File Offset: 0x0028C344
		public void Clear(ConduitFlow manager)
		{
			if (this.clearJob.Count == 0)
			{
				this.clearJob.Reset(this);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishTemperatureToSim>(this.publishTemperatureToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToSim>(this.publishDiseaseToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.ResetConduit>(this.resetConduit);
			}
			this.clearPermanentDiseaseContainer.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToSim.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToSim.Initialize(this.conduits.Count, manager);
			this.resetConduit.Initialize(this.conduits.Count, manager);
			this.clearPermanentDiseaseContainer.Run(this);
			GlobalJobManager.Run(this.clearJob);
			for (int num = 0; num != this.conduits.Count; num++)
			{
				Game.Instance.conduitDiseaseManager.Free(this.diseaseHandles[num]);
			}
			for (int num2 = 0; num2 != this.conduits.Count; num2++)
			{
				Game.Instance.conduitTemperatureManager.Free(this.temperatureHandles[num2]);
			}
			this.cells.Clear();
			this.diseaseContentsVisible.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.pullDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.diseaseHandles.Clear();
			this.temperatureHandles.Clear();
			this.structureTemperatureHandles.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x000D9E7E File Offset: 0x000D807E
		public ConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x060058DB RID: 22747 RVA: 0x000D9E8C File Offset: 0x000D808C
		public ConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x060058DC RID: 22748 RVA: 0x000D9E9A File Offset: 0x000D809A
		public void SetConduitConnections(int idx, ConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x0028E310 File Offset: 0x0028C510
		public float GetConduitTemperature(int idx)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
			global::Debug.Assert(!float.IsNaN(temperature));
			return temperature;
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x0028E348 File Offset: 0x0028C548
		public void SetConduitTemperatureData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x0028E374 File Offset: 0x0028C574
		public ConduitDiseaseManager.Data GetDiseaseData(int idx)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			return Game.Instance.conduitDiseaseManager.GetData(handle);
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x0028E3A0 File Offset: 0x0028C5A0
		public void SetDiseaseData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
		}

		// Token: 0x060058E1 RID: 22753 RVA: 0x000D9EA9 File Offset: 0x000D80A9
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x060058E2 RID: 22754 RVA: 0x0028E3CC File Offset: 0x0028C5CC
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

		// Token: 0x060058E3 RID: 22755 RVA: 0x0028E418 File Offset: 0x0028C618
		public ConduitFlow.Conduit GetConduitFromDirection(int idx, ConduitFlow.FlowDirections direction)
		{
			ConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case ConduitFlow.FlowDirections.Down:
				if (conduitConnections.down == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.down];
			case ConduitFlow.FlowDirections.Left:
				if (conduitConnections.left == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.left];
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				if (conduitConnections.right == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.right];
			default:
				if (direction == ConduitFlow.FlowDirections.Up)
				{
					if (conduitConnections.up == -1)
					{
						return ConduitFlow.Conduit.Invalid;
					}
					return this.conduits[conduitConnections.up];
				}
				break;
			}
			return ConduitFlow.Conduit.Invalid;
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x0028E4DC File Offset: 0x0028C6DC
		public void BeginFrame(ConduitFlow manager)
		{
			if (this.beginFrameJob.Count == 0)
			{
				this.beginFrameJob.Reset(this);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InitializeContentsTask>(this.initializeContents);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InvalidateLastFlow>(this.invalidateLastFlow);
			}
			this.initializeContents.Initialize(this.conduits.Count, manager);
			this.invalidateLastFlow.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.beginFrameJob);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x0028E560 File Offset: 0x0028C760
		public void EndFrame(ConduitFlow manager)
		{
			if (this.endFrameJob.Count == 0)
			{
				this.endFrameJob.Reset(this);
				this.endFrameJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToGame>(this.publishDiseaseToGame);
			}
			this.publishTemperatureToGame.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToGame.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToGame.Run(this);
			GlobalJobManager.Run(this.endFrameJob);
		}

		// Token: 0x060058E6 RID: 22758 RVA: 0x0028E5DC File Offset: 0x0028C7DC
		public void UpdateFlowDirection(ConduitFlow manager)
		{
			if (this.updateFlowDirectionJob.Count == 0)
			{
				this.updateFlowDirectionJob.Reset(this);
				this.updateFlowDirectionJob.Add<ConduitFlow.SOAInfo.FlowThroughVacuum>(this.flowThroughVacuum);
			}
			this.flowThroughVacuum.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.updateFlowDirectionJob);
		}

		// Token: 0x060058E7 RID: 22759 RVA: 0x000D9EB7 File Offset: 0x000D80B7
		public void ResetLastFlowInfo(int idx)
		{
			this.lastFlowInfo[idx] = ConduitFlow.ConduitFlowInfo.DEFAULT;
		}

		// Token: 0x060058E8 RID: 22760 RVA: 0x0028E638 File Offset: 0x0028C838
		public void SetLastFlowInfo(int idx, ConduitFlow.FlowDirections direction, ref ConduitFlow.ConduitContents contents)
		{
			if (this.lastFlowInfo[idx].direction == ConduitFlow.FlowDirections.None)
			{
				this.lastFlowInfo[idx] = new ConduitFlow.ConduitFlowInfo
				{
					direction = direction,
					contents = contents
				};
			}
		}

		// Token: 0x060058E9 RID: 22761 RVA: 0x000D9ECA File Offset: 0x000D80CA
		public ConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x060058EA RID: 22762 RVA: 0x000D9ED8 File Offset: 0x000D80D8
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x060058EB RID: 22763 RVA: 0x000D9EE6 File Offset: 0x000D80E6
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x000D9EF4 File Offset: 0x000D80F4
		public void SetPermittedFlowDirections(int idx, ConduitFlow.FlowDirections permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x0028E684 File Offset: 0x0028C884
		public ConduitFlow.FlowDirections AddPermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] |= delta;
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x0028E6B0 File Offset: 0x0028C8B0
		public ConduitFlow.FlowDirections RemovePermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] &= ~delta;
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x000D9F03 File Offset: 0x000D8103
		public ConduitFlow.FlowDirections GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x000D9F11 File Offset: 0x000D8111
		public void SetTargetFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x000D9F20 File Offset: 0x000D8120
		public ConduitFlow.FlowDirections GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x060058F2 RID: 22770 RVA: 0x000D9F2E File Offset: 0x000D812E
		public void SetSrcFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x000D9F3D File Offset: 0x000D813D
		public ConduitFlow.FlowDirections GetPullDirection(int idx)
		{
			return this.pullDirections[idx];
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x000D9F4B File Offset: 0x000D814B
		public void SetPullDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.pullDirections[idx] = directions;
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x000D9F5A File Offset: 0x000D815A
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x060058F6 RID: 22774 RVA: 0x000D9F68 File Offset: 0x000D8168
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x04003EA2 RID: 16034
		private List<ConduitFlow.Conduit> conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x04003EA3 RID: 16035
		private List<ConduitFlow.ConduitConnections> conduitConnections = new List<ConduitFlow.ConduitConnections>();

		// Token: 0x04003EA4 RID: 16036
		private List<ConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<ConduitFlow.ConduitFlowInfo>();

		// Token: 0x04003EA5 RID: 16037
		private List<ConduitFlow.ConduitContents> initialContents = new List<ConduitFlow.ConduitContents>();

		// Token: 0x04003EA6 RID: 16038
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04003EA7 RID: 16039
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04003EA8 RID: 16040
		private List<int> cells = new List<int>();

		// Token: 0x04003EA9 RID: 16041
		private List<ConduitFlow.FlowDirections> permittedFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003EAA RID: 16042
		private List<ConduitFlow.FlowDirections> srcFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003EAB RID: 16043
		private List<ConduitFlow.FlowDirections> pullDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003EAC RID: 16044
		private List<ConduitFlow.FlowDirections> targetFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003EAD RID: 16045
		private List<HandleVector<int>.Handle> structureTemperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003EAE RID: 16046
		private List<HandleVector<int>.Handle> temperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003EAF RID: 16047
		private List<HandleVector<int>.Handle> diseaseHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003EB0 RID: 16048
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer>();

		// Token: 0x04003EB1 RID: 16049
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim> publishTemperatureToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim>();

		// Token: 0x04003EB2 RID: 16050
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim> publishDiseaseToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim>();

		// Token: 0x04003EB3 RID: 16051
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit> resetConduit = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit>();

		// Token: 0x04003EB4 RID: 16052
		private ConduitFlow.SOAInfo.ConduitJob clearJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003EB5 RID: 16053
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask> initializeContents = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask>();

		// Token: 0x04003EB6 RID: 16054
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow> invalidateLastFlow = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow>();

		// Token: 0x04003EB7 RID: 16055
		private ConduitFlow.SOAInfo.ConduitJob beginFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003EB8 RID: 16056
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame> publishTemperatureToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame>();

		// Token: 0x04003EB9 RID: 16057
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame> publishDiseaseToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame>();

		// Token: 0x04003EBA RID: 16058
		private ConduitFlow.SOAInfo.ConduitJob endFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003EBB RID: 16059
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum> flowThroughVacuum = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum>();

		// Token: 0x04003EBC RID: 16060
		private ConduitFlow.SOAInfo.ConduitJob updateFlowDirectionJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x020010E1 RID: 4321
		private abstract class ConduitTask : DivisibleTask<ConduitFlow.SOAInfo>
		{
			// Token: 0x060058F8 RID: 22776 RVA: 0x000D9F77 File Offset: 0x000D8177
			public ConduitTask(string name) : base(name)
			{
			}

			// Token: 0x04003EBD RID: 16061
			public ConduitFlow manager;
		}

		// Token: 0x020010E2 RID: 4322
		private class ConduitTaskDivision<Task> : TaskDivision<Task, ConduitFlow.SOAInfo> where Task : ConduitFlow.SOAInfo.ConduitTask, new()
		{
			// Token: 0x060058F9 RID: 22777 RVA: 0x0028E818 File Offset: 0x0028CA18
			public void Initialize(int conduitCount, ConduitFlow manager)
			{
				base.Initialize(conduitCount);
				Task[] tasks = this.tasks;
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].manager = manager;
				}
			}
		}

		// Token: 0x020010E3 RID: 4323
		private class ConduitJob : WorkItemCollection<ConduitFlow.SOAInfo.ConduitTask, ConduitFlow.SOAInfo>
		{
			// Token: 0x060058FB RID: 22779 RVA: 0x0028E854 File Offset: 0x0028CA54
			public void Add<Task>(ConduitFlow.SOAInfo.ConduitTaskDivision<Task> taskDivision) where Task : ConduitFlow.SOAInfo.ConduitTask, new()
			{
				foreach (Task task in taskDivision.tasks)
				{
					base.Add(task);
				}
			}
		}

		// Token: 0x020010E4 RID: 4324
		private class ClearPermanentDiseaseContainer : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060058FD RID: 22781 RVA: 0x000D9F90 File Offset: 0x000D8190
			public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer")
			{
			}

			// Token: 0x060058FE RID: 22782 RVA: 0x0028E88C File Offset: 0x0028CA8C
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.ForcePermanentDiseaseContainer(num, false);
				}
			}
		}

		// Token: 0x020010E5 RID: 4325
		private class PublishTemperatureToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060058FF RID: 22783 RVA: 0x000D9F9D File Offset: 0x000D819D
			public PublishTemperatureToSim() : base("PublishTemperatureToSim")
			{
			}

			// Token: 0x06005900 RID: 22784 RVA: 0x0028E8B8 File Offset: 0x0028CAB8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.temperatureHandles[num];
					if (handle.IsValid())
					{
						float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
						this.manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
					}
				}
			}
		}

		// Token: 0x020010E6 RID: 4326
		private class PublishDiseaseToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x06005901 RID: 22785 RVA: 0x000D9FAA File Offset: 0x000D81AA
			public PublishDiseaseToSim() : base("PublishDiseaseToSim")
			{
			}

			// Token: 0x06005902 RID: 22786 RVA: 0x0028E92C File Offset: 0x0028CB2C
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.diseaseHandles[num];
					if (handle.IsValid())
					{
						ConduitDiseaseManager.Data data = Game.Instance.conduitDiseaseManager.GetData(handle);
						int num2 = soaInfo.cells[num];
						this.manager.grid[num2].contents.diseaseIdx = data.diseaseIdx;
						this.manager.grid[num2].contents.diseaseCount = data.diseaseCount;
					}
				}
			}
		}

		// Token: 0x020010E7 RID: 4327
		private class ResetConduit : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x06005903 RID: 22787 RVA: 0x000D9FB7 File Offset: 0x000D81B7
			public ResetConduit() : base("ResetConduitTask")
			{
			}

			// Token: 0x06005904 RID: 22788 RVA: 0x0028E9C8 File Offset: 0x0028CBC8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					this.manager.grid[soaInfo.cells[num]].conduitIdx = -1;
				}
			}
		}

		// Token: 0x020010E8 RID: 4328
		private class InitializeContentsTask : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x06005905 RID: 22789 RVA: 0x000D9FC4 File Offset: 0x000D81C4
			public InitializeContentsTask() : base("SetInitialContents")
			{
			}

			// Token: 0x06005906 RID: 22790 RVA: 0x0028EA10 File Offset: 0x0028CC10
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					int num2 = soaInfo.cells[num];
					ConduitFlow.ConduitContents conduitContents = soaInfo.conduits[num].GetContents(this.manager);
					if (conduitContents.mass <= 0f)
					{
						conduitContents = ConduitFlow.ConduitContents.Empty;
					}
					soaInfo.initialContents[num] = conduitContents;
					this.manager.grid[num2].contents = conduitContents;
				}
			}
		}

		// Token: 0x020010E9 RID: 4329
		private class InvalidateLastFlow : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x06005907 RID: 22791 RVA: 0x000D9FD1 File Offset: 0x000D81D1
			public InvalidateLastFlow() : base("InvalidateLastFlow")
			{
			}

			// Token: 0x06005908 RID: 22792 RVA: 0x0028EA94 File Offset: 0x0028CC94
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.lastFlowInfo[num] = ConduitFlow.ConduitFlowInfo.DEFAULT;
				}
			}
		}

		// Token: 0x020010EA RID: 4330
		private class PublishTemperatureToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x06005909 RID: 22793 RVA: 0x000D9FDE File Offset: 0x000D81DE
			public PublishTemperatureToGame() : base("PublishTemperatureToGame")
			{
			}

			// Token: 0x0600590A RID: 22794 RVA: 0x0028EAC8 File Offset: 0x0028CCC8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x020010EB RID: 4331
		private class PublishDiseaseToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600590B RID: 22795 RVA: 0x000D9FEB File Offset: 0x000D81EB
			public PublishDiseaseToGame() : base("PublishDiseaseToGame")
			{
			}

			// Token: 0x0600590C RID: 22796 RVA: 0x0028EB28 File Offset: 0x0028CD28
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x020010EC RID: 4332
		private class FlowThroughVacuum : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600590D RID: 22797 RVA: 0x000D9FF8 File Offset: 0x000D81F8
			public FlowThroughVacuum() : base("FlowThroughVacuum")
			{
			}

			// Token: 0x0600590E RID: 22798 RVA: 0x0028EB88 File Offset: 0x0028CD88
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					ConduitFlow.Conduit conduit = soaInfo.conduits[num];
					int cell = conduit.GetCell(this.manager);
					if (this.manager.grid[cell].contents.element == SimHashes.Vacuum)
					{
						soaInfo.srcFlowDirections[conduit.idx] = ConduitFlow.FlowDirections.None;
					}
				}
			}
		}
	}

	// Token: 0x020010ED RID: 4333
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04003EBE RID: 16062
		public ConduitFlowPriority priority;

		// Token: 0x04003EBF RID: 16063
		public Action<float> callback;
	}

	// Token: 0x020010EE RID: 4334
	[DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
	public struct GridNode
	{
		// Token: 0x04003EC0 RID: 16064
		public int conduitIdx;

		// Token: 0x04003EC1 RID: 16065
		public ConduitFlow.ConduitContents contents;
	}

	// Token: 0x020010EF RID: 4335
	public struct SerializedContents
	{
		// Token: 0x0600590F RID: 22799 RVA: 0x0028EBFC File Offset: 0x0028CDFC
		public SerializedContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			this.element = element;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseHash = ((disease_idx != byte.MaxValue) ? Db.Get().Diseases[(int)disease_idx].id.GetHashCode() : 0);
			this.diseaseCount = disease_count;
			if (this.diseaseCount <= 0)
			{
				this.diseaseHash = 0;
			}
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x000DA005 File Offset: 0x000D8205
		public SerializedContents(ConduitFlow.ConduitContents src)
		{
			this = new ConduitFlow.SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
		}

		// Token: 0x04003EC2 RID: 16066
		public SimHashes element;

		// Token: 0x04003EC3 RID: 16067
		public float mass;

		// Token: 0x04003EC4 RID: 16068
		public float temperature;

		// Token: 0x04003EC5 RID: 16069
		public int diseaseHash;

		// Token: 0x04003EC6 RID: 16070
		public int diseaseCount;
	}

	// Token: 0x020010F0 RID: 4336
	[Flags]
	public enum FlowDirections : byte
	{
		// Token: 0x04003EC8 RID: 16072
		None = 0,
		// Token: 0x04003EC9 RID: 16073
		Down = 1,
		// Token: 0x04003ECA RID: 16074
		Left = 2,
		// Token: 0x04003ECB RID: 16075
		Right = 4,
		// Token: 0x04003ECC RID: 16076
		Up = 8,
		// Token: 0x04003ECD RID: 16077
		All = 15
	}

	// Token: 0x020010F1 RID: 4337
	[DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
	public struct ConduitConnections
	{
		// Token: 0x04003ECE RID: 16078
		public int left;

		// Token: 0x04003ECF RID: 16079
		public int right;

		// Token: 0x04003ED0 RID: 16080
		public int up;

		// Token: 0x04003ED1 RID: 16081
		public int down;

		// Token: 0x04003ED2 RID: 16082
		public static readonly ConduitFlow.ConduitConnections DEFAULT = new ConduitFlow.ConduitConnections
		{
			left = -1,
			right = -1,
			up = -1,
			down = -1
		};
	}

	// Token: 0x020010F2 RID: 4338
	[DebuggerDisplay("{direction}:{contents.element}")]
	public struct ConduitFlowInfo
	{
		// Token: 0x04003ED3 RID: 16083
		public ConduitFlow.FlowDirections direction;

		// Token: 0x04003ED4 RID: 16084
		public ConduitFlow.ConduitContents contents;

		// Token: 0x04003ED5 RID: 16085
		public static readonly ConduitFlow.ConduitFlowInfo DEFAULT = new ConduitFlow.ConduitFlowInfo
		{
			direction = ConduitFlow.FlowDirections.None,
			contents = ConduitFlow.ConduitContents.Empty
		};
	}

	// Token: 0x020010F3 RID: 4339
	[DebuggerDisplay("conduit {idx}")]
	[Serializable]
	public struct Conduit : IEquatable<ConduitFlow.Conduit>
	{
		// Token: 0x06005913 RID: 22803 RVA: 0x000DA02C File Offset: 0x000D822C
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x000DA035 File Offset: 0x000D8235
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(ConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06005915 RID: 22805 RVA: 0x000DA048 File Offset: 0x000D8248
		public void SetPermittedFlowDirections(ConduitFlow.FlowDirections permitted, ConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x000DA05C File Offset: 0x000D825C
		public ConduitFlow.FlowDirections GetTargetFlowDirection(ConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x000DA06F File Offset: 0x000D826F
		public void SetTargetFlowDirection(ConduitFlow.FlowDirections directions, ConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x0028ECD8 File Offset: 0x0028CED8
		public ConduitFlow.ConduitContents GetContents(ConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			contents.temperature = soaInfo.GetConduitTemperature(this.idx);
			ConduitDiseaseManager.Data diseaseData = soaInfo.GetDiseaseData(this.idx);
			contents.diseaseIdx = diseaseData.diseaseIdx;
			contents.diseaseCount = diseaseData.diseaseCount;
			return contents;
		}

		// Token: 0x06005919 RID: 22809 RVA: 0x0028ED4C File Offset: 0x0028CF4C
		public void SetContents(ConduitFlow manager, ConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			soaInfo.SetConduitTemperatureData(this.idx, ref contents);
			soaInfo.ForcePermanentDiseaseContainer(this.idx, contents.diseaseIdx != byte.MaxValue);
			soaInfo.SetDiseaseData(this.idx, ref contents);
		}

		// Token: 0x0600591A RID: 22810 RVA: 0x000DA083 File Offset: 0x000D8283
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x0600591B RID: 22811 RVA: 0x000DA096 File Offset: 0x000D8296
		public ConduitFlow.ConduitContents GetInitialContents(ConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x0600591C RID: 22812 RVA: 0x000DA0A9 File Offset: 0x000D82A9
		public int GetCell(ConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x000DA0BC File Offset: 0x000D82BC
		public bool Equals(ConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x04003ED6 RID: 16086
		public static readonly ConduitFlow.Conduit Invalid = new ConduitFlow.Conduit(-1);

		// Token: 0x04003ED7 RID: 16087
		public readonly int idx;
	}

	// Token: 0x020010F4 RID: 4340
	[DebuggerDisplay("{element} M:{mass} T:{temperature}")]
	public struct ConduitContents
	{
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x000DA0D9 File Offset: 0x000D82D9
		public float mass
		{
			get
			{
				return this.initial_mass + this.added_mass - this.removed_mass;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06005920 RID: 22816 RVA: 0x000DA0EF File Offset: 0x000D82EF
		public float movable_mass
		{
			get
			{
				return this.initial_mass - this.removed_mass;
			}
		}

		// Token: 0x06005921 RID: 22817 RVA: 0x0028EDBC File Offset: 0x0028CFBC
		public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			global::Debug.Assert(!float.IsNaN(temperature));
			this.element = element;
			this.initial_mass = mass;
			this.added_mass = 0f;
			this.removed_mass = 0f;
			this.temperature = temperature;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x06005922 RID: 22818 RVA: 0x000DA0FE File Offset: 0x000D82FE
		public void ConsolidateMass()
		{
			this.initial_mass += this.added_mass;
			this.added_mass = 0f;
			this.initial_mass -= this.removed_mass;
			this.removed_mass = 0f;
		}

		// Token: 0x06005923 RID: 22819 RVA: 0x0028EE14 File Offset: 0x0028D014
		public float GetEffectiveCapacity(float maximum_capacity)
		{
			float mass = this.mass;
			return Mathf.Max(0f, maximum_capacity - mass);
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x000DA13C File Offset: 0x000D833C
		public void AddMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			this.added_mass += amount;
		}

		// Token: 0x06005925 RID: 22821 RVA: 0x0028EE38 File Offset: 0x0028D038
		public float RemoveMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			float result = 0f;
			float num = this.mass - amount;
			if (num < 0f)
			{
				amount += num;
				result = -num;
				global::Debug.Assert(false);
			}
			this.removed_mass += amount;
			return result;
		}

		// Token: 0x04003ED8 RID: 16088
		public SimHashes element;

		// Token: 0x04003ED9 RID: 16089
		private float initial_mass;

		// Token: 0x04003EDA RID: 16090
		private float added_mass;

		// Token: 0x04003EDB RID: 16091
		private float removed_mass;

		// Token: 0x04003EDC RID: 16092
		public float temperature;

		// Token: 0x04003EDD RID: 16093
		public byte diseaseIdx;

		// Token: 0x04003EDE RID: 16094
		public int diseaseCount;

		// Token: 0x04003EDF RID: 16095
		public static readonly ConduitFlow.ConduitContents Empty = new ConduitFlow.ConduitContents
		{
			element = SimHashes.Vacuum,
			initial_mass = 0f,
			added_mass = 0f,
			removed_mass = 0f,
			temperature = 0f,
			diseaseIdx = byte.MaxValue,
			diseaseCount = 0
		};
	}

	// Token: 0x020010F5 RID: 4341
	[DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
	private struct Network
	{
		// Token: 0x04003EE0 RID: 16096
		public List<int> cells;

		// Token: 0x04003EE1 RID: 16097
		public FlowUtilityNetwork network;
	}

	// Token: 0x020010F6 RID: 4342
	private struct BuildNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x06005927 RID: 22823 RVA: 0x0028EEF8 File Offset: 0x0028D0F8
		public BuildNetworkTask(ConduitFlow.Network network, int conduit_count)
		{
			this.network = network;
			this.distance_nodes = QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.Allocate();
			this.distances_via_sources = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sources = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.distances_via_sinks = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sinks = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sources_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
			this.from_sinks_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x0028EF68 File Offset: 0x0028D168
		public void Finish()
		{
			this.distances_via_sinks.Recycle();
			this.distances_via_sources.Recycle();
			this.distance_nodes.Recycle();
			this.from_sources.Recycle();
			this.from_sinks.Recycle();
			this.from_sources_graph.Recycle();
			this.from_sinks_graph.Recycle();
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x0028EFC4 File Offset: 0x0028D1C4
		private void ComputeFlow(ConduitFlow outer)
		{
			this.from_sources_graph.Build(outer, this.network.network.sources, this.network.network.sinks, true);
			this.from_sinks_graph.Build(outer, this.network.network.sinks, this.network.network.sources, false);
			this.from_sources_graph.Merge(this.from_sinks_graph);
			this.from_sources_graph.BreakCycles();
			this.from_sources_graph.WriteFlow(false);
			this.from_sinks_graph.WriteFlow(true);
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x0028F060 File Offset: 0x0028D260
		private void ComputeOrder(ConduitFlow outer)
		{
			foreach (int cell in this.from_sources_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sources_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell2 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell2,
						distance = 0
					});
				}
				goto IL_21D;
			}
			IL_B3:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode = this.distance_nodes.Dequeue();
			int conduitIdx = outer.grid[distanceNode.cell].conduitIdx;
			if (conduitIdx != -1)
			{
				this.distances_via_sources[distanceNode.cell] = distanceNode.distance;
				ConduitFlow.ConduitConnections conduitConnections = outer.soaInfo.GetConduitConnections(conduitIdx);
				ConduitFlow.FlowDirections permittedFlowDirections = outer.soaInfo.GetPermittedFlowDirections(conduitIdx);
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.up),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.down),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.left),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.right),
						distance = distanceNode.distance + 1
					});
				}
			}
			IL_21D:
			if (this.distance_nodes.Count != 0)
			{
				goto IL_B3;
			}
			this.from_sources.AddRange(this.distances_via_sources);
			this.from_sources.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value - a.Value);
			this.distance_nodes.Clear();
			foreach (int cell3 in this.from_sinks_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell3,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sinks_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell4 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell4,
						distance = 0
					});
				}
				goto IL_508;
			}
			IL_32A:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode2 = this.distance_nodes.Dequeue();
			int conduitIdx2 = outer.grid[distanceNode2.cell].conduitIdx;
			if (conduitIdx2 != -1)
			{
				if (!this.distances_via_sources.ContainsKey(distanceNode2.cell))
				{
					this.distances_via_sinks[distanceNode2.cell] = distanceNode2.distance;
				}
				ConduitFlow.ConduitConnections conduitConnections2 = outer.soaInfo.GetConduitConnections(conduitIdx2);
				if (conduitConnections2.up != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.up) & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.up),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.down != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.down) & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.down),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.left != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.left) & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.left),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.right != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.right) & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.right),
						distance = distanceNode2.distance + 1
					});
				}
			}
			IL_508:
			if (this.distance_nodes.Count == 0)
			{
				this.from_sinks.AddRange(this.distances_via_sinks);
				this.from_sinks.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => a.Value - b.Value);
				this.network.cells.Capacity = Mathf.Max(this.network.cells.Capacity, this.from_sources.Count + this.from_sinks.Count);
				foreach (KeyValuePair<int, int> keyValuePair in this.from_sources)
				{
					this.network.cells.Add(keyValuePair.Key);
				}
				foreach (KeyValuePair<int, int> keyValuePair2 in this.from_sinks)
				{
					this.network.cells.Add(keyValuePair2.Key);
				}
				return;
			}
			goto IL_32A;
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x000DA15C File Offset: 0x000D835C
		public void Run(ConduitFlow outer)
		{
			this.ComputeFlow(outer);
			this.ComputeOrder(outer);
		}

		// Token: 0x04003EE2 RID: 16098
		private ConduitFlow.Network network;

		// Token: 0x04003EE3 RID: 16099
		private QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.PooledQueue distance_nodes;

		// Token: 0x04003EE4 RID: 16100
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sources;

		// Token: 0x04003EE5 RID: 16101
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;

		// Token: 0x04003EE6 RID: 16102
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sinks;

		// Token: 0x04003EE7 RID: 16103
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;

		// Token: 0x04003EE8 RID: 16104
		private ConduitFlow.BuildNetworkTask.Graph from_sources_graph;

		// Token: 0x04003EE9 RID: 16105
		private ConduitFlow.BuildNetworkTask.Graph from_sinks_graph;

		// Token: 0x020010F7 RID: 4343
		[DebuggerDisplay("cell {cell}:{distance}")]
		private struct DistanceNode
		{
			// Token: 0x04003EEA RID: 16106
			public int cell;

			// Token: 0x04003EEB RID: 16107
			public int distance;
		}

		// Token: 0x020010F8 RID: 4344
		[DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
		private struct Graph
		{
			// Token: 0x0600592C RID: 22828 RVA: 0x0028F6D8 File Offset: 0x0028D8D8
			public Graph(FlowUtilityNetwork network)
			{
				this.conduit_flow = null;
				this.vertex_cells = HashSetPool<int, ConduitFlow>.Allocate();
				this.edges = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.cycles = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.bfs_traversal = QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.visited = HashSetPool<int, ConduitFlow>.Allocate();
				this.pseudo_sources = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.sources = HashSetPool<int, ConduitFlow>.Allocate();
				this.sinks = HashSetPool<int, ConduitFlow>.Allocate();
				this.dfs_path = HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dfs_traversal = ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dead_ends = HashSetPool<int, ConduitFlow>.Allocate();
				this.cycle_vertices = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
			}

			// Token: 0x0600592D RID: 22829 RVA: 0x0028F770 File Offset: 0x0028D970
			public void Recycle()
			{
				this.vertex_cells.Recycle();
				this.edges.Recycle();
				this.cycles.Recycle();
				this.bfs_traversal.Recycle();
				this.visited.Recycle();
				this.pseudo_sources.Recycle();
				this.sources.Recycle();
				this.sinks.Recycle();
				this.dfs_path.Recycle();
				this.dfs_traversal.Recycle();
				this.dead_ends.Recycle();
				this.cycle_vertices.Recycle();
			}

			// Token: 0x0600592E RID: 22830 RVA: 0x0028F804 File Offset: 0x0028DA04
			public void Build(ConduitFlow conduit_flow, List<FlowUtilityNetwork.IItem> sources, List<FlowUtilityNetwork.IItem> sinks, bool are_dead_ends_pseudo_sources)
			{
				this.conduit_flow = conduit_flow;
				this.sources.Clear();
				for (int i = 0; i < sources.Count; i++)
				{
					int cell = sources[i].Cell;
					if (conduit_flow.grid[cell].conduitIdx != -1)
					{
						this.sources.Add(cell);
					}
				}
				this.sinks.Clear();
				for (int j = 0; j < sinks.Count; j++)
				{
					int cell2 = sinks[j].Cell;
					if (conduit_flow.grid[cell2].conduitIdx != -1)
					{
						this.sinks.Add(cell2);
					}
				}
				global::Debug.Assert(this.bfs_traversal.Count == 0);
				this.visited.Clear();
				foreach (int num in this.sources)
				{
					this.bfs_traversal.Enqueue(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = num,
						direction = ConduitFlow.FlowDirections.None
					});
					this.visited.Add(num);
				}
				this.pseudo_sources.Clear();
				this.dead_ends.Clear();
				this.cycles.Clear();
				while (this.bfs_traversal.Count != 0)
				{
					ConduitFlow.BuildNetworkTask.Graph.Vertex node = this.bfs_traversal.Dequeue();
					this.vertex_cells.Add(node.cell);
					ConduitFlow.FlowDirections flowDirections = ConduitFlow.FlowDirections.None;
					int num2 = 4;
					if (node.direction != ConduitFlow.FlowDirections.None)
					{
						flowDirections = ConduitFlow.Opposite(node.direction);
						num2 = 3;
					}
					int conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
					for (int num3 = 0; num3 != num2; num3++)
					{
						flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
						ConduitFlow.Conduit conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);
						ConduitFlow.BuildNetworkTask.Graph.Vertex new_node = this.WalkPath(conduitIdx, conduitFromDirection.idx, flowDirections, are_dead_ends_pseudo_sources);
						if (new_node.is_valid)
						{
							ConduitFlow.BuildNetworkTask.Graph.Edge item = new ConduitFlow.BuildNetworkTask.Graph.Edge
							{
								vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
								{
									new ConduitFlow.BuildNetworkTask.Graph.Vertex
									{
										cell = node.cell,
										direction = flowDirections
									},
									new_node
								}
							};
							if (new_node.cell == node.cell)
							{
								this.cycles.Add(item);
							}
							else if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == new_node.cell && edge.vertices[1].cell == node.cell) && !this.edges.Contains(item))
							{
								this.edges.Add(item);
								if (this.visited.Add(new_node.cell))
								{
									if (this.IsSink(new_node.cell))
									{
										this.pseudo_sources.Add(new_node);
									}
									else
									{
										this.bfs_traversal.Enqueue(new_node);
									}
								}
							}
						}
					}
					if (this.bfs_traversal.Count == 0)
					{
						foreach (ConduitFlow.BuildNetworkTask.Graph.Vertex item2 in this.pseudo_sources)
						{
							this.bfs_traversal.Enqueue(item2);
						}
						this.pseudo_sources.Clear();
					}
				}
			}

			// Token: 0x0600592F RID: 22831 RVA: 0x0028FBD0 File Offset: 0x0028DDD0
			private bool IsEndpoint(int cell)
			{
				global::Debug.Assert(cell != -1);
				return this.conduit_flow.grid[cell].conduitIdx == -1 || this.sources.Contains(cell) || this.sinks.Contains(cell) || this.dead_ends.Contains(cell);
			}

			// Token: 0x06005930 RID: 22832 RVA: 0x000DA16C File Offset: 0x000D836C
			private bool IsSink(int cell)
			{
				return this.sinks.Contains(cell);
			}

			// Token: 0x06005931 RID: 22833 RVA: 0x0028FC2C File Offset: 0x0028DE2C
			private bool IsJunction(int cell)
			{
				global::Debug.Assert(cell != -1);
				ConduitFlow.GridNode gridNode = this.conduit_flow.grid[cell];
				global::Debug.Assert(gridNode.conduitIdx != -1);
				ConduitFlow.ConduitConnections conduitConnections = this.conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
				return 2 < this.JunctionValue(conduitConnections.down) + this.JunctionValue(conduitConnections.left) + this.JunctionValue(conduitConnections.up) + this.JunctionValue(conduitConnections.right);
			}

			// Token: 0x06005932 RID: 22834 RVA: 0x000DA17A File Offset: 0x000D837A
			private int JunctionValue(int conduit)
			{
				if (conduit != -1)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06005933 RID: 22835 RVA: 0x0028FCB8 File Offset: 0x0028DEB8
			private ConduitFlow.BuildNetworkTask.Graph.Vertex WalkPath(int root_conduit, int conduit, ConduitFlow.FlowDirections direction, bool are_dead_ends_pseudo_sources)
			{
				if (conduit == -1)
				{
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				int cell;
				for (;;)
				{
					cell = this.conduit_flow.soaInfo.GetCell(conduit);
					if (this.IsEndpoint(cell) || this.IsJunction(cell))
					{
						break;
					}
					direction = ConduitFlow.Opposite(direction);
					bool flag = true;
					for (int num = 0; num != 3; num++)
					{
						direction = ConduitFlow.ComputeNextFlowDirection(direction);
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
						if (conduitFromDirection.idx != -1)
						{
							conduit = conduitFromDirection.idx;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						goto Block_4;
					}
				}
				return new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					cell = cell,
					direction = direction
				};
				Block_4:
				if (are_dead_ends_pseudo_sources)
				{
					this.pseudo_sources.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = cell,
						direction = ConduitFlow.ComputeNextFlowDirection(direction)
					});
					this.dead_ends.Add(cell);
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				ConduitFlow.BuildNetworkTask.Graph.Vertex result = default(ConduitFlow.BuildNetworkTask.Graph.Vertex);
				result.cell = cell;
				direction = (result.direction = ConduitFlow.Opposite(ConduitFlow.ComputeNextFlowDirection(direction)));
				return result;
			}

			// Token: 0x06005934 RID: 22836 RVA: 0x0028FDC4 File Offset: 0x0028DFC4
			public void Merge(ConduitFlow.BuildNetworkTask.Graph inverted_graph)
			{
				using (List<ConduitFlow.BuildNetworkTask.Graph.Edge>.Enumerator enumerator = inverted_graph.edges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge = enumerator.Current;
						ConduitFlow.BuildNetworkTask.Graph.Edge candidate = inverted_edge.Invert();
						if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.Equals(inverted_edge) || edge.Equals(candidate)))
						{
							this.edges.Add(candidate);
							this.vertex_cells.Add(candidate.vertices[0].cell);
							this.vertex_cells.Add(candidate.vertices[1].cell);
						}
					}
				}
				int num = 1000;
				for (int num2 = 0; num2 != num; num2++)
				{
					global::Debug.Assert(num2 != num - 1);
					bool flag = false;
					using (HashSet<int>.Enumerator enumerator2 = this.vertex_cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int cell = enumerator2.Current;
							if (!this.IsSink(cell) && !this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == cell))
							{
								int num3 = inverted_graph.edges.FindIndex((ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge) => inverted_edge.vertices[1].cell == cell);
								if (num3 != -1)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge3 = inverted_graph.edges[num3];
									for (int num4 = 0; num4 != this.edges.Count; num4++)
									{
										ConduitFlow.BuildNetworkTask.Graph.Edge edge2 = this.edges[num4];
										if (edge2.vertices[0].cell == edge3.vertices[0].cell && edge2.vertices[1].cell == edge3.vertices[1].cell)
										{
											this.edges[num4] = edge2.Invert();
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}

			// Token: 0x06005935 RID: 22837 RVA: 0x00290028 File Offset: 0x0028E228
			public void BreakCycles()
			{
				this.visited.Clear();
				foreach (int num in this.vertex_cells)
				{
					if (!this.visited.Contains(num))
					{
						this.dfs_path.Clear();
						this.dfs_traversal.Clear();
						this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
						{
							cell = num,
							parent = null
						});
						while (this.dfs_traversal.Count != 0)
						{
							ConduitFlow.BuildNetworkTask.Graph.DFSNode dfsnode = this.dfs_traversal[this.dfs_traversal.Count - 1];
							this.dfs_traversal.RemoveAt(this.dfs_traversal.Count - 1);
							bool flag = false;
							for (ConduitFlow.BuildNetworkTask.Graph.DFSNode parent = dfsnode.parent; parent != null; parent = parent.parent)
							{
								if (parent.cell == dfsnode.cell)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								for (int num2 = this.edges.Count - 1; num2 != -1; num2--)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge = this.edges[num2];
									if (edge.vertices[0].cell == dfsnode.parent.cell && edge.vertices[1].cell == dfsnode.cell)
									{
										this.cycles.Add(edge);
										this.edges.RemoveAt(num2);
									}
								}
							}
							else if (this.visited.Add(dfsnode.cell))
							{
								foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.edges)
								{
									if (edge2.vertices[0].cell == dfsnode.cell)
									{
										this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
										{
											cell = edge2.vertices[1].cell,
											parent = dfsnode
										});
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x06005936 RID: 22838 RVA: 0x00290278 File Offset: 0x0028E478
			public void WriteFlow(bool cycles_only = false)
			{
				if (!cycles_only)
				{
					foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge in this.edges)
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator = edge.Iter(this.conduit_flow);
						while (vertexIterator.IsValid())
						{
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertexIterator.cell].conduitIdx, vertexIterator.direction);
							vertexIterator.Next();
						}
					}
				}
				foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.cycles)
				{
					this.cycle_vertices.Clear();
					ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator2 = edge2.Iter(this.conduit_flow);
					vertexIterator2.Next();
					while (vertexIterator2.IsValid())
					{
						this.cycle_vertices.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
						{
							cell = vertexIterator2.cell,
							direction = vertexIterator2.direction
						});
						vertexIterator2.Next();
					}
					if (this.cycle_vertices.Count > 1)
					{
						int i = 0;
						int num = this.cycle_vertices.Count - 1;
						ConduitFlow.FlowDirections direction = edge2.vertices[0].direction;
						while (i <= num)
						{
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex = this.cycle_vertices[i];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex.cell].conduitIdx, ConduitFlow.Opposite(direction));
							direction = vertex.direction;
							i++;
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex2 = this.cycle_vertices[num];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex2.cell].conduitIdx, vertex2.direction);
							num--;
						}
						this.dead_ends.Add(this.cycle_vertices[i].cell);
						this.dead_ends.Add(this.cycle_vertices[num].cell);
					}
				}
			}

			// Token: 0x04003EEC RID: 16108
			private ConduitFlow conduit_flow;

			// Token: 0x04003EED RID: 16109
			private HashSetPool<int, ConduitFlow>.PooledHashSet vertex_cells;

			// Token: 0x04003EEE RID: 16110
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList edges;

			// Token: 0x04003EEF RID: 16111
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList cycles;

			// Token: 0x04003EF0 RID: 16112
			private QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledQueue bfs_traversal;

			// Token: 0x04003EF1 RID: 16113
			private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

			// Token: 0x04003EF2 RID: 16114
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList pseudo_sources;

			// Token: 0x04003EF3 RID: 16115
			public HashSetPool<int, ConduitFlow>.PooledHashSet sources;

			// Token: 0x04003EF4 RID: 16116
			private HashSetPool<int, ConduitFlow>.PooledHashSet sinks;

			// Token: 0x04003EF5 RID: 16117
			private HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledHashSet dfs_path;

			// Token: 0x04003EF6 RID: 16118
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledList dfs_traversal;

			// Token: 0x04003EF7 RID: 16119
			public HashSetPool<int, ConduitFlow>.PooledHashSet dead_ends;

			// Token: 0x04003EF8 RID: 16120
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList cycle_vertices;

			// Token: 0x020010F9 RID: 4345
			[DebuggerDisplay("{cell}:{direction}")]
			public struct Vertex : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Vertex>
			{
				// Token: 0x17000558 RID: 1368
				// (get) Token: 0x06005937 RID: 22839 RVA: 0x000DA183 File Offset: 0x000D8383
				public bool is_valid
				{
					get
					{
						return this.cell != -1;
					}
				}

				// Token: 0x06005938 RID: 22840 RVA: 0x000DA191 File Offset: 0x000D8391
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Vertex rhs)
				{
					return this.direction == rhs.direction && this.cell == rhs.cell;
				}

				// Token: 0x04003EF9 RID: 16121
				public ConduitFlow.FlowDirections direction;

				// Token: 0x04003EFA RID: 16122
				public int cell;

				// Token: 0x04003EFB RID: 16123
				public static ConduitFlow.BuildNetworkTask.Graph.Vertex INVALID = new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					direction = ConduitFlow.FlowDirections.None,
					cell = -1
				};
			}

			// Token: 0x020010FA RID: 4346
			[DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
			public struct Edge : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Edge>
			{
				// Token: 0x17000559 RID: 1369
				// (get) Token: 0x0600593A RID: 22842 RVA: 0x000DA1B1 File Offset: 0x000D83B1
				public bool is_valid
				{
					get
					{
						return this.vertices != null;
					}
				}

				// Token: 0x0600593B RID: 22843 RVA: 0x00290520 File Offset: 0x0028E720
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Edge rhs)
				{
					if (this.vertices == null)
					{
						return rhs.vertices == null;
					}
					return rhs.vertices != null && (this.vertices.Length == rhs.vertices.Length && this.vertices.Length == 2 && this.vertices[0].Equals(rhs.vertices[0])) && this.vertices[1].Equals(rhs.vertices[1]);
				}

				// Token: 0x0600593C RID: 22844 RVA: 0x002905A4 File Offset: 0x0028E7A4
				public ConduitFlow.BuildNetworkTask.Graph.Edge Invert()
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge
					{
						vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
						{
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[1].cell,
								direction = ConduitFlow.Opposite(this.vertices[1].direction)
							},
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[0].cell,
								direction = ConduitFlow.Opposite(this.vertices[0].direction)
							}
						}
					};
				}

				// Token: 0x0600593D RID: 22845 RVA: 0x000DA1BC File Offset: 0x000D83BC
				public ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator Iter(ConduitFlow conduit_flow)
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator(conduit_flow, this);
				}

				// Token: 0x04003EFC RID: 16124
				public ConduitFlow.BuildNetworkTask.Graph.Vertex[] vertices;

				// Token: 0x04003EFD RID: 16125
				public static readonly ConduitFlow.BuildNetworkTask.Graph.Edge INVALID = new ConduitFlow.BuildNetworkTask.Graph.Edge
				{
					vertices = null
				};

				// Token: 0x020010FB RID: 4347
				[DebuggerDisplay("{cell}:{direction}")]
				public struct VertexIterator
				{
					// Token: 0x0600593F RID: 22847 RVA: 0x000DA1CA File Offset: 0x000D83CA
					public VertexIterator(ConduitFlow conduit_flow, ConduitFlow.BuildNetworkTask.Graph.Edge edge)
					{
						this.conduit_flow = conduit_flow;
						this.edge = edge;
						this.cell = edge.vertices[0].cell;
						this.direction = edge.vertices[0].direction;
					}

					// Token: 0x06005940 RID: 22848 RVA: 0x00290678 File Offset: 0x0028E878
					public void Next()
					{
						int conduitIdx = this.conduit_flow.grid[this.cell].conduitIdx;
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, this.direction);
						global::Debug.Assert(conduitFromDirection.idx != -1);
						this.cell = conduitFromDirection.GetCell(this.conduit_flow);
						if (this.cell == this.edge.vertices[1].cell)
						{
							return;
						}
						this.direction = ConduitFlow.Opposite(this.direction);
						bool flag = false;
						for (int num = 0; num != 3; num++)
						{
							this.direction = ConduitFlow.ComputeNextFlowDirection(this.direction);
							if (this.conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.direction).idx != -1)
							{
								flag = true;
								break;
							}
						}
						global::Debug.Assert(flag);
						if (!flag)
						{
							this.cell = this.edge.vertices[1].cell;
						}
					}

					// Token: 0x06005941 RID: 22849 RVA: 0x000DA208 File Offset: 0x000D8408
					public bool IsValid()
					{
						return this.cell != this.edge.vertices[1].cell;
					}

					// Token: 0x04003EFE RID: 16126
					public int cell;

					// Token: 0x04003EFF RID: 16127
					public ConduitFlow.FlowDirections direction;

					// Token: 0x04003F00 RID: 16128
					private ConduitFlow conduit_flow;

					// Token: 0x04003F01 RID: 16129
					private ConduitFlow.BuildNetworkTask.Graph.Edge edge;
				}
			}

			// Token: 0x020010FC RID: 4348
			[DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
			private class DFSNode
			{
				// Token: 0x04003F02 RID: 16130
				public int cell;

				// Token: 0x04003F03 RID: 16131
				public ConduitFlow.BuildNetworkTask.Graph.DFSNode parent;
			}
		}
	}

	// Token: 0x02001102 RID: 4354
	private struct ConnectContext
	{
		// Token: 0x0600594F RID: 22863 RVA: 0x000DA2AF File Offset: 0x000D84AF
		public ConnectContext(ConduitFlow outer)
		{
			this.outer = outer;
			this.cells = ListPool<int, ConduitFlow>.Allocate();
			this.cells.Capacity = Mathf.Max(this.cells.Capacity, outer.soaInfo.NumEntries);
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x000DA2E9 File Offset: 0x000D84E9
		public void Finish()
		{
			this.cells.Recycle();
		}

		// Token: 0x04003F0D RID: 16141
		public ListPool<int, ConduitFlow>.PooledList cells;

		// Token: 0x04003F0E RID: 16142
		public ConduitFlow outer;
	}

	// Token: 0x02001103 RID: 4355
	private struct ConnectTask : IWorkItem<ConduitFlow.ConnectContext>
	{
		// Token: 0x06005951 RID: 22865 RVA: 0x000DA2F6 File Offset: 0x000D84F6
		public ConnectTask(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x002907CC File Offset: 0x0028E9CC
		public void Run(ConduitFlow.ConnectContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				int num2 = context.cells[num];
				int conduitIdx = context.outer.grid[num2].conduitIdx;
				if (conduitIdx != -1)
				{
					UtilityConnections connections = context.outer.networkMgr.GetConnections(num2, true);
					if (connections != (UtilityConnections)0)
					{
						ConduitFlow.ConduitConnections @default = ConduitFlow.ConduitConnections.DEFAULT;
						int num3 = num2 - 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
						{
							@default.left = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
						{
							@default.right = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 - Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
						{
							@default.down = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
						{
							@default.up = context.outer.grid[num3].conduitIdx;
						}
						context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
					}
				}
			}
		}

		// Token: 0x04003F0F RID: 16143
		private int start;

		// Token: 0x04003F10 RID: 16144
		private int end;
	}

	// Token: 0x02001104 RID: 4356
	private class UpdateNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06005953 RID: 22867 RVA: 0x000DA306 File Offset: 0x000D8506
		// (set) Token: 0x06005954 RID: 22868 RVA: 0x000DA30E File Offset: 0x000D850E
		public bool continue_updating { get; private set; }

		// Token: 0x06005955 RID: 22869 RVA: 0x000DA317 File Offset: 0x000D8517
		public UpdateNetworkTask(ConduitFlow.Network network)
		{
			this.continue_updating = true;
			this.network = network;
		}

		// Token: 0x06005956 RID: 22870 RVA: 0x00290920 File Offset: 0x0028EB20
		public void Run(ConduitFlow conduit_flow)
		{
			global::Debug.Assert(this.continue_updating);
			this.continue_updating = false;
			foreach (int num in this.network.cells)
			{
				int conduitIdx = conduit_flow.grid[num].conduitIdx;
				if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx)))
				{
					this.continue_updating = true;
				}
			}
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x002909B0 File Offset: 0x0028EBB0
		public void Finish(ConduitFlow conduit_flow)
		{
			foreach (int num in this.network.cells)
			{
				ConduitFlow.ConduitContents contents = conduit_flow.grid[num].contents;
				contents.ConsolidateMass();
				conduit_flow.grid[num].contents = contents;
			}
		}

		// Token: 0x04003F11 RID: 16145
		private ConduitFlow.Network network;
	}
}
