using System;
using UnityEngine;

// Token: 0x02001F5D RID: 8029
public class CritterSensorSideScreen : SideScreenContent
{
	// Token: 0x0600A983 RID: 43395 RVA: 0x0010E1BD File Offset: 0x0010C3BD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.countCrittersToggle.onClick += this.ToggleCritters;
		this.countEggsToggle.onClick += this.ToggleEggs;
	}

	// Token: 0x0600A984 RID: 43396 RVA: 0x0010E1F3 File Offset: 0x0010C3F3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCritterCountSensor>() != null;
	}

	// Token: 0x0600A985 RID: 43397 RVA: 0x00401978 File Offset: 0x003FFB78
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x0600A986 RID: 43398 RVA: 0x0010E201 File Offset: 0x0010C401
	private void ToggleCritters()
	{
		this.targetSensor.countCritters = !this.targetSensor.countCritters;
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
	}

	// Token: 0x0600A987 RID: 43399 RVA: 0x0010E232 File Offset: 0x0010C432
	private void ToggleEggs()
	{
		this.targetSensor.countEggs = !this.targetSensor.countEggs;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x0400854F RID: 34127
	public LogicCritterCountSensor targetSensor;

	// Token: 0x04008550 RID: 34128
	public KToggle countCrittersToggle;

	// Token: 0x04008551 RID: 34129
	public KToggle countEggsToggle;

	// Token: 0x04008552 RID: 34130
	public KImage crittersCheckmark;

	// Token: 0x04008553 RID: 34131
	public KImage eggsCheckmark;
}
