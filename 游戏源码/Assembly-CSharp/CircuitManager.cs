using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using STRINGS;
using UnityEngine;

// Token: 0x02001070 RID: 4208
public class CircuitManager
{
	// Token: 0x060055CC RID: 21964 RVA: 0x000D7F85 File Offset: 0x000D6185
	public void Connect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Add(generator);
		this.dirty = true;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x000D7FA3 File Offset: 0x000D61A3
	public void Disconnect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Remove(generator);
		this.dirty = true;
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x000D7FC1 File Offset: 0x000D61C1
	public void Connect(IEnergyConsumer consumer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Add(consumer);
		this.dirty = true;
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x000D7FDF File Offset: 0x000D61DF
	public void Disconnect(IEnergyConsumer consumer, bool isDestroy)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Remove(consumer);
		if (!isDestroy)
		{
			consumer.SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
		}
		this.dirty = true;
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x000D8007 File Offset: 0x000D6207
	public void Connect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Add(bridge);
		this.dirty = true;
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x000D801D File Offset: 0x000D621D
	public void Disconnect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Remove(bridge);
		this.dirty = true;
	}

	// Token: 0x060055D2 RID: 21970 RVA: 0x0027FB10 File Offset: 0x0027DD10
	public float GetPowerDraw(ushort circuitID, Generator generator)
	{
		float result = 0f;
		if ((int)circuitID < this.circuitInfo.Count)
		{
			CircuitManager.CircuitInfo value = this.circuitInfo[(int)circuitID];
			this.circuitInfo[(int)circuitID] = value;
			this.circuitInfo[(int)circuitID] = value;
		}
		return result;
	}

	// Token: 0x060055D3 RID: 21971 RVA: 0x0027FB58 File Offset: 0x0027DD58
	public ushort GetCircuitID(int cell)
	{
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return (ushort)((networkForCell == null) ? 65535 : networkForCell.id);
	}

	// Token: 0x060055D4 RID: 21972 RVA: 0x0027FB88 File Offset: 0x0027DD88
	public ushort GetVirtualCircuitID(object virtualKey)
	{
		UtilityNetwork networkForVirtualKey = Game.Instance.electricalConduitSystem.GetNetworkForVirtualKey(virtualKey);
		return (ushort)((networkForVirtualKey == null) ? 65535 : networkForVirtualKey.id);
	}

	// Token: 0x060055D5 RID: 21973 RVA: 0x000D8033 File Offset: 0x000D6233
	public ushort GetCircuitID(ICircuitConnected ent)
	{
		if (!ent.IsVirtual)
		{
			return this.GetCircuitID(ent.PowerCell);
		}
		return this.GetVirtualCircuitID(ent.VirtualCircuitKey);
	}

	// Token: 0x060055D6 RID: 21974 RVA: 0x000D8056 File Offset: 0x000D6256
	public void Sim200msFirst(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x060055D7 RID: 21975 RVA: 0x000D8056 File Offset: 0x000D6256
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x060055D8 RID: 21976 RVA: 0x0027FBB8 File Offset: 0x0027DDB8
	private void Refresh(float dt)
	{
		UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem = Game.Instance.electricalConduitSystem;
		if (electricalConduitSystem.IsDirty || this.dirty)
		{
			electricalConduitSystem.Update();
			IList<UtilityNetwork> networks = electricalConduitSystem.GetNetworks();
			while (this.circuitInfo.Count < networks.Count)
			{
				CircuitManager.CircuitInfo circuitInfo = new CircuitManager.CircuitInfo
				{
					generators = new List<Generator>(),
					consumers = new List<IEnergyConsumer>(),
					batteries = new List<Battery>(),
					inputTransformers = new List<Battery>(),
					outputTransformers = new List<Generator>()
				};
				circuitInfo.bridgeGroups = new List<WireUtilityNetworkLink>[5];
				for (int i = 0; i < circuitInfo.bridgeGroups.Length; i++)
				{
					circuitInfo.bridgeGroups[i] = new List<WireUtilityNetworkLink>();
				}
				this.circuitInfo.Add(circuitInfo);
			}
			this.Rebuild();
		}
	}

	// Token: 0x060055D9 RID: 21977 RVA: 0x0027FC98 File Offset: 0x0027DE98
	public void Rebuild()
	{
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.generators.Clear();
			circuitInfo.consumers.Clear();
			circuitInfo.batteries.Clear();
			circuitInfo.inputTransformers.Clear();
			circuitInfo.outputTransformers.Clear();
			circuitInfo.minBatteryPercentFull = 1f;
			for (int j = 0; j < circuitInfo.bridgeGroups.Length; j++)
			{
				circuitInfo.bridgeGroups[j].Clear();
			}
			this.circuitInfo[i] = circuitInfo;
		}
		this.consumersShadow.AddRange(this.consumers);
		foreach (IEnergyConsumer energyConsumer in this.consumersShadow)
		{
			ushort circuitID = this.GetCircuitID(energyConsumer);
			if (circuitID != 65535)
			{
				Battery battery = energyConsumer as Battery;
				if (battery != null)
				{
					CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[(int)circuitID];
					if (battery.powerTransformer != null)
					{
						circuitInfo2.inputTransformers.Add(battery);
					}
					else
					{
						circuitInfo2.batteries.Add(battery);
						circuitInfo2.minBatteryPercentFull = Mathf.Min(this.circuitInfo[(int)circuitID].minBatteryPercentFull, battery.PercentFull);
					}
					this.circuitInfo[(int)circuitID] = circuitInfo2;
				}
				else
				{
					this.circuitInfo[(int)circuitID].consumers.Add(energyConsumer);
				}
			}
		}
		this.consumersShadow.Clear();
		for (int k = 0; k < this.circuitInfo.Count; k++)
		{
			this.circuitInfo[k].consumers.Sort((IEnergyConsumer a, IEnergyConsumer b) => a.WattsNeededWhenActive.CompareTo(b.WattsNeededWhenActive));
		}
		foreach (Generator generator in this.generators)
		{
			ushort circuitID2 = this.GetCircuitID(generator);
			if (circuitID2 != 65535)
			{
				if (generator.GetType() == typeof(PowerTransformer))
				{
					this.circuitInfo[(int)circuitID2].outputTransformers.Add(generator);
				}
				else
				{
					this.circuitInfo[(int)circuitID2].generators.Add(generator);
				}
			}
		}
		foreach (WireUtilityNetworkLink wireUtilityNetworkLink in this.bridges)
		{
			ushort circuitID3 = this.GetCircuitID(wireUtilityNetworkLink);
			if (circuitID3 != 65535)
			{
				Wire.WattageRating maxWattageRating = wireUtilityNetworkLink.GetMaxWattageRating();
				this.circuitInfo[(int)circuitID3].bridgeGroups[(int)maxWattageRating].Add(wireUtilityNetworkLink);
			}
		}
		this.dirty = false;
	}

	// Token: 0x060055DA RID: 21978 RVA: 0x0027FF70 File Offset: 0x0027E170
	private float GetBatteryJoulesAvailable(List<Battery> batteries, out int num_powered)
	{
		float result = 0f;
		num_powered = 0;
		for (int i = 0; i < batteries.Count; i++)
		{
			if (batteries[i].JoulesAvailable > 0f)
			{
				result = batteries[i].JoulesAvailable;
				num_powered = batteries.Count - i;
				break;
			}
		}
		return result;
	}

	// Token: 0x060055DB RID: 21979 RVA: 0x0027FFC4 File Offset: 0x0027E1C4
	public void Sim200msLast(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 0.2f)
		{
			return;
		}
		this.elapsedTime -= 0.2f;
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.wattsUsed = 0f;
			this.activeGenerators.Clear();
			List<Generator> list = circuitInfo.generators;
			List<IEnergyConsumer> list2 = circuitInfo.consumers;
			List<Battery> batteries = circuitInfo.batteries;
			List<Generator> outputTransformers = circuitInfo.outputTransformers;
			batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			bool flag = false;
			bool flag2 = list.Count > 0;
			for (int j = 0; j < list.Count; j++)
			{
				Generator generator = list[j];
				if (generator.JoulesAvailable > 0f)
				{
					flag = true;
					this.activeGenerators.Add(generator);
				}
			}
			this.activeGenerators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			if (!flag)
			{
				for (int k = 0; k < outputTransformers.Count; k++)
				{
					if (outputTransformers[k].JoulesAvailable > 0f)
					{
						flag = true;
					}
				}
			}
			float num = 1f;
			for (int l = 0; l < batteries.Count; l++)
			{
				Battery battery = batteries[l];
				if (battery.JoulesAvailable > 0f)
				{
					flag = true;
				}
				num = Mathf.Min(num, battery.PercentFull);
			}
			for (int m = 0; m < circuitInfo.inputTransformers.Count; m++)
			{
				Battery battery2 = circuitInfo.inputTransformers[m];
				num = Mathf.Min(num, battery2.PercentFull);
			}
			circuitInfo.minBatteryPercentFull = num;
			if (flag)
			{
				for (int n = 0; n < list2.Count; n++)
				{
					IEnergyConsumer energyConsumer = list2[n];
					float num2 = energyConsumer.WattsUsed * 0.2f;
					if (num2 > 0f)
					{
						bool flag3 = false;
						for (int num3 = 0; num3 < this.activeGenerators.Count; num3++)
						{
							Generator g = this.activeGenerators[num3];
							num2 = this.PowerFromGenerator(num2, g, energyConsumer);
							if (num2 <= 0f)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							for (int num4 = 0; num4 < outputTransformers.Count; num4++)
							{
								Generator g2 = outputTransformers[num4];
								num2 = this.PowerFromGenerator(num2, g2, energyConsumer);
								if (num2 <= 0f)
								{
									flag3 = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							num2 = this.PowerFromBatteries(num2, batteries, energyConsumer);
							flag3 = (num2 <= 0.01f);
						}
						if (flag3)
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed;
						}
						else
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed - num2 / 0.2f;
						}
						energyConsumer.SetConnectionStatus(flag3 ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
					else
					{
						energyConsumer.SetConnectionStatus(flag ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
				}
			}
			else if (flag2)
			{
				for (int num5 = 0; num5 < list2.Count; num5++)
				{
					list2[num5].SetConnectionStatus(CircuitManager.ConnectionStatus.Unpowered);
				}
			}
			else
			{
				for (int num6 = 0; num6 < list2.Count; num6++)
				{
					list2[num6].SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
				}
			}
			this.circuitInfo[i] = circuitInfo;
		}
		for (int num7 = 0; num7 < this.circuitInfo.Count; num7++)
		{
			CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[num7];
			circuitInfo2.batteries.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.inputTransformers.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.generators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num8 = 0f;
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.generators, ref num8);
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.outputTransformers, ref num8);
			float num9 = 0f;
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.generators, ref num9);
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.outputTransformers, ref num9);
			circuitInfo2.minBatteryPercentFull = 1f;
			for (int num10 = 0; num10 < circuitInfo2.batteries.Count; num10++)
			{
				float percentFull = circuitInfo2.batteries[num10].PercentFull;
				if (percentFull < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull;
				}
			}
			for (int num11 = 0; num11 < circuitInfo2.inputTransformers.Count; num11++)
			{
				float percentFull2 = circuitInfo2.inputTransformers[num11].PercentFull;
				if (percentFull2 < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull2;
				}
			}
			circuitInfo2.wattsUsed += num8 / 0.2f;
			this.circuitInfo[num7] = circuitInfo2;
		}
		for (int num12 = 0; num12 < this.circuitInfo.Count; num12++)
		{
			CircuitManager.CircuitInfo circuitInfo3 = this.circuitInfo[num12];
			circuitInfo3.batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num13 = 0f;
			this.ChargeTransformers<Battery>(circuitInfo3.inputTransformers, circuitInfo3.batteries, ref num13);
			circuitInfo3.wattsUsed += num13 / 0.2f;
			this.circuitInfo[num12] = circuitInfo3;
		}
		for (int num14 = 0; num14 < this.circuitInfo.Count; num14++)
		{
			CircuitManager.CircuitInfo circuitInfo4 = this.circuitInfo[num14];
			bool is_connected_to_something_useful = circuitInfo4.generators.Count + circuitInfo4.consumers.Count + circuitInfo4.outputTransformers.Count > 0;
			this.UpdateBatteryConnectionStatus(circuitInfo4.batteries, is_connected_to_something_useful, num14);
			bool flag4 = circuitInfo4.generators.Count > 0 || circuitInfo4.outputTransformers.Count > 0;
			if (!flag4)
			{
				using (List<Battery>.Enumerator enumerator = circuitInfo4.batteries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.JoulesAvailable > 0f)
						{
							flag4 = true;
							break;
						}
					}
				}
			}
			this.UpdateBatteryConnectionStatus(circuitInfo4.inputTransformers, flag4, num14);
			this.circuitInfo[num14] = circuitInfo4;
			for (int num15 = 0; num15 < circuitInfo4.generators.Count; num15++)
			{
				Generator generator2 = circuitInfo4.generators[num15];
				ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -generator2.JoulesAvailable, StringFormatter.Replace(BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", generator2.gameObject.GetProperName()), null);
			}
		}
		for (int num16 = 0; num16 < this.circuitInfo.Count; num16++)
		{
			CircuitManager.CircuitInfo circuitInfo5 = this.circuitInfo[num16];
			this.CheckCircuitOverloaded(0.2f, num16, circuitInfo5.wattsUsed);
		}
	}

	// Token: 0x060055DC RID: 21980 RVA: 0x00280758 File Offset: 0x0027E958
	private float PowerFromBatteries(float joules_needed, List<Battery> batteries, IEnergyConsumer c)
	{
		int num2;
		do
		{
			float num = this.GetBatteryJoulesAvailable(batteries, out num2) * (float)num2;
			float num3 = (num < joules_needed) ? num : joules_needed;
			joules_needed -= num3;
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num3, c.Name, null);
			float joules = num3 / (float)num2;
			for (int i = batteries.Count - num2; i < batteries.Count; i++)
			{
				batteries[i].ConsumeEnergy(joules);
			}
		}
		while (joules_needed >= 0.01f && num2 > 0);
		return joules_needed;
	}

	// Token: 0x060055DD RID: 21981 RVA: 0x002807D4 File Offset: 0x0027E9D4
	private float PowerFromGenerator(float joules_needed, Generator g, IEnergyConsumer c)
	{
		float num = Mathf.Min(g.JoulesAvailable, joules_needed);
		joules_needed -= num;
		g.ApplyDeltaJoules(-num, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num, c.Name, null);
		return joules_needed;
	}

	// Token: 0x060055DE RID: 21982 RVA: 0x00280814 File Offset: 0x0027EA14
	private void ChargeBatteries(List<Battery> sink_batteries, List<Generator> source_generators, ref float joules_used)
	{
		if (sink_batteries.Count == 0)
		{
			return;
		}
		foreach (Generator generator in source_generators)
		{
			for (bool flag = true; flag && generator.JoulesAvailable >= 1f; flag = this.ChargeBatteriesFromGenerator(sink_batteries, generator, ref joules_used))
			{
			}
		}
	}

	// Token: 0x060055DF RID: 21983 RVA: 0x00280884 File Offset: 0x0027EA84
	private bool ChargeBatteriesFromGenerator(List<Battery> sink_batteries, Generator source_generator, ref float joules_used)
	{
		float num = source_generator.JoulesAvailable;
		float num2 = 0f;
		for (int i = 0; i < sink_batteries.Count; i++)
		{
			Battery battery = sink_batteries[i];
			if (battery != null && source_generator != null && battery.gameObject != source_generator.gameObject)
			{
				float num3 = battery.Capacity - battery.JoulesAvailable;
				if (num3 > 0f)
				{
					float num4 = Mathf.Min(num3, num / (float)(sink_batteries.Count - i));
					battery.AddEnergy(num4);
					num -= num4;
					num2 += num4;
				}
			}
		}
		if (num2 > 0f)
		{
			source_generator.ApplyDeltaJoules(-num2, false);
			joules_used += num2;
			return true;
		}
		return false;
	}

	// Token: 0x060055E0 RID: 21984 RVA: 0x00280934 File Offset: 0x0027EB34
	private void UpdateBatteryConnectionStatus(List<Battery> batteries, bool is_connected_to_something_useful, int circuit_id)
	{
		foreach (Battery battery in batteries)
		{
			if (!(battery == null))
			{
				if (battery.powerTransformer == null)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.NotConnected);
				}
				else if ((int)this.GetCircuitID(battery) == circuit_id)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
				}
			}
		}
	}

	// Token: 0x060055E1 RID: 21985 RVA: 0x002809B8 File Offset: 0x0027EBB8
	private void ChargeTransformer<T>(Battery sink_transformer, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		if (source_energy_producers.Count <= 0)
		{
			return;
		}
		float num = Mathf.Min(sink_transformer.Capacity - sink_transformer.JoulesAvailable, sink_transformer.ChargeCapacity);
		if (num <= 0f)
		{
			return;
		}
		float num2 = num;
		float num3 = 0f;
		for (int i = 0; i < source_energy_producers.Count; i++)
		{
			T t = source_energy_producers[i];
			if (t.JoulesAvailable > 0f)
			{
				float num4 = Mathf.Min(t.JoulesAvailable, num2 / (float)(source_energy_producers.Count - i));
				t.ConsumeEnergy(num4);
				num2 -= num4;
				num3 += num4;
			}
		}
		sink_transformer.AddEnergy(num3);
		joules_used += num3;
	}

	// Token: 0x060055E2 RID: 21986 RVA: 0x00280A6C File Offset: 0x0027EC6C
	private void ChargeTransformers<T>(List<Battery> sink_transformers, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		foreach (Battery sink_transformer in sink_transformers)
		{
			this.ChargeTransformer<T>(sink_transformer, source_energy_producers, ref joules_used);
		}
	}

	// Token: 0x060055E3 RID: 21987 RVA: 0x00280ABC File Offset: 0x0027ECBC
	private void CheckCircuitOverloaded(float dt, int id, float watts_used)
	{
		UtilityNetwork networkByID = Game.Instance.electricalConduitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)networkByID;
			if (electricalUtilityNetwork != null)
			{
				electricalUtilityNetwork.UpdateOverloadTime(dt, watts_used, this.circuitInfo[id].bridgeGroups);
			}
		}
	}

	// Token: 0x060055E4 RID: 21988 RVA: 0x000D805F File Offset: 0x000D625F
	public float GetWattsUsedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		return this.circuitInfo[(int)circuitID].wattsUsed;
	}

	// Token: 0x060055E5 RID: 21989 RVA: 0x00280B00 File Offset: 0x0027ED00
	public float GetWattsNeededWhenActive(ushort originCircuitId)
	{
		if (originCircuitId == 65535)
		{
			return -1f;
		}
		HashSet<ushort> hashSet = new HashSet<ushort>();
		HashSet<ushort> hashSet2 = new HashSet<ushort>();
		HashSet<ushort> hashSet3 = new HashSet<ushort>();
		hashSet2.Add(originCircuitId);
		int num = 0;
		while (hashSet2.Count > 0)
		{
			num++;
			if (num > 100)
			{
				break;
			}
			foreach (ushort num2 in hashSet2)
			{
				if (num2 >= 0 && (int)num2 < this.circuitInfo.Count)
				{
					foreach (Battery battery in this.circuitInfo[(int)num2].inputTransformers)
					{
						ushort circuitID = battery.powerTransformer.CircuitID;
						if (battery.powerTransformer.CircuitID != 65535)
						{
							hashSet3.Add(circuitID);
						}
					}
					hashSet.Add(num2);
				}
			}
			hashSet2.Clear();
			foreach (ushort item in hashSet3)
			{
				if (!hashSet.Contains(item))
				{
					hashSet2.Add(item);
				}
			}
			hashSet3.Clear();
		}
		HashSet<ushort> hashSet4 = hashSet;
		Dictionary<ushort, float> dictionary = new Dictionary<ushort, float>();
		foreach (ushort num3 in hashSet4)
		{
			if (num3 >= 0 && (int)num3 < this.circuitInfo.Count)
			{
				float num4 = 0f;
				foreach (IEnergyConsumer energyConsumer in this.circuitInfo[(int)num3].consumers)
				{
					num4 += energyConsumer.WattsNeededWhenActive;
				}
				dictionary.Add(num3, num4);
			}
		}
		Dictionary<ushort, float> dictionary2 = new Dictionary<ushort, float>();
		foreach (Battery battery2 in this.circuitInfo[(int)originCircuitId].inputTransformers)
		{
			float b;
			dictionary.TryGetValue(battery2.powerTransformer.CircuitID, out b);
			float b2 = Mathf.Min(battery2.powerTransformer.WattageRating, b);
			float a;
			dictionary2.TryGetValue(battery2.powerTransformer.CircuitID, out a);
			dictionary2[battery2.powerTransformer.CircuitID] = Mathf.Max(a, b2);
		}
		float num5;
		dictionary.TryGetValue(originCircuitId, out num5);
		foreach (KeyValuePair<ushort, float> keyValuePair in dictionary2)
		{
			ushort num6;
			float num7;
			keyValuePair.Deconstruct(out num6, out num7);
			float num8 = num7;
			num5 += num8;
		}
		return num5;
	}

	// Token: 0x060055E6 RID: 21990 RVA: 0x00280E40 File Offset: 0x0027F040
	public float GetWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			if (!(generator == null) && generator.IsProducingPower())
			{
				num += generator.WattageRating;
			}
		}
		return num;
	}

	// Token: 0x060055E7 RID: 21991 RVA: 0x00280EC8 File Offset: 0x0027F0C8
	public float GetPotentialWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			num += generator.WattageRating;
		}
		return num;
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x00280F40 File Offset: 0x0027F140
	public float GetJoulesAvailableOnCircuit(ushort circuitID)
	{
		int num;
		return this.GetBatteryJoulesAvailable(this.GetBatteriesOnCircuit(circuitID), out num) * (float)num;
	}

	// Token: 0x060055E9 RID: 21993 RVA: 0x000D8080 File Offset: 0x000D6280
	public ReadOnlyCollection<Generator> GetGeneratorsOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].generators.AsReadOnly();
	}

	// Token: 0x060055EA RID: 21994 RVA: 0x000D80A2 File Offset: 0x000D62A2
	public ReadOnlyCollection<IEnergyConsumer> GetConsumersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].consumers.AsReadOnly();
	}

	// Token: 0x060055EB RID: 21995 RVA: 0x000D80C4 File Offset: 0x000D62C4
	public ReadOnlyCollection<Battery> GetTransformersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].inputTransformers.AsReadOnly();
	}

	// Token: 0x060055EC RID: 21996 RVA: 0x000D80E6 File Offset: 0x000D62E6
	public List<Battery> GetBatteriesOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].batteries;
	}

	// Token: 0x060055ED RID: 21997 RVA: 0x000D8103 File Offset: 0x000D6303
	public float GetMinBatteryPercentFullOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		return this.circuitInfo[(int)circuitID].minBatteryPercentFull;
	}

	// Token: 0x060055EE RID: 21998 RVA: 0x000D8124 File Offset: 0x000D6324
	public bool HasBatteries(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].batteries.Count + this.circuitInfo[(int)circuitID].inputTransformers.Count > 0;
	}

	// Token: 0x060055EF RID: 21999 RVA: 0x000D8160 File Offset: 0x000D6360
	public bool HasGenerators(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].generators.Count + this.circuitInfo[(int)circuitID].outputTransformers.Count > 0;
	}

	// Token: 0x060055F0 RID: 22000 RVA: 0x000D819C File Offset: 0x000D639C
	public bool HasGenerators()
	{
		return this.generators.Count > 0;
	}

	// Token: 0x060055F1 RID: 22001 RVA: 0x000D81AC File Offset: 0x000D63AC
	public bool HasConsumers(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].consumers.Count > 0;
	}

	// Token: 0x060055F2 RID: 22002 RVA: 0x00280F60 File Offset: 0x0027F160
	public float GetMaxSafeWattageForCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkByID((int)circuitID) as ElectricalUtilityNetwork;
		if (electricalUtilityNetwork == null)
		{
			return 0f;
		}
		return electricalUtilityNetwork.GetMaxSafeWattage();
	}

	// Token: 0x04003C3A RID: 15418
	public const ushort INVALID_ID = 65535;

	// Token: 0x04003C3B RID: 15419
	private const int SimUpdateSortKey = 1000;

	// Token: 0x04003C3C RID: 15420
	private const float MIN_POWERED_THRESHOLD = 0.01f;

	// Token: 0x04003C3D RID: 15421
	private bool dirty = true;

	// Token: 0x04003C3E RID: 15422
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x04003C3F RID: 15423
	private HashSet<IEnergyConsumer> consumers = new HashSet<IEnergyConsumer>();

	// Token: 0x04003C40 RID: 15424
	private HashSet<WireUtilityNetworkLink> bridges = new HashSet<WireUtilityNetworkLink>();

	// Token: 0x04003C41 RID: 15425
	private float elapsedTime;

	// Token: 0x04003C42 RID: 15426
	private List<CircuitManager.CircuitInfo> circuitInfo = new List<CircuitManager.CircuitInfo>();

	// Token: 0x04003C43 RID: 15427
	private List<IEnergyConsumer> consumersShadow = new List<IEnergyConsumer>();

	// Token: 0x04003C44 RID: 15428
	private List<Generator> activeGenerators = new List<Generator>();

	// Token: 0x02001071 RID: 4209
	private struct CircuitInfo
	{
		// Token: 0x04003C45 RID: 15429
		public List<Generator> generators;

		// Token: 0x04003C46 RID: 15430
		public List<IEnergyConsumer> consumers;

		// Token: 0x04003C47 RID: 15431
		public List<Battery> batteries;

		// Token: 0x04003C48 RID: 15432
		public List<Battery> inputTransformers;

		// Token: 0x04003C49 RID: 15433
		public List<Generator> outputTransformers;

		// Token: 0x04003C4A RID: 15434
		public List<WireUtilityNetworkLink>[] bridgeGroups;

		// Token: 0x04003C4B RID: 15435
		public float minBatteryPercentFull;

		// Token: 0x04003C4C RID: 15436
		public float wattsUsed;
	}

	// Token: 0x02001072 RID: 4210
	public enum ConnectionStatus
	{
		// Token: 0x04003C4E RID: 15438
		NotConnected,
		// Token: 0x04003C4F RID: 15439
		Unpowered,
		// Token: 0x04003C50 RID: 15440
		Powered
	}
}
