using System;

// Token: 0x020003CB RID: 971
public class LimitValveTuning
{
	// Token: 0x06001023 RID: 4131 RVA: 0x000AD0B2 File Offset: 0x000AB2B2
	public static NonLinearSlider.Range[] GetDefaultSlider()
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(70f, 100f),
			new NonLinearSlider.Range(30f, 500f)
		};
	}

	// Token: 0x04000B61 RID: 2913
	public const float MAX_LIMIT = 500f;

	// Token: 0x04000B62 RID: 2914
	public const float DEFAULT_LIMIT = 100f;
}
