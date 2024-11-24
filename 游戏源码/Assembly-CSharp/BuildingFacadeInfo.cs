using System;
using System.Collections.Generic;
using Database;

// Token: 0x0200096F RID: 2415
public class BuildingFacadeInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000BC4DF File Offset: 0x000BA6DF
	// (set) Token: 0x06002B9B RID: 11163 RVA: 0x000BC4E7 File Offset: 0x000BA6E7
	public string id { get; set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000BC4F0 File Offset: 0x000BA6F0
	// (set) Token: 0x06002B9D RID: 11165 RVA: 0x000BC4F8 File Offset: 0x000BA6F8
	public string name { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000BC501 File Offset: 0x000BA701
	// (set) Token: 0x06002B9F RID: 11167 RVA: 0x000BC509 File Offset: 0x000BA709
	public string desc { get; set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06002BA0 RID: 11168 RVA: 0x000BC512 File Offset: 0x000BA712
	// (set) Token: 0x06002BA1 RID: 11169 RVA: 0x000BC51A File Offset: 0x000BA71A
	public PermitRarity rarity { get; set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x000BC523 File Offset: 0x000BA723
	// (set) Token: 0x06002BA3 RID: 11171 RVA: 0x000BC52B File Offset: 0x000BA72B
	public string animFile { get; set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06002BA4 RID: 11172 RVA: 0x000BC534 File Offset: 0x000BA734
	// (set) Token: 0x06002BA5 RID: 11173 RVA: 0x000BC53C File Offset: 0x000BA73C
	public string[] dlcIds { get; set; }

	// Token: 0x06002BA6 RID: 11174 RVA: 0x001E00B8 File Offset: 0x001DE2B8
	public BuildingFacadeInfo(string id, string name, string desc, PermitRarity rarity, string prefabId, string animFile, string[] dlcIds, Dictionary<string, string> workables = null)
	{
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.rarity = rarity;
		this.prefabId = prefabId;
		this.animFile = animFile;
		this.workables = workables;
		this.dlcIds = dlcIds;
	}

	// Token: 0x04001D52 RID: 7506
	public string prefabId;

	// Token: 0x04001D54 RID: 7508
	public Dictionary<string, string> workables;
}
