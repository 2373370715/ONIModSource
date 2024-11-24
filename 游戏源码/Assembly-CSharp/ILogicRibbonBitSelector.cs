using System;

// Token: 0x02001F84 RID: 8068
public interface ILogicRibbonBitSelector
{
	// Token: 0x0600AA46 RID: 43590
	void SetBitSelection(int bit);

	// Token: 0x0600AA47 RID: 43591
	int GetBitSelection();

	// Token: 0x0600AA48 RID: 43592
	int GetBitDepth();

	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x0600AA49 RID: 43593
	string SideScreenTitle { get; }

	// Token: 0x17000AD4 RID: 2772
	// (get) Token: 0x0600AA4A RID: 43594
	string SideScreenDescription { get; }

	// Token: 0x0600AA4B RID: 43595
	bool SideScreenDisplayWriterDescription();

	// Token: 0x0600AA4C RID: 43596
	bool SideScreenDisplayReaderDescription();

	// Token: 0x0600AA4D RID: 43597
	bool IsBitActive(int bit);

	// Token: 0x0600AA4E RID: 43598
	int GetOutputValue();

	// Token: 0x0600AA4F RID: 43599
	int GetInputValue();

	// Token: 0x0600AA50 RID: 43600
	void UpdateVisuals();
}
