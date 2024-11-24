using System;

// Token: 0x02001FCD RID: 8141
public interface ICheckboxControl
{
	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x0600AC53 RID: 44115
	string CheckboxTitleKey { get; }

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x0600AC54 RID: 44116
	string CheckboxLabel { get; }

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x0600AC55 RID: 44117
	string CheckboxTooltip { get; }

	// Token: 0x0600AC56 RID: 44118
	bool GetCheckboxValue();

	// Token: 0x0600AC57 RID: 44119
	void SetCheckboxValue(bool value);
}
