using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EA3 RID: 7843
public class PlanCategoryNotifications : MonoBehaviour
{
	// Token: 0x0600A490 RID: 42128 RVA: 0x0010AC7B File Offset: 0x00108E7B
	public void ToggleAttention(bool active)
	{
		if (!this.AttentionImage)
		{
			return;
		}
		this.AttentionImage.gameObject.SetActive(active);
	}

	// Token: 0x0400809C RID: 32924
	public Image AttentionImage;
}
