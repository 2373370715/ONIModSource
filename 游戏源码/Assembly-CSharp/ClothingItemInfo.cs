using System;
using Database;

// Token: 0x0200096E RID: 2414
public class ClothingItemInfo : IBlueprintInfo, IBlueprintDlcInfo
{
	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000BC479 File Offset: 0x000BA679
	// (set) Token: 0x06002B8E RID: 11150 RVA: 0x000BC481 File Offset: 0x000BA681
	public string id { get; set; }

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000BC48A File Offset: 0x000BA68A
	// (set) Token: 0x06002B90 RID: 11152 RVA: 0x000BC492 File Offset: 0x000BA692
	public string name { get; set; }

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000BC49B File Offset: 0x000BA69B
	// (set) Token: 0x06002B92 RID: 11154 RVA: 0x000BC4A3 File Offset: 0x000BA6A3
	public string desc { get; set; }

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000BC4AC File Offset: 0x000BA6AC
	// (set) Token: 0x06002B94 RID: 11156 RVA: 0x000BC4B4 File Offset: 0x000BA6B4
	public PermitRarity rarity { get; set; }

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000BC4BD File Offset: 0x000BA6BD
	// (set) Token: 0x06002B96 RID: 11158 RVA: 0x000BC4C5 File Offset: 0x000BA6C5
	public string animFile { get; set; }

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06002B97 RID: 11159 RVA: 0x000BC4CE File Offset: 0x000BA6CE
	// (set) Token: 0x06002B98 RID: 11160 RVA: 0x000BC4D6 File Offset: 0x000BA6D6
	public string[] dlcIds { get; set; }

	// Token: 0x06002B99 RID: 11161 RVA: 0x001E0030 File Offset: 0x001DE230
	public ClothingItemInfo(string id, string name, string desc, PermitCategory category, PermitRarity rarity, string animFile)
	{
		Option<ClothingOutfitUtility.OutfitType> outfitTypeFor = PermitCategories.GetOutfitTypeFor(category);
		if (outfitTypeFor.IsNone())
		{
			throw new Exception(string.Format("Expected permit category {0} on ClothingItemResource \"{1}\" to have an {2} but none found.", category, id, "OutfitType"));
		}
		this.id = id;
		this.name = name;
		this.desc = desc;
		this.outfitType = outfitTypeFor.Unwrap();
		this.category = category;
		this.rarity = rarity;
		this.animFile = animFile;
		this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x04001D49 RID: 7497
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x04001D4A RID: 7498
	public PermitCategory category;
}
