using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001C28 RID: 7208
public class SubEntry
{
	// Token: 0x060095F9 RID: 38393 RVA: 0x00101AE4 File Offset: 0x000FFCE4
	public SubEntry()
	{
	}

	// Token: 0x060095FA RID: 38394 RVA: 0x003A0030 File Offset: 0x0039E230
	public SubEntry(string id, string parentEntryID, List<ContentContainer> contentContainers, string name)
	{
		this.id = id;
		this.parentEntryID = parentEntryID;
		this.name = name;
		this.contentContainers = contentContainers;
		if (!string.IsNullOrEmpty(this.lockID))
		{
			foreach (ContentContainer contentContainer in contentContainers)
			{
				contentContainer.lockID = this.lockID;
			}
		}
		if (string.IsNullOrEmpty(this.sortString))
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				this.sortString = UI.StripLinkFormatting(this.title);
				return;
			}
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x170009BC RID: 2492
	// (get) Token: 0x060095FB RID: 38395 RVA: 0x00101AF7 File Offset: 0x000FFCF7
	// (set) Token: 0x060095FC RID: 38396 RVA: 0x00101AFF File Offset: 0x000FFCFF
	public List<ContentContainer> contentContainers { get; set; }

	// Token: 0x170009BD RID: 2493
	// (get) Token: 0x060095FD RID: 38397 RVA: 0x00101B08 File Offset: 0x000FFD08
	// (set) Token: 0x060095FE RID: 38398 RVA: 0x00101B10 File Offset: 0x000FFD10
	public string parentEntryID { get; set; }

	// Token: 0x170009BE RID: 2494
	// (get) Token: 0x060095FF RID: 38399 RVA: 0x00101B19 File Offset: 0x000FFD19
	// (set) Token: 0x06009600 RID: 38400 RVA: 0x00101B21 File Offset: 0x000FFD21
	public string id { get; set; }

	// Token: 0x170009BF RID: 2495
	// (get) Token: 0x06009601 RID: 38401 RVA: 0x00101B2A File Offset: 0x000FFD2A
	// (set) Token: 0x06009602 RID: 38402 RVA: 0x00101B32 File Offset: 0x000FFD32
	public string name { get; set; }

	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06009603 RID: 38403 RVA: 0x00101B3B File Offset: 0x000FFD3B
	// (set) Token: 0x06009604 RID: 38404 RVA: 0x00101B43 File Offset: 0x000FFD43
	public string title { get; set; }

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06009605 RID: 38405 RVA: 0x00101B4C File Offset: 0x000FFD4C
	// (set) Token: 0x06009606 RID: 38406 RVA: 0x00101B54 File Offset: 0x000FFD54
	public string subtitle { get; set; }

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x06009607 RID: 38407 RVA: 0x00101B5D File Offset: 0x000FFD5D
	// (set) Token: 0x06009608 RID: 38408 RVA: 0x00101B65 File Offset: 0x000FFD65
	public Sprite icon { get; set; }

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x06009609 RID: 38409 RVA: 0x00101B6E File Offset: 0x000FFD6E
	// (set) Token: 0x0600960A RID: 38410 RVA: 0x00101B76 File Offset: 0x000FFD76
	public int layoutPriority { get; set; }

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x0600960B RID: 38411 RVA: 0x00101B7F File Offset: 0x000FFD7F
	// (set) Token: 0x0600960C RID: 38412 RVA: 0x00101B87 File Offset: 0x000FFD87
	public bool disabled { get; set; }

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x0600960D RID: 38413 RVA: 0x00101B90 File Offset: 0x000FFD90
	// (set) Token: 0x0600960E RID: 38414 RVA: 0x00101B98 File Offset: 0x000FFD98
	public string lockID { get; set; }

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x0600960F RID: 38415 RVA: 0x00101BA1 File Offset: 0x000FFDA1
	// (set) Token: 0x06009610 RID: 38416 RVA: 0x00101BA9 File Offset: 0x000FFDA9
	public string[] dlcIds { get; set; }

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x06009611 RID: 38417 RVA: 0x00101BB2 File Offset: 0x000FFDB2
	// (set) Token: 0x06009612 RID: 38418 RVA: 0x00101BBA File Offset: 0x000FFDBA
	public string[] forbiddenDLCIds { get; set; }

	// Token: 0x06009613 RID: 38419 RVA: 0x00101BC3 File Offset: 0x000FFDC3
	public string[] GetDlcIds()
	{
		return this.dlcIds;
	}

	// Token: 0x06009614 RID: 38420 RVA: 0x00101BCB File Offset: 0x000FFDCB
	public string[] GetForbiddenDlCIds()
	{
		return this.forbiddenDLCIds;
	}

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x06009615 RID: 38421 RVA: 0x00101BD3 File Offset: 0x000FFDD3
	// (set) Token: 0x06009616 RID: 38422 RVA: 0x00101BDB File Offset: 0x000FFDDB
	public string sortString { get; set; }

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x06009617 RID: 38423 RVA: 0x00101BE4 File Offset: 0x000FFDE4
	// (set) Token: 0x06009618 RID: 38424 RVA: 0x00101BEC File Offset: 0x000FFDEC
	public bool showBeforeGeneratedCategoryLinks { get; set; }

	// Token: 0x04007481 RID: 29825
	public ContentContainer lockedContentContainer;

	// Token: 0x04007488 RID: 29832
	public Color iconColor = Color.white;
}
