using System;

// Token: 0x02001F39 RID: 7993
[Serializable]
public struct ButtonMenuTextOverride
{
	// Token: 0x17000AC4 RID: 2756
	// (get) Token: 0x0600A8B9 RID: 43193 RVA: 0x0010D8FB File Offset: 0x0010BAFB
	public bool IsValid
	{
		get
		{
			return !string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.ToolTip);
		}
	}

	// Token: 0x17000AC5 RID: 2757
	// (get) Token: 0x0600A8BA RID: 43194 RVA: 0x0010D924 File Offset: 0x0010BB24
	public bool HasCancelText
	{
		get
		{
			return !string.IsNullOrEmpty(this.CancelText) && !string.IsNullOrEmpty(this.CancelToolTip);
		}
	}

	// Token: 0x040084A4 RID: 33956
	public LocString Text;

	// Token: 0x040084A5 RID: 33957
	public LocString CancelText;

	// Token: 0x040084A6 RID: 33958
	public LocString ToolTip;

	// Token: 0x040084A7 RID: 33959
	public LocString CancelToolTip;
}
