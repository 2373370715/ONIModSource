using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A6C RID: 2668
public struct InfraredVisualizerData
{
	// Token: 0x06003123 RID: 12579 RVA: 0x001FE15C File Offset: 0x001FC35C
	public void Update()
	{
		float num = 0f;
		if (this.temperatureAmount != null)
		{
			num = this.temperatureAmount.value;
		}
		else if (this.structureTemperature.IsValid())
		{
			num = GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
		else if (this.primaryElement != null)
		{
			num = this.primaryElement.Temperature;
		}
		else if (this.temperatureVulnerable != null)
		{
			num = this.temperatureVulnerable.InternalTemperature;
		}
		else if (this.critterTemperatureMonitorInstance != null)
		{
			num = this.critterTemperatureMonitorInstance.GetTemperatureInternal();
		}
		if (num < 0f)
		{
			return;
		}
		Color32 c = SimDebugView.Instance.NormalizedTemperature(num);
		this.controller.OverlayColour = c;
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x001FE224 File Offset: 0x001FC424
	public InfraredVisualizerData(GameObject go)
	{
		this.controller = go.GetComponent<KBatchedAnimController>();
		if (this.controller != null)
		{
			this.temperatureAmount = Db.Get().Amounts.Temperature.Lookup(go);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(go);
			this.primaryElement = go.GetComponent<PrimaryElement>();
			this.temperatureVulnerable = go.GetComponent<TemperatureVulnerable>();
			this.critterTemperatureMonitorInstance = go.GetSMI<CritterTemperatureMonitor.Instance>();
			return;
		}
		this.temperatureAmount = null;
		this.structureTemperature = HandleVector<int>.InvalidHandle;
		this.primaryElement = null;
		this.temperatureVulnerable = null;
		this.critterTemperatureMonitorInstance = null;
	}

	// Token: 0x04002121 RID: 8481
	public KAnimControllerBase controller;

	// Token: 0x04002122 RID: 8482
	public AmountInstance temperatureAmount;

	// Token: 0x04002123 RID: 8483
	public HandleVector<int>.Handle structureTemperature;

	// Token: 0x04002124 RID: 8484
	public PrimaryElement primaryElement;

	// Token: 0x04002125 RID: 8485
	public TemperatureVulnerable temperatureVulnerable;

	// Token: 0x04002126 RID: 8486
	public CritterTemperatureMonitor.Instance critterTemperatureMonitorInstance;
}
