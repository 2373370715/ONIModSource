using System;
using System.Diagnostics;

// Token: 0x02000F23 RID: 3875
[DebuggerDisplay("{name}")]
public class PowerTransformer : Generator
{
	// Token: 0x06004E17 RID: 19991 RVA: 0x000D2C64 File Offset: 0x000D0E64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.battery = base.GetComponent<Battery>();
		base.Subscribe<PowerTransformer>(-592767678, PowerTransformer.OnOperationalChangedDelegate);
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x000D2C8F File Offset: 0x000D0E8F
	public override void ApplyDeltaJoules(float joules_delta, bool can_over_power = false)
	{
		this.battery.ConsumeEnergy(-joules_delta);
		base.ApplyDeltaJoules(joules_delta, can_over_power);
	}

	// Token: 0x06004E19 RID: 19993 RVA: 0x000D2CA6 File Offset: 0x000D0EA6
	public override void ConsumeEnergy(float joules)
	{
		this.battery.ConsumeEnergy(joules);
		base.ConsumeEnergy(joules);
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x000D2CBB File Offset: 0x000D0EBB
	private void OnOperationalChanged(object data)
	{
		this.UpdateJoulesLostPerSecond();
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x000D2CC3 File Offset: 0x000D0EC3
	private void UpdateJoulesLostPerSecond()
	{
		if (this.operational.IsOperational)
		{
			this.battery.joulesLostPerSecond = 0f;
			return;
		}
		this.battery.joulesLostPerSecond = 3.3333333f;
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x00266EDC File Offset: 0x002650DC
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		float joulesAvailable = this.operational.IsOperational ? Math.Min(this.battery.JoulesAvailable, base.WattageRating * dt) : 0f;
		base.AssignJoulesAvailable(joulesAvailable);
		ushort circuitID = this.battery.CircuitID;
		ushort circuitID2 = base.CircuitID;
		bool flag = circuitID == circuitID2 && circuitID != ushort.MaxValue;
		if (this.mLoopDetected != flag)
		{
			this.mLoopDetected = flag;
			this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.PowerLoopDetected, this.mLoopDetected, this);
		}
	}

	// Token: 0x04003647 RID: 13895
	private Battery battery;

	// Token: 0x04003648 RID: 13896
	private bool mLoopDetected;

	// Token: 0x04003649 RID: 13897
	private static readonly EventSystem.IntraObjectHandler<PowerTransformer> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PowerTransformer>(delegate(PowerTransformer component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
