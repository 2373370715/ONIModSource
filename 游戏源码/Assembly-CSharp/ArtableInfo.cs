using System;
using Database;

// Token: 0x0200096D RID: 2413
public class ArtableInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000BC413 File Offset: 0x000BA613
	// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000BC41B File Offset: 0x000BA61B
	public string id { get; set; }

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000BC424 File Offset: 0x000BA624
	// (set) Token: 0x06002B83 RID: 11139 RVA: 0x000BC42C File Offset: 0x000BA62C
	public string name { get; set; }

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000BC435 File Offset: 0x000BA635
	// (set) Token: 0x06002B85 RID: 11141 RVA: 0x000BC43D File Offset: 0x000BA63D
	public string desc { get; set; }

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000BC446 File Offset: 0x000BA646
	// (set) Token: 0x06002B87 RID: 11143 RVA: 0x000BC44E File Offset: 0x000BA64E
	public PermitRarity rarity { get; set; }

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06002B88 RID: 11144 RVA: 0x000BC457 File Offset: 0x000BA657
	// (set) Token: 0x06002B89 RID: 11145 RVA: 0x000BC45F File Offset: 0x000BA65F
	public string animFile { get; set; }

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000BC468 File Offset: 0x000BA668
	// (set) Token: 0x06002B8B RID: 11147 RVA: 0x000BC470 File Offset: 0x000BA670
	public string[] dlcIds { get; set; }

	// Token: 0x06002B8C RID: 11148 RVA: 0x001DFFBC File Offset: 0x001DE1BC
	public ArtableInfo(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, string status_id, string prefabId, string symbolname = "")
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFile;
		this.anim = anim;
		this.decor_value = decor_value;
		this.cheer_on_complete = cheer_on_complete;
		this.status_id = status_id;
		this.prefabId = prefabId;
		this.symbolname = symbolname;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001D3F RID: 7487
	public string anim;

	// Token: 0x04001D40 RID: 7488
	public int decor_value;

	// Token: 0x04001D41 RID: 7489
	public bool cheer_on_complete;

	// Token: 0x04001D42 RID: 7490
	public string status_id;

	// Token: 0x04001D43 RID: 7491
	public string prefabId;

	// Token: 0x04001D44 RID: 7492
	public string symbolname;
}
