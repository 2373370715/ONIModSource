using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	public string temperatureAttributeName = "Temperature";

	public float skinThickness = 0.002f;

	public string skinThicknessAttributeModifierName = DUPLICANTS.MODIFIERS.BASEDUPLICANT.NAME;

	public AttributeModifier averageTemperatureTransferPerSecond;

	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	private PrimaryElement primaryElement;

	public RunningWeightedAverage average_kilowatts_exchanged;

	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

	private float lastTemperatureRecordTime;

	public bool LastTemperatureRecordIsReliable
	{
		get
		{
			if (Time.time - lastTemperatureRecordTime < 2f && average_kilowatts_exchanged.HasEverHadValidValues)
			{
				return average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
			}
			return false;
		}
	}

	protected override void OnPrefabInit()
	{
		primaryElement = GetComponent<PrimaryElement>();
		average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f);
		averageTemperatureTransferPerSecond = new AttributeModifier(temperatureAttributeName + "Delta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, is_multiplier: false, uiOnly: true, is_readonly: false);
		this.GetAttributes().Add(averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, skinThickness, skinThicknessAttributeModifierName);
		attributeInstance.Add(modifier);
		base.OnSpawn();
	}

	protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
	{
		if (!(Time.time < lastTemperatureRecordTime + 0.2f) && Sim.IsValidHandle(simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(simHandle);
			if (Game.Instance.simData.elementChunks[handleIndex].deltaKJ != 0f)
			{
				average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
				lastTemperatureRecordTime = Time.time;
			}
		}
	}

	private void Update()
	{
		unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);
	}

	public void Sim200ms(float dt)
	{
		averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(average_kilowatts_exchanged.GetUnweightedAverage, primaryElement.Element.specificHeatCapacity, primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier nonSimTemperatureModifier in NonSimTemperatureModifiers)
		{
			num += nonSimTemperatureModifier.Value;
		}
		if (Sim.IsValidHandle(simHandle))
		{
			float num2 = num * (primaryElement.Mass * 1000f) * primaryElement.Element.specificHeatCapacity * 0.001f;
			float delta_kj = num2 * dt;
			SimMessages.ModifyElementChunkEnergy(simHandle, delta_kj);
			heatEffect.SetHeatBeingProducedValue(num2);
		}
		else
		{
			heatEffect.SetHeatBeingProducedValue(0f);
		}
	}

	public void RefreshRegistration()
	{
		SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier);
		thickness = attributeInstance.GetTotalValue();
		simHandle = -1;
		SimRegister();
	}

	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}
}
