using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F07 RID: 7943
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockButton")]
public class ScheduleBlockButton : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600A776 RID: 42870 RVA: 0x003F8CDC File Offset: 0x003F6EDC
	public void Setup(int hour)
	{
		if (hour < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
		}
		else if (hour >= 21)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
		}
		base.gameObject.name = "ScheduleBlock_" + hour.ToString();
		this.ToggleHighlight(false);
	}

	// Token: 0x0600A777 RID: 42871 RVA: 0x003F8D54 File Offset: 0x003F6F54
	public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
	{
		ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
		if (scheduleGroup != null)
		{
			this.image.color = scheduleGroup.uiColor;
			this.toolTip.SetSimpleTooltip(scheduleGroup.Name);
			return;
		}
		this.toolTip.SetSimpleTooltip("UNKNOWN");
	}

	// Token: 0x0600A778 RID: 42872 RVA: 0x0010CAD0 File Offset: 0x0010ACD0
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHighlight(true);
	}

	// Token: 0x0600A779 RID: 42873 RVA: 0x0010CAD9 File Offset: 0x0010ACD9
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHighlight(false);
	}

	// Token: 0x0600A77A RID: 42874 RVA: 0x0010CAE2 File Offset: 0x0010ACE2
	private void ToggleHighlight(bool on)
	{
		this.highlightObject.SetActive(on);
	}

	// Token: 0x040083AD RID: 33709
	[SerializeField]
	private Image image;

	// Token: 0x040083AE RID: 33710
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x040083AF RID: 33711
	[SerializeField]
	private GameObject highlightObject;
}
