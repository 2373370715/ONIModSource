using System;
using UnityEngine;

// Token: 0x0200202B RID: 8235
public class TimeOfDayPositioner : KMonoBehaviour
{
	// Token: 0x0600AF55 RID: 44885 RVA: 0x00420210 File Offset: 0x0041E410
	public void SetTargetTimetable(GameObject TimetableRow)
	{
		if (TimetableRow == null)
		{
			this.targetRect = null;
			base.transform.SetParent(null);
			return;
		}
		RectTransform rectTransform = TimetableRow.GetComponent<HierarchyReferences>().GetReference<RectTransform>("BlockContainer").rectTransform();
		this.targetRect = rectTransform;
		base.transform.SetParent(this.targetRect.transform);
	}

	// Token: 0x0600AF56 RID: 44886 RVA: 0x00420270 File Offset: 0x0041E470
	private void Update()
	{
		if (base.transform.parent != this.targetRect.transform)
		{
			base.transform.parent = this.targetRect.transform;
		}
		float f = GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width;
		(base.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Round(f), 0f);
	}

	// Token: 0x04008A0B RID: 35339
	private RectTransform targetRect;
}
