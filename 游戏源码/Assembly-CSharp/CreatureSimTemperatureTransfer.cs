using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001149 RID: 4425
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	// Token: 0x06005A78 RID: 23160 RVA: 0x00294788 File Offset: 0x00292988
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20, true);
		this.averageTemperatureTransferPerSecond = new AttributeModifier(this.temperatureAttributeName + "Delta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, false, true, false);
		this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x00294800 File Offset: 0x00292A00
	protected override void OnSpawn()
	{
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.skinThickness, this.skinThicknessAttributeModifierName, false, false, true);
		attributeInstance.Add(modifier);
		base.OnSpawn();
	}

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06005A7A RID: 23162 RVA: 0x000DAF2E File Offset: 0x000D912E
	public bool LastTemperatureRecordIsReliable
	{
		get
		{
			return Time.time - this.lastTemperatureRecordTime < 2f && this.average_kilowatts_exchanged.HasEverHadValidValues && this.average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
		}
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x00294864 File Offset: 0x00292A64
	protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
	{
		if (Time.time < this.lastTemperatureRecordTime + 0.2f)
		{
			return;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(this.simHandle);
			if (Game.Instance.simData.elementChunks[handleIndex].deltaKJ == 0f)
			{
				return;
			}
			this.average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
			this.lastTemperatureRecordTime = Time.time;
		}
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x000DAF65 File Offset: 0x000D9165
	private void Update()
	{
		this.unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x00294900 File Offset: 0x00292B00
	public void Sim200ms(float dt)
	{
		this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetUnweightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier attributeModifier in this.NonSimTemperatureModifiers)
		{
			num += attributeModifier.Value;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			float num2 = num * (this.primaryElement.Mass * 1000f) * this.primaryElement.Element.specificHeatCapacity * 0.001f;
			float delta_kj = num2 * dt;
			SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kj);
			this.heatEffect.SetHeatBeingProducedValue(num2);
			return;
		}
		this.heatEffect.SetHeatBeingProducedValue(0f);
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x002949F8 File Offset: 0x00292BF8
	public void RefreshRegistration()
	{
		base.SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier);
		this.thickness = attributeInstance.GetTotalValue();
		this.simHandle = -1;
		base.SimRegister();
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x000DAF72 File Offset: 0x000D9172
	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}

	// Token: 0x04003FD1 RID: 16337
	public string temperatureAttributeName = "Temperature";

	// Token: 0x04003FD2 RID: 16338
	public float skinThickness = DUPLICANTSTATS.STANDARD.Temperature.SKIN_THICKNESS;

	// Token: 0x04003FD3 RID: 16339
	public string skinThicknessAttributeModifierName = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04003FD4 RID: 16340
	public AttributeModifier averageTemperatureTransferPerSecond;

	// Token: 0x04003FD5 RID: 16341
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04003FD6 RID: 16342
	private PrimaryElement primaryElement;

	// Token: 0x04003FD7 RID: 16343
	public RunningWeightedAverage average_kilowatts_exchanged;

	// Token: 0x04003FD8 RID: 16344
	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

	// Token: 0x04003FD9 RID: 16345
	private float lastTemperatureRecordTime;
}
