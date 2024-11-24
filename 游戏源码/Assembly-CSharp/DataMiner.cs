using System;
using UnityEngine;

// Token: 0x02001202 RID: 4610
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class DataMiner : ComplexFabricator
{
	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06005E23 RID: 24099 RVA: 0x000DD7D2 File Offset: 0x000DB9D2
	public float OperatingTemp
	{
		get
		{
			return this.pe.Temperature;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06005E24 RID: 24100 RVA: 0x000DD7DF File Offset: 0x000DB9DF
	public float TemperatureScaleFactor
	{
		get
		{
			return 1f - DataMinerConfig.TEMPERATURE_SCALING_RANGE.LerpFactorClamped(this.OperatingTemp);
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06005E25 RID: 24101 RVA: 0x000DD7F7 File Offset: 0x000DB9F7
	public float EfficiencyRate
	{
		get
		{
			return DataMinerConfig.PRODUCTION_RATE_SCALE.Lerp(this.TemperatureScaleFactor);
		}
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x000DD809 File Offset: 0x000DBA09
	protected override float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		return base.ComputeWorkProgress(dt, recipe) * this.EfficiencyRate;
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x000DD81A File Offset: 0x000DBA1A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DataMinerEfficiency, this);
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x000DD852 File Offset: 0x000DBA52
	public override void Sim1000ms(float dt)
	{
		base.Sim1000ms(dt);
		this.meter.SetPositionPercent(this.TemperatureScaleFactor);
	}

	// Token: 0x040042A7 RID: 17063
	[MyCmpReq]
	private PrimaryElement pe;

	// Token: 0x040042A8 RID: 17064
	private MeterController meter;
}
