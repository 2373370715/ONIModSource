using System;

// Token: 0x020015DC RID: 5596
[Serializable]
public struct SicknessExposureInfo
{
	// Token: 0x060073F6 RID: 29686 RVA: 0x000EC1A0 File Offset: 0x000EA3A0
	public SicknessExposureInfo(string id, string infection_source_info)
	{
		this.sicknessID = id;
		this.sourceInfo = infection_source_info;
	}

	// Token: 0x040056BD RID: 22205
	public string sicknessID;

	// Token: 0x040056BE RID: 22206
	public string sourceInfo;
}
