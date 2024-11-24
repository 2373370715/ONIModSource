using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F0D RID: 7949
public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x0600A7A5 RID: 42917 RVA: 0x0010CC07 File Offset: 0x0010AE07
	public void OnPointerEnter(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x0600A7A6 RID: 42918 RVA: 0x0010CC0F File Offset: 0x0010AE0F
	private void RunCallbacks()
	{
		if (Input.GetMouseButton(0) && this.onLeftClick != null)
		{
			this.onLeftClick();
		}
	}

	// Token: 0x0600A7A7 RID: 42919 RVA: 0x0010CC07 File Offset: 0x0010AE07
	public void OnPointerDown(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x040083C4 RID: 33732
	public Image image;

	// Token: 0x040083C5 RID: 33733
	public System.Action onLeftClick;
}
