using System;

// Token: 0x02000829 RID: 2089
public class PathProberSensor : Sensor
{
	// Token: 0x06002559 RID: 9561 RVA: 0x000B86AA File Offset: 0x000B68AA
	public PathProberSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = sensors.GetComponent<Navigator>();
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x000B86BF File Offset: 0x000B68BF
	public override void Update()
	{
		this.navigator.UpdateProbe(false);
	}

	// Token: 0x04001940 RID: 6464
	private Navigator navigator;
}
