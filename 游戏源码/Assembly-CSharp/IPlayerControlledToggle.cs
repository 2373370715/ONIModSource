using System;

// Token: 0x02001FA8 RID: 8104
public interface IPlayerControlledToggle
{
	// Token: 0x0600AB3E RID: 43838
	void ToggledByPlayer();

	// Token: 0x0600AB3F RID: 43839
	bool ToggledOn();

	// Token: 0x0600AB40 RID: 43840
	KSelectable GetSelectable();

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x0600AB41 RID: 43841
	string SideScreenTitleKey { get; }

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x0600AB42 RID: 43842
	// (set) Token: 0x0600AB43 RID: 43843
	bool ToggleRequested { get; set; }
}
