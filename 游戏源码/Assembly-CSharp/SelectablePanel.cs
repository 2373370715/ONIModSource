using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F14 RID: 7956
public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x0600A7D0 RID: 42960 RVA: 0x0010CE2B File Offset: 0x0010B02B
	public void OnDeselect(BaseEventData evt)
	{
		base.gameObject.SetActive(false);
	}
}
