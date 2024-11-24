using System;
using Database;

// Token: 0x02000970 RID: 2416
public class BalloonArtistFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06002BA7 RID: 11175 RVA: 0x000BC545 File Offset: 0x000BA745
	// (set) Token: 0x06002BA8 RID: 11176 RVA: 0x000BC54D File Offset: 0x000BA74D
	public string id { get; set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06002BA9 RID: 11177 RVA: 0x000BC556 File Offset: 0x000BA756
	// (set) Token: 0x06002BAA RID: 11178 RVA: 0x000BC55E File Offset: 0x000BA75E
	public string name { get; set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000BC567 File Offset: 0x000BA767
	// (set) Token: 0x06002BAC RID: 11180 RVA: 0x000BC56F File Offset: 0x000BA76F
	public string desc { get; set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06002BAD RID: 11181 RVA: 0x000BC578 File Offset: 0x000BA778
	// (set) Token: 0x06002BAE RID: 11182 RVA: 0x000BC580 File Offset: 0x000BA780
	public PermitRarity rarity { get; set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06002BAF RID: 11183 RVA: 0x000BC589 File Offset: 0x000BA789
	// (set) Token: 0x06002BB0 RID: 11184 RVA: 0x000BC591 File Offset: 0x000BA791
	public string animFile { get; set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x000BC59A File Offset: 0x000BA79A
	// (set) Token: 0x06002BB2 RID: 11186 RVA: 0x000BC5A2 File Offset: 0x000BA7A2
	public string[] dlcIds { get; set; }

	// Token: 0x06002BB3 RID: 11187 RVA: 0x000BC5AB File Offset: 0x000BA7AB
	public BalloonArtistFacadeInfo(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.balloonFacadeType = balloonFacadeType;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001D5B RID: 7515
	public BalloonArtistFacadeType balloonFacadeType;
}
