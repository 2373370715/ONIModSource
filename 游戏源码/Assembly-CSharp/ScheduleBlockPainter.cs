using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F08 RID: 7944
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600A77C RID: 42876 RVA: 0x0010CAF0 File Offset: 0x0010ACF0
	public void SetEntry(ScheduleScreenEntry entry)
	{
		this.entry = entry;
	}

	// Token: 0x0600A77D RID: 42877 RVA: 0x0010CAF9 File Offset: 0x0010ACF9
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A77E RID: 42878 RVA: 0x0010CAF9 File Offset: 0x0010ACF9
	public void OnDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A77F RID: 42879 RVA: 0x0010CAF9 File Offset: 0x0010ACF9
	public void OnEndDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A780 RID: 42880 RVA: 0x0010CB02 File Offset: 0x0010AD02
	public void OnPointerDown(PointerEventData eventData)
	{
		ScheduleBlockPainter.paintCounter = 0;
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A781 RID: 42881 RVA: 0x003F8DA8 File Offset: 0x003F6FA8
	private void PaintBlocksBelow(PointerEventData eventData)
	{
		if (ScheduleScreen.Instance.SelectedPaint.IsNullOrWhiteSpace())
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
		if (list != null && list.Count > 0)
		{
			ScheduleBlockButton component = list[0].gameObject.GetComponent<ScheduleBlockButton>();
			if (component != null)
			{
				if (this.entry.PaintBlock(component))
				{
					string sound = GlobalAssets.GetSound("ScheduleMenu_Select", false);
					if (sound != null)
					{
						EventInstance instance = SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
						instance.setParameterByName("Drag_Count", (float)ScheduleBlockPainter.paintCounter, false);
						ScheduleBlockPainter.paintCounter++;
						SoundEvent.EndOneShot(instance);
						this.previousBlockTriedPainted = component.gameObject;
						return;
					}
				}
				else if (this.previousBlockTriedPainted != component.gameObject)
				{
					this.previousBlockTriedPainted = component.gameObject;
					string sound2 = GlobalAssets.GetSound("ScheduleMenu_Select_none", false);
					if (sound2 != null)
					{
						SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
					}
				}
			}
		}
	}

	// Token: 0x040083B0 RID: 33712
	private ScheduleScreenEntry entry;

	// Token: 0x040083B1 RID: 33713
	private static int paintCounter;

	// Token: 0x040083B2 RID: 33714
	private GameObject previousBlockTriedPainted;
}
