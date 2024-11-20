using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class EnergyGenerator : Generator, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
		public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";
		}
	}

		public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	public float GetSliderMin(int index)
	{
		return 0f;
	}

	public float GetSliderMax(int index)
	{
		return 100f;
	}

	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	string ISliderControl.GetSliderTooltip(int index)
	{
		ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), component.RequestedItemTag.ProperName(), this.batteryRefillPercent * 100f);
	}

	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EnergyGenerator.EnsureStatusItemAvailable();
		base.Subscribe<EnergyGenerator>(824508782, EnergyGenerator.OnActiveChangedDelegate);
		if (!this.ignoreBatteryRefillPercent)
		{
			base.gameObject.AddOrGet<CopyBuildingSettings>();
			base.Subscribe<EnergyGenerator>(-905833192, EnergyGenerator.OnCopySettingsDelegate);
		}
	}

	private void OnCopySettings(object data)
	{
		EnergyGenerator component = ((GameObject)data).GetComponent<EnergyGenerator>();
		if (component != null)
		{
			this.batteryRefillPercent = component.batteryRefillPercent;
		}
	}

	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasMeter)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
		}
	}

	private bool IsConvertible(float dt)
	{
		bool flag = true;
		foreach (EnergyGenerator.InputItem inputItem in this.formula.inputs)
		{
			float massAvailable = this.storage.GetMassAvailable(inputItem.tag);
			float num = inputItem.consumptionRate * dt;
			flag = (flag && massAvailable >= num);
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.hasMeter)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[0];
			float positionPercent = this.storage.GetMassAvailable(inputItem.tag) / inputItem.maxStoredMass;
			this.meter.SetPositionPercent(positionPercent);
		}
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		bool value = false;
		if (this.operational.IsOperational)
		{
			bool flag = false;
			List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
			if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
			{
				using (List<Battery>.Enumerator enumerator = batteriesOnCircuit.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Battery battery = enumerator.Current;
						if (this.batteryRefillPercent <= 0f && battery.PercentFull <= 0f)
						{
							flag = true;
							break;
						}
						if (battery.PercentFull < this.batteryRefillPercent)
						{
							flag = true;
							break;
						}
					}
					goto IL_105;
				}
			}
			flag = true;
			IL_105:
			if (!this.ignoreBatteryRefillPercent)
			{
				this.selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !flag, null);
			}
			if (this.delivery != null)
			{
				this.delivery.Pause(!flag, "Circuit has sufficient energy");
			}
			if (this.formula.inputs != null)
			{
				bool flag2 = this.IsConvertible(dt);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag2, this.formula);
				if (flag2)
				{
					foreach (EnergyGenerator.InputItem inputItem2 in this.formula.inputs)
					{
						float amount = inputItem2.consumptionRate * dt;
						this.storage.ConsumeIgnoringDisease(inputItem2.tag, amount);
					}
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					foreach (EnergyGenerator.OutputItem output in this.formula.outputs)
					{
						this.Emit(output, dt, component);
					}
					base.GenerateJoules(base.WattageRating * dt, false);
					this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
					value = true;
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.inputs == null || this.formula.inputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.inputs.Length; i++)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[i];
			string arg = inputItem.tag.ProperName();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.outputs == null || this.formula.outputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.outputs.Length; i++)
		{
			EnergyGenerator.OutputItem outputItem = this.formula.outputs[i];
			string arg = ElementLoader.FindElementByHash(outputItem.element).tag.ProperName();
			Descriptor item = default(Descriptor);
			if (outputItem.minTemperature > 0f)
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
			}
			else
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
			}
			list.Add(item);
		}
		return list;
	}

	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

		public static StatusItem BatteriesSufficientlyFull
	{
		get
		{
			return EnergyGenerator.batteriesSufficientlyFull;
		}
	}

	public static void EnsureStatusItemAvailable()
	{
		if (EnergyGenerator.batteriesSufficientlyFull == null)
		{
			EnergyGenerator.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	public static EnergyGenerator.Formula CreateSimpleFormula(Tag input_element, float input_mass_rate, float max_stored_input_mass, SimHashes output_element = SimHashes.Void, float output_mass_rate = 0f, bool store_output_mass = true, CellOffset output_offset = default(CellOffset), float min_output_temperature = 0f)
	{
		EnergyGenerator.Formula result = default(EnergyGenerator.Formula);
		result.inputs = new EnergyGenerator.InputItem[]
		{
			new EnergyGenerator.InputItem(input_element, input_mass_rate, max_stored_input_mass)
		};
		if (output_element != SimHashes.Void)
		{
			result.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(output_element, output_mass_rate, store_output_mass, output_offset, min_output_temperature)
			};
		}
		else
		{
			result.outputs = null;
		}
		return result;
	}

	private void Emit(EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
	{
		Element element = ElementLoader.FindElementByHash(output.element);
		float num = output.creationRate * dt;
		if (output.store)
		{
			if (element.IsGas)
			{
				this.storage.AddGasChunk(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			if (element.IsLiquid)
			{
				this.storage.AddLiquid(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num, root_pe.Temperature, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, true, false, true, false);
			return;
		}
		else
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), output.emitOffset);
			float temperature = Mathf.Max(root_pe.Temperature, output.minTemperature);
			if (element.IsGas)
			{
				SimMessages.ModifyMass(num2, num, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temperature, output.element);
				return;
			}
			if (element.IsLiquid)
			{
				ushort elementIndex = ElementLoader.GetElementIndex(output.element);
				FallingWater.instance.AddParticle(num2, elementIndex, num, temperature, byte.MaxValue, 0, true, false, false, false);
				return;
			}
			element.substance.SpawnResource(Grid.CellToPosCCC(num2, Grid.SceneLayer.Front), num, temperature, byte.MaxValue, 0, true, false, false);
			return;
		}
	}

	[MyCmpAdd]
	private Storage storage;

	[MyCmpGet]
	private ManualDeliveryKG delivery;

	[SerializeField]
	[Serialize]
	private float batteryRefillPercent = 0.5f;

	public bool ignoreBatteryRefillPercent;

	public bool hasMeter = true;

	private static StatusItem batteriesSufficientlyFull;

	public Meter.Offset meterOffset;

	[SerializeField]
	public EnergyGenerator.Formula formula;

	private MeterController meter;

	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	[DebuggerDisplay("{tag} -{consumptionRate} kg/s")]
	[Serializable]
	public struct InputItem
	{
		public InputItem(Tag tag, float consumption_rate, float max_stored_mass)
		{
			this.tag = tag;
			this.consumptionRate = consumption_rate;
			this.maxStoredMass = max_stored_mass;
		}

		public Tag tag;

		public float consumptionRate;

		public float maxStoredMass;
	}

	[DebuggerDisplay("{element} {creationRate} kg/s")]
	[Serializable]
	public struct OutputItem
	{
		public OutputItem(SimHashes element, float creation_rate, bool store, float min_temperature = 0f)
		{
			this = new EnergyGenerator.OutputItem(element, creation_rate, store, CellOffset.none, min_temperature);
		}

		public OutputItem(SimHashes element, float creation_rate, bool store, CellOffset emit_offset, float min_temperature = 0f)
		{
			this.element = element;
			this.creationRate = creation_rate;
			this.store = store;
			this.emitOffset = emit_offset;
			this.minTemperature = min_temperature;
		}

		public SimHashes element;

		public float creationRate;

		public bool store;

		public CellOffset emitOffset;

		public float minTemperature;
	}

	[Serializable]
	public struct Formula
	{
		public EnergyGenerator.InputItem[] inputs;

		public EnergyGenerator.OutputItem[] outputs;

		public Tag meterTag;
	}
}
