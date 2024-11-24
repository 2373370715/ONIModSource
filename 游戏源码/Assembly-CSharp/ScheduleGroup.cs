using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02000B90 RID: 2960
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06003899 RID: 14489 RVA: 0x000C49F4 File Offset: 0x000C2BF4
	// (set) Token: 0x0600389A RID: 14490 RVA: 0x000C49FC File Offset: 0x000C2BFC
	public int defaultSegments { get; private set; }

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x0600389B RID: 14491 RVA: 0x000C4A05 File Offset: 0x000C2C05
	// (set) Token: 0x0600389C RID: 14492 RVA: 0x000C4A0D File Offset: 0x000C2C0D
	public string description { get; private set; }

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x0600389D RID: 14493 RVA: 0x000C4A16 File Offset: 0x000C2C16
	// (set) Token: 0x0600389E RID: 14494 RVA: 0x000C4A1E File Offset: 0x000C2C1E
	public string notificationTooltip { get; private set; }

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x0600389F RID: 14495 RVA: 0x000C4A27 File Offset: 0x000C2C27
	// (set) Token: 0x060038A0 RID: 14496 RVA: 0x000C4A2F File Offset: 0x000C2C2F
	public List<ScheduleBlockType> allowedTypes { get; private set; }

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060038A1 RID: 14497 RVA: 0x000C4A38 File Offset: 0x000C2C38
	// (set) Token: 0x060038A2 RID: 14498 RVA: 0x000C4A40 File Offset: 0x000C2C40
	public bool alarm { get; private set; }

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060038A3 RID: 14499 RVA: 0x000C4A49 File Offset: 0x000C2C49
	// (set) Token: 0x060038A4 RID: 14500 RVA: 0x000C4A51 File Offset: 0x000C2C51
	public Color uiColor { get; private set; }

	// Token: 0x060038A5 RID: 14501 RVA: 0x000C4A5A File Offset: 0x000C2C5A
	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
		this.uiColor = uiColor;
	}

	// Token: 0x060038A6 RID: 14502 RVA: 0x000C4A95 File Offset: 0x000C2C95
	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	// Token: 0x060038A7 RID: 14503 RVA: 0x000C4AA3 File Offset: 0x000C2CA3
	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
