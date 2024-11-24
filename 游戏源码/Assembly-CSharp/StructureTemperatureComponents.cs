using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020019BB RID: 6587
public class StructureTemperatureComponents : KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
	// Token: 0x0600892A RID: 35114 RVA: 0x00356284 File Offset: 0x00354484
	public HandleVector<int>.Handle Add(GameObject go)
	{
		StructureTemperaturePayload structureTemperaturePayload = new StructureTemperaturePayload(go);
		return base.Add(go, new StructureTemperatureHeader
		{
			dirty = false,
			simHandle = -1,
			isActiveBuilding = false
		}, ref structureTemperaturePayload);
	}

	// Token: 0x0600892B RID: 35115 RVA: 0x000F9CF3 File Offset: 0x000F7EF3
	public static void ClearInstanceMap()
	{
		StructureTemperatureComponents.handleInstanceMap.Clear();
	}

	// Token: 0x0600892D RID: 35117 RVA: 0x003562C4 File Offset: 0x003544C4
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		this.InitializeStatusItem();
		base.OnPrefabInit(handle);
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
		structureTemperaturePayload.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
		new_data.isActiveBuilding = (structureTemperaturePayload.building.Def.SelfHeatKilowattsWhenActive != 0f || structureTemperaturePayload.ExhaustKilowatts != 0f);
		base.SetHeader(handle, new_data);
	}

	// Token: 0x0600892E RID: 35118 RVA: 0x00356354 File Offset: 0x00354554
	private void InitializeStatusItem()
	{
		if (this.operatingEnergyStatusItem != null)
		{
			return;
		}
		this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.operatingEnergyStatusItem.resolveStringCallback = delegate(string str, object ev_data)
		{
			int key = (int)ev_data;
			HandleVector<int>.Handle handle = StructureTemperatureComponents.handleInstanceMap[key];
			StructureTemperaturePayload payload = base.GetPayload(handle);
			if (str != BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
			{
				try
				{
					return string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				}
				catch (Exception obj)
				{
					global::Debug.LogWarning(obj);
					global::Debug.LogWarning(BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
					global::Debug.LogWarning(str);
					return str;
				}
			}
			string text = "";
			foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
			{
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, energySource.source, GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
			}
			str = string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), text);
			return str;
		};
	}

	// Token: 0x0600892F RID: 35119 RVA: 0x003563AC File Offset: 0x003545AC
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		if (structureTemperaturePayload.operational != null && structureTemperatureHeader.isActiveBuilding)
		{
			structureTemperaturePayload.primaryElement.Subscribe(824508782, delegate(object ev_data)
			{
				StructureTemperatureComponents.OnActiveChanged(handle);
			});
		}
		structureTemperaturePayload.maxTemperature = ((structureTemperaturePayload.overheatable != null) ? structureTemperaturePayload.overheatable.OverheatTemperature : 10000f);
		if (structureTemperaturePayload.maxTemperature <= 0f)
		{
			global::Debug.LogError("invalid max temperature");
		}
		base.SetPayload(handle, ref structureTemperaturePayload);
		this.SimRegister(handle, ref structureTemperatureHeader, ref structureTemperaturePayload);
	}

	// Token: 0x06008930 RID: 35120 RVA: 0x00356468 File Offset: 0x00354668
	private static void OnActiveChanged(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = structureTemperaturePayload.Temperature;
		new_data.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, new_data);
	}

	// Token: 0x06008931 RID: 35121 RVA: 0x000F9D07 File Offset: 0x000F7F07
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		this.SimUnregister(handle);
		base.OnCleanUp(handle);
	}

	// Token: 0x06008932 RID: 35122 RVA: 0x003564AC File Offset: 0x003546AC
	public override void Sim200ms(float dt)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<StructureTemperatureHeader> list;
		List<StructureTemperaturePayload> list2;
		base.GetDataLists(out list, out list2);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList.Capacity = Math.Max(pooledList.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList2 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList2.Capacity = Math.Max(pooledList2.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList3 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList3.Capacity = Math.Max(pooledList3.Capacity, list.Count);
		for (int num4 = 0; num4 != list.Count; num4++)
		{
			StructureTemperatureHeader structureTemperatureHeader = list[num4];
			if (Sim.IsValidHandle(structureTemperatureHeader.simHandle))
			{
				pooledList.Add(num4);
				if (structureTemperatureHeader.dirty)
				{
					pooledList2.Add(num4);
					structureTemperatureHeader.dirty = false;
					list[num4] = structureTemperatureHeader;
				}
				if (structureTemperatureHeader.isActiveBuilding)
				{
					pooledList3.Add(num4);
				}
			}
		}
		foreach (int index in pooledList2)
		{
			StructureTemperaturePayload structureTemperaturePayload = list2[index];
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
		}
		foreach (int index2 in pooledList2)
		{
			if (list2[index2].pendingEnergyModifications != 0f)
			{
				StructureTemperaturePayload structureTemperaturePayload2 = list2[index2];
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload2.simHandleCopy, structureTemperaturePayload2.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload2.pendingEnergyModifications = 0f;
				list2[index2] = structureTemperaturePayload2;
			}
		}
		foreach (int index3 in pooledList3)
		{
			StructureTemperaturePayload structureTemperaturePayload3 = list2[index3];
			if (structureTemperaturePayload3.operational == null || structureTemperaturePayload3.operational.IsActive)
			{
				num++;
				if (!structureTemperaturePayload3.isActiveStatusItemSet)
				{
					num3++;
					structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, structureTemperaturePayload3.simHandleCopy);
					structureTemperaturePayload3.isActiveStatusItemSet = true;
				}
				structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.OperatingKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
				if (structureTemperaturePayload3.ExhaustKilowatts != 0f)
				{
					num2++;
					StructureTemperatureComponents.ExhaustHeat(structureTemperaturePayload3.GetExtents(), structureTemperaturePayload3.ExhaustKilowatts, structureTemperaturePayload3.maxTemperature, dt);
					structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.ExhaustKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
				}
			}
			else if (structureTemperaturePayload3.isActiveStatusItemSet)
			{
				num3++;
				structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, null, null);
				structureTemperaturePayload3.isActiveStatusItemSet = false;
			}
			list2[index3] = structureTemperaturePayload3;
		}
		pooledList3.Recycle();
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06008933 RID: 35123 RVA: 0x0035681C File Offset: 0x00354A1C
	public static void ExhaustHeat(Extents extents, float kw, float maxTemperature, float dt)
	{
		int num = extents.width * extents.height;
		float num2 = kw * dt / (float)num;
		for (int i = 0; i < extents.height; i++)
		{
			int num3 = extents.y + i;
			for (int j = 0; j < extents.width; j++)
			{
				int num4 = extents.x + j;
				int num5 = num3 * Grid.WidthInCells + num4;
				float num6 = Mathf.Min(Grid.Mass[num5], 1.5f) / 1.5f;
				float kilojoules = num2 * num6;
				SimMessages.ModifyEnergy(num5, kilojoules, maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
			}
		}
	}

	// Token: 0x06008934 RID: 35124 RVA: 0x003568B8 File Offset: 0x00354AB8
	private static void UpdateSimState(ref StructureTemperaturePayload payload)
	{
		DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
		float internalTemperature = payload.primaryElement.InternalTemperature;
		BuildingDef def = payload.building.Def;
		float mass = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		float overheat_temperature = (payload.overheatable != null) ? payload.overheatable.OverheatTemperature : 10000f;
		if (!payload.enabled || payload.bypass)
		{
			mass = 0f;
		}
		Extents extents = payload.GetExtents();
		ushort idx = payload.primaryElement.Element.idx;
		if (payload.heatEffect != null)
		{
			float num = (payload.operational == null || payload.operational.IsActive) ? payload.ExhaustKilowatts : 0f;
			payload.heatEffect.SetHeatBeingProducedValue(Mathf.Clamp(operatingKilowatts + num, 0f, float.MaxValue));
		}
		SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
	}

	// Token: 0x06008935 RID: 35125 RVA: 0x003569C0 File Offset: 0x00354BC0
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
		float result;
		if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
		{
			if (!payload.bypass)
			{
				int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
				result = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
			}
			else
			{
				int i = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
				result = Grid.Temperature[i];
			}
		}
		else
		{
			result = payload.primaryElement.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06008936 RID: 35126 RVA: 0x00356A6C File Offset: 0x00354C6C
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = temperature;
		structureTemperatureHeader.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, structureTemperatureHeader);
		if (!structureTemperatureHeader.isActiveBuilding && Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
		{
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
			if (structureTemperaturePayload.pendingEnergyModifications != 0f)
			{
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload.simHandleCopy, structureTemperaturePayload.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload.pendingEnergyModifications = 0f;
				GameComps.StructureTemperatures.SetPayload(handle, ref structureTemperaturePayload);
			}
		}
	}

	// Token: 0x06008937 RID: 35127 RVA: 0x00356B18 File Offset: 0x00354D18
	public void ProduceEnergy(HandleVector<int>.Handle handle, float delta_kilojoules, string source, float display_dt)
	{
		StructureTemperaturePayload payload = base.GetPayload(handle);
		if (Sim.IsValidHandle(payload.simHandleCopy))
		{
			SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0f, 10000f);
		}
		else
		{
			payload.pendingEnergyModifications += delta_kilojoules;
			StructureTemperatureHeader header = base.GetHeader(handle);
			header.dirty = true;
			base.SetHeader(handle, header);
		}
		payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
		base.SetPayload(handle, ref payload);
	}

	// Token: 0x06008938 RID: 35128 RVA: 0x00356B98 File Offset: 0x00354D98
	private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(List<StructureTemperaturePayload.EnergySource> sources, float kw, string source)
	{
		if (sources == null)
		{
			sources = new List<StructureTemperaturePayload.EnergySource>();
		}
		bool flag = false;
		for (int i = 0; i < sources.Count; i++)
		{
			if (sources[i].source == source)
			{
				sources[i].Accumulate(kw);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
		}
		return sources;
	}

	// Token: 0x06008939 RID: 35129 RVA: 0x00356BF8 File Offset: 0x00354DF8
	public static void DoStateTransition(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
		}
	}

	// Token: 0x0600893A RID: 35130 RVA: 0x00356C34 File Offset: 0x00354E34
	public static void DoMelt(PrimaryElement primary_element)
	{
		Element element = primary_element.Element;
		if (element.highTempTransitionTarget != SimHashes.Unobtanium)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
			Building.CreateBuildingMeltedNotification(primary_element.gameObject);
			Util.KDestroyGameObject(primary_element.gameObject);
		}
	}

	// Token: 0x0600893B RID: 35131 RVA: 0x00356CB0 File Offset: 0x00354EB0
	public static void DoOverheat(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615, null);
		}
	}

	// Token: 0x0600893C RID: 35132 RVA: 0x00356CF8 File Offset: 0x00354EF8
	public static void DoNoLongerOverheated(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937, null);
		}
	}

	// Token: 0x0600893D RID: 35133 RVA: 0x000F9D17 File Offset: 0x000F7F17
	public bool IsEnabled(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).enabled;
	}

	// Token: 0x0600893E RID: 35134 RVA: 0x00356D40 File Offset: 0x00354F40
	private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.enabled = isEnabled;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x0600893F RID: 35135 RVA: 0x000F9D25 File Offset: 0x000F7F25
	public void Enable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, true);
	}

	// Token: 0x06008940 RID: 35136 RVA: 0x000F9D2F File Offset: 0x000F7F2F
	public void Disable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, false);
	}

	// Token: 0x06008941 RID: 35137 RVA: 0x000F9D39 File Offset: 0x000F7F39
	public bool IsBypassed(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).bypass;
	}

	// Token: 0x06008942 RID: 35138 RVA: 0x00356D74 File Offset: 0x00354F74
	private void Bypass(HandleVector<int>.Handle handle, bool bypass)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.bypass = bypass;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x06008943 RID: 35139 RVA: 0x000F9D47 File Offset: 0x000F7F47
	public void Bypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, true);
	}

	// Token: 0x06008944 RID: 35140 RVA: 0x000F9D51 File Offset: 0x000F7F51
	public void UnBypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, false);
	}

	// Token: 0x06008945 RID: 35141 RVA: 0x00356DA8 File Offset: 0x00354FA8
	protected void SimRegister(HandleVector<int>.Handle handle, ref StructureTemperatureHeader header, ref StructureTemperaturePayload payload)
	{
		if (payload.simHandleCopy != -1)
		{
			return;
		}
		PrimaryElement primaryElement = payload.primaryElement;
		if (primaryElement.Mass <= 0f)
		{
			return;
		}
		if (primaryElement.Element.IsTemperatureInsulated)
		{
			return;
		}
		payload.simHandleCopy = -2;
		string dbg_name = primaryElement.name;
		HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle2 = Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name);
		}, null, "StructureTemperature.SimRegister");
		BuildingDef def = primaryElement.GetComponent<Building>().Def;
		float internalTemperature = primaryElement.InternalTemperature;
		float massForTemperatureModification = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		Extents extents = payload.GetExtents();
		ushort idx = primaryElement.Element.idx;
		SimMessages.AddBuildingHeatExchange(extents, massForTemperatureModification, internalTemperature, def.ThermalConductivity, operatingKilowatts, idx, handle2.index);
		header.simHandle = payload.simHandleCopy;
		base.SetData(handle, header, ref payload);
	}

	// Token: 0x06008946 RID: 35142 RVA: 0x00356E98 File Offset: 0x00355098
	private static void OnSimRegistered(HandleVector<int>.Handle handle, int sim_handle, string dbg_name)
	{
		if (!GameComps.StructureTemperatures.IsValid(handle))
		{
			return;
		}
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy == -2)
		{
			StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
			new_data.simHandle = sim_handle;
			structureTemperaturePayload.simHandleCopy = sim_handle;
			GameComps.StructureTemperatures.SetData(handle, new_data, ref structureTemperaturePayload);
			structureTemperaturePayload.primaryElement.Trigger(-1555603773, sim_handle);
			int cell = Grid.PosToCell(structureTemperaturePayload.building.transform.GetPosition());
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Created, structureTemperaturePayload.building, sim_handle));
			return;
		}
		SimMessages.RemoveBuildingHeatExchange(sim_handle, -1);
	}

	// Token: 0x06008947 RID: 35143 RVA: 0x00356F60 File Offset: 0x00355160
	protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
	{
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister", null);
			return;
		}
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy != -1)
		{
			int cell = Grid.PosToCell(structureTemperaturePayload.building);
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Destroyed, structureTemperaturePayload.building, structureTemperaturePayload.simHandleCopy));
			if (Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
			{
				int handleIndex = Sim.GetHandleIndex(structureTemperaturePayload.simHandleCopy);
				structureTemperaturePayload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
				SimMessages.RemoveBuildingHeatExchange(structureTemperaturePayload.simHandleCopy, -1);
				StructureTemperatureComponents.handleInstanceMap.Remove(structureTemperaturePayload.simHandleCopy);
			}
			structureTemperaturePayload.simHandleCopy = -1;
			new_data.simHandle = -1;
			base.SetData(handle, new_data, ref structureTemperaturePayload);
		}
	}

	// Token: 0x04006750 RID: 26448
	private const float MAX_PRESSURE = 1.5f;

	// Token: 0x04006751 RID: 26449
	private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();

	// Token: 0x04006752 RID: 26450
	private StatusItem operatingEnergyStatusItem;
}
