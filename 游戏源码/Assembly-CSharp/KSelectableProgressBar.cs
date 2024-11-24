using System;

// Token: 0x02000A78 RID: 2680
public class KSelectableProgressBar : KSelectable
{
	// Token: 0x06003182 RID: 12674 RVA: 0x001FF2EC File Offset: 0x001FD4EC
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04002147 RID: 8519
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04002148 RID: 8520
	private int scaleAmount = 100;
}
