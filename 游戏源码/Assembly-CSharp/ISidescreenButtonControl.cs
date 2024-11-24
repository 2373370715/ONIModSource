using System;

// Token: 0x02001F38 RID: 7992
public interface ISidescreenButtonControl
{
	// Token: 0x17000AC2 RID: 2754
	// (get) Token: 0x0600A8B1 RID: 43185
	string SidescreenButtonText { get; }

	// Token: 0x17000AC3 RID: 2755
	// (get) Token: 0x0600A8B2 RID: 43186
	string SidescreenButtonTooltip { get; }

	// Token: 0x0600A8B3 RID: 43187
	void SetButtonTextOverride(ButtonMenuTextOverride textOverride);

	// Token: 0x0600A8B4 RID: 43188
	bool SidescreenEnabled();

	// Token: 0x0600A8B5 RID: 43189
	bool SidescreenButtonInteractable();

	// Token: 0x0600A8B6 RID: 43190
	void OnSidescreenButtonPressed();

	// Token: 0x0600A8B7 RID: 43191
	int HorizontalGroupID();

	// Token: 0x0600A8B8 RID: 43192
	int ButtonSideScreenSortOrder();
}
