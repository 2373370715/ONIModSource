using System;

// Token: 0x02000A77 RID: 2679
public class KSelectableHealthBar : KSelectable
{
	// Token: 0x06003180 RID: 12672 RVA: 0x001FF2A8 File Offset: 0x001FD4A8
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04002145 RID: 8517
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04002146 RID: 8518
	private int scaleAmount = 100;
}
