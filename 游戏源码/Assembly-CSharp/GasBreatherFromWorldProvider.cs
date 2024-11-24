using System;

// Token: 0x02001364 RID: 4964
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	// Token: 0x060065F4 RID: 26100 RVA: 0x002CDB24 File Offset: 0x002CBD24
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor = new SuffocationMonitor.Instance(oxygen_breather);
		this.suffocationMonitor.StartSM();
		this.safeCellMonitor = new SafeCellMonitor.Instance(oxygen_breather);
		this.safeCellMonitor.StartSM();
		this.oxygenBreather = oxygen_breather;
		this.nav = this.oxygenBreather.GetComponent<Navigator>();
	}

	// Token: 0x060065F5 RID: 26101 RVA: 0x000E27FD File Offset: 0x000E09FD
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suffocationMonitor.StopSM("Removed gas provider");
		this.safeCellMonitor.StopSM("Removed gas provider");
	}

	// Token: 0x060065F6 RID: 26102 RVA: 0x000E281F File Offset: 0x000E0A1F
	public bool ShouldEmitCO2()
	{
		return this.nav.CurrentNavType != NavType.Tube;
	}

	// Token: 0x060065F7 RID: 26103 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x060065F8 RID: 26104 RVA: 0x000E2832 File Offset: 0x000E0A32
	public bool IsLowOxygen()
	{
		return this.oxygenBreather.IsLowOxygenAtMouthCell();
	}

	// Token: 0x060065F9 RID: 26105 RVA: 0x002CDB78 File Offset: 0x002CBD78
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.nav.CurrentNavType != NavType.Tube)
		{
			SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
			if (getBreathableElement == SimHashes.Vacuum)
			{
				return false;
			}
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback), this, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed, 3, handle.index);
		}
		return true;
	}

	// Token: 0x060065FA RID: 26106 RVA: 0x000E283F File Offset: 0x000E0A3F
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((GasBreatherFromWorldProvider)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x060065FB RID: 26107 RVA: 0x002CDBDC File Offset: 0x002CBDDC
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (this.oxygenBreather == null || this.oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			return;
		}
		if (ElementLoader.elements[(int)mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
		{
			this.oxygenBreather.Trigger(-935848905, mass_cb_info);
		}
		Game.Instance.accumulators.Accumulate(this.oxygenBreather.O2Accumulator, mass_cb_info.mass);
		float value = -mass_cb_info.mass;
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, this.oxygenBreather.GetProperName(), null);
		this.oxygenBreather.Consume(mass_cb_info);
	}

	// Token: 0x04004C7F RID: 19583
	private SuffocationMonitor.Instance suffocationMonitor;

	// Token: 0x04004C80 RID: 19584
	private SafeCellMonitor.Instance safeCellMonitor;

	// Token: 0x04004C81 RID: 19585
	private OxygenBreather oxygenBreather;

	// Token: 0x04004C82 RID: 19586
	private Navigator nav;
}
