using System;
using System.Collections.Generic;

// Token: 0x02001F92 RID: 8082
public interface INToggleSideScreenControl
{
	// Token: 0x17000ADF RID: 2783
	// (get) Token: 0x0600AA9B RID: 43675
	string SidescreenTitleKey { get; }

	// Token: 0x17000AE0 RID: 2784
	// (get) Token: 0x0600AA9C RID: 43676
	List<LocString> Options { get; }

	// Token: 0x17000AE1 RID: 2785
	// (get) Token: 0x0600AA9D RID: 43677
	List<LocString> Tooltips { get; }

	// Token: 0x17000AE2 RID: 2786
	// (get) Token: 0x0600AA9E RID: 43678
	string Description { get; }

	// Token: 0x17000AE3 RID: 2787
	// (get) Token: 0x0600AA9F RID: 43679
	int SelectedOption { get; }

	// Token: 0x17000AE4 RID: 2788
	// (get) Token: 0x0600AAA0 RID: 43680
	int QueuedOption { get; }

	// Token: 0x0600AAA1 RID: 43681
	void QueueSelectedOption(int option);
}
