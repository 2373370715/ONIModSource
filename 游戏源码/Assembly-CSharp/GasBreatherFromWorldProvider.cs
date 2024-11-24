public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	private SuffocationMonitor.Instance suffocationMonitor;

	private SafeCellMonitor.Instance safeCellMonitor;

	private OxygenBreather oxygenBreather;

	private Navigator nav;

	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		suffocationMonitor = new SuffocationMonitor.Instance(oxygen_breather);
		suffocationMonitor.StartSM();
		safeCellMonitor = new SafeCellMonitor.Instance(oxygen_breather);
		safeCellMonitor.StartSM();
		oxygenBreather = oxygen_breather;
		nav = oxygenBreather.GetComponent<Navigator>();
	}

	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		suffocationMonitor.StopSM("Removed gas provider");
		safeCellMonitor.StopSM("Removed gas provider");
	}

	public bool ShouldEmitCO2()
	{
		return nav.CurrentNavType != NavType.Tube;
	}

	public bool ShouldStoreCO2()
	{
		return false;
	}

	public bool IsLowOxygen()
	{
		return oxygenBreather.IsLowOxygenAtMouthCell();
	}

	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (nav.CurrentNavType != NavType.Tube)
		{
			SimHashes getBreathableElement = oxygen_breather.GetBreathableElement;
			if (getBreathableElement == SimHashes.Vacuum)
			{
				return false;
			}
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(OnSimConsumeCallback, this, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(oxygen_breather.mouthCell, getBreathableElement, gas_consumed, 3, handle.index);
		}
		return true;
	}

	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((GasBreatherFromWorldProvider)data).OnSimConsume(mass_cb_info);
	}

	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (!(oxygenBreather == null) && !oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			if (ElementLoader.elements[mass_cb_info.elemIdx].id == SimHashes.ContaminatedOxygen)
			{
				oxygenBreather.Trigger(-935848905, mass_cb_info);
			}
			Game.Instance.accumulators.Accumulate(oxygenBreather.O2Accumulator, mass_cb_info.mass);
			float value = 0f - mass_cb_info.mass;
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, oxygenBreather.GetProperName());
			oxygenBreather.Consume(mass_cb_info);
		}
	}
}
