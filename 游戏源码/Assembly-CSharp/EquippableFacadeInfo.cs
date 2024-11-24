using System;
using Database;

// Token: 0x02000972 RID: 2418
public class EquippableFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06002BC1 RID: 11201 RVA: 0x000BC691 File Offset: 0x000BA891
	// (set) Token: 0x06002BC2 RID: 11202 RVA: 0x000BC699 File Offset: 0x000BA899
	public string id { get; set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06002BC3 RID: 11203 RVA: 0x000BC6A2 File Offset: 0x000BA8A2
	// (set) Token: 0x06002BC4 RID: 11204 RVA: 0x000BC6AA File Offset: 0x000BA8AA
	public string name { get; set; }

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06002BC5 RID: 11205 RVA: 0x000BC6B3 File Offset: 0x000BA8B3
	// (set) Token: 0x06002BC6 RID: 11206 RVA: 0x000BC6BB File Offset: 0x000BA8BB
	public string desc { get; set; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06002BC7 RID: 11207 RVA: 0x000BC6C4 File Offset: 0x000BA8C4
	// (set) Token: 0x06002BC8 RID: 11208 RVA: 0x000BC6CC File Offset: 0x000BA8CC
	public PermitRarity rarity { get; set; }

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x000BC6D5 File Offset: 0x000BA8D5
	// (set) Token: 0x06002BCA RID: 11210 RVA: 0x000BC6DD File Offset: 0x000BA8DD
	public string animFile { get; set; }

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x06002BCB RID: 11211 RVA: 0x000BC6E6 File Offset: 0x000BA8E6
	// (set) Token: 0x06002BCC RID: 11212 RVA: 0x000BC6EE File Offset: 0x000BA8EE
	public string[] dlcIds { get; set; }

	// Token: 0x06002BCD RID: 11213 RVA: 0x001E0108 File Offset: 0x001DE308
	public EquippableFacadeInfo(string id, string name, string desc, PermitRarity rarity, string defID, string buildOverride, string animFile)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.defID = defID;
		this.buildOverride = buildOverride;
		this.animFile = animFile;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001D68 RID: 7528
	public string buildOverride;

	// Token: 0x04001D69 RID: 7529
	public string defID;
}
