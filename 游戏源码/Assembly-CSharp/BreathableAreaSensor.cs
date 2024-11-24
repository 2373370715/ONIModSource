using System;

// Token: 0x02000821 RID: 2081
public class BreathableAreaSensor : Sensor
{
	// Token: 0x06002542 RID: 9538 RVA: 0x000B855F File Offset: 0x000B675F
	public BreathableAreaSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x001CBE48 File Offset: 0x001CA048
	public override void Update()
	{
		if (this.breather == null)
		{
			this.breather = base.GetComponent<OxygenBreather>();
		}
		bool flag = this.isBreathable;
		this.isBreathable = (this.breather.IsBreathableElement || this.breather.HasTag(GameTags.InTransitTube));
		if (this.isBreathable != flag)
		{
			if (this.isBreathable)
			{
				base.Trigger(99949694, null);
				return;
			}
			base.Trigger(-1189351068, null);
		}
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000B8568 File Offset: 0x000B6768
	public bool IsBreathable()
	{
		return this.isBreathable;
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000B8570 File Offset: 0x000B6770
	public bool IsUnderwater()
	{
		return this.breather.IsUnderLiquid;
	}

	// Token: 0x0400192B RID: 6443
	private bool isBreathable;

	// Token: 0x0400192C RID: 6444
	private OxygenBreather breather;
}
