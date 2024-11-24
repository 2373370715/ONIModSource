using System;
using System.Collections.Generic;

// Token: 0x02001F5F RID: 8031
public interface IDispenser
{
	// Token: 0x0600A990 RID: 43408
	List<Tag> DispensedItems();

	// Token: 0x0600A991 RID: 43409
	Tag SelectedItem();

	// Token: 0x0600A992 RID: 43410
	void SelectItem(Tag tag);

	// Token: 0x0600A993 RID: 43411
	void OnOrderDispense();

	// Token: 0x0600A994 RID: 43412
	void OnCancelDispense();

	// Token: 0x0600A995 RID: 43413
	bool HasOpenChore();

	// Token: 0x14000037 RID: 55
	// (add) Token: 0x0600A996 RID: 43414
	// (remove) Token: 0x0600A997 RID: 43415
	event System.Action OnStopWorkEvent;
}
