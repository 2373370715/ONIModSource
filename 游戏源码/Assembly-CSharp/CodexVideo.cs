using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C66 RID: 7270
public class CodexVideo : CodexWidget<CodexVideo>
{
	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x0600977C RID: 38780 RVA: 0x00102725 File Offset: 0x00100925
	// (set) Token: 0x0600977D RID: 38781 RVA: 0x0010272D File Offset: 0x0010092D
	public string name { get; set; }

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x0600977F RID: 38783 RVA: 0x0010273F File Offset: 0x0010093F
	// (set) Token: 0x0600977E RID: 38782 RVA: 0x00102736 File Offset: 0x00100936
	public string videoName
	{
		get
		{
			return "--> " + (this.name ?? "NULL");
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06009780 RID: 38784 RVA: 0x0010275A File Offset: 0x0010095A
	// (set) Token: 0x06009781 RID: 38785 RVA: 0x00102762 File Offset: 0x00100962
	public string overlayName { get; set; }

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06009782 RID: 38786 RVA: 0x0010276B File Offset: 0x0010096B
	// (set) Token: 0x06009783 RID: 38787 RVA: 0x00102773 File Offset: 0x00100973
	public List<string> overlayTexts { get; set; }

	// Token: 0x06009784 RID: 38788 RVA: 0x0010277C File Offset: 0x0010097C
	public void ConfigureVideo(VideoWidget videoWidget, string clipName, string overlayName = null, List<string> overlayTexts = null)
	{
		videoWidget.SetClip(Assets.GetVideo(clipName), overlayName, overlayTexts);
	}

	// Token: 0x06009785 RID: 38789 RVA: 0x0010278D File Offset: 0x0010098D
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureVideo(contentGameObject.GetComponent<VideoWidget>(), this.name, this.overlayName, this.overlayTexts);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
