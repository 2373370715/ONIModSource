using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FA8 RID: 4008
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : StateMachineComponent<SpaceHeater.StatesInstance>, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06005106 RID: 20742 RVA: 0x000D4D42 File Offset: 0x000D2F42
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x06005107 RID: 20743 RVA: 0x000D4D4A File Offset: 0x000D2F4A
	public float MaxPower
	{
		get
		{
			return 240f;
		}
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06005108 RID: 20744 RVA: 0x000D4D51 File Offset: 0x000D2F51
	public float MinPower
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06005109 RID: 20745 RVA: 0x000D4D58 File Offset: 0x000D2F58
	public float MaxSelfHeatKWs
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x0600510A RID: 20746 RVA: 0x000D4D5F File Offset: 0x000D2F5F
	public float MinSelfHeatKWs
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x0600510B RID: 20747 RVA: 0x000D4D66 File Offset: 0x000D2F66
	public float MaxExhaustedKWs
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x0600510C RID: 20748 RVA: 0x000D4D6D File Offset: 0x000D2F6D
	public float MinExhaustedKWs
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x0600510D RID: 20749 RVA: 0x000D4D74 File Offset: 0x000D2F74
	public float CurrentSelfHeatKW
	{
		get
		{
			return Mathf.Lerp(this.MinSelfHeatKWs, this.MaxSelfHeatKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x000D4D8D File Offset: 0x000D2F8D
	public float CurrentExhaustedKW
	{
		get
		{
			return Mathf.Lerp(this.MinExhaustedKWs, this.MaxExhaustedKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600510F RID: 20751 RVA: 0x000D4DA6 File Offset: 0x000D2FA6
	public float CurrentPowerConsumption
	{
		get
		{
			return Mathf.Lerp(this.MinPower, this.MaxPower, this.UserSliderSetting);
		}
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x000D4DBF File Offset: 0x000D2FBF
	public static void GenerateHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		if (smi.master.produceHeat)
		{
			SpaceHeater.AddExhaustHeat(smi, dt);
			SpaceHeater.AddSelfHeat(smi, dt);
		}
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x00270558 File Offset: 0x0026E758
	private static float AddExhaustHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentExhaustedKW = smi.master.CurrentExhaustedKW;
		StructureTemperatureComponents.ExhaustHeat(smi.master.extents, currentExhaustedKW, smi.master.overheatTemperature, dt);
		return currentExhaustedKW;
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x00270590 File Offset: 0x0026E790
	public static void RefreshHeatEffect(SpaceHeater.StatesInstance smi)
	{
		if (smi.master.heatEffect != null && smi.master.produceHeat)
		{
			float heatBeingProducedValue = smi.IsInsideState(smi.sm.online.heating) ? (smi.master.CurrentExhaustedKW + smi.master.CurrentSelfHeatKW) : 0f;
			smi.master.heatEffect.SetHeatBeingProducedValue(heatBeingProducedValue);
		}
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x00270608 File Offset: 0x0026E808
	private static float AddSelfHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentSelfHeatKW = smi.master.CurrentSelfHeatKW;
		GameComps.StructureTemperatures.ProduceEnergy(smi.master.structureTemperature, currentSelfHeatKW * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return currentSelfHeatKW;
	}

	// Token: 0x06005114 RID: 20756 RVA: 0x00270648 File Offset: 0x0026E848
	public void SetUserSpecifiedPowerConsumptionValue(float value)
	{
		if (this.produceHeat)
		{
			this.UserSliderSetting = (value - this.MinPower) / (this.MaxPower - this.MinPower);
			SpaceHeater.RefreshHeatEffect(base.smi);
			this.energyConsumer.BaseWattageRating = this.CurrentPowerConsumption;
		}
	}

	// Token: 0x06005115 RID: 20757 RVA: 0x00270698 File Offset: 0x0026E898
	protected override void OnPrefabInit()
	{
		if (this.produceHeat)
		{
			this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.heatStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = string.Format(str, GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				return str;
			};
			this.heatStatusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				string text = string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentSelfHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentExhaustedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				str = str.Replace("{1}", text);
				return str;
			};
		}
		base.OnPrefabInit();
	}

	// Token: 0x06005116 RID: 20758 RVA: 0x00270730 File Offset: 0x0026E930
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.extents = base.GetComponent<OccupyArea>().GetExtents();
		this.overheatTemperature = base.GetComponent<BuildingComplete>().Def.OverheatTemperature;
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.smi.StartSM();
		this.SetUserSpecifiedPowerConsumptionValue(this.CurrentPowerConsumption);
	}

	// Token: 0x06005117 RID: 20759 RVA: 0x000D4DDE File Offset: 0x000D2FDE
	public void SetLiquidHeater()
	{
		this.heatLiquid = true;
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x002707D0 File Offset: 0x0026E9D0
	private SpaceHeater.MonitorState MonitorHeating(float dt)
	{
		this.monitorCells.Clear();
		GameUtil.GetNonSolidCells(Grid.PosToCell(base.transform.GetPosition()), this.radius, this.monitorCells);
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < this.monitorCells.Count; i++)
		{
			if (Grid.Mass[this.monitorCells[i]] > this.minimumCellMass && ((Grid.Element[this.monitorCells[i]].IsGas && !this.heatLiquid) || (Grid.Element[this.monitorCells[i]].IsLiquid && this.heatLiquid)))
			{
				num++;
				num2 += Grid.Temperature[this.monitorCells[i]];
			}
		}
		if (num == 0)
		{
			if (!this.heatLiquid)
			{
				return SpaceHeater.MonitorState.NotEnoughGas;
			}
			return SpaceHeater.MonitorState.NotEnoughLiquid;
		}
		else
		{
			if (num2 / (float)num >= this.targetTemperature)
			{
				return SpaceHeater.MonitorState.TooHot;
			}
			return SpaceHeater.MonitorState.ReadyToHeat;
		}
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x002708D0 File Offset: 0x0026EAD0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600511A RID: 20762 RVA: 0x000D4DE7 File Offset: 0x000D2FE7
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600511B RID: 20763 RVA: 0x000D4DEE File Offset: 0x000D2FEE
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.ELECTRICAL.WATT;
		}
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x000D4DFA File Offset: 0x000D2FFA
	public float GetSliderMin(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MinPower;
	}

	// Token: 0x0600511E RID: 20766 RVA: 0x000D4E10 File Offset: 0x000D3010
	public float GetSliderMax(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MaxPower;
	}

	// Token: 0x0600511F RID: 20767 RVA: 0x000D4E26 File Offset: 0x000D3026
	public float GetSliderValue(int index)
	{
		return this.CurrentPowerConsumption;
	}

	// Token: 0x06005120 RID: 20768 RVA: 0x000D4E2E File Offset: 0x000D302E
	public void SetSliderValue(float value, int index)
	{
		this.SetUserSpecifiedPowerConsumptionValue(value);
	}

	// Token: 0x06005121 RID: 20769 RVA: 0x000D4E37 File Offset: 0x000D3037
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06005122 RID: 20770 RVA: 0x000D4E3E File Offset: 0x000D303E
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP"), GameUtil.GetFormattedHeatEnergyRate((this.CurrentSelfHeatKW + this.CurrentExhaustedKW) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
	}

	// Token: 0x04003892 RID: 14482
	public float targetTemperature = 308.15f;

	// Token: 0x04003893 RID: 14483
	public float minimumCellMass;

	// Token: 0x04003894 RID: 14484
	public int radius = 2;

	// Token: 0x04003895 RID: 14485
	[SerializeField]
	private bool heatLiquid;

	// Token: 0x04003896 RID: 14486
	[Serialize]
	public float UserSliderSetting;

	// Token: 0x04003897 RID: 14487
	public bool produceHeat;

	// Token: 0x04003898 RID: 14488
	private StatusItem heatStatusItem;

	// Token: 0x04003899 RID: 14489
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x0400389A RID: 14490
	private Extents extents;

	// Token: 0x0400389B RID: 14491
	private float overheatTemperature;

	// Token: 0x0400389C RID: 14492
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400389D RID: 14493
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400389E RID: 14494
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x0400389F RID: 14495
	[MyCmpGet]
	private EnergyConsumer energyConsumer;

	// Token: 0x040038A0 RID: 14496
	private List<int> monitorCells = new List<int>();

	// Token: 0x02000FA9 RID: 4009
	public class StatesInstance : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance
	{
		// Token: 0x06005124 RID: 20772 RVA: 0x000D4E92 File Offset: 0x000D3092
		public StatesInstance(SpaceHeater master) : base(master)
		{
		}
	}

	// Token: 0x02000FAA RID: 4010
	public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
	{
		// Token: 0x06005125 RID: 20773 RVA: 0x00270938 File Offset: 0x0026EB38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			base.serializable = StateMachine.SerializeType.Never;
			this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp.resolveStringCallback = delegate(string str, object obj)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)obj;
				return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.offline.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).EventTransition(GameHashes.OperationalChanged, this.online, (SpaceHeater.StatesInstance smi) => smi.master.operational.IsOperational);
			this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (SpaceHeater.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.online.heating).Update("spaceheater_online", delegate(SpaceHeater.StatesInstance smi, float dt)
			{
				switch (smi.master.MonitorHeating(dt))
				{
				case SpaceHeater.MonitorState.ReadyToHeat:
					smi.GoTo(this.online.heating);
					return;
				case SpaceHeater.MonitorState.TooHot:
					smi.GoTo(this.online.overtemp);
					return;
				case SpaceHeater.MonitorState.NotEnoughLiquid:
					smi.GoTo(this.online.undermassliquid);
					return;
				case SpaceHeater.MonitorState.NotEnoughGas:
					smi.GoTo(this.online.undermassgas);
					return;
				default:
					return;
				}
			}, UpdateRate.SIM_4000ms, false);
			this.online.heating.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).ToggleStatusItem((SpaceHeater.StatesInstance smi) => smi.master.heatStatusItem, (SpaceHeater.StatesInstance smi) => smi).Update(new Action<SpaceHeater.StatesInstance, float>(SpaceHeater.GenerateHeat), UpdateRate.SIM_200ms, false).Exit(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Exit(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect));
			this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, null);
			this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, null);
			this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, null);
		}

		// Token: 0x040038A1 RID: 14497
		public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;

		// Token: 0x040038A2 RID: 14498
		public SpaceHeater.States.OnlineStates online;

		// Token: 0x040038A3 RID: 14499
		private StatusItem statusItemUnderMassLiquid;

		// Token: 0x040038A4 RID: 14500
		private StatusItem statusItemUnderMassGas;

		// Token: 0x040038A5 RID: 14501
		private StatusItem statusItemOverTemp;

		// Token: 0x02000FAB RID: 4011
		public class OnlineStates : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
		{
			// Token: 0x040038A6 RID: 14502
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;

			// Token: 0x040038A7 RID: 14503
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;

			// Token: 0x040038A8 RID: 14504
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;

			// Token: 0x040038A9 RID: 14505
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
		}
	}

	// Token: 0x02000FAD RID: 4013
	private enum MonitorState
	{
		// Token: 0x040038B3 RID: 14515
		ReadyToHeat,
		// Token: 0x040038B4 RID: 14516
		TooHot,
		// Token: 0x040038B5 RID: 14517
		NotEnoughLiquid,
		// Token: 0x040038B6 RID: 14518
		NotEnoughGas
	}
}
