using System;
using Database;

// Token: 0x02000973 RID: 2419
public class MonumentPartInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x1700018B RID: 395
	// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000BC6F7 File Offset: 0x000BA8F7
	// (set) Token: 0x06002BCF RID: 11215 RVA: 0x000BC6FF File Offset: 0x000BA8FF
	public string id { get; set; }

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000BC708 File Offset: 0x000BA908
	// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x000BC710 File Offset: 0x000BA910
	public string name { get; set; }

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000BC719 File Offset: 0x000BA919
	// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x000BC721 File Offset: 0x000BA921
	public string desc { get; set; }

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000BC72A File Offset: 0x000BA92A
	// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x000BC732 File Offset: 0x000BA932
	public PermitRarity rarity { get; set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000BC73B File Offset: 0x000BA93B
	// (set) Token: 0x06002BD7 RID: 11223 RVA: 0x000BC743 File Offset: 0x000BA943
	public string animFile { get; set; }

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06002BD8 RID: 11224 RVA: 0x000BC74C File Offset: 0x000BA94C
	// (set) Token: 0x06002BD9 RID: 11225 RVA: 0x000BC754 File Offset: 0x000BA954
	public string[] dlcIds { get; set; }

	// Token: 0x06002BDA RID: 11226 RVA: 0x001E015C File Offset: 0x001DE35C
	public MonumentPartInfo(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, MonumentPartResource.Part part, string[] dlcIds)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.animFile = animFilename;
		this.state = state;
		this.symbolName = symbolName;
		this.part = part;
		this.dlcIds = dlcIds;
	}

	// Token: 0x04001D71 RID: 7537
	public string state;

	// Token: 0x04001D72 RID: 7538
	public string symbolName;

	// Token: 0x04001D73 RID: 7539
	public MonumentPartResource.Part part;
}
