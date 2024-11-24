using System;

// Token: 0x0200082A RID: 2090
public class PickupableSensor : Sensor
{
	// Token: 0x0600255B RID: 9563 RVA: 0x000B86CD File Offset: 0x000B68CD
	public PickupableSensor(Sensors sensors) : base(sensors)
	{
		this.worker = base.GetComponent<WorkerBase>();
		this.pathProber = base.GetComponent<PathProber>();
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x000B86EE File Offset: 0x000B68EE
	public override void Update()
	{
		GlobalChoreProvider.Instance.UpdateFetches(this.pathProber);
		Game.Instance.fetchManager.UpdatePickups(this.pathProber, this.worker);
	}

	// Token: 0x04001941 RID: 6465
	private PathProber pathProber;

	// Token: 0x04001942 RID: 6466
	private WorkerBase worker;
}
