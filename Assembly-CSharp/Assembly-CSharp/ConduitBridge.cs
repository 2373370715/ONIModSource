﻿using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : ConduitBridgeBase, IBridgedNetworkItem
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.accumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	protected override void OnCleanUp()
	{
		Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		float num = contents.mass;
		if (this.desiredMassTransfer != null)
		{
			num = this.desiredMassTransfer(dt, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, null);
		}
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			float num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			if (num2 <= 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			flowManager.RemoveElement(this.inputCell, num2);
			Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
			if (this.OnMassTransfer != null)
			{
				this.OnMassTransfer(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, null);
				return;
			}
		}
		else
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		UtilityNetwork networkForCell = networkManager.GetNetworkForCell(this.inputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
		networkForCell = networkManager.GetNetworkForCell(this.outputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		bool flag = false;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
	}

	public int GetNetworkCell()
	{
		return this.inputCell;
	}

	[SerializeField]
	public ConduitType type;

	private int inputCell;

	private int outputCell;

	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
}
