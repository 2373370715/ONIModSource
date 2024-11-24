using System;
using UnityEngine;

// Token: 0x0200095C RID: 2396
[Serializable]
public class AudioSheet
{
	// Token: 0x04001CEF RID: 7407
	public TextAsset asset;

	// Token: 0x04001CF0 RID: 7408
	public string defaultType;

	// Token: 0x04001CF1 RID: 7409
	public AudioSheet.SoundInfo[] soundInfos;

	// Token: 0x0200095D RID: 2397
	public class SoundInfo : Resource
	{
		// Token: 0x04001CF2 RID: 7410
		public string File;

		// Token: 0x04001CF3 RID: 7411
		public string Anim;

		// Token: 0x04001CF4 RID: 7412
		public string Type;

		// Token: 0x04001CF5 RID: 7413
		public string RequiredDlcId;

		// Token: 0x04001CF6 RID: 7414
		public float MinInterval;

		// Token: 0x04001CF7 RID: 7415
		public string Name0;

		// Token: 0x04001CF8 RID: 7416
		public int Frame0;

		// Token: 0x04001CF9 RID: 7417
		public string Name1;

		// Token: 0x04001CFA RID: 7418
		public int Frame1;

		// Token: 0x04001CFB RID: 7419
		public string Name2;

		// Token: 0x04001CFC RID: 7420
		public int Frame2;

		// Token: 0x04001CFD RID: 7421
		public string Name3;

		// Token: 0x04001CFE RID: 7422
		public int Frame3;

		// Token: 0x04001CFF RID: 7423
		public string Name4;

		// Token: 0x04001D00 RID: 7424
		public int Frame4;

		// Token: 0x04001D01 RID: 7425
		public string Name5;

		// Token: 0x04001D02 RID: 7426
		public int Frame5;

		// Token: 0x04001D03 RID: 7427
		public string Name6;

		// Token: 0x04001D04 RID: 7428
		public int Frame6;

		// Token: 0x04001D05 RID: 7429
		public string Name7;

		// Token: 0x04001D06 RID: 7430
		public int Frame7;

		// Token: 0x04001D07 RID: 7431
		public string Name8;

		// Token: 0x04001D08 RID: 7432
		public int Frame8;

		// Token: 0x04001D09 RID: 7433
		public string Name9;

		// Token: 0x04001D0A RID: 7434
		public int Frame9;

		// Token: 0x04001D0B RID: 7435
		public string Name10;

		// Token: 0x04001D0C RID: 7436
		public int Frame10;

		// Token: 0x04001D0D RID: 7437
		public string Name11;

		// Token: 0x04001D0E RID: 7438
		public int Frame11;
	}
}
