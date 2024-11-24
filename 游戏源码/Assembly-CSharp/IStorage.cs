using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001405 RID: 5125
public interface IStorage
{
	// Token: 0x06006942 RID: 26946
	bool ShouldShowInUI();

	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06006943 RID: 26947
	// (set) Token: 0x06006944 RID: 26948
	bool allowUIItemRemoval { get; set; }

	// Token: 0x06006945 RID: 26949
	GameObject Drop(GameObject go, bool do_disease_transfer = true);

	// Token: 0x06006946 RID: 26950
	List<GameObject> GetItems();

	// Token: 0x06006947 RID: 26951
	bool IsFull();

	// Token: 0x06006948 RID: 26952
	bool IsEmpty();

	// Token: 0x06006949 RID: 26953
	float Capacity();

	// Token: 0x0600694A RID: 26954
	float RemainingCapacity();

	// Token: 0x0600694B RID: 26955
	float GetAmountAvailable(Tag tag);

	// Token: 0x0600694C RID: 26956
	void ConsumeIgnoringDisease(Tag tag, float amount);
}
