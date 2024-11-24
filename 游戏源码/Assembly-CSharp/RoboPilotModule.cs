using System;

// Token: 0x02001922 RID: 6434
public class RoboPilotModule : KMonoBehaviour
{
	// Token: 0x0600860B RID: 34315 RVA: 0x0034AE04 File Offset: 0x00349004
	protected override void OnSpawn()
	{
		this.databankStorage = base.GetComponent<Storage>();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.UpdateMeter(null);
		this.databankStorage.SetOffsets(RoboPilotModule.dataDeliveryOffsets);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Subscribe(-887025858, new Action<object>(this.OnRocketLanded));
		base.GetComponent<RocketModuleCluster>().CraftInterface.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
	}

	// Token: 0x0600860C RID: 34316 RVA: 0x0034AED8 File Offset: 0x003490D8
	private void OnLaunchConditionChanged(object data)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component.CraftInterface.IsLaunchRequested())
		{
			component.CraftInterface.GetComponent<Clustercraft>().Launch(false);
		}
	}

	// Token: 0x0600860D RID: 34317 RVA: 0x0034AF0C File Offset: 0x0034910C
	private void OnRocketLanded(object o)
	{
		if (this.consumeDataBanksOnLand)
		{
			LaunchConditionManager lcm = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcm);
			float amount = Math.Min((float)(SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id).OneBasedDistance * this.dataBankConsumption), this.databankStorage.MassStored());
			this.databankStorage.ConsumeIgnoringDisease(this.dataBankType, amount);
		}
	}

	// Token: 0x0600860E RID: 34318 RVA: 0x000F7D61 File Offset: 0x000F5F61
	public void ConsumeDataBanksInFlight()
	{
		this.databankStorage.ConsumeIgnoringDisease(this.dataBankType, (float)this.dataBankConsumption);
	}

	// Token: 0x0600860F RID: 34319 RVA: 0x000F7D7B File Offset: 0x000F5F7B
	private void UpdateMeter(object data = null)
	{
		this.meter.SetPositionPercent(this.databankStorage.MassStored() / this.databankStorage.Capacity());
	}

	// Token: 0x06008610 RID: 34320 RVA: 0x000F7D9F File Offset: 0x000F5F9F
	public bool HasResourcesToMove(int distance)
	{
		return this.databankStorage.MassStored() > (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x06008611 RID: 34321 RVA: 0x000F7DB7 File Offset: 0x000F5FB7
	public float GetDataBanksStored()
	{
		if (!(this.databankStorage != null))
		{
			return 0f;
		}
		return this.databankStorage.MassStored();
	}

	// Token: 0x06008612 RID: 34322 RVA: 0x000F7DD8 File Offset: 0x000F5FD8
	public float FlightEfficiencyModifier()
	{
		if (this.GetDataBanksStored() > 0f)
		{
			return this.flightEfficiencyModifier;
		}
		return 0f;
	}

	// Token: 0x04006530 RID: 25904
	private MeterController meter;

	// Token: 0x04006531 RID: 25905
	private Storage databankStorage;

	// Token: 0x04006532 RID: 25906
	public int dataBankConsumption = 2;

	// Token: 0x04006533 RID: 25907
	public Tag dataBankType;

	// Token: 0x04006534 RID: 25908
	private float flightEfficiencyModifier = 0.1f;

	// Token: 0x04006535 RID: 25909
	public bool consumeDataBanksOnLand;

	// Token: 0x04006536 RID: 25910
	private static CellOffset[] dataDeliveryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(2, 0),
		new CellOffset(3, 0),
		new CellOffset(-1, 0),
		new CellOffset(-2, 0),
		new CellOffset(-3, 0)
	};
}
