using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C88 RID: 3208
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x000C801B File Offset: 0x000C621B
	// (set) Token: 0x06003DB4 RID: 15796 RVA: 0x000C8023 File Offset: 0x000C6223
	public float lastEnvTemp { get; private set; }

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x000C802C File Offset: 0x000C622C
	// (set) Token: 0x06003DB6 RID: 15798 RVA: 0x000C8034 File Offset: 0x000C6234
	public float lastGasTemp { get; private set; }

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x000C803D File Offset: 0x000C623D
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x000C8045 File Offset: 0x000C6245
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
		base.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00232118 File Offset: 0x00230318
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.gameObject.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
		this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x000C806F File Offset: 0x000C626F
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.UpdateState(dt);
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x000C80A1 File Offset: 0x000C62A1
	private static bool UpdateStateCb(int cell, object data)
	{
		AirConditioner airConditioner = data as AirConditioner;
		airConditioner.cellCount++;
		airConditioner.envTemp += Grid.Temperature[cell];
		return true;
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x002321A8 File Offset: 0x002303A8
	private void UpdateState(float dt)
	{
		bool value = this.consumer.IsSatisfied;
		this.envTemp = 0f;
		this.cellCount = 0;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, AirConditioner.UpdateStateCbDelegate);
			this.envTemp /= (float)this.cellCount;
		}
		this.lastEnvTemp = this.envTemp;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < items.Count; i++)
		{
			PrimaryElement component = items[i].GetComponent<PrimaryElement>();
			if (component.Mass > 0f && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
			{
				value = true;
				this.lastGasTemp = component.Temperature;
				float num = component.Temperature + this.temperatureDelta;
				if (num < 1f)
				{
					num = 1f;
					this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
				}
				else
				{
					this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0f);
				}
				float num2 = (this.isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, num, component.DiseaseIdx, component.DiseaseCount);
				component.KeepZeroMassObject = true;
				float num3 = num2 / component.Mass;
				int num4 = (int)((float)component.DiseaseCount * num3);
				component.Mass -= num2;
				component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
				float num5 = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
				float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
				this.lastSampleTime = Time.time;
				this.heatEffect.SetHeatBeingProducedValue(Mathf.Abs(num5));
				GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num5, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
				break;
			}
		}
		if (Time.time - this.lastSampleTime > 2f)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0f, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
			this.lastSampleTime = Time.time;
		}
		this.operational.SetActive(value, false);
		this.UpdateStatus();
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x000C80CF File Offset: 0x000C62CF
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateState(0f);
		}
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x000C80E9 File Offset: 0x000C62E9
	private void OnActiveChanged(object data)
	{
		this.UpdateStatus();
		if (this.operational.IsActive)
		{
			this.heatEffect.enabled = true;
			return;
		}
		this.heatEffect.enabled = false;
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00232460 File Offset: 0x00230660
	private void UpdateStatus()
	{
		if (this.operational.IsActive)
		{
			if (this.lowTempLag >= 1f && !this.showingLowTemp)
			{
				this.statusHandle = (this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, this));
				this.showingLowTemp = true;
				this.showingHotEnv = false;
				return;
			}
			if (this.lowTempLag <= 0f && (this.showingHotEnv || this.showingLowTemp))
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
			if (this.statusHandle == Guid.Empty)
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
		}
		else
		{
			this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x002325D4 File Offset: 0x002307D4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
		Element element = ElementLoader.FindElementByName(this.isLiquidConditioner ? "Water" : "Oxygen");
		float num;
		if (this.isLiquidConditioner)
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 10000f);
		}
		else
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 1000f);
		}
		float dtu = num * 1f;
		Descriptor item = default(Descriptor);
		string txt = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		string tooltip = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Descriptor item2 = default(Descriptor);
		item2.SetupDescriptor(string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.GASCOOLING, formattedTemperature), string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING, formattedTemperature), Descriptor.DescriptorType.Effect);
		list.Add(item2);
		return list;
	}

	// Token: 0x04002A00 RID: 10752
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002A01 RID: 10753
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04002A02 RID: 10754
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002A03 RID: 10755
	[MyCmpReq]
	private ConduitConsumer consumer;

	// Token: 0x04002A04 RID: 10756
	[MyCmpReq]
	private BuildingComplete building;

	// Token: 0x04002A05 RID: 10757
	[MyCmpGet]
	private OccupyArea occupyArea;

	// Token: 0x04002A06 RID: 10758
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04002A07 RID: 10759
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04002A08 RID: 10760
	public float temperatureDelta = -14f;

	// Token: 0x04002A09 RID: 10761
	public float maxEnvironmentDelta = -50f;

	// Token: 0x04002A0A RID: 10762
	private float lowTempLag;

	// Token: 0x04002A0B RID: 10763
	private bool showingLowTemp;

	// Token: 0x04002A0C RID: 10764
	public bool isLiquidConditioner;

	// Token: 0x04002A0D RID: 10765
	private bool showingHotEnv;

	// Token: 0x04002A10 RID: 10768
	private Guid statusHandle;

	// Token: 0x04002A11 RID: 10769
	[Serialize]
	private float targetTemperature;

	// Token: 0x04002A12 RID: 10770
	private int cooledAirOutputCell = -1;

	// Token: 0x04002A13 RID: 10771
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002A14 RID: 10772
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04002A15 RID: 10773
	private float lastSampleTime = -1f;

	// Token: 0x04002A16 RID: 10774
	private float envTemp;

	// Token: 0x04002A17 RID: 10775
	private int cellCount;

	// Token: 0x04002A18 RID: 10776
	private static readonly Func<int, object, bool> UpdateStateCbDelegate = (int cell, object data) => AirConditioner.UpdateStateCb(cell, data);
}
