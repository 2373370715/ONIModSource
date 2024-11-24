using System;
using UnityEngine;

// Token: 0x02001E40 RID: 7744
public class MotdData_Box
{
	// Token: 0x0600A23C RID: 41532 RVA: 0x003DC604 File Offset: 0x003DA804
	public bool ShouldDisplay()
	{
		long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		return num >= this.startTime && this.finishTime >= num;
	}

	// Token: 0x04007E93 RID: 32403
	public string category;

	// Token: 0x04007E94 RID: 32404
	public string guid;

	// Token: 0x04007E95 RID: 32405
	public long startTime;

	// Token: 0x04007E96 RID: 32406
	public long finishTime;

	// Token: 0x04007E97 RID: 32407
	public string title;

	// Token: 0x04007E98 RID: 32408
	public string text;

	// Token: 0x04007E99 RID: 32409
	public string image;

	// Token: 0x04007E9A RID: 32410
	public string href;

	// Token: 0x04007E9B RID: 32411
	public Texture2D resolvedImage;

	// Token: 0x04007E9C RID: 32412
	public bool resolvedImageIsFromDisk;
}
